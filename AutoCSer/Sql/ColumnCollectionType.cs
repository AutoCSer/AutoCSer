using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 列集合类型
    /// </summary>
    internal enum ColumnCollectionType
    {
        /// <summary>
        /// 普通集合
        /// </summary>
        None,
        /// <summary>
        /// 主键
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// 普通索引
        /// </summary>
        Index,
        /// <summary>
        /// 唯一索引
        /// </summary>
        UniqueIndex,
    }
}
