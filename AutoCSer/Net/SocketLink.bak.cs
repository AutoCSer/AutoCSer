using System;
using System.Net.Sockets;

namespace fastCSharp.Net
{
    /// <summary>
    /// 套接字链表
    /// </summary>
    internal sealed class SocketLink : fastCSharp.Threading.Link<SocketLink>
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// 套接字计数
        /// </summary>
        internal int Count;
        /// <summary>
        /// 超时是否调用 Shutdown，否则直接 Dispose
        /// </summary>
        internal bool IsShutdown;
        /// <summary>
        /// 设置释放套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isShutdown"></param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        private void set(Socket socket, bool isShutdown)
        {
            Socket = socket;
            IsShutdown = isShutdown;
        }
        /// <summary>
        /// 释放字链表
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        private void dispose()
        {
            Socket socket = Socket;
            Socket = null;
            if (IsShutdown) socket.Shutdown(SocketShutdown.Both);
            socket.Dispose();
        }
        /// <summary>
        /// 设置套接字计数
        /// </summary>
        /// <param name="socket"></param>
        internal void SetCount(Socket socket)
        {
            Socket = socket;
            Count = 1;
        }
        /// <summary>
        /// 新增套接字
        /// </summary>
        /// <param name="maxActiveCount"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal bool NewCount(int maxActiveCount)
        {
            if (Count < maxActiveCount)
            {
                ++Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 新增套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="maxClient"></param>
        /// <returns></returns>
        internal bool NewCount(ref Socket socket, int maxClient)
        {
            if (Count < maxClient)
            {
                SocketLink link = YieldPool.Default.Pop() ?? new SocketLink();
                link.Socket = socket;
                Next = link;
                ++Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取下一个套接字
        /// </summary>
        /// <returns></returns>
        internal SocketLink GetNextCount()
        {
            if (Next == null) return --Count == 0 ? this : null;
            SocketLink next = Next;
            Next = Next.Next;
            --Count;
            next.Next = null;
            return next;
        }

        /// <summary>
        /// 垃圾清理
        /// </summary>
        private static YieldLink disposeLink;
        /// <summary>
        /// 添加垃圾清理任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isShutdown"></param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static void AddDispose(ref Socket socket, bool isShutdown)
        {
            SocketLink link = YieldPool.Default.Pop() ?? new SocketLink();
            link.set(socket, isShutdown);
            disposeLink.PushNotNull(link);
            socket = null;
        }
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary>
        private static int isDisposeTimer;
        /// <summary>
        /// 触发定时任务
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static void OnDisposeTimer()
        {
            if (!disposeLink.IsEmpty && System.Threading.Interlocked.CompareExchange(ref isDisposeTimer, 1, 0) == 0)
            {
                onDisposeTimer();
                isDisposeTimer = 0;
            }
        }
        /// <summary>
        /// 触发定时任务
        /// </summary>
        private static void onDisposeTimer()
        {
            SocketLink head = null, end = null;
            do
            {
                try
                {
                    do
                    {
                        SocketLink link = disposeLink.SinglePop();
                        if (link == null)
                        {
                            if (head == end)
                            {
                                if (end != null) YieldPool.Default.PushNotNull(end);
                            }
                            else YieldPool.Default.PushLink(head, end);
                            return;
                        }
                        link.dispose();
                        if (end == null) head = end = link;
                        else
                        {
                            end.Next = link;
                            end = link;
                        }
                    }
                    while (true);
                }
                catch
                {
                    fastCSharp.Log.CatchCount.Add(fastCSharp.Log.CatchCount.Type.SocketLink_Dispose);
                }
            }
            while (true);
        }
    }
}
