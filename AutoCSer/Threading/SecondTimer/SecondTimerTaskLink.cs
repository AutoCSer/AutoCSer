using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时任务节点链表
    /// </summary>
    internal struct SecondTimerTaskLink
    {
        /// <summary>
        /// 任务尾节点
        /// </summary>
        private SecondTimerTaskNode end;
        /// <summary>
        /// 任务首节点
        /// </summary>
        private SecondTimerTaskNode head;
        /// <summary>
        /// 添加尾节点
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(SecondTimerTaskNode next)
        {
            if (end == null) head = end = next;
            else
            {
                next.DoubleLinkPrevious = end;
                end.DoubleLinkNext = next;
                end = next;
            }
        }
        /// <summary>
        /// 将另外一个链表的首节点添加到尾节点并返回下一个节点
        /// </summary>
        /// <param name="otherHead"></param>
        /// <returns></returns>
        internal SecondTimerTaskNode AppendOtherHead(SecondTimerTaskNode otherHead)
        {
            SecondTimerTaskNode next = otherHead.DoubleLinkNext;
            if (next == null)
            {
                if (otherHead.KeepMode != SecondTimerKeepMode.Canceled) Append(otherHead);
            }
            else
            {
                if (otherHead.KeepMode != SecondTimerKeepMode.Canceled)
                {
                    otherHead.DoubleLinkNext = null;
                    Append(otherHead);
                }
                next.DoubleLinkPrevious = null;
            }
            return next;
        }
        /// <summary>
        /// 获取首节点并且清除数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SecondTimerTaskNode GetClear()
        {
            SecondTimerTaskNode value = head;
            head = end = null;
            return value;
        }
    }
}
