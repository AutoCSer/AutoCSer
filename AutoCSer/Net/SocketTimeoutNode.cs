using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 套接字超时链表节点
    /// </summary>
    public abstract class SocketTimeoutNode
    {
        /// <summary>
        /// 超时秒数
        /// </summary>
        internal long TimeoutSeconds;
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
        internal SocketTimeoutNode NextTimeout;
        /// <summary>
        /// 上一个超时节点
        /// </summary>
        internal SocketTimeoutNode PreviousTimeout;
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
        internal byte IsSetReceiveTimeout;
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
            this.TimeoutSeconds = timeoutSeconds;
            timeoutSocket = socket;
        }
        /// <summary>
        /// 设置超时秒数
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <param name="socket"></param>
        /// <param name="timeoutCount"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetTimeout(long timeoutSeconds, Socket socket, ushort timeoutCount)
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
            this.TimeoutSeconds = timeoutSeconds;
            timeoutSocket = Socket;
        }
        /// <summary>
        /// 获取超时操作验证标识
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte GetTimeoutSocket(ref Socket socket)
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
        internal void WaitCancelTimeout()
        {
            while (IsSetReceiveTimeout == 0) AutoCSer.Threading.ThreadYield.Yield();
            IsSetReceiveTimeout = 0;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeTimeout()
        {
            if (NextTimeout != null)
            {
                PreviousTimeout.NextTimeout = NextTimeout;
                NextTimeout.PreviousTimeout = PreviousTimeout;
                PreviousTimeout = NextTimeout = null;
            }
        }
        /// <summary>
        /// 超时释放套接字
        /// </summary>
        /// <param name="socket"></param>
        internal virtual void TimeoutDisposeSocket(Socket socket)
        {
            if (Interlocked.CompareExchange(ref Socket, null, socket) == socket)
            {
                AutoCSer.Net.TcpServer.CommandBase.CloseServer(socket);
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
            if (socket != null) AutoCSer.Net.TcpServer.CommandBase.CloseServer(socket);
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
