using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Net.Sockets;
using System.Net;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 服务
    /// </summary>
    internal class Server : IDisposable
    {
        /// <summary>
        /// TCP监听服务器端套接字
        /// </summary>
        protected System.Net.Sockets.Socket socket;
        /// <summary>
        /// HTTP 注册管理服务
        /// </summary>
        internal readonly HttpRegister.Server RegisterServer;
        /// <summary>
        /// 已绑定域名数量
        /// </summary>
        internal int DomainCount;
        /// <summary>
        /// 套接字等待事件
        /// </summary>
        protected AutoCSer.Threading.Thread.AutoWaitHandle socketHandle;
        /// <summary>
        /// 套接字链表头部
        /// </summary>
        protected SocketLink socketHead;
        /// <summary>
        /// TCP 服务端口信息
        /// </summary>
        private HostPort host;
        /// <summary>
        /// 是否安全服务
        /// </summary>
        internal readonly bool IsSSL;
        /// <summary>
        /// 是否启动成功
        /// </summary>
        internal bool IsStart
        {
            get { return socket != null; }
        }
        /// <summary>
        /// HTTP服务
        /// </summary>
        /// <param name="server">HTTP 注册管理服务</param>
        /// <param name="host">TCP 服务端口信息</param>
        /// <param name="isSSL">是否安全服务</param>
        /// <param name="isStart">是否启动服务</param>
        internal Server(HttpRegister.Server server, ref HostPort host, bool isSSL, bool isStart = true)
        {
            this.RegisterServer = server;
            this.host = host;
            IsSSL = isSSL;
            DomainCount = 1;
            if (isStart) start();
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Dispose()
        {
            if (socket != null)
            {
#if DotNetStandard
                AutoCSer.Net.TcpServer.CommandBase.CloseServer(socket);
#else
                socket.Dispose();
#endif
                socket = null;
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        protected void start()
        {
            try
            {
                IPAddress ipAddress = HostPort.HostToIPAddress(host.Host, RegisterServer.TcpServer.Log);
                socket = new System.Net.Sockets.Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(ipAddress, host.Port));
                socket.Listen(int.MaxValue);
                socketHandle.Set(0);
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(GetSocket);
                if (host.Port == 0) RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Warn, "HTTP 服务端口为 0");
            }
            catch (Exception error)
            {
                Dispose();
                RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error, "HTTP 服务器端口 " + host.Host + ":" + host.Port.toString() + " TCP连接失败)");
            }
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetSocket()
        {
            //ThreadPriority priority = Thread.CurrentThread.Priority;
            if (IsSSL)
            {
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(new AutoCSer.Net.Http.UnionType { Value = this }.SslServer.OnSocket);
            }
            else AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(OnSocket);
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;
            getSocket();
            //Thread.CurrentThread.Priority = priority;
            socketHandle.Set();
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocket()
        {
            System.Net.Sockets.Socket listenSocket = socket;
            SocketLink newSocketLink, head;
            while (socket != null)
            {
                try
                {
                    NEXT:
                    newSocketLink = new SocketLink();
                    newSocketLink.Socket = listenSocket.Accept();
                    if (socket == null) return;
                    do
                    {
                        if ((head = socketHead) == null)
                        {
                            newSocketLink.LinkNext = null;
                            if (Interlocked.CompareExchange(ref socketHead, newSocketLink, null) == null)
                            {
                                socketHandle.Set();
                                newSocketLink = null;
                                goto NEXT;
                            }
                        }
                        else
                        {
                            newSocketLink.LinkNext = head;
                            if (Interlocked.CompareExchange(ref socketHead, newSocketLink, head) == head)
                            {
                                newSocketLink = null;
                                goto NEXT;
                            }
                        }
                        AutoCSer.Threading.ThreadYield.YieldOnly();
                    }
                    while (true);
                }
                catch (Exception error)
                {
                    if (socket == null) break;
                    RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 套接字处理
        /// </summary>
        internal void OnSocket()
        {
            while (this.socket != null)
            {
                socketHandle.Wait();
                SocketLink socket = Interlocked.Exchange(ref socketHead, null);
                do
                {
                    try
                    {
                        while (socket != null) socket = socket.Start(this);
                        break;
                    }
                    catch (Exception error)
                    {
                        RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Debug, error);
                    }
                    socket = socket.Cancel();
                }
                while (true);
            }
            socketHead = null;
        }
        /// <summary>
        /// 停止服务监听
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StopListen()
        {
            if (socket != null)
            {
#if DotNetStandard
                AutoCSer.Net.TcpServer.CommandBase.CloseServer(socket);
#else
                socket.Dispose();
#endif
                socket = null;
            }
        }
    }
}
