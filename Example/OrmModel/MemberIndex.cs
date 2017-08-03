using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 生成成员索引
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class MemberIndex
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 生成成员索引
        /// </summary>
        [AutoCSer.Sql.Member(IsMemberIndex = true)]
        public int UpdateIndex;
    }
}
