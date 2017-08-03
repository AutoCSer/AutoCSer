using System;

namespace AutoCSer.TestCase.ChatData
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct UserLogin
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 用户登录类型
        /// </summary>
        public UserLoginType Type;
    }
}
