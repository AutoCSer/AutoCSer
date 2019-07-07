using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public abstract class DoubleLink<valueType>
        where valueType : DoubleLink<valueType>
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal valueType DoubleLinkNext;
        /// <summary>
        /// 上一个节点
        /// </summary>
        internal valueType DoubleLinkPrevious;
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeNotEnd()
        {
            DoubleLinkNext.DoubleLinkPrevious = DoubleLinkPrevious;
            if (DoubleLinkPrevious != null)
            {
                DoubleLinkPrevious.DoubleLinkNext = DoubleLinkNext;
                DoubleLinkPrevious = null;
            }
            DoubleLinkNext = null;
        }
        
        /// <summary>
        /// 双向链表
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct YieldLink
        {
            /// <summary>
            /// 链表尾部
            /// </summary>
            internal valueType End;
            /// <summary>
            /// 链表访问锁
            /// </summary>
            private int linkLock;
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushNotNull(valueType value)
            {
                while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePush);
                if (End == null)
                {
                    End = value;
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
                else
                {
                    End.DoubleLinkNext = value;
                    value.DoubleLinkPrevious = End;
                    End = value;
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PopNotNull(valueType value)
            {
                while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
                if (value == End)
                {
                    if ((End = value.DoubleLinkPrevious) != null) End.DoubleLinkNext = value.DoubleLinkPrevious = null;
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
                else
                {
                    value.freeNotEnd();
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
            }
            ///// <summary>
            ///// 清除数据
            ///// </summary>
            //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            //internal void Clear()
            //{
            //    while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
            //    End = null;
            //    System.Threading.Interlocked.Exchange(ref linkLock, 0);
            //}
        }
    }
}
