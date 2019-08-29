using System;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Extension;
using System.IO;
using System.Runtime.CompilerServices;
using System.Net;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册写服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.TcpRegister)]
    public partial class Server :  TcpInternalServer.TimeVerifyServer, IDisposable
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "TcpRegister";
        /// <summary>
        /// 端口分配配置
        /// </summary>
        private static readonly Config config;
        /// <summary>
        /// 全局注册标识
        /// </summary>
        private readonly long ticks = AutoCSer.Pub.StartTime.Ticks;
        /// <summary>
        /// 命令
        /// </summary>
        private enum command
        {
            /// <summary>
            /// TCP 服务端注册
            /// </summary>
            register = 2,
            /// <summary>
            /// TCP 服务端轮询
            /// </summary>
            getLog,
            /// <summary>
            /// 分配服务端口号
            /// </summary>
            getPort,
            /// <summary>
            /// 注册 TCP 服务信息
            /// </summary>
            registerServer,
            /// <summary>
            /// 失败重连检测 TCP 服务信息
            /// </summary>
            checkRegister,
            /// <summary>
            /// 注销TCP服务信息
            /// </summary>
            removeRegister,
        }

        /// <summary>
        /// 客户端信息池
        /// </summary>
        private AutoCSer.Threading.IndexValuePool<ClientInfo> clientPool;
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        private readonly Dictionary<HashString, ServerSet> serverSets = DictionaryCreator.CreateHashString<ServerSet>();
        /// <summary>
        /// 主机端口分配池
        /// </summary>
        private readonly Dictionary<HashString, Ports> ipPorts = DictionaryCreator.CreateHashString<Ports>();
        /// <summary>
        /// 是否需要保存
        /// </summary>
        private bool isNew;
        /// <summary>
        /// 是否释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// TCP 内部注册写服务
        /// </summary>
        public Server()
        {
            clientPool.Reset(256);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0) PopNotNull(this);
        }
        /// <summary>
        /// TCP 服务端注册
        /// </summary>
        /// <returns>TCP 服务端标识</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false, CommandIdentity = (int)command.register)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private ClientId register()
        {
            int index, identity;
            object arrayLock = clientPool.ArrayLock;
            Monitor.Enter(arrayLock);
            try
            {
                index = clientPool.GetIndexContinue();//不能写成一行，可能造成Pool先入栈然后被修改，导致索引溢出
                identity = clientPool.Array[index].Identity;
            }
            finally { Monitor.Exit(arrayLock); }
            return new ClientId { Tick = ticks, Index = index, Identity = identity };
        }
        /// <summary>
        /// TCP 服务端轮询
        /// </summary>
        /// <param name="clientId">TCP 服务端标识</param>
        /// <param name="onLog">TCP 服务注册通知委托</param>
        [TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.TcpQueue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = (int)command.getLog)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getLog(ClientId clientId, Func<TcpServer.ReturnValue<Log>, bool> onLog)
        {
            if (clientId.Tick == ticks)
            {
                object arrayLock = clientPool.ArrayLock;
                Monitor.Enter(arrayLock);
                if (clientPool.Array[clientId.Index].Set(clientId.Identity, onLog))
                {
                    try
                    {
                        foreach (ServerSet serverSet in serverSets.Values) serverSet.OnLog(onLog);
                        onLog(Log.RegisterLoaded);
                    }
                    finally { Monitor.Exit(arrayLock); }
                    return;
                }
                Monitor.Exit(arrayLock);
            }
            onLog(Log.ClientError);
        }
        /// <summary>
        /// 设置只读模式
        /// </summary>
        /// <param name="sender"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void SetReadCommand(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender)
        {
            sender.SetCommand((int)command.register);
            sender.SetCommand((int)command.getLog);
        }
        /// <summary>
        /// 分配服务端口号
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = (int)command.getPort)]
        private int getPort(ClientId clientId, string host)
        {
            if (ticks == clientId.Tick && host != null)
            {
                IPAddress ip = HostPort.HostToIPAddress(host);
                if (ip != null)
                {
                    HashString ipHash = ip.ToString();
                    object arrayLock = clientPool.ArrayLock;
                    Ports ports;
                    Monitor.Enter(arrayLock);
                    if (clientPool.Array[clientId.Index].Identity == clientId.Identity)
                    {
                        try
                        {
                            if (!ipPorts.TryGetValue(ipHash, out ports)) ipPorts.Add(ipHash, ports = new Ports(config));
                            return ports.Next;
                        }
                        finally { Monitor.Exit(arrayLock); }
                    }
                    Monitor.Exit(arrayLock);
                }
            }
            return 0;
        }
        /// <summary>
        /// 注册 TCP 服务信息
        /// </summary>
        /// <param name="ticks">全局注册标识</param>
        /// <param name="server">TCP 服务信息</param>
        /// <returns>注册状态</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = (int)command.registerServer)]
        private bool register(long ticks, ServerInfo server)
        {
            if (ticks == this.ticks && server != null && server.HostToIpAddress())
            {
                server.IsCheckRegister = false;
                HashString serverName = server.Name, ipHash = config.IsPort(server.Port) ? (HashString)server.Host : default(HashString);
                ServerSet serverSet;
                Ports ports;
                Log log = new Log { Type = LogType.RegisterServer, Server = server };
                object arrayLock = clientPool.ArrayLock;
                Monitor.Enter(arrayLock);
                if (clientPool.Array[server.ClientIndex].Identity == server.ClientIdentity)
                {
                    try
                    {
                        if (serverSets.TryGetValue(serverName, out serverSet)) serverSet.Add(log);
                        else serverSets.Add(serverName, serverSet = new ServerSet(log));
                        isNew = true;
                        onLog(log);
                        if (config.IsPort(server.Port))
                        {
                            if (!ipPorts.TryGetValue(ipHash, out ports)) ipPorts.Add(ipHash, ports = new Ports(config));
                            ports.Set(server.Port);
                        }
                    }
                    finally { Monitor.Exit(arrayLock); }
                    return true;
                }
                Monitor.Exit(arrayLock);
            }
            return false;
        }
        /// <summary>
        /// 失败重连检测 TCP 服务信息
        /// </summary>
        /// <param name="ticks">全局注册标识</param>
        /// <param name="server">TCP 服务信息</param>
        /// <returns>注册状态</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = (int)command.checkRegister)]
        private bool checkRegister(long ticks, ServerInfo server)
        {
            if (ticks == this.ticks && server != null && server.HostToIpAddress())
            {
                server.IsCheckRegister = true;
                HashString serverName = server.Name, ipHash = config.IsPort(server.Port) ? (HashString)server.Host : default(HashString);
                ServerSet serverSet;
                Ports ports;
                Log log = new Log { Type = LogType.RegisterServer, Server = server };
                bool isNewLog;
                object arrayLock = clientPool.ArrayLock;
                Monitor.Enter(arrayLock);
                if (clientPool.Array[server.ClientIndex].Identity == server.ClientIdentity)
                {
                    try
                    {
                        if (serverSets.TryGetValue(serverName, out serverSet)) isNewLog = serverSet.Check(log);
                        else
                        {
                            serverSets.Add(serverName, serverSet = new ServerSet(log));
                            isNewLog = true;
                        }
                        isNew |= isNewLog;
                        if (isNewLog)
                        {
                            onLog(log);
                            if (config.IsPort(server.Port))
                            {
                                if (!ipPorts.TryGetValue(ipHash, out ports)) ipPorts.Add(ipHash, ports = new Ports(config));
                                ports.Set(server.Port);
                            }
                        }
                    }
                    finally { Monitor.Exit(arrayLock); }
                    return true;
                }
                Monitor.Exit(arrayLock);
            }
            return false;
        }

        /// <summary>
        /// 注销TCP服务信息
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="server"></param>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = (int)command.removeRegister)]
        private void removeRegister(long ticks, ServerInfo server)
        {
            if (ticks == this.ticks && server != null && server.HostToIpAddress())
            {
                Log log = new Log { Type = LogType.RemoveServer };
                HashString serverName = server.Name, ipHash = config.IsPort(server.Port) ? (HashString)server.Host : default(HashString);
                ServerSet serverSet;
                Ports ports;
                object arrayLock = clientPool.ArrayLock;
                Monitor.Enter(arrayLock);
                if (clientPool.Array[server.ClientIndex].Identity == server.ClientIdentity && serverSets.TryGetValue(serverName, out serverSet))
                {
                    Log removeLog = serverSet.Remove(server);
                    if (removeLog != null)
                    {
                        try
                        {
                            isNew = true;
                            if (serverSet.Server == null) serverSets.Remove(serverName);
                            log.Server = removeLog.Server;
                            onLog(log);
                            if (config.IsPort(server.Port) && ipPorts.TryGetValue(ipHash, out ports)) ports.Clear(server.Port);
                        }
                        finally { Monitor.Exit(arrayLock); }
                        return;
                    }
                }
                Monitor.Exit(arrayLock);
            }
        }
        /// <summary>
        /// 日志回调处理
        /// </summary>
        /// <param name="log"></param>
        private void onLog(Log log)
        {
            int clientCount = clientPool.PoolIndex;
            if (clientCount != 0)
            {
                foreach (ClientInfo client in clientPool.Array)
                {
                    client.OnLog(log);
                    if (--clientCount == 0) break;
                }
            }
        }
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal Server DoubleLinkNext;
        /// <summary>
        /// 上一个节点
        /// </summary>
        internal Server DoubleLinkPrevious;
        /// <summary>
        /// 缓存文件名称
        /// </summary>
        private string cacheFile;
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public override void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            base.SetTcpServer(tcpServer);
            cacheFile = AutoCSer.Config.Pub.Default.CachePath + ServerName + (ServerName == tcpServer.ServerName ? null : ("_" + tcpServer.ServerName)) + ".cache";
            fromCacheFile();
            PushNotNull(this);
        }
        /// <summary>
        /// 从缓存文件恢复TCP服务信息集合
        /// </summary>
        private void fromCacheFile()
        {
            try
            {
                if (File.Exists(cacheFile))
                {
                    Cache cache = default(Cache);
                    if (AutoCSer.BinarySerialize.DeSerializer.DeSerialize(File.ReadAllBytes(cacheFile), ref cache).State == BinarySerialize.DeSerializeState.Success)
                    {
                        foreach (PortCache ipPort in cache.IpPorts) ipPorts.Add(ipPort.Host, new Ports(config, ipPort.Port));
                        foreach (ServerSetCache serverCache in cache.ServerSets) serverSets.Add(serverCache.Name, new ServerSet(serverCache, this));
                    }
                    else server.Log.Add(AutoCSer.Log.LogType.Error, "反序列化失败 " + cacheFile);
                }
            }
            catch (Exception error)
            {
                server.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 设置主机端口
        /// </summary>
        /// <param name="server"></param>
        internal void SetIpPort(ServerInfo server)
        {
            if (server != null && config.IsPort(server.Port))
            {
                HashString ipHash = server.Host;
                Ports ports;
                if (!ipPorts.TryGetValue(ipHash, out ports)) ipPorts.Add(ipHash, ports = new Ports(config));
                ports.Set(server.Port);
            }
        }
        /// <summary>
        /// 保存 TCP 服务信息集合到缓存文件
        /// </summary>
        private unsafe void saveCacheFile()
        {
            object arrayLock = clientPool.ArrayLock;
            Monitor.Enter(arrayLock);
            try
            {
                ServerSetCache[] serverCache = new ServerSetCache[serverSets.Count];
                PortCache[] portCache = new PortCache[ipPorts.Count];
                int index = 0;
                foreach (KeyValuePair<HashString, ServerSet> ServerSet in serverSets) serverCache[index++].Set(ServerSet.Key.String, ServerSet.Value);
                index = 0;
                foreach (KeyValuePair<HashString, Ports> ipPort in ipPorts) portCache[index++].Set(ipPort.Key.String, ipPort.Value.Current);
                File.WriteAllBytes(cacheFile, AutoCSer.BinarySerialize.Serializer.Serialize(new Cache { ServerSets = serverCache, IpPorts = portCache }).Data);
                isNew = false;
            }
            catch (Exception error)
            {
                server.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally { Monitor.Exit(arrayLock); }
        }
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary> 
        private int isTimer = config.SaveSeconds;
        /// <summary>
        /// 激活计时器
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly DateTime timer = Date.NowTime.Now;
        /// <summary>
        /// 定时器触发日志写入
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer()
        {
            if (isNew && Interlocked.Decrement(ref isTimer) == 0)
            {
                saveCacheFile();
                isTimer = config.SaveSeconds;
            }
        }

        /// <summary>
        /// 链表尾部
        /// </summary>
        internal static Server ServerEnd;
        /// <summary>
        /// 链表访问锁
        /// </summary>
        private static int serverLinkLock;
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PushNotNull(Server value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref serverLinkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePush);
            if (ServerEnd == null)
            {
                ServerEnd = value;
                System.Threading.Interlocked.Exchange(ref serverLinkLock, 0);
                Date.NowTime.Flag |= Date.NowTime.OnTimeFlag.TcpRegister;
            }
            else
            {
                ServerEnd.DoubleLinkNext = value;
                value.DoubleLinkPrevious = ServerEnd;
                ServerEnd = value;
                System.Threading.Interlocked.Exchange(ref serverLinkLock, 0);
            }
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PopNotNull(Server value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref serverLinkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
            if (value == ServerEnd)
            {
                if ((ServerEnd = value.DoubleLinkPrevious) != null) ServerEnd.DoubleLinkNext = value.DoubleLinkPrevious = null;
                System.Threading.Interlocked.Exchange(ref serverLinkLock, 0);
            }
            else
            {
                value.freeNotEnd();
                System.Threading.Interlocked.Exchange(ref serverLinkLock, 0);
            }
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeNotEnd()
        {
            DoubleLinkNext.DoubleLinkPrevious = DoubleLinkPrevious;
            if (DoubleLinkPrevious != null)
            {
                DoubleLinkPrevious.DoubleLinkNext = DoubleLinkNext;
                DoubleLinkPrevious = null;
            }
            DoubleLinkNext = null;
        }

        static Server()
        {
            config = ConfigLoader.GetUnion(typeof(Config)).Config ?? new Config();
            config.CheckPort();
        }
    }
}
