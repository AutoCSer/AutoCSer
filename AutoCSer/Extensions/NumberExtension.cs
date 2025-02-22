using System;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    public unsafe static class NumberExtension
    {
        /// <summary>
        /// 16位除以10转乘法的位移
        /// </summary>
        public const int Div10_16Shift = 19;
        /// <summary>
        /// 16位除以10转乘法的乘数
        /// </summary>
        public const uint Div10_16Mul = ((1 << Div10_16Shift) + 9) / 10;
        /// <summary>
        /// 32位除以10000转乘法的乘数
        /// </summary>
        public const ulong Div10000Mul = ((1L << 45) + 9999) / 10000;
        /// <summary>
        /// 32位除以10000转乘法的位移
        /// </summary>
        public const int Div10000Shift = 45;
        /// <summary>
        /// 32位除以100000000转乘法的乘数
        /// </summary>
        public const ulong Div100000000Mul = ((1L << 58) + 99999999) / 100000000;
        /// <summary>
        /// 32位除以100000000转乘法的位移
        /// </summary>
        public const int Div100000000Shift = 58;

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
        /// <returns>字符串长度</returns>
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
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this byte value)
        {
            long chars;
            return new string((char*)&chars, 0, ToString(value, (char*)&chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(byte value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.GetPrepCharSizeCurrent(3)) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(byte value, char* chars)
        {
            if (value < 10)
            {
                *chars = (char)(value + '0');
                return 1;
            }
            if (value >= 100)
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                chars[2] = (char)((value - value10 * 10) + '0');
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030;
                return 3;
            }
            else
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                return 2;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this sbyte value)
        {
            long chars;
            return new string((char*)&chars, 0, ToString(value, (char*)&chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(sbyte value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.GetPrepCharSizeCurrent(4)) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(sbyte value, char* chars)
        {
            if (value >= 0)
            {
                if (value < 10)
                {
                    *chars = (char)(value + '0');
                    return 1;
                }
                if (value >= 100)
                {
                    value -= 100;
                    *chars = '1';
                    int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030;
                    return 3;
                }
                else
                {
                    int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                    return 2;
                }
            }
            int value32 = -value;
            if (value32 < 10)
            {
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return 2;
            }
            if (value32 >= 100)
            {
                value32 -= 100;
                *(int*)chars = '-' + ('1' << 16);
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 2) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return 4;
            }
            else
            {
                *chars = '-';
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 1) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return 3;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this ushort value)
        {
            char* chars = stackalloc char[5 + 3];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(ushort value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.GetPrepCharSizeCurrent(5)) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(ushort value, char* chars)
        {
            if (value < 10)
            {
                *chars = (char)(value + '0');
                return 1;
            }
            if (value >= 10000)
            {
                int value10 = (int)((uint)(value * Div10_16Mul) >> Div10_16Shift);
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 3) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                value = (ushort)((value10 * Div10_16Mul) >> Div10_16Shift);
                *(int*)(chars + 1) = ((value100 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030;
                *chars = (char)(value + '0');
                return 5;
            }
            if (value >= 100)
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                if (value >= 1000)
                {
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030;
                    return 4;
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030;
                    return 3;
                }
            }
            else
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                return 2;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this short value)
        {
            char* chars = stackalloc char[6 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(short value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.GetPrepCharSizeCurrent(6)) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(short value, char* chars)
        {
            if (value >= 0) return ToString((ushort)value, chars);
            int value32 = -value;
            if (value32 < 10)
            {
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return 2;
            }
            if (value32 >= 10000)
            {
                int value10 = (int)((uint)(value32 * Div10_16Mul) >> Div10_16Shift);
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 4) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                value32 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 2) = ((value100 - value10 * 10) << 16) | (value10 - value32 * 10) | 0x300030;
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return 6;
            }
            if (value32 >= 100)
            {
                if (value32 >= 1000)
                {
                    *chars = '-';
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 3) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value100 - value10 * 10) << 16) | value10 | 0x300030;
                    return 5;
                }
                else
                {
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    *(int*)chars = '-' + ((value100 + '0') << 16);
                    return 4;
                }
            }
            else
            {
                *chars = '-';
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 1) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return 3;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this uint value)
        {
            char* chars = stackalloc char[10 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(uint value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.GetPrepCharSizeCurrent(10 + 3)) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(uint value, char* chars)
        {
            if (value >= 100000000)
            {
                uint value100000000 = (uint)((value * (ulong)Div100000000Mul) >> Div100000000Shift);
                if (value100000000 >= 10)
                {
                    uint value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    toString8(value - value100000000 * 100000000, chars + 2);
                    return 10;
                }
                *chars = (char)(value100000000 + '0');
                toString8(value - value100000000 * 100000000, chars + 1);
                return 9;
            }
            return toString99999999(value, chars);
        }
        /// <summary>
        /// 小于100000000的正整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private unsafe static int toString99999999(uint value, char* chars)
        {
            if (value < 10)
            {
                *chars = (char)(value + '0');
                return 1;
            }
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        toString4(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        return 8;
                    }
                    else
                    {
                        toString4(value - value10000 * 10000, chars + 3);
                        chars[2] = (char)((value10000 - value10 * 10) + '0');
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10 - value * 10) << 16) | value | 0x300030U;
                        return 7;
                    }
                }
                if (value10000 >= 10)
                {
                    toString4(value - value10000 * 10000, chars + 2);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    return 6;
                }
                toString4(value - value10000 * 10000, chars + 1);
                *chars = (char)(value10000 + '0');
                return 5;
            }
            if (value >= 100)
            {
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                if (value >= 1000)
                {
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    return 4;
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030U;
                    return 3;
                }
            }
            else
            {
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return 2;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this int value)
        {
            char* chars = stackalloc char[12];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static void ToString(int value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.GetPrepCharSizeCurrent(12 + 3)) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(int value, char* chars)
        {
            if (value >= 0) return ToString((uint)value, chars);
            uint value32 = (uint)-value;
            if (value32 >= 100000000)
            {
                uint value100000000 = (uint)((value32 * (ulong)Div100000000Mul) >> Div100000000Shift);
                if (value100000000 >= 10)
                {
                    uint value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *chars = '-';
                    toString8(value32 - value100000000 * 100000000, chars + 3);
                    return 11;
                }
                *(uint*)chars = '-' + ((value100000000 + '0') << 16);
                toString8(value32 - value100000000 * 100000000, chars + 2);
                return 10;
            }
            return toString_99999999(value32, chars);
        }
        /// <summary>
        /// 绝对值小于100000000的负整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private unsafe static int toString_99999999(uint value, char* chars)
        {
            if (value < 10)
            {
                *(uint*)chars = '-' + ((value + '0') << 16);
                return 2;
            }
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        toString4(value - value10000 * 10000, chars + 5);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 3) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        *chars = '-';
                        return 9;
                    }
                    else
                    {
                        toString4(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        *(uint*)chars = '-' + ((value + '0') << 16);
                        return 8;
                    }
                }
                if (value10000 >= 10)
                {
                    toString4(value - value10000 * 10000, chars + 3);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    *chars = '-';
                    return 7;
                }
                toString4(value - value10000 * 10000, chars + 2);
                *(uint*)chars = '-' + ((value10000 + '0') << 16);
                return 6;
            }
            if (value >= 100)
            {
                if (value >= 1000)
                {
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 3) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    *chars = '-';
                    return 5;
                }
                else
                {
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    *(uint*)chars = '-' + ((value100 + '0') << 16);
                    return 4;
                }
            }
            else
            {
                *chars = '-';
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return 3;
            }
        }
        /// <summary>
        /// 4位十进制数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        private unsafe static void toString4(uint value, char* chars)
        {
            uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
            value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
        }
        /// <summary>
        /// 8位十进制数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void toString8(uint value, char* chars)
        {
            uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
            toString4(value10000, chars);
            toString4(value - value10000 * 10000U, chars + 4);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this ulong value)
        {
            char* chars = stackalloc char[20];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(ulong value, CharStream charStream)
        {
            charStream.PrepCharSize(20 + 3);
            UnsafeToString(value, charStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void UnsafeToString(ulong value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.CurrentChar) << 1;
        }
        /// <summary>
        /// 16位十进制数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void toString16(ulong value, char* chars)
        {
            ulong value100000000 = value / 100000000;
            toString8((uint)value100000000, chars);
            toString8((uint)(value - value100000000 * 100000000), chars + 8);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal unsafe static int ToString(ulong value, char* chars)
        {
            if (value >= 10000000000000000L)
            {
                ulong value10000000000000000 = value / 10000000000000000UL;
                value -= value10000000000000000 * 10000000000000000UL;
                uint value32 = (uint)value10000000000000000;
                if (value32 >= 100)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        toString16(value, chars + 4);
                        return 20;
                    }
                    *(uint*)(chars + 1) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    *chars = (char)(value100 + '0');
                    toString16(value, chars + 3);
                    return 19;
                }
                if (value32 >= 10)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    toString16(value, chars + 2);
                    return 18;
                }
                *chars = (char)(value32 + '0');
                toString16(value, chars + 1);
                return 17;
            }
            if (value >= 100000000)
            {
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    uint value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift), value8 = value32 - value10000 * 10000;
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                        if (value10000 >= 1000)
                        {
                            *(uint*)(chars + 2) = ((value10000 - value32 * 10) << 16) | (value32 - value100 * 10) | 0x300030U;
                            value32 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)chars = ((value100 - value32 * 10) << 16) | value32 | 0x300030U;
                            toString4(value8, chars + 4);
                            toString8((uint)value, chars + 8);
                            return 16;
                        }
                        else
                        {
                            chars[2] = (char)((value10000 - value32 * 10) + '0');
                            *(uint*)chars = ((value32 - value100 * 10) << 16) | value100 | 0x300030U;
                            toString4(value8, chars + 3);
                            toString8((uint)value, chars + 7);
                            return 15;
                        }
                    }
                    if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10000 - value32 * 10) << 16) | value32 | 0x300030U;
                        toString4(value8, chars + 2);
                        toString8((uint)value, chars + 6);
                        return 14;
                    }
                    *chars = (char)(value10000 + '0');
                    toString4(value8, chars + 1);
                    toString8((uint)value, chars + 5);
                    return 13;
                }
                if (value32 >= 100)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        toString8((uint)value, chars + 4);
                        return 12;
                    }
                    else
                    {
                        chars[2] = (char)((value32 - value10000 * 10) + '0');
                        *(uint*)chars = ((value10000 - value100 * 10) << 16) | value100 | 0x300030U;
                        toString8((uint)value, chars + 3);
                        return 11;
                    }
                }
                if (value32 >= 10)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    toString8((uint)value, chars + 2);
                    return 10;
                }
                *chars = (char)(value32 + '0');
                toString8((uint)value, chars + 1);
                return 9;
            }
            return toString99999999((uint)value, chars);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this long value)
        {
            char* chars = stackalloc char[22 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static void ToString(long value, CharStream charStream)
        {
            charStream.PrepCharSize(22 + 3);
            UnsafeToString(value, charStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void UnsafeToString(long value, CharStream charStream)
        {
            charStream.Data.CurrentIndex += ToString(value, charStream.CurrentChar) << 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static int ToString(long value, char* chars)
        {
            if (value >= 0) return ToString((ulong)value, chars);
            *chars = '-';
            return ToString((ulong)-value, chars + 1) + 1;
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
        /// 半字节转十六进制字符
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static uint ToHex(uint data)
        {
            return data < 10 ? data + '0' : (data + ('0' + 'A' - '9' - 1));
        }
        ///// <summary>
        ///// 一个字节转换为十六进制两个字符
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //internal static int ToHex2(int value)
        //{
        //    int high = value >> 4;
        //    value &= 15;
        //    if (high >= 10) high += ('A' - '9' - 1);
        //    if (value >= 10) value += ('A' - '9' - 1);
        //    return high + (value << 16) + ('0' + ('0' << 16));
        //}
        /// <summary>
        /// 16b 数字转换成4个16进制字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        internal unsafe static void ToHex4(uint value, char* chars)
        {
            *chars = (char)ToHex(value >> 12);
            *(chars + 1) = (char)ToHex((value >> 8) & 15);
            *(chars + 2) = (char)ToHex((value >> 4) & 15);
            *(chars + 3) = (char)ToHex(value & 15);
        }
        /// <summary>
        /// 数字转换成16进制字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        internal unsafe static char* GetToHex(uint value, char* chars)
        {
            if (value >= 0x100)
            {
                if (value >= 0x1000)
                {
                    ToHex4(value, chars);
                    return chars + 4;
                }
                *chars = (char)ToHex(value >> 8);
                *(chars + 1) = (char)ToHex((value >> 4) & 15);
                *(chars + 2) = (char)ToHex(value & 15);
                return chars + 3;
            }
            if (value >= 0x10)
            {
                *chars = (char)ToHex(value >> 4);
                *(chars + 1) = (char)ToHex(value & 15);
                return chars + 2;
            }
            *chars = (char)ToHex(value);
            return chars + 1;
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
        /// <summary>
        /// 填充第一个有效二进制位后面的空位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static uint FullBit(this uint value)
        {
            value |= value >> 16;
            value |= value >> 8;
            value |= value >> 4;
            value |= value >> 2;
            return value | (value >> 1);
        }
        /// <summary>
        /// 向上去 2 的幂次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static uint UpToPower2(this uint value)
        {
            return (value & (value - 1)) != 0 ? value.FullBit() + 1 : value;
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
        /// 获取二进制1位的个数
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>二进制1位的个数</returns>
        public static int bitCount(this ulong value)
        {
            value -= ((value >> 1) & 0x5555555555555555UL);//2:2
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);//4:4
            value += value >> 4;
            value &= 0x0f0f0f0f0f0f0f0fUL;//8:8

            value += (value >> 8);
            value += (value >> 16);
            return (byte)(value + (value >> 32));
        }
        ///// <summary>
        ///// 获取二进制1位的个数
        ///// </summary>
        ///// <param name="value">数据</param>
        ///// <returns>二进制1位的个数</returns>
        //public static int bitCount(this uint value)
        //{
        //    //return bitCounts[(byte)value] + bitCounts[(byte)(value >> 8)] + bitCounts[(byte)(value >> 16)] + bitCounts[value >> 24];

        //    //value = (value & 0x49249249) + ((value >> 1) & 0x49249249) + ((value >> 2) & 0x49249249);
        //    //value = (value & 0xc71c71c7) + ((value >> 3) & 0xc71c71c7);
        //    //uint div = (uint)(((ulong)value * (((1UL << 37) + 62) / 63)) >> 37);
        //    //return (int)(value - (div << 6) + div);

        //    //value = (value & 0x49249249) + ((value >> 1) & 0x49249249) + ((value >> 2) & 0x49249249);
        //    //value = (value & 0x71c71c7) + ((value >> 3) & 0x71c71c7) + (value >> 30);
        //    //uint nextValue = (uint)((value * 0x41041042UL) >> 36);
        //    //return (int)(value - (nextValue << 6) + nextValue);

        //    value -= ((value >> 1) & 0x55555555U);//2:2
        //    value = (value & 0x33333333U) + ((value >> 2) & 0x33333333U);//4:4
        //    value += value >> 4;
        //    value &= 0x0f0f0f0f;//8:8

        //    //uint div = (uint)(((ulong)value * (((1UL << 39) + 254) / 255)) >> 39);
        //    //return (int)(value - (div << 8) + div);
        //    value += (value >> 8);
        //    return (byte)(value + (value >> 16));
        //}
        /// <summary>
        /// 2^n相关32位deBruijn序列集合
        /// </summary>
        private static AutoCSer.Memory.Pointer deBruijn32;
        /// <summary>
        /// 2^n相关32位deBruijn序列
        /// </summary>
        public const uint DeBruijn32Number = 0x04653adfU;
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>有效位长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int bits(this uint value)
        {
            return (value & 0x80000000U) == 0 ? deBruijn32.Byte[((value.FullBit() + 1) * DeBruijn32Number) >> 27] : 32;
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
        /// 求 2 的 x 次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int DeBruijnLog2(this uint value)
        {
            return deBruijn32.Byte[(value * DeBruijn32Number) >> 27];
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

        static NumberExtension()
        {
            deBruijn32 = Unmanaged.GetStaticPointer(32, true);
            byte* deBruijn32Data = deBruijn32.Byte;
            for (byte bit = 0; bit != 32; ++bit) deBruijn32Data[((1U << bit) * DeBruijn32Number) >> 27] = bit;
        }
    }
}
