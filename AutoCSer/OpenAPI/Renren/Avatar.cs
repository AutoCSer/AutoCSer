using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// 用户头像
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Avatar
    {
        /// <summary>
        /// 头像类型
        /// </summary>
        public AvatarSize type;
        /// <summary>
        /// 头像地址
        /// </summary>
        public string url;
    }
}
