using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端套接字数据发送
    /// </summary>
    public abstract class ClientSocketSenderBase : Sender
    {
        /// <summary>
        /// TCP 服务客户端创建器
        /// </summary>
        protected readonly ClientSocketCreator clientCreator;
        /// <summary>
        /// 发送变换数据
        /// </summary>
        internal ulong SendMarkData;
        /// <summary>
        /// 客户端最大未处理命令数量
        /// </summary>
        protected readonly int queueCommandSize;
        /// <summary>
        /// 当前发送命令数量
        /// </summary>
        protected volatile int buildCommandCount;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal AutoCSer.Threading.AutoWaitHandle OutputWaitHandle;
        /// <summary>
        /// 套接字发送数据次数
        /// </summary>
        internal int SendCount { get { return OutputWaitHandle.Reserved; } }
        /// <summary>
        /// 当前队列命令数量
        /// </summary>
        protected int commandCount;
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal ClientSocketSenderBase() : base(null) { }
#endif
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务客户端套接字</param>
        internal ClientSocketSenderBase(ClientSocketBase socket)
            : base(socket.Socket)
        {
            clientCreator = socket.ClientCreator;

            this.queueCommandSize = Math.Max(socket.ClientCreator.Attribute.GetQueueCommandSize, 1);
            OutputWaitHandle.Set(0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Close()
        {
            if (!isClose)
            {
                isClose = true;
                OutputWaitHandle.Set();
            }
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        internal abstract void Check();
        /// <summary>
        /// 创建输出
        /// </summary>
        internal virtual void VirtualBuildOutput()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 增加当前发送命令数量
        /// </summary>
        /// <param name="buildCount"></param>
        protected void addBuildCommandCount(int buildCount)
        {
            if (buildCount < queueCommandSize)
            {
                if ((buildCommandCount += buildCount) >= queueCommandSize)
                {
                    Interlocked.Add(ref commandCount, -queueCommandSize);
                    buildCommandCount -= queueCommandSize;
                }
            }
            else
            {
                int oldBuildCount = buildCount;
                do
                {
                    buildCount -= queueCommandSize;
                }
                while (buildCount >= queueCommandSize);

                if ((buildCommandCount += buildCount) < queueCommandSize) Interlocked.Add(ref commandCount, buildCount - oldBuildCount);
                else
                {
                    Interlocked.Add(ref commandCount, buildCount - oldBuildCount - queueCommandSize);
                    buildCommandCount -= queueCommandSize;
                }
            }
        }

        /// <summary>
        /// 默认空回调委托
        /// </summary>
        /// <param name="value"></param>
        private static void nullCallback(ReturnValue value) { }
        /// <summary>
        /// 默认空回调委托
        /// </summary>
        internal static readonly Action<ReturnValue> NullCallback = nullCallback;
    }
}
