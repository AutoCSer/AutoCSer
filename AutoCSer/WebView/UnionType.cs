using System;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// AJAX 调用配置
        /// </summary>
        [FieldOffset(0)]
        public AjaxAttribute AjaxAttribute;
        /// <summary>
        /// AJAX 调用
        /// </summary>
        [FieldOffset(0)]
        public AjaxBase AjaxBase;
        /// <summary>
        /// WEB 调用配置
        /// </summary>
        [FieldOffset(0)]
        public CallAttribute CallAttribute;
        /// <summary>
        /// WEB 调用
        /// </summary>
        [FieldOffset(0)]
        public CallSynchronize CallSynchronize;
        /// <summary>
        /// WEB 异步调用
        /// </summary>
        [FieldOffset(0)]
        public CallAsynchronousBase CallAsynchronousBase;
        /// <summary>
        /// 公用 AJAX 调用
        /// </summary>
        [FieldOffset(0)]
        public PubAjax PubAjax;
        /// <summary>
        /// WEB 视图配置
        /// </summary>
        [FieldOffset(0)]
        public ViewAttribute ViewAttribute;
    }
}
