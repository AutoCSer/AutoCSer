using System;

namespace AutoCSer.MessagePack
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType : byte
    {
        /// <summary>
        /// 错误数据
        /// </summary>
        Error,
        /// <summary>
        /// 空指针
        /// </summary>
        Null,
        /// <summary>
        /// 整数
        /// </summary>
        Integer,
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
        /// 内存数据块
        /// </summary>
        Memory,
        /// <summary>
        /// K-V
        /// </summary>
        Map,
        /// <summary>
        /// 数组
        /// </summary>
        Array,
        /// <summary>
        /// 扩展数据
        /// </summary>
        Extension,
        /// <summary>
        /// 保留
        /// </summary>
        Reserved,
    }
}
