using System;
using AutoCSer.Extension;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务套接字数据发送
    /// </summary>
    public abstract partial class ServerSocketSender : ServerSocketSenderBase
    {
        /// <summary>
        /// 创建输出线程调用类型
        /// </summary>
        internal readonly AutoCSer.Threading.Thread.CallType BuildOutputThreadCallType;
        /// <summary>
        /// TCP 服务器端异步保持调用集合
        /// </summary>
        internal Dictionary<int, IServerKeepCallback> KeepCallbacks;

        /// <summary>
        /// TCP 服务端套接字输出信息链表
        /// </summary>
        internal ServerOutput.OutputLink.YieldQueue Outputs = new ServerOutput.OutputLink.YieldQueue(new ServerOutput.ReturnTypeOutput());
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
        /// <param name="buildOutputThreadCallType">创建输出线程调用类型</param>
        /// <param name="isBuildOutputThread">创建输出是否开启线程</param>
        /// <param name="outputSleep">等待输出休眠时间</param>
        internal ServerSocketSender(ServerSocket socket, AutoCSer.Threading.Thread.CallType buildOutputThreadCallType, bool isBuildOutputThread, int outputSleep)
            : base(socket, isBuildOutputThread, outputSleep)
        {
            BuildOutputThreadCallType = buildOutputThreadCallType;
        }
        /// <summary>
        /// 添加 TCP 服务器端异步保持调用
        /// </summary>
        /// <param name="commandIndex"></param>
        /// <param name="keepCallback"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddKeepCallback(uint commandIndex, IServerKeepCallback keepCallback)
        {
            if (KeepCallbacks == null) KeepCallbacks = DictionaryCreator.CreateInt<IServerKeepCallback>();
            KeepCallbacks[(int)commandIndex] = keepCallback;
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
        /// <summary>
        /// 服务端创建输出是否开启线程
        /// </summary>
        internal readonly bool IsServerBuildOutputThread;
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
        /// <param name="buildOutputThreadCallType">创建输出线程调用类型</param>
        internal ServerSocketSender(ServerSocket<attributeType, serverType, socketType, socketSenderType> socket, AutoCSer.Threading.Thread.CallType buildOutputThreadCallType)
            : base(socket, buildOutputThreadCallType, socket.Server.Attribute.GetIsServerBuildOutputThread, socket.Server.Attribute.GetServerOutputSleep)
        {
            Server = socket.Server;
            IsServerBuildOutputThread = Server.Attribute.GetIsServerBuildOutputThread;
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
        /// <param name="count">输出数量</param>
        /// <returns>是否改变输出缓冲区</returns>
        protected unsafe byte setSendData(byte* start, int count)
        {
            UnmanagedStream outputStream = OutputSerializer.Stream;
            int outputLength = outputStream.ByteSize, bufferLength = Buffer.Length, dataLength = outputLength - ServerOutput.OutputLink.StreamStartIndex, newBufferCount = 0, compressionDataSize = 0;
            byte isNewBuffer = 0;
            if (outputLength <= bufferLength)
            {
                if (outputStream.Data.ByteSize != bufferLength)
                {
                    newBufferCount = 1;
                    Memory.CopyNotNull(outputStream.Data.Byte + ServerOutput.OutputLink.StreamStartIndex, start + ServerOutput.OutputLink.StreamStartIndex, dataLength);
                }
                sendData.Set(Buffer.Buffer, Buffer.StartIndex + ServerOutput.OutputLink.StreamStartIndex, dataLength);
            }
            else
            {
                outputStream.GetSubBuffer(ref CopyBuffer, ServerOutput.OutputLink.StreamStartIndex);
                newBufferCount = CopyBuffer.PoolBuffer.Pool == null ? 2 : 1;
                sendData.Set(CopyBuffer.Buffer, CopyBuffer.StartIndex + ServerOutput.OutputLink.StreamStartIndex, dataLength);
                if (CopyBuffer.Length <= Server.SendBufferMaxSize)
                {
                    Buffer.Free();
                    CopyBuffer.CopyToClear(ref Buffer);
                    isNewBuffer = 1;
                }
            }
            if (count == 1)
            {
                if ((dataLength -= ServerOutput.OutputLink.StreamStartIndex) >= Server.MinCompressSize)
                {
                    SubArray<byte> oldSendData = sendData;
                    if (AutoCSer.IO.Compression.DeflateCompressor.Get(sendData.Array, sendData.Start + (sizeof(uint) + sizeof(int)), dataLength, ref CompressBuffer, ref sendData, sizeof(uint) + sizeof(int) * 2, sizeof(uint) + sizeof(int) * 2))
                    {
                        compressionDataSize = sendData.Length;
                        sendData.MoveStart(-(sizeof(uint) + sizeof(int) * 2));
                        fixed (byte* dataFixed = sendData.Array, oldSendDataFixed = oldSendData.Array)
                        {
                            byte* dataStart = dataFixed + sendData.Start;
                            *(uint*)dataStart = *(uint*)(oldSendDataFixed + oldSendData.Start);
                            *(int*)(dataStart + sizeof(uint)) = -compressionDataSize;
                            *(int*)(dataStart + (sizeof(uint) + sizeof(int))) = dataLength;
                            if (ServerSocket.MarkData != 0)
                            {
                                CommandBuffer.Mark32(dataStart + (sizeof(uint) + sizeof(int) * 2), ServerSocket.MarkData, (compressionDataSize + 3) & (int.MaxValue - 3));
                            }
                        }
                    }
                    if (CompressBuffer.Buffer != null && CompressBuffer.PoolBuffer.Pool == null) ++newBufferCount;
                }
                if (compressionDataSize == 0 && dataLength >= sizeof(uint) && ServerSocket.MarkData != 0)
                {
                    fixed (byte* dataFixed = sendData.Array)
                    {
                        CommandBuffer.Mark64(dataFixed + (sendData.Start + ServerOutput.OutputLink.StreamStartIndex), ServerSocket.MarkData, (sendData.Length - (ServerOutput.OutputLink.StreamStartIndex - 3)) & (int.MaxValue - 3));
                    }
                }
            }
            else
            {
                if (dataLength >= Server.MinCompressSize)
                {
                    if (AutoCSer.IO.Compression.DeflateCompressor.Get(sendData.Array, sendData.Start, dataLength, ref CompressBuffer, ref sendData, sizeof(uint) + sizeof(int) * 2, sizeof(int)))
                    {
                        compressionDataSize = sendData.Length;
                        sendData.MoveStart(-(sizeof(uint) + sizeof(int) * 2));
                        fixed (byte* dataFixed = sendData.Array)
                        {
                            byte* dataStart = dataFixed + sendData.Start;
                            *(uint*)dataStart = ClientCommand.KeepCommand.MergeIndex;
                            *(int*)(dataStart + sizeof(uint)) = -compressionDataSize;
                            *(int*)(dataStart + (sizeof(uint) + sizeof(int))) = dataLength;
                            if (ServerSocket.MarkData != 0)
                            {
                                CommandBuffer.Mark32(dataStart + (sizeof(uint) + sizeof(int) * 2), ServerSocket.MarkData, (compressionDataSize + 3) & (int.MaxValue - 3));
                            }
                        }
                    }
                    if (CompressBuffer.Buffer != null && CompressBuffer.PoolBuffer.Pool == null) ++newBufferCount;
                }
                if (compressionDataSize == 0)
                {
                    sendData.MoveStart(-ServerOutput.OutputLink.StreamStartIndex);
                    fixed (byte* dataFixed = sendData.Array)
                    {
                        byte* dataStart = dataFixed + sendData.Start;
                        *(uint*)dataStart = ClientCommand.KeepCommand.MergeIndex;
                        *(int*)(dataStart + sizeof(uint)) = dataLength;
                        if (ServerSocket.MarkData != 0)
                        {
                            CommandBuffer.Mark64(dataStart + ServerOutput.OutputLink.StreamStartIndex, ServerSocket.MarkData, (dataLength + 3) & (int.MaxValue - 3));
                        }
                    }
                }
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
        /// 获取输出信息
        /// </summary>
        /// <param name="commandIndex">会话索引</param>
        /// <returns>输出信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerOutput.ReturnTypeOutput TryGetOutput(uint commandIndex)
        {
            ServerOutput.ReturnTypeOutput output = AutoCSer.Threading.RingPool<ServerOutput.ReturnTypeOutput>.Default.Pop();
            if (output == null)
            {
                try
                {
                    output = new ServerOutput.ReturnTypeOutput();
                }
                catch (Exception error)
                {
                    Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
                    return null;
                }
            }
            output.CommandIndex = commandIndex;
            return output;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="commandIndex">会话索引</param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TryPush(uint commandIndex, bool isBuildOutputThread)
        {
            if (IsSocket)
            {
                ServerOutput.ReturnTypeOutput output = TryGetOutput(commandIndex);
                if (output != null)
                {
                    push(output, isBuildOutputThread);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="commandIndex">会话索引</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Push(uint commandIndex)
        {
            if (IsSocket)
            {
                ServerOutput.ReturnTypeOutput output = AutoCSer.Threading.RingPool<ServerOutput.ReturnTypeOutput>.Default.Pop() ?? new ServerOutput.ReturnTypeOutput();
                output.CommandIndex = commandIndex;
                push(output);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="commandIndex">会话索引</param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Push(uint commandIndex, bool isBuildOutputThread)
        {
            if (IsSocket)
            {
                ServerOutput.ReturnTypeOutput output = AutoCSer.Threading.RingPool<ServerOutput.ReturnTypeOutput>.Default.Pop() ?? new ServerOutput.ReturnTypeOutput();
                output.CommandIndex = commandIndex;
                push(output, isBuildOutputThread);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="value">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push(ReturnType value)
        {
            return Push(TcpServer.Server.GetCommandIndex(ServerSocket.CommandIndex, value));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push()
        {
            return Push(TcpServer.Server.GetCommandIndex(ServerSocket.CommandIndex, ReturnType.Success));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="commandIndex">会话标识</param>
        /// <param name="returnType">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push(uint commandIndex, ReturnType returnType)
        {
            return Push(TcpServer.Server.GetCommandIndex(commandIndex, returnType));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="commandIndex">会话标识</param>
        /// <param name="returnType">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool PushNoThread(uint commandIndex, ReturnType returnType)
        {
            if (IsSocket)
            {
                ServerOutput.ReturnTypeOutput output = AutoCSer.Threading.RingPool<ServerOutput.ReturnTypeOutput>.Default.Pop() ?? new ServerOutput.ReturnTypeOutput();
                output.CommandIndex = TcpServer.Server.GetCommandIndex(commandIndex, returnType);
                if (Outputs.IsPushHead(output) && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) new AutoCSer.Threading.Thread.CallInfo { Value = this, Type = BuildOutputThreadCallType }.Call();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取输出信息
        /// </summary>
        /// <typeparam name="outputParameterType"></typeparam>
        /// <param name="commandIndex">会话标识</param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>输出信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerOutput.Output<outputParameterType> TryGetOutput<outputParameterType>(uint commandIndex, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            ServerOutput.Output<outputParameterType> output = AutoCSer.Threading.RingPool<ServerOutput.Output<outputParameterType>>.Default.Pop();
            if (output == null)
            {
                try
                {
                    output = new ServerOutput.Output<outputParameterType>();
                }
                catch (Exception error)
                {
                    Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
                    return null;
                }
            }
            output.Set(commandIndex, outputInfo, ref outputParameter);
            return output;
        }
        /// <summary>
        /// 获取输出信息
        /// </summary>
        /// <typeparam name="outputParameterType"></typeparam>
        /// <param name="commandIndex">会话标识</param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>输出信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerOutput.Output<outputParameterType> GetOutput<outputParameterType>(uint commandIndex, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            ServerOutput.Output<outputParameterType> output = AutoCSer.Threading.RingPool<ServerOutput.Output<outputParameterType>>.Default.Pop() ?? new ServerOutput.Output<outputParameterType>();
            output.Set(commandIndex, outputInfo, ref outputParameter);
            return output;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="commandIndex">会话标识</param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        internal bool TryPush<outputParameterType>(uint commandIndex, TcpServer.OutputInfo outputInfo, ref ReturnValue<outputParameterType> outputParameter)
            where outputParameterType : struct
        {
            if (outputParameter.Type == ReturnType.Success)
            {
                if (IsSocket)
                {
                    ServerOutput.Output<outputParameterType> output = TryGetOutput<outputParameterType>(commandIndex, outputInfo, ref outputParameter.Value);
                    if (output != null)
                    {
                        push(output, outputInfo.IsBuildOutputThread);
                        return true;
                    }
                }
                return false;
            }
            return TryPush(TcpServer.Server.GetCommandIndex(commandIndex, outputParameter.Type), outputInfo.IsBuildOutputThread);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="commandIndex">会话标识</param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Push<outputParameterType>(uint commandIndex, TcpServer.OutputInfo outputInfo, ref ReturnValue<outputParameterType> outputParameter)
            where outputParameterType : struct
        {
            if (outputParameter.Type == ReturnType.Success)
            {
                if (IsSocket)
                {
                    push(GetOutput<outputParameterType>(commandIndex, outputInfo, ref outputParameter.Value), outputInfo.IsBuildOutputThread);
                    return true;
                }
                return false;
            }
            return Push(TcpServer.Server.GetCommandIndex(commandIndex, outputParameter.Type), outputInfo.IsBuildOutputThread);
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
                push(GetOutput<outputParameterType>(ServerSocket.CommandIndex, outputInfo, ref outputParameter));
                return true;
            }
            return false;
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
        /// 尝试启动创建输出线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryBuildOutput()
        {
            if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0)
            {
                if (IsBuildOutputThread) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, BuildOutputThreadCallType);
                else new AutoCSer.Threading.Thread.CallInfo { Value = this, Type = BuildOutputThreadCallType }.Call();
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
                if (isBuildOutputThread & IsBuildOutputThread) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, BuildOutputThreadCallType);
                else new AutoCSer.Threading.Thread.CallInfo { Value = this, Type = BuildOutputThreadCallType }.Call();
            }
        }

        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CustomData(byte[] data)
        {
            if (IsSocket)
            {
                ServerOutput.CustomDataOutput output = AutoCSer.Threading.RingPool<ServerOutput.CustomDataOutput>.Default.Pop() ?? new ServerOutput.CustomDataOutput();
                if (data == null) output.Data.Set(0, 0);
                else output.Data.Set(data, 0, data.Length);
                push(output);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CustomData(ref SubArray<byte> data)
        {
            if (IsSocket)
            {
                ServerOutput.CustomDataOutput output = AutoCSer.Threading.RingPool<ServerOutput.CustomDataOutput>.Default.Pop() ?? new ServerOutput.CustomDataOutput();
                output.Data = data;
                push(output);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除 TCP 服务器端异步保持调用
        /// </summary>
        /// <param name="commandIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelKeepCallback(int commandIndex)
        {
            IServerKeepCallback keepCallback;
            if (KeepCallbacks != null && KeepCallbacks.TryGetValue(commandIndex, out keepCallback))
            {
                KeepCallbacks.Remove(commandIndex);
                keepCallback.CancelKeep();
            }
        }
    }
}
