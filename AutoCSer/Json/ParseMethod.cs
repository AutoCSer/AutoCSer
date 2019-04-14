using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 基本类型解析函数配置
    /// </summary>
    internal sealed class ParseMethod : Attribute { }
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class Parser
    {
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>解析状态</returns>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref bool value)
        {
            byte isSpace = 0;
        START:
            switch (*Current & 7)
            {
                case 'f' & 7:
                    if (*(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48)
                        && (int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                    {
                        value = false;
                        Current += 5;
                        return;
                    }
                    break;
                case 't' & 7:
                    if (*(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48)
                        && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                    {
                        value = true;
                        Current += 4;
                        return;
                    }
                    break;
                case '0' & 7:
                    if (*Current == '0')
                    {
                        value = false;
                        ++Current;
                        return;
                    }
                    break;
                case '1' & 7:
                    if (*Current == '1')
                    {
                        value = true;
                        ++Current;
                        return;
                    }
                    break;
                case '"' & 7:
                case '\'' & 7:
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((uint)(*Current - '0') < 2) value = *(byte*)Current++ != '0';
                        else if (*Current == 'f')
                        {
                            if (*(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48) && (int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                            {
                                value = false;
                                Current += 5;
                            }
                            else
                            {
                                ParseState = ParseState.NotBool;
                                return;
                            }
                        }
                        else if (*(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                        {
                            value = true;
                            Current += 4;
                        }
                        else
                        {
                            ParseState = ParseState.NotBool;
                            return;
                        }
                        if (Current == end) ParseState = ParseState.CrashEnd;
                        else if (*Current == Quote) ++Current;
                        else ParseState = ParseState.NotBool;
                        return;
                    }
                    break;
            }
            if (isSpace == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    isSpace = 1;
                    goto START;
                }
            }
            ParseState = ParseState.NotBool;
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>解析状态</returns>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref bool? value)
        {
            byte isSpace = 0;
        START:
            switch (*Current & 7)
            {
                case 'f' & 7:
                    if (*Current == 'f')
                    {
                        if (*(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48)
                            && (int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                        {
                            value = false;
                            Current += 5;
                            return;
                        }
                    }
                    else if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48)
                        && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                    {
                        value = null;
                        Current += 4;
                        return;
                    }
                    break;
                case 't' & 7:
                    if (*(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48)
                        && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                    {
                        value = true;
                        Current += 4;
                        return;
                    }
                    break;
                case '0' & 7:
                    if (*Current == '0')
                    {
                        value = false;
                        ++Current;
                        return;
                    }
                    break;
                case '1' & 7:
                    if (*Current == '1')
                    {
                        value = true;
                        ++Current;
                        return;
                    }
                    break;
                case '"' & 7:
                case '\'' & 7:
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((uint)(*Current - '0') < 2) value = *(byte*)Current++ != '0';
                        else if (*Current == 'f')
                        {
                            if (*(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48) && (int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                            {
                                value = false;
                                Current += 5;
                            }
                            else
                            {
                                ParseState = ParseState.NotBool;
                                return;
                            }
                        }
                        else if (*(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                        {
                            value = true;
                            Current += 4;
                        }
                        else
                        {
                            ParseState = ParseState.NotBool;
                            return;
                        }
                        if (Current == end) ParseState = ParseState.CrashEnd;
                        else if (*Current == Quote) ++Current;
                        else ParseState = ParseState.NotBool;
                        return;
                    }
                    break;
            }
            if (isSpace == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    isSpace = 1;
                    goto START;
                }
            }
            ParseState = ParseState.NotBool;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref byte value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            parseHex32(ref number);
                            value = (byte)number;
                        }
                        else value = (byte)parseUInt32(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = (byte)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = (byte)number;
                return;
            }
            value = (byte)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref byte? value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (isNull())
                    {
                        value = null;
                        return;
                    }
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            parseHex32(ref number);
                            value = (byte)number;
                        }
                        else value = (byte)parseUInt32(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = (byte)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = (byte)number;
                return;
            }
            value = (byte)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref sbyte value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                parseHex32(ref number);
                                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                            }
                            else value = sign == 0 ? (sbyte)(byte)parseUInt32(number) : (sbyte)-(int)parseUInt32(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            value = sign == 0 ? (sbyte)(byte)parseUInt32(number) : (sbyte)-(int)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref sbyte? value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (isNull())
                        {
                            value = null;
                            return;
                        }
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                parseHex32(ref number);
                                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                            }
                            else value = sign == 0 ? (sbyte)(byte)parseUInt32(number) : (sbyte)-(int)parseUInt32(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            value = sign == 0 ? (sbyte)(byte)parseUInt32(number) : (sbyte)-(int)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ushort value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            parseHex32(ref number);
                            value = (ushort)number;
                        }
                        else value = (ushort)parseUInt32(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = (ushort)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = (ushort)number;
                return;
            }
            value = (ushort)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ushort? value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (isNull())
                    {
                        value = null;
                        return;
                    }
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            parseHex32(ref number);
                            value = (ushort)number;
                        }
                        else value = (ushort)parseUInt32(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = (ushort)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = (ushort)number;
                return;
            }
            value = (ushort)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref short value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                parseHex32(ref number);
                                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                            }
                            else value = sign == 0 ? (short)(ushort)parseUInt32(number) : (short)-(int)parseUInt32(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            value = sign == 0 ? (short)(ushort)parseUInt32(number) : (short)-(int)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref short? value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (isNull())
                        {
                            value = null;
                            return;
                        }
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                parseHex32(ref number);
                                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                            }
                            else value = sign == 0 ? (short)(ushort)parseUInt32(number) : (short)-(int)parseUInt32(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            value = sign == 0 ? (short)(ushort)parseUInt32(number) : (short)-(int)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref uint value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            parseHex32(ref number);
                            value = number;
                        }
                        else value = parseUInt32(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = number;
                return;
            }
            value = parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref uint? value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (isNull())
                    {
                        value = null;
                        return;
                    }
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            parseHex32(ref number);
                            value = number;
                        }
                        else value = parseUInt32(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = number;
                return;
            }
            value = parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref int value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                parseHex32(ref number);
                                value = sign == 0 ? (int)number : -(int)number;
                            }
                            else value = sign == 0 ? (int)parseUInt32(number) : -(int)parseUInt32(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            value = sign == 0 ? (int)parseUInt32(number) : -(int)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref int? value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (isNull())
                        {
                            value = null;
                            return;
                        }
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                parseHex32(ref number);
                                value = sign == 0 ? (int)number : -(int)number;
                            }
                            else value = sign == 0 ? (int)parseUInt32(number) : -(int)parseUInt32(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            value = sign == 0 ? (int)parseUInt32(number) : -(int)parseUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ulong value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            value = parseHex64();
                        }
                        else value = parseUInt64(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                value = parseHex64();
                return;
            }
            value = parseUInt64(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ulong? value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (isNull())
                    {
                        value = null;
                        return;
                    }
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (number == 0)
                        {
                            if (*Current == Quote)
                            {
                                value = 0;
                                ++Current;
                                return;
                            }
                            if (*Current != 'x')
                            {
                                ParseState = ParseState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            value = parseHex64();
                        }
                        else value = parseUInt64(number);
                        if (ParseState == ParseState.Success)
                        {
                            if (Current == end) ParseState = ParseState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else ParseState = ParseState.NotNumber;
                        }
                        return;
                    }
                    ParseState = ParseState.NotNumber;
                    return;
                }
            }
            if (++Current == end)
            {
                value = number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                value = parseHex64();
                return;
            }
            value = parseUInt64(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref long value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                value = (long)parseHex64();
                            }
                            else value = (long)parseUInt64(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote)
                                {
                                    if (sign != 0) value = -value;
                                    ++Current;
                                }
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (long)(int)number : -(long)(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                value = (long)parseHex64();
                if (sign != 0) value = -value;
                return;
            }
            value = (long)parseUInt64(number);
            if (sign != 0) value = -value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref long? value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (isNull())
                {
                    value = null;
                    return;
                }
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        ParseState = ParseState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (isNull())
                        {
                            value = null;
                            return;
                        }
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (number == 0)
                            {
                                if (*Current == Quote)
                                {
                                    value = 0;
                                    ++Current;
                                    return;
                                }
                                if (*Current != 'x')
                                {
                                    ParseState = ParseState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    ParseState = ParseState.CrashEnd;
                                    return;
                                }
                                value = (long)parseHex64();
                            }
                            else value = (long)parseUInt64(number);
                            if (ParseState == ParseState.Success)
                            {
                                if (Current == end) ParseState = ParseState.CrashEnd;
                                else if (*Current == Quote)
                                {
                                    if (sign != 0) value = -value;
                                    ++Current;
                                }
                                else ParseState = ParseState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            ParseState = ParseState.NotNumber;
                            return;
                        }
                        sign = 1;
                    }
                }
            }
            if (++Current == end)
            {
                value = sign == 0 ? (long)(int)number : -(long)(int)number;
                return;
            }
            if (number == 0)
            {
                if (*Current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                long hexValue = (long)parseHex64();
                value = sign == 0 ? hexValue : -hexValue;
                return;
            }
            long value64 = (long)parseUInt64(number);
            value = sign == 0 ? value64 : -value64;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref float value)
        {
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberType.Number:
                    if (float.TryParse(getNumberString(end), out value)) Current = end;
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.String:
                    if (float.TryParse(getNumberString(end), out value)) Current = end + 1;
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.NaN: value = float.NaN; return;
                case NumberType.PositiveInfinity: value = float.PositiveInfinity; return;
                case NumberType.NegativeInfinity: value = float.NegativeInfinity; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref float? value)
        {
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberType.Number:
                    float parseValue;
                    if (float.TryParse(getNumberString(end), out parseValue))
                    {
                        Current = end;
                        value = parseValue;
                    }
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.String:
                    float parseStringValue;
                    if (float.TryParse(getNumberString(end), out parseStringValue))
                    {
                        Current = end + 1;
                        value = parseStringValue;
                    }
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.NaN: value = float.NaN; return;
                case NumberType.PositiveInfinity: value = float.PositiveInfinity; return;
                case NumberType.NegativeInfinity: value = float.NegativeInfinity; return;
                case NumberType.Null: value = null; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref double value)
        {
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberType.Number:
                    if (double.TryParse(getNumberString(end), out value)) Current = end;
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.String:
                    if (double.TryParse(getNumberString(end), out value)) Current = end + 1;
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.NaN: value = double.NaN; return;
                case NumberType.PositiveInfinity: value = double.PositiveInfinity; return;
                case NumberType.NegativeInfinity: value = double.NegativeInfinity; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref double? value)
        {
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberType.Number:
                    double parseValue;
                    if (double.TryParse(getNumberString(end), out parseValue))
                    {
                        Current = end;
                        value = parseValue;
                    }
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.String:
                    double parseStringValue;
                    if (double.TryParse(getNumberString(end), out parseStringValue))
                    {
                        Current = end + 1;
                        value = parseStringValue;
                    }
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.NaN: value = double.NaN; return;
                case NumberType.PositiveInfinity: value = double.PositiveInfinity; return;
                case NumberType.NegativeInfinity: value = double.NegativeInfinity; return;
                case NumberType.Null: value = null; return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref decimal value)
        {
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberType.Number:
                    if (decimal.TryParse(getNumberString(end), out value)) Current = end;
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.String:
                    if (decimal.TryParse(getNumberString(end), out value)) Current = end + 1;
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.NaN:
                case NumberType.PositiveInfinity:
                case NumberType.NegativeInfinity:
                    ParseState = ParseState.NotNumber;
                    return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref decimal? value)
        {
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberType.Number:
                    decimal parseValue;
                    if (decimal.TryParse(getNumberString(end), out parseValue))
                    {
                        Current = end;
                        value = parseValue;
                    }
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.String:
                    decimal parseStringValue;
                    if (decimal.TryParse(getNumberString(end), out parseStringValue))
                    {
                        Current = end + 1;
                        value = parseStringValue;
                    }
                    else ParseState = ParseState.NotNumber;
                    return;
                case NumberType.NaN:
                case NumberType.PositiveInfinity:
                case NumberType.NegativeInfinity:
                    ParseState = ParseState.NotNumber;
                    return;
                case NumberType.Null: value = null; return;
            }
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref char value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == '\\')
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (*Current == 'u')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                            {
                                ParseState = ParseState.NotChar;
                                return;
                            }
                            value = (char)parseHex4();
                        }
                        else if (*Current == 'x')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                            {
                                ParseState = ParseState.NotChar;
                                return;
                            }
                            value = (char)parseHex2();
                        }
                        else value = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
                    }
                    else
                    {
                        if (*Current == Quote)
                        {
                            ParseState = ParseState.NotChar;
                            return;
                        }
                        value = *Current;
                    }
                }
                else value = *Current;
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if (*Current == Quote)
                {
                    ++Current;
                    return;
                }
            }
            else if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotChar;
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref char? value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == '\\')
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (*Current == 'u')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                            {
                                ParseState = ParseState.NotChar;
                                return;
                            }
                            value = (char)parseHex4();
                        }
                        else if (*Current == 'x')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                            {
                                ParseState = ParseState.NotChar;
                                return;
                            }
                            value = (char)parseHex2();
                        }
                        else value = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
                    }
                    else
                    {
                        if (*Current == Quote)
                        {
                            ParseState = ParseState.NotChar;
                            return;
                        }
                        value = *Current;
                    }
                }
                else value = *Current;
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                if (*Current == Quote)
                {
                    ++Current;
                    return;
                }
            }
            else
            {
                if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                {
                    value = null;
                    Current += 4;
                    return;
                }
                if (isSpace == 0)
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    isSpace = 1;
                    goto START;
                }
            }
            ParseState = ParseState.NotChar;
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref DateTime value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                string timeString = parseString();
                if (!string.IsNullOrEmpty(timeString))
                {
                    DateTime parseTime;
                    if (timeString[0] == '/')
                    {
                        if (Parser.parseTime(timeString, out parseTime))
                        {
                            value = parseTime;
                            return;
                        }
                    }
                    else
                    {
                        if (DateTime.TryParse(timeString, out parseTime))
                        {
                            value = parseTime;
                            return;
                        }
                    }
                }
            }
            else
            {
                if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                {
                    if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                    {
                        value = DateTime.MinValue;
                        Current += 4;
                        return;
                    }
                    if (*Current == 'n' && (int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                    {
                        long millisecond = 0;
                        Current += 9;
                        Parse(ref millisecond);
                        if (ParseState != ParseState.Success) return;
                        if (Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (*Current == ')')
                        {
                            value = JavascriptLocalMinTime.AddTicks(millisecond * TimeSpan.TicksPerMillisecond);
                            ++Current;
                            return;
                        }
                    }
                }
                if (isSpace == 0)
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    isSpace = 1;
                    goto START;
                }
            }
            ParseState = ParseState.NotDateTime;
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref DateTime? value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                string timeString = parseString();
                if (!string.IsNullOrEmpty(timeString))
                {
                    DateTime parseTime;
                    if (timeString[0] == '/')
                    {
                        if (Parser.parseTime(timeString, out parseTime))
                        {
                            value = parseTime;
                            return;
                        }
                    }
                    else
                    {
                        if (DateTime.TryParse(timeString, out parseTime))
                        {
                            value = parseTime;
                            return;
                        }
                    }
                }
            }
            else
            {
                if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                {
                    if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                    {
                        value = null;
                        Current += 4;
                        return;
                    }
                    if (*Current == 'n' && (int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                    {
                        long millisecond = 0;
                        Current += 9;
                        Parse(ref millisecond);
                        if (ParseState != ParseState.Success) return;
                        if (Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        if (*Current == ')')
                        {
                            value = JavascriptLocalMinTime.AddTicks(millisecond * TimeSpan.TicksPerMillisecond);
                            ++Current;
                            return;
                        }
                    }
                }
                if (isSpace == 0)
                {
                    space();
                    if (ParseState != ParseState.Success) return;
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    isSpace = 1;
                    goto START;
                }
            }
            ParseState = ParseState.NotDateTime;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref System.Guid value)
        {
            if (*Current == '\'' || *Current == '"')
            {
                GuidCreator guid = new GuidCreator();
                parse(ref guid);
                value = guid.Value;
                return;
            }
            space();
            if (ParseState != ParseState.Success) return;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            if (*Current == '\'' || *Current == '"')
            {
                GuidCreator guid = new GuidCreator();
                parse(ref guid);
                value = guid.Value;
                return;
            }
            ParseState = ParseState.NotGuid;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref System.Guid? value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                GuidCreator guid = new GuidCreator();
                parse(ref guid);
                value = guid.Value;
                return;
            }
            if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value = null;
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotGuid;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref string value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                value = parseString();
                return;
            }
            if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value = null;
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotString;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref SubString value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                char* start = Current;
                if (searchEscape() == 0)
                {
                    if (ParseState == ParseState.Success)
                    {
                        if (this.json == null) value = new string(start, 0, (int)(Current++ - start));
                        else value.Set(this.json, (int)(start - jsonFixed), (int)(Current++ - start));
                    }
                    return;
                }
                if (Config.IsTempString && this.json != null)
                {
                    char* writeEnd = parseEscape();
                    if (writeEnd != null) value.Set(this.json, (int)(start - jsonFixed), (int)(writeEnd - start));
                }
                else
                {
                    string newValue = parseEscape(start);
                    if (newValue != null) value.Set(newValue, 0, newValue.Length);
                }
                return;
            }
            if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value.SetNull();
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotString;
        }
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref object value)
        {
            Node node = default(Node);
            Parse(ref node);
            if (ParseState == ParseState.Success)
            {
                if (node.Type == NodeType.Null) value = null;
                else value = node;
            }
            //if (isNull())
            //{
            //    value = null;
            //    return;
            //}
            //space();
            //if (ParseState != ParseState.Success) return;
            //if (Current == end)
            //{
            //    ParseState = ParseState.CrashEnd;
            //    return;
            //}
            //if (isNull())
            //{
            //    value = null;
            //    return;
            //}
            //Ignore();
            //if (ParseState == ParseState.Success) value = new object();
        }
        /// <summary>
        /// 类型解析
        /// </summary>
        /// <param name="type">类型</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref Type type)
        {
            byte isSpace = 0;
        START:
            if (*Current == '{')
            {
                RemoteType remoteType = default(RemoteType);
                TypeParser<RemoteType>.Parse(this, ref remoteType);
                if (ParseState != ParseState.Success) return;
                if (!remoteType.TryGet(out type)) ParseState = ParseState.ErrorType;
                return;
            }
            if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                type = null;
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.ErrorType;
        }
        /// <summary>
        /// JSON节点解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref Node value)
        {
            LeftArray<Node> nodeArray = default(LeftArray<Node>), nameArray = default(LeftArray<Node>);
            NEXTNODE:
            space();
            if (ParseState != ParseState.Success) return;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            switch (*Current & 7)
            {
                case '"' & 7:
                case '\'' & 7:
                    if (*Current == '"' || *Current == '\'')
                    {
                        parseStringNode(ref value);
                        if (ParseState == ParseState.Success) goto CHECKNODE;
                        return;
                    }
                    goto NUMBER;
                case '{' & 7:
                    if (*Current == '{')
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        value.SetDictionary();
                        if (IsFirstObject())
                        {
                            nodeArray.Add(value);
                            goto DICTIONARYNAME;
                        }
                        if (ParseState == ParseState.Success) goto CHECKNODE;
                        return;
                    }
                    if (*Current == '[')
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        value.SetList();
                        if (IsFirstArrayValue())
                        {
                            nodeArray.Add(value);
                            value = default(Node);
                            goto NEXTNODE;
                        }
                        if (ParseState == ParseState.Success) goto CHECKNODE;
                        return;
                    }
                    goto NUMBER;
                case 't' & 7:
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
                    {
                        Current += 4;
                        value.Int64 = 1;
                        value.Type = NodeType.Bool;
                        goto CHECKNODE;
                    }
                    goto NUMBER;
                case 'f' & 7:
                    if (*Current == 'f')
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char) && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
                        {
                            Current += 5;
                            value.Int64 = 0;
                            value.Type = NodeType.Bool;
                            goto CHECKNODE;
                        }
                        break;
                    }
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                    {
                        if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                        {
                            value.Type = NodeType.Null;
                            Current += 4;
                            goto CHECKNODE;
                        }
                        if ((int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                        {
                            long millisecond = 0;
                            Current += 9;
                            Parse(ref millisecond);
                            if (ParseState != ParseState.Success) return;
                            if (Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (*Current == ')')
                            {
                                value.Int64 = JavascriptLocalMinTimeTicks + millisecond * TimeSpan.TicksPerMillisecond;
                                value.Type = NodeType.DateTimeTick;
                                ++Current;
                                goto CHECKNODE;
                            }
                            break;
                        }
                    }
                    goto NUMBER;
                default:
                    NUMBER:
                    char* numberEnd = null;
                    switch (searchNumber(ref numberEnd))
                    {
                        case NumberType.Number:
                            if (json == null) value.SubString = new string(Current, 0, (int)(numberEnd - Current));
                            else value.SubString.Set(this.json, (int)(Current - jsonFixed), (int)(numberEnd - Current));
                            Current = numberEnd;
                            value.SetNumberString(Quote);
                            goto CHECKNODE;
                        case NumberType.String:
                            if (json == null) value.SubString = new string(Current, 0, (int)(numberEnd - Current));
                            else value.SubString.Set(this.json, (int)(Current - jsonFixed), (int)(numberEnd - Current));
                            Current = numberEnd + 1;
                            value.SetNumberString(Quote);
                            goto CHECKNODE;
                        case NumberType.NaN: value.Type = NodeType.NaN; goto CHECKNODE;
                        case NumberType.PositiveInfinity: value.Type = NodeType.PositiveInfinity; goto CHECKNODE;
                        case NumberType.NegativeInfinity: value.Type = NodeType.NegativeInfinity; goto CHECKNODE;
                    }
                    break;
            }
            ParseState = ParseState.UnknownValue;
            return;

            DICTIONARYNAME:
            if (*Current == '"' || *Current == '\'') parseStringNode(ref value);
            else
            {
                char* nameStart = Current;
                SearchNameEnd();
                if (this.json == null)
                {
                    int length = (int)(Current - nameStart);
                    if (length == 0) value.SubString.Set(string.Empty, 0, 0);
                    else value.SubString = new string(nameStart, 0, length);
                }
                else value.SubString.Set(this.json, (int)(nameStart - jsonFixed), (int)(Current - nameStart));
                value.Type = NodeType.String;
            }
            if (ParseState != ParseState.Success || SearchColon() == 0) return;
            nameArray.Add(value);
            goto NEXTNODE;

            CHECKNODE:
            if (nodeArray.Length != 0)
            {
                Node parentNode = nodeArray.Array[nodeArray.Length - 1];
                switch (parentNode.Type)
                {
                    case NodeType.Dictionary:
                        LeftArray<KeyValue<Node, Node>> dictionary = parentNode.Dictionary;
                        dictionary.Add(new KeyValue<Node, Node>(ref nameArray.Array[--nameArray.Length], ref value));
                        if (IsNextObject())
                        {
                            nodeArray.Array[nodeArray.Length - 1].SetDictionary(ref dictionary);
                            goto DICTIONARYNAME;
                        }
                        value.SetDictionary(ref dictionary);
                        --nodeArray.Length;
                        goto CHECKNODE;
                    case NodeType.Array:
                        LeftArray<Node> list = parentNode.Array;
                        list.Add(value);
                        if (IsNextArrayValue())
                        {
                            nodeArray.Array[nodeArray.Length - 1].SetList(ref list);
                            goto NEXTNODE;
                        }
                        value.SetList(ref list);
                        --nodeArray.Length;
                        goto CHECKNODE;
                }
            }
        }
        //[ParseMethod]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        //public void Parse(ref Node value)
        //{
        //    space();
        //    if (ParseState != ParseState.Success) return;
        //    if (Current == end)
        //    {
        //        ParseState = ParseState.CrashEnd;
        //        return;
        //    }
        //    switch (*Current & 7)
        //    {
        //        case '"' & 7:
        //        case '\'' & 7:
        //            if (*Current == '"' || *Current == '\'')
        //            {
        //                parseStringNode(ref value);
        //                return;
        //            }
        //            goto NUMBER;
        //        case '{' & 7:
        //            if (*Current == '{')
        //            {
        //                LeftArray<KeyValue<Node, Node>> dictionary = default(LeftArray<KeyValue<Node, Node>>);
        //                parseDictionaryNode(ref dictionary);
        //                if (ParseState == ParseState.Success) value.SetDictionary(ref dictionary);
        //                return;
        //            }
        //            if (*Current == '[')
        //            {
        //                LeftArray<Node> list = default(LeftArray<Node>);
        //                parseListNode(ref list);
        //                if (ParseState == ParseState.Success) value.SetList(ref list);
        //                {
        //                    value.Type = NodeType.Array;
        //                }
        //                return;
        //            }
        //            goto NUMBER;
        //        case 't' & 7:
        //            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
        //            {
        //                Current += 4;
        //                value.Int64 = 1;
        //                value.Type = NodeType.Bool;
        //                return;
        //            }
        //            goto NUMBER;
        //        case 'f' & 7:
        //            if (*Current == 'f')
        //            {
        //                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char) && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
        //                {
        //                    Current += 5;
        //                    value.Int64 = 0;
        //                    value.Type = NodeType.Bool;
        //                    return;
        //                }
        //                break;
        //            }
        //            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
        //            {
        //                if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
        //                {
        //                    value.Type = NodeType.Null;
        //                    Current += 4;
        //                    return;
        //                }
        //                if ((int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
        //                {
        //                    long millisecond = 0;
        //                    Current += 9;
        //                    Parse(ref millisecond);
        //                    if (ParseState != ParseState.Success) return;
        //                    if (Current == end)
        //                    {
        //                        ParseState = ParseState.CrashEnd;
        //                        return;
        //                    }
        //                    if (*Current == ')')
        //                    {
        //                        value.Int64 = JavascriptLocalMinTimeTicks + millisecond * Date.MillisecondTicks;
        //                        value.Type = NodeType.DateTimeTick;
        //                        ++Current;
        //                        return;
        //                    }
        //                    break;
        //                }
        //            }
        //            goto NUMBER;
        //        default:
        //            NUMBER:
        //            char* numberEnd = null;
        //            switch (searchNumber(ref numberEnd))
        //            {
        //                case NumberType.Number:
        //                    if (json == null) value.SubString = new string(Current, 0, (int)(numberEnd - Current));
        //                    else value.SubString.Set(this.json, (int)(Current - jsonFixed), (int)(numberEnd - Current));
        //                    Current = numberEnd;
        //                    value.SetNumberString(Quote);
        //                    return;
        //                case NumberType.String:
        //                    if (json == null) value.SubString = new string(Current, 0, (int)(numberEnd - Current));
        //                    else value.SubString.Set(this.json, (int)(Current - jsonFixed), (int)(numberEnd - Current));
        //                    Current = numberEnd + 1;
        //                    value.SetNumberString(Quote);
        //                    return;
        //                case NumberType.NaN: value.Type = NodeType.NaN; return;
        //                case NumberType.PositiveInfinity: value.Type = NodeType.PositiveInfinity; return;
        //                case NumberType.NegativeInfinity: value.Type = NodeType.NegativeInfinity; return;
        //            }
        //            break;
        //    }
        //    ParseState = ParseState.UnknownValue;
        //}

        /// <summary>
        /// 基本类型解析函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> parseMethods;
        /// <summary>
        /// 获取基本类型解析函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>解析函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetParseMethod(Type type)
        {
            MethodInfo method;
            return parseMethods.TryGetValue(type, out method) ? method : null;
        }
    }
}
