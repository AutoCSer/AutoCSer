using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表节点
    /// </summary>
    public abstract partial class Link<T>
        where T : Link<T>
    {
        /// <summary>
        /// 链表（用于冲突概率低的场景）
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct YieldLink
        {
            /// <summary>
            /// 链表头部
            /// </summary>
            internal T Head;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private AutoCSer.Threading.SpinLock popLock;
            /// <summary>
            /// 是否空链表
            /// </summary>
            public bool IsEmpty
            {
                get { return Head == null; }
            }
            /// <summary>
            /// 获取链表头部并清除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal T GetClear()
            {
                return System.Threading.Interlocked.Exchange(ref Head, null);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value">不可为 null</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push(T value)
            {
                T headValue;
                do
                {
                    if ((headValue = Head) == null)
                    {
                        value.LinkNext = null;
                        if (System.Threading.Interlocked.CompareExchange(ref Head, value, null) == null) return;
                    }
                    else
                    {
                        value.LinkNext = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref Head, value, headValue) == headValue) return;
                    }
                    AutoCSer.Threading.ThreadYield.Yield();
                }
                while (true);
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public T Pop()
            {
                T headValue;
                popLock.EnterYield();
                do
                {
                    if ((headValue = Head) == null)
                    {
                        popLock.Exit();
                        return null;
                    }
                    if (System.Threading.Interlocked.CompareExchange(ref Head, headValue.LinkNext, headValue) == headValue)
                    {
                        popLock.Exit();
                        headValue.LinkNext = null;
                        return headValue;
                    }
                    AutoCSer.Threading.ThreadYield.Yield();
                }
                while (true);
            }
            /// <summary>
            /// 弹出节点（单线程）
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public T SinglePop()
            {
                T headValue;
                do
                {
                    if ((headValue = Head) == null) return null;
                    if (System.Threading.Interlocked.CompareExchange(ref Head, headValue.LinkNext, headValue) == headValue)
                    {
                        headValue.LinkNext = null;
                        return headValue;
                    }
                    ThreadYield.Yield();
                }
                while (true);
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            internal void ClearCache(int count)
            {
                T headValue = System.Threading.Interlocked.Exchange(ref Head, null);
                if (headValue != null && count != 0)
                {
                    T end = headValue;
                    while (--count != 0)
                    {
                        if (end.LinkNext == null)
                        {
                            PushLink(headValue, end);
                            return;
                        }
                        end = end.LinkNext;
                    }
                    end.LinkNext = null;
                    PushLink(headValue, end);
                }
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="value">链表头部</param>
            /// <param name="end">链表尾部</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushLink(T value, T end)
            {
                T headValue;
                do
                {
                    if ((headValue = Head) == null)
                    {
                        end.LinkNext = null;
                        if (System.Threading.Interlocked.CompareExchange(ref Head, value, null) == null) return;
                    }
                    else
                    {
                        end.LinkNext = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref Head, value, headValue) == headValue) return;
                    }
                    ThreadYield.Yield();
                }
                while (true);
            }
        }
        /// <summary>
        /// 链表节点队列
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct Queue
        {
            /// <summary>
            /// 节点队列头部
            /// </summary>
            private T head;
            /// <summary>
            /// 节点队列尾部
            /// </summary>
            private T end;
            /// <summary>
            /// 是否空队列
            /// </summary>
            public bool IsEmpty
            {
                get { return head == end; }
            }
            /// <summary>
            /// 节点队列链表
            /// </summary>
            /// <param name="head">节点队列头部</param>
            internal Queue(T head)
            {
                this.head = end = head;
            }
            /// <summary>
            /// 获取链表头部并清除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal T GetClear()
            {
                T value = head.LinkNext;
                end = head;
                head.LinkNext = null;
                return value;
            }
            /// <summary>
            /// 获取链表并清除数据
            /// </summary>
            /// <param name="end"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal T GetClear(out T end)
            {
                end = this.end;
                T value = head.LinkNext;
                this.end = head;
                head.LinkNext = null;
                return value;
            }
            /// <summary>
            /// 头节点 next 设置为 null
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void SetHeaderNextNull()
            {
                head.LinkNext = null;
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Push(T value)
            {
                end.LinkNext = value;
                end = value;
            }
            /// <summary>
            /// 弹出节点（不处理下一个节点，可能会造成少许暂时的内存泄露）
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public T PopOnly()
            {
                return head == end ? null : UnsafePopOnly();
            }
            /// <summary>
            /// 弹出节点（不处理下一个节点，可能会造成少许暂时的内存泄露）
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public T UnsafePopOnly()
            {
                T value = head.LinkNext;
                if (value == end) end = head;
                else head.LinkNext = value.LinkNext;
                return value;
            }
            /// <summary>
            /// 链表互换
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Exchange(ref Queue value)
            {
                Queue temp = value;
                value = this;
                this = temp;
            }
        }
        /// <summary>
        /// 链表节点队列
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct YieldQueue
        {
            /// <summary>
            /// 链表头部
            /// </summary>
            internal T Head;
            /// <summary>
            /// 链表
            /// </summary>
            private T end;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private AutoCSer.Threading.SpinLock queueLock;
            /// <summary>
            /// 节点队列链表
            /// </summary>
            /// <param name="head">节点队列头部</param>
            internal YieldQueue(T head)
            {
                this.Head = end = head;
                queueLock = default(AutoCSer.Threading.SpinLock);
            }
            /// <summary>
            /// 是否空链表
            /// </summary>
            public bool IsEmpty
            {
                get { return Head == end; }
            }
            /// <summary>
            /// 获取链表头部并清除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal T GetClear()
            {
                queueLock.EnterYield();
                T value = Head.LinkNext;
                end = Head;
                Head.LinkNext = null;
                queueLock.Exit();
                return value;
            }
            /// <summary>
            /// 获取链表并清除数据
            /// </summary>
            /// <param name="end"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal T GetClear(out T end)
            {
                queueLock.EnterYield();
                end = this.end;
                T value = Head.LinkNext;
                this.end = Head;
                Head.LinkNext = null;
                queueLock.Exit();
                return value;
            }
            /// <summary>
            /// 获取链表并清除数据
            /// </summary>
            /// <param name="end"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void GetToEndClear(ref T end)
            {
                queueLock.EnterYield();
                if (Head != this.end)
                {
                    end.LinkNext = Head.LinkNext;
                    end = this.end;
                    Head.LinkNext = null;
                    this.end = Head;
                }
                queueLock.Exit();
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push(T value)
            {
                queueLock.EnterYield();
                end.LinkNext = value;
                end = value;
                queueLock.Exit();
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool IsPushHead(T value)
            {
                queueLock.EnterYield();
                T end = this.end;
                this.end.LinkNext = value;
                T head = Head;
                this.end = value;
                queueLock.Exit();
                return head == end;
            }
            /// <summary>
            /// 添加首节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool TryPushHead(T value)
            {
                queueLock.EnterYield();
                if (Head == end)
                {
                    Head.LinkNext = value;
                    end = value;
                    queueLock.Exit();
                    return true;
                }
                queueLock.Exit();
                return false;
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="head"></param>
            /// <param name="end"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushHead(ref T head, T end)
            {
                queueLock.EnterYield();
                if (this.Head == this.end)
                {
                    this.end = end;
                    this.Head.LinkNext = head;
                    queueLock.Exit();
                }
                else
                {
                    end.LinkNext = this.Head.LinkNext;
                    this.Head.LinkNext = head;
                    queueLock.Exit();
                }
                head = null;
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="head"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool IsPushHead(ref T head, T end)
            {
                queueLock.EnterYield();
                if (this.Head == this.end)
                {
                    this.end = end;
                    this.Head.LinkNext = head;
                    queueLock.Exit();
                    head = null;
                    return true;
                }
                end.LinkNext = this.Head.LinkNext;
                this.Head.LinkNext = head;
                queueLock.Exit();
                head = null;
                return false;
            }
        }

        ///// <summary>
        ///// 简单队列
        ///// </summary>
        //public struct SimpleQueue
        //{
        //    /// <summary>
        //    /// 首节点
        //    /// </summary>
        //    private T head;
        //    /// <summary>
        //    /// 尾节点
        //    /// </summary>
        //    private T end;
        //    /// <summary>
        //    /// 添加节点
        //    /// </summary>
        //    /// <param name="next">尾节点</param>
        //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //    public void Append(T next)
        //    {
        //        if (head == null) head = end = next;
        //        else
        //        {
        //            end.LinkNext = next;
        //            end = next;
        //        }
        //    }
        //    /// <summary>
        //    /// 获取首节点并清除队列
        //    /// </summary>
        //    /// <returns></returns>
        //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //    public T GetClear()
        //    {
        //        T value = head;
        //        head = end = null;
        //        return value;
        //    }

        //    /// <summary>
        //    /// 获取队列节点集合
        //    /// </summary>
        //    /// <param name="head">队列首节点</param>
        //    /// <returns></returns>
        //    public static System.Collections.Generic.IEnumerable<T> GetQueue(T head)
        //    {
        //        do
        //        {
        //            T next = head.LinkNext;
        //            head.LinkNext = null;
        //            yield return head;
        //            head = next;
        //        }
        //        while (head != null);
        //    }
        //}
    }
}
