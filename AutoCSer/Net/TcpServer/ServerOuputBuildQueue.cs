using System;
using System.Runtime.CompilerServices;
using AutoCSer.Net.TcpServer.ServerOutput;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 服务端输出队列
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal sealed class ServerOuputBuildQueue
    {
        /// <summary>
        /// 服务端输出队列头部节点
        /// </summary>
        private sealed class OutputHead : ServerOutput.OutputLink
        {
            /// <summary>
            /// 创建输出信息
            /// </summary>
            /// <param name="sender">TCP 服务套接字数据发送</param>
            /// <param name="buildInfo">输出创建参数</param>
            /// <returns></returns>
            internal override int Build(ServerSocketSender sender, ref SenderBuildInfo buildInfo)
            {
                throw new InvalidOperationException();
            }
            /// <summary>
            /// 释放 TCP 服务端套接字输出信息
            /// </summary>
            /// <returns></returns>
            internal override OutputLink TryFreeBuildQueue()
            {
                OutputLink next = LinkNext;
                LinkNext = null;
                return next;
            }
        }
        private readonly ulong pad0, pad1, pad2, pad3, pad4, pad5, pad6;
        /// <summary>
        /// 服务端输出队列头部节点
        /// </summary>
        private readonly OutputHead Head;
        /// <summary>
        /// 队列头部
        /// </summary>
        private ServerOutput.OutputLink head;
        private readonly ulong pad10, pad11, pad12, pad13, pad14, pad15, pad16;
        /// <summary>
        /// 队列尾部
        /// </summary>
        private ServerOutput.OutputLink end;
        /// <summary>
        /// 添加数据访问锁
        /// </summary>
        private int pushLock;
        /// <summary>
        /// 是否正在输出
        /// </summary>
        internal int IsOutput;
        private readonly ulong pad20, pad21, pad22, pad23, pad24, pad25, pad26;
        /// <summary>
        /// 单线程读队列
        /// </summary>
        /// <param name="free">空闲节点</param>
        internal ServerOuputBuildQueue()
        {
            head = end = Head = new OutputHead();
            Head.LinkNext = null;
        }
        /// <summary>
        /// 添加输出
        /// </summary>
        /// <param name="output"></param>
        /// <returns>是否正在输出</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Push(ServerOutput.OutputLink output)
        {
            AutoCSer.Threading.Interlocked.CompareExchangeYieldOnly(ref pushLock);
            int IsOutput = this.IsOutput;
            end.LinkNext = output;
            this.IsOutput = 1;
            end = output;
            pushLock = 0;
            return IsOutput;
        }
        /// <summary>
        /// 弹出输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerOutput.OutputLink Pop()
        {
            ServerOutput.OutputLink value = head.TryFreeBuildQueue();
            if (value != null)
            {
                head = value;
                return value;
            }
            return null;
        }
        /// <summary>
        /// 设置队列节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PushHead()
        {
            Head.LinkNext = head;
            head = Head;
        }
        /// <summary>
        /// 设置是否正在输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int TryOutputEnd()
        {
            if (head.LinkNext == null)
            {
                AutoCSer.Threading.Interlocked.CompareExchangeYieldOnly(ref pushLock);
                if (head.LinkNext == null)
                {
                    IsOutput = 0;
                    pushLock = 0;
                    return 1;
                }
                pushLock = 0;
            }
            return 0;
        }
        /// <summary>
        /// 设置是否正在输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int TryOutput()
        {
            AutoCSer.Threading.Interlocked.CompareExchangeYieldOnly(ref pushLock);
            int IsOutput = this.IsOutput;
            this.IsOutput = 1;
            pushLock = 0;
            return IsOutput;
        }
    }
}
