using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// TCP 内部服务客户端套接字
    /// </summary>
    public sealed class ClientSocket : TcpStreamServer.ClientSocket<ServerAttribute>
    {
        /// <summary>
        /// TCP 调用客户端套接字
        /// </summary>
        /// <param name="commandClient">TCP 调用客户端</param>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        internal ClientSocket(TcpStreamServer.Client<ServerAttribute> commandClient, IPAddress ipAddress, int port, int createVersion)
            : base(commandClient, ipAddress, port, createVersion, int.MaxValue)
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
            if (CommandClient.IsDisposed == 0)
            {
                CommandClient.SocketWait.Reset();
                ClientSocket socket = new ClientSocket(this);
                CommandClient.CreateSocket = socket;
                if (CommandClient.IsDisposed != 0) socket.DisposeSocket();
            }
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        private void close()
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
                    receiveAsyncEventArgs.Completed -= onReceive;
                    DisposeSocket();
                    SocketAsyncEventArgsPool.PushNotNull(ref receiveAsyncEventArgs);
                }
#endif
            }
            catch (Exception error)
            {
                CommandClient.AddLog(error);
            }
            CloseFree();
        }
        /// <summary>
        /// 版本有效性检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int checkCreate()
        {
            if ((CommandClient.IsDisposed | (CreateVersion ^ CommandClient.CreateVersion)) == 0) return 1;
            close();
            return 0;
        }
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        internal override void Create()
        {
            bool isErrorLog = false, isReceiveAsync = false;
            socketError = SocketError.Success;
            do
            {
                if (checkCreate() == 0) return;
                if (isSleep)
                {
                    isSleep = false;
                    Thread.Sleep(CommandClient.TryCreateSleep);
                    if (checkCreate() == 0) return;
                }
                try
                {
                    Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
#if !MONO
                    Socket.ReceiveBufferSize = CommandClient.ReceiveBufferPool.Size;
                    Socket.SendBufferSize = CommandClient.SendBufferPool.Size;
#endif
                    Socket.Connect(ipAddress, port);
                    if (checkCreate() == 0) return;
#if DOTNET2
                    if (onReceiveAsyncCallback == null) onReceiveAsyncCallback = onReceive;
#else
                    if (receiveAsyncEventArgs == null)
                    {
                        receiveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
                        receiveAsyncEventArgs.Completed += onReceive;
                    }
#endif
                    if (ReceiveBuffer.Buffer == null) CommandClient.ReceiveBufferPool.Get(ref ReceiveBuffer);
                    if (Sender == null) SetSender(new ClientSocketSender(this));
                    receiveBufferSize = ReceiveBuffer.PoolBuffer.Pool.Size;
                    receiveCount = receiveIndex = 0;
                    ReceiveType = TcpServer.ClientSocketReceiveType.CommandIdentity;
#if DOTNET2
                    Socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, Socket);
                    if (socketError == SocketError.Success)
#else
                    receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
                    if (Socket.ReceiveAsync(receiveAsyncEventArgs))
#endif
                    {
                        isReceiveAsync = true;
                        if (verifyMethod(CommandClient))
                        {
                            if (CommandClient.SetSocket(this))
                            {
                                if (isErrorLog)
                                {
                                    CommandClient.Log.add(AutoCSer.Log.LogType.Debug, CommandClient.Attribute.ServerName + " 客户端 TCP 连接成功 " + ipAddress.ToString() + ":" + port.toString());
                                }
                                return;
                            }
                        }
                        if (Socket != null)
                        {
#if DotNetStandard
                            AutoCSer.Net.TcpServer.CommandBase.CloseClientNotNull(Socket);
#else
                            Socket.Dispose();
#endif
                            Socket = null;
                        }
                        return;
                    }
                }
                catch (Exception error)
                {
                    if (!isErrorLog)
                    {
                        isErrorLog = true;
                        CommandClient.Log.add(AutoCSer.Log.LogType.Debug, error, CommandClient.Attribute.ServerName + " 客户端 TCP 连接失败 " + ipAddress.ToString() + ":" + port.toString());
                    }
                }
                if (isReceiveAsync) return;
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
                Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
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
                                case TcpServer.ClientSocketReceiveType.CommandIdentity: if (dataSizeAsync(count)) return; break;
                                case TcpServer.ClientSocketReceiveType.Data: if (dataAsync(count)) return; break;
                                case TcpServer.ClientSocketReceiveType.BigData: if (bigDataAsync(count)) return; break;
                                case TcpServer.ClientSocketReceiveType.CompressionData: if (compressionDataAsync(count)) return; break;
                                case TcpServer.ClientSocketReceiveType.CompressionBigData: if (compressionBigDataAsync(count)) return; break;
                            }
                        }
#if DOTNET2
                    }
#endif
                }
            }
            catch (Exception error)
            {
                Log.add(AutoCSer.Log.LogType.Debug, error);
            }
            if (CheckCreateVersion())
            {
                //Console.WriteLine("... CreateNew ...");
                close();
                CreateNew();
            }
            else close();
            freeCommandQueue();
        }
    }
}
