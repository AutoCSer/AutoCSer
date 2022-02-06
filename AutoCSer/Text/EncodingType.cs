using System;

namespace AutoCSer
{
    /// <summary>
    /// 字符编码
    /// </summary>
    [Flags]
    internal enum EncodingType : byte
    {
        /// <summary>
        /// ASCII
        /// </summary>
        Ascii = 1,
        /// <summary>
        /// 兼容 ASCII
        /// </summary>
        CompatibleAscii = 2,
        /// <summary>
        /// Unicode
        /// </summary>
        Unicode = 4,
    }
}
