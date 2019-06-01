using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// TCP 服务套接字数据发送
    /// </summary>
    public abstract partial class ServerSocketSender : TcpServer.ServerSocketSenderBase
    {
        /// <summary>
        /// TCP 服务端套接字输出信息链表
        /// </summary>
        internal ServerOutput.OutputLink.YieldQueue Outputs = new ServerOutput.OutputLink.YieldQueue(new ServerOutput.ReturnTypeOutput());
        /// <summary>
        /// 服务端任务类型
        /// </summary>
        public readonly ServerTaskType ServerTaskType;
#if !NOJIT
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        internal ServerSocketSender() : base() { }
#endif
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务套接字</param>
        /// <param name="attribute"></param>
        internal ServerSocketSender(TcpServer.ServerSocket socket, ServerAttribute attribute)
            : base(socket, attribute.GetIsServerBuildOutputThread, attribute.GetServerOutputSleep)
        {
            ServerTaskType = attribute.ServerTaskType;
        }
    }
    /// <summary>
    /// TCP 服务套接字数据发送
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="serverType">TCP 服务类型</typeparam>
    /// <typeparam name="socketType">TCP 服务端套接字类型</typeparam>
    /// <typeparam name="socketSenderType">TCP 服务套接字数据发送类型</typeparam>
    public abstract class ServerSocketSender<attributeType, serverType, socketType, socketSenderType> : ServerSocketSender
        where attributeType : ServerAttribute
        where serverType : Server<attributeType, serverType, socketSenderType>
        where socketType : ServerSocket<attributeType, serverType, socketType, socketSenderType>
        where socketSenderType : ServerSocketSender<attributeType, serverType, socketType, socketSenderType>
    {
        /// <summary>
        /// TCP 服务
        /// </summary>
        internal readonly serverType Server;
#if !NOJIT
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        internal ServerSocketSender() : base() { }
#endif
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务套接字</param>
        internal ServerSocketSender(ServerSocket<attributeType, serverType, socketType, socketSenderType> socket)
            : base(socket, socket.Server.Attribute)
        {
            Server = socket.Server;
        }
        /// <summary>
        /// 释放接收数据缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void freeBuffer()
        {
            Buffer.Free();
            freeCopyBuffer();
            freeOutput();
        }
        /// <summary>
        /// 释放输出信息
        /// </summary>
        protected void freeOutput()
        {
            freeSerializer();
            //BinarySerialize.Serializer serializer = Interlocked.Exchange(ref OutputSerializer, null);
            //if (serializer != null) serializer.Free();
            //Json.Serializer jsonSerializer = Interlocked.Exchange(ref OutputJsonSerializer, null);
            //if (jsonSerializer != null) jsonSerializer.Free();
            ServerOutput.OutputLink head = Outputs.GetClear();
            //IsOutput = 0;
            Interlocked.Exchange(ref IsOutput, 0);
            ServerOutput.OutputLink.CancelLink(head);
        }
        /// <summary>
        /// 设置发送数据
        /// </summary>
        /// <param name="start">数据起始位置</param>
        /// <returns>是否改变输出缓冲区</returns>
        protected unsafe byte setSendData(byte* start)
        {
            UnmanagedStream outputStream = OutputSerializer.Stream;
            int outputLength = outputStream.ByteSize, bufferLength = Buffer.Length, newBufferCount = 0;
            byte isNewBuffer = 0;
            if (outputLength <= bufferLength)
            {
                if (outputStream.Data.ByteSize != bufferLength)
                {
                    newBufferCount = 1;
                    Memory.CopyNotNull(outputStream.Data.Byte, start, outputLength);
                }
                sendData.Set(Buffer.Buffer, Buffer.StartIndex, outputLength);
            }
            else
            {
                outputStream.GetSubBuffer(ref CopyBuffer);
                newBufferCount = CopyBuffer.PoolBuffer.Pool == null ? 2 : 1;
                sendData.Set(CopyBuffer.Buffer, CopyBuffer.StartIndex, outputLength);
                if (CopyBuffer.Length <= Server.SendBufferMaxSize)
                {
                    Buffer.Free();
                    CopyBuffer.CopyToClear(ref Buffer);
                    isNewBuffer = 1;
                }
            }
            if (outputLength >= Server.MinCompressSize)
            {
                if (AutoCSer.IO.Compression.DeflateCompressor.Get(sendData.Array, sendData.Start, outputLength, ref CompressBuffer, ref sendData, sizeof(int) * 2, sizeof(int) * 2))
                {
                    int compressionDataSize = sendData.Length;
                    sendData.MoveStart(-(sizeof(int) * 2));
                    fixed (byte* dataFixed = sendData.Array)
                    {
                        byte* dataStart = dataFixed + sendData.Start;
                        *(int*)dataStart = *(int*)-compressionDataSize;
                        *(int*)(dataStart + sizeof(int)) = outputLength;
                    }
                }
                if (CompressBuffer.Buffer != null && CompressBuffer.PoolBuffer.Pool == null) ++newBufferCount;
            }
            if (newBufferCount != 0) Interlocked.Add(ref Server.SendNewBufferCount, newBufferCount);
            return isNewBuffer;
        }
        /// <summary>
        /// 错误日志处理
        /// </summary>
        /// <param name="error"></param>
        internal override void VirtualAddLog(Exception error)
        {
            AddLog(error);
        }
        /// <summary>
        /// 错误日志处理
        /// </summary>
        /// <param name="error"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void AddLog(Exception error)
        {
            Server.Log.Add(AutoCSer.Log.LogType.Error, error);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push()
        {
            return Push(TcpServer.ReturnType.Success);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="value">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push(TcpServer.ReturnType value)
        {
            if (IsSocket)
            {
                ServerOutput.ReturnTypeOutput output = AutoCSer.Threading.RingPool<ServerOutput.ReturnTypeOutput>.Default.Pop() ?? new ServerOutput.ReturnTypeOutput();
                output.ReturnType = value;
                push(output);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push<outputParameterType>(TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ServerOutput.Output<outputParameterType> output = AutoCSer.Threading.RingPool<ServerOutput.Output<outputParameterType>>.Default.Pop() ?? new ServerOutput.Output<outputParameterType>();
                output.Set(outputInfo, ref outputParameter);
                push(output);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push<outputParameterType>(TcpServer.OutputInfo outputInfo, ref TcpServer.ReturnValue<outputParameterType> outputParameter)
            where outputParameterType : struct
        {
            if (outputParameter.Type == TcpServer.ReturnType.Success)
            {
                if (IsSocket)
                {
                    ServerOutput.Output<outputParameterType> output = AutoCSer.Threading.RingPool<ServerOutput.Output<outputParameterType>>.Default.Pop() ?? new ServerOutput.Output<outputParameterType>();
                    output.Set(outputInfo, ref outputParameter.Value);
                    push(output);
                    return true;
                }
                return false;
            }
            return Push(outputParameter.Type);
        }
        /// <summary>
        /// 添加输出信息
        /// </summary>
        /// <param name="output">当前输出信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void push(ServerOutput.OutputLink output)
        {
            //Outputs.Push(output);
            //TryBuildOutput();
            if (Outputs.IsPushHead(output)) TryBuildOutput();
        }
        /// <summary>
        /// 添加输出信息
        /// </summary>
        /// <param name="output">当前输出信息</param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void push(ServerOutput.OutputLink output, bool isBuildOutputThread)
        {
            //Outputs.Push(output);
            //TryBuildOutput();
            if (Outputs.IsPushHead(output)) TryBuildOutput(isBuildOutputThread);
        }
        /// <summary>
        /// 获取远程表达式服务端节点标识
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isBuildOutputThread"></param>
        internal void GetRemoteExpressionNodeId(ref SubArray<byte> data, bool isBuildOutputThread)
        {
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
            try
            {
                RemoteExpression.ServerNodeIdChecker.Input inputParameter = default(RemoteExpression.ServerNodeIdChecker.Input);
                if (DeSerialize(ref data, ref inputParameter, false))
                {
                    TcpServer.ReturnValue<RemoteExpression.ServerNodeIdChecker.Output> outputParameter = new RemoteExpression.ServerNodeIdChecker.Output { Return = RemoteExpression.Node.Get(inputParameter.Types) };
                    Push(isBuildOutputThread ? RemoteExpression.ServerNodeIdChecker.Output.OutputThreadInfo : RemoteExpression.ServerNodeIdChecker.Output.OutputInfo, ref outputParameter);
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
        /// <param name="isBuildOutputThread"></param>
        internal void GetRemoteExpression(ref SubArray<byte> data, bool isBuildOutputThread)
        {
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
            try
            {
                RemoteExpression.ClientNode inputParameter = default(RemoteExpression.ClientNode);
                if (DeSerialize(ref data, ref inputParameter, false))
                {
                    TcpServer.ReturnValue<RemoteExpression.ReturnValue.Output> outputParameter = new RemoteExpression.ReturnValue.Output { Return = inputParameter.GetReturnValue() };
                    Push(isBuildOutputThread ? RemoteExpression.ReturnValue.Output.OutputThreadInfo : RemoteExpression.ReturnValue.Output.OutputInfo, ref outputParameter);
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
        /// 尝试启动创建输出线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryBuildOutput()
        {
            if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0)
            {
                if (IsBuildOutputThread) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, Threading.Thread.CallType.TcpServerSocketSenderVirtualBuildOutput);
                else VirtualBuildOutput();
            }
        }
        /// <summary>
        /// 尝试启动创建输出线程
        /// </summary>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryBuildOutput(bool isBuildOutputThread)
        {
            if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0)
            {
                if (isBuildOutputThread & IsBuildOutputThread) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, Threading.Thread.CallType.TcpServerSocketSenderVirtualBuildOutput);
                else VirtualBuildOutput();
            }
        }
    }
}
