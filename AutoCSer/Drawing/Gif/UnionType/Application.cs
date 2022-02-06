using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Drawing.Gif.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Application
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 应用程序扩展
        /// </summary>
        [FieldOffset(0)]
        public Gif.Application Value;
    }
}
