﻿using System;
using AutoCSer.Log;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    public abstract unsafe class Server : ServerBase
    {
        /// <summary>
        /// 用户命令起始位置
        /// </summary>
        internal const int CommandStartIndex = 128;
        /// <summary>
        /// 连接检测命令
        /// </summary>
        internal const int CheckCommandIndex = CommandStartIndex - 1;
        /// <summary>
        /// 远程表达式服务端节点调用命令
        /// </summary>
        internal const int RemoteExpressionCommandIndex = CheckCommandIndex - 1;
        /// <summary>
        /// 远程表达式服务端节点标识解析命令
        /// </summary>
        internal const int RemoteExpressionNodeIdCommandIndex = RemoteExpressionCommandIndex - 1;
        /// <summary>
        /// 客户端流合并命令
        /// </summary>
        internal const int MergeCommandIndex = RemoteExpressionNodeIdCommandIndex - 1;
        /// <summary>
        /// 自定义数据包命令
        /// </summary>
        internal const int CustomDataCommandIndex = MergeCommandIndex - 1;
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        internal const int CancelKeepCommandIndex = CustomDataCommandIndex - 1;
        /// <summary>
        /// 最小系统命令
        /// </summary>
        internal const int MinCommandIndex = CancelKeepCommandIndex;
        ///// <summary>
        ///// 用户命令数据起始位置
        ///// </summary>
        //internal const int CommandDataIndex = 0x20202000;
        ///// <summary>
        ///// 关闭链接命令
        ///// </summary>
        //private static readonly byte[] closeCommandData = BitConverter.GetBytes(CloseCommandIndex + CommandDataIndex);
        ///// <summary>
        ///// 连接检测命令
        ///// </summary>
        //private static readonly byte[] checkCommandData = BitConverter.GetBytes(CheckCommandIndex + CommandDataIndex);
        ///// <summary>
        ///// 流合并命令
        ///// </summary>
        //private static readonly byte[] mergeCommandData = BitConverter.GetBytes(MergeCommandIndex + CommandDataIndex);
        /// <summary>
        /// 最小接收/发送数据字节数
        /// </summary>
        internal const int MinSocketSize = 512;
        /// <summary>
        /// 自定义队列关键字名称
        /// </summary>
        internal const string ServerCallQueueParameterName = "AutoCSerTcpQueueKey";
        /// <summary>
        /// JSON 序列化配置
        /// </summary>
        internal static readonly AutoCSer.Json.SerializeConfig JsonConfig;

        /// <summary>
        /// 默认验证函数调用次数
        /// </summary>
        internal const byte DefaultVerifyMethodCount = 2;
        /// <summary>
        /// 会话索引有效位
        /// </summary>
        internal const int CommandIndexBits = 29;
        /// <summary>
        /// 会话索引最大值
        /// </summary>
        internal const uint CommandIndexAnd = ((1U << CommandIndexBits) - 1);
        /// <summary>
        /// 会话索引参数
        /// </summary>
        internal const uint CommandFlagsAnd = uint.MaxValue ^ CommandIndexAnd;
        /// <summary>
        /// 获取返回值类型
        /// </summary>
        /// <param name="commandIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnType GetReturnType(ref uint commandIndex)
        {
            ReturnType type = (ReturnType)(byte)(commandIndex >> CommandIndexBits);
            commandIndex &= CommandIndexAnd;
            return type;
        }
        /// <summary>
        /// 获取命令参数
        /// </summary>
        /// <param name="commandIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static CommandFlags GetCommandFlags(ref uint commandIndex)
        {
            CommandFlags flags = (CommandFlags)(commandIndex & CommandFlagsAnd);
            commandIndex &= CommandIndexAnd;
            return flags;
        }
        /// <summary>
        /// 获取会话索引
        /// </summary>
        /// <param name="commandIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static uint GetCommandIndex(uint commandIndex, ReturnType type)
        {
            return (commandIndex & TcpServer.Server.CommandIndexAnd) | ((uint)(byte)type << TcpServer.Server.CommandIndexBits);
        }
        static Server()
        {
            JsonConfig = (AutoCSer.Json.SerializeConfig)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Json.SerializeConfig));
            if (JsonConfig == null) JsonConfig = AutoCSer.Json.SerializeConfig.CreateInternal();
            else JsonConfig = AutoCSer.MemberCopy.Copyer<AutoCSer.Json.SerializeConfig>.MemberwiseClone(JsonConfig);
            JsonConfig.IsInfinityToNaN = false;
        }

        /// <summary>
        /// 自定义队列
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.IServerCallQueueSet serverCallQueue;
        /// <summary>
        /// 自定义数据包处理
        /// </summary>
        private readonly Action<SubArray<byte>> onCustomData;
        /// <summary>
        /// 获取客户端请求线程调用类型
        /// </summary>
        private readonly AutoCSer.Threading.ThreadTaskType getSocketThreadCallType;

        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="serverCallQueue">自定义队列</param>
        /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="getSocketThreadCallType">同步验证接口</param>
        /// <param name="callQueueCount">独占的 TCP 服务器端同步调用队列数量</param>
        /// <param name="isCallQueueLink">是否提供独占的 TCP 服务器端同步调用队列（低优先级）</param>
        /// <param name="isSynchronousVerifyMethod">验证函数是否同步调用</param>
        internal Server(ServerBaseAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, AutoCSer.Net.TcpServer.IServerCallQueueSet serverCallQueue, byte extendCommandBits, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Threading.ThreadTaskType getSocketThreadCallType, int callQueueCount, bool isCallQueueLink, bool isSynchronousVerifyMethod)
            : base(attribute, verify, extendCommandBits, log, callQueueCount, isCallQueueLink, isSynchronousVerifyMethod)
        {
            this.serverCallQueue = serverCallQueue;
            this.onCustomData = onCustomData;
            this.getSocketThreadCallType = getSocketThreadCallType;
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (serverCallQueue != null) serverCallQueue.Dispose();
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        public virtual bool Start()
        {
            if (start())
            {
                startGetSocket();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取客户端套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void startGetSocket()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, getSocketThreadCallType);
            Thread.Sleep(0);
            AutoCSer.DomainUnload.Unloader.Add(this, DomainUnload.Type.TcpCommandBaseDispose);
        }
        /// <summary>
        /// 自定义数据包处理
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CustomData(ref SubArray<byte> data)
        {
            if (onCustomData == null) Log.Debug("客户端自定义数据包被丢弃", LogLevel.Debug | LogLevel.AutoCSer);
            else
            {
                try
                {
                    onCustomData(data);
                }
                catch (Exception error)
                {
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
            }
        }
#if NOJIT
        /// <summary>
        /// 自定义队列
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected IServerCallQueue getServerCallQueue<queueKeyType>()
        {
            return serverCallQueue.Get<queueKeyType>();
        }
#else
        /// <summary>
        /// 自定义队列
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected IServerCallQueue<queueKeyType> getServerCallQueue<queueKeyType>()
        {
            return serverCallQueue.Get<queueKeyType>();
        }
#endif
    }
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="serverType"></typeparam>
    /// <typeparam name="serverSocketSenderType"></typeparam>
    public abstract unsafe class Server<serverType, serverSocketSenderType> : Server
        where serverType : Server<serverType, serverSocketSenderType>
        where serverSocketSenderType : ServerSocketSender
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="serverCallQueue">自定义队列</param>
        /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="getSocketThreadCallType">同步验证接口</param>
        /// <param name="callQueueCount">独占的 TCP 服务器端同步调用队列数量</param>
        /// <param name="isCallQueueLink">是否提供独占的 TCP 服务器端同步调用队列（低优先级）</param>
        /// <param name="isSynchronousVerifyMethod">验证函数是否同步调用</param>
        internal Server(ServerBaseAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, AutoCSer.Net.TcpServer.IServerCallQueueSet serverCallQueue, byte extendCommandBits, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Threading.ThreadTaskType getSocketThreadCallType, int callQueueCount, bool isCallQueueLink, bool isSynchronousVerifyMethod)
            : base(attribute, verify, serverCallQueue, extendCommandBits, onCustomData, log, getSocketThreadCallType, callQueueCount, isCallQueueLink, isSynchronousVerifyMethod)
        {
        }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="index">命令序号</param>
        /// <param name="sender">TCP 内部服务套接字数据发送</param>
        /// <param name="data">命令数据</param>
        public abstract void DoCommand(int index, serverSocketSenderType sender, ref SubArray<byte> data);
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="index"></param>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="dataSize"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DoCommand(int index, serverSocketSenderType socket, ref SubBuffer.PoolBufferFull buffer, int dataSize)
        {
            SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = dataSize };
            DoCommand(index, socket, ref data);
            buffer.PoolBuffer.Free();
        }
    }
}
