using System;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct CallSynchronize
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// WEB 调用
        /// </summary>
        [FieldOffset(0)]
        public WebView.CallSynchronize Value;
    }
}
