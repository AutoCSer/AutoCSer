using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ByteArray
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 字节数组
        /// </summary>
        [FieldOffset(0)]
        public byte[] Value;
    }
}
