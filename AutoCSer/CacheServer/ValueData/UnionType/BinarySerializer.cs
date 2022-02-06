using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct BinarySerializer
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 数据序列化
        /// </summary>
        [FieldOffset(0)]
        public ValueData.BinarySerializer Value;
    }
}
