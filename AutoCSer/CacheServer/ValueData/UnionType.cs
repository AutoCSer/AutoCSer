using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.ValueData
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
        /// 字符串
        /// </summary>
        [FieldOffset(0)]
        public string String;
        /// <summary>
        /// 字节数组
        /// </summary>
        [FieldOffset(0)]
        public byte[] ByteArray;
        /// <summary>
        /// decimal 参数
        /// </summary>
        [FieldOffset(0)]
        public Decimal Decimal;
        /// <summary>
        /// Guid 参数
        /// </summary>
        [FieldOffset(0)]
        public Guid Guid;
        /// <summary>
        /// 数据序列化
        /// </summary>
        [FieldOffset(0)]
        public BinarySerializer BinarySerializer;
    }
}
