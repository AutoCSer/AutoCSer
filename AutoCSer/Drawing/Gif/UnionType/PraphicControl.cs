using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Drawing.Gif.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct PraphicControl
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 图形控制扩展
        /// </summary>
        [FieldOffset(0)]
        public Gif.PraphicControl Value;
    }
}
