using System;

namespace AutoCSer
{
    /// <summary>
    /// 可控逻辑值
    /// </summary>
    internal enum NullableBool : byte
    {
        /// <summary>
        /// 空值
        /// </summary>
        Null,
        /// <summary>
        /// 逻辑假值
        /// </summary>
        False,
        /// <summary>
        /// 逻辑真值
        /// </summary>
        True
    }
}
