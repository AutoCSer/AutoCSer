using System;
using System.Runtime.InteropServices;

namespace AutoCSer.IO
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 文件流写入器
        /// </summary>
        [FieldOffset(0)]
        public AutoCSer.IO.FileStreamWriter FileStreamWriter;
    }
}
