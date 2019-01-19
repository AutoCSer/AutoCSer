using System;
using System.Threading;

namespace fastCSharp.Threading
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public abstract partial class Link<valueType>
        where valueType : Link<valueType>
    {
        /// <summary>
        /// 链表（用于冲突概率高的场景，比如并发每秒 KW 级的访问）
        /// </summary>
        public sealed class LockLink
        {
            /// <summary>
            /// 链表头部
            /// </summary>
            private volatile valueType head;
            /// <summary>
            /// 对象池访问锁
            /// </summary>
            private readonly object poolLock = new object();
            /// <summary>
            /// 是否空队列
            /// </summary>
            public bool IsEmpty
            {
                get { return head == null; }
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
            public void Push(valueType value)
            {
                Monitor.Enter(poolLock);
                value.Next = head;
                head = value;
                Monitor.Exit(poolLock);
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <returns></returns>
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
            public valueType Pop()
            {
                Monitor.Enter(poolLock);
                if (head == null)
                {
                    Monitor.Exit(poolLock);
                    return null;
                }
                valueType value = head;
                head = head.Next;
                Monitor.Exit(poolLock);
                value.Next = null;
                return value;
            }
        }
    }
}
