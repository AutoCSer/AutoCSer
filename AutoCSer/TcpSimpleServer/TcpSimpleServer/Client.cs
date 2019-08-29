using System;
using System.Threading;
using System.Net.Sockets;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Net;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务客户端
    /// </summary>
    public abstract class Client : TcpServer.CommandBase
    {
        /// <summary>
        /// 是否需要心跳检测
        /// </summary>
        protected int isCheck;
        /// <summary>
        /// 套接字发送数据次数
        /// </summary>
        public int SendCount { get; internal set; }
        /// <summary>
        /// 套接字接收数据次数
        /// </summary>
        public int ReceiveCount { get; internal set; }
        /// <summary>
        /// 序列化参数编号
        /// </summary>
        protected int serializeParameterIndex;
        /// <summary>
        /// 服务主机名称
        /// </summary>
        internal string Host;
        /// <summary>
        /// 服务 IP 地址
        /// </summary>
        internal IPAddress IpAddress;
        /// <summary>
        /// 服务端口
        /// </summary>
        internal int Port;

        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 输出数据流
        /// </summary>
        protected readonly UnmanagedStream outputStream;
        /// <summary>
        /// 输出数据二进制序列化
        /// </summary>
        protected readonly BinarySerialize.Serializer outputSerializer;
        /// <summary>
        /// 输出数据 JSON 序列化
        /// </summary>
        protected Json.Serializer outputJsonSerializer;
        /// <summary>
        /// 回调数据二进制反序列化
        /// </summary>
        protected BinarySerialize.DeSerializer receiveDeSerializer;
        /// <summary>
        /// 回调数据 JSON 解析
        /// </summary>
        protected Json.Parser receiveJsonParser;
        /// <summary>
        /// 发送变换数据
        /// </summary>
        internal ulong SendMarkData;
        /// <summary>
        /// 接收变换数据
        /// </summary>
        internal ulong ReceiveMarkData;
        /// <summary>
        /// TCP 服务套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// 最大输入数据字节数
        /// </summary>
        protected readonly int maxInputSize;
        /// <summary>
        /// 下一个心跳检测
        /// </summary>
        internal Client CheckNext;
        /// <summary>
        /// 上一个心跳检测
        /// </summary>
        internal Client CheckPrevious;
        /// <summary>
        /// 心跳检测超时秒数
        /// </summary>
        internal long CheckTimeoutSeconds;
        /// <summary>
        /// 客户端心跳检测定时
        /// </summary>
        internal ClientCheckTimer CheckTimer;
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal Client() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="host">监听主机名称</param>
        /// <param name="port">监听端口</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="sendBufferMaxSize">发送数据缓存区最大字节大小</param>
        /// <param name="minCompressSize">压缩启用最低字节数量</param>
        /// <param name="log">日志接口</param>
        /// <param name="maxInputSize">最大输入数据字节数</param>
        internal Client(string host, int port, string serviceName, int sendBufferMaxSize, int minCompressSize, ILog log, int maxInputSize) : base(serviceName, sendBufferMaxSize, minCompressSize, log)
        {
            this.Host = host;
            this.Port = port;
            IpAddress = HostPort.HostToIPAddress(this.Host, Log);

            this.maxInputSize = maxInputSize <= 0 ? int.MaxValue : maxInputSize;
            outputStream = (outputSerializer = BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new BinarySerialize.Serializer()).SetTcpServer();
        }
        /// <summary>
        /// 检测主机名称是否可用
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        protected bool check(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
            {
                Log.Add(AutoCSer.Log.LogType.Error, Host + " IP 解析失败");
                return false;
            }
            if (port == 0)
            {
                Log.Add(AutoCSer.Log.LogType.Error, ServerName + " 端口号不能为 0");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Check()
        {
            if (IsDisposed == 0)
            {
                if (Socket != null)
                {
                    if (isCheck == 0) isCheck = 1;
                    else check();
                }
                CheckTimer.Reset(this);
            }
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        protected abstract void check();
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeCheck()
        {
            FreeCheckReset();
            CheckPrevious = null;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeCheckReset()
        {
            CheckPrevious.CheckNext = CheckNext;
            CheckNext.CheckPrevious = CheckPrevious;
            CheckNext = null;
        }
    }
    /// <summary>
    /// TCP 服务客户端
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    public abstract partial class Client<attributeType> : Client
        where attributeType : ServerAttribute
    {

        /// <summary>
        /// TCP 服务调用配置
        /// </summary>
        internal readonly attributeType Attribute;
        /// <summary>
        /// 远程表达式客户端检测服务端映射标识
        /// </summary>
        private readonly RemoteExpressionServerNodeIdChecker remoteExpressionServerNodeIdChecker;
        /// <summary>
        /// 套接字访问锁
        /// </summary>
        internal readonly object SocketLock = new object();

#if !NOJIT
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal Client() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="maxInputSize">最大输入数据字节数</param>
        public Client(attributeType attribute, ILog log, int maxInputSize)
            : base(attribute.Host, attribute.Port, attribute.ServerName, attribute.ClientSendBufferMaxSize, attribute.GetMinCompressSize, log, maxInputSize)
        {
            Attribute = attribute;
            SubBuffer.Pool.GetPool(attribute.GetSendBufferSize).Get(ref Buffer);
            if (attribute.IsRemoteExpression) remoteExpressionServerNodeIdChecker = new RemoteExpressionServerNodeIdChecker { Client = this };
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (IsDisposed == 0)
            {
                Monitor.Enter(SocketLock);
                try
                {
                    if (IsDisposed == 0)
                    {
                        IsDisposed = 1;
                        closeSocket();

                        if (CheckTimer != null)
                        {
                            CheckTimer.Free(this);
                            ClientCheckTimer.FreeNotNull(CheckTimer);
                        }
                        outputSerializer.Free();
                        if (outputJsonSerializer != null) outputJsonSerializer.Free();
                        Buffer.Free();
                    }
                }
                finally { Monitor.Exit(SocketLock); }
            }
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <returns></returns>
        internal abstract bool CallVerifyMethod();
        /// <summary>
        /// 创建套接字
        /// </summary>
        public void TryCreateSocket()
        {
            if (check(IpAddress, Port))
            {
                Monitor.Enter(SocketLock);
                try
                {
                    getSocket();
                }
                finally { Monitor.Exit(SocketLock); }
            }
        }
        /// <summary>
        /// 获取 TCP 服务客户端套接字
        /// </summary>
        /// <returns></returns>
        protected bool getSocket()
        {
            if (Socket != null) return true;
            Socket = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            bool isVerifyMethod = false;
            try
            {
#if !MONO
                Socket.ReceiveBufferSize = Socket.SendBufferSize = Buffer.Length;
#endif
                Socket.Connect(IpAddress, Port);
                if (isVerifyMethod = CallVerifyMethod())
                {
                    if (Attribute.GetCheckSeconds > 0 && CheckTimer == null)
                    {
                        CheckTimer = ClientCheckTimer.Get(Attribute.GetCheckSeconds);
                        if (IsDisposed == 0) CheckTimer.Push(this);
                        else
                        {
                            ClientCheckTimer.Free(ref CheckTimer);
                            return isVerifyMethod = false;
                        }
                    }
                    return true;
                }
            }
            finally
            {
                if (!isVerifyMethod) closeSocket();
            }
            return false;
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        protected void closeSocket()
        {
            if (Socket != null)
            {
                AutoCSer.Net.TcpServer.CommandBuffer.CloseClientNotNull(Socket);
                Socket = null;
            }
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <typeparam name="inputParameterType"></typeparam>
        /// <param name="commandInfo"></param>
        /// <param name="inputParameter"></param>
        /// <param name="clientBuffer"></param>
        private unsafe void build<inputParameterType>(TcpServer.CommandInfoBase commandInfo, ref inputParameterType inputParameter, ref ClientBuffer clientBuffer)
            where inputParameterType : struct
        {
            byte* start = outputStream.Data.Byte;
            outputStream.ByteSize = sizeof(uint) + sizeof(int);
            if ((commandInfo.CommandFlags & TcpServer.CommandFlags.JsonSerialize) == 0)
            {
                if (commandInfo.IsSimpleSerializeInputParamter) SimpleSerialize.TypeSerializer<inputParameterType>.Serializer(outputStream, ref inputParameter);
                else
                {
                    int parameterIndex = commandInfo.InputParameterIndex;
                    if (serializeParameterIndex == parameterIndex) outputSerializer.SerializeTcpServerNext(ref inputParameter);
                    else
                    {
                        outputSerializer.SerializeTcpServer(ref inputParameter);
                        serializeParameterIndex = parameterIndex;
                    }
                }
            }
            else
            {
                if (outputJsonSerializer == null)
                {
                    outputJsonSerializer = Json.Serializer.YieldPool.Default.Pop() ?? new Json.Serializer();
                    outputJsonSerializer.SetTcpServer();
                }
                outputJsonSerializer.SerializeTcpServer(ref inputParameter, outputSerializer.Stream);
            }
            int dataLength = outputStream.ByteSize - (sizeof(uint) + sizeof(int));
            if (dataLength <= maxInputSize)
            {
                byte* write = outputStream.Data.Byte;
                *(uint*)write = (uint)commandInfo.Command | (uint)commandInfo.CommandFlags;
                *(int*)(write + sizeof(uint)) = dataLength;

                if (outputStream.ByteSize <= Buffer.Length)
                {
                    if (start != write) Memory.CopyNotNull(write, start, outputStream.ByteSize);
                    clientBuffer.Data.Set(Buffer.Buffer, Buffer.StartIndex, outputStream.ByteSize);
                }
                else
                {
                    outputStream.GetSubBuffer(ref clientBuffer.CopyBuffer);
                    clientBuffer.SetSendDataCopyBuffer(outputStream.ByteSize);
                    if (clientBuffer.CopyBuffer.Length <= SendBufferMaxSize)
                    {
                        Buffer.Free();
                        clientBuffer.CopyBuffer.CopyToClear(ref Buffer);
                    }
                }
                if (dataLength < MinCompressSize || !clientBuffer.CompressSendData(dataLength, SendMarkData))
                {
                    if (SendMarkData == 0) clientBuffer.IsError = true;
                    else clientBuffer.MarkSendData(dataLength, SendMarkData);
                }
            }
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <typeparam name="outputParameterType"></typeparam>
        /// <param name="commandInfo"></param>
        /// <param name="outputParameter"></param>
        /// <param name="clientBuffer"></param>
        private unsafe void receive<outputParameterType>(TcpServer.CommandInfoBase commandInfo, ref outputParameterType outputParameter, ref ClientBuffer clientBuffer)
            where outputParameterType : struct
        {
            int compressionDataSize, dataSize = 0, nextSize, receiveSize = Socket.Receive(Buffer.Buffer, Buffer.StartIndex, Buffer.Length, SocketFlags.None, out clientBuffer.SocketError);
            ++ReceiveCount;
            if (receiveSize >= sizeof(int) * 2)
            {
                fixed (byte* bufferFixed = Buffer.Buffer)
                {
                    byte* start = bufferFixed + Buffer.StartIndex;
                    if ((compressionDataSize = *(int*)start) > 0)
                    {
                        if ((nextSize = compressionDataSize + sizeof(int) - receiveSize) == 0)
                        {
                            clientBuffer.IsError = false;
                            if (ReceiveMarkData != 0) Mark(Buffer.Buffer, ReceiveMarkData, Buffer.StartIndex + sizeof(int), compressionDataSize);
                            clientBuffer.SetReceiveData(ref Buffer, compressionDataSize);
                            deSerialize(commandInfo, ref outputParameter, ref clientBuffer);
                            return;
                        }
                        if (nextSize > 0 && clientBuffer.SocketError == SocketError.Success)
                        {
                            if (nextSize <= Buffer.Length) goto RECEIVE;
                            else goto BIGBUFFER;
                        }
                    }
                    else if (compressionDataSize < 0)
                    {
                        if ((compressionDataSize = -compressionDataSize) <= (dataSize = *(int*)(start + sizeof(int))))
                        {
                            if ((nextSize = compressionDataSize + sizeof(int) * 2 - receiveSize) == 0)
                            {
                                clientBuffer.IsError = false;
                                if (ReceiveMarkData != 0) Mark(Buffer.Buffer, ReceiveMarkData, Buffer.StartIndex + sizeof(int) * 2, compressionDataSize);
                                if (clientBuffer.DeCompressReceiveData(ref Buffer, compressionDataSize, dataSize)) deSerialize(commandInfo, ref outputParameter, ref clientBuffer);
                                return;
                            }
                            if (nextSize > 0)
                            {
                                if (nextSize <= Buffer.Length) goto RECEIVE;
                                else goto BIGBUFFER;
                            }
                        }
                    }
                    else if (receiveSize == sizeof(int) * 2)
                    {
                        clientBuffer.ReturnType = (TcpServer.ReturnType)(*(start + sizeof(int)));
                        return;
                    }
                }
            }
            clientBuffer.ReturnType = TcpServer.ReturnType.ClientReceiveError;
            return;
            BIGBUFFER:
            clientBuffer.CopyBufferData(ref Buffer, receiveSize + nextSize, receiveSize);
            if (clientBuffer.CopyBuffer.Length > SendBufferMaxSize)
            {
                do
                {
                    int count = Socket.Receive(Buffer.Buffer, Buffer.StartIndex + receiveSize, nextSize, SocketFlags.None, out clientBuffer.SocketError);
                    ++ReceiveCount;
                    if ((nextSize -= count) == 0)
                    {
                        clientBuffer.IsError = false;
                        if (dataSize == 0)
                        {
                            if (ReceiveMarkData != 0) Mark(clientBuffer.CopyBuffer.Buffer, ReceiveMarkData, clientBuffer.CopyBuffer.StartIndex + sizeof(int), compressionDataSize);
                            clientBuffer.SetReceiveData(compressionDataSize);
                            deSerialize(commandInfo, ref outputParameter, ref clientBuffer);
                        }
                        else
                        {
                            if (ReceiveMarkData != 0) Mark(clientBuffer.CopyBuffer.Buffer, ReceiveMarkData, clientBuffer.CopyBuffer.StartIndex + sizeof(int) * 2, compressionDataSize);
                            if (clientBuffer.DeCompressReceiveData(compressionDataSize, dataSize)) deSerialize(commandInfo, ref outputParameter, ref clientBuffer);
                        }
                        return;
                    }
                    if (count <= 0 || clientBuffer.SocketError != SocketError.Success)
                    {
                        clientBuffer.ReturnType = TcpServer.ReturnType.ClientReceiveError;
                        return;
                    }
                    receiveSize += count;
                }
                while (true);
            }
            Buffer.Free();
            clientBuffer.CopyBuffer.CopyToClear(ref Buffer);
            RECEIVE:
            do
            {
                int count = Socket.Receive(Buffer.Buffer, Buffer.StartIndex + receiveSize, nextSize, SocketFlags.None, out clientBuffer.SocketError);
                ++ReceiveCount;
                if ((nextSize -= count) == 0)
                {
                    clientBuffer.IsError = false;
                    if (dataSize == 0)
                    {
                        if (ReceiveMarkData != 0) Mark(Buffer.Buffer, ReceiveMarkData, Buffer.StartIndex + sizeof(int), compressionDataSize);
                        clientBuffer.SetReceiveData(ref Buffer, compressionDataSize);
                        deSerialize(commandInfo, ref outputParameter, ref clientBuffer);
                    }
                    else
                    {
                        if (ReceiveMarkData != 0) Mark(Buffer.Buffer, ReceiveMarkData, Buffer.StartIndex + sizeof(int) * 2, compressionDataSize);
                        if (clientBuffer.DeCompressReceiveData(ref Buffer, compressionDataSize, dataSize)) deSerialize(commandInfo, ref outputParameter, ref clientBuffer);
                    }
                    return;
                }
                if (count <= 0 || clientBuffer.SocketError != SocketError.Success)
                {
                    clientBuffer.ReturnType = TcpServer.ReturnType.ClientReceiveError;
                    return;
                }
                receiveSize += count;
            }
            while (true);
        }
        /// <summary>
        /// 数据反序列化
        /// </summary>
        /// <typeparam name="outputParameterType"></typeparam>
        /// <param name="commandInfo"></param>
        /// <param name="outputParameter"></param>
        /// <param name="clientBuffer"></param>
        private unsafe void deSerialize<outputParameterType>(TcpServer.CommandInfoBase commandInfo, ref outputParameterType outputParameter, ref ClientBuffer clientBuffer)
            where outputParameterType : struct
        {
            if ((commandInfo.CommandFlags & TcpServer.CommandFlags.JsonSerialize) == 0)
            {
                if (commandInfo.IsSimpleSerializeOutputParamter)
                {
                    fixed (byte* dataFixed = clientBuffer.Data.Array)
                    {
                        byte* start = dataFixed + clientBuffer.Data.Start, end = start + clientBuffer.Data.Length;
                        if (SimpleSerialize.TypeDeSerializer<outputParameterType>.DeSerialize(start, ref outputParameter, end) == end)
                        {
                            clientBuffer.ReturnType = TcpServer.ReturnType.Success;
                            return;
                        }
                    }
                }
                else
                {
                    if (receiveDeSerializer == null)
                    {
                        receiveDeSerializer = BinarySerialize.DeSerializer.YieldPool.Default.Pop() ?? new BinarySerialize.DeSerializer();
                        receiveDeSerializer.SetTcpServer(AutoCSer.BinarySerialize.DeSerializer.DefaultConfig, null);
                    }
                    if (receiveDeSerializer.DeSerializeTcpServer(ref clientBuffer.Data, ref outputParameter))
                    {
                        clientBuffer.ReturnType = TcpServer.ReturnType.Success;
                        return;
                    }
                }
            }
            else
            {
                if (receiveJsonParser == null)
                {
                    receiveJsonParser = Json.Parser.YieldPool.Default.Pop() ?? new Json.Parser();
                    receiveJsonParser.SetTcpServer();
                }
                if (receiveJsonParser.ParseTcpServer(ref clientBuffer.Data, ref outputParameter))
                {
                    clientBuffer.ReturnType = TcpServer.ReturnType.Success;
                    return;
                }
            }
            clientBuffer.ReturnType = TcpServer.ReturnType.ClientDeSerializeError;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="commandInfo">命令信息</param>
        /// <param name="inputParameter">输入参数</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>返回值类型</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe TcpServer.ReturnType Get<inputParameterType, outputParameterType>(TcpServer.CommandInfoBase commandInfo, ref inputParameterType inputParameter, ref outputParameterType outputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            ClientBuffer clientBuffer = default(ClientBuffer);
            Monitor.Enter(SocketLock);
            try
            {
                if (IsDisposed == 0)
                {
                    if (commandInfo.IsVerifyMethod || getSocket())
                    {
                        fixed (byte* dataFixed = Buffer.Buffer)
                        {
                            outputStream.Reset(dataFixed + Buffer.StartIndex, Buffer.Length);
                            build(commandInfo, ref inputParameter, ref clientBuffer);
                        }
                        if (clientBuffer.Send(this))
                        {
                            clientBuffer.Free();
                            receive(commandInfo, ref outputParameter, ref clientBuffer);
                        }
                    }
                    else clientBuffer.ReturnType = TcpServer.ReturnType.ClientException;
                }
            }
            finally
            {
                isCheck = 0;
                if (clientBuffer.IsError) closeSocket();
                Monitor.Exit(SocketLock);
                clientBuffer.Free();
            }
            return clientBuffer.ReturnType;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="commandInfo">命令信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>返回值类型</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe TcpServer.ReturnType Get<outputParameterType>(TcpServer.CommandInfoBase commandInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            ClientBuffer clientBuffer = default(ClientBuffer);
            Monitor.Enter(SocketLock);
            try
            {
                if (IsDisposed == 0)
                {
                    if (getSocket())
                    {
                        fixed (byte* dataFixed = Buffer.Buffer)
                        {
                            *(uint*)(dataFixed + Buffer.StartIndex) = (uint)commandInfo.Command | (uint)TcpServer.CommandFlags.NullData | (uint)commandInfo.CommandFlags;
                            clientBuffer.IsError = true;
                            int size = Socket.Send(Buffer.Buffer, Buffer.StartIndex, sizeof(uint), SocketFlags.None, out clientBuffer.SocketError);
                            ++SendCount;
                            if (size == sizeof(uint)) receive(commandInfo, ref outputParameter, ref clientBuffer);
                            else clientBuffer.ReturnType = TcpServer.ReturnType.ClientSendError;
                        }
                    }
                    else clientBuffer.ReturnType = TcpServer.ReturnType.ClientException;
                }
            }
            finally
            {
                isCheck = 0;
                if (clientBuffer.IsError) closeSocket();
                Monitor.Exit(SocketLock);
                clientBuffer.Free();
            }
            return clientBuffer.ReturnType;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="commandInfo">命令信息</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>返回值类型</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe TcpServer.ReturnType Call<inputParameterType>(TcpServer.CommandInfoBase commandInfo, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            ClientBuffer clientBuffer = default(ClientBuffer);
            Monitor.Enter(SocketLock);
            try
            {
                if (IsDisposed == 0)
                {
                    if (commandInfo.IsVerifyMethod || getSocket())
                    {
                        fixed (byte* dataFixed = Buffer.Buffer)
                        {
                            outputStream.Reset(dataFixed + Buffer.StartIndex, Buffer.Length);
                            build(commandInfo, ref inputParameter, ref clientBuffer);
                        }
                        if (clientBuffer.Send(this))
                        {
                            int size = Socket.Receive(Buffer.Buffer, Buffer.StartIndex, sizeof(int), SocketFlags.None, out clientBuffer.SocketError);
                            ++ReceiveCount;
                            if (size == sizeof(uint)) clientBuffer.SetReturnType((TcpServer.ReturnType)Buffer.Buffer[Buffer.StartIndex]);
                            else clientBuffer.ReturnType = TcpServer.ReturnType.ClientReceiveError;
                        }
                    }
                    else clientBuffer.ReturnType = TcpServer.ReturnType.ClientException;
                }
            }
            finally
            {
                isCheck = 0;
                if (clientBuffer.IsError) closeSocket();
                Monitor.Exit(SocketLock);
                clientBuffer.Free();
            }
            return clientBuffer.ReturnType;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="commandInfo">命令信息</param>
        /// <returns>返回值类型</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe TcpServer.ReturnType Call(TcpServer.CommandInfoBase commandInfo)
        {
            TcpServer.ReturnType returnType = TcpServer.ReturnType.ClientDisposed;
            SocketError socketError;
            bool isError = false;
            Monitor.Enter(SocketLock);
            try
            {
                if (IsDisposed == 0)
                {
                    if (getSocket())
                    {
                        byte[] buffer = Buffer.Buffer;
                        fixed (byte* dataFixed = buffer)
                        {
                            byte* start = dataFixed + Buffer.StartIndex;
                            *(uint*)start = (uint)commandInfo.Command | (uint)TcpServer.CommandFlags.NullData | (uint)commandInfo.CommandFlags;
                            isError = true;
                            int size = Socket.Send(buffer, Buffer.StartIndex, sizeof(uint), SocketFlags.None, out socketError);
                            ++SendCount;
                            if (size == sizeof(uint))
                            {
                                size = Socket.Receive(buffer, Buffer.StartIndex, sizeof(int), SocketFlags.None, out socketError);
                                ++ReceiveCount;
                                if (size == sizeof(uint))
                                {
                                    returnType = (TcpServer.ReturnType)(*start);
                                    isError = false;
                                }
                                else returnType = TcpServer.ReturnType.ClientReceiveError;
                            }
                            else returnType = TcpServer.ReturnType.ClientSendError;
                        }
                    }
                    else returnType = TcpServer.ReturnType.ClientException;
                }
            }
            finally
            {
                isCheck = 0;
                if (isError) closeSocket();
                Monitor.Exit(SocketLock);
            }
            return returnType;
        }
        /// <summary>
        /// 获取远程表达式服务端节点标识
        /// </summary>
        /// <param name="types">表达式服务端节点类型集合</param>
        /// <returns>表达式服务端节点标识集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal TcpServer.ReturnValue<int[]> GetRemoteExpressionNodeId(RemoteType[] types)
        {
            RemoteExpression.ServerNodeIdChecker.Input inputParameter = new RemoteExpression.ServerNodeIdChecker.Input { Types = types };
            RemoteExpression.ServerNodeIdChecker.Output outputParameter = default(RemoteExpression.ServerNodeIdChecker.Output);
            TcpServer.ReturnType returnType = Get(RemoteExpression.ServerNodeIdChecker.Input.CommandInfo, ref inputParameter, ref outputParameter);
            return new TcpServer.ReturnValue<int[]> { Type = returnType, Value = outputParameter.Return };
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <param name="clientNode">客户端远程表达式节点</param>
        /// <returns>返回值类型</returns>
        public TcpServer.ReturnType GetRemoteExpressionClientNode(RemoteExpression.Node node, out RemoteExpression.ClientNode clientNode)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            TcpServer.ReturnType returnType = checker.Check(node);
            clientNode = returnType == TcpServer.ReturnType.Success ? new RemoteExpression.ClientNode { Node = node, Checker = checker, ClientNodeId = node.ClientNodeId } : default(RemoteExpression.ClientNode);
            return returnType;
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>客户端远程表达式节点</returns>
        public RemoteExpression.ClientNode GetRemoteExpressionClientNode(RemoteExpression.Node node)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            if (checker.Check(node) == TcpServer.ReturnType.Success) return new RemoteExpression.ClientNode { Node = node, Checker = checker, ClientNodeId = node.ClientNodeId };
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取客户端远程表达式参数节点
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="node">远程表达式参数节点</param>
        /// <returns>客户端远程表达式参数节点</returns>
        public RemoteExpression.ClientNode<returnType> GetRemoteExpressionClientNodeParameter<returnType>(RemoteExpression.Node<returnType> node)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            if (checker.Check(node) == TcpServer.ReturnType.Success) return new RemoteExpression.ClientNode<returnType> { Node = node, Checker = checker };
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        private TcpServer.ReturnValue<RemoteExpression.ReturnValue> getRemoteExpression(ref RemoteExpression.ClientNode node)
        {
            RemoteExpression.ReturnValue.Output outputParameter = default(RemoteExpression.ReturnValue.Output);
            TcpServer.ReturnType returnType = Get(RemoteExpression.ClientNode.CommandInfo, ref node, ref outputParameter);
            return new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = returnType, Value = outputParameter.Return };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnType CallRemoteExpression(RemoteExpression.Node node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                TcpServer.ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                return type == TcpServer.ReturnType.Success ? getRemoteExpression(ref clientNode).Type : type;
            }
            return TcpServer.ReturnType.RemoteExpressionNotSupport;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        public TcpServer.ReturnValue<returnType> GetRemoteExpression<returnType>(RemoteExpression.Node<returnType> node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                TcpServer.ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                if (type == TcpServer.ReturnType.Success)
                {
                    TcpServer.ReturnValue<RemoteExpression.ReturnValue> value = getRemoteExpression(ref clientNode);
                    if (value.Type == TcpServer.ReturnType.Success) return ((RemoteExpression.ReturnValue<returnType>)value.Value).Value;
                    return new TcpServer.ReturnValue<returnType> { Type = value.Type };
                }
                return new TcpServer.ReturnValue<returnType> { Type = type };
            }
            return new TcpServer.ReturnValue<returnType> { Type = TcpServer.ReturnType.RemoteExpressionNotSupport };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnValue<RemoteExpression.ReturnValue> GetRemoteExpression(RemoteExpression.ClientNode node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                return node.Checker != remoteExpressionServerNodeIdChecker ? getRemoteExpression(ref node) : new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = TcpServer.ReturnType.RemoteExpressionCheckerError };
            }
            return new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = TcpServer.ReturnType.RemoteExpressionNotSupport };
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        protected unsafe override void check()
        {
            SocketError socketError;
            bool isError = false;
            if (Monitor.TryEnter(SocketLock))
            {
                try
                {
                    if (Socket != null && isCheck != 0 && IsDisposed == 0)
                    {
                        byte[] buffer = Buffer.Buffer;
                        fixed (byte* dataFixed = buffer)
                        {
                            byte* start = dataFixed + Buffer.StartIndex;
                            *(uint*)start = (uint)TcpServer.Server.CheckCommandIndex | (uint)TcpServer.CommandFlags.NullData;
                            isError = true;
                            int size = Socket.Send(buffer, Buffer.StartIndex, sizeof(uint), SocketFlags.None, out socketError);
                            ++SendCount;
                            if (size == sizeof(uint))
                            {
                                size = Socket.Receive(buffer, Buffer.StartIndex, sizeof(int), SocketFlags.None, out socketError);
                                ++ReceiveCount;
                                if (size == sizeof(uint) && (TcpServer.ReturnType)(*start) == TcpServer.ReturnType.Success) isError = false;
                            }
                        }
                    }
                }
                finally
                {
                    if (isError) closeSocket();
                    Monitor.Exit(SocketLock);
                }
            }
        }
    }
}
