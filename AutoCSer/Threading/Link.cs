using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class Link<T>
        where T : Link<T>
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal T LinkNext;
        /// <summary>
        /// 获取并清除下一个节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T GetLinkNextClear()
        {
            T value = LinkNext;
            LinkNext = null;
            return value;
        }
        /// <summary>
        /// 缓存对象链表（用于冲突概率低的场景）
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public struct YieldPool
        {
            /// <summary>
            /// 是否需要释放资源
            /// </summary>
            private readonly static bool isDisponse = typeof(IDisposable).IsAssignableFrom(typeof(T));
            /// <summary>
            /// 链表头部
            /// </summary>
            private T head;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private AutoCSer.Threading.SpinLock popLock;
            /// <summary>
            /// 缓存数量
            /// </summary>
            private int count;
            /// <summary>
            /// 最大缓存数量（非精确数量）
            /// </summary>
            private readonly int maxCount;
            /// <summary>
            /// 链表
            /// </summary>
            /// <param name="maxCount">链表缓存池默认缓存数量</param>
            internal YieldPool(int maxCount)
            {
                head = null;
                popLock = default(AutoCSer.Threading.SpinLock);
                count = 0;
                this.maxCount = maxCount;
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            private void push(T value)
            {
                System.Threading.Interlocked.Increment(ref count);
                T headValue;
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
                    AutoCSer.Threading.ThreadYield.Yield();
                }
                while (true);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value">不可为 null</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push(T value)
            {
                if (count < maxCount) push(value);
                else if (isDisponse) ((IDisposable)value).Dispose();
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value">不可为 null</param>
            /// <returns>是否添加成功</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int IsPush(T value)
            {
                if (count < maxCount)
                {
                    push(value);
                    return 1;
                }
                if (isDisponse) ((IDisposable)value).Dispose();
                return 0;
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <returns></returns>
            public T Pop()
            {
                T headValue;
                popLock.EnterYield();
                do
                {
                    if ((headValue = head) == null)
                    {
                        popLock.Exit();
                        return null;
                    }
                    if (System.Threading.Interlocked.CompareExchange(ref head, headValue.LinkNext, headValue) == headValue)
                    {
                        popLock.Exit();
                        System.Threading.Interlocked.Decrement(ref count);
                        headValue.LinkNext = null;
                        return headValue;
                    }
                    AutoCSer.Threading.ThreadYield.Yield();
                }
                while (true);
            }
            /// <summary>
            /// 释放列表
            /// </summary>
            /// <param name="value"></param>
            private void disposeLink(T value)
            {
                while (value != null)
                {
                    ((IDisposable)value).Dispose();
                    value = value.LinkNext;
                }
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            internal void ClearCache(int count)
            {
                T headValue = System.Threading.Interlocked.Exchange(ref head, null);
                System.Threading.Interlocked.Exchange(ref this.count, 0);
                if (count <= 0)
                {
                    if (isDisponse) disposeLink(headValue);
                }
                else if (headValue != null)
                {
                    int pushCount = count;
                    T end = headValue;
                    while (--count != 0)
                    {
                        if (end.LinkNext == null)
                        {
                            PushLink(headValue, end, pushCount - count);
                            return;
                        }
                        end = end.LinkNext;
                    }
                    T next = end.LinkNext;
                    end.LinkNext = null;
                    PushLink(headValue, end, pushCount);
                    if (isDisponse) disposeLink(next);
                }
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="value">链表头部</param>
            /// <param name="end">链表尾部</param>
            /// <param name="count">数据数量</param>
            internal void PushLink(T value, T end, int count)
            {
                System.Threading.Interlocked.Add(ref this.count, count);
                T headValue;
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
                    AutoCSer.Threading.ThreadYield.Yield();
                }
                while (true);
            }

            /// <summary>
            /// 链表节点池
            /// </summary>
            public static YieldPool Default;
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count"></param>
            private static void defaultClearCache(int count)
            {
                Default.ClearCache(count);
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            private static void defaultReleaseFree()
            {
                Default.ClearCache(AutoCSer.Common.ProcessorCount);
            }

            static YieldPool()
            {
                LinkPoolParameter parameter = AutoCSer.Common.Config.GetLinkPoolParameter(typeof(T));
                Default = new YieldPool(parameter.MaxObjectCount);
                if (parameter.MaxObjectCount > 0)
                {
                    if (parameter.IsClearCache) AutoCSer.Memory.Common.AddClearCache(defaultClearCache);
                    if (parameter.ReleaseFreeTimeoutSeconds > 0)
                    {
                        new SecondTimerActionTask(AutoCSer.Threading.SecondTimer.InternalTaskArray, defaultReleaseFree, parameter.ReleaseFreeTimeoutSeconds, SecondTimerThreadMode.Synchronous, SecondTimerKeepMode.Before, parameter.ReleaseFreeTimeoutSeconds).AppendTaskArray();
                    }
                    AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(YieldPool));
                }
            }
        }
    }
}
