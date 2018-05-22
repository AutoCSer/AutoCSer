using System;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 查找匹配记录集合
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    internal abstract class FindAwaiter<valueType> : Awaiter<LeftArray<valueType>>
        where valueType : class
    {
        /// <summary>
        /// 数据匹配委托
        /// </summary>
        protected readonly Func<valueType, bool> isValue;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="isValue"></param>
        internal FindAwaiter(Func<valueType, bool> isValue)
        {
            this.isValue = isValue;
        }
    }
}
