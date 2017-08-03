using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 定时触发 Sql 扩展
    /// </summary>
    internal static class OnTime
    {
        /// <summary>
        /// 设置定时触发类型
        /// </summary>
        /// <param name="flag"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Set(Date.NowTime.OnTimeFlag flag)
        {
            Date.NowTime.Flag |= flag;
        }
        /// <summary>
        /// 触发定时任务
        /// </summary>
        /// <param name="flag"></param>
        private static void onTime(Date.NowTime.OnTimeFlag flag)
        {
            if ((flag & Date.NowTime.OnTimeFlag.SqlCountMember) != 0)
            {
                for (Cache.Whole.CountMember countMember = Cache.Whole.CountMember.CountMembers.End; countMember != null; countMember = countMember.DoubleLinkPrevious) countMember.OnTimer();
            }
        }

        static OnTime()
        {
            Date.NowTime.SqlOnTime = onTime;
        }
    }
}
