using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    internal static class Number
    {
        internal unsafe static int ToString(int value, byte* chars)
        {
            RangeLength range = toString(value, (char*)chars);
            char* read = (char*)chars + range.Start;
            byte* end = chars + range.Length;
            do
            {
                *chars = (byte)*read++;
            }
            while (++chars != end);
            return range.Length;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        private unsafe static RangeLength toString(int value, char* chars)
        {
            if (value >= 0) return toString((uint)value, chars);
            uint value32 = (uint)-value;
            if (value32 >= 100000000)
            {
                uint value100000000 = (uint)((value32 * (ulong)AutoCSer.Extension.Number.Div100000000Mul) >> AutoCSer.Extension.Number.Div100000000Shift);
                value32 -= value100000000 * 100000000;
                uint value10000 = (uint)((value32 * AutoCSer.Extension.Number.Div10000Mul) >> AutoCSer.Extension.Number.Div10000Shift);
                uIntToString(value10000, chars + 4);
                uIntToString(value32 - value10000 * 10000, chars + 8);
                if (value100000000 >= 10)
                {
                    value10000 = (value100000000 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)(chars + 2) = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *(chars + 1) = '-';
                    return new RangeLength(1, 11);
                }
                *(uint*)(chars + 2) = '-' + ((value100000000 + '0') << 16);
                return new RangeLength(2, 10);
            }
            return toString_99999999(value32, chars);
        }
        /// <summary>
        /// 绝对值小于100000000的负整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        private unsafe static RangeLength toString_99999999(uint value, char* chars)
        {
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * AutoCSer.Extension.Number.Div10000Mul) >> AutoCSer.Extension.Number.Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        uIntToString(value - value10000 * 10000, chars + 6);
                        value = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                        *(uint*)(chars + 4) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                        *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        *(chars + 1) = '-';
                        return new RangeLength(1, 9);
                    }
                    else
                    {
                        uIntToString(value - value10000 * 10000, chars + 4);
                        value = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        *(uint*)chars = '-' + ((value + '0') << 16);
                        return new RangeLength(0, 8);
                    }
                }
                if (value10000 >= 10)
                {
                    uIntToString(value - value10000 * 10000, chars + 4);
                    value = (value10000 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)(chars + 2) = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    *(chars + 1) = '-';
                    return new RangeLength(1, 7);
                }
                uIntToString(value - value10000 * 10000, chars + 2);
                *(uint*)chars = '-' + ((value10000 + '0') << 16);
                return new RangeLength(0, 6);
            }
            if (value >= 100)
            {
                if (value >= 1000)
                {
                    uint value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    uint value100 = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)(chars + 4) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)(chars + 2) = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    *(chars + 1) = '-';
                    return new RangeLength(1, 5);
                }
                else
                {
                    uint value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    uint value100 = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    *(uint*)chars = '-' + ((value100 + '0') << 16);
                    return new RangeLength(0, 4);
                }
            }
            if (value >= 10)
            {
                *chars = '-';
                uint value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return new RangeLength(0, 3);
            }
            *(uint*)chars = '-' + ((value + '0') << 16);
            return new RangeLength(0, 2);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        private unsafe static RangeLength toString(uint value, char* chars)
        {
            if (value >= 100000000)
            {
                uint value100000000 = (uint)((value * (ulong)AutoCSer.Extension.Number.Div100000000Mul) >> AutoCSer.Extension.Number.Div100000000Shift);
                value -= value100000000 * 100000000;
                uint value10000 = (uint)((value * AutoCSer.Extension.Number.Div10000Mul) >> AutoCSer.Extension.Number.Div10000Shift);
                uIntToString(value10000, chars + 2);
                uIntToString(value - value10000 * 10000, chars + 6);
                if (value100000000 >= 10)
                {
                    value10000 = (value100000000 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)chars = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    return new RangeLength(0, 10);
                }
                *(chars + 1) = (char)(value100000000 + '0');
                return new RangeLength(1, 9);
            }
            return new RangeLength(0, toString99999999(value, chars));
        }
        /// <summary>
        /// 小于100000000的正整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private unsafe static int toString99999999(uint value, char* chars)
        {
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * AutoCSer.Extension.Number.Div10000Mul) >> AutoCSer.Extension.Number.Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        uIntToString(value - value10000 * 10000, chars + 4);
                        value = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                        *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        return 8;
                    }
                    else
                    {
                        uIntToString(value - value10000 * 10000, chars + 3);
                        chars[2] = (char)((value10000 - value10 * 10) + '0');
                        value = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                        *(uint*)chars = ((value10 - value * 10) << 16) | value | 0x300030U;
                        return 7;
                    }
                }
                if (value10000 >= 10)
                {
                    uIntToString(value - value10000 * 10000, chars + 2);
                    value = (value10000 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)chars = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    return 6;
                }
                uIntToString(value - value10000 * 10000, chars + 1);
                chars[0] = (char)(value10000 + '0');
                return 5;
            }
            if (value >= 100)
            {
                uint value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                if (value >= 1000)
                {
                    uint value100 = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    return 4;
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    uint value100 = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                    *(uint*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030U;
                    return 3;
                }
            }
            if (value >= 10)
            {
                uint value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return 2;
            }
            *chars = (char)(value + '0');
            return 1;
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        private unsafe static void uIntToString(uint value, char* chars)
        {
            uint value10 = (value * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
            uint value100 = (value10 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
            *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
            value10 = (value100 * AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
            *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
        }
    }
}
