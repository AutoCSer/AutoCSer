using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class Parser
    {
        /// <summary>
        /// 32 位整数最大值
        /// </summary>
        private const uint maxInt32 = uint.MaxValue / 10;
        /// <summary>
        /// 64 位整数最大值
        /// </summary>
        private const ulong maxInt64 = ulong.MaxValue / 10;
        /// <summary>
        /// 整数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ParseIntState parseInt32(out uint value)
        {
            ParseIntState state = ParseIntState.None;
        START:
            switch (*Current & 15)
            {
                case '0' & 15:
                    if (*Current == '0')
                    {
                        if (++Current == end || *Current != 'x')
                        {
                            value = 0;
                            return state;
                        }
                        if (++Current == end)
                        {
                            value = 0;
                            ParseState = ParseState.CrashEnd;
                            return ParseIntState.Error;
                        }
                        return state | parseHex32(out value);
                    }
                    break;
                case '1' & 15:
                case '3' & 15:
                case '4' & 15:
                case '5' & 15:
                case '6' & 15:
                case '8' & 15:
                case '9' & 15:
                    if ((value = (uint)(*Current - '0')) < 10)
                    {
                        if (++Current != end) parseInt32Next(ref value);
                        return state;
                    }
                    break;
                case '2' & 15:
                    if (*Current == '2')
                    {
                        value = 2;
                        if (++Current != end) parseInt32Next(ref value);
                        return state;
                    }
                    else if (*Current == '"')
                    {
                        Quote = '"';
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                        {
                            ++Current;
                            return checkQuote(state |= parseInt32(out value));
                        }
                    }
                    break;
                case '7' & 15:
                    if (*Current == '7')
                    {
                        value = 7;
                        if (++Current != end) parseInt32Next(ref value);
                        return state;
                    }
                    else if (*Current == '\'')
                    {
                        Quote = '\'';
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                        {
                            ++Current;
                            return checkQuote(state |= parseInt32(out value));
                        }
                    }
                    break;
                case '-' & 15:
                    if (*Current == '-')
                    {
                        if (++Current == end)
                        {
                            value = 0;
                            ParseState = ParseState.CrashEnd;
                            return ParseIntState.Error;
                        }
                        if (state != ParseIntState.Negative)
                        {
                            state = ParseIntState.Negative;
                            goto START;
                        }
                        Quote = (char)1;
                    }
                    break;
                //case '+' & 15:
                case 'n' & 15:
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char)
                        && *(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                    {
                        if (state != ParseIntState.Negative)
                        {
                            value = 0;
                            Current += 4;
                            return ParseIntState.Null;
                        }
                        Quote = (char)1;
                    }
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (ParseState != ParseState.Success)
                    {
                        value = 0;
                        return ParseIntState.Error;
                    }
                    if (Current == end)
                    {
                        value = 0;
                        ParseState = ParseState.CrashEnd;
                        return ParseIntState.Error;
                    }
                    Quote = (char)1;
                    goto START;
                }
            }
            value = 0;
            ParseState = ParseState.NotInteger;
            return ParseIntState.Error;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns></returns>
        private ParseIntState parseHex32(out uint value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    value = 0;
                    ParseState = ParseState.NotHex;
                    return ParseIntState.Error;
                }
                number += 10;
            }
            value = number;
            if (++Current == end) return ParseIntState.None;
            if (isEndHex)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return ParseIntState.None;
                        number += 10;
                    }
                    value = (value << 4) + number;
                    if ((value & 0xf0000000U) != 0) return ParseIntState.None;
                }
                while (++Current != end);
            }
            else
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return ParseIntState.None;
                        number += 10;
                    }
                    value = (value << 4) + number;
                    ++Current;
                    if ((value & 0xf0000000U) != 0) return ParseIntState.None;
                }
                while (true);
            }
            return ParseIntState.None;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void parseInt32Next(ref uint value)
        {
            uint number;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9) return;
                    value = value * 10 + number;
                    if (++Current == end || value > maxInt32) return;
                }
                while (true);
            }
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value = value * 10 + number;
                ++Current;
                if (value > maxInt32) return;
            }
        }
        /// <summary>
        /// 检测整数字符串结束
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private ParseIntState checkQuote(ParseIntState state)
        {
            if ((state & ParseIntState.Error) == 0)
            {
                if ((state & ParseIntState.Null) == 0)
                {
                    if (Current >= end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return ParseIntState.Error;
                    }
                    if (*Current == Quote)
                    {
                        ++Current;
                        return state;
                    }
                }
                ParseState = ParseState.NotInteger;
            }
            return ParseIntState.Error;
        }
        /// <summary>
        /// 整数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ParseIntState parseInt64(out ulong value)
        {
            ParseIntState state = ParseIntState.None;
        START:
            switch (*Current & 15)
            {
                case '0' & 15:
                    if (*Current == '0')
                    {
                        if (++Current == end || *Current != 'x')
                        {
                            value = 0;
                            return state;
                        }
                        if (++Current == end)
                        {
                            value = 0;
                            ParseState = ParseState.CrashEnd;
                            return ParseIntState.Error;
                        }
                        return state | parseHex64(out value);
                    }
                    break;
                case '1' & 15:
                case '3' & 15:
                case '4' & 15:
                case '5' & 15:
                case '6' & 15:
                case '8' & 15:
                case '9' & 15:
                    if ((value = (uint)(*Current - '0')) < 10)
                    {
                        if (++Current != end) parseInt64Next(ref value);
                        return state;
                    }
                    break;
                case '2' & 15:
                    if (*Current == '2')
                    {
                        value = 2;
                        if (++Current != end) parseInt64Next(ref value);
                        return state;
                    }
                    else if (*Current == '"')
                    {
                        Quote = '"';
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                        {
                            ++Current;
                            return checkQuote(state |= parseInt64(out value));
                        }
                    }
                    break;
                case '7' & 15:
                    if (*Current == '7')
                    {
                        value = 7;
                        if (++Current != end) parseInt64Next(ref value);
                        return state;
                    }
                    else if (*Current == '\'')
                    {
                        Quote = '\'';
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                        {
                            ++Current;
                            return checkQuote(state |= parseInt64(out value));
                        }
                    }
                    break;
                case '-' & 15:
                    if (*Current == '-')
                    {
                        if (++Current == end)
                        {
                            value = 0;
                            ParseState = ParseState.CrashEnd;
                            return ParseIntState.Error;
                        }
                        if (state != ParseIntState.Negative)
                        {
                            state = ParseIntState.Negative;
                            goto START;
                        }
                        Quote = (char)1;
                    }
                    break;
                //case '+' & 15:
                case 'n' & 15:
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char)
                        && *(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                    {
                        if (state != ParseIntState.Negative)
                        {
                            value = 0;
                            Current += 4;
                            return ParseIntState.Null;
                        }
                        Quote = (char)1;
                    }
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (ParseState != ParseState.Success)
                    {
                        value = 0;
                        return ParseIntState.Error;
                    }
                    if (Current == end)
                    {
                        value = 0;
                        ParseState = ParseState.CrashEnd;
                        return ParseIntState.Error;
                    }
                    Quote = (char)1;
                    goto START;
                }
            }
            value = 0;
            ParseState = ParseState.NotInteger;
            return ParseIntState.Error;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns></returns>
        private ParseIntState parseHex64(out ulong value)
        {
            ulong number = (ulong)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    value = 0;
                    ParseState = ParseState.NotHex;
                    return ParseIntState.Error;
                }
                number += 10;
            }
            value = number;
            if (++Current == end) return ParseIntState.None;
            if (isEndHex)
            {
                do
                {
                    if ((number = (ulong)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfUL) > 5) return ParseIntState.None;
                        number += 10;
                    }
                    value = (value << 4) + number;
                    if ((value & 0xf000000000000000UL) != 0) return ParseIntState.None;
                }
                while (++Current != end);
            }
            else
            {
                do
                {
                    if ((number = (ulong)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfUL) > 5) return ParseIntState.None;
                        number += 10;
                    }
                    value = (value << 4) + number;
                    ++Current;
                    if ((value & 0xf000000000000000UL) != 0) return ParseIntState.None;
                }
                while (true);
            }
            return ParseIntState.None;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void parseInt64Next(ref ulong value)
        {
            ulong number;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (ulong)(*Current - '0')) > 9) return;
                    value = value * 10 + number;
                    if (++Current == end || value > maxInt64) return;
                }
                while (true);
            }
            while ((number = (ulong)(*Current - '0')) < 10)
            {
                value = value * 10 + number;
                ++Current;
                if (value > maxInt64) return;
            }
        }

        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref byte value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if (state == ParseIntState.None)
            {
                if ((value32 & 0xffffff00U) == 0)
                {
                    value = (byte)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) ParseState = ParseState.OutOfRange;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref byte? value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if (state == ParseIntState.None)
            {
                if ((value32 & 0xffffff00U) == 0)
                {
                    value = (byte)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0)
            {
                if (state == ParseIntState.Null) value = null;
                else ParseState = ParseState.OutOfRange;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref sbyte value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value32 & 0xffffff80U) == 0)
                    {
                        value = (sbyte)(byte)value32;
                        return;
                    }
                }
                else if (value32 <= 0xffffff80U)
                {
                    value = (sbyte)-(int)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) ParseState = ParseState.NotInteger;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref sbyte? value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value32 & 0xffffff80U) == 0)
                    {
                        value = (sbyte)(byte)value32;
                        return;
                    }
                }
                else if (value32 <= 0xffffff80U)
                {
                    value = (sbyte)-(int)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) value = null;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref ushort value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if (state == ParseIntState.None)
            {
                if ((value32 & 0xffff0000U) == 0)
                {
                    value = (ushort)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) ParseState = ParseState.OutOfRange;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref ushort? value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if (state == ParseIntState.None)
            {
                if ((value32 & 0xffff0000U) == 0)
                {
                    value = (ushort)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0)
            {
                if (state == ParseIntState.Null) value = null;
                else ParseState = ParseState.OutOfRange;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref short value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value32 & 0xffff8000U) == 0)
                    {
                        value = (short)(ushort)value32;
                        return;
                    }
                }
                else if (value32 <= 0xffff8000U)
                {
                    value = (short)-(int)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) ParseState = ParseState.NotInteger;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref short? value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value32 & 0xffff8000U) == 0)
                    {
                        value = (short)(ushort)value32;
                        return;
                    }
                }
                else if (value32 <= 0xffff8000U)
                {
                    value = (short)-(int)value32;
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) value = null;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref uint value)
        {
            Quote = (char)0;
            ParseIntState state = parseInt32(out value);
            if (state != ParseIntState.None && (state & ParseIntState.Error) == 0) ParseState = ParseState.OutOfRange;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref uint? value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if (state == ParseIntState.None)
            {
                value = value32;
                return;
            }
            else if ((state & ParseIntState.Error) == 0)
            {
                if (state == ParseIntState.Null) value = null;
                else ParseState = ParseState.OutOfRange;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref int value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value32 & 0x80000000U) == 0)
                    {
                        value = (int)value32;
                        return;
                    }
                }
                else if (value32 <= 0x80000000U)
                {
                    value = (int)(0U - value32);
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) ParseState = ParseState.NotInteger;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref int? value)
        {
            Quote = (char)0;
            uint value32;
            ParseIntState state = parseInt32(out value32);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value32 & 0x80000000U) == 0)
                    {
                        value = (int)value32;
                        return;
                    }
                }
                else if (value32 <= 0x80000000U)
                {
                    value = (int)(0U - value32);
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) value = null;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref ulong value)
        {
            Quote = (char)0;
            ParseIntState state = parseInt64(out value);
            if (state != ParseIntState.None && (state & ParseIntState.Error) == 0) ParseState = ParseState.OutOfRange;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref ulong? value)
        {
            Quote = (char)0;
            ulong value64;
            ParseIntState state = parseInt64(out value64);
            if (state == ParseIntState.None)
            {
                value = value64;
                return;
            }
            else if ((state & ParseIntState.Error) == 0)
            {
                if (state == ParseIntState.Null) value = null;
                else ParseState = ParseState.OutOfRange;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref long value)
        {
            Quote = (char)0;
            ulong value64;
            ParseIntState state = parseInt64(out value64);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value64 & 0x8000000000000000U) == 0)
                    {
                        value = (long)value64;
                        return;
                    }
                }
                else if (value64 <= 0x8000000000000000U)
                {
                    value = (long)(0UL - value64);
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) ParseState = ParseState.NotInteger;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref long? value)
        {
            Quote = (char)0;
            ulong value64;
            ParseIntState state = parseInt64(out value64);
            if ((state & (ParseIntState.Error | ParseIntState.Null)) == 0)
            {
                if (state == ParseIntState.None)
                {
                    if ((value64 & 0x8000000000000000U) == 0)
                    {
                        value = (long)value64;
                        return;
                    }
                }
                else if (value64 <= 0x8000000000000000U)
                {
                    value = (long)(0UL - value64);
                    return;
                }
                ParseState = ParseState.OutOfRange;
            }
            else if ((state & ParseIntState.Error) == 0) value = null;
        }
    }
}
