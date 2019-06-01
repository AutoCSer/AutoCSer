using System;
using AutoCSer.Log;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务
    /// </summary>
    public static class Server
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
        /// JSON 序列化配置
        /// </summary>
        internal static readonly AutoCSer.Json.SerializeConfig JsonConfig;

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
        /// <summary>
        /// 添加 TCP 任务队列（不允许添加重复的任务实例，否则可能造成严重后果）
        /// </summary>
        /// <param name="call">TCP 服务器端同步调用</param>
        public static void AppendTcpQueue(ServerCallBase call)
        {
            if (call != null)
            {
                if (AutoCSer.Net.TcpServer.ServerCallQueue.Default.CheckAdd(call)) return;
                throw new InvalidOperationException();
            }
        }
        static Server()
        {
            JsonConfig = AutoCSer.Json.ConfigLoader.GetUnion(typeof(AutoCSer.Json.SerializeConfig)).SerializeConfig;
            if (JsonConfig == null) JsonConfig = AutoCSer.Json.SerializeConfig.CreateInternal();
            else JsonConfig = AutoCSer.MemberCopy.Copyer<AutoCSer.Json.SerializeConfig>.MemberwiseClone(JsonConfig);
            JsonConfig.IsInfinityToNaN = false;
        }
    }
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    public abstract unsafe class Server<attributeType> : ServerBase<attributeType>
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// 自定义数据包处理
        /// </summary>
        private readonly Action<SubArray<byte>> onCustomData;
        /// <summary>
        /// 获取客户端请求线程调用类型
        /// </summary>
        private readonly AutoCSer.Threading.Thread.CallType getSocketThreadCallType;

        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="getSocketThreadCallType">同步验证接口</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        internal Server(attributeType attribute, Func<System.Net.Sockets.Socket, bool> verify, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Threading.Thread.CallType getSocketThreadCallType, bool isCallQueue)
            : base(attribute, verify, log, isCallQueue)
        {
            this.onCustomData = onCustomData;
            this.getSocketThreadCallType = getSocketThreadCallType;
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
            if (onCustomData == null) Log.Add(AutoCSer.Log.LogType.Info, "客户端自定义数据包被丢弃");
            else
            {
                try
                {
                    onCustomData(data);
                }
                catch (Exception error)
                {
                    Log.Add(AutoCSer.Log.LogType.Error, error);
                }
            }
        }
    }
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    /// <typeparam name="serverType"></typeparam>
    /// <typeparam name="serverSocketSenderType"></typeparam>
    public abstract unsafe class Server<attributeType, serverType, serverSocketSenderType> : Server<attributeType>
        where serverType : Server<attributeType, serverType, serverSocketSenderType>
        where attributeType : ServerAttribute
        where serverSocketSenderType : ServerSocketSender
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="getSocketThreadCallType">同步验证接口</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        internal Server(attributeType attribute, Func<System.Net.Sockets.Socket, bool> verify, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Threading.Thread.CallType getSocketThreadCallType, bool isCallQueue)
            : base(attribute, verify, onCustomData, log, getSocketThreadCallType, isCallQueue)
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
