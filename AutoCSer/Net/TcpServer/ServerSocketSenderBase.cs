using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务套接字数据发送
    /// </summary>
    public abstract partial class ServerSocketSenderBase : Sender
    {
        /// <summary>
        /// TCP 服务套接字
        /// </summary>
        internal readonly ServerSocket ServerSocket;
        /// <summary>
        /// 等待输出休眠时间
        /// </summary>
        internal readonly int OutputSleep;
        /// <summary>
        /// 创建输出是否开启线程
        /// </summary>
        internal readonly bool IsBuildOutputThread;
        /// <summary>
        /// 关闭事件
        /// </summary>
        public event Action OnClose;
        /// <summary>
        /// 关闭事件（TCP 服务器端同步调用任务处理）
        /// </summary>
        public event Action OnCloseTask;
        /// <summary>
        /// 输出数据 JSON 序列化
        /// </summary>
        internal Json.Serializer OutputJsonSerializer;
        /// <summary>
        /// 输出数据二进制序列化
        /// </summary>
        internal BinarySerialize.Serializer OutputSerializer;
        /// <summary>
        /// 输出数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 输出复制数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull CopyBuffer;
        /// <summary>
        /// 压缩数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull CompressBuffer;

        /// <summary>
        /// 客户端信息
        /// </summary>
        public object ClientObject;
        /// <summary>
        /// 发送数据
        /// </summary>
        protected SubArray<byte> sendData;
        /// <summary>
        /// 时间验证时钟周期
        /// </summary>
        public long TimeVerifyTicks;
        /// <summary>
        /// 序列化参数编号
        /// </summary>
        private int serializeParameterIndex;
        /// <summary>
        /// 是否正在输出
        /// </summary>
        internal int IsOutput;
        /// <summary>
        /// 套接字是否有效
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public bool IsSocket
        {
            get
            {
                return ServerSocket.Socket == Socket;
            }
        }
#if !NOJIT
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        internal ServerSocketSenderBase() : base(null) { }
#endif
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务套接字</param>
        /// <param name="isBuildOutputThread">创建输出是否开启线程</param>
        /// <param name="outputSleep">等待输出休眠时间</param>
        internal ServerSocketSenderBase(ServerSocket socket, bool isBuildOutputThread, int outputSleep)
            : base(socket.Socket)
        {
            this.ServerSocket = socket;
            IsBuildOutputThread = isBuildOutputThread;
            OutputSleep = outputSleep;
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetCommand(int methodIndex)
        {
            return ServerSocket.SetCommand(methodIndex);
        }
        /// <summary>
        /// 设置基础命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetBaseCommand(int command)
        {
            return ServerSocket.SetBaseCommand(command);
        }
        /// <summary>
        /// 调用关闭事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void callOnClose()
        {
            ServerCallTask.Task.Add(new ServerSocketSenderCloseTask { Sender = this });
            if (OnClose != null) OnClose();
        }
        /// <summary>
        /// 调用关闭事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnCloseTask()
        {
            if (OnCloseTask != null)
            {
                try
                {
                    OnCloseTask();
                }
                catch (Exception error)
                {
                    VirtualAddLog(error);
                }
            }
        }
        /// <summary>
        /// 错误日志处理
        /// </summary>
        /// <param name="error"></param>
        internal abstract void VirtualAddLog(Exception error);
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void freeCopyBuffer()
        {
            CopyBuffer.Free();
            CompressBuffer.Free();
        }
        /// <summary>
        /// 释放序列化器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void freeSerializer()
        {
            if (OutputSerializer != null)
            {
                OutputSerializer.Free();
                OutputSerializer = null;
                if (OutputJsonSerializer != null)
                {
                    OutputJsonSerializer.Free();
                    OutputJsonSerializer = null;
                }
            }
        }
        /// <summary>
        /// 设置变换数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal virtual bool SetMarkData(ulong value)
        {
            ServerSocket.MarkData = value;
            return true;
        }
        /// <summary>
        /// 通过函数验证处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void SetVerifyMethod()
        {
            ServerSocket.IsVerifyMethod = true;
        }
        /// <summary>
        /// 添加自定义 TCP 服务器端同步调用任务
        /// </summary>
        /// <param name="task">任务委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AddTask(Action task)
        {
            if (task != null) ServerCallTask.Task.Add(new CustomServerCall { Sender = this, Task = task });
        }
        /// <summary>
        /// 添加自定义 TCP 服务器端同步调用任务
        /// </summary>
        /// <param name="task">任务委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AddWaitTask(Action task)
        {
            if (task != null)
            {
                CustomWaitServerCall serverCall = new CustomWaitServerCall { Sender = this, Task = task };
                serverCall.Wait.Set(0);
                ServerCallTask.Task.Add(serverCall);
                serverCall.Wait.Wait();
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="outputInfo"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Serialize<valueType>(OutputInfo outputInfo, ref valueType value)
            where valueType : struct
        {
            if (outputInfo.IsSimpleSerializeOutputParamter) SimpleSerialize.TypeSerializer<valueType>.Serializer(OutputSerializer.Stream, ref value);
            else
            {
                int parameterIndex = outputInfo.OutputParameterIndex;
                if (serializeParameterIndex == parameterIndex) OutputSerializer.SerializeTcpServerNext(ref value);
                else
                {
                    OutputSerializer.SerializeTcpServer(ref value);
                    serializeParameterIndex = parameterIndex;
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void JsonSerialize<valueType>(ref valueType value)
            where valueType : struct
        {
            if (OutputJsonSerializer == null)
            {
                OutputJsonSerializer = Json.Serializer.YieldPool.Default.Pop() ?? new Json.Serializer();
                OutputJsonSerializer.SetTcpServer();
            }
            OutputJsonSerializer.SerializeTcpServer(ref value, OutputSerializer.Stream);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe bool DeSerialize<valueType>(ref SubArray<byte> data, ref valueType value, bool isSimpleSerialize = false)
            where valueType : struct
        {
            return ServerSocket.DeSerialize(ref data, ref value, isSimpleSerialize);
        }
        /// <summary>
        /// 创建输出
        /// </summary>
        internal virtual void VirtualBuildOutput()
        {
            throw new InvalidOperationException();
        }
    }
}
