using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务端套接字
    /// </summary>
    public abstract unsafe class ServerSocket : SocketTimeoutLink
    {
        /// <summary>
        /// 默认验证函数调用次数
        /// </summary>
        internal const byte DefaultVerifyMethodCount = 2;

        /// <summary>
        /// 二进制反序列化配置参数
        /// </summary>
        private readonly AutoCSer.BinarySerialize.DeSerializeConfig binaryDeSerializeConfig;
        /// <summary>
        /// 命令位图
        /// </summary>
        protected Pointer.Size commandData;
        /// <summary>
        /// 命令位图
        /// </summary>
        protected MemoryMap commands;
#if DOTNET2
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        protected AsyncCallback onReceiveAsyncCallback;
        /// <summary>
        /// 接收数据套接字异步事件对象
        /// </summary>
        protected IAsyncResult receiveAsyncEventArgs;
#else
        /// <summary>
        /// 接收数据套接字异步事件对象
        /// </summary>
        protected SocketAsyncEventArgs receiveAsyncEventArgs;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        protected int receiveAsyncLock;
#endif
#endif
        /// <summary>
        /// 变换数据
        /// </summary>
        internal ulong MarkData;
        /// <summary>
        /// 接收数据二进制反序列化
        /// </summary>
        internal BinarySerialize.DeSerializer ReceiveDeSerializer;
        /// <summary>
        /// 接收数据 JSON 解析
        /// </summary>
        internal Json.Parser ReceiveJsonParser;
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull ReceiveBuffer;
        /// <summary>
        /// 临时接收数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull ReceiveBigBuffer;
        /// <summary>
        /// 接收数据缓冲区字节大小
        /// </summary>
        protected int receiveBufferSize;

        /// <summary>
        /// 接收数据超时
        /// </summary>
        protected DateTime receiveTimeout;
        /// <summary>
        /// 接收数据起始位置
        /// </summary>
        protected byte* receiveDataStart;
        /// <summary>
        /// 当前处理会话标识
        /// </summary>
        internal uint CommandIndex;
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
        /// 当前接收临时数据字节数
        /// </summary>
        protected int receiveBigBufferCount;
        /// <summary>
        /// 套接字错误
        /// </summary>
        protected SocketError socketError = SocketError.Success;
        /// <summary>
        /// TCP 服务端套接字接收数据回调类型
        /// </summary>
        internal ServerSocketReceiveType ReceiveType;
        /// <summary>
        /// 验证函数调用次数
        /// </summary>
        protected byte verifyMethodCount = DefaultVerifyMethodCount;
        /// <summary>
        /// 是否通过函数验证
        /// </summary>
        internal bool IsVerifyMethod;
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        /// <param name="binaryDeSerializeConfig">二进制反序列化配置参数</param>
        internal ServerSocket(AutoCSer.BinarySerialize.DeSerializeConfig binaryDeSerializeConfig)
        {
            this.binaryDeSerializeConfig = binaryDeSerializeConfig;
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal abstract bool SetCommand(int methodIndex);
        /// <summary>
        /// 设置基础命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal abstract bool SetBaseCommand(int command);
        /// <summary>
        /// 判断命令是否有效
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe bool IsCommand(int index)
        {
            return commands.Map == null || commands.Get(index) != 0;
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
        internal bool DeSerialize<valueType>(ref SubArray<byte> data, ref valueType value, bool isSimpleSerialize)
            where valueType : struct
        {
            if ((CommandIndex & (uint)CommandFlags.JsonSerialize) == 0)
            {
                if (isSimpleSerialize)
                {
                    fixed (byte* dataFixed = data.Array)
                    {
                        byte* start = dataFixed + data.Start, end = start + data.Length;
                        return SimpleSerialize.TypeDeSerializer<valueType>.DeSerialize(start, ref value, end) == end;
                    }
                }
                if (ReceiveDeSerializer == null)
                {
                    ReceiveDeSerializer = BinarySerialize.DeSerializer.YieldPool.Default.Pop() ?? new BinarySerialize.DeSerializer();
                    ReceiveDeSerializer.SetTcpServer(binaryDeSerializeConfig, this);
                }
                return ReceiveDeSerializer.DeSerializeTcpServer(ref data, ref value);
                //if (ReceiveDeSerializer.DeSerializeTcpServer(ref data, ref value)) return true;
                //if (data.Length > 1 << 20) System.IO.File.WriteAllBytes((++testIdentity).ToString() + "." + data.Length.ToString(), data.ToArray());
                //return false;
            }
            if (ReceiveJsonParser == null)
            {
                ReceiveJsonParser = Json.Parser.YieldPool.Default.Pop() ?? new Json.Parser();
                ReceiveJsonParser.SetTcpServer();
            }
            return ReceiveJsonParser.ParseTcpServer(ref data, ref value);
        }
        /// <summary>
        /// 释放数据反序列化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeReceiveDeSerializer()
        {
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
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CloseFree()
        {
            ReceiveBuffer.Free();
            ReceiveBigBuffer.TryFree();
            FreeReceiveDeSerializer();
            Unmanaged.Free(ref commandData);
        }
    }
    /// <summary>
    /// TCP 服务端套接字
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="serverType">TCP 服务类型</typeparam>
    /// <typeparam name="socketType">TCP 服务端套接字类型</typeparam>
    /// <typeparam name="socketSenderType">TCP 服务套接字数据发送</typeparam>
    public abstract class ServerSocket<attributeType, serverType, socketType, socketSenderType> : ServerSocket
        where attributeType : ServerAttribute
        where serverType : Server<attributeType, serverType, socketSenderType>
        where socketType : ServerSocket<attributeType, serverType, socketType, socketSenderType>
        where socketSenderType : ServerSocketSender<attributeType, serverType, socketType, socketSenderType>
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long TaskTicks;
        /// <summary>
        /// 下一个任务
        /// </summary>
        internal socketType NextTask;
        /// <summary>
        /// TCP 服务
        /// </summary>
        internal readonly serverType Server;
        /// <summary>
        /// TCP 服务端套接字
        /// </summary>
        /// <param name="server">TCP调用服务端</param>
        internal ServerSocket(serverType server)
            : base(server.BinaryDeSerializeConfig)
        {
            Server = server;
            //if (server.VerifyCommandIdentity == 0) IsVerifyMethod = true;
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
        internal override bool SetCommand(int methodIndex)
        {
            return setCommand(methodIndex + TcpServer.Server.CommandStartIndex);
        }
        /// <summary>
        /// 设置基础命令索引信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal override bool SetBaseCommand(int command)
        {
            return command < TcpServer.Server.CommandStartIndex && setCommand(command);
        }
    }
}
