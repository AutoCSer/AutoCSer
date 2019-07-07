using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 套接字超时链表节点
    /// </summary>
    public abstract class SocketTimeoutLink
    {
        /// <summary>
        /// 超时秒数
        /// </summary>
        private long timeoutSeconds;
        /// <summary>
        /// 套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// 超时操作验证标识
        /// </summary>
        private Socket timeoutSocket;
        /// <summary>
        /// 下一个超时节点
        /// </summary>
        private SocketTimeoutLink nextTimeout;
        /// <summary>
        /// 上一个超时节点
        /// </summary>
        private SocketTimeoutLink previousTimeout;
        ///// <summary>
        ///// 超时是否调用 Shutdown，否则直接 Dispose
        ///// </summary>
        //protected bool isShutdown;
        /// <summary>
        /// 超时计数
        /// </summary>
        private ushort timeoutCount;
        /// <summary>
        /// 是否设置了超时
        /// </summary>
        private byte isSetReceiveTimeout;
        /// <summary>
        /// 接收数据量过低次数
        /// </summary>
        internal byte ReceiveSizeLessCount;
        /// <summary>
        /// 设置超时秒数
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setTimeout(long timeoutSeconds, Socket socket)
        {
            this.timeoutSeconds = timeoutSeconds;
            timeoutSocket = socket;
        }
        /// <summary>
        /// 设置超时秒数
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <param name="socket"></param>
        /// <param name="timeoutCount"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setTimeout(long timeoutSeconds, Socket socket, ushort timeoutCount)
        {
            setTimeout(timeoutSeconds, socket);
            this.timeoutCount = timeoutCount;
        }
        /// <summary>
        /// 设置超时秒数
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setTimeout(long timeoutSeconds)
        {
            this.timeoutSeconds = timeoutSeconds;
            timeoutSocket = Socket;
        }
        /// <summary>
        /// 获取超时操作验证标识
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private byte getTimeoutSocket(ref Socket socket)
        {
            if (timeoutCount == 0)
            {
                socket = timeoutSocket;
                timeoutSocket = null;
                return 0;
            }
            --timeoutCount;
            return 1;
        }
        /// <summary>
        /// 等待取消超时
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void waitCancelTimeout()
        {
            while (isSetReceiveTimeout == 0) AutoCSer.Threading.ThreadYield.Yield(Threading.ThreadYield.Type.SocketTimeoutLinkCancelTimeout);
            isSetReceiveTimeout = 0;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeTimeout()
        {
            if (nextTimeout != null)
            {
                previousTimeout.nextTimeout = nextTimeout;
                nextTimeout.previousTimeout = previousTimeout;
                previousTimeout = nextTimeout = null;
            }
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DisposeSocket(Socket socket)
        {
            //bool isShutdown = this.isShutdown;
            if (Interlocked.CompareExchange(ref Socket, null, socket) == socket)
            {
                //if (isShutdown)
                //{
                //    try
                //    {
                //        socket.Shutdown(SocketShutdown.Both);
                //    }
                //    catch { AutoCSer.Log.CatchCount.Add(Log.CatchCount.Type.SocketTimeoutLink_Dispose); }
                //    finally { socket.Dispose(); }
                //}
                //else
#if DotNetStandard
                AutoCSer.Net.TcpServer.CommandBase.CloseServer(socket);
#else
                socket.Dispose();
#endif
            }
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DisposeSocket()
        {
            Socket socket = Socket;
            Socket = null;
            if (socket != null)
            {
#if DotNetStandard
                AutoCSer.Net.TcpServer.CommandBase.CloseServer(socket);
#else
                socket.Dispose();
#endif
            }
        }
        ///// <summary>
        ///// 释放套接字
        ///// </summary>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void TryDisposeSocket()
        //{
        //    Socket socket = Socket;
        //    bool isShutdown = this.isShutdown;
        //    Socket = null;
        //    if (socket != null)
        //    {
        //        try
        //        {
        //            using (socket)
        //            {
        //                if (isShutdown) socket.Shutdown(SocketShutdown.Both);
        //            }
        //        }
        //        catch
        //        {
        //            AutoCSer.Log.CatchCount.Add(AutoCSer.Log.CatchCount.Type.SocketTimeoutLink_Dispose);
        //        }
        //    }
        //}

        /// <summary>
        /// 套接字超时
        /// </summary>
        internal sealed class TimerLink : AutoCSer.Threading.TimerLink<TimerLink>
        {
            /// <summary>
            /// 链表首节点
            /// </summary>
            internal SocketTimeoutLink Head;
            /// <summary>
            /// 链表尾部
            /// </summary>
            internal SocketTimeoutLink End;
            /// <summary>
            /// 链表访问锁
            /// </summary>
            private int queueLock;
            /// <summary>
            /// 定时器双向链表节点
            /// </summary>
            /// <param name="seconds">超时秒数</param>
            private TimerLink(int seconds)
                : base(seconds)
            {
                set(this);
            }
            ///// <summary>
            ///// 添加超时套接字
            ///// </summary>
            ///// <param name="value"></param>
            //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            //internal void Push(SocketTimeoutLink value)
            //{
            //    value.setTimeout(currentSeconds + seconds);
            //    push(value);
            //}
            /// <summary>
            /// 添加超时套接字
            /// </summary>
            /// <param name="value"></param>
            /// <param name="socket"></param>
            /// <param name="count"></param>
            internal void Push(SocketTimeoutLink value, Socket socket, ushort count = 0)
            {
                value.setTimeout(currentSeconds + seconds, socket, count);
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePush);
                if (End == null)
                {
                    End = Head = value;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                else
                {
                    End.nextTimeout = value;
                    value.previousTimeout = End;
                    End = value;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                value.isSetReceiveTimeout = 1;
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <param name="value"></param>
            internal void Cancel(SocketTimeoutLink value)
            {
                value.waitCancelTimeout();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePop);
                if (value == End)
                {
                    if ((End = value.previousTimeout) == null)
                    {
                        Head = null;
                        System.Threading.Interlocked.Exchange(ref queueLock, 0);
                    }
                    else
                    {
                        End.nextTimeout = value.previousTimeout = null;
                        System.Threading.Interlocked.Exchange(ref queueLock, 0);
                    }
                }
                else if (value == Head)
                {
                    Head = value.nextTimeout;
                    Head.previousTimeout = value.nextTimeout = null;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                else
                {
                    value.freeTimeout();
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
                SocketTimeoutLink head = Head;
                if (head != null && head.timeoutSeconds <= currentSeconds && Interlocked.CompareExchange(ref isTimer, 1, 0) == 0)
                {
                    onTimer();
                    System.Threading.Interlocked.Exchange(ref isTimer, 0);
                }
            }
            /// <summary>
            /// 定时器触发
            /// </summary>
            private void onTimer()
            {
                do
                {
                    try
                    {
                        do
                        {
                            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimerLinkQueuePop);
                            Socket socket = null;
                            if (Head == null || Head.timeoutSeconds > currentSeconds)
                            {
                                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                                return;
                            }
                            SocketTimeoutLink value = Head;
                            if (Head.getTimeoutSocket(ref socket) == 0)
                            {
                                if ((Head = Head.nextTimeout) == null)
                                {
                                    End = null;
                                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                                }
                                else
                                {
                                    value.nextTimeout = null;
                                    Head.previousTimeout = null;
                                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                                }
                                value.DisposeSocket(socket);
                            }
                            else
                            {
                                value.timeoutSeconds = currentSeconds + seconds;
                                if ((Head = Head.nextTimeout) == null)
                                {
                                    Head = value;
                                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                                }
                                else
                                {
                                    value.previousTimeout = End;
                                    End.nextTimeout = value;
                                    value.nextTimeout = null;
                                    Head.previousTimeout = null;
                                    End = value;
                                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                                }
                            }
                        }
                        while (true);
                    }
                    catch
                    {
                        AutoCSer.Log.CatchCount.Add(AutoCSer.Log.CatchCount.Type.SocketTimeoutLink_Dispose);
                    }
                }
                while (true);
            }

            /// <summary>
            /// 获取套接字超时
            /// </summary>
            /// <param name="seconds"></param>
            /// <returns></returns>
            internal static TimerLink Get(int seconds)
            {
                ++seconds;
                Monitor.Enter(timeoutLock);
                TimerLink timeout = TimeoutEnd;
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
                    timeout = new TimerLink(seconds);
                }
                finally { Monitor.Exit(timeoutLock); }
                Date.NowTime.Flag |= Date.NowTime.OnTimeFlag.TcpServerSocketTimerLink;
                return timeout;
            }
        }
    }
    ///// <summary>
    ///// 套接字超时链表节点
    ///// </summary>
    ///// <typeparam name="valueType"></typeparam>
    //public abstract class SocketTimeoutLink<valueType> : SocketTimeoutLink
    //    where valueType : SocketTimeoutLink<valueType>
    //{
    //    /// <summary>
    //    /// 下一个节点
    //    /// </summary>
    //    private valueType poolNext;
    //    /// <summary>
    //    /// 链表
    //    /// </summary>
    //    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    //    internal struct YieldPoolLink
    //    {
    //        /// <summary>
    //        /// 链表头部
    //        /// </summary>
    //        private valueType head;
    //        /// <summary>
    //        /// 弹出节点访问锁
    //        /// </summary>
    //        private int popLock;
    //        /// <summary>
    //        /// 添加节点
    //        /// </summary>
    //        /// <param name="value"></param>
    //        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //        internal void PushNotNull(valueType value)
    //        {
    //            valueType headValue;
    //            do
    //            {
    //                if ((headValue = head) == null)
    //                {
    //                    value.poolNext = null;
    //                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
    //                }
    //                else
    //                {
    //                    value.poolNext = headValue;
    //                    if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
    //                }
    //                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
    //            }
    //            while (true);
    //        }
    //        /// <summary>
    //        /// 弹出节点
    //        /// </summary>
    //        /// <returns></returns>
    //        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //        public valueType Pop()
    //        {
    //            valueType headValue;
    //            AutoCSer.Threading.Interlocked.CompareExchangeYield(ref popLock, AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
    //            do
    //            {
    //                if ((headValue = head) == null)
    //                {
    //                    AutoCSer.Threading.Interlocked.popLock = 0;
    //                    return null;
    //                }
    //                if (System.Threading.Interlocked.CompareExchange(ref head, headValue.poolNext, headValue) == headValue)
    //                {
    //                    AutoCSer.Threading.Interlocked.popLock = 0;
    //                    headValue.poolNext = null;
    //                    return headValue;
    //                }
    //                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
    //            }
    //            while (true);
    //        }
    //        /// <summary>
    //        /// 清除缓存数据
    //        /// </summary>
    //        /// <param name="count">保留缓存数据数量</param>
    //        internal void ClearCache(int count)
    //        {
    //            valueType head = Interlocked.Exchange(ref this.head, null);
    //            if (head != null && count != 0)
    //            {
    //                valueType end = head;
    //                while (--count != 0)
    //                {
    //                    if (end.poolNext == null)
    //                    {
    //                        PushLink(head, end);
    //                        return;
    //                    }
    //                    end = end.poolNext;
    //                }
    //                end.poolNext = null;
    //                PushLink(head, end);
    //            }
    //        }
    //        /// <summary>
    //        /// 添加链表
    //        /// </summary>
    //        /// <param name="value">链表头部</param>
    //        /// <param name="end">链表尾部</param>
    //        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //        internal void PushLink(valueType value, valueType end)
    //        {
    //            valueType headValue;
    //            do
    //            {
    //                if ((headValue = head) == null)
    //                {
    //                    end.poolNext = null;
    //                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
    //                }
    //                else
    //                {
    //                    end.poolNext = headValue;
    //                    if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
    //                }
    //                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
    //            }
    //            while (true);
    //        }
    //    }
    //    /// <summary>
    //    /// 链表节点池
    //    /// </summary>
    //    internal static YieldLink Pool;
    //}
}
