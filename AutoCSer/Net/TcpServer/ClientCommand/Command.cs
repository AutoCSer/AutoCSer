using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// TCP 客户端命令
    /// </summary>
    internal abstract class CommandBase : AutoCSer.Threading.Link<CommandBase>, AutoCSer.Threading.ILinkTask
    {
        /// <summary>
        /// 输出流起始位置
        /// </summary>
        internal const int StreamStartIndex = sizeof(uint) + sizeof(int);

        /// <summary>
        /// TCP调用命令
        /// </summary>
        internal CommandInfo CommandInfo;
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>下一个命令</returns>
        internal virtual CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long TaskTimestamp;
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        long AutoCSer.Threading.ILinkTask.LinkTaskTimestamp
        {
            get { return TaskTimestamp; }
            set { TaskTimestamp = value; }
        }
        /// <summary>
        /// 下一个任务
        /// </summary>
        internal CommandBase NextTask;
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        AutoCSer.Threading.ILinkTask AutoCSer.Threading.ILinkTask.NextLinkTask
        {
            get { return NextTask; }
            set { NextTask = new UnionType { Value = value  }.ClientCommandBase; }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SingleRunLinkTask(ref AutoCSer.Threading.ILinkTask next)
        {
            next = NextTask;
            NextTask = null;
            onReceiveTask();
        }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        internal virtual void OnReceive(ref SubArray<byte> data)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 接收数据回调处理任务
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnReceiveTask(ref CommandBase next)
        {
            next = NextTask;
            NextTask = null;
            onReceiveTask();
        }
        /// <summary>
        /// 接收数据回调处理任务
        /// </summary>
        /// <param name="next"></param>
        /// <param name="currentTaskTimestamp"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnReceiveTask(ref CommandBase next, ref long currentTaskTimestamp)
        {
            next = NextTask;
            currentTaskTimestamp = TaskTimestamp;
            NextTask = null;
            onReceiveTask();
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        internal virtual void onReceiveTask()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 超时处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Timeout()
        {
            SubArray<byte> data = new SubArray<byte> { Start = (int)(byte)ReturnType.Timeout };
            OnReceive(ref data);
        }

        /// <summary>
        /// 获取返回值类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnType GetReturnType(ref SubArray<byte> data)
        {
            if (data.Array == null)
            {
                uint value = (uint)data.Start;
                if ((value & 0x7fffff00) == 0)
                {
                    ReturnType type = (ReturnType)(byte)value;
                    if (type != ReturnType.ClientDeSerializeError) return type;
                }
                return ReturnType.ClientNullData;
            }
            return ReturnType.ClientDeSerializeError;
        }
        /// <summary>
        /// 取消命令调用
        /// </summary>
        /// <param name="head"></param>
        /// <param name="returnType"></param>
        internal static void CancelLink(CommandBase head, ReturnType returnType = ReturnType.ClientDisposed)
        {
            SubArray<byte> data = new SubArray<byte> { Start = (int)(byte)returnType };
            CommandBase next = null;
            do
            {
                try
                {
                    do
                    {
                        next = head.GetLinkNextClear();
                        head.OnReceive(ref data);
                        if (next == null) return;
                        head = next;
                    }
                    while (true);
                }
                catch { }
                if (next == null) return;
                head = next;
            }
            while (true);
        }
    }
    /// <summary>
    /// TCP 客户端命令
    /// </summary>
    internal abstract class Command : CommandBase
    {
        /// <summary>
        /// TCP 客户端套接字
        /// </summary>
        internal ClientSocket Socket;
        ///// <summary>
        ///// 终止保持回调
        ///// </summary>
        ///// <param name="identity">保持回调序号</param>
        //internal virtual void CancelKeep(int identity)
        //{
        //    throw new InvalidOperationException();
        //}
        /// <summary>
        /// 终止保持回调
        /// </summary>
        /// <param name="commandIndex">命令会话标识</param>
        internal virtual void CancelKeep(int commandIndex)
        {
            throw new InvalidOperationException();
        }
    }
}
