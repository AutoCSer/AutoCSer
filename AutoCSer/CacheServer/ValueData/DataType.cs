using System;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 数据类型
    /// </summary>
    internal enum DataType : byte
    {
        /// <summary>
        /// 空值，用于返回值
        /// </summary>
        Null,
        /// <summary>
        /// 二进制序列化
        /// </summary>
        BinarySerialize,
        /// <summary>
        /// JSON 序列化
        /// </summary>
        Json,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 字节数组
        /// </summary>
        ByteArray,
        /// <summary>
        /// 数字
        /// </summary>
        Decimal,
        /// <summary>
        /// Guid
        /// </summary>
        Guid,

        /// <summary>
        /// ulong
        /// </summary>
        ULong,
        /// <summary>
        /// long
        /// </summary>
        Long,
        /// <summary>
        /// uint
        /// </summary>
        UInt,
        /// <summary>
        /// int
        /// </summary>
        Int,
        /// <summary>
        /// ushort
        /// </summary>
        UShort,
        /// <summary>
        /// short
        /// </summary>
        Short,
        /// <summary>
        /// byte
        /// </summary>
        Byte,
        /// <summary>
        /// sbyte
        /// </summary>
        SByte,
        /// <summary>
        /// 字符
        /// </summary>
        Char,
        /// <summary>
        /// 逻辑值
        /// </summary>
        Bool,
        /// <summary>
        /// 浮点数
        /// </summary>
        Float,
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        Double,
        /// <summary>
        /// 时间值
        /// </summary>
        DateTime,
    }
}
