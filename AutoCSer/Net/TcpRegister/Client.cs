using System;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using System.Net;

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
        private readonly HashString serviceName;
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
        /// 客户端轮询
        /// </summary>
        private readonly Action<TcpServer.ReturnValue<ServerLog>> logHandle;
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
        private readonly HashSet<IServer> servers;
        /// <summary>
        /// 客户端保持回调
        /// </summary>
        private TcpServer.KeepCallback logKeep;
        /// <summary>
        /// TCP 注册服务客户端
        /// </summary>
        /// <param name="serviceName">TCP 注册服务服务名称</param>
        private Client(ref HashString serviceName)
        {
#if NoAutoCSer
            throw new Exception(); 
#else
            this.serviceName = serviceName;
            servers = HashSetCreator.CreateAny<IServer>();
            registerClient = new Server.TcpInternalClient(TcpInternalServer.ServerAttribute.GetConfig(serviceName, typeof(AutoCSer.Net.TcpRegister.Server)));
            logHandle = onLog;
            checkSocketVersion = registerClient._TcpClient_.CreateCheckSocketVersion(onNewSocket);
#endif
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
                try
                {
                    if (logKeep != null)
                    {
                        logKeep.Dispose();
                        logKeep = null;
                    }
                    if ((logKeep = registerClient.getLog(logHandle)) != null)
                    {
                        Monitor.Enter(registerLock);
                        try
                        {
                            foreach (IServer server in servers) registerClient.appendLog(server.CreateServerLog(LogType.RegisterServer));
                        }
                        finally { Monitor.Exit(registerLock); }
                    }
                    else registerClient._TcpClient_.Log.Error("TCP 注册服务客户端 " + serviceName.String.String + " 获取日志失败", LogLevel.Error | LogLevel.AutoCSer);
                }
                catch (Exception error)
                {
                    registerClient._TcpClient_.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
            }
#endif
        }
        /// <summary>
        /// 客户端轮询
        /// </summary>
        /// <param name="result">轮询结果</param>
        private void onLog(TcpServer.ReturnValue<ServerLog> result)
        {
            if (result.Type == TcpServer.ReturnType.Success)
            {
                switch (result.Value.LogType)
                {
                    case LogType.RegisterServer: registerServer(result.Value); return;
                    case LogType.RemoveServer: removeServer(result.Value); return;
                    default:
                        registerClient._TcpClient_.Log.Error("未知的 TCP 内部注册服务更新日志类型 " + result.Value.toJson(), LogLevel.Error | LogLevel.AutoCSer);
                        return;
                }
            }
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="log"></param>
        private void registerServer(ServerLog log)
        {
            HashString name = log.Name;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Add(log);
                else serverSets.Add(name, serverSet = new ClientServerSet(log));
            }
            finally { Monitor.Exit(serverSetLock); }
        }
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="log"></param>
        private void removeServer(ServerLog log)
        {
            HashString name = log.Name;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Remove(log);
            }
            finally { Monitor.Exit(serverSetLock); }
        }
        /// <summary>
        /// TCP 客户端注册
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        internal void Register(IClient client)
        {
            HashString name = client.ServerName ?? string.Empty;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Add(client);
                else
                {
                    serverSets.Add(name, serverSet = new ClientServerSet(client));
                    client.OnServerChange(null);
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
            HashString name = client.ServerName ?? string.Empty;
            ClientServerSet serverSet;
            Monitor.Enter(serverSetLock);
            try
            {
                if (serverSets.TryGetValue(name, out serverSet)) serverSet.Remove(client);
            }
            finally { Monitor.Exit(serverSetLock); }
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
            ServerLog serverInfo = server.CreateServerLog(LogType.RegisterServer);
            Monitor.Enter(registerLock);
            try
            {
                servers.Add(server);
                if (registerClient.appendLog(serverInfo).Value) return true;
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
            ServerLog log = server.CreateServerLog(LogType.RemoveServer);
            Monitor.Enter(registerLock);
            try
            {
                servers.Remove(server);
                registerClient.appendLog(log);
            }
            catch (Exception error)
            {
                server.AddLog(error);
            }
            finally { Monitor.Exit(registerLock); }
#endif
        }

        /// <summary>
        /// 获取TCP注册服务名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static HashString getServiceName(Client client)
        {
            return client.serviceName;
        }
        /// <summary>
        /// TCP注册服务客户端缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashString, Client> clients = new AutoCSer.Threading.LockLastDictionary<HashString, Client>(getServiceName);
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
                Client client;
                HashString nameKey = serviceName;
                if (!clients.TryGetValue(nameKey, out client))
                {
                    try
                    {
                        clients.Set(nameKey, client = new Client(ref nameKey));
                    }
                    catch (Exception error)
                    {
                        Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    finally { clients.Exit(); }
                }
                return client;
            }
            return null;
        }
    }
}
