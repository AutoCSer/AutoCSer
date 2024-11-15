﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端套接字
    /// </summary>
    public abstract unsafe class ClientSocketBase
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal readonly ClientSocketCreator ClientCreator;
        /// <summary>
        /// 服务 IP 地址
        /// </summary>
        internal readonly IPAddress IpAddress;
        /// <summary>
        /// 服务端口
        /// </summary>
        internal readonly int Port;
        /// <summary>
        /// 最大输入数据字节数
        /// </summary>
        internal readonly int MaxInputSize;
        /// <summary>
        /// 日志处理接口
        /// </summary>
        internal readonly ILog Log;
        /// <summary>
        /// 客户端延迟日志
        /// </summary>
        internal readonly ClientLazyLog LazyLog;
#if DOTNET2
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        protected AsyncCallback onReceiveAsyncCallback;
#else
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        protected EventHandler<SocketAsyncEventArgs> onReceiveAsyncCallback;
        /// <summary>
        /// 接收数据套接字异步事件对象
        /// </summary>
        protected SocketAsyncEventArgs receiveAsyncEventArgs;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        protected AutoCSer.Threading.SpinLock receiveAsyncLock;
#endif
#endif
        /// <summary>
        /// 输出数据 JSON 序列化
        /// </summary>
        internal AutoCSer.JsonSerializer OutputJsonSerializer;
        /// <summary>
        /// 输出数据二进制序列化
        /// </summary>
        internal BinarySerializer OutputSerializer;
        /// <summary>
        /// 客户端心跳检测定时
        /// </summary>
        internal ClientCheckTimer CheckTimer;
        /// <summary>
        /// 接收变换数据
        /// </summary>
        internal ulong ReceiveMarkData;
        /// <summary>
        /// TCP 套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal ClientSocketSenderBase Sender;
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
        /// 套接字错误
        /// </summary>
        protected SocketError socketError;
        /// <summary>
        /// 服务更新版本号
        /// </summary>
        internal int CreateVersion;

        /// <summary>
        /// 临时接收数据缓冲区当前接收数据字节数
        /// </summary>
        protected int receiveBigBufferCount;
        /// <summary>
        /// 下一个心跳检测
        /// </summary>
        internal ClientSocketBase CheckNext;
        /// <summary>
        /// 上一个心跳检测
        /// </summary>
        internal ClientSocketBase CheckPrevious;
        /// <summary>
        /// 心跳检测超时秒数
        /// </summary>
        internal long CheckTimeoutSeconds;
        /// <summary>
        /// 当前接收数据字节数
        /// </summary>
        protected int receiveCount;
        /// <summary>
        /// 当前处理接收数据字节数
        /// </summary>
        protected int receiveIndex;
        /// <summary>
        /// 接收数据起始位置
        /// </summary>
        protected byte* receiveDataStart;
        /// <summary>
        /// 当前数据压缩后的字节大小
        /// </summary>
        protected int compressionDataSize;
        /// <summary>
        /// 当前数据字节大小
        /// </summary>
        protected int dataSize;

        /// <summary>
        /// 获取命令回调序号
        /// </summary>
        internal ClientSocketReceiveType ReceiveType;
        /// <summary>
        /// 套接字接收数据次数
        /// </summary>
        internal int ReceiveCount;
        /// <summary>
        /// 回调数据二进制反序列化
        /// </summary>
        internal AutoCSer.BinaryDeSerializer ReceiveDeSerializer;
        /// <summary>
        /// 回调数据 JSON 解析
        /// </summary>
        internal AutoCSer.JsonDeSerializer ReceiveJsonDeSerializer;
        /// <summary>
        /// 序列化参数编号
        /// </summary>
        private int serializeParameterIndex;
        /// <summary>
        /// 是否通过验证函数
        /// </summary>
        protected bool isVerifyMethod;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected bool isClose;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal bool IsClose
        {
            get { return isClose; }
        }
        /// <summary>
        /// 重建连接是否休眠
        /// </summary>
        protected bool isSleep;

        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        /// <param name="clientCreator">TCP 服务客户端创建器</param>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="maxInputSize"></param>
        internal ClientSocketBase(ClientSocketCreator clientCreator, IPAddress ipAddress, int port, int maxInputSize)
        {
            ClientCreator = clientCreator;
            this.IpAddress = ipAddress;
            this.Port = port;
            Log = clientCreator.CommandClient.Log;
            MaxInputSize = maxInputSize > 0 ? maxInputSize : int.MaxValue;
            LazyLog = clientCreator.Attribute.ClientLazyLogSeconds > 0 ? new ClientLazyLog(Math.Max(clientCreator.Attribute.ClientLazyLogSeconds + 1, int.MaxValue), this) : ClientLazyLog.Null;
        }
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        internal abstract void Create();
        /// <summary>
        /// 释放套接字
        /// </summary>
        internal void DisposeSocket()
        {
            Socket socket = Socket;
            Socket = null;
            if (socket != null)
            {
                ClientCreator.OnDisposeSocket(this);
                AutoCSer.Net.TcpServer.CommandBuffer.ShutdownClient(socket);
            }
        }
        /// <summary>
        /// 验证函数调用
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected bool verifyMethod(ClientBase client)
        {
            if (isVerifyMethod = client.SocketVerifyMethod(Sender))
            {
                if (client.Attribute.GetCheckSeconds > 0 && CheckTimer == null)
                {
                    ClientCheckTimer checkTimer = new ClientCheckTimer(this, Math.Max(client.Attribute.GetCheckSeconds, 1));
                    if (Interlocked.CompareExchange(ref CheckTimer, checkTimer, null) == null)
                    {
                        if (client.IsDisposed == 0) checkTimer.AppendTaskArray();
                        else return false;
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断服务更新版本号是否有效
        /// </summary>
        /// <param name="oldSocket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool IsSocketVersion(ref ClientSocketBase oldSocket)
        {
            if (oldSocket == null || CreateVersion > oldSocket.CreateVersion)
            {
                oldSocket = this;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 套接字操作失败重新创建版本检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckCreateVersion()
        {
            return (ClientCreator.CommandClient.IsDisposed | (CreateVersion ^ ClientCreator.CreateVersion)) == 0
                && Interlocked.CompareExchange(ref ClientCreator.CreateVersion, CreateVersion + 1, CreateVersion) == CreateVersion;
        }
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        internal abstract void CreateNew();
        /// <summary>
        /// 创建 TCP 服务客户端套接字失败休眠
        /// </summary>
        internal void CreateSleep()
        {
            ClientCreator.CommandClient.SocketWait.PulseReset();
            if (Socket != null)
            {
                AutoCSer.Net.TcpServer.CommandBase.ShutdownClient(Socket);
                Socket = null;
            }
            Thread.Sleep(ClientCreator.CommandClient.TryCreateSleep);
        }
        /// <summary>
        /// 验证失败休眠
        /// </summary>
        internal void VerifyMethodSleep()
        {
            if (ClientCreator.VerifyCount < ClientCreator.Attribute.VerifyCount && CheckCreateVersion())
            {
                CreateSleep();
                CreateNew();
            }
            else
            {
                if (CreateVersion == ClientCreator.CreateVersion)
                {
                    ClientCreator.CommandClient.SocketWait.PulseReset();
                    Thread.Sleep(ClientCreator.CommandClient.TryCreateSleep);
                }
                if (Socket != null)
                {
                    AutoCSer.Net.TcpServer.CommandBase.ShutdownClient(Socket);
                    Socket = null;
                }
            }
        }
        /// <summary>
        /// 版本有效性检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected int checkCreate()
        {
            if (ClientCreator.CommandClient.IsDisposed == 0)
            {
                if (CreateVersion == ClientCreator.CreateVersion) return 1;
            }
            close();
            if (CreateVersion == ClientCreator.CreateVersion) ClientCreator.CommandClient.SocketWait.Set();
            return 0;
        }
        /// <summary>
        /// 关闭客户端
        /// </summary>
        protected abstract void close();
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Check()
        {
            return !isClose && Sender.Check();
        }
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
        /// <summary>
        /// 释放输出数据序列化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeOutputSerializer()
        {
            if (OutputSerializer != null)
            {
                OutputSerializer.Free();
                if (OutputJsonSerializer != null) OutputJsonSerializer.Free();
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="commandInfo"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Serialize<valueType>(CommandInfo commandInfo, ref valueType value)
            where valueType : struct
        {
            if (commandInfo.SimpleSerializeInputParamter == 0)
            {
                int parameterIndex = commandInfo.InputParameterIndex;
                if (serializeParameterIndex == parameterIndex) OutputSerializer.SerializeTcpServerNext(ref value);
                else
                {
                    OutputSerializer.SerializeTcpServer(ref value);
                    serializeParameterIndex = parameterIndex;
                }
            }
            else SimpleSerialize.TypeSerializer<valueType>.Serializer(OutputSerializer.Stream, ref value);
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
                OutputJsonSerializer = AutoCSer.JsonSerializer.YieldPool.Default.Pop() ?? new AutoCSer.JsonSerializer();
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
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DeSerialize<valueType>(ref SubArray<byte> data, ref valueType value)
            where valueType : struct
        {
            AutoCSer.BinaryDeSerializer deSerializer = Interlocked.Exchange(ref ReceiveDeSerializer, null);
            if (deSerializer == null)
            {
                deSerializer = AutoCSer.BinaryDeSerializer.YieldPool.Default.Pop() ?? new AutoCSer.BinaryDeSerializer();
                deSerializer.SetTcpServer(AutoCSer.BinaryDeSerializer.DefaultConfig, null);
            }
            bool isValue = deSerializer.DeSerializeTcpServer(ref data, ref value);
            if (Interlocked.CompareExchange(ref ReceiveDeSerializer, deSerializer, null) != null) deSerializer.Free();
            return isValue;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DeSerializeJson<valueType>(ref SubArray<byte> data, ref valueType value)
            where valueType : struct
        {
            AutoCSer.JsonDeSerializer jsonDeSerializer = Interlocked.Exchange(ref ReceiveJsonDeSerializer, null);
            if (jsonDeSerializer == null)
            {
                jsonDeSerializer = AutoCSer.JsonDeSerializer.YieldPool.Default.Pop() ?? new AutoCSer.JsonDeSerializer();
                jsonDeSerializer.SetTcpServer();
            }
            bool isValue = jsonDeSerializer.DeSerializeTcpServer(ref data, ref value);
            if (Interlocked.CompareExchange(ref ReceiveJsonDeSerializer, jsonDeSerializer, null) != null) jsonDeSerializer.Free();
            return isValue;
        }
        /// <summary>
        /// 释放回调数据反序列化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeReceiveDeSerializer()
        {
            AutoCSer.BinaryDeSerializer deSerializer = Interlocked.Exchange(ref ReceiveDeSerializer, null);
            if (deSerializer != null) deSerializer.Free();
            AutoCSer.JsonDeSerializer jsonDeSerializer = Interlocked.Exchange(ref ReceiveJsonDeSerializer, null);
            if (jsonDeSerializer != null) jsonDeSerializer.Free();
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
            if (Sender != null) Sender.Close();
            if (ClientCreator.CommandClient.IsDisposed != 0) LazyLog.KeepMode = AutoCSer.Threading.SecondTimerKeepMode.Canceled;
        }
    }
}
