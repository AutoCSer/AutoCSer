using System;

namespace AutoCSer
{
    /// <summary>
    /// 排序委托
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class SortOrder<valueType>
    {
        /// <summary>
        /// 升序委托
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int ascending(valueType left, valueType right)
        {
            return -1;
        }
        /// <summary>
        /// 升序委托
        /// </summary>
        internal static readonly Func<valueType, valueType, int> Ascending = ascending;
        /// <summary>
        /// 降序委托
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int descending(valueType left, valueType right)
        {
            return 1;
        }
        /// <summary>
        /// 降序委托
        /// </summary>
        internal static readonly Func<valueType, valueType, int> Descending = descending;
    }
}
