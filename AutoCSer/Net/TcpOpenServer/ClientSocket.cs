using System;
using System.Net.Sockets;
using AutoCSer.Extensions;
using System.Threading;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 内部服务客户端套接字
    /// </summary>
    public sealed class ClientSocket : TcpServer.ClientSocket
    {
        /// <summary>
        /// TCP 调用客户端套接字
        /// </summary>
        /// <param name="clientCreator">TCP 服务客户端创建器</param>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        internal ClientSocket(TcpServer.ClientSocketCreator clientCreator, IPAddress ipAddress, int port, int createVersion)
            : base(clientCreator, ipAddress, port, createVersion, clientCreator.Attribute.GetMaxInputSize)
        {
        }
        /// <summary>
        /// TCP 调用客户端套接字
        /// </summary>
        /// <param name="socket">TCP 内部服务客户端套接字</param>
        internal ClientSocket(ClientSocket socket)
            : base(socket)
        {
        }
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        internal override void CreateNew()
        {
            if (ClientCreator.CommandClient.IsDisposed == 0)
            {
                ClientCreator.CommandClient.SocketWait.Reset();
                ClientSocket socket = new ClientSocket(this);
                ClientCreator.CreateSocket = socket;
                if (ClientCreator.CommandClient.IsDisposed == 0) return;
                socket.DisposeSocket();
            }
            ClientCreator.CommandClient.SocketWait.Set();
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        protected override void close()
        {
            isClose = true;
            try
            {
#if DOTNET2
                DisposeSocket();
#else
                if (receiveAsyncEventArgs == null) DisposeSocket();
                else
                {
                    receiveAsyncEventArgs.Completed -= onReceiveAsyncCallback;
                    DisposeSocket();
                    SocketAsyncEventArgsPool.PushNotNull(ref receiveAsyncEventArgs);
                }
#endif
            }
            catch (Exception error)
            {
                ClientCreator.CommandClient.AddLog(error);
            }
            CloseFree();
            disposeCommandPoolTimeout();
        }
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        internal override void Create()
        {
            bool isErrorLog = false;
            socketError = SocketError.Success;
            do
            {
                if (checkCreate() == 0) return;
                if (isSleep)
                {
                    isSleep = false;
                    Thread.Sleep(ClientCreator.CommandClient.FristTryCreateSleep);
                }
                try
                {
                    Socket = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
#if !MONO
                    Socket.ReceiveBufferSize = ClientCreator.CommandClient.ReceiveBufferPool.Size;
                    Socket.SendBufferSize = ClientCreator.CommandClient.SendBufferPool.Size;
#endif
                    Socket.Connect(IpAddress, Port);
                    if (checkCreate() == 0) return;
                    if (onReceiveAsyncCallback == null) onReceiveAsyncCallback = onReceive;
#if !DOTNET2
                    if (receiveAsyncEventArgs == null)
                    {
                        receiveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
                        receiveAsyncEventArgs.Completed += onReceiveAsyncCallback;
                    }
#endif
                    if (ReceiveBuffer.Buffer == null) ClientCreator.CommandClient.ReceiveBufferPool.Get(ref ReceiveBuffer);
                    if (Sender == null) SetSender(new ClientSocketSender(this));
                    receiveBufferSize = ReceiveBuffer.PoolBuffer.Pool.Size;
                    receiveCount = receiveIndex = 0;
                    ReceiveType = TcpServer.ClientSocketReceiveType.CommandIdentityAgain;
#if DOTNET2
                    Socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, Socket);
                    if (socketError == SocketError.Success)
#else
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock.Lock, 1);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
                    if (Socket.ReceiveAsync(receiveAsyncEventArgs))
#endif
                    {
#if !DOTNET2 && !DotNetStandard
                        receiveAsyncLock.Exit();
#endif
                        if (verifyMethod() && ClientCreator.OnSocketVerifyMethod(this))
                        {
                            if (isErrorLog)
                            {
                                ClientCreator.CommandClient.Log.Debug(ClientCreator.Attribute.ServerName + " 客户端 TCP 连接成功 " + IpAddress.ToString() + ":" + Port.toString(), LogLevel.Debug | LogLevel.AutoCSer);
                            }
                            ClientCreator.VerifyCount = 0;
                            return;
                        }
                    }
                    ++ClientCreator.VerifyCount;
                }
                catch (Exception error)
                {
                    if (!isErrorLog)
                    {
                        isErrorLog = true;
                        ClientCreator.CommandClient.Log.Exception(error, ClientCreator.Attribute.ServerName + " 客户端 TCP 连接失败 " + IpAddress.ToString() + ":" + Port.toString(), LogLevel.Exception | LogLevel.AutoCSer);
                    }
                }
                if (Sender != null)
                {
                    VerifyMethodSleep();
                    return;
                }
                ClientCreator.VerifyCount = 0;
                CreateSleep();
            }
            while (true);
        }
#if DOTNET2
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onReceive(IAsyncResult async)
#else
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
            try
            {
#if DOTNET2
                Socket socket = (Socket)async.AsyncState;
                if (socket == Socket)
                {
                    int count = socket.EndReceive(async, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                    if (count > 0)
                    {
                        ++ReceiveCount;
                        switch (ReceiveType)
                        {
                            case TcpServer.ClientSocketReceiveType.CommandIdentity:
                            case TcpServer.ClientSocketReceiveType.CommandIdentityAgain:
                                if (commandIdentityAsync(count)) return;
                                break;
                            case TcpServer.ClientSocketReceiveType.Data: if (dataAsync(count)) return; break;
                            case TcpServer.ClientSocketReceiveType.BigData: if (bigDataAsync(count)) return; break;
                        }
                    }
#if DOTNET2
                    }
                    else Log.Error(IpAddress.ToString() + ":" + Port.toString() + " " + socketError.ToString());
#endif
                }
#if !DOTNET2
                else
                {
                    socketError = receiveAsyncEventArgs.SocketError;
                    Log.Error(IpAddress.ToString() + ":" + Port.toString() + " " + socketError.ToString());
                }
#endif
            }
            catch (Exception error)
            {
                Log.Exception(error, IpAddress.ToString() + ":" + Port.toString());
            }
            if (CheckCreateVersion())
            {
                close();
                CreateNew();
            }
            else
            {
                close();
                if (CreateVersion == ClientCreator.CreateVersion) ClientCreator.CommandClient.SocketWait.Set();
            }
        }
        /// <summary>
        /// 通知服务端取消保持回调
        /// </summary>
        /// <param name="commandIndex">会话标识</param>
        internal override void CancelKeep(int commandIndex)
        {
            new UnionType.ClientSocketSender { Object = Sender }.Value.CancelKeep(commandIndex);
        }
    }
}
