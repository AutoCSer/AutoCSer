using System;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct AjaxBase
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// AJAX 调用
        /// </summary>
        [FieldOffset(0)]
        public WebView.AjaxBase Value;
    }
}
