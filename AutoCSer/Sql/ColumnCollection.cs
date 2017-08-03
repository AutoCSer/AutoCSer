using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 列集合
    /// </summary>
    internal sealed class ColumnCollection
    {
        /// <summary>
        /// 列集合名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 列集合类型
        /// </summary>
        public ColumnCollectionType Type;
        /// <summary>
        /// 列集合
        /// </summary>
        public Column[] Columns;
    }
}
