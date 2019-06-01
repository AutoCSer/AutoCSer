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
        /// 16位除以10转乘法的乘数
        /// </summary>
        public const uint Div10_16Mul = ((1 << 19) + 9) / 10;
        /// <summary>
        /// 16位除以10转乘法的位移
        /// </summary>
        public const int Div10_16Shift = 19;
        //public const int Div100_16Mul = ((1 << 22) + 99) / 100;
        //public const int Div100_16Shift = 22;
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
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static void ToString(byte value, CharStream charStream)
        {
            charStream.PrepLength(3);
            charStream.ByteSize += ToString(value, charStream.CurrentChar) * sizeof(char);
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static void ToString(sbyte value, CharStream charStream)
        {
            charStream.PrepLength(4);
            charStream.ByteSize += ToString(value, charStream.CurrentChar) * sizeof(char);
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static void ToString(ushort value, CharStream charStream)
        {
            charStream.PrepLength(5);
            charStream.ByteSize += ToString(value, charStream.CurrentChar) * sizeof(char);
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static void ToString(short value, CharStream charStream)
        {
            charStream.PrepLength(6);
            charStream.ByteSize += ToString(value, charStream.CurrentChar) * sizeof(char);
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static void ToString(uint value, CharStream charStream)
        {
            char* chars = charStream.GetPrepSizeCurrent(10 + 3);
            charStream.UnsafeSimpleWriteNotNull(chars, ToString(value, chars));
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
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value100000000 >= 10)
                {
                    uIntToString(value10000, chars + 2);
                    uIntToString(value - value10000 * 10000, chars + 6);
                    value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return 10;
                }
                uIntToString(value10000, chars + 1);
                uIntToString(value - value10000 * 10000, chars + 5);
                *chars = (char)(value100000000 + '0');
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
                        uIntToString(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        return 8;
                    }
                    else
                    {
                        uIntToString(value - value10000 * 10000, chars + 3);
                        chars[2] = (char)((value10000 - value10 * 10) + '0');
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10 - value * 10) << 16) | value | 0x300030U;
                        return 7;
                    }
                }
                if (value10000 >= 10)
                {
                    uIntToString(value - value10000 * 10000, chars + 2);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    return 6;
                }
                uIntToString(value - value10000 * 10000, chars + 1);
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe static void ToString(int value, CharStream charStream)
        {
            char* chars = charStream.GetPrepSizeCurrent(12 + 3);
            charStream.UnsafeSimpleWriteNotNull(chars, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal unsafe static int ToString(int value, char* chars)
        {
            if (value >= 0) return ToString((uint)value, chars);
            uint value32 = (uint)-value;
            if (value32 >= 100000000)
            {
                uint value100000000 = (uint)((value32 * (ulong)Div100000000Mul) >> Div100000000Shift);
                value32 -= value100000000 * 100000000;
                uint value10000 = (uint)((value32 * Div10000Mul) >> Div10000Shift);
                if (value100000000 >= 10)
                {
                    uIntToString(value10000, chars + 3);
                    uIntToString(value32 - value10000 * 10000, chars + 7);
                    value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *chars = '-';
                    return 11;
                }
                uIntToString(value10000, chars + 2);
                uIntToString(value32 - value10000 * 10000, chars + 6);
                *(uint*)chars = '-' + ((value100000000 + '0') << 16);
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
                        uIntToString(value - value10000 * 10000, chars + 5);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 3) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        *chars = '-';
                        return 9;
                    }
                    else
                    {
                        uIntToString(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        *(uint*)chars = '-' + ((value + '0') << 16);
                        return 8;
                    }
                }
                if (value10000 >= 10)
                {
                    uIntToString(value - value10000 * 10000, chars + 3);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    *chars = '-';
                    return 7;
                }
                uIntToString(value - value10000 * 10000, chars + 2);
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
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        private unsafe static void uIntToString(uint value, char* chars)
        {
            uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
            value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe static string toString(this ulong value)
        {
            char* chars = stackalloc char[20];
            RangeLength index = ToString(value, chars);
            return new string(chars + index.Start, 0, index.Length);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe static void ToString(ulong value, CharStream charStream)
        {
            charStream.PrepLength(20 + 3);
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
            char* chars = charStream.CurrentChar;
            RangeLength index = ToString(value, chars);
            charStream.UnsafeSimpleWriteNotNull(chars + index.Start, index.Length);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal unsafe static RangeLength ToString(ulong value, char* chars)
        {
            if (value >= 10000000000000000L)
            {
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                uIntToString(value10000, chars + 12);
                uIntToString((uint)value - value10000 * 10000U, chars + 16);
                value = value100000000 / 100000000;
                value100000000 -= value * 100000000;
                value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                uIntToString(value10000, chars + 4);
                uIntToString((uint)value100000000 - value10000 * 10000U, chars + 8);
                uint value32 = (uint)value;
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    if (value32 >= 1000)
                    {
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        return new RangeLength(0, 20);
                    }
                    *(chars + 1) = (char)(value100 + '0');
                    return new RangeLength(1, 19);
                }
                if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return new RangeLength(2, 18);
                }
                *(chars + 3) = (char)(value32 + '0');
                return new RangeLength(3, 17);
            }
            if (value >= 100000000)
            {
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                uIntToString(value10000, chars + 8);
                uIntToString((uint)value - value10000 * 10000U, chars + 12);
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                    uIntToString(value32 - value10000 * 10000, chars + 4);
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                        if (value10000 >= 1000)
                        {
                            *(uint*)(chars + 2) = ((value10000 - value32 * 10) << 16) | (value32 - value100 * 10) | 0x300030U;
                            value32 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)chars = ((value100 - value32 * 10) << 16) | value32 | 0x300030U;
                            return new RangeLength(0, 16);
                        }
                        else
                        {
                            chars[3] = (char)((value10000 - value32 * 10) + '0');
                            *(uint*)(chars + 1) = ((value32 - value100 * 10) << 16) | value100 | 0x300030U;
                            return new RangeLength(1, 15);
                        }
                    }
                    if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value32 * 10) << 16) | value32 | 0x300030U;
                        return new RangeLength(2, 14);
                    }
                    *(chars + 3) = (char)(value10000 + '0');
                    return new RangeLength(3, 13);
                }
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        *(uint*)(chars + 6) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        return new RangeLength(4, 12);
                    }
                    else
                    {
                        chars[7] = (char)((value32 - value10000 * 10) + '0');
                        *(uint*)(chars + 5) = ((value10000 - value100 * 10) << 16) | value100 | 0x300030U;
                        return new RangeLength(5, 11);
                    }
                }
                if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 6) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return new RangeLength(6, 10);
                }
                *(chars + 7) = (char)(value32 + '0');
                return new RangeLength(7, 9);
            }
            return new RangeLength(0, toString99999999((uint)value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe static string toString(this long value)
        {
            char* chars = stackalloc char[22 + 2];
            RangeLength index = ToString(value, chars);
            return new string(chars + index.Start, 0, index.Length);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe static void ToString(long value, CharStream charStream)
        {
            charStream.PrepLength(22 + 3);
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
            char* chars = charStream.CurrentChar;
            RangeLength index = ToString(value, chars);
            charStream.UnsafeSimpleWriteNotNull(chars + index.Start, index.Length);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal unsafe static RangeLength ToString(long value, char* chars)
        {
            if (value >= 0) return ToString((ulong)value, chars);
            ulong value64 = (ulong)-value;
            if (value64 >= 10000000000000000L)
            {
                ulong value100000000 = value64 / 100000000;
                value64 -= value100000000 * 100000000;
                uint value10000 = (uint)((value64 * Div10000Mul) >> Div10000Shift);
                uIntToString(value10000, chars + 14);
                uIntToString((uint)value64 - value10000 * 10000U, chars + 18);
                value64 = value100000000 / 100000000;
                value100000000 -= value64 * 100000000;
                value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                uIntToString(value10000, chars + 6);
                uIntToString((uint)value100000000 - value10000 * 10000U, chars + 10);
                uint value32 = (uint)value64;
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    if (value32 >= 1000)
                    {
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        *(chars + 1) = '-';
                        return new RangeLength(1, 21);
                    }
                    *(uint*)(chars + 2) = '-' + ((value100 + '0') << 16);
                    return new RangeLength(2, 20);
                }
                if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 4) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *(chars + 3) = '-';
                    return new RangeLength(3, 19);
                }
                *(uint*)(chars + 4) = '-' + ((value32 + '0') << 16);
                return new RangeLength(4, 18);
            }
            if (value64 >= 100000000)
            {
                ulong value100000000 = value64 / 100000000;
                value64 -= value100000000 * 100000000;
                uint value10000 = (uint)((value64 * Div10000Mul) >> Div10000Shift);
                uIntToString(value10000, chars + 10);
                uIntToString((uint)value64 - value10000 * 10000U, chars + 14);
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift);
                    uIntToString(value32 - value10000 * 10000, chars + 6);
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value10000 - value32 * 10) << 16) | (value32 - value100 * 10) | 0x300030U;
                        if (value10000 >= 1000)
                        {
                            value32 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)(chars + 2) = ((value100 - value32 * 10) << 16) | value32 | 0x300030U;
                            *(chars + 1) = '-';
                            return new RangeLength(1, 17);
                        }
                        *(uint*)(chars + 2) = '-' + ((value100 + '0') << 16);
                        return new RangeLength(2, 16);
                    }
                    if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 4) = ((value10000 - value32 * 10) << 16) | value32 | 0x300030U;
                        *(chars + 3) = '-';
                        return new RangeLength(3, 15);
                    }
                    *(uint*)(chars + 4) = '-' + ((value10000 + '0') << 16);
                    return new RangeLength(4, 14);
                }
                if (value32 >= 100)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 8) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    if (value32 >= 1000)
                    {
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 6) = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        *(chars + 5) = '-';
                        return new RangeLength(5, 13);
                    }
                    *(uint*)(chars + 6) = '-' + ((value100 + '0') << 16);
                    return new RangeLength(6, 12);
                }
                if (value32 >= 10)
                {
                    value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 8) = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *(chars + 7) = '-';
                    return new RangeLength(7, 11);
                }
                *(uint*)(chars + 8) = '-' + ((value32 + '0') << 16);
                return new RangeLength(8, 10);
            }
            return new RangeLength(0, toString_99999999((uint)value64, chars));
        }

        /// <summary>
        /// 数字转换成16进制字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        internal unsafe static void ToHex16(uint value, char* chars)
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
                    ToHex16(value, chars);
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
        /// 半字节转十六进制字符
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static uint ToHex(uint data)
        {
            return data < 10 ? data + '0' : (data + ('0' + 'A' - '9' - 1));
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
        private static Pointer deBruijn32;
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
        /// 求 2 的 x 次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int DeBruijnLog2(this uint value)
        {
            return deBruijn32.Byte[(value * DeBruijn32Number) >> 27];
        }
        static Number()
        {
            deBruijn32 = new Pointer { Data = Unmanaged.GetStatic64(32, true) };
            byte* deBruijn32Data = deBruijn32.Byte;
            for (byte bit = 0; bit != 32; ++bit) deBruijn32Data[((1U << bit) * DeBruijn32Number) >> 27] = bit;
        }
    }
}
