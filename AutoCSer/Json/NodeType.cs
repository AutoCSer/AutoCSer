using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType : byte
    {
        /// <summary>
        /// 空值
        /// </summary>
        Null,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 未解析字符串
        /// </summary>
        QuoteString,
        /// <summary>
        /// 解析错误的字符串
        /// </summary>
        ErrorQuoteString,
        /// <summary>
        /// 数字字符串
        /// </summary>
        NumberString,
        /// <summary>
        /// 非数值
        /// </summary>
        NaN,
        /// <summary>
        /// 正无穷
        /// </summary>
        PositiveInfinity,
        /// <summary>
        /// 负无穷
        /// </summary>
        NegativeInfinity,
        /// <summary>
        /// 时间周期值
        /// </summary>
        DateTimeTick,
        /// <summary>
        /// 逻辑值
        /// </summary>
        Bool,
        /// <summary>
        /// 列表
        /// </summary>
        Array,
        /// <summary>
        /// 字典
        /// </summary>
        Dictionary,
    }
}
