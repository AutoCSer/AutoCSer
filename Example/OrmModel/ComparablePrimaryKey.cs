using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 多关键字实现 IComparable 接口
    /// </summary>
    [AutoCSer.Data.PrimaryKey(IsComparable = true)]
    [AutoCSer.Sql.Model]
    public partial class ComparablePrimaryKey
    {
        /// <summary>
        /// 关键字 1
        /// </summary>
        [AutoCSer.Sql.Member(PrimaryKeyIndex = 1)]
        public int Key1;
        /// <summary>
        /// 关键字 2
        /// </summary>
        [AutoCSer.Sql.Member(PrimaryKeyIndex = 2)]
        public int Key2;
    }
}
