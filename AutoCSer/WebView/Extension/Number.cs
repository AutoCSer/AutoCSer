using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    internal unsafe static partial class Number_WebView
    {
        /// <summary>
        /// 数字值转换为十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">十六进制字符串</param>
        /// <returns>0表示相等</returns>
        internal unsafe static uint CheckHex(this uint value, char* hexs)
        {
            return ((uint)*hexs ^ Number.ToHex(value >> 28))
                | ((uint)*(hexs + 1) ^ Number.ToHex((value >> 24) & 15))
                | ((uint)*(hexs + 2) ^ Number.ToHex((value >> 20) & 15))
                | ((uint)*(hexs + 3) ^ Number.ToHex((value >> 16) & 15))
                | ((uint)*(hexs + 4) ^ Number.ToHex((value >> 12) & 15))
                | ((uint)*(hexs + 5) ^ Number.ToHex((value >> 8) & 15))
                | ((uint)*(hexs + 6) ^ Number.ToHex((value >> 4) & 15))
                | ((uint)*(hexs + 7) ^ Number.ToHex(value & 15));
        }
        /// <summary>
        /// 转换16位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">16位十六进制字符串</param>
        /// <returns>0表示相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static uint CheckHex(this ulong value, char* hexs)
        {
            return CheckHex((uint)value, hexs + 8) == 0 ? CheckHex((uint)(value >> 32), hexs) : 1;
        }
    }
}
