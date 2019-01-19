using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 服务器端时间
    /// </summary>
    [ClientType(Name = "AutoCSer.ServerTime", MemberName = null)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ServerTime
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        [OutputAjax]
        public DateTime Now;
    }
}
