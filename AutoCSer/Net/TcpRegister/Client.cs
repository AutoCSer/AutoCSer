using System;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务客户端
    /// </summary>
    internal sealed class Client
    {
        /// <summary>
        /// TCP注册服务名称
        /// </summary>
        private string serviceName;
#if !NoAutoCSer
        /// <summary>
        /// TCP 注册服务客户端
        /// </summary>
        private readonly Server.TcpInternalClient registerClient;
#endif
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.CheckSocketVersion checkSocketVersion;
        /// <summary>
        /// TCP 注册服务访问锁
        /// </summary>
        private readonly object registerLock = new object();
        /// <summary>
        /// 客户端创建等待事件
        /// </summary>
        private AutoCSer.Threading.WaitHandle createWait;
        /// <summary>
        /// TCP 服务端标识
        /// </summary>
        private ClientId clientId;
        /// <summary>
        /// 客户端轮询
        /// </summary>
        private Action<TcpServer.ReturnValue<Log>> logHandle;
        /// <summary>
        /// 客户端保持回调
        /// </summary>
        private TcpServer.KeepCallback logKeep;
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        private readonly Dictionary<HashString, ClientServerSet> serverSets = DictionaryCreator.CreateHashString<ClientServerSet>();
        /// <summary>
        /// TCP 服务信息集合访问锁
        /// </summary>
        private readonly object serverSetLock = new object();
        /// <summary>
        /// TCP 内部服务集合
        /// </summary>
        private LeftArray<IServer> servers;
        /// <summary>
        /// 是否客户端错误
        /// </summary>
        private bool isClientErrorLog;
        /// <summary>
        /// 服务是否加载完毕
        /// </summary>
        private bool isRegisterLoaded;
        /// <summary>
        /// TCP 注册服务客户端
        /// </summary>
        /// <param name="serviceName">TCP 注册服务服务名称</param>
        private Client(string serviceName)
        {
#if NoAutoCSer
            throw new Exception(); 
#else
            //attribute = AutoCSer.config.pub.LoadConfig(new AutoCSer.code.cSharp.tcpServer(), serviceName);
            //attribute.IsIdentityCommand = true;
            //attribute.TcpRegister = null;
            this.serviceName = serviceName;
            createWait.Set(0);
            registerClient = new Server.TcpInternalClient(TcpInternalServer.ServerAttribute.GetConfig(serviceName, typeof(AutoCSer.Net.TcpRegister.Server)));
            //isNewClientErrorLog = true;
            logHandle = onLog;
            checkSocketVersion = registerClient._TcpClient_.CreateCheckSocketVersion(onNewSocket);
#endif
        }
        /// <summary>
        /// 关闭 TCP 注册服务客户端
        /// </summary>
        private void close()
        {
#if NoAutoCSer
            throw new Exception();
#else
            registerClient.Dispose();
#endif
            clientId.Tick = 0;
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onNewSocket(TcpServer.ClientSocketEventParameter parameter)
        {
#if NoAutoCSer
            throw new Exception();
#else
            if (parameter.Type == TcpServer.ClientSocketEventParameter.EventType.SetSocket)
            {
                isClientErrorLog = false;
                do
                {
                    try
                    {
                        if (isClientErrorLog)
                        {
                            isClientErrorLog = false;
                            clientId.Tick = 0;
                        }
                        if (clientId.Tick == 0)
                        {
                            clientId = registerClient.register();
                            if (clientId.Tick != 0) createWait.Set();
                        }
                        if (clientId.Tick != 0)
                        {
                            if (logKeep != null)
                            {
                                logKeep.Dispose();
                                logKeep = null;
                            }
                            if (serverSets.Count != 0)
                            {
                                Monitor.Enter(serverSetLock);
                                try
                                {
                                    LeftArray<HashString> removeKeys = new LeftArray<HashString>(serverSets.Count);
                                    foreach (KeyValuePair<HashString, ClientServerSet> serverSet in serverSets)
                                    {
                                        if (serverSet.Value.Clear()) removeKeys.Add(serverSet.Key);
                                    }
                                    foreach (HashString name in removeKeys) serverSets.Remove(name);
                                }
                                finally { Monitor.Exit(serverSetLock); }
                            }
                            isRegisterLoaded = false;
                            if ((logKeep = registerClient.getLog(clientId, logHandle)) != null)
                            {
                                byte isError = 0;
                                foreach (IServer server in servers)
                                {
                                    Monitor.Enter(registerLock);
                                    try
                                    {
                                        ServerInfo serverInfo = server.TcpRegisterInfo;
                                        long clientTick = clientId.Tick;
                                        if (serverInfo != null && serverInfo.RegisterTick != clientTick)
                                        {
                                            serverInfo.ClientIndex = clientId.Index;
                                            serverInfo.ClientIdentity = clientId.Identity;
                                            if (registerClient.checkRegister(clientTick, serverInfo).Value) serverInfo.RegisterTick = clientTick;
                                            else if (isError == 0)
                                            {
                                                isError = 1;
                                                if (registerClient.checkRegister(clientTick, serverInfo).Value) serverInfo.RegisterTick = clientTick;
                                            }
                                        }
                                    }
                                    catch (Exception error)
                                    {
                                        isError = 1;
                                        server.AddLog(error);
                                    }
                                    finally { Monitor.Exit(registerLock); }
                                }
                                return;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        registerClient._TcpClient_.Log.Add(AutoCSer.Log.LogType.Debug, error, null, true);
                    }
                    Thread.Sleep(registerClient._TcpClient_.TryCreateSleep);
                }
                while (registerClient._TcpClient_.IsDisposed == 0);
            }
#endif
        }
        /// <summary>
        /// 客户端轮询
        /// </summary>
        /// <param name="result">轮询结果</param>
        private void onLog(TcpServer.ReturnValue<Log> result)
        {
            if (result.Type == TcpServer.ReturnType.Success)
            {
                switch (result.Value.Type)
                {
                    case LogType.RegisterServer: registerServer(result.Value); return;
                    case LogType.RemoveServer: removeServer(result.Value); return;
                    case LogType.RegisterLoaded:
                        isRegisterLoaded = true;
                        Monitor.Enter(serverSetLock);
                        try
                        {
                            foreach (ClientServerSet serverSet in serverSets.Values) serverSet.OnLoaded();
                        }
                        finally { Monitor.Exit(serverSetLock); }
                        return;
                    case LogType.ClientError: isClientErrorLog = true; return;
                }
            }
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="log"></param>
        private void registerServer(Log log)
        {
            HashString name = log.Server.Name;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Add(log, isRegisterLoaded);
                else serverSets.Add(name, serverSet = new ClientServerSet(log));
            }
            finally { Monitor.Exit(serverSetLock); }
        }
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="log"></param>
        private void removeServer(Log log)
        {
            HashString name = log.Server.Name;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Remove(log, isRegisterLoaded);
            }
            finally { Monitor.Exit(serverSetLock); }
        }
        /// <summary>
        /// TCP 客户端注册
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        internal void Register(IClient client)
        {
            HashString name = client.ServerName;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Add(client, isRegisterLoaded);
                else
                {
                    serverSets.Add(name, serverSet = new ClientServerSet(client));
                    if (isRegisterLoaded) client.OnServerChange(null);
                }
            }
            finally { Monitor.Exit(serverSetLock); }
        }
        /// <summary>
        /// TCP 客户端注销
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        internal void Remove(IClient client)
        {
            HashString name = client.ServerName;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet) && serverSet.Remove(client)) serverSets.Remove(name);
            }
            finally { Monitor.Exit(serverSetLock); }
        }

        /// <summary>
        /// 获取服务端口
        /// </summary>
        /// <typeparam name="serverAttributeType"></typeparam>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal bool GetPort<serverAttributeType>(serverAttributeType attribute)
            where serverAttributeType : TcpServer.ServerBaseAttribute, TcpRegister.IServerAttribute
        {
            if (clientId.Tick != 0)
            {
                Monitor.Enter(registerLock);
                try
                {
#if NoAutoCSer
                    throw new Exception();
#else
                    attribute.ClientRegisterPort = registerClient.getPort(clientId, attribute.ClientRegisterHost).Value;
#endif
                }
                finally { Monitor.Exit(registerLock); }
                if (attribute.ClientRegisterPort != 0)
                {
                    attribute.Port = attribute.ClientRegisterPort;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 注册TCP服务端
        /// </summary>
        /// <param name="server"></param>
        /// <returns>是否注册成功</returns>
        internal bool Register(IServer server)
        {
#if NoAutoCSer
            throw new Exception();
#else
            ServerInfo serverInfo = server.CreateServerInfo();
            serverInfo.ClientIndex = clientId.Index;
            serverInfo.ClientIdentity = clientId.Identity;
            Monitor.Enter(registerLock);
            try
            {
                servers.Add(server);
                long tick = clientId.Tick;
                if (registerClient.register(tick, serverInfo).Value)
                {
                    server.TcpRegisterInfo = serverInfo;
                    serverInfo.RegisterTick = tick;
                    return true;
                }
            }
            catch (Exception error)
            {
                server.AddLog(error);
            }
            finally { Monitor.Exit(registerLock); }
#endif
            return false;
        }
        /// <summary>
        /// 删除注册TCP服务端
        /// </summary>
        /// <param name="server"></param>
        internal void RemoveRegister(IServer server)
        {
#if NoAutoCSer
                throw new Exception();
#else
            if (server.TcpRegisterInfo != null)
            {
                Monitor.Enter(registerLock);
                try
                {
                    if (server.TcpRegisterInfo != null)
                    {
                        servers.RemoveToEnd(server);
                        if (registerClient.removeRegister(clientId.Tick, server.TcpRegisterInfo).Type == AutoCSer.Net.TcpServer.ReturnType.Success) server.TcpRegisterInfo = null;
                    }
                }
                catch (Exception error)
                {
                    server.AddLog(error);
                }
                finally { Monitor.Exit(registerLock); }
            }
#endif
        }

        /// <summary>
        /// TCP注册服务客户端缓存
        /// </summary>
        private static readonly Dictionary<HashString, Client> clients = DictionaryCreator.CreateHashString<Client>();
        /// <summary>
        /// TCP注册服务客户端 访问锁
        /// </summary>
        private static readonly object clientsLock = new object();
        /// <summary>
        /// 获取 TCP 注册服务客户端
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="Log">日志处理</param>
        /// <returns>TCP注册服务客户端,失败返回null</returns>
        internal static Client Get(string serviceName, ILog Log)
        {
            if (!string.IsNullOrEmpty(serviceName))
            {
                int count = int.MinValue;
                Client client = null;
                HashString nameKey = serviceName;
                Monitor.Enter(clientsLock);
                try
                {
                    if (!clients.TryGetValue(nameKey, out client))
                    {
                        try
                        {
                            client = new Client(serviceName);
                        }
                        catch (Exception error)
                        {
                            Log.Add(AutoCSer.Log.LogType.Error, error);
                        }
                        if (client != null)
                        {
                            count = clients.Count;
                            clients.Add(nameKey, client);
                        }
                    }
                }
                finally { Monitor.Exit(clientsLock); }
                if (count == 0) AutoCSer.DomainUnload.Unloader.Add(null, AutoCSer.DomainUnload.Type.TcpRegisterClientClose);
                if (client != null)
                {
                    client.createWait.Wait();
                    return client;
                }
            }
            return null;
        }
        /// <summary>
        /// 关闭 TCP 注册服务客户端
        /// </summary>
        internal static void Close()
        {
            Monitor.Enter(clientsLock);
            try
            {
                if (clients.Count != 0)
                {
                    foreach (Client client in clients.Values) client.close();
                    clients.Clear();
                }
            }
            finally { Monitor.Exit(clientsLock); }
        }
    }
}
