using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表节点
    /// </summary>
    public abstract partial class Link<valueType>
        where valueType : Link<valueType>
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
            private valueType head;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private int popLock;
            /// <summary>
            /// 是否空链表
            /// </summary>
            public bool IsEmpty
            {
                get { return head == null; }
            }
            /// <summary>
            /// 获取链表头部并清除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal valueType GetClear()
            {
                return System.Threading.Interlocked.Exchange(ref this.head, null);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushNotNull(valueType value)
            {
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
            /// 弹出节点
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
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
                        headValue.LinkNext = null;
                        return headValue;
                    }
                    ThreadYield.Yield(ThreadYield.Type.YieldLinkPop);
                }
                while (true);
            }
            /// <summary>
            /// 弹出节点（单线程）
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public valueType SinglePop()
            {
                valueType headValue;
                do
                {
                    if ((headValue = head) == null) return null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, headValue.LinkNext, headValue) == headValue)
                    {
                        headValue.LinkNext = null;
                        return headValue;
                    }
                    ThreadYield.Yield(ThreadYield.Type.YieldLinkPop);
                }
                while (true);
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            internal void ClearCache(int count)
            {
                valueType headValue = System.Threading.Interlocked.Exchange(ref head, null);
                if (headValue != null && count != 0)
                {
                    valueType end = headValue;
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
            internal void PushLink(valueType value, valueType end)
            {
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
        /// 链表节点队列
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct Queue
        {
            /// <summary>
            /// 节点队列链表
            /// </summary>
            /// <param name="head">节点队列头部</param>
            internal Queue(valueType head)
            {
                this.head = end = head;
            }
            /// <summary>
            /// 节点队列头部
            /// </summary>
            private valueType head;
            /// <summary>
            /// 节点队列尾部
            /// </summary>
            private valueType end;
            /// <summary>
            /// 是否空队列
            /// </summary>
            public bool IsEmpty
            {
                get { return head == end; }
            }
            /// <summary>
            /// 获取链表头部并清除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal valueType GetClear()
            {
                valueType value = head.LinkNext;
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
            internal valueType GetClear(out valueType end)
            {
                end = this.end;
                valueType value = head.LinkNext;
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
            public void Push(valueType value)
            {
                end.LinkNext = value;
                end = value;
            }
            /// <summary>
            /// 弹出节点（不处理下一个节点，可能会造成少许暂时的内存泄露）
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public valueType PopOnly()
            {
                return head == end ? null : UnsafePopOnly();
            }
            /// <summary>
            /// 弹出节点（不处理下一个节点，可能会造成少许暂时的内存泄露）
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public valueType UnsafePopOnly()
            {
                valueType value = head.LinkNext;
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
            internal valueType Head;
            /// <summary>
            /// 链表
            /// </summary>
            private valueType end;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private int queueLock;
            /// <summary>
            /// 节点队列链表
            /// </summary>
            /// <param name="head">节点队列头部</param>
            internal YieldQueue(valueType head)
            {
                this.Head = end = head;
                queueLock = 0;
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
            internal valueType GetClear()
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePop);
                valueType value = Head.LinkNext;
                end = Head;
                Head.LinkNext = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                return value;
            }
            /// <summary>
            /// 获取链表并清除数据
            /// </summary>
            /// <param name="end"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal valueType GetClear(out valueType end)
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePop);
                end = this.end;
                valueType value = Head.LinkNext;
                this.end = Head;
                Head.LinkNext = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                return value;
            }
            /// <summary>
            /// 获取链表并清除数据
            /// </summary>
            /// <param name="end"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void GetToEndClear(ref valueType end)
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePop);
                if (Head != this.end)
                {
                    end.LinkNext = Head.LinkNext;
                    end = this.end;
                    Head.LinkNext = null;
                    this.end = Head;
                }
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push(valueType value)
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePush);
                end.LinkNext = value;
                end = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool IsPushHead(valueType value)
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePush);
                valueType end = this.end;
                this.end.LinkNext = value;
                valueType head = Head;
                this.end = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                return head == end;
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="head"></param>
            /// <param name="end"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushHead(ref valueType head, valueType end)
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePush);
                if (this.Head == this.end)
                {
                    this.end = end;
                    this.Head.LinkNext = head;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                else
                {
                    end.LinkNext = this.Head.LinkNext;
                    this.Head.LinkNext = head;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
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
            internal bool IsPushHead(ref valueType head, valueType end)
            {
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldQueuePush);
                if (this.Head == this.end)
                {
                    this.end = end;
                    this.Head.LinkNext = head;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                    head = null;
                    return true;
                }
                end.LinkNext = this.Head.LinkNext;
                this.Head.LinkNext = head;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                head = null;
                return false;
            }
        }
    }
}
