using System;
using System.Threading;

namespace AutoCSer
{
    /// <summary>
    /// 常用公共定义
    /// </summary>
    public static partial class Pub
    {
        ///// <summary>
        ///// 项目常量，不可修改
        ///// </summary>
        //internal const string AutoCSer = "AutoCSer";
        /// <summary>
        /// AutoCSer 爬虫标识
        /// </summary>
        public const string HttpSpiderUserAgent = "AutoCSer spider";
        /// <summary>
        /// 最小时间值
        /// </summary>
        public static readonly DateTime MinTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// 默认自增标识
        /// </summary>
        private static long identity;
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public static long Identity
        {
            get { return Interlocked.Increment(ref identity); }
        }
    }
}
