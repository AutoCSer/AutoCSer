using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 套接字超时链表
    /// </summary>
    internal sealed class SocketTimeoutLink : AutoCSer.Threading.SecondTimerNode
    {
        /// <summary>
        /// 超时秒数
        /// </summary>
        private readonly int seconds;
        /// <summary>
        /// 链表首节点
        /// </summary>
        internal SocketTimeoutNode Head;
        /// <summary>
        /// 链表尾部
        /// </summary>
        internal SocketTimeoutNode End;
        /// <summary>
        /// 链表访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// 定时器双向链表节点
        /// </summary>
        /// <param name="seconds">超时秒数</param>
        internal SocketTimeoutLink(int seconds)
        {
            this.seconds = seconds + 1;
            AutoCSer.Threading.SecondTimer.InternalTaskArray.NodeLink.PushNotNull(this);
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
        internal void Push(SocketTimeoutNode value, Socket socket, ushort count = 0)
        {
            value.SetTimeout(AutoCSer.Threading.SecondTimer.CurrentSeconds + seconds, socket, count);
            queueLock.EnterYield();
            if (End == null)
            {
                End = Head = value;
                queueLock.Exit();
            }
            else
            {
                End.NextTimeout = value;
                value.PreviousTimeout = End;
                End = value;
                queueLock.Exit();
            }
            value.IsSetReceiveTimeout = 1;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <param name="value"></param>
        internal void Cancel(SocketTimeoutNode value)
        {
            value.WaitCancelTimeout();
            queueLock.EnterYield();
            if (value == End)
            {
                if ((End = value.PreviousTimeout) == null)
                {
                    Head = null;
                    queueLock.Exit();
                }
                else
                {
                    End.NextTimeout = value.PreviousTimeout = null;
                    queueLock.Exit();
                }
            }
            else if (value == Head)
            {
                Head = value.NextTimeout;
                Head.PreviousTimeout = value.NextTimeout = null;
                queueLock.Exit();
            }
            else
            {
                value.FreeTimeout();
                queueLock.Exit();
            }
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            SocketTimeoutNode head = Head;
            if (head != null && head.TimeoutSeconds <= AutoCSer.Threading.SecondTimer.CurrentSeconds)
            {
                do
                {
                    try
                    {
                        do
                        {
                            queueLock.EnterYield();
                            Socket socket = null;
                            if (Head == null || Head.TimeoutSeconds > AutoCSer.Threading.SecondTimer.CurrentSeconds)
                            {
                                queueLock.Exit();
                                return;
                            }
                            SocketTimeoutNode value = Head;
                            if (Head.GetTimeoutSocket(ref socket) == 0)
                            {
                                if ((Head = Head.NextTimeout) == null)
                                {
                                    End = null;
                                    queueLock.Exit();
                                }
                                else
                                {
                                    value.NextTimeout = null;
                                    Head.PreviousTimeout = null;
                                    queueLock.Exit();
                                }
                                value.TimeoutDisposeSocket(socket);
                            }
                            else
                            {
                                value.TimeoutSeconds = AutoCSer.Threading.SecondTimer.CurrentSeconds + seconds;
                                if ((Head = Head.NextTimeout) == null)
                                {
                                    Head = value;
                                    queueLock.Exit();
                                }
                                else
                                {
                                    value.PreviousTimeout = End;
                                    End.NextTimeout = value;
                                    value.NextTimeout = null;
                                    Head.PreviousTimeout = null;
                                    End = value;
                                    queueLock.Exit();
                                }
                            }
                        }
                        while (true);
                    }
                    catch(Exception error)
                    {
                        AutoCSer.LogHelper.Exception(error, null, LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
                    }
                }
                while (true);
            }
        }

        /// <summary>
        /// 释放套接字超时
        /// </summary>
        /// <param name="timeout"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Free(ref SocketTimeoutLink timeout)
        {
            if (timeout != null)
            {
                AutoCSer.Threading.SecondTimer.InternalTaskArray.NodeLink.PopNotNull(timeout);
                timeout = null;
            }
        }
    }
}
