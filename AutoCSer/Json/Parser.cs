using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class Parser : AutoCSer.Threading.Link<Parser>
    {
        /// <summary>
        /// JSON 转换时间差
        /// </summary>
        internal static readonly DateTime JavascriptLocalMinTime;
        /// <summary>
        ///  Json转换时间差
        /// </summary>
        internal static readonly long JavascriptLocalMinTimeTicks;
        /// <summary>
        /// 默认解析所有成员
        /// </summary>
        internal static readonly ParseAttribute AllMemberAttribute = ConfigLoader.GetUnion(typeof(ParseAttribute)).ParseAttribute ?? new ParseAttribute { Filter = Metadata.MemberFilters.Instance, IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly ParseConfig DefaultConfig = ConfigLoader.GetUnion(typeof(ParseConfig)).ParseConfig ?? new ParseConfig();
        /// <summary>
        /// 字符状态位查询表格
        /// </summary>
        private readonly byte* bits = AutoCSer.Json.Parser.ParseBits.Byte;
        /// <summary>
        /// 转义字符集合
        /// </summary>
        private readonly char* escapeChars = escapeCharData.Char;
        /// <summary>
        /// 配置参数
        /// </summary>
        internal ParseConfig Config;
        /// <summary>
        /// 成员位图
        /// </summary>
        public AutoCSer.Metadata.MemberMap MemberMap { internal get; set; }
        /// <summary>
        /// JSON 字符串
        /// </summary>
        private string json;
        ///// <summary>
        ///// 二进制缓冲区
        ///// </summary>
        //internal byte[] Buffer;
        ///// <summary>
        ///// 二进制缓冲区
        ///// </summary>
        //internal byte[] Buffer { get; private set; }
        /// <summary>
        /// 匿名类型数据
        /// </summary>
        private LeftArray<KeyValue<Type, object>> anonymousTypes;
        /// <summary>
        /// Json字符串起始位置
        /// </summary>
        private char* jsonFixed;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        internal char* Current;
        /// <summary>
        /// 自定义序列化获取当前读取数据位置
        /// </summary>
        public char* CustomRead
        {
            get { return Current; }
        }
        /// <summary>
        /// 解析结束位置
        /// </summary>
        private char* end;
        /// <summary>
        /// 最后一个字符
        /// </summary>
        private char endChar = '}';
        /// <summary>
        /// 当前字符串引号
        /// </summary>
        internal char Quote;
        /// <summary>
        /// 解析状态
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal ParseState ParseState;
        /// <summary>
        /// 解析状态
        /// </summary>
        public ParseState State { get { return ParseState; } }
        /// <summary>
        /// 是否以空格字符结束
        /// </summary>
        private bool isEndSpace;
        /// <summary>
        /// 是否以10进制数字字符结束
        /// </summary>
        private bool isEndDigital;
        /// <summary>
        /// 是否以16进制数字字符结束
        /// </summary>
        private bool isEndHex;
        /// <summary>
        /// 是否以数字字符结束
        /// </summary>
        private bool isEndNumber;
        /// <summary>
        /// JSON 解析器
        /// </summary>
        internal Parser() { }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
        private ParseResult parse<valueType>(ref SubString json, ref valueType value, ParseConfig config)
        {
            fixed (char* jsonFixed = (this.json = json.String))
            {
                Current = (this.jsonFixed = jsonFixed) + json.Start;
                this.Config = config ?? DefaultConfig;
                end = Current + json.Length;
                parse(ref value);
                if (ParseState == ParseState.Success) return new ParseResult { State = ParseState.Success, MemberMap = MemberMap };
                return new ParseResult { State = ParseState, MemberMap = MemberMap, Json = json, Index = (int)(Current - jsonFixed) - json.Start };
            }
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
        private ParseResult parse<valueType>(string json, ref valueType value, ParseConfig config)
        {
            fixed (char* jsonFixed = (this.json = json))
            {
                Current = this.jsonFixed = jsonFixed;
                this.Config = config ?? DefaultConfig;
                end = jsonFixed + json.Length;
                parse(ref value);
                if (ParseState == ParseState.Success) return new ParseResult { State = ParseState.Success, MemberMap = MemberMap };
                return new ParseResult { State = ParseState, MemberMap = MemberMap, Json = json, Index = (int)(Current - jsonFixed) };
            }
        }
        /// <summary>
        /// Json解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="length">Json长度</param>
        /// <param name="value">目标数据</param>
        /// <returns>解析状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private ParseResult parse<valueType>(char* json, int length, ref valueType value)//, ParseConfig config, byte[] buffer
        {
            Config = DefaultConfig;//config ?? 
            //Buffer = buffer;
            end = (jsonFixed = Current = json) + length;
            parse(ref value);
            if (ParseState == ParseState.Success) return new ParseResult { State = ParseState.Success, MemberMap = MemberMap };
            return new ParseResult { State = ParseState, MemberMap = MemberMap, Json = new string(json, 0, length), Index = (int)(Current - jsonFixed) };
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>解析状态</returns>
        private void parse<valueType>(ref valueType value)
        {
            if (endChar != *(end - 1))
            {
                if (((endChar = *(end - 1)) & 0xff00) == 0)
                {
                    isEndSpace = (bits[(byte)endChar] & AutoCSer.Json.Parser.ParseSpaceBit) == 0;
                    if ((uint)(endChar - '0') < 10) isEndDigital = isEndHex = isEndNumber = true;
                    else
                    {
                        isEndDigital = false;
                        if ((uint)((endChar | 0x20) - 'a') < 6) isEndHex = isEndNumber = true;
                        else
                        {
                            isEndHex = false;
                            isEndNumber = (bits[(byte)endChar] & AutoCSer.Json.Parser.ParseNumberBit) == 0;
                        }
                    }
                }
                else isEndSpace = isEndDigital = isEndHex = isEndNumber = false;
            }
            ParseState = ParseState.Success;
            TypeParser<valueType>.Parse(this, ref value);
            if (ParseState == ParseState.Success)
            {
                if (Current == end || !Config.IsEndSpace) return;
                space();
                if (ParseState == ParseState.Success)
                {
                    if (Current == end) return;
                    ParseState = ParseState.CrashEnd;
                }
            }
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TypeParse<valueType>(ref valueType value)
        {
            TypeParser<valueType>.Parse(this, ref value);
            return ParseState == ParseState.Success;
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType TypeParse<valueType>()
        {
            valueType value = default(valueType);
            TypeParser<valueType>.Parse(this, ref value);
            return ParseState == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// 自定义序列化重置当前读取数据位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool VerifyRead(int size)
        {
            if ((Current += size) <= end) return true;
            ParseState = ParseState.Custom;
            return false;
        }
        /// <summary>
        /// 移动当前读取数据位置，负数表示自定义序列化失败
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool MoveRead(int size)
        {
            if (size >= 0)
            {
                if ((Current += size) <= end) return true;
            }
            if (ParseState == ParseState.Success) ParseState = ParseState.Custom;
            return false;
        }
        /// <summary>
        /// 释放 JSON 解析器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            json = null;
            Config = null;
            //Buffer = null;
            MemberMap = null;
            anonymousTypes.SetNull();
            YieldPool.Default.PushNotNull(this);
        }
        /// <summary>
        /// 扫描空格字符
        /// </summary>
        private void space()
        {
        SPACE:
            if (isEndSpace)
            {
                do
                {
                    if (Current == end) return;
                    if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseSpaceBit) | *(((byte*)Current) + 1)) != 0)
                    {
                        if (*Current == '/') break;
                        return;
                    }
                    ++Current;
                }
                while (true);
            }
            else
            {
                while (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseSpaceBit) | *(((byte*)Current) + 1)) == 0) ++Current;
                if (*Current != '/' || Current == end) return;
            }
            if (++Current == end)
            {
                ParseState = ParseState.UnknownNote;
                return;
            }
            if (*Current == '/')
            {
                if (endChar == '\n')
                {
                    while (*++Current != '\n') ;
                    ++Current;
                }
                else
                {
                    do
                    {
                        if (++Current == end) return;
                    }
                    while (*Current != '\n');
                }
                goto SPACE;
            }
            if (*Current == '*')
            {
                if (++Current == end)
                {
                    ParseState = ParseState.NoteNotRound;
                    return;
                }
                if (endChar == '/')
                {
                    do
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.NoteNotRound;
                            return;
                        }
                        while (*Current != '/') ++Current;
                        if (*(Current - 1) == '*')
                        {
                            ++Current;
                            goto SPACE;
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.NoteNotRound;
                            return;
                        }
                    }
                    while (true);
                }
                do
                {
                    while (*Current != '*')
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.NoteNotRound;
                            return;
                        }
                    }
                    if (++Current == end)
                    {
                        ParseState = ParseState.NoteNotRound;
                        return;
                    }
                    if (*Current == '/')
                    {
                        if (++Current == end) return;
                        goto SPACE;
                    }
                }
                while (true);
            }
            ParseState = ParseState.UnknownNote;
        }
        /// <summary>
        /// 是否null
        /// </summary>
        /// <returns>是否null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isNull()
        {
            if (*Current == 'n')
            {
                if (*(long*)Current == ('n' | ('u' << 16) | ((long)'l' << 32) | ((long)'l' << 48)) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                {
                    Current += 4;
                    return true;
                }
                ParseState = ParseState.NotNull;
            }
            return false;
        }
        /// <summary>
        /// 是否非数字 NaN / Infinity
        /// </summary>
        /// <returns>是否非数字NaN</returns>
        private NumberType isNaNPositiveInfinity()
        {
            if (*Current == 'N')
            {
                if (*(int*)(Current + 1) == ('a' + ('N' << 16)) && (int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                {
                    Current += 3;
                    return NumberType.NaN;
                }
            }
            else if (*Current == 'I')
            {
                if (((*(long*)Current ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(Current + 4) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0
                    && (int)((byte*)end - (byte*)Current) >= 8 * sizeof(char))
                {
                    Current += 8;
                    return NumberType.PositiveInfinity;
                }
            }
            ParseState = ParseState.NotNumber;
            return NumberType.Error;
        }
        /// <summary>
        /// 是否 -Infinity
        /// </summary>
        /// <returns>是否 -Infinity</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private NumberType isNegativeInfinity()
        {
            if (((*(long*)(Current + 1) ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(Current + 5) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0
                && (int)((byte*)end - (byte*)Current) >= 9 * sizeof(char))
            {
                Current += 9;
                return NumberType.NegativeInfinity;
            }
            ParseState = ParseState.NotNumber;
            return NumberType.Error;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        private void parseHex32(ref uint value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    ParseState = ParseState.NotHex;
                    return;
                }
                number += 10;
            }
            value = number;
            if (++Current == end) return;
            if (isEndHex)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return;
                        number += 10;
                    }
                    value <<= 4;
                    value += number;
                }
                while (++Current != end);
                return;
            }
            do
            {
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return;
                    number += 10;
                }
                value <<= 4;
                ++Current;
                value += number;
            }
            while (true);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private uint parseUInt32(uint value)
        {
            uint number;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9) return value;
                    value *= 10;
                    value += number;
                    if (++Current == end) return value;
                }
                while (true);
            }
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value *= 10;
                ++Current;
                value += (byte)number;
            }
            return value;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private ulong parseUInt64(uint value)
        {
            char* end32 = Current + 8;
            if (end32 > end) end32 = end;
            uint number;
            do
            {
                if ((number = (uint)(*Current - '0')) > 9) return value;
                value *= 10;
                value += number;
            }
            while (++Current != end32);
            if (Current == end) return value;
            ulong value64 = value;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9) return value64;
                    value64 *= 10;
                    value64 += number;
                    if (++Current == end) return value64;
                }
                while (true);
            }
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value64 *= 10;
                ++Current;
                value64 += (byte)number;
            }
            return value64;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <returns>数字</returns>
        private ulong parseHex64()
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    ParseState = ParseState.NotHex;
                    return 0;
                }
                number += 10;
            }
            if (++Current == end) return number;
            uint high = number;
            char* end32 = Current + 7;
            if (end32 > end) end32 = end;
            do
            {
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return high;
                    number += 10;
                }
                high <<= 4;
                high += number;
            }
            while (++Current != end32);
            if (Current == end) return high;
            char* start = Current;
            ulong low = number;
            if (isEndHex)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                        {
                            return low | (ulong)high << ((int)((byte*)Current - (byte*)start) << 1);
                        }
                        number += 10;
                    }
                    low <<= 4;
                    low += number;
                }
                while (++Current != end);
                return low | (ulong)high << ((int)((byte*)Current - (byte*)start) << 1);
            }
            do
            {
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                    {
                        return low | (ulong)high << ((int)((byte*)Current - (byte*)start) << 1);
                    }
                    number += 10;
                }
                low <<= 4;
                ++Current;
                low += number;
            }
            while (true);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private uint parseHex2()
        {
            uint code = (uint)(*++Current - '0'), number = (uint)(*++Current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            return (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        private uint parseHex4()
        {
            uint code = (uint)(*++Current - '0'), number = (uint)(*++Current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code <<= 12;
            code += (number << 8);
            if ((number = (uint)(*++Current - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code += (number << 4);
            number = (uint)(*++Current - '0');
            return code + (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
        }
        /// <summary>
        /// 查找数字结束位置
        /// </summary>
        /// <param name="numberEnd">数字结束位置</param>
        /// <returns>数字类型</returns>
        private NumberType searchNumber(ref char* numberEnd)
        {
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)Current) + 1)) != 0)
            {
                space();
                if (ParseState != ParseState.Success) return NumberType.Error;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return NumberType.Error;
                }
                if (*Current == '"' || *Current == '\'')
                {
                    Quote = *Current;
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return NumberType.Error;
                    }
                    numberEnd = Current;
                    if (endChar == Quote)
                    {
                        while (*numberEnd != Quote) ++numberEnd;
                    }
                    else
                    {
                        while (*numberEnd != Quote)
                        {
                            if (++numberEnd == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return NumberType.Error;
                            }
                        }
                    }
                    return NumberType.String;
                }
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)Current) + 1)) != 0) return isNaNPositiveInfinity();
            }
            numberEnd = Current;
            if (isEndNumber)
            {
                while (++numberEnd != end && ((bits[*(byte*)numberEnd] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            else
            {
                while (((bits[*(byte*)++numberEnd] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            return (int)(numberEnd - Current) != 1 || *Current != '-' ? NumberType.Number : isNegativeInfinity();
        }
        /// <summary>
        /// 查找数字结束位置
        /// </summary>
        /// <param name="numberEnd">数字结束位置</param>
        /// <returns>数字类型</returns>
        private NumberType searchNumberNull(ref char* numberEnd)
        {
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)Current) + 1)) != 0)
            {
                if (isNull()) return NumberType.Null;
                space();
                if (ParseState != ParseState.Success) return NumberType.Error;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return NumberType.Error;
                }
                if (*Current == '"' || *Current == '\'')
                {
                    Quote = *Current;
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return NumberType.Error;
                    }
                    numberEnd = Current;
                    if (endChar == Quote)
                    {
                        while (*numberEnd != Quote) ++numberEnd;
                    }
                    else
                    {
                        while (*numberEnd != Quote)
                        {
                            if (++numberEnd == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return NumberType.Error;
                            }
                        }
                    }
                    return NumberType.String;
                }
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)Current) + 1)) != 0) return isNull() ? NumberType.Null : isNaNPositiveInfinity();
            }
            numberEnd = Current;
            if (isEndNumber)
            {
                while (++numberEnd != end && ((bits[*(byte*)numberEnd] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            else
            {
                while (((bits[*(byte*)++numberEnd] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            return (int)(numberEnd - Current) != 1 || *Current != '-' ? NumberType.Number : isNegativeInfinity();
        }
        /// <summary>
        /// 获取数字字符串
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private string getNumberString(char* end)
        {
            int length = (int)(end - Current);
            return json == null || json.Length != length ? new string(Current, 0, length) : json;
        }
        /// <summary>
        /// 时间解析 /Date(xxx)/
        /// </summary>
        /// <param name="timeString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private unsafe static bool parseTime(string timeString, out DateTime value)
        {
            if (timeString.Length > 8)
            {
                fixed (char* timeFixed = timeString)
                {
                    if (*(long*)(timeFixed + 1) == 'D' + ('a' << 16) + ((long)'t' << 32) + ((long)'e' << 48))
                    {
                        char* end = timeFixed + (timeString.Length - 2);
                        if (((*(timeFixed + 5) ^ '(') | (*(int*)end ^ (')' + ('/' << 16)))) == 0)
                        {
                            char* start = timeFixed + 6;
                            bool isSign;
                            if (*start == '-')
                            {
                                if (timeString.Length == 9)
                                {
                                    value = DateTime.MinValue;
                                    return false;
                                }
                                isSign = true;
                                ++start;
                            }
                            else isSign = false;
                            uint code = (uint)(*start - '0');
                            if (code < 10)
                            {
                                long millisecond = code;
                                while (++start != end)
                                {
                                    if ((code = (uint)(*start - '0')) >= 10)
                                    {
                                        value = DateTime.MinValue;
                                        return false;
                                    }
                                    millisecond *= 10;
                                    millisecond += code;
                                }
                                value = JavascriptLocalMinTime.AddTicks((isSign ? -millisecond : millisecond) * TimeSpan.TicksPerMillisecond);
                                return true;
                            }
                        }
                    }
                }
            }
            value = DateTime.MinValue;
            return false;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        private void parse(ref GuidCreator value)
        {
            if ((int)((byte*)end - (byte*)Current) < 38 * sizeof(char))
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            Quote = *Current;
            value.Byte3 = (byte)parseHex2();
            value.Byte2 = (byte)parseHex2();
            value.Byte1 = (byte)parseHex2();
            value.Byte0 = (byte)parseHex2();
            if (*++Current != '-')
            {
                ParseState = ParseState.NotGuid;
                return;
            }
            value.Byte45 = (ushort)parseHex4();
            if (*++Current != '-')
            {
                ParseState = ParseState.NotGuid;
                return;
            }
            value.Byte67 = (ushort)parseHex4();
            if (*++Current != '-')
            {
                ParseState = ParseState.NotGuid;
                return;
            }
            value.Byte8 = (byte)parseHex2();
            value.Byte9 = (byte)parseHex2();
            if (*++Current != '-')
            {
                ParseState = ParseState.NotGuid;
                return;
            }
            value.Byte10 = (byte)parseHex2();
            value.Byte11 = (byte)parseHex2();
            value.Byte12 = (byte)parseHex2();
            value.Byte13 = (byte)parseHex2();
            value.Byte14 = (byte)parseHex2();
            value.Byte15 = (byte)parseHex2();
            if (*++Current == Quote)
            {
                ++Current;
                return;
            }
            ParseState = ParseState.NotGuid;
        }
        /// <summary>
        /// 查找字符串中的转义符
        /// </summary>
        private byte searchEscape()
        {
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote) return 0;
                        if (*Current == '\\') return 1;
                        if (*Current == '\n')
                        {
                            ParseState = ParseState.StringEnter;
                            return 0;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote) return 0;
                    if (*Current == '\\') return 1;
                    if (*Current == '\n')
                    {
                        ParseState = ParseState.StringEnter;
                        return 0;
                    }
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return 0;
                }
            }
            while (true);
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <returns>写入结束位置</returns>
        private char* parseEscape()
        {
            char* write = Current;
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return null;
                }
                *write++ = (char)parseHex4();
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return null;
                }
                *write++ = (char)parseHex2();
            }
            else
            {
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return null;
                }
                *write++ = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
            }
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return null;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return write;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            ParseState = ParseState.StringEnter;
                            return null;
                        }
                    }
                    *write++ = *Current++;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return write;
                    }
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        ParseState = ParseState.StringEnter;
                        return null;
                    }
                }
                *write++ = *Current++;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return null;
                }
            }
            while (true);
        }
        /// <summary>
        /// 获取转义后的字符串长度
        /// </summary>
        /// <returns>字符串长度</returns>
        private int parseEscapeSize()
        {
            char* start = Current;
            int length = 0;
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return 0;
                }
                length += 5;
                Current += 5;
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return 0;
                }
                length += 3;
                Current += 3;
            }
            else
            {
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return 0;
                }
                ++length;
                ++Current;
            }
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return 0;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            length = (int)(Current - start) - length;
                            Current = start;
                            return length;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            ParseState = ParseState.StringEnter;
                            return 0;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        length = (int)(Current - start) - length;
                        Current = start;
                        return length;
                    }
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        ParseState = ParseState.StringEnter;
                        return 0;
                    }
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return 0;
                }
            }
            while (true);
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="write">当前写入位置</param>
        private void parseEscapeUnsafe(char* write)
        {
        NEXT:
            if (*++Current == 'u') *write++ = (char)parseHex4();
            else if (*Current == 'x') *write++ = (char)parseHex2();
            else *write++ = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
            do
            {
                if (*++Current == Quote)
                {
                    ++Current;
                    return;
                }
                if (*Current == '\\') goto NEXT;
                *write++ = *Current;
            }
            while (true);
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="start"></param>
        /// <returns>字符串,失败返回null</returns>
        private string parseEscape(char* start)
        {
            int size = parseEscapeSize();
            if (size != 0)
            {
                int left = (int)(Current - start);
                string value = AutoCSer.Extension.StringExtension.FastAllocateString(left + size);
                fixed (char* valueFixed = value)
                {
                    AutoCSer.Memory.CopyNotNull((void*)start, valueFixed, left << 1);
                    parseEscapeUnsafe(valueFixed + left);
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <returns>字符串,失败返回null</returns>
        private string parseString()
        {
            Quote = *Current;
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return null;
            }
            char* start = Current;
            if (searchEscape() == 0) return ParseState == ParseState.Success ? new string(start, 0, (int)(Current++ - start)) : null;
            if (Config.IsTempString)
            {
                char* writeEnd = parseEscape();
                return writeEnd != null ? new string(start, 0, (int)(writeEnd - start)) : null;
            }
            return parseEscape(start);
        }
        /// <summary>
        /// 查找转义字符串结束位置
        /// </summary>
        private void searchEscapeEnd()
        {
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                Current += 5;
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                Current += 3;
            }
            else
            {
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                ++Current;
            }
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote) return;
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            ParseState = ParseState.StringEnter;
                            return;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote) return;
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        ParseState = ParseState.StringEnter;
                        return;
                    }
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 解析字符串节点
        /// </summary>
        /// <param name="value"></param>
        private void parseStringNode(ref Node value)
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
                    int length = (int)(Current++ - start);
                    if (length == 0) value.SubString.Set(string.Empty, 0, 0);
                    else if (this.json == null) value.SubString = new string(start, 0, length);
                    else value.SubString.Set(this.json, (int)(start - jsonFixed), length);
                    value.Type = NodeType.String;
                }
                return;
            }
            if (this.json != null)
            {
                char* escapeStart = Current;
                searchEscapeEnd();
                if (ParseState == ParseState.Success)
                {
                    value.SubString.Set(this.json, (int)(start - jsonFixed), (int)(Current - start));
                    value.SetQuoteString((int)(escapeStart - start), Quote, Config.IsTempString);
                    ++Current;
                }
            }
            else
            {
                string newValue = parseEscape(start);
                if (newValue != null)
                {
                    value.SubString.Set(newValue, 0, newValue.Length);
                    value.Type = NodeType.String;
                }
            }
        }
        ///// <summary>
        ///// 解析列表节点
        ///// </summary>
        ///// <param name="value"></param>
        //private void parseListNode(ref LeftArray<Node> value)
        //{
        //    if (++Current == end)
        //    {
        //        ParseState = ParseState.CrashEnd;
        //        return;
        //    }
        //    if (IsFirstArrayValue())
        //    {
        //        do
        //        {
        //            Node node = default(Node);
        //            Parse(ref node);
        //            if (ParseState != ParseState.Success) return;
        //            value.Add(node);
        //        }
        //        while (IsNextArrayValue());
        //    }
        //}
        ///// <summary>
        ///// 解析字典节点
        ///// </summary>
        ///// <param name="value"></param>
        //private void parseDictionaryNode(ref LeftArray<KeyValue<Node, Node>> value)
        //{
        //    if (++Current == end)
        //    {
        //        ParseState = ParseState.CrashEnd;
        //        return;
        //    }
        //    if (IsFirstObject())
        //    {
        //        do
        //        {
        //            Node name = default(Node);
        //            if (*Current == '"' || *Current == '\'') parseStringNode(ref name);
        //            else
        //            {
        //                char* nameStart = Current;
        //                SearchNameEnd();
        //                if (this.json == null)
        //                {
        //                    int length = (int)(Current - nameStart);
        //                    if (length == 0) name.SubString.Set(string.Empty, 0, 0);
        //                    else name.SubString = new string(nameStart, 0, length);
        //                }
        //                else name.SubString.Set(this.json, (int)(nameStart - jsonFixed), (int)(Current - nameStart));
        //                name.Type = NodeType.String;
        //            }
        //            if (ParseState != ParseState.Success || SearchColon() == 0) return;
        //            Node node = default(Node);
        //            Parse(ref node);
        //            if (ParseState != ParseState.Success) return;
        //            value.Add(new KeyValue<Node, Node>(ref name, ref node));
        //        }
        //        while (IsNextObject());
        //    }
        //}
        /// <summary>
        /// 忽略对象
        /// </summary>
        public void Ignore()
        {
            LeftArray<NodeType> TypeArray = default(LeftArray<NodeType>);
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
                        ignoreString();
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
                        if (IsFirstObject())
                        {
                            TypeArray.Add(NodeType.Dictionary);
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
                        if (IsFirstArrayValue())
                        {
                            TypeArray.Add(NodeType.Array);
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
                        goto CHECKNODE;
                    }
                    goto NUMBER;
                case 'f' & 7:
                    if (*Current == 'f')
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char) && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
                        {
                            Current += 5;
                            goto CHECKNODE;
                        }
                        break;
                    }
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
                    {
                        if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                        {
                            Current += 4;
                            goto CHECKNODE;
                        }
                        if ((int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                        {
                            Current += 9;
                            ignoreNumber();
                            if (ParseState != ParseState.Success) return;
                            if (Current == end)
                            {
                                ParseState = ParseState.CrashEnd;
                                return;
                            }
                            if (*Current == ')')
                            {
                                ++Current;
                                goto CHECKNODE;
                            }
                            break;
                        }
                    }
                    goto NUMBER;
                default:
                NUMBER:
                    ignoreNumber();
                    if (ParseState == ParseState.Success) goto CHECKNODE;
                    return;
            }
            ParseState = ParseState.UnknownValue;
            return;

            DICTIONARYNAME:
            if (*Current == '\'' || *Current == '"') ignoreString();
            else ignoreName();
            if (ParseState != ParseState.Success || SearchColon() == 0) return;
            goto NEXTNODE;

            CHECKNODE:
            if (TypeArray.Length != 0)
            {
                switch (TypeArray.Array[TypeArray.Length - 1])
                {
                    case NodeType.Dictionary:
                        if (IsNextObject()) goto DICTIONARYNAME;
                        --TypeArray.Length;
                        goto CHECKNODE;
                    case NodeType.Array:
                        if (IsNextArrayValue()) goto NEXTNODE;
                        --TypeArray.Length;
                        goto CHECKNODE;
                }
            }
        }
        //public void Ignore()
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
        //                ignoreString();
        //                return;
        //            }
        //            goto NUMBER;
        //        case '{' & 7:
        //            if (*Current == '{')
        //            {
        //                ignoreObject();
        //                return;
        //            }
        //            if (*Current == '[')
        //            {
        //                ignoreArray();
        //                return;
        //            }
        //            goto NUMBER;
        //        case 't' & 7:
        //            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
        //            {
        //                Current += 4;
        //                return;
        //            }
        //            goto NUMBER;
        //        case 'f' & 7:
        //            if (*Current == 'f')
        //            {
        //                if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char) && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
        //                {
        //                    Current += 5;
        //                    return;
        //                }
        //                break;
        //            }
        //            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
        //            {
        //                if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
        //                {
        //                    Current += 4;
        //                    return;
        //                }
        //                if ((int)((byte*)end - (byte*)Current) > 9 * sizeof(char) && ((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
        //                {
        //                    Current += 9;
        //                    ignoreNumber();
        //                    if (ParseState != ParseState.Success) return;
        //                    if (Current == end)
        //                    {
        //                        ParseState = ParseState.CrashEnd;
        //                        return;
        //                    }
        //                    if (*Current == ')')
        //                    {
        //                        ++Current;
        //                        return;
        //                    }
        //                    break;
        //                }
        //            }
        //            goto NUMBER;
        //        default:
        //            NUMBER:
        //            ignoreNumber();
        //            return;
        //    }
        //    ParseState = ParseState.UnknownValue;
        //}
        /// <summary>
        /// 忽略字符串
        /// </summary>
        private void ignoreString()
        {
            Quote = *Current;
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            //char* start = Current;
            if (searchEscape() == 0)
            {
                if (ParseState == ParseState.Success) ++Current;
                return;
            }
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                Current += 5;
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                Current += 3;
            }
            else
            {
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
                ++Current;
            }
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            ParseState = ParseState.StringEnter;
                            return;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return;
                    }
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        ParseState = ParseState.StringEnter;
                        return;
                    }
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 忽略数字
        /// </summary>
        private void ignoreNumber()
        {
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)Current) + 1)) == 0)
            {
                while (++Current != end && ((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNumberBit) | *(((byte*)Current) + 1)) == 0) ;
                return;
            }
            ParseState = ParseState.NotNumber;
        }
        ///// <summary>
        ///// 忽略数组
        ///// </summary>
        //private void ignoreArray()
        //{
        //    if (++Current == end)
        //    {
        //        ParseState = ParseState.CrashEnd;
        //        return;
        //    }
        //    if (IsFirstArrayValue())
        //    {
        //        do
        //        {
        //            Ignore();
        //            if (ParseState != ParseState.Success) return;
        //        }
        //        while (IsNextArrayValue());
        //    }
        //}
        /// <summary>
        /// 是否存在下一个数组数据
        /// </summary>
        /// <returns>是否存在下一个数组数据</returns>
        internal bool IsFirstArrayValue()
        {
            if (*Current == ']')
            {
                ++Current;
                return false;
            }
            space();
            if (ParseState != ParseState.Success) return false;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return false;
            }
            if (*Current == ']')
            {
                ++Current;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 是否存在下一个数组数据
        /// </summary>
        /// <returns>是否存在下一个数组数据</returns>
        internal bool IsNextArrayValue()
        {
            if (*Current == ',')
            {
                ++Current;
                return true;
            }
            if (*Current == ']')
            {
                ++Current;
                return false;
            }
            space();
            if (ParseState != ParseState.Success) return false;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return false;
            }
            if (*Current == ',')
            {
                ++Current;
                return true;
            }
            if (*Current == ']')
            {
                ++Current;
                return false;
            }
            ParseState = ParseState.NotArrayValue;
            return false;
        }
        /// <summary>
        /// 忽略对象
        /// </summary>
        private void ignoreObject()
        {
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return;
            }
            if (IsFirstObject())
            {
                if (*Current == '\'' || *Current == '"') ignoreString();
                else ignoreName();
                if (ParseState != ParseState.Success || SearchColon() == 0) return;
                Ignore();
                while (ParseState == ParseState.Success && IsNextObject())
                {
                    if (*Current == '\'' || *Current == '"') ignoreString();
                    else ignoreName();
                    if (ParseState != ParseState.Success || SearchColon() == 0) return;
                    Ignore();
                }
            }
        }
        /// <summary>
        /// 判断是否存在第一个成员
        /// </summary>
        /// <returns>是否存在第一个成员</returns>
        internal bool IsFirstObject()
        {
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            space();
            if (ParseState != ParseState.Success) return false;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return false;
            }
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            ParseState = ParseState.NotFoundName;
            return false;
        }
        /// <summary>
        /// 判断是否存在下一个成员
        /// </summary>
        /// <returns>是否存在下一个成员</returns>
        internal bool IsNextObject()
        {
            byte isSpace = 0;
        START:
            if (*Current == ',')
            {
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return false;
                }
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                space();
                if (ParseState != ParseState.Success) return false;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return false;
                }
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                ParseState = ParseState.NotFoundName;
                return false;
            }
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return false;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return false;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotObject;
            return false;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal byte* IsName(byte* names, ref int index)
        {
            int length = *(short*)names;
            if (length == 0)
            {
                if (*Current == '}')
                {
                    index = -1;
                    ++Current;
                    return names;
                }
            }
            else if (AutoCSer.Memory.SimpleEqualNotNull((byte*)Current, names += sizeof(short), length) && (int)((byte*)end - (byte*)Current) >= length)
            {
                Current = (char*)((byte*)Current + length);
                return names + length;
            }
            return null;
        }
        /// <summary>
        /// 忽略成员名称
        /// </summary>
        private void ignoreName()
        {
            do
            {
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return;
                }
            }
            while (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameBit) | *(((byte*)Current) + 1)) == 0);
        }
        /// <summary>
        /// 查找名称直到结束
        /// </summary>
        internal void SearchNameEnd()
        {
            if (ParseState == ParseState.Success)
            {
                while (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (++Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                }
                //while (++Current != end && ((bits[*(byte*)Current] & nameBit) | *(((byte*)Current) + 1)) == 0) ;
                //if (Current == end)
                //{
                //    State = ParseState.CrashEnd;
                //    return;
                //}
            }
        }
        /// <summary>
        /// 查找冒号
        /// </summary>
        /// <returns>是否找到</returns>
        internal byte SearchColon()
        {
            if (*Current == ':')
            {
                ++Current;
                return 1;
            }
            space();
            if (ParseState != ParseState.Success) return 0;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return 0;
            }
            if (*Current == ':')
            {
                ++Current;
                return 1;
            }
            ParseState = ParseState.NotFoundColon;
            return 0;
        }
        /// <summary>
        /// 查找对象起始位置
        /// </summary>
        /// <returns>是否查找到</returns>
        internal bool SearchObject()
        {
            byte isSpace = 0;
        START:
            if (*Current == '{')
            {
                ++Current;
                return true;
            }
            if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                Current += 4;
                return false;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return false;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return false;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotObject;
            return false;
        }
        /// <summary>
        /// 获取成员名称第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示失败</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal char GetFirstName()
        {
            if (*Current == '\'' || *Current == '"')
            {
                Quote = *Current;
                return NextStringChar();
            }
            Quote = (char)0;
            return *Current;
        }
        /// <summary>
        /// 获取成员名称下一个字符
        /// </summary>
        /// <returns>第一个字符,0表示失败</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal char GetNextName()
        {
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return (char)0;
            }
            return ((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseNameBit) | *(((byte*)Current) + 1)) == 0 ? *Current : (char)0;
        }
        /// <summary>
        /// 读取下一个字符
        /// </summary>
        /// <returns>字符,结束或者错误返回0</returns>
        internal char NextStringChar()
        {
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return (char)0;
            }
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
            {
                if (*Current == Quote)
                {
                    ++Current;
                    return Quote = (char)0;
                }
                if (*Current == '\\')
                {
                    if (*++Current == 'u')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                        {
                            ParseState = ParseState.CrashEnd;
                            return (char)0;
                        }
                        return (char)parseHex4();
                    }
                    if (*Current == 'x')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                        {
                            ParseState = ParseState.CrashEnd;
                            return (char)0;
                        }
                        return (char)parseHex2();
                    }
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return (char)0;
                    }
                    return *Current < escapeCharSize ? escapeChars[*Current] : *Current;
                }
                if (*Current == '\n')
                {
                    ParseState = ParseState.StringEnter;
                    return (char)0;
                }
            }
            return *Current;
        }
        /// <summary>
        /// 查找字符串直到结束
        /// </summary>
        internal void SearchStringEnd()
        {
            if (Quote != 0 && ParseState == ParseState.Success)
            {
                //++Current;
                do
                {
                    if (Current == end)
                    {
                        ParseState = ParseState.CrashEnd;
                        return;
                    }
                    if (endChar == Quote)
                    {
                        do
                        {
                            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                            {
                                if (*Current == Quote)
                                {
                                    ++Current;
                                    return;
                                }
                                if (*Current == '\\') goto NEXT;
                                if (*Current == '\n')
                                {
                                    ParseState = ParseState.StringEnter;
                                    return;
                                }
                            }
                            ++Current;
                        }
                        while (true);
                    }
                    do
                    {
                        if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote)
                            {
                                ++Current;
                                return;
                            }
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                ParseState = ParseState.StringEnter;
                                return;
                            }
                        }
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                    }
                    while (true);
                NEXT:
                    if (*++Current == 'u')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        Current += 5;
                    }
                    else if (*Current == 'x')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        Current += 3;
                    }
                    else
                    {
                        if (Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return;
                        }
                        ++Current;
                    }
                }
                while (true);
            }
        }
        /// <summary>
        /// 设置匿名类型数据
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        internal void SetAnonymousType<valueType>(valueType value)
        {
            foreach (KeyValue<Type, object> type in anonymousTypes)
            {
                if (type.Key == typeof(valueType)) return;
            }
            anonymousTypes.Add(new KeyValue<Type, object>(typeof(valueType), MemberCopy.Copyer<valueType>.MemberwiseClone(value)));
        }
        /// <summary>
        /// 找不到构造函数
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="isAnonymousType"></param>
        internal void CheckNoConstructor<valueType>(ref valueType value, bool isAnonymousType)
        {
            Func<Type, object> constructor = Config.Constructor;
            if (constructor == null)
            {
                if (isAnonymousType)
                {
                    foreach (KeyValue<Type, object> type in anonymousTypes)
                    {
                        if (type.Key == typeof(valueType))
                        {
                            value = MemberCopy.Copyer<valueType>.MemberwiseClone((valueType)type.Value);
                            return;
                        }
                    }
                }
                --Current;
                ignoreObject();
                return;
            }
            object newValue = constructor(typeof(valueType));
            if (newValue == null)
            {
                --Current;
                ignoreObject();
                return;
            }
            value = (valueType)newValue;
        }
        /// <summary>
        /// 查找数组起始位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">目标数组</param>
        internal void SearchArray<valueType>(ref valueType[] value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '[')
            {
                ++Current;
                if (value == null) value = NullValue<valueType>.Array;
                return;
            }
            if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value = null;
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return;
                if (Current != end)
                {
                    isSpace = 1;
                    goto START;
                }
            }
            ParseState = ParseState.CrashEnd;
        }
        /// <summary>
        /// 查找字符串引号并返回第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示null</returns>
        internal char SearchQuote()
        {
            if (*Current == '\'' || *Current == '"')
            {
                Quote = *Current;
                return NextStringChar();
            }
            if (isNull()) return Quote = (char)0;
            space();
            if (ParseState != ParseState.Success) return (char)0;
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return (char)0;
            }
            if (*Current == '\'' || *Current == '"')
            {
                Quote = *Current;
                return NextStringChar();
            }
            if (isNull()) return Quote = (char)0;
            ParseState = ParseState.NotString;
            return (char)0;
        }
        /// <summary>
        /// 查找枚举引号并返回第一个字符
        /// </summary>
        /// <returns>第一个字符,0表示null</returns>
        internal char SearchEnumQuote()
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                return NextEnumChar();
            }
            if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                Current += 4;
                return Quote = (char)0;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return (char)0;
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return (char)0;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotEnumChar;
            return (char)0;
        }
        /// <summary>
        /// 获取下一个枚举字符
        /// </summary>
        /// <returns>下一个枚举字符,0表示null</returns>
        internal char NextEnumChar()
        {
            if (++Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return (char)0;
            }
            if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
            {
                if (*Current == Quote)
                {
                    ++Current;
                    return Quote = (char)0;
                }
                if (*Current == '\\' || *Current == '\n')
                {
                    ParseState = ParseState.NotEnumChar;
                    return (char)0;
                }
            }
            return *Current;
        }
        /// <summary>
        /// 查找下一个枚举字符
        /// </summary>
        /// <returns>下一个枚举字符,0表示null</returns>
        internal char SearchNextEnum()
        {
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.Json.Parser.ParseEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return Quote = (char)0;
                    }
                    if (*Current == '\\' || *Current == '\n')
                    {
                        ParseState = ParseState.NotEnumChar;
                        return (char)0;
                    }
                }
                else if (*Current == ',')
                {
                    do
                    {
                        if (++Current == end)
                        {
                            ParseState = ParseState.CrashEnd;
                            return (char)0;
                        }
                    }
                    while (*Current == ' ');
                    if (*Current == Quote)
                    {
                        ++Current;
                        return Quote = (char)0;
                    }
                    if (*Current == '\\' || *Current == '\n')
                    {
                        ParseState = ParseState.NotEnumChar;
                        return (char)0;
                    }
                    return *Current;
                }
                if (++Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return (char)0;
                }
            }
            while (true);
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        internal bool IsEnumNumber()
        {
            if ((uint)(*Current - '0') < 10) return true;
            space();
            if (ParseState != ParseState.Success) return false;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return false;
            }
            return (uint)(*Current - '0') < 10;
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        internal bool IsEnumNumberFlag()
        {
            if ((uint)(*Current - '0') < 10 || *Current == '-') return true;
            space();
            if (ParseState != ParseState.Success) return false;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return false;
            }
            return (uint)(*Current - '0') < 10 || *Current == '-';
        }
        /// <summary>
        /// 查找字典起始位置
        /// </summary>
        /// <returns>是否查找到</returns>
        private byte searchDictionary()
        {
            byte isSpace = 0;
        START:
            if (*Current == '{')
            {
                ++Current;
                return 1;
            }
            if (*Current == '[')
            {
                ++Current;
                return 2;
            }
            if (*(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                Current += 4;
                return 0;
            }
            if (isSpace == 0)
            {
                space();
                if (ParseState != ParseState.Success) return 0;
                if (Current == end)
                {
                    ParseState = ParseState.CrashEnd;
                    return 0;
                }
                isSpace = 1;
                goto START;
            }
            ParseState = ParseState.NotObject;
            return 0;
        }
        /// <summary>
        /// 对象是否结束
        /// </summary>
        /// <returns>对象是否结束</returns>
        private byte isDictionaryObjectEnd()
        {
            if (*Current == '}')
            {
                ++Current;
                return 1;
            }
            space();
            if (ParseState != ParseState.Success) return 1;
            if (Current == end)
            {
                ParseState = ParseState.CrashEnd;
                return 1;
            }
            if (*Current == '}')
            {
                ++Current;
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 是否null
        /// </summary>
        /// <returns>是否null</returns>
        private bool tryNull()
        {
            if (isNull()) return true;
            space();
            return isNull();
        }

        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByte<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumByte.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByte<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumSByte.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShort<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumShort.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShort<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUShort.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumInt<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumInt.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUInt<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUInt.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLong<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumLong.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULong<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumULong.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByteFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumByte.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByteFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumSByte.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShortFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumShort.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShortFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUShort.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumIntFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumInt.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUIntFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUInt.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLongFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumLong.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULongFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumULong.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structParse<valueType>(ref valueType value) where valueType : struct
        {
            TypeParser<valueType>.ParseStruct(this, ref value);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void typeParse<valueType>(ref valueType value)
        {
            TypeParser<valueType>.ParseClass(this, ref value);
        }

        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="values">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void array<valueType>(ref valueType[] values)
        {
            TypeParser<valueType>.Array(this, ref values);
        }
        /// <summary>
        /// 字典解析
        /// </summary>
        /// <param name="dictionary">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionary<valueType, dictionaryValueType>(ref Dictionary<valueType, dictionaryValueType> dictionary)
        {
            byte type = searchDictionary();
            if (type == 0) dictionary = null;
            else
            {
                dictionary = AutoCSer.DictionaryCreator.CreateAny<valueType, dictionaryValueType>();
                if (type == 1)
                {
                    if (isDictionaryObjectEnd() == 0)
                    {
                        valueType key = default(valueType);
                        dictionaryValueType value = default(dictionaryValueType);
                        do
                        {
                            TypeParser<valueType>.Parse(this, ref key);
                            if (ParseState != ParseState.Success || SearchColon() == 0) return;
                            TypeParser<dictionaryValueType>.Parse(this, ref value);
                            if (ParseState != ParseState.Success) return;
                            dictionary.Add(key, value);
                        }
                        while (IsNextObject());
                    }
                }
                else if (IsFirstArrayValue())
                {
                    KeyValue<valueType, dictionaryValueType> value = default(KeyValue<valueType, dictionaryValueType>);
                    do
                    {
                        TypeParser<KeyValue<valueType, dictionaryValueType>>.ParseValue(this, ref value);
                        if (ParseState != ParseState.Success) return;
                        dictionary.Add(value.Key, value.Value);
                    }
                    while (IsNextArrayValue());
                }
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableEnumParse<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            if (tryNull()) value = null;
            else
            {
                valueType newValue = value.HasValue ? value.Value : default(valueType);
                TypeParser<valueType>.DefaultParser(this, ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableParse<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            if (tryNull()) value = null;
            else if (ParseState == ParseState.Success)
            {
                valueType newValue = value.HasValue ? value.Value : default(valueType);
                TypeParser<valueType>.Parse(this, ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void keyValuePairParse<keyType, valueType>(ref KeyValuePair<keyType, valueType> value)
        {
            if (SearchObject())
            {
                KeyValue<keyType, valueType> keyValue = new KeyValue<keyType, valueType>(value.Key, value.Value);
                TypeParser<KeyValue<keyType, valueType>>.ParseMembers(this, ref keyValue);
                value = new KeyValuePair<keyType, valueType>(keyValue.Key, keyValue.Value);
            }
            else value = new KeyValuePair<keyType, valueType>();
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void listConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeParser<argumentType>.ArrayIndex(this, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.ListConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void collectionConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeParser<argumentType>.ArrayIndex(this, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.CollectionConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumerableConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeParser<argumentType>.ArrayIndex(this, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.EnumerableConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void arrayConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            TypeParser<argumentType>.Array(this, ref values);
            if (ParseState == ParseState.Success)
            {
                if (values == null) value = default(valueType);
                else value = Emit.ArrayConstructor<valueType, argumentType>.Constructor(values);
            }
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionaryConstructor<dictionaryType, keyType, valueType>(ref dictionaryType value)
        {
            KeyValuePair<keyType, valueType>[] values = null;
            int count = TypeParser<KeyValuePair<keyType, valueType>>.ArrayIndex(this, ref values);
            if (count == -1) value = default(dictionaryType);
            else
            {
                Dictionary<keyType, valueType> dictionary = AutoCSer.DictionaryCreator.CreateAny<keyType, valueType>(count);
                if (count != 0)
                {
                    foreach (KeyValuePair<keyType, valueType> keyValue in values)
                    {
                        dictionary.Add(keyValue.Key, keyValue.Value);
                        if (--count == 0) break;
                    }
                }
                value = Emit.DictionaryConstructor<dictionaryType, keyType, valueType>.Constructor(dictionary);
            }
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <param name="value">目标数据</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void baseParse<valueType, childType>(ref childType value) where childType : valueType
        {
            if (value == null)
            {
                if (SearchObject())
                {
                    Func<childType> constructor = AutoCSer.Emit.Constructor<childType>.New;
                    if (constructor == null)
                    {
                        CheckNoConstructor(ref value, false);
                        if (value == null) return;
                    }
                    else value = constructor();
                    valueType newValue = value;
                    TypeParser<valueType>.ParseMembers(this, ref newValue);
                }
            }
            else
            {
                valueType newValue = value;
                TypeParser<valueType>.ParseClass(this, ref newValue);
            }
        }

        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <param name="isTempString"></param>
        /// <returns>解析是否成功</returns>
        private bool parseQuoteString(ref SubString value, int escapeIndex, char quote, int isTempString)
        {
            fixed (char* jsonFixed = value.String)
            {
                char* start = jsonFixed + value.Start;
                end = start + value.Length;
                Quote = quote;
                Current = start + escapeIndex;
                endChar = *end++;
                if (isTempString == 0)
                {
                    string newValue = parseEscape(start);
                    if (newValue != null)
                    {
                        value.Set(newValue, 0, newValue.Length);
                        return true;
                    }
                }
                else
                {
                    char* writeEnd = parseEscape();
                    if (writeEnd != null)
                    {
                        value.Set(value.String, (int)(start - jsonFixed), (int)(writeEnd - start));
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <param name="isTempString"></param>
        /// <returns>解析是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool ParseQuoteString(ref SubString value, int escapeIndex, char quote, int isTempString)
        {
            Parser parser = YieldPool.Default.Pop() ?? new Parser();
            try
            {
                return parser.parseQuoteString(ref value, escapeIndex, quote, isTempString);
            }
            finally { YieldPool.Default.PushNotNull(parser); }
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charStream"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <returns>解析是否成功</returns>
        internal bool ParseQuoteString(ref SubString value, CharStream charStream, int escapeIndex, char quote)
        {
            fixed (char* jsonFixed = value.String)
            {
                char* start = jsonFixed + value.Start;
                end = start + value.Length;
                Quote = quote;
                Current = start + escapeIndex;
                endChar = *end++;

                int size = parseEscapeSize();
                if (size != 0)
                {
                    int left = (int)(Current - start);
                    char* valueFixed = charStream.GetPrepSizeCurrent(size += left);
                    AutoCSer.Memory.CopyNotNull((void*)start, valueFixed, left << 1);
                    parseEscapeUnsafe(valueFixed + left);
                    charStream.ByteSize += size * sizeof(char);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult Parse<valueType>(SubString json, ref valueType value, ParseConfig config = null)
        {
            return Parse(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Parse<valueType>(SubString json, ParseConfig config = null)
        {
            valueType value = default(valueType);
            return Parse(ref json, ref value, config).State == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult Parse<valueType>(ref SubString json, ref valueType value, ParseConfig config = null)
        {
            if (json.Length == 0) return new ParseResult { State = ParseState.NullJson };
            Parser parser = YieldPool.Default.Pop() ?? new Parser();
            try
            {
                return parser.parse<valueType>(ref json, ref value, config);
            }
            finally { parser.Free(); }
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Parse<valueType>(ref SubString json, ParseConfig config = null)
        {
            valueType value = default(valueType);
            return Parse(ref json, ref value, config).State == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult Parse<valueType>(string json, ref valueType value, ParseConfig config = null)
        {
            if (string.IsNullOrEmpty(json)) return new ParseResult { State = ParseState.NullJson };
            Parser parser = YieldPool.Default.Pop() ?? new Parser();
            try
            {
                return parser.parse<valueType>(json, ref value, config);
            }
            finally { parser.Free(); }
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Parse<valueType>(string json, ParseConfig config = null)
        {
            valueType value = default(valueType);
            return Parse(json, ref value, config).State == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// Json解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">Json 字符串</param>
        /// <param name="length">Json 长度</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ParseResult UnsafeParse<valueType>(char* json, int length, ref valueType value)
        {
            Parser parser = YieldPool.Default.Pop() ?? new Parser();
            try
            {
                return parser.parse<valueType>(json, length, ref value);//, config, buffer
            }
            finally { parser.Free(); }
        }

        /// <summary>
        /// 转义字符集合尺寸
        /// </summary>
        private const int escapeCharSize = 128;
        /// <summary>
        /// 转义字符集合
        /// </summary>
        private static Pointer escapeCharData;
        /// <summary>
        /// JSON 解析数字
        /// </summary>
        internal const byte ParseNumberBit = 16;
        /// <summary>
        /// JSON 解析键值
        /// </summary>
        internal const byte ParseNameBit = 32;
        /// <summary>
        /// JSON 解析键值开始
        /// </summary>
        internal const byte ParseNameStartBit = 64;
        /// <summary>
        /// JSON 解析空格[ ,\t,\r,\n,160]
        /// </summary>
        internal const byte ParseSpaceBit = 128;
        /// <summary>
        /// JSON 解析转义查找
        /// </summary>
        internal const byte ParseEscapeSearchBit = 8;
        /// <summary>
        /// Javascript 转义位[\r,\n,\\,"]
        /// </summary>
        internal const byte EscapeBit = 4;
        /// <summary>
        /// JSON 解析字符状态位
        /// </summary>
        internal static Pointer ParseBits;

        static Parser()
        {
            JavascriptLocalMinTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddTicks(Date.LocalTimeTicks);
            JavascriptLocalMinTimeTicks = JavascriptLocalMinTime.Ticks;

            ParseBits = new Pointer { Data = Unmanaged.GetStatic64(256 + (escapeCharSize * sizeof(char)), false) };
            byte* bits = ParseBits.Byte;
            AutoCSer.Memory.Fill((ulong*)bits, ulong.MaxValue, 256 >> 3);
            for (char value = '0'; value <= '9'; ++value) bits[value] &= (ParseNumberBit | ParseNameBit) ^ 255;
            for (char value = 'A'; value <= 'F'; ++value) bits[value] &= (ParseNameBit | ParseNameStartBit | ParseNumberBit) ^ 255;
            for (char value = 'a'; value <= 'f'; ++value) bits[value] &= (ParseNameBit | ParseNameStartBit | ParseNumberBit) ^ 255;
            for (char value = 'G'; value <= 'Z'; ++value) bits[value] &= (ParseNameBit | ParseNameStartBit) ^ 255;
            for (char value = 'g'; value <= 'z'; ++value) bits[value] &= (ParseNameBit | ParseNameStartBit) ^ 255;
            bits['\t'] &= ParseSpaceBit ^ 255;
            bits['\r'] &= (ParseSpaceBit | EscapeBit) ^ 255;
            bits['\n'] &= (ParseSpaceBit | ParseEscapeSearchBit | EscapeBit) ^ 255;
            bits[' '] &= ParseSpaceBit ^ 255;
            bits[0xA0] &= ParseSpaceBit ^ 255;
            bits['x'] &= ParseNumberBit ^ 255;
            bits['+'] &= ParseNumberBit ^ 255;
            bits['-'] &= ParseNumberBit ^ 255;
            bits['.'] &= ParseNumberBit ^ 255;
            bits['_'] &= (ParseNameBit | ParseNameStartBit) ^ 255;
            bits['\''] &= (ParseNameStartBit | ParseEscapeSearchBit) ^ 255;
            bits['"'] &= (ParseNameStartBit | ParseEscapeSearchBit | EscapeBit) ^ 255;
            bits['\\'] &= (ParseEscapeSearchBit | EscapeBit) ^ 255;

            char* escapeCharDataChar = (char*)(bits + 256);
            escapeCharData = new Pointer { Data = escapeCharDataChar };
            for (int value = 0; value != escapeCharSize; ++value) escapeCharDataChar[value] = (char)value;
            escapeCharDataChar['0'] = ' ';
            escapeCharDataChar['B'] = escapeCharDataChar['b'] = '\b';
            escapeCharDataChar['F'] = escapeCharDataChar['f'] = '\f';
            escapeCharDataChar['N'] = escapeCharDataChar['n'] = '\n';
            escapeCharDataChar['R'] = escapeCharDataChar['r'] = '\r';
            escapeCharDataChar['T'] = escapeCharDataChar['t'] = '\t';
            escapeCharDataChar['V'] = escapeCharDataChar['v'] = '\v';

            parseMethods = AutoCSer.DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(Parser).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(ParseMethod), false))
                {
                    parseMethods.Add(method.GetParameters()[0].ParameterType.GetElementType(), method);
                }
            }
        }
    }
}
