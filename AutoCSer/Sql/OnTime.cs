using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 定时触发 Sql 扩展
    /// </summary>
    //internal sealed class OnTime : Date.NowTime.OnTime
    {
        /// <summary>
        /// 触发定时任务
        /// </summary>
        internal override void OnTimer()
        {
            for (Cache.Whole.CountMember countMember = Cache.Whole.CountMember.CountMembers.End; countMember != null; countMember = countMember.DoubleLinkPrevious) countMember.OnTimer();
        }
        /// <summary>
        /// 设置定时触发器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set()
        {
            if (Date.NowTime.SqlOnTime == null)
            {
                Date.NowTime.SqlOnTime = this;
                Date.NowTime.IsOnTime = true;
            }
        }
        /// <summary>
        /// 定时触发 Sql 扩展
        /// </summary>
        internal static readonly OnTime Default = new OnTime();
    }
}
