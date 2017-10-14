using System;

namespace AutoCSer.Net.WebClient.Form
{
    /// <summary>
    /// web表单类型配置
    /// </summary>
    public sealed class FormAttribute : AutoCSer.Metadata.MemberFilterAttribute.InstanceField
    {
        /// <summary>
        /// 默认web表单类型配置
        /// </summary>
        internal static readonly FormAttribute AllMember = new FormAttribute { IsAllMember = true };
        /// <summary>
        /// 是否序列化所有成员
        /// </summary>
        public bool IsAllMember;
    }
}
