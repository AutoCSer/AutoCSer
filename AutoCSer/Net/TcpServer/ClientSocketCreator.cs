﻿using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端创建器
    /// </summary>
    public sealed class ClientSocketCreator
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        public readonly ClientBase CommandClient;
        /// <summary>
        /// TCP 服务调用配置
        /// </summary>
        internal ServerBaseAttribute Attribute { get { return CommandClient.Attribute; } }
        /// <summary>
        /// 服务主机名称
        /// </summary>
        internal string Host;
        /// <summary>
        /// 服务 IP 地址
        /// </summary>
        internal IPAddress IpAddress;
        /// <summary>
        /// 服务端口
        /// </summary>
        internal int Port;
        /// <summary>
        /// 验证失败次数
        /// </summary>
        internal int VerifyCount;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        internal ClientSocketBase Socket;
        /// <summary>
        /// 当前正在建立的连接
        /// </summary>
        internal ClientSocketBase CreateSocket;
        /// <summary>
        /// 服务更新版本号
        /// </summary>
        internal volatile int CreateVersion;
        /// <summary>
        /// TCP 服务客户端创建器
        /// </summary>
        /// <param name="commandClient">TCP 服务客户端</param>
        internal ClientSocketCreator(ClientBase commandClient)
        {
            this.CommandClient = commandClient;
            Host = Attribute.Host;
            Port = Attribute.Port;

            IpAddress = HostPort.HostToIPAddress(this.Host, CommandClient.Log);
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DisposeSocket()
        {
            if (CreateSocket != null) CreateSocket.DisposeSocket();
        }

        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ClientSocketBase WaitSocket()
        {
            return Socket ?? waitSocket();
        }
        /// <summary>
        /// 等待套接字
        /// </summary>
        /// <returns></returns>
        private TcpServer.ClientSocketBase waitSocket()
        {
            if (CommandClient.TcpRegisterName == null) TryCreateSocket();
            CommandClient.SocketWait.Wait();
            return Socket;
        }
        /// <summary>
        /// 尝试创建第一个套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TryCreateSocket()
        {
            if (Interlocked.CompareExchange(ref CreateVersion, 1, 0) == 0) createSocket(IpAddress, Port, 1);
        }
        /// <summary>
        /// 检测主机名称是否可用
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool check(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
            {
                CommandClient.Log.Error(Host + " IP 解析失败", LogLevel.Error | LogLevel.AutoCSer);
                return false;
            }
            if (port == 0)
            {
                CommandClient.Log.Error(CommandClient.Attribute.ServerName + " 端口号不能为 0", LogLevel.Error | LogLevel.AutoCSer);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建套接字
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        private void createSocket(IPAddress ipAddress, int port, int createVersion)
        {
            if (check(ipAddress, port))
            {
                ClientSocketBase createSocket = CommandClient.CreateSocketByCreator(this, ipAddress, port, createVersion);
                if (createSocket != null)
                {
                    CreateSocket = createSocket;
                    return;
                }
            }
            CommandClient.SocketWait.Set();
        }
        /// <summary>
        /// 套接字验证通过以后的处理
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal bool OnSocketVerifyMethod(TcpServer.ClientSocketBase socket)
        {
            do
            {
                ClientSocketBase oldSocket = this.Socket;
                if (oldSocket != null && oldSocket.CreateVersion > socket.CreateVersion) return false;
                if (Interlocked.CompareExchange(ref this.Socket, socket, oldSocket) == oldSocket)
                {
                    CommandClient.SocketWait.Set();
                    CommandClient.CallOnSocket(this, socket, ClientSocketEventParameter.EventType.SetSocket);
                    return true;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 设置 TCP 客户端套接字事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnSetSocket()
        {
            if (Socket != null) CommandClient.CallOnSetSocketOnly(this, Socket);
        }
        /// <summary>
        /// 服务更新
        /// </summary>
        /// <param name="serverSet"></param>
        internal void OnServerChange(TcpRegister.ServerSet serverSet)
        {
            TcpRegister.ServerLog server = serverSet.Server;
            IPAddress ipAddress = HostPort.HostToIPAddress(server.Host, CommandClient.Log);
            if (server.Port == Port && ipAddress.Equals(IpAddress)) TryCreateSocket();
            else
            {
                Host = server.Host;
                createSocket(IpAddress = ipAddress, Port = server.Port, Interlocked.Increment(ref CreateVersion));
            }
        }
        /// <summary>
        /// 移除 TCP 服务客户端套接字
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnDisposeSocket(TcpServer.ClientSocketBase socket)
        {
            if (Interlocked.CompareExchange(ref Socket, null, socket) == socket) CommandClient.CallOnSocket(this, socket, ClientSocketEventParameter.EventType.SocketDisposed);
        }
    }
}
