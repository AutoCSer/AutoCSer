using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenStreamServer
{
    /// <summary>
    /// TCP 内部服务套接字数据发送
    /// </summary>
    public sealed class ServerSocketSender : TcpStreamServer.ServerSocketSender<ServerAttribute, Server, ServerSocket, ServerSocketSender>
    {
#if DOTNET2
        /// <summary>
        /// 发送数据异步回调
        /// </summary>
        private AsyncCallback onSendAsyncCallback;
#else
        /// <summary>
        /// 发送数据异步事件
        /// </summary>
        private SocketAsyncEventArgs sendAsyncEventArgs;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        private int sendAsyncLock;
#endif
#endif
        /// <summary>
        /// 发送数据量过低次数
        /// </summary>
        private int sendSizeLessCount;
#if !NOJIT
        /// <summary>
        /// TCP 开放服务套接字数据发送
        /// </summary>
        internal ServerSocketSender() : base() { }
#endif
        /// <summary>
        /// TCP 开放服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字</param>
        internal ServerSocketSender(ServerSocket socket)
            : base(socket)
        {
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Close()
        {
            if (!isClose && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) close();
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        private void closeSocket()
        {
            if (isClose) freeOutput();
            else
            {
                isClose = true;
                try
                {
#if DOTNET2
                    ServerSocket.DisposeSocket();
#else
                    if (sendAsyncEventArgs == null) ServerSocket.DisposeSocket();
                    else
                    {
                        sendAsyncEventArgs.Completed -= onSend;
                        ServerSocket.DisposeSocket();
                        SocketAsyncEventArgsPool.PushNotNull(ref sendAsyncEventArgs);
                    }
#endif
                }
                catch (Exception error)
                {
                    Server.AddLog(error);
                }
                freeBuffer();
                callOnClose();
            }
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        private void close()
        {
            if (isClose) freeOutput();
            else
            {
                isClose = true;
                try
                {
#if DOTNET2
                    if (onSendAsyncCallback != null)
                    {
                        onSendAsyncCallback = null;
                        ServerSocket.DisposeSocket();
                    }
#else
                    if (sendAsyncEventArgs != null)
                    {
                        sendAsyncEventArgs.Completed -= onSend;
                        ServerSocket.DisposeSocket();
                        SocketAsyncEventArgsPool.PushNotNull(ref sendAsyncEventArgs);
                    }
#endif
                }
                catch (Exception error)
                {
                    Server.AddLog(error);
                }
                freeBuffer();
                callOnClose();
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override void VirtualBuildOutput()
        {
            BuildOutput();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal unsafe void BuildOutput()
        {
            if (IsSocket)
            {
                TcpStreamServer.ServerOutput.OutputLink head = null, end;
                TcpServer.SenderBuildInfo buildInfo = new TcpServer.SenderBuildInfo { SendBufferSize = Server.SendBufferPool.Size, IsClientAwaiter = Server.Attribute.IsClientAwaiter };
                UnmanagedStream outputStream;
                try
                {
                    if (Buffer.Buffer == null) Server.SendBufferPool.Get(ref Buffer);
                    if (OutputSerializer == null) outputStream = (OutputSerializer = BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new BinarySerialize.Serializer()).SetTcpServer();
                    else outputStream = OutputSerializer.Stream;
                    int outputSleep = OutputSleep, currentOutputSleep;
                    do
                    {
                        buildInfo.IsNewBuffer = 0;
                        fixed (byte* dataFixed = Buffer.Buffer)
                        {
                            byte* start = dataFixed + Buffer.StartIndex;
                            STREAM:
                            using (outputStream)
                            {
                                if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                buildInfo.Clear();
                                outputStream.ByteSize = 0;
                                currentOutputSleep = outputSleep;
                                if ((head = Outputs.GetClear(out end)) == null) goto CHECK;
                                LOOP:
                                do
                                {
                                    head = head.Build(this, ref buildInfo);
                                    if (buildInfo.IsSend != 0 || !ServerSocket.IsVerifyMethod)
                                    {
                                        if (head != null) Outputs.PushHead(ref head, end);
                                        goto SEND;
                                    }
                                }
                                while (head != null);
                                if (!Outputs.IsEmpty)
                                {
                                    head = Outputs.GetClear(out end);
                                    goto LOOP;
                                }
                                CHECK:
                                if (currentOutputSleep >= 0)
                                {
                                    Thread.Sleep(currentOutputSleep);
                                    if (!Outputs.IsEmpty)
                                    {
                                        head = Outputs.GetClear(out end);
                                        currentOutputSleep = 0;
                                        goto LOOP;
                                    }
                                }
                                if (outputStream.ByteSize == 0)
                                {
                                    outputStream.Dispose();
                                    goto END;
                                }
                                SEND:
                                buildInfo.IsNewBuffer = setSendData(start);
                            }
                            switch (send())
                            {
                                case TcpOpenServer.SendState.Asynchronous: return;
                                case TcpOpenServer.SendState.Error: buildInfo.IsError = true; return;
                            }
                            if (!Outputs.IsEmpty)
                            {
                                if (buildInfo.IsNewBuffer == 0) goto STREAM;
                                goto FIXEDEND;
                            }
                            END:
                            //IsOutput = 0;
                            if (IsSocket)
                            {
                                Interlocked.Exchange(ref IsOutput, 0);
                                if (!Outputs.IsEmpty)
                                {
                                    if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0)
                                    {
                                        if (buildInfo.IsNewBuffer == 0) goto STREAM;
                                        goto FIXEDEND;
                                    }
                                }
                                else if (!IsSocket && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) buildInfo.IsClose = true;
                            }
                            else buildInfo.IsClose = true;
                            return;
                            FIXEDEND:;
                        }
                    }
                    while (buildInfo.IsNewBuffer != 0);
                }
                catch (Exception error)
                {
                    Server.Log.Add(AutoCSer.Log.LogType.Error, error);
                    buildInfo.IsError = true;
                }
                finally
                {
                    if (buildInfo.IsError | buildInfo.IsClose)
                    {
                        if (buildInfo.IsError) closeSocket();
                        else close();
                        TcpStreamServer.ServerOutput.OutputLink.CancelLink(head);
                    }
                }
            }
            else close();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>发送数据状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private TcpOpenServer.SendState send()
        {
#if !DOTNET2
            START:
#endif
            if (IsSocket)
            {
#if DOTNET2
                SocketError socketError;
                if (onSendAsyncCallback == null) onSendAsyncCallback = onSend;
                Socket.BeginSend(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError, onSendAsyncCallback, Socket);
                if (socketError == SocketError.Success) return TcpOpenServer.SendState.Asynchronous;
#else
                if (sendAsyncEventArgs == null)
                {
                    sendAsyncEventArgs = SocketAsyncEventArgsPool.Get();
                    sendAsyncEventArgs.Completed += onSend;
                }
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                sendAsyncEventArgs.SetBuffer(sendData.Array, sendData.Start, sendData.Length);
                if (Socket.SendAsync(sendAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref sendAsyncLock, 0);
#endif
                    return TcpOpenServer.SendState.Asynchronous;
                }
                if (sendAsyncEventArgs.SocketError == SocketError.Success)
                {
                    sendData.MoveStart(sendAsyncEventArgs.BytesTransferred);
#if !DotNetStandard
                    Interlocked.Exchange(ref sendAsyncLock, 0);
#endif
                    if (sendData.Length == 0)
                    {
                        freeCopyBuffer();
                        return TcpOpenServer.SendState.Synchronize;
                    }
                    goto START;
                }
#endif
            }
            return TcpOpenServer.SendState.Error;
        }
#if DOTNET2
        /// <summary>
        /// 数据发送完成后的回调委托
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onSend(IAsyncResult async)
#else
        /// <summary>
        /// 数据发送完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onSend(object sender, SocketAsyncEventArgs async)
#endif
        {
            //IsBuildOutputSynchronize = 0;
            bool isOutput = false;
            try
            {
#if DOTNET2
                Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
                if (socket == Socket)
                {
                    SocketError socketError;
                    int count = socket.EndSend(async, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
                if (sendAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = sendAsyncEventArgs.BytesTransferred;
#endif
                        sendData.MoveStart(count);
                        if (sendData.Length == 0)
                        {
                            sendSizeLessCount = 0;
                            freeCopyBuffer();
                            isOutput = true;
                        }
                        else if (count >= TcpServer.Server.MinSocketSize || (count > 0 && sendSizeLessCount++ == 0))
                        {
                            switch (send())
                            {
                                case TcpOpenServer.SendState.Asynchronous: return;
                                case TcpOpenServer.SendState.Synchronize: isOutput = true; break;
                            }
                        }
#if DOTNET2
                    }
#endif
                }
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            if (IsSocket)
            {
                if (isOutput)
                {
                    if (Outputs.IsEmpty)
                    {
                        Interlocked.Exchange(ref IsOutput, 0);
                        //IsOutput = 0;
                        if (!Outputs.IsEmpty)
                        {
                            if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) BuildOutput();
                        }
                        else
                        {
                            if (!IsSocket && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) close();
                        }
                    }
                    else BuildOutput();
                }
                else closeSocket();
            }
            else close();
        }
    }
}
