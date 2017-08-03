using System;

namespace AutoCSer.Sql.MySql
{
    /// <summary>
    /// 索引列
    /// </summary>
    internal sealed class IndexColumn
    {
        /// <summary>
        /// 数据列
        /// </summary>
        public Column Column;
        /// <summary>
        /// 是否不允许重复
        /// </summary>
        public ColumnCollectionType Type;
        /// <summary>
        /// 列序号
        /// </summary>
        public int Index;
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNull;
    }
}
