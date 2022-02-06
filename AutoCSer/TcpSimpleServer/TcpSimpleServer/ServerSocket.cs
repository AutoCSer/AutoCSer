using System;
using System.Net.Sockets;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务端套接字
    /// </summary>
    public abstract unsafe class ServerSocket : SocketTimeoutNode
    {
        /// <summary>
        /// 二进制反序列化配置参数
        /// </summary>
        private readonly AutoCSer.BinarySerialize.DeSerializeConfig binaryDeSerializeConfig;
        /// <summary>
        /// 命令位图
        /// </summary>
        protected AutoCSer.Memory.Pointer commandData;
        /// <summary>
        /// 命令位图
        /// </summary>
        protected MemoryMap commands;
#if DOTNET2
        /// <summary>
        /// 套接字异步回调
        /// </summary>
        protected AsyncCallback asyncCallback;
        /// <summary>
        /// 套接字异步事件对象
        /// </summary>
        protected IAsyncResult asyncEventArgs;
#else
        /// <summary>
        /// 套接字异步回调
        /// </summary>
        protected EventHandler<SocketAsyncEventArgs> asyncCallback;
        /// <summary>
        /// 套接字异步事件对象
        /// </summary>
        protected SocketAsyncEventArgs asyncEventArgs;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        protected AutoCSer.Threading.SleepFlagSpinLock asyncLock;
#endif
#endif
        /// <summary>
        /// 变换数据
        /// </summary>
        internal ulong MarkData;
        /// <summary>
        /// 接收数据二进制反序列化
        /// </summary>
        internal AutoCSer.BinaryDeSerializer ReceiveDeSerializer;
        /// <summary>
        /// 接收数据 JSON 解析
        /// </summary>
        internal AutoCSer.JsonDeSerializer ReceiveJsonParser;
        /// <summary>
        /// 临时接收数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull ReceiveBigBuffer;

        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 接收数据缓冲区字节大小
        /// </summary>
        protected int bufferSize;

        /// <summary>
        /// 套接字最后接收数据时间
        /// </summary>
        internal DateTime LastReceiveTime;
        /// <summary>
        /// 接收数据超时
        /// </summary>
        protected DateTime receiveTimeout;
        /// <summary>
        /// 数据起始位置
        /// </summary>
        protected byte* bufferStart;
        /// <summary>
        /// 当前接收数据字节数
        /// </summary>
        protected int receiveCount;
        /// <summary>
        /// 当前处理接收数据字节数
        /// </summary>
        protected int receiveIndex;
        /// <summary>
        /// 当前数据压缩后的字节大小
        /// </summary>
        protected int compressionDataSize;
        /// <summary>
        /// 当前数据字节大小
        /// </summary>
        protected int dataSize;
        /// <summary>
        /// 当前命令
        /// </summary>
        protected int command;
        /// <summary>
        /// 当前命令
        /// </summary>
        protected int commandIndex
        {
            get { return (int)((uint)command & TcpServer.Server.CommandIndexAnd); }
        }
        /// <summary>
        /// 当前接收临时数据字节数
        /// </summary>
        protected int receiveBigBufferCount;
        /// <summary>
        /// 套接字错误
        /// </summary>
        protected SocketError socketError = SocketError.Success;
        /// <summary>
        /// TCP 服务端套接字回调类型
        /// </summary>
        internal ServerSocketType SocketType;
        /// <summary>
        /// 验证函数调用次数
        /// </summary>
        protected byte verifyMethodCount;
        /// <summary>
        /// 是否通过函数验证
        /// </summary>
        internal bool IsVerifyMethod;
        /// <summary>
        /// 客户端信息
        /// </summary>
        public object ClientObject;

        /// <summary>
        /// 输出数据流
        /// </summary>
        internal UnmanagedStream OutputStream;
        /// <summary>
        /// 输出数据 JSON 序列化
        /// </summary>
        internal AutoCSer.JsonSerializer OutputJsonSerializer;
        /// <summary>
        /// 输出数据二进制序列化
        /// </summary>
        internal BinarySerializer OutputSerializer;
        /// <summary>
        /// 输出信息
        /// </summary>
        internal ServerBuffer OutputBuffer;
        /// <summary>
        /// 序列化参数编号
        /// </summary>
        protected int serializeParameterIndex;
        /// <summary>
        /// 时间验证时钟周期
        /// </summary>
        internal long TimeVerifyTicks;

#if !NOJIT
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        internal ServerSocket() : base() { }
#endif
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        /// <param name="binaryDeSerializeConfig">二进制反序列化配置参数</param>
        /// <param name="verifyMethodCount">验证函数调用次数</param>
        internal ServerSocket(AutoCSer.BinarySerialize.DeSerializeConfig binaryDeSerializeConfig, byte verifyMethodCount)
        {
            this.binaryDeSerializeConfig = binaryDeSerializeConfig;
            this.verifyMethodCount = verifyMethodCount;
        }
        /// <summary>
        /// 判断命令是否有效
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe bool IsCommand(int index)
        {
            return commands.Map == null || (commands.Get(index) != 0 && commandData.Data != null);
        }
        /// <summary>
        /// 通过函数验证处理
        /// </summary>
        public virtual void SetVerifyMethod()
        {
            IsVerifyMethod = true;
        }
        /// <summary>
        /// 通过函数验证处理
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void SetVerifyMethod(ServerSocket socket)
        {
            socket.SetVerifyMethod();
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>是否成功</returns>
        public unsafe bool DeSerialize<valueType>(ref SubArray<byte> data, ref valueType value, bool isSimpleSerialize = false)
            where valueType : struct
        {
            if (((uint)command & (uint)TcpServer.CommandFlags.JsonSerialize) == 0)
            {
                if (isSimpleSerialize)
                {
                    fixed (byte* dataFixed = data.GetFixedBuffer())
                    {
                        byte* start = dataFixed + data.Start, end = start + data.Length;
                        return SimpleSerialize.TypeDeSerializer<valueType>.DeSerialize(start, ref value, end) == end;
                    }
                }
                if (ReceiveDeSerializer == null)
                {
                    ReceiveDeSerializer = AutoCSer.BinaryDeSerializer.YieldPool.Default.Pop() ?? new AutoCSer.BinaryDeSerializer();
                    ReceiveDeSerializer.SetTcpServer(binaryDeSerializeConfig, this);
                }
                return ReceiveDeSerializer.DeSerializeTcpServer(ref data, ref value);
                //if (ReceiveDeSerializer.DeSerializeTcpServer(ref data, ref value)) return true;
                //if (data.Length > 1 << 20) System.IO.File.WriteAllBytes((++testIdentity).ToString() + "." + data.Length.ToString(), data.ToArray());
                //return false;
            }
            if (ReceiveJsonParser == null)
            {
                ReceiveJsonParser = AutoCSer.JsonDeSerializer.YieldPool.Default.Pop() ?? new AutoCSer.JsonDeSerializer();
                ReceiveJsonParser.SetTcpServer();
            }
            return ReceiveJsonParser.DeSerializeTcpServer(ref data, ref value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="socket"></param>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool DeSerialize<valueType>(ServerSocket socket, ref SubArray<byte> data, ref valueType value, bool isSimpleSerialize)
            where valueType : struct
        {
            return socket.DeSerialize(ref data, ref value, isSimpleSerialize);
        }
        /// <summary>
        /// 释放数据序列化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeSerializer()
        {
            OutputStream = null;
            if (ReceiveDeSerializer != null)
            {
                ReceiveDeSerializer.Free();
                ReceiveDeSerializer = null;
            }
            if (ReceiveJsonParser != null)
            {
                ReceiveJsonParser.Free();
                ReceiveJsonParser = null;
            }
            if (OutputSerializer != null)
            {
                OutputSerializer.Free();
                OutputSerializer = null;
            }
            if (OutputJsonSerializer != null)
            {
                OutputJsonSerializer.Free();
                OutputJsonSerializer = null;
            }
        }
    }
    /// <summary>
    /// TCP 服务端套接字
    /// </summary>
    /// <typeparam name="serverType">TCP 服务类型</typeparam>
    /// <typeparam name="socketType">TCP 服务端套接字类型</typeparam>
    public abstract class ServerSocket<serverType, socketType> : ServerSocket
        where serverType : Server<serverType, socketType>
        where socketType : ServerSocket<serverType, socketType>
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long TaskTimestamp;
        /// <summary>
        /// 下一个任务
        /// </summary>
        internal socketType NextTask;
        /// <summary>
        /// TCP 服务
        /// </summary>
        internal readonly serverType Server;
#if !NOJIT
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        internal ServerSocket() : base() { }
#endif
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        /// <param name="server">TCP调用服务端</param>
        internal ServerSocket(serverType server): base(server.BinaryDeSerializeConfig, server.VerifyMethodCount)
        {
            Server = server;
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private unsafe bool setCommand(int command)
        {
            if ((uint)command <= (uint)Server.MaxCommand
                && (commandData.Data != null || (commands.Map == null && Server.CreateCommandData(ref commandData, ref commands))))
            {
                commands.Set(command);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetCommand(int methodIndex)
        {
            return setCommand(methodIndex + TcpServer.Server.CommandStartIndex);
        }
        /// <summary>
        /// 设置基础命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetBaseCommand(int command)
        {
            return command < TcpServer.Server.CommandStartIndex && setCommand(command);
        }
        /// <summary>
        /// 错误日志处理
        /// </summary>
        /// <param name="error"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Log(Exception error)
        {
            Server.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 错误日志处理
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="error"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Log(ServerSocket<serverType, socketType> socket, Exception error)
        {
            socket.Log(error);
        }
        /// <summary>
        /// 创建输出数据
        /// </summary>
        /// <typeparam name="outputParameterType"></typeparam>
        /// <param name="outputInfo"></param>
        /// <param name="outputParameter"></param>
        internal unsafe void BuildOutput<outputParameterType>(TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            int dataLength;
            fixed (byte* dataFixed = Buffer.GetFixedBuffer())
            {
                byte* start = dataFixed + Buffer.StartIndex;
                OutputStream.Reset(start, Buffer.Length);
                OutputStream.Data.CurrentIndex = sizeof(int);
                using (OutputStream)
                {
                    if (((uint)command & (uint)TcpServer.CommandFlags.JsonSerialize) == 0)
                    {
                        if (outputInfo.IsSimpleSerializeOutputParamter) SimpleSerialize.TypeSerializer<outputParameterType>.Serializer(OutputSerializer.Stream, ref outputParameter);
                        else
                        {
                            int parameterIndex = outputInfo.OutputParameterIndex;
                            if (serializeParameterIndex == parameterIndex) OutputSerializer.SerializeTcpServerNext(ref outputParameter);
                            else
                            {
                                OutputSerializer.SerializeTcpServer(ref outputParameter);
                                serializeParameterIndex = parameterIndex;
                            }
                        }
                    }
                    else
                    {
                        if (OutputJsonSerializer == null)
                        {
                            OutputJsonSerializer = AutoCSer.JsonSerializer.YieldPool.Default.Pop() ?? new AutoCSer.JsonSerializer();
                            OutputJsonSerializer.SetTcpServer();
                        }
                        OutputJsonSerializer.SerializeTcpServer(ref outputParameter, OutputSerializer.Stream);
                    }
                    byte* write = OutputStream.Data.Byte;
                    dataLength = OutputStream.Data.CurrentIndex - sizeof(int);
                    *(int*)write = dataLength;
                    if (OutputStream.Data.CurrentIndex <= Buffer.Length)
                    {
                        if (start != write) AutoCSer.Memory.Common.CopyNotNull(write, start, OutputStream.Data.CurrentIndex);
                        OutputBuffer.Data.Set(Buffer.Buffer, Buffer.StartIndex, OutputStream.Data.CurrentIndex);
                    }
                    else
                    {
                        OutputStream.Data.GetSubBuffer(ref OutputBuffer.CopyBuffer);
                        OutputBuffer.SetSendDataCopyBuffer(OutputStream.Data.CurrentIndex);
                        if (OutputBuffer.CopyBuffer.Length <= Server.SendBufferMaxSize)
                        {
                            Buffer.Free();
                            OutputBuffer.CopyBuffer.CopyToClear(ref Buffer);
#if !DOTNET2
                            asyncEventArgs.SetBuffer(Buffer.Buffer, Buffer.StartIndex, bufferSize = Buffer.Length);
#endif
                        }
                    }
                }
            }
            if (dataLength < Server.MinCompressSize || !OutputBuffer.CompressSendData(dataLength, MarkData))
            {
                if (MarkData != 0) OutputBuffer.MarkSendData(dataLength, MarkData);
            }
        }
    }
}
