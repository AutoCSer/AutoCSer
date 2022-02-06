using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Drawing.Gif.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct PlainText
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 图形文本扩展
        /// </summary>
        [FieldOffset(0)]
        public Gif.PlainText Value;
    }
}
