using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    public unsafe static partial class Number
    {
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static uint uIntToBytes(uint value)
        {
            uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift, value1000 = (value100 * Div10_16Mul) >> Div10_16Shift;
            return ((value - value10 * 10) << 24) | ((value10 - value100 * 10) << 16) | ((value100 - value1000 * 10) << 8) | value1000 | 0x30303030U;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToBytes(uint value, byte* chars)
        {
            if (value >= 100000000)
            {
                uint value100000000 = (uint)((value * (ulong)Div100000000Mul) >> Div100000000Shift);
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value100000000 >= 10)
                {
                    *(uint*)(chars + 2) = uIntToBytes(value10000);
                    *(uint*)(chars + 6) = uIntToBytes(value - value10000 * 10000);
                    value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(ushort*)chars = (ushort)(((value100000000 - value10000 * 10) << 8) | value10000 | 0x3030U);
                    return 10;
                }
                *(uint*)(chars + 1) = uIntToBytes(value10000);
                *(uint*)(chars + 5) = uIntToBytes(value - value10000 * 10000);
                *chars = (byte)(value100000000 + '0');
                return 9;
            }
            return toBytes99999999(value, chars);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal unsafe static int ToBytes(int value, byte* chars)
        {
            if (value >= 0) return ToBytes((uint)value, chars);
            uint value32 = (uint)-value;
            if (value32 >= 100000000)
            {
                uint value100000000 = (uint)((value32 * (ulong)Div100000000Mul) >> Div100000000Shift);
                value32 -= value100000000 * 100000000;
                uint value10000 = (uint)((value32 * Div10000Mul) >> Div10000Shift);
                if (value100000000 >= 10)
                {
                    *(uint*)(chars + 3) = uIntToBytes(value10000);
                    *(uint*)(chars + 7) = uIntToBytes(value32 - value10000 * 10000);
                    value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(ushort*)(chars + 1) = (ushort)(((value100000000 - value10000 * 10) << 8) | value10000 | 0x3030U);
                    *chars = (byte)'-';
                    return 11;
                }
                *(uint*)(chars + 2) = uIntToBytes(value10000);
                *(uint*)(chars + 6) = uIntToBytes(value32 - value10000 * 10000);
                *(ushort*)chars = (ushort)('-' + ((value100000000 + '0') << 8));
                return 10;
            }
            return toBytes_99999999(value32, chars);
        }
        /// <summary>
        /// 绝对值小于100000000的负整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private unsafe static int toBytes_99999999(uint value, byte* chars)
        {
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        *(uint*)(chars + 5) = uIntToBytes(value - value10000 * 10000);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        uint value1000 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 1) = ((value10000 - value10 * 10) << 24) | ((value10 - value * 10) << 16) | ((value - value1000 * 10) << 8) | value1000 | 0x30303030U;
                        *chars = (byte)'-';
                        return 9;
                    }
                    else
                    {
                        *(uint*)(chars + 4) = uIntToBytes(value - value10000 * 10000);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10000 - value10 * 10) << 24) | ((value10 - value * 10) << 16) | ((value + '0') << 8) | '-' | 0x30303000U;
                        return 8;
                    }
                }
                if (value10000 >= 10)
                {
                    *(uint*)(chars + 3) = uIntToBytes(value - value10000 * 10000);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(ushort*)(chars + 1) = (ushort)(((value10000 - value * 10) << 8) | value | 0x3030U);
                    *chars = (byte)'-';
                    return 7;
                }
                *(uint*)(chars + 2) = uIntToBytes(value - value10000 * 10000);
                *(ushort*)chars = (ushort)('-' + ((value10000 + '0') << 8));
                return 6;
            }
            if (value >= 100)
            {
                if (value >= 1000)
                {
                    *(uint*)(chars + 1) = uIntToBytes(value); 
                    *chars = (byte)'-';
                    return 5;
                }
                else
                {
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value - value10 * 10) << 24) | ((value10 - value100 * 10) << 16) | ((value100 + '0') << 8) | '-' | 0x30303000U;
                    return 4;
                }
            }
            if (value >= 10)
            {
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)chars = ((value - value10 * 10) << 16) | (value10 << 8) | '-' | 0x303000U;
                return 3;
            }
            *(uint*)chars = '-' + ((value + '0') << 8);
            return 2;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal static RangeLength ToBytes(ulong value, byte* chars)
        {
            if (value >= 10000000000000000L)
            {
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                *(uint*)(chars + 12) = uIntToBytes(value10000);
                *(uint*)(chars + 16) = uIntToBytes((uint)value - value10000 * 10000U);
                value = value100000000 / 100000000;
                value100000000 -= value * 100000000;
                value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                *(uint*)(chars + 4) = uIntToBytes(value10000);
                *(uint*)(chars + 8) = uIntToBytes((uint)value100000000 - value10000 * 10000U);
                uint value32 = (uint)value;
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        uint value1000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value32 - value10000 * 10) << 24) | ((value10000 - value100 * 10) << 16) | ((value100 - value1000 * 10) << 8) | value1000 | 0x30303030U;
                        return new RangeLength(0, 20);
                    }
                    *(uint*)chars = ((value32 - value10000 * 10) << 24) | ((value10000 - value100 * 10) << 16) | (value100 << 8) | 0x30303030U;
                    return new RangeLength(1, 19);
                }
                if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value32 - value10000 * 10) << 24) | (value10000 << 16) | 0x30303030U;
                    return new RangeLength(2, 18);
                }
                *(chars + 3) = (byte)(value32 + '0');
                return new RangeLength(3, 17);
            }
            if (value >= 100000000)
            {
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                *(uint*)(chars + 8) = uIntToBytes(value10000);
                *(uint*)(chars + 12) = uIntToBytes((uint)value - value10000 * 10000U);
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                    *(uint*)(chars + 4) = uIntToBytes(value32 - value10000 * 10000);
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                        if (value10000 >= 1000)
                        {
                            uint value1000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)chars = ((value10000 - value32 * 10) << 24) | ((value32 - value100 * 10) << 16) | ((value100 - value1000 * 10) << 8) | value1000 | 0x30303030U;
                            return new RangeLength(0, 16);
                        }
                        else
                        {
                            *(uint*)chars = ((value10000 - value32 * 10) << 24) | ((value32 - value100 * 10) << 16) | (value100 << 8) | 0x30303030U;
                            return new RangeLength(1, 15);
                        }
                    }
                    if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10000 - value32 * 10) << 24) | (value32 << 16) | 0x30303030U;
                        return new RangeLength(2, 14);
                    }
                    *(chars + 3) = (byte)(value10000 + '0');
                    return new RangeLength(3, 13);
                }
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        uint value1000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 24) | ((value10000 - value100 * 10) << 16) | ((value100 - value1000 * 10) << 8) | value1000 | 0x30303030U;
                        return new RangeLength(4, 12);
                    }
                    else
                    {
                        *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 24) | ((value10000 - value100 * 10) << 16) | (value100 << 8) | 0x30303030U;
                        return new RangeLength(5, 11);
                    }
                }
                if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 24) | (value10000 << 16) | 0x30303030U;
                    return new RangeLength(6, 10);
                }
                *(chars + 7) = (byte)(value32 + '0');
                return new RangeLength(7, 9);
            }
            return new RangeLength(0, toBytes99999999((uint)value, chars));
        }
        /// <summary>
        /// 小于100000000的正整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private static int toBytes99999999(uint value, byte* chars)
        {
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        *(uint*)(chars + 4) = uIntToBytes(value - value10000 * 10000);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        uint value1000 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10000 - value10 * 10) << 24) | ((value10 - value * 10) << 16) | ((value - value1000 * 10) << 8) | value1000 | 0x30303030U;
                        return 8;
                    }
                    else
                    {
                        *(uint*)(chars + 3) = uIntToBytes(value - value10000 * 10000);
                        chars[2] = (byte)((value10000 - value10 * 10) + '0');
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(ushort*)chars = (ushort)(((value10 - value * 10) << 8) | value | 0x30303030U);
                        return 7;
                    }
                }
                if (value10000 >= 10)
                {
                    *(uint*)(chars + 2) = uIntToBytes(value - value10000 * 10000);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(ushort*)chars = (ushort)(((value10000 - value * 10) << 8) | value | 0x30303030U);
                    return 6;
                }
                *(uint*)(chars + 1) = uIntToBytes(value - value10000 * 10000);
                *chars = (byte)(value10000 + '0');
                return 5;
            }
            if (value >= 100)
            {
                if (value >= 1000)
                {
                    *(uint*)chars = uIntToBytes(value);
                    return 4;
                }
                else
                {
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value - value10 * 10) << 16) | ((value10 - value100 * 10) << 8) | value100 | 0x30303030U;
                    return 3;
                }
            }
            if (value >= 10)
            {
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)chars = ((value - value10 * 10) << 8) | value10 | 0x30303030U;
                return 2;
            }
            *chars = (byte)(value + '0');
            return 1;
        }

        /// <summary>
        /// 转换8位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>8位十六进制字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string toHex(this uint value)
        {
            string hexs = StringExtension.FastAllocateString(8);
            fixed (char* hexFixed = hexs) toHex(value, hexFixed);
            return hexs;
        }
        /// <summary>
        /// 数字值转换为十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">十六进制字符串</param>
        private static void toHex(uint value, char* hexs)
        {
            *hexs = (char)ToHex(value >> 28);
            *(hexs + 1) = (char)ToHex((value >> 24) & 15);
            *(hexs + 2) = (char)ToHex((value >> 20) & 15);
            *(hexs + 3) = (char)ToHex((value >> 16) & 15);
            *(hexs + 4) = (char)ToHex((value >> 12) & 15);
            *(hexs + 5) = (char)ToHex((value >> 8) & 15);
            *(hexs + 6) = (char)ToHex((value >> 4) & 15);
            *(hexs + 7) = (char)ToHex(value & 15);
        }
        /// <summary>
        /// 转换16位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">16位十六进制字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void toHex(this ulong value, char* hexs)
        {
            toHex((uint)value, hexs + 8);
            toHex((uint)(value >> 32), hexs);
        }
        /// <summary>
        /// 转换16位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>16位十六进制字符串</returns>
        public static string toHex(this ulong value)
        {
            string hexs = StringExtension.FastAllocateString(16);
            fixed (char* hexFixed = hexs) toHex(value, hexFixed);
            return hexs;
        }
        /// <summary>
        /// 数字值转换为十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">十六进制字符串</param>
        private unsafe static void toHex(uint value, byte* hexs)
        {
            *hexs = (byte)ToHex(value >> 28);
            *(hexs + 1) = (byte)ToHex((value >> 24) & 15);
            *(hexs + 2) = (byte)ToHex((value >> 20) & 15);
            *(hexs + 3) = (byte)ToHex((value >> 16) & 15);
            *(hexs + 4) = (byte)ToHex((value >> 12) & 15);
            *(hexs + 5) = (byte)ToHex((value >> 8) & 15);
            *(hexs + 6) = (byte)ToHex((value >> 4) & 15);
            *(hexs + 7) = (byte)ToHex(value & 15);
        }
        /// <summary>
        /// 转换16位十六进制字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">16位十六进制字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToHex(this ulong value, byte* hexs)
        {
            toHex((uint)value, hexs + 8);
            toHex((uint)(value >> 32), hexs);
        }
        /// <summary>
        /// 求平方根
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="mod">余数</param>
        /// <returns>平方根</returns>
        public static uint sqrt(this uint value, out uint mod)
        {
            uint sqrtValue = 0;
            if ((mod = value) >= 0x40000000)
            {
                sqrtValue = 0x8000;
                mod -= 0x40000000;
            }
            value = (sqrtValue << 15) + 0x10000000;
            if (mod >= value)
            {
                sqrtValue |= 0x4000;
                mod -= value;
            }
            value = (sqrtValue << 14) + 0x4000000;
            if (mod >= value)
            {
                sqrtValue |= 0x2000;
                mod -= value;
            }
            value = (sqrtValue << 13) + 0x1000000;
            if (mod >= value)
            {
                sqrtValue |= 0x1000;
                mod -= value;
            }
            value = (sqrtValue << 12) + 0x400000;
            if (mod >= value)
            {
                sqrtValue |= 0x800;
                mod -= value;
            }
            value = (sqrtValue << 11) + 0x100000;
            if (mod >= value)
            {
                sqrtValue |= 0x400;
                mod -= value;
            }
            value = (sqrtValue << 10) + 0x40000;
            if (mod >= value)
            {
                sqrtValue |= 0x200;
                mod -= value;
            }
            value = (sqrtValue << 9) + 0x10000;
            if (mod >= value)
            {
                sqrtValue |= 0x100;
                mod -= value;
            }
            value = (sqrtValue << 8) + 0x4000;
            if (mod >= value)
            {
                sqrtValue |= 0x80;
                mod -= value;
            }
            value = (sqrtValue << 7) + 0x1000;
            if (mod >= value)
            {
                sqrtValue |= 0x40;
                mod -= value;
            }
            value = (sqrtValue << 6) + 0x400;
            if (mod >= value)
            {
                sqrtValue |= 0x20;
                mod -= value;
            }
            value = (sqrtValue << 5) + 0x100;
            if (mod >= value)
            {
                sqrtValue |= 0x10;
                mod -= value;
            }
            value = (sqrtValue << 4) + 0x40;
            if (mod >= value)
            {
                sqrtValue |= 0x8;
                mod -= value;
            }
            value = (sqrtValue << 3) + 0x10;
            if (mod >= value)
            {
                sqrtValue |= 0x4;
                mod -= value;
            }
            value = (sqrtValue << 2) + 0x4;
            if (mod >= value)
            {
                sqrtValue |= 0x2;
                mod -= value;
            }
            value = (sqrtValue << 1) + 0x1;
            if (mod >= value)
            {
                sqrtValue++;
                mod -= value;
            }
            return sqrtValue;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>最后二进制0位的长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int endBits(this uint value)
        {
            return value != 0 ? deBruijn32.Byte[((value & (0U - value)) * DeBruijn32Number) >> 27] : 0;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>最后二进制0位的长度</returns>
        public static int endBits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0
                ? (value != 0 ? endBits((uint)(value >> 32)) + 32 : 0)
                : endBits((uint)value);
            //return value != 0 ? DeBruijn64[((value & (0UL - value)) * DeBruijn64Number) >> 58] : 0;
        }
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>有效位长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int bits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0 ? bits((uint)value) : (bits((uint)(value >> 32)) + 32);
            //if ((value & 0x8000000000000000UL) == 0)
            //{
            //    ulong code = value;
            //    code |= code >> 32;
            //    code |= code >> 16;
            //    code |= code >> 8;
            //    code |= code >> 4;
            //    code |= code >> 2;
            //    code |= code >> 1;
            //    return DeBruijn64[((++code) * DeBruijn64Number) >> 58];
            //}
            //else return 32;
        }
        /// <summary>
        /// 十六进制字符转字节
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static uint ParseHex(uint hex)
        {
            uint value = hex - '0';
            return value >= 10 ? (hex & 0xdf) - ('0' + 'A' - '9' - 1) : value;
        }
        /// <summary>
        /// 16进制字符串转换成整数
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <returns>整数</returns>
        internal static unsafe uint ParseHex32(byte* start)
        {
            return (ParseHex(*start) << 28)
                | (ParseHex(*(start + 1)) << 24)
                | (ParseHex(*(start + 2)) << 20)
                | (ParseHex(*(start + 3)) << 16)
                | (ParseHex(*(start + 4)) << 12)
                | (ParseHex(*(start + 5)) << 8)
                | (ParseHex(*(start + 6)) << 4)
                | ParseHex(*(start + 7));
        }
    }
}
