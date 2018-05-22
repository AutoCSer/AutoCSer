using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenStreamServer
{
    /// <summary>
    /// TCP 服务端
    /// </summary>
    public abstract unsafe class Server : TcpStreamServer.Server<ServerAttribute, Server, ServerSocketSender>
    {
        /// <summary>
        /// 套接字等待事件
        /// </summary>
        private AutoCSer.Threading.Thread.AutoWaitHandle socketHandle;
        /// <summary>
        /// 套接字链表头部
        /// </summary>
        private SocketLink socketHead;
        /// <summary>
        /// TCP 服务端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">同步验证接口</param>
        /// <param name="log">日志接口</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public Server(ServerAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, ILog log)
            : base(attribute, verify, log)
        {
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal override void GetSocket()
        {
            //ThreadPriority priority = Thread.CurrentThread.Priority;
            ReceiveVerifyCommandTimeout = SocketTimeoutLink.TimerLink.Get(Attribute.ReceiveVerifyCommandSeconds > 0 ? Attribute.ReceiveVerifyCommandSeconds : ServerAttribute.DefaultReceiveVerifyCommandSeconds);
            socketHandle.Set(0);
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(OnSocket);
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;
            if (verify == null) getSocket();
            else getSocketVerify();
            //Thread.CurrentThread.Priority = priority;
            socketHandle.Set();
            SocketTimeoutLink.TimerLink.Free(ref ReceiveVerifyCommandTimeout);
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocket()
        {
            Socket listenSocket = this.Socket;
            SocketLink head, newSocketLink;
            while (isListen != 0)
            {
                try
                {
                    NEXT:
                    newSocketLink = new SocketLink();
                    newSocketLink.Socket = listenSocket.Accept();
                    if (isListen == 0)
                    {
                        if (this.Socket != null)
                        {
                            this.Socket = null;
#if DotNetStandard
                            AutoCSer.Net.TcpServer.CommandBase.CloseServer(listenSocket);
#else
                            listenSocket.Dispose();
#endif
                        }
                        return;
                    }
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
                    if (isListen == 0) return;
                    Log.Add(AutoCSer.Log.LogType.Error, error);
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocketVerify()
        {
            Socket listenSocket = this.Socket;
            SocketLink head, newSocketLink;
            while (isListen != 0)
            {
                try
                {
                    NEXT:
                    newSocketLink = new SocketLink();
                    ACCEPT:
                    newSocketLink.Socket = listenSocket.Accept();
                    if (isListen == 0)
                    {
                        if (this.Socket != null)
                        {
                            this.Socket = null;
#if DotNetStandard
                            AutoCSer.Net.TcpServer.CommandBase.CloseServer(listenSocket);
#else
                            listenSocket.Dispose();
#endif
                        }
                        return;
                    }
                    if (verify(newSocketLink.Socket))
                    {
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
                    newSocketLink.DisposeSocket();
                    goto ACCEPT;
                }
                catch (Exception error)
                {
                    if (isListen == 0) return;
                    Log.Add(AutoCSer.Log.LogType.Error, error);
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 套接字处理
        /// </summary>
        internal void OnSocket()
        {
            ServerSocket serverSocket = null;
            while (isListen != 0)
            {
                socketHandle.Wait();
                SocketLink socket = Interlocked.Exchange(ref socketHead, null);
                do
                {
                    try
                    {
                        while (socket != null) socket = socket.Start(this, ref serverSocket);
                        break;
                    }
                    catch (Exception error)
                    {
                        Log.Add(AutoCSer.Log.LogType.Debug, error);
                    }
                    if (serverSocket != null)
                    {
                        serverSocket.Close();
                        serverSocket = null;
                    }
                    socket = socket.Cancel();
                }
                while (true);
            }
            socketHead = null;
        }
    }
}
