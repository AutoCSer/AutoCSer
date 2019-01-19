using System;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.Poll
{
    /// <summary>
    /// 轮询验证输入参数信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct VerifyInfo
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public int UserId;
        /// <summary>
        /// 验证信息
        /// </summary>
        public string Verify;
    }
}
