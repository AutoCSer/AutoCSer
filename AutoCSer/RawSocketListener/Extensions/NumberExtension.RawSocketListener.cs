﻿using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数字扩展操作
    /// </summary>
    public static class NumberExtensionRawSocketListener
    {
        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>十六进制字符串</returns>
        public unsafe static string toHex(this ushort value)
        {
            string hexs = AutoCSer.Extensions.StringExtension.FastAllocateString(4);
            uint value32 = value;
            fixed (char* chars = hexs)
            {
                * chars = (char)AutoCSer.Extensions.NumberExtension.ToHex(value32 >> 12);
                *(chars + 1) = (char)AutoCSer.Extensions.NumberExtension.ToHex((value32 >> 8) & 15);
                *(chars + 2) = (char)AutoCSer.Extensions.NumberExtension.ToHex((value32 >> 4) & 15);
                *(chars + 3) = (char)AutoCSer.Extensions.NumberExtension.ToHex(value32 & 15);
            }
            return hexs;
        }
    }
}
