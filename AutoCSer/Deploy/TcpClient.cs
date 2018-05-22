using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署服务客户端
    /// </summary>
    internal sealed class TcpClient
    {
        /// <summary>
        /// 服务命名
        /// </summary>
        private readonly string name;
        /// <summary>
        /// 部署服务客户端
        /// </summary>
        internal readonly Client Client;
#if !NoAutoCSer
        /// <summary>
        /// 部署服务客户端
        /// </summary>
        internal Server.TcpInternalClient TcpInternalClient;
#endif
        /// <summary>
        /// 部署服务客户端 TCP 套接字更新访问锁
        /// </summary>
        private readonly object newSocketLock = new object();
        /// <summary>
        /// 当前部署服务客户端 TCP 套接字
        /// </summary>
        private AutoCSer.Net.TcpServer.ClientSocketBase socket;
        /// <summary>
        /// 部署服务客户端标识
        /// </summary>
        internal AutoCSer.Net.IndexIdentity ClientId;
        /// <summary>
        /// 部署任务状态更新日志保持回调
        /// </summary>
        private AutoCSer.Net.TcpServer.KeepCallback logKeepCallback;
        /// <summary>
        /// 部署服务客户端是否可用
        /// </summary>
        internal bool IsClient;
        /// <summary>
        /// 部署服务客户端
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        internal TcpClient(Client client, string name, AutoCSer.Net.TcpInternalServer.ServerAttribute attribute)
        {
            this.Client = client;
            this.name = name;
#if NoAutoCSer
            throw new Exception();
#else
            TcpInternalClient = new Server.TcpInternalClient(attribute);
            TcpInternalClient._TcpClient_.OnSetSocket(onClientSocket);
#endif
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="socket"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketBase socket)
        {
            Monitor.Enter(newSocketLock);
            try
            {
                if (socket.IsSocketVersion(ref this.socket))
                {
                    try
                    {
                        if (logKeepCallback != null)
                        {
                            logKeepCallback.Dispose();
                            logKeepCallback = null;
                        }
#if NoAutoCSer
                        throw new Exception();
#else
                        ClientId = TcpInternalClient.register();
                        logKeepCallback = TcpInternalClient.getLog(ClientId, onLog);
#endif
                        IsClient = true;
                        Client.OnClient(name);
                        return;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
#if DotNetStandard
                        AutoCSer.Net.TcpServer.CommandBase.CloseClient(socket.Socket);
#else
                        socket.Socket.Dispose();
#endif
                    }
                    IsClient = false;
                }
            }
            finally { Monitor.Exit(newSocketLock); }
        }
        /// <summary>
        /// 部署任务状态更新日志回调
        /// </summary>
        /// <param name="log"></param>
        private void onLog(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Deploy.Log> log)
        {
            if (log.Type == Net.TcpServer.ReturnType.Success) Client.OnGetLog(name, log.Value);
        }
    }
}
