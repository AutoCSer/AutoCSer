using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端心跳检测定时
    /// </summary>
    internal sealed class ClientCheckTimer : AutoCSer.Threading.TimerLink<ClientCheckTimer>
    {
        /// <summary>
        /// 链表首节点
        /// </summary>
        internal ClientSocketBase Head;
        /// <summary>
        /// 链表尾部
        /// </summary>
        internal ClientSocketBase End;
        /// <summary>
        /// 链表访问锁
        /// </summary>
        private int queueLock;
        /// <summary>
        /// 客户端心跳检测定时
        /// </summary>
        /// <param name="seconds">超时秒数</param>
        private ClientCheckTimer(int seconds)
            : base(seconds)
        {
            set(this);
        }
        /// <summary>
        /// 添加心跳检测
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Push(ClientSocketBase value)
        {
            value.CheckTimeoutSeconds = currentSeconds + seconds;
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePush);
            if (End == null)
            {
                End = Head = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
            else
            {
                End.CheckNext = value;
                value.CheckPrevious = End;
                End = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 释放心跳检测
        /// </summary>
        /// <param name="value"></param>
        internal void Free(ClientSocketBase value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePop);
            if (value == Head)
            {
                if ((Head = value.CheckNext) == null)
                {
                    End = null;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                else
                {
                    value.CheckNext = null;
                    Head.CheckPrevious = null;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
            }
            else if (value == End)
            {
                End = value.CheckPrevious;
                value.CheckPrevious = null;
                End.CheckNext = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
            else
            {
                if (value.CheckNext != null) value.FreeCheck();
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <param name="currentSecond"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private ClientSocketBase pop(long currentSecond)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePop);
            if (Head == null || Head.CheckTimeoutSeconds > currentSecond)
            {
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                return null;
            }
            ClientSocketBase value = Head;
            if ((Head = Head.CheckNext) == null)
            {
                End = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
            else
            {
                value.CheckNext = null;
                Head.CheckPrevious = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
            return value;
        }
        /// <summary>
        /// 重置心跳检测
        /// </summary>
        /// <param name="value"></param>
        internal void Reset(ClientSocketBase value)
        {
            long newSeconds = currentSeconds + seconds;
            if (value.CheckTimeoutSeconds != newSeconds)
            {
                value.CheckTimeoutSeconds = newSeconds;
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePush);
                if (value != End)
                {
                    if (value.CheckNext == null)
                    {
                        if (End == null) Head = value;
                        else
                        {
                            End.CheckNext = value;
                            value.CheckPrevious = End;
                        }
                    }
                    else
                    {
                        if (value == Head)
                        {
                            (Head = value.CheckNext).CheckPrevious = null;
                            value.CheckNext = null;
                        }
                        else value.FreeCheckReset();
                        End.CheckNext = value;
                        value.CheckPrevious = End;
                    }
                    End = value;
                }
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 激活计时器
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly DateTime timer = Date.NowTime.Now;
        /// <summary>
        /// 定时器触发
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer()
        {
            ++currentSeconds;
            ClientSocketBase head = Head;
            if (head != null && head.CheckTimeoutSeconds <= currentSeconds && Interlocked.CompareExchange(ref isTimer, 1, 0) == 0)
            {
                do
                {
                    if ((head = pop(currentSeconds)) == null)
                    {
                        System.Threading.Interlocked.Exchange(ref isTimer, 0);
                        return;
                    }
                    head.Check();
                }
                while (true);
              
            }
        }

        /// <summary>
        /// 获取客户端心跳检测定时
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        internal static ClientCheckTimer Get(int seconds)
        {
            if (seconds <= 0) seconds = 1;
            Monitor.Enter(timeoutLock);
            ClientCheckTimer timeout = TimeoutEnd;
            while (timeout != null)
            {
                if (timeout.seconds == seconds)
                {
                    ++timeout.count;
                    Monitor.Exit(timeoutLock);
                    return timeout;
                }
                timeout = timeout.DoubleLinkPrevious;
            }
            try
            {
                timeout = new ClientCheckTimer(seconds);
            }
            finally { Monitor.Exit(timeoutLock); }
            Date.NowTime.Flag |= Date.NowTime.OnTimeFlag.TcpClientCheckTimer;
            return timeout;
        }
    }
}
