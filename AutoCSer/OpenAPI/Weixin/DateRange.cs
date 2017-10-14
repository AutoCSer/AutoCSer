using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 时间跨度
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DateRange
    {
        /// <summary>
        /// 获取数据的起始日期，begin_date和end_date的差值需小于“最大时间跨度”（比如最大时间跨度为1时，begin_date和end_date的差值只能为0，才能小于1），否则会报错
        /// </summary>
        public string begin_date;
        /// <summary>
        /// 获取数据的结束日期，end_date允许设置的最大值为昨日
        /// </summary>
        public string end_date;
        /// <summary>
        /// 时间跨度检测
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <param name="maxDays"></param>
        /// <returns></returns>
        public static DateRange Check(DateTime begin_date, byte days, byte maxDays)
        {
            if (--days < (byte)7)
            {
                DateTime today = Date.NowTime.Now;
                if (begin_date < (today = new DateTime(today.Year, today.Month, today.Day)))
                {
                    if (days == 0)
                    {
                        string dateString = begin_date.toDateString();
                        return new DateRange { begin_date = dateString, end_date = dateString };
                    }
                    DateTime endDate = begin_date.AddDays(days);
                    if (endDate >= today) endDate = today.AddDays(-1);
                    return new DateRange { begin_date = begin_date.toDateString(), end_date = endDate.toDateString() };
                }
            }
            return default(DateRange);
        }
        /// <summary>
        /// 时间检测
        /// </summary>
        /// <param name="begin_date"></param>
        /// <returns></returns>
        public static string ToString(DateTime begin_date)
        {
            DateTime today = Date.NowTime.Now;
            return begin_date < (today = new DateTime(today.Year, today.Month, today.Day)) ? begin_date.toDateString('-') : null;
        }
    }
}
