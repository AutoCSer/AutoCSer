using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 选择所有字段成员
    /// </summary>
    [AutoCSer.Sql.Model(MemberFilters = AutoCSer.Metadata.MemberFilters.InstanceField)]
    public partial class InstanceField
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 非公共字段成员，如果默认设置 AutoCSer.Metadata.MemberFilters.PublicInstanceField 将忽略此字段
        /// </summary>
        protected int ProtectedField;
    }
}
