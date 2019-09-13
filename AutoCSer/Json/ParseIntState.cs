using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 整数解析状态
    /// </summary>
    [Flags]
    internal enum ParseIntState : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        None = 0,
        /// <summary>
        /// 负数
        /// </summary>
        Negative = 1,
        /// <summary>
        /// null 值
        /// </summary>
        Null = 2,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,
        /// <summary>
        /// 十六进制
        /// </summary>
        Hex = 8,
    }
}
