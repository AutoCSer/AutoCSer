using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 签名计算成员配置
    /// </summary>
    internal sealed class SignMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 默认签名计算成员配置
        /// </summary>
        internal static readonly SignMemberAttribute Default = new SignMemberAttribute();
        /// <summary>
        /// 是否需要Utf-8编码
        /// </summary>
        internal bool IsEncodeUtf8 = true;
    }
}
