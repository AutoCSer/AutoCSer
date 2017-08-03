using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 表格信息
    /// </summary>
    internal sealed class TableColumnCollection
    {
        /// <summary>
        /// 列集合
        /// </summary>
        public ColumnCollection Columns;
        /// <summary>
        /// 主键
        /// </summary>
        public ColumnCollection PrimaryKey;
        /// <summary>
        /// 自增列
        /// </summary>
        public Column Identity;
        /// <summary>
        /// 索引集合
        /// </summary>
        public ColumnCollection[] Indexs;
    }
}
