using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// 访问令牌用户
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct TokenUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int id;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string name;
        /// <summary>
        /// 头像集合
        /// </summary>
        public Avatar[] avatar;
    }
}
