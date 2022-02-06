using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 时间差异类型
    /// </summary>
    public enum ExpressionCallDateDiffType : byte
    {
        /// <summary>
        /// 默认无传参
        /// </summary>
        NONE,
        /// <summary>
        /// 年
        /// </summary>
        yy,
        /// <summary>
        /// 年
        /// </summary>
        yyyy = yy,
        /// <summary>
        /// 季度
        /// </summary>
        qq,
        /// <summary>
        /// 季度
        /// </summary>
        q = qq,
        /// <summary>
        /// 月
        /// </summary>
        mm,
        /// <summary>
        /// 月
        /// </summary>
        m = mm,
        /// <summary>
        /// 年中的日
        /// </summary>
        dy,
        /// <summary>
        /// 年中的日
        /// </summary>
        y = dy,
        /// <summary>
        /// 日
        /// </summary>
        dd,
        /// <summary>
        /// 日
        /// </summary>
        d = dd,
        /// <summary>
        /// 周
        /// </summary>
        wk,
        /// <summary>
        /// 周
        /// </summary>
        ww = wk,
        /// <summary>
        /// 星期
        /// </summary>
        dw,
        /// <summary>
        /// 星期
        /// </summary>
        w = dw,
        /// <summary>
        /// 小时
        /// </summary>
        hh,
        /// <summary>
        /// 分钟
        /// </summary>
        mi,
        /// <summary>
        /// 分钟
        /// </summary>
        n = mi,
        /// <summary>
        /// 秒
        /// </summary>
        ss,
        /// <summary>
        /// 秒
        /// </summary>
        s = ss,
        /// <summary>
        /// 毫秒
        /// </summary>
        ms,
        /// <summary>
        /// 微妙
        /// </summary>
        mcs,
        /// <summary>
        /// 纳秒
        /// </summary>
        ns
    }
}
