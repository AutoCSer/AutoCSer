using System;

namespace AutoCSer.OpenAPI
{
    /// <summary>
    /// 微博编码类型
    /// </summary>
    public enum MicroblogEncoding : byte
    {
        /// <summary>
        /// 汉字两字节，英文一字节
        /// </summary>
        WordByte,
        /// <summary>
        /// 汉字三字节，英文一字节
        /// </summary>
        Utf8
    }
}
