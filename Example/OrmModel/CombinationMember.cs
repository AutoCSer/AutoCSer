using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 自定义组合列
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class CombinationMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 自定义组合列
        /// </summary>
        public Member.Range IndexRange;
    }
}
