using System;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端套接字初始化处理
    /// </summary>
    public sealed class CheckSocketVersion : IDisposable
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        private readonly ClientBase client;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly Action<ClientSocketEventParameter> onClientSocketHandle;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly Action<ClientSocketEventParameter> onCheckSocketVersion;
        /// <summary>
        /// 当前缓存主服务客户端 TCP 套接字
        /// </summary>
        private ClientSocketBase socket;
        /// <summary>
        /// 是否连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                ClientSocketBase socket = this.socket;
                return socket != null && !socket.IsClose;
            }
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="client">TCP 服务客户端</param>
        /// <param name="onCheckSocketVersion">TCP 客户端套接字初始化处理</param>
        internal CheckSocketVersion(ClientBase client, Action<ClientSocketEventParameter> onCheckSocketVersion)
        {
            if (onCheckSocketVersion == null) throw new ArgumentNullException();
            this.client = client;
            this.onCheckSocketVersion = onCheckSocketVersion;
            onClientSocketHandle = onClientSocket;
            client.OnSocket(onClientSocketHandle);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            client.RemoveOnSetSocket(onClientSocketHandle);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter">TCP 客户端套接字事件参数</param>
        private void onClientSocket(ClientSocketEventParameter parameter)
        {
            if (parameter.Type != ClientSocketEventParameter.EventType.SetSocket || parameter.Socket.IsSocketVersion(ref socket))
            {
                onCheckSocketVersion(parameter);
            }
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DisposeSocket()
        {
            System.Net.Sockets.Socket socket = this.socket.Socket;
            if (socket != null) AutoCSer.Net.TcpServer.CommandBase.ShutdownClient(socket);
        }
    }
}
