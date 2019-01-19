using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 公用错误处理参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PubErrorParameter
    {
        /// <summary>
        /// 错误信息
        /// </summary>
#pragma warning disable
        public string error;
#pragma warning restore
    }
}
