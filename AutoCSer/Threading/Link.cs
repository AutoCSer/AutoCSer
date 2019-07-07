using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public abstract partial class Link<valueType>
        where valueType : Link<valueType>
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal valueType LinkNext;
        /// <summary>
        /// 获取并清除下一个节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType GetLinkNextClear()
        {
            valueType value = LinkNext;
            LinkNext = null;
            return value;
        }
        /// <summary>
        /// 缓存对象链表（用于冲突概率低的场景）
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public struct YieldPoolLink
        {
            /// <summary>
            /// 缓存数量
            /// </summary>
            private readonly static int maxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(valueType));
            /// <summary>
            /// 是否需要释放资源
            /// </summary>
            private readonly static bool isDisponse = typeof(IDisposable).IsAssignableFrom(typeof(valueType));
            /// <summary>
            /// 链表头部
            /// </summary>
            private valueType head;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private int popLock;
            /// <summary>
            /// 缓存数量
            /// </summary>
            private int count;
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            internal void PushNotNull(valueType value)
            {
                if (count >= maxCount)
                {
                    if (isDisponse) ((IDisposable)value).Dispose();
                    return;
                }
                System.Threading.Interlocked.Increment(ref count);
                valueType headValue;
                do
                {
                    if ((headValue = head) == null)
                    {
                        value.LinkNext = null;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                    }
                    else
                    {
                        value.LinkNext = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
                    }
                    ThreadYield.Yield(ThreadYield.Type.YieldLinkPush);
                }
                while (true);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            internal int IsPushNotNull(valueType value)
            {
                if (count >= maxCount) return 0;
                System.Threading.Interlocked.Increment(ref count);
                valueType headValue;
                do
                {
                    if ((headValue = head) == null)
                    {
                        value.LinkNext = null;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return 1;
                    }
                    else
                    {
                        value.LinkNext = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return 1;
                    }
                    ThreadYield.Yield(ThreadYield.Type.YieldLinkPush);
                }
                while (true);
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <returns></returns>
            public valueType Pop()
            {
                valueType headValue;
                while (System.Threading.Interlocked.CompareExchange(ref popLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
                do
                {
                    if ((headValue = head) == null)
                    {
                        System.Threading.Interlocked.Exchange(ref popLock, 0);
                        return null;
                    }
                    if (System.Threading.Interlocked.CompareExchange(ref head, headValue.LinkNext, headValue) == headValue)
                    {
                        System.Threading.Interlocked.Exchange(ref popLock, 0);
                        System.Threading.Interlocked.Decrement(ref count);
                        headValue.LinkNext = null;
                        return headValue;
                    }
                    ThreadYield.Yield(ThreadYield.Type.YieldLinkPop);
                }
                while (true);
            }
            /// <summary>
            /// 释放列表
            /// </summary>
            /// <param name="value"></param>
            private void disposeLink(valueType value)
            {
                do
                {
                    ((IDisposable)value).Dispose();
                }
                while ((value = value.LinkNext) != null);
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            internal void ClearCache(int count)
            {
                valueType headValue = System.Threading.Interlocked.Exchange(ref head, null);
                this.count = 0;
                if (headValue != null)
                {
                    if (count == 0)
                    {
                        if (isDisponse) disposeLink(headValue);
                    }
                    else
                    {
                        int pushCount = count;
                        valueType end = headValue;
                        while (--count != 0)
                        {
                            if (end.LinkNext == null)
                            {
                                PushLink(headValue, end, pushCount - count);
                                return;
                            }
                            end = end.LinkNext;
                        }
                        valueType next = end.LinkNext;
                        end.LinkNext = null;
                        PushLink(headValue, end, pushCount);
                        if (isDisponse && next != null) disposeLink(next);
                    }
                }
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="value">链表头部</param>
            /// <param name="end">链表尾部</param>
            /// <param name="count">数据数量</param>
            internal void PushLink(valueType value, valueType end, int count)
            {
                System.Threading.Interlocked.Add(ref this.count, count);
                valueType headValue;
                do
                {
                    if ((headValue = head) == null)
                    {
                        end.LinkNext = null;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                    }
                    else
                    {
                        end.LinkNext = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
                    }
                    ThreadYield.Yield(ThreadYield.Type.YieldLinkPush);
                }
                while (true);
            }
        }
        /// <summary>
        /// 链表节点池
        /// </summary>
        public static class YieldPool
        {
            /// <summary>
            /// 链表节点池
            /// </summary>
            public static YieldPoolLink Default;
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            private static void clearCache(int count)
            {
                Default.ClearCache(count);
            }

            static YieldPool()
            {
                AutoCSer.Pub.ClearCaches += clearCache;
            }
        }
    }
}
