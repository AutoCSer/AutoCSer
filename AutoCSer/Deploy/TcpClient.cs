﻿using System;
using System.Threading;
using AutoCSer.Extensions;

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
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.CheckSocketVersion checkSocketVersion;
        ///// <summary>
        ///// 部署服务客户端标识
        ///// </summary>
        //internal AutoCSer.Net.IndexIdentity ClientId;
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
        /// <param name="verifyMethod"></param>
        internal TcpClient(Client client, string name, AutoCSer.Net.TcpInternalServer.ServerAttribute attribute, Func<Server.TcpInternalClient, AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> verifyMethod)
        {
            this.Client = client;
            this.name = name;
#if NoAutoCSer
            throw new Exception();
#else
            TcpInternalClient = new Server.TcpInternalClient(attribute, verifyMethod);
            checkSocketVersion = TcpInternalClient._TcpClient_.CreateCheckSocketVersion(onClientSocket);
#endif
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            try
            {
                switch (parameter.Type)
                {
                    case AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket:
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
                            //ClientId = TcpInternalClient.register();
                            //logKeepCallback = TcpInternalClient.getLog(ClientId, onLog);
                            logKeepCallback = TcpInternalClient.getLog(onLog);
#endif
                            IsClient = true;
                            return;
                        }
                        catch (Exception error)
                        {
                            AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                            checkSocketVersion.DisposeSocket();
                        }
                        IsClient = false;
                        break;
                    case AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SocketDisposed:
                        IsClient = false;
                        break;
                }
            }
            finally
            {
                Client.OnClient(parameter.Type, name);
            }
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
