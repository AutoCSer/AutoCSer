using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DoubleLink<T>
        where T : DoubleLink<T>
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal T DoubleLinkNext;
        /// <summary>
        /// 上一个节点
        /// </summary>
        internal T DoubleLinkPrevious;
        /// <summary>
        /// 重置链表状态
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ResetDoubleLink()
        {
            DoubleLinkNext = DoubleLinkPrevious = null;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <param name="linkLock">链表访问锁</param>
        /// <returns>是否弹出节点，false 表示不允许重复弹出操作</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool freeNotEnd(ref AutoCSer.Threading.SpinLock linkLock)
        {
            if (DoubleLinkNext != null)
            {
                DoubleLinkNext.DoubleLinkPrevious = DoubleLinkPrevious;
                if (DoubleLinkPrevious != null)
                {
                    DoubleLinkPrevious.DoubleLinkNext = DoubleLinkNext;
                    DoubleLinkPrevious = null;
                }
                DoubleLinkNext = null;
                linkLock.Exit();
                return true;
            }
            linkLock.Exit(); //已经被释放的不做处理
            return false;
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
            internal T End;
            /// <summary>
            /// 链表访问锁
            /// </summary>
            private AutoCSer.Threading.SpinLock linkLock;
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushNotNull(T value)
            {
                linkLock.EnterYield();
                if (End == null)
                {
                    End = value;
                    linkLock.Exit();
                }
                else
                {
                    End.DoubleLinkNext = value;
                    value.DoubleLinkPrevious = End;
                    End = value;
                    linkLock.Exit();
                }
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否弹出节点，false 表示不允许重复弹出操作</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool PopNotNull(T value)
            {
                linkLock.EnterYield();
                if (value == End)
                {
                    End = value.DoubleLinkPrevious;
                    if (End != null)
                    {
                        End.DoubleLinkNext = null;
                        value.DoubleLinkPrevious = null;
                    }
                    linkLock.Exit();
                    return true;
                }
                return value.freeNotEnd(ref linkLock);
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
