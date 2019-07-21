using System;
using System.Threading;
using AutoCSer.Extension;
using System.IO;
using AutoCSer.Log;
using System.Diagnostics;
using AutoCSer.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// HTTP 注册管理服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.HttpServerRegister)]
    public partial class Server : TcpInternalServer.TimeVerifyServer, IDisposable
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        internal const string ServerName = "HttpServerRegister";
        /// <summary>
        /// 最大域名字节长度
        /// </summary>
        internal const int MaxDomainSize = 256 + 8;
        /// <summary>
        /// HTTP 服务相关参数
        /// </summary>
        internal static readonly Config Config = ConfigLoader.GetUnion(typeof(Config)).Config ?? new Config();
        /// <summary>
        /// 程序集信息缓存
        /// </summary>
        private static readonly Dictionary<HashString, Assembly> assemblyCache = DictionaryCreator.CreateHashString<Assembly>();
        /// <summary>
        /// 程序集信息访问锁
        /// </summary>
        private static readonly object assemblyLock = new object();
        /// <summary>
        /// 文件监视器
        /// </summary>
        internal AutoCSer.IO.CreateFlieTimeoutWatcher FileWatcher;
        /// <summary>
        /// TCP域名服务缓存文件名
        /// </summary>
        private string cacheFileName;
        /// <summary>
        /// 域名搜索
        /// </summary>
        private DomainSearchData domains = DomainSearchData.Default;
        /// <summary>
        /// HTTP域名服务集合访问锁
        /// </summary>
        private readonly object domainLock = new object();
        /// <summary>
        /// TCP服务端口信息集合
        /// </summary>
        private Dictionary<HostPort, Http.Server> hosts = DictionaryCreator.CreateHostPort<Http.Server>();
        /// <summary>
        /// TCP服务端口信息集合访问锁
        /// </summary>
        private readonly object hostLock = new object();
        /// <summary>
        /// 本地服务
        /// </summary>
        internal HttpDomainServer.Server LocalDomainServer;
        /// <summary>
        /// 缓存域名服务加载事件
        /// </summary>
        public event Action OnLoadCacheDomain;
        /// <summary>
        /// 停止监听事件
        /// </summary>
        public event Action OnStopListen;
        /// <summary>
        /// 文件监视是否超时
        /// </summary>
        private int isFileWatcherTimeout;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 是否正在加载缓存
        /// </summary>
        private bool isLoadCache;
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public override void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            base.SetTcpServer(tcpServer);
            cacheFileName = AutoCSer.Config.Pub.Default.CachePath + ServerName + (ServerName == tcpServer.Attribute.ServerName ? null : ("_" + tcpServer.Attribute.ServerName)) + ".cache";
            FileWatcher = new AutoCSer.IO.CreateFlieTimeoutWatcher(ProcessCopyer.Config.CheckTimeoutSeconds, this, AutoCSer.IO.CreateFlieTimeoutType.HttpServerRegister, tcpServer.Log);
            if (!AutoCSer.Config.Pub.Default.IsService && ProcessCopyer.Config.WatcherPath != null)
            {
                try
                {
                    FileWatcher.Add(ProcessCopyer.Config.WatcherPath);
                }
                catch (Exception error)
                {
                    tcpServer.Log.Add(AutoCSer.Log.LogType.Error, error, ProcessCopyer.Config.WatcherPath);
                }
            }
            try
            {
                if (System.IO.File.Exists(cacheFileName))
                {
                    Cache[] cache = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Cache[]>(System.IO.File.ReadAllBytes(cacheFileName));
                    isLoadCache = true;
                    if (OnLoadCacheDomain != null) OnLoadCacheDomain();
                    for (int index = cache.Length; index != 0; )
                    {
                        try
                        {
                            start(ref cache[--index]);
                        }
                        catch (Exception error)
                        {
                            tcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                tcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            if (isLoadCache) isLoadCache = false;
            else if (OnLoadCacheDomain != null) OnLoadCacheDomain();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                server.Dispose();
                if (LocalDomainServer == null)
                {
                    FileWatcher.Dispose();

                    Monitor.Enter(domainLock);
                    DomainSearchData domains = this.domains;
                    this.domains = DomainSearchData.Default;
                    Monitor.Exit(domainLock);
                    domains.Close();
                    //domains.Dispose();

                    Monitor.Enter(hostLock);
                    try
                    {
                        if (hosts.Count != 0)
                        {
                            foreach (Http.Server httpServer in hosts.Values) httpServer.Dispose();
                            hosts.Clear();
                        }
                    }
                    finally { Monitor.Exit(hostLock); }
                }
                else LocalDomainServer.Dispose();
            }
        }
        /// <summary>
        /// 文件监视超时处理
        /// </summary>
        internal void OnFileWatcherTimeout()
        {
            if (Interlocked.CompareExchange(ref isFileWatcherTimeout, 1, 0) == 0)
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    FileInfo file = new FileInfo(process.MainModule.FileName);
                    if (ProcessCopyer.Config.WatcherPath == null)
                    {
                        ProcessStartInfo info = new ProcessStartInfo(file.FullName, null);
                        info.UseShellExecute = true;
                        info.WorkingDirectory = file.DirectoryName;
                        Dispose();
                        using (Process newProcess = Process.Start(info)) Environment.Exit(-1);
                    }
                    else
                    {
                        ProcessCopyClient.Remove();
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(FileWatcherTimeout);
                    }
                }
            }
        }
        /// <summary>
        /// 文件监视超时处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FileWatcherTimeout()
        {
            Dispose();
            while (!ProcessCopyClient.Copy()) Thread.Sleep(AutoCSer.Diagnostics.ProcessCopyer.Config.CheckTimeoutSeconds * 1000);
            Environment.Exit(-1);
        }
        /// <summary>
        /// 启动域名服务
        /// </summary>
        /// <param name="assemblyFile">程序集文件名,包含路径</param>
        /// <param name="serverTypeName">服务程序类型名称</param>
        /// <param name="domain">域名信息</param>
        /// <param name="isShareAssembly">是否共享程序集</param>
        /// <returns>域名服务启动状态</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private RegisterState start(string assemblyFile, string serverTypeName, Domain domain, bool isShareAssembly)
        {
            Cache register = new Cache { AssemblyFile = assemblyFile, ServerTypeName = serverTypeName, Domains = new Domain[] { domain }, IsShareAssembly = isShareAssembly };
            return start(ref register);
        }
        /// <summary>
        /// 启动域名服务
        /// </summary>
        /// <param name="assemblyFile">程序集文件名,包含路径</param>
        /// <param name="serverTypeName">服务程序类型名称</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="isShareAssembly">是否共享程序集</param>
        /// <returns>域名服务启动状态</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe RegisterState start(string assemblyFile, string serverTypeName, Domain[] domains, bool isShareAssembly)
        {
            Cache register = new Cache { AssemblyFile = assemblyFile, ServerTypeName = serverTypeName, Domains = domains, IsShareAssembly = isShareAssembly };
            return start(ref register);
        }
        /// <summary>
        /// 启动域名服务
        /// </summary>
        /// <param name="register">域名注册信息</param>
        /// <returns>域名服务启动状态</returns>
        private RegisterState start(ref Cache register)
        {
            if (isDisposed != 0) return RegisterState.Disposed;
            if (register.Domains.length() == 0) return RegisterState.DomainError;
            FileInfo assemblyFile = new FileInfo(register.AssemblyFile);
            if (!System.IO.File.Exists(register.AssemblyFile))
            {
                server.Log.Add(AutoCSer.Log.LogType.Error, "未找到程序集 " + register.AssemblyFile);
                return RegisterState.NotFoundAssembly;
            }
            RegisterState state = RegisterState.Unknown;
            DomainServer domainServer = new DomainServer { Register = register, RegisterServer = this };
            int domainCount = domainServer.CheckDomain(ref state);
            if (state == RegisterState.Success)
            {
                try
                {
                    state = RegisterState.StartError;
                    Assembly assembly = null;
                    DirectoryInfo directory = assemblyFile.Directory;
                    KeyValue<Domain, int>[] domainFlags = register.Domains.getArray(value => new KeyValue<Domain, int>(value, 0));
                    HashString pathKey = register.AssemblyFile;
                    Monitor.Enter(assemblyLock);
                    try
                    {
                        if (!register.IsShareAssembly || !assemblyCache.TryGetValue(pathKey, out assembly))
                        {
                            string serverPath = Config.WorkPath + ((ulong)AutoCSer.Pub.StartTime.Ticks).toHex() + ((ulong)AutoCSer.Pub.Identity).toHex() + AutoCSer.Extension.DirectoryExtension.Separator;
                            Directory.CreateDirectory(serverPath);
                            foreach (FileInfo file in directory.GetFiles()) file.CopyTo(serverPath + file.Name);
                            assembly = Assembly.LoadFrom(serverPath + assemblyFile.Name);
                            if (register.IsShareAssembly) assemblyCache.Add(pathKey, assembly);
                        }
                    }
                    finally { Monitor.Exit(assemblyLock); }
                    domainServer.HttpDomainServer = (HttpDomainServer.Server)Activator.CreateInstance(assembly.GetType(register.ServerTypeName));
                    domainServer.HttpDomainServer.LoadCheckPath = getLoadCheckPath(directory).FullName;
                    if (domainServer.HttpDomainServer.Start(this, register.Domains, domainServer.RemoveFileWatcher))
                    {
                        if (FileWatcher != null) FileWatcher.Add(directory.FullName);
                        domainServer.FileWatcherPath = directory.FullName;
                        if ((state = start(register.Domains)) == RegisterState.Success)
                        {
                            domainServer.DomainCount = register.Domains.Length;
                            domainServer.Domains = domainFlags;
                            domainServer.IsStart = true;
                            if (!isLoadCache)
                            {
                                Monitor.Enter(domainLock);
                                try
                                {
                                    Cache[] registers = domains.GetSaveCache();
                                    if (registers != null) System.IO.File.WriteAllBytes(cacheFileName, AutoCSer.BinarySerialize.Serializer.Serialize(registers));
                                }
                                finally { Monitor.Exit(domainLock); }
                            }
                            server.Log.Add(AutoCSer.Log.LogType.Info, @"domain success
" + register.Domains.joinString(@"
", domain => domain.DomainData.toStringNotNull() + (domain.Host.Host == null ? null : (" [" + domain.Host.Host + ":" + domain.Host.Port.toString() + "]")) + (domain.SslHost.Host == null ? null : (" [" + domain.SslHost.Host + ":" + domain.SslHost.Port.toString() + "]"))), new System.Diagnostics.StackFrame(), false);
                            return RegisterState.Success;
                        }
                    }
                }
                catch (Exception error)
                {
                    server.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
            }
            foreach (Domain domain in register.Domains)
            {
                if (domainCount-- == 0) break;
                Monitor.Enter(domainLock);
                domains.Remove(domain.DomainData);
                Monitor.Exit(domainLock);
            }
            domainServer.Dispose();
            return state;
        }
        /// <summary>
        /// 域名状态检测
        /// </summary>
        /// <param name="domain">域名信息</param>
        /// <param name="domainServer">域名服务</param>
        /// <returns>域名状态</returns>
        internal RegisterState CheckDomain(ref Domain domain, DomainServer domainServer)
        {
            byte[] domainData = domain.DomainData;
            if (domain.Host.Host == null)
            {
                if (domain.SslHost.Host == null)
                {
                    if (domainData.length() == 0) return RegisterState.DomainError;
                    int portIndex = AutoCSer.Memory.indexOfNotNull(domainData, (byte)':');
                    if (portIndex == -1) domain.Host.Set(domainData.toStringNotNull(), 80);
                    else if (portIndex == 0) return RegisterState.DomainError;
                    else
                    {
                        if (!int.TryParse(domainData.toStringNotNull(portIndex + 1, domainData.Length - portIndex - 1), out domain.Host.Port)) return RegisterState.DomainError;
                        domain.Host.Host = domainData.toStringNotNull(0, portIndex);
                    }
                    if (!domain.Host.HostToIPAddress()) return RegisterState.DomainError;
                }
                else
                {
                    if (domain.SslHost.Port == 0) domain.SslHost.Port = 443;
                    if (domainData.length() == 0)
                    {
                        if (domain.SslHost.Host.Length == 0) return RegisterState.HostError;
                        string host = domain.SslHost.Host;
                        if (domain.SslHost.Port != 443) host += ":" + domain.SslHost.Port.toString();
                        domain.DomainData = domainData = host.getBytes();
                        server.Log.Add(AutoCSer.Log.LogType.Error, domain.SslHost.Host + " 缺少指定域名");
                    }
                    else if (domain.SslHost.Port != 443 && AutoCSer.Memory.indexOfNotNull(domainData, (byte)':') == -1)
                    {
                        domain.DomainData = domainData = (domainData.toStringNotNull() + ":" + domain.SslHost.Port.toString()).getBytes();
                    }
                    if (!domain.SslHost.HostToIPAddress()) return RegisterState.HostError;
                }
            }
            else
            {
                if (domain.Host.Port == 0) domain.Host.Port = 80;
                if (domain.SslHost.Host != null && domain.SslHost.Port == 0) domain.SslHost.Port = 443;
                if (domainData.length() == 0)
                {
                    if (domain.Host.Host.Length == 0) return RegisterState.HostError;
                    string host = domain.Host.Host;
                    if (domain.Host.Port != 80) host += ":" + domain.Host.Port.toString();
                    domain.DomainData = domainData = host.getBytes();
                    server.Log.Add(AutoCSer.Log.LogType.Error, domain.Host.Host + " 缺少指定域名");
                }
                else if (domain.SslHost.Host == null)
                {
                    if (domain.Host.Port != 80 && AutoCSer.Memory.indexOfNotNull(domainData, (byte)':') == -1)
                    {
                        domain.DomainData = domainData = (domainData.toStringNotNull() + ":" + domain.Host.Port.toString()).getBytes();
                    }
                }
                else
                {
                    if (!domain.SslHost.HostToIPAddress()) return RegisterState.HostError;
                }
                if (!domain.Host.HostToIPAddress()) return RegisterState.HostError;
            }
            if (domainData.Length > MaxDomainSize) return RegisterState.DomainError;
            domainData.toLowerNotNull();
            DomainSearchData removeDomains = null;
            Monitor.Enter(domainLock);
            try
            {
                domains = domains.Add(domainData, domainServer, out removeDomains);
            }
            finally
            {
                Monitor.Exit(domainLock);
                //if (removeDomains != null) removeDomains.Dispose();
            }
            return removeDomains == null ? RegisterState.DomainExists : RegisterState.Success;
        }
        /// <summary>
        /// 启动TCP服务
        /// </summary>
        /// <param name="domains">域名信息集合</param>
        /// <returns>HTTP服务启动状态</returns>
        private RegisterState start(Domain[] domains)
        {
            int hostCount = 0, startCount = 0, stopCount = 0;
            foreach (Domain domain in domains)
            {
                if (!domain.IsOnlyHost)
                {
                    if (domain.Host.Host != null)
                    {
                        RegisterState state = start(ref domain.Host);
                        if (state != RegisterState.Success) break;
                    }
                    if (domain.SslHost.Host != null)
                    {
                        RegisterState state = startSsl(domain);
                        if (state != RegisterState.Success)
                        {
                            if (domain.Host.Host != null) ++stopCount;
                            break;
                        }
                    }
                    ++startCount;
                }
                ++hostCount;
                ++stopCount;
            }
            if (startCount != 0 && hostCount == domains.Length) return RegisterState.Success;
            foreach (Domain domain in domains)
            {
                if (stopCount-- == 0) break;
                if (!domain.IsOnlyHost)
                {
                    stop(ref domain.Host);
                    stop(ref domain.SslHost);
                }
            }
            return RegisterState.TcpError;
        }
        /// <summary>
        /// 启动TCP服务
        /// </summary>
        /// <param name="host">TCP服务端口信息</param>
        /// <returns>HTTP服务启动状态</returns>
        private RegisterState start(ref HostPort host)
        {
            RegisterState state = RegisterState.TcpError;
            Http.Server httpServer = null;
            Monitor.Enter(hostLock);
            try
            {
                if (hosts.TryGetValue(host, out httpServer))
                {
                    if (!httpServer.IsSSL)
                    {
                        ++httpServer.DomainCount;
                        return RegisterState.Success;
                    }
                    httpServer = null;
                    state = RegisterState.SslMatchError;
                }
                else
                {
                    state = RegisterState.CreateServerError;
                    httpServer = new Http.Server(this, ref host, false);
                    if (httpServer.IsStart)
                    {
                        hosts.Add(host, httpServer);
                        return RegisterState.Success;
                    }
                }
            }
            catch (Exception error)
            {
                server.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally { Monitor.Exit(hostLock); }
            if (httpServer != null) httpServer.Dispose();
            return state;
        }
        /// <summary>
        /// 启动TCP服务
        /// </summary>
        /// <param name="domain">域名信息</param>
        /// <returns>HTTP服务启动状态</returns>
        private RegisterState startSsl(Domain domain)
        {
            RegisterState state = RegisterState.TcpError;
            Http.Server httpServer = null;
            Monitor.Enter(hostLock);
            try
            {
                if (hosts.TryGetValue(domain.SslHost, out httpServer))
                {
                    if (httpServer.IsSSL)
                    {
                        if (new AutoCSer.Net.Http.UnionType { Value = httpServer }.SslServer.SetCertificate(domain))
                        {
                            ++httpServer.DomainCount;
                            return RegisterState.Success;
                        }
                        state = RegisterState.CertificateError;
                    }
                    else state = RegisterState.SslMatchError;
                    httpServer = null;
                }
                else
                {
                    state = RegisterState.CreateServerError;
                    Http.SslServer server = new Http.SslServer(this, domain);
                    httpServer = server;
                    if (server.IsCertificate)
                    {
                        if (httpServer.IsStart)
                        {
                            hosts.Add(domain.SslHost, httpServer);
                            return RegisterState.Success;
                        }
                    }
                    else state = RegisterState.CertificateError;
                }
            }
            catch (Exception error)
            {
                server.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally { Monitor.Exit(hostLock); }
            if (httpServer != null) httpServer.Dispose();
            return state;
        }
        /// <summary>
        /// 停止TCP服务
        /// </summary>
        /// <param name="host">TCP服务端口信息</param>
        private void stop(ref HostPort host)
        {
            Http.Server httpServer;
            Monitor.Enter(hostLock);
            try
            {
                if (hosts.TryGetValue(host, out httpServer))
                {
                    if (--httpServer.DomainCount == 0) hosts.Remove(host);
                    else httpServer = null;
                }
            }
            finally { Monitor.Exit(hostLock); }
            if (httpServer != null) httpServer.Dispose();
        }
        /// <summary>
        /// 获取域名服务信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="size">域名长度</param>
        /// <returns>域名服务信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe HttpDomainServer.Server GetServer(byte* domain, int size)
        {
            if (LocalDomainServer == null)
            {
                if (size > MaxDomainSize || size == 0) return null;
                DomainServer server = domains.Get(domain, size);
                return server != null && server.IsStart ? server.HttpDomainServer : null;
            }
            return LocalDomainServer;
        }
        /// <summary>
        /// 停止域名服务
        /// </summary>
        /// <param name="domains">域名信息集合</param>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void stop(Domain[] domains)
        {
            if (domains != null)
            {
                foreach (Domain domain in domains) stop(domain);
            }
        }
        /// <summary>
        /// 停止域名服务
        /// </summary>
        /// <param name="domain">域名信息</param>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void stop(Domain domain)
        {
            byte[] domainData = domain.DomainData;
            if (domainData != null)
            {
                DomainServer domainServer;
                domainData.toLowerNotNull();
                Monitor.Enter(domainLock);
                try
                {
                    domainServer = domains.Remove(domainData);
                }
                finally
                {
                    Monitor.Exit(domainLock);
                }
                if (domainServer != null && domainServer.Domains != null)
                {
                    for (int index = domainServer.Domains.Length; index != 0; )
                    {
                        KeyValue<Domain, int> stopDomain = domainServer.Domains[--index];
                        if ((stopDomain.Value | (stopDomain.Key.DomainData.Length ^ domainData.Length)) == 0
                            && AutoCSer.Memory.SimpleEqualNotNull(stopDomain.Key.DomainData, domainData, domainData.Length)
                            && Interlocked.CompareExchange(ref domainServer.Domains[index].Value, 1, 0) == 0)
                        {
                            if (!stopDomain.Key.IsOnlyHost)
                            {
                                stop(ref stopDomain.Key.Host);
                                stop(ref stopDomain.Key.SslHost);
                            }
                            if (Interlocked.Decrement(ref domainServer.DomainCount) == 0) domainServer.Dispose();
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 停止所有端口监听
        /// </summary>
        /// <param name="isStop"></param>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void stopListen(bool isStop)
        {
            if (isStop)
            {
                if (FileWatcher != null)
                {
                    FileWatcher.Dispose();
                    FileWatcher = null;
                }
                Monitor.Enter(hostLock);
                try
                {
                    foreach (Http.Server server in hosts.Values) server.StopListen();
                }
                catch (Exception error)
                {
                    server.AddLog(error);
                }
                finally { Monitor.Exit(hostLock); }
                Monitor.Enter(domainLock);
                try
                {
                    domains.StopListen();
                }
                catch (Exception error)
                {
                    server.AddLog(error);
                }
                finally { Monitor.Exit(domainLock); }
                if (OnStopListen != null) OnStopListen();
            }
        }

        /// <summary>
        /// 启动本地服务
        /// </summary>
        /// <typeparam name="domainType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="hosts"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private RegisterState start<domainType>(string workPath, HostPort[] hosts, ILog log) where domainType : HttpDomainServer.Server
        {
            return start<domainType>(workPath, hosts.getArray(host => new Domain { Host = host }), log);
        }
        /// <summary>
        /// 启动本地服务
        /// </summary>
        /// <typeparam name="domainType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="domains"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private RegisterState start<domainType>(string workPath, Domain[] domains, ILog log) where domainType : HttpDomainServer.Server
        {
            RegisterState state = RegisterState.StartError;
#if NoAutoCSer
            throw new Exception();
#else
            TcpInternalServer tcpServer = new TcpInternalServer(null, null, this, null, log);
            base.SetTcpServer(tcpServer);
            Assembly assembly = typeof(domainType).Assembly;
            LocalDomainServer = AutoCSer.Emit.Constructor<domainType>.New();
            LocalDomainServer.LoadCheckPath = getLoadCheckPath(new DirectoryInfo(workPath ?? AutoCSer.PubPath.ApplicationPath)).FullName;
            if (LocalDomainServer.Start(this, domains, null))
            {
                if ((state = start(domains)) == RegisterState.Success)
                {
                    AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Info, @"domain success
" + domains.joinString(@"
", domain => domain.Host.Host + ":" + domain.Host.Port.toString()), new System.Diagnostics.StackFrame(), false);
                    return RegisterState.Success;
                }
            }
#endif
            return state;
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="log"></param>
        /// <param name="domains"></param>
        /// <returns></returns>
        public static Server Create<domainServerType>(string workPath, ILog log, params Domain[] domains) where domainServerType : HttpDomainServer.Server
        {
            Server server = new Server();
            RegisterState state = RegisterState.Unknown;
            try
            {
                if ((state = server.start<domainServerType>(workPath, domains, log)) == RegisterState.Success) return server;
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Debug, "HTTP服务启动失败 " + state.ToString());
            }
            finally
            {
                if (state != RegisterState.Success) server.Dispose();
            }
            return null;
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="domains"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(string workPath, params Domain[] domains) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(workPath, null, domains);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="domains"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(params Domain[] domains) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(null, null, domains);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="log"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        public static Server Create<domainServerType>(string workPath, ILog log, params HostPort[] hosts) where domainServerType : HttpDomainServer.Server
        {
            Server server = new Server();
            RegisterState state = RegisterState.Unknown;
            try
            {
                if ((state = server.start<domainServerType>(workPath, hosts, log)) == RegisterState.Success) return server;
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Debug, "HTTP服务启动失败 " + state.ToString());
            }
            finally
            {
                if (state != RegisterState.Success) server.Dispose();
            }
            return null;
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(string workPath, params HostPort[] hosts) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(workPath, null, hosts);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(params HostPort[] hosts) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(null, null, hosts);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="log"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(string workPath, ILog log, params string[] hosts) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(workPath, log, hosts.getArray(host => new HostPort { Host = host, Port = 80 }));
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(params string[] hosts) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(null, null, hosts.getArray(host => new HostPort { Host = host, Port = 80 }));
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="workPath"></param>
        /// <param name="host"></param>
        /// <param name="log"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(string workPath, string host, ILog log, int port = 80) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(workPath, log, new HostPort { Host = host, Port = port });
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="host"></param>
        /// <param name="log"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(string host, ILog log, int port = 80) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(null, log, new HostPort { Host = host, Port = port });
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <typeparam name="domainServerType"></typeparam>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create<domainServerType>(string host, int port = 80) where domainServerType : HttpDomainServer.Server
        {
            return Create<domainServerType>(null, null, new HostPort { Host = host, Port = port });
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="workPath"></param>
        /// <param name="log"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        public static Server Create(string workPath, ILog log, params HostPort[] hosts)
        {
            return Create<HttpDomainServer.FileServer>(workPath, log, hosts);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="workPath"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(string workPath, params HostPort[] hosts)
        {
            return Create<HttpDomainServer.FileServer>(workPath, null, hosts);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(params HostPort[] hosts)
        {
            return Create<HttpDomainServer.FileServer>(null, null, hosts);
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="workPath"></param>
        /// <param name="log"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(string workPath, ILog log, params string[] hosts)
        {
            return Create<HttpDomainServer.FileServer>(workPath, log, hosts.getArray(host => new HostPort { Host = host, Port = 80 }));
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="hosts"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(params string[] hosts)
        {
            return Create<HttpDomainServer.FileServer>(null, null, hosts.getArray(host => new HostPort { Host = host, Port = 80 }));
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="workPath"></param>
        /// <param name="host"></param>
        /// <param name="log"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(string workPath, string host, ILog log, int port = 80)
        {
            return Create<HttpDomainServer.FileServer>(workPath, log, new HostPort { Host = host, Port = port });
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="host"></param>
        /// <param name="log"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(string host, ILog log, int port = 80)
        {
            return Create<HttpDomainServer.FileServer>(null, log, new HostPort { Host = host, Port = port });
        }
        /// <summary>
        /// 本地模式
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Server Create(string host, int port = 80)
        {
            return Create<HttpDomainServer.FileServer>(null, null, new HostPort { Host = host, Port = port });
        }
        /// <summary>
        /// 获取域名服务加载检测路径
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static DirectoryInfo getLoadCheckPath(DirectoryInfo directory)
        {
            DirectoryInfo loadDirectory = directory;
            do
            {
                string loadPath = loadDirectory.Name;
#if MONO
                if (string.Compare(loadPath, "Release", StringComparison.Ordinal) == 0 || string.Compare(loadPath, "bin", StringComparison.Ordinal) == 0
#if DotNetStandard
                    || loadPath.StartsWith("netcoreapp", StringComparison.Ordinal)
#endif
                    || string.Compare(loadPath, "Debug", StringComparison.Ordinal) == 0)
#else
                if (string.Compare(loadPath, "Release", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(loadPath, "bin", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(loadPath, "Debug", StringComparison.OrdinalIgnoreCase) == 0)
#endif
                {
                    loadDirectory = loadDirectory.Parent;
                    if (loadDirectory == null) return directory;
                }
                else return loadDirectory;
            }
            while (true);
        }
    }
}
