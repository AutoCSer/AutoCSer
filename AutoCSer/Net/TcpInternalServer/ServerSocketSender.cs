﻿using System;
using System.Threading;
using System.Net.Sockets;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// TCP 内部服务套接字数据发送
    /// </summary>
    public sealed class ServerSocketSender : TcpServer.ServerSocketSender<Server, ServerSocket, ServerSocketSender>
    {
#if !NOJIT
        /// <summary>
        /// TCP 内部服务套接字数据发送
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
        /// TCP 内部服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字</param>
        internal ServerSocketSender(ServerSocket socket)
            : base(socket, Threading.ThreadTaskType.TcpInternalServerSocketSenderBuildOutput)
        {
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
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
        //[AutoCSer.IOS.Preserve(Conditional = true)]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void closeSocket()
        {
            close();
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
                    ServerSocket.DisposeSocket();
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
                SocketError socketError;
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
                            RESET:
                                currentOutputWaitType = outputWaitType;
                                if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                buildInfo.Clear();
                                outputStream.Data.CurrentIndex = TcpServer.ServerOutput.OutputLink.StreamStartIndex;
                                if ((head = Outputs.GetClear(out end)) == null) goto CHECK;
                            LOOP:
                                do
                                {
                                    head = head.Build(this, ref buildInfo);
                                    if (buildInfo.IsSend != 0 || !ServerSocket.IsVerifyMethod) goto SETDATA;
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
                                if (buildInfo.Count == 0) goto DISPOSE;
                            SETDATA:
                                buildInfo.IsNewBuffer = setSendData(start, buildInfo.Count);
                            SEND:
                                if (IsSocket)
                                {
                                    int sendSize = Socket.Send(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError);
                                    sendData.MoveStart(sendSize);
                                    if (sendData.Length == 0)
                                    {
                                        buildInfo.SendSizeLessCount = 0;
                                        if (buildInfo.IsNewBuffer == 0)
                                        {
                                            freeCopyBuffer();
                                            if (head == null) goto RESET;
                                            if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                            buildInfo.Clear();
                                            outputStream.Data.CurrentIndex = TcpServer.ServerOutput.OutputLink.StreamStartIndex;
                                            currentOutputWaitType = outputWaitType;
                                            goto LOOP;
                                        }
                                        CompressBuffer.TryFree();
                                        if (head != null) Outputs.PushHead(ref head, end);
                                        //currentOutputSleep = outputSleep;
                                        goto DISPOSE;
                                    }
                                    if (socketError == SocketError.Success && (sendSize >= TcpServer.Server.MinSocketSize || (sendSize > 0 && buildInfo.SendSizeLessCount++ == 0))) goto SEND;
                                    buildInfo.IsError = true;
                                }
                                buildInfo.IsNewBuffer = 0;
                            DISPOSE: ;
                            }
                            if (buildInfo.IsNewBuffer == 0)
                            {
                                if (!buildInfo.IsError)
                                {
                                    if (IsSocket)
                                    {
                                        //IsOutput = 0;
                                        Interlocked.Exchange(ref IsOutput, 0);
                                        if (!Outputs.IsEmpty)
                                        {
                                            if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) goto STREAM;
                                        }
                                        else if (!IsSocket && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) buildInfo.IsClose = true;
                                    }
                                    else buildInfo.IsClose = true;
                                }
                                return;
                            }
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
                    else (GetRemoteExpressionNodeIdServerCall.Pop() ?? new GetRemoteExpressionNodeIdServerCall()).Set(this, Server.ServerAttribute.RemoteExpressionTask, ref inputParameter.Types);
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
                    else (GetRemoteExpressionServerCall.Pop() ?? new GetRemoteExpressionServerCall()).Set(this, Server.ServerAttribute.RemoteExpressionTask, ref inputParameter);
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
