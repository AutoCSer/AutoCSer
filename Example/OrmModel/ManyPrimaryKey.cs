using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 多关键字
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class ManyPrimaryKey
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
