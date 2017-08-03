using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据列成员忽略配置
    /// </summary>
    public sealed class IgnoreMemberAttribute : MemberAttribute
    {
        /// <summary>
        /// 禁止当前安装
        /// </summary>
        internal override bool GetIsIgnoreCurrent
        {
            get { return true; }
        }
    }
}
