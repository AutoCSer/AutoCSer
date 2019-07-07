using System;
using AutoCSer.Extension;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 内部服务套接字数据发送
    /// </summary>
    public sealed class ServerSocketSender : TcpServer.ServerSocketSender<ServerAttribute, Server, ServerSocket, ServerSocketSender>
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
        /// <param name="socket">TCP 开放服务套接字</param>
        internal ServerSocketSender(ServerSocket socket)
            : base(socket, Threading.Thread.CallType.TcpOpenServerSocketSenderBuildOutput)
        {
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public Func<TcpServer.ReturnValue, bool> GetCallback(TcpServer.OutputInfo outputInfo)
        {
            return outputInfo.IsKeepCallback == 0 ? (AutoCSer.Threading.RingPool<ServerCallback>.Default.Pop() ?? new ServerCallback(0)).Set(this, outputInfo.IsBuildOutputThread) : (new ServerCallback(1)).SetKeep(this, outputInfo.IsBuildOutputThread);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public Func<TcpServer.ReturnValue<returnType>, bool> GetCallback<outputParameterType, returnType>(TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
#if NOJIT
            where outputParameterType : struct, IReturnParameter
#else
 where outputParameterType : struct, IReturnParameter<returnType>
#endif
        {
            if (outputInfo.IsKeepCallback == 0)
            {
                return (AutoCSer.Threading.RingPool<ServerCallback<outputParameterType, returnType>>.Default.Pop() ?? new ServerCallback<outputParameterType, returnType>(0)).Set(this, outputInfo, ref outputParameter);
            }
            return (new ServerCallback<outputParameterType, returnType>(1)).SetKeep(this, outputInfo, ref outputParameter);
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
                        //ServerSocket.DisposeSocket();
                    }
#else
                    if (sendAsyncEventArgs != null)
                    {
                        sendAsyncEventArgs.Completed -= onSend;
                        //ServerSocket.DisposeSocket();
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
        internal unsafe void BuildOutput()
        {
            if (IsSocket)
            {
                TcpServer.ServerOutput.OutputLink head = null, end;
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
                                outputStream.ByteSize = TcpServer.ServerOutput.OutputLink.StreamStartIndex;
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
                                if (buildInfo.Count == 0)
                                {
                                    outputStream.Dispose();
                                    goto END;
                                }
                            SEND:
                                buildInfo.IsNewBuffer = setSendData(start, buildInfo.Count);
                            }
                            switch (send())
                            {
                                case SendState.Asynchronous: return;
                                case SendState.Error: buildInfo.IsError = true; return;
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
                        FIXEDEND: ;
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
                        TcpServer.ServerOutput.OutputLink.CancelLink(head);
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
        private SendState send()
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
                if (socketError == SocketError.Success) return SendState.Asynchronous;
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
                    return SendState.Asynchronous;
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
                        return SendState.Synchronize;
                    }
                    goto START;
                }
#endif
            }
            return SendState.Error;
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
                                case SendState.Asynchronous: return;
                                case SendState.Synchronize: isOutput = true; break;
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
        /// <summary>
        /// 获取远程表达式服务端节点标识
        /// </summary>
        /// <param name="data"></param>
        internal void GetRemoteExpressionNodeId(ref SubArray<byte> data)
        {
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
            try
            {
                RemoteExpression.ServerNodeIdChecker.Input inputParameter = default(RemoteExpression.ServerNodeIdChecker.Input);
                if (DeSerialize(ref data, ref inputParameter, false))
                {
                    if (Server.Attribute.RemoteExpressionServerTask == TcpServer.ServerTaskType.Synchronous)
                    {
                        TcpServer.ReturnValue<RemoteExpression.ServerNodeIdChecker.Output> outputParameter = new RemoteExpression.ServerNodeIdChecker.Output { Return = RemoteExpression.Node.Get(inputParameter.Types) };
                        Push<RemoteExpression.ServerNodeIdChecker.Output>(ServerSocket.CommandIndex, IsBuildOutputThread ? RemoteExpression.ServerNodeIdChecker.Output.OutputThreadInfo : RemoteExpression.ServerNodeIdChecker.Output.OutputInfo, ref outputParameter);
                    }
                    else (AutoCSer.Threading.RingPool<GetRemoteExpressionNodeIdServerCall>.Default.Pop() ?? new GetRemoteExpressionNodeIdServerCall()).Set(this, Server.Attribute.RemoteExpressionServerTask, inputParameter.Types);
                    return;
                }
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
            }
            catch (Exception error)
            {
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                AddLog(error);
            }
            Push(returnType);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="data"></param>
        internal void GetRemoteExpression(ref SubArray<byte> data)
        {
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
            try
            {
                RemoteExpression.ClientNode inputParameter = default(RemoteExpression.ClientNode);
                if (DeSerialize(ref data, ref inputParameter, false))
                {
                    if (Server.Attribute.RemoteExpressionServerTask == TcpServer.ServerTaskType.Synchronous)
                    {
                        TcpServer.ReturnValue<RemoteExpression.ReturnValue.Output> outputParameter = new RemoteExpression.ReturnValue.Output { Return = inputParameter.GetReturnValue() };
                        Push<RemoteExpression.ReturnValue.Output>(ServerSocket.CommandIndex, IsServerBuildOutputThread ? RemoteExpression.ReturnValue.Output.OutputThreadInfo : RemoteExpression.ReturnValue.Output.OutputInfo, ref outputParameter);
                    }
                    else (AutoCSer.Threading.RingPool<GetRemoteExpressionServerCall>.Default.Pop() ?? new GetRemoteExpressionServerCall()).Set(this, Server.Attribute.RemoteExpressionServerTask, ref inputParameter);
                    return;
                }
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
            }
            catch (Exception error)
            {
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                AddLog(error);
            }
            Push(returnType);
        }
    }
}
