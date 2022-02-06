using System;
using AutoCSer.Extensions;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 内部服务套接字数据发送
    /// </summary>
    public sealed class ServerSocketSender : TcpServer.ServerSocketSender<Server, ServerSocket, ServerSocketSender>
    {
#if DOTNET2
        /// <summary>
        /// 发送数据异步回调
        /// </summary>
        private AsyncCallback onSendAsyncCallback;
#else
        /// <summary>
        /// 发送数据异步回调
        /// </summary>
        private EventHandler<SocketAsyncEventArgs> onSendAsyncCallback;
        /// <summary>
        /// 发送数据异步事件
        /// </summary>
        private SocketAsyncEventArgs sendAsyncEventArgs;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        private AutoCSer.Threading.SpinLock sendAsyncLock;
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
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Func<TcpServer.ReturnValue, bool> GetCallbackEmit(TcpServer.OutputInfo outputInfo)
        {
            if (outputInfo.IsKeepCallback == 0) return new ServerCallback(this, outputInfo.IsBuildOutputThread).Callback;
            return new ServerCallbackKeep(this, outputInfo.IsBuildOutputThread).Callback;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Func<TcpServer.ReturnValue<returnType>, bool> GetCallbackEmit<outputParameterType, returnType>(TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct, IReturnParameter<returnType>
        {
            if (outputInfo.IsKeepCallback == 0) return new ServerCallback<outputParameterType, returnType>(this, outputInfo, ref outputParameter).Callback;
            return new ServerCallbackKeep<outputParameterType, returnType>(this, outputInfo, ref outputParameter).Callback;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Func<returnType, bool> GetCallbackReturn<outputParameterType, returnType>(TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct, IReturnParameter<returnType>
        {
            if (outputInfo.IsKeepCallback == 0) return new ServerCallback<outputParameterType, returnType>(this, outputInfo, ref outputParameter).CallbackReturn;
            return new ServerCallbackKeep<outputParameterType, returnType>(this, outputInfo, ref outputParameter).CallbackReturn;
        }
#endif
        /// <summary>
        /// TCP 开放服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 开放服务套接字</param>
        internal ServerSocketSender(ServerSocket socket)
            : base(socket, Threading.ThreadTaskType.TcpOpenServerSocketSenderBuildOutput)
        {
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ServerCallback GetCallback(TcpServer.OutputInfo outputInfo)
        {
            if (outputInfo.IsKeepCallback == 0) return new ServerCallback(this, outputInfo.IsBuildOutputThread);
            return new ServerCallbackKeep(this, outputInfo.IsBuildOutputThread);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ServerCallback<returnType> GetCallback<outputParameterType, returnType>(TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
#if NOJIT
            where outputParameterType : struct, IReturnParameter
#else
            where outputParameterType : struct, IReturnParameter<returnType>
#endif
        {
            if (outputInfo.IsKeepCallback == 0) return new ServerCallback<outputParameterType, returnType>(this, outputInfo, ref outputParameter);
            return new ServerCallbackKeep<outputParameterType, returnType>(this, outputInfo, ref outputParameter);
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
                        sendAsyncEventArgs.Completed -= onSendAsyncCallback;
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
                        sendAsyncEventArgs.Completed -= onSendAsyncCallback;
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
                TcpServer.SenderBuildInfo buildInfo = new TcpServer.SenderBuildInfo { SendBufferSize = Server.SendBufferPool.Size };
                UnmanagedStream outputStream;
                try
                {
                    if (Buffer.Buffer == null) Server.SendBufferPool.Get(ref Buffer);
                    if (OutputSerializer == null) outputStream = (OutputSerializer = BinarySerializer.YieldPool.Default.Pop() ?? new BinarySerializer()).SetTcpServer();
                    else outputStream = OutputSerializer.Stream;
                    TcpServer.OutputWaitType outputWaitType = Server.ServerAttribute.OutputWaitType, currentOutputWaitType;
                    do
                    {
                        buildInfo.IsNewBuffer = 0;
                        fixed (byte* dataFixed = Buffer.GetFixedBuffer())
                        {
                            byte* start = dataFixed + Buffer.StartIndex;
                        STREAM:
                            using (outputStream)
                            {
                                currentOutputWaitType = outputWaitType;
                                if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                buildInfo.Clear();
                                outputStream.Data.CurrentIndex = TcpServer.ServerOutput.OutputLink.StreamStartIndex;
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
                                switch (currentOutputWaitType)
                                {
                                    case TcpServer.OutputWaitType.ThreadYield:
                                        AutoCSer.Threading.ThreadYield.YieldOnly();
                                        if (!Outputs.IsEmpty)
                                        {
                                            head = Outputs.GetClear(out end);
                                            currentOutputWaitType = TcpServer.OutputWaitType.DontWait;
                                            goto LOOP;
                                        }
                                        break;
                                    case TcpServer.OutputWaitType.ThreadSleep:
                                        System.Threading.Thread.Sleep(0);
                                        if (!Outputs.IsEmpty)
                                        {
                                            head = Outputs.GetClear(out end);
                                            currentOutputWaitType = TcpServer.OutputWaitType.DontWait;
                                            goto LOOP;
                                        }
                                        break;
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
                    Server.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
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
                if (onSendAsyncCallback == null) onSendAsyncCallback = onSend;
#if DOTNET2
                SocketError socketError;
                Socket.BeginSend(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError, onSendAsyncCallback, Socket);
                if (socketError == SocketError.Success) return SendState.Asynchronous;
#else
                if (sendAsyncEventArgs == null)
                {
                    sendAsyncEventArgs = SocketAsyncEventArgsPool.Get();
                    sendAsyncEventArgs.Completed += onSendAsyncCallback;
                }
#if !DotNetStandard
                sendAsyncLock.EnterYield();
#endif
                sendAsyncEventArgs.SetBuffer(sendData.Array, sendData.Start, sendData.Length);
                if (Socket.SendAsync(sendAsyncEventArgs))
                {
#if !DotNetStandard
                    sendAsyncLock.Exit();
#endif
                    return SendState.Asynchronous;
                }
                if (sendAsyncEventArgs.SocketError == SocketError.Success)
                {
                    sendData.MoveStart(sendAsyncEventArgs.BytesTransferred);
#if !DotNetStandard
                    sendAsyncLock.Exit();
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
                Socket socket = (Socket)async.AsyncState;
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
                Server.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
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
                    if (Server.ServerAttribute.RemoteExpressionTask == TcpServer.ServerTaskType.Synchronous)
                    {
                        TcpServer.ReturnValue<RemoteExpression.ServerNodeIdChecker.Output> outputParameter = new RemoteExpression.ServerNodeIdChecker.Output { Return = RemoteExpression.Node.Get(inputParameter.Types) };
                        Push<RemoteExpression.ServerNodeIdChecker.Output>(ServerSocket.CommandIndex, IsBuildOutputThread ? RemoteExpression.ServerNodeIdChecker.Output.OutputThreadInfo : RemoteExpression.ServerNodeIdChecker.Output.OutputInfo, ref outputParameter);
                    }
                    else (AutoCSer.Threading.RingPool<GetRemoteExpressionNodeIdServerCall>.Default.Pop() ?? new GetRemoteExpressionNodeIdServerCall()).Set(this, ref Server.ServerAttribute, inputParameter.Types);
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
                    if (Server.ServerAttribute.RemoteExpressionTask == TcpServer.ServerTaskType.Synchronous)
                    {
                        TcpServer.ReturnValue<RemoteExpression.ReturnValue.Output> outputParameter = new RemoteExpression.ReturnValue.Output { Return = inputParameter.GetReturnValue() };
                        Push<RemoteExpression.ReturnValue.Output>(ServerSocket.CommandIndex, IsBuildOutputThread ? RemoteExpression.ReturnValue.Output.OutputThreadInfo : RemoteExpression.ReturnValue.Output.OutputInfo, ref outputParameter);
                    }
                    else (AutoCSer.Threading.RingPool<GetRemoteExpressionServerCall>.Default.Pop() ?? new GetRemoteExpressionServerCall()).Set(this, ref Server.ServerAttribute, ref inputParameter);
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
