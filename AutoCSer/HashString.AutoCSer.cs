using System;

namespace AutoCSer
{
    /// <summary>
    /// 字符串 HASH
    /// </summary>
    public partial struct HashString
    {
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator string(HashString value) { return value.String.ToString(); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator SubString(HashString value) { return value.String; }
    }
}
