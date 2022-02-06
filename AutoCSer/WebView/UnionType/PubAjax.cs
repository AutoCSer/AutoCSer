using System;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct PubAjax
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 公用 AJAX 调用
        /// </summary>
        [FieldOffset(0)]
        public WebView.PubAjax Value;
    }
}
