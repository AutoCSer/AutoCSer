using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 远程成员配置
    /// </summary>
    public sealed partial class RemoteMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName;
    }
}
