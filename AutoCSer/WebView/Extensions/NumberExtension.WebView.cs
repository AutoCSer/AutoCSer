using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    internal unsafe static partial class NumberExtensionWebView
    {
        /// <summary>
        /// 数字值转换为十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">十六进制字符串</param>
        /// <returns>0表示相等</returns>
        internal unsafe static uint CheckHex(this uint value, char* hexs)
        {
            return ((uint)*hexs ^ NumberExtension.ToHex(value >> 28))
                | ((uint)*(hexs + 1) ^ NumberExtension.ToHex((value >> 24) & 15))
                | ((uint)*(hexs + 2) ^ NumberExtension.ToHex((value >> 20) & 15))
                | ((uint)*(hexs + 3) ^ NumberExtension.ToHex((value >> 16) & 15))
                | ((uint)*(hexs + 4) ^ NumberExtension.ToHex((value >> 12) & 15))
                | ((uint)*(hexs + 5) ^ NumberExtension.ToHex((value >> 8) & 15))
                | ((uint)*(hexs + 6) ^ NumberExtension.ToHex((value >> 4) & 15))
                | ((uint)*(hexs + 7) ^ NumberExtension.ToHex(value & 15));
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
