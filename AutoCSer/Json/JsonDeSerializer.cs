using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Json;
using AutoCSer.Memory;

namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer : AutoCSer.Threading.Link<JsonDeSerializer>
    {
        /// <summary>
        /// 字符串缓存区大小
        /// </summary>
        private const int stringBufferSize = 33;
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
        internal static readonly JsonDeSerializeAttribute AllMemberAttribute = (JsonDeSerializeAttribute)AutoCSer.Configuration.Common.Get(typeof(JsonDeSerializeAttribute)) ?? new JsonDeSerializeAttribute { Filter = Metadata.MemberFilters.Instance, IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly DeSerializeConfig DefaultConfig = (DeSerializeConfig)AutoCSer.Configuration.Common.Get(typeof(DeSerializeConfig)) ?? new DeSerializeConfig();
        /// <summary>
        /// 字符状态位查询表格
        /// </summary>
        private readonly byte* bits = AutoCSer.JsonDeSerializer.DeSerializeBits.Byte;
        /// <summary>
        /// 转义字符集合
        /// </summary>
        private readonly char* escapeChars = escapeCharData.Char;
        /// <summary>
        /// 配置参数
        /// </summary>
        internal DeSerializeConfig Config;
        /// <summary>
        /// 临时字符串
        /// </summary>
        private string stringBuffer = string.Empty;
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
        private LeftArray<KeyValue<Type, object>> anonymousTypes = new LeftArray<KeyValue<Type, object>>(0);
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
        internal DeSerializeState DeSerializeState;
        /// <summary>
        /// 解析状态
        /// </summary>
        public DeSerializeState State { get { return DeSerializeState; } }
        /// <summary>
        /// 获取解析状态
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <returns>解析状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DeSerializeState GetState(JsonDeSerializer jsonDeSerializer)
        {
            return jsonDeSerializer.DeSerializeState;
        }
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
        internal JsonDeSerializer() { }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
        private DeSerializeResult deSerialize<valueType>(ref SubString json, ref valueType value, DeSerializeConfig config)
        {
            fixed (char* jsonFixed = (this.json = json.GetFixedBuffer()))
            {
                Current = (this.jsonFixed = jsonFixed) + json.Start;
                this.Config = config ?? DefaultConfig;
                end = Current + json.Length;
                deSerialize(ref value);
                if (DeSerializeState == DeSerializeState.Success) return new DeSerializeResult { State = DeSerializeState.Success, MemberMap = MemberMap };
                return new DeSerializeResult { State = DeSerializeState, MemberMap = MemberMap, Json = json, Index = (int)(Current - jsonFixed) - json.Start };
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
        private DeSerializeResult deSerialize<valueType>(string json, ref valueType value, DeSerializeConfig config)
        {
            fixed (char* jsonFixed = (this.json = json))
            {
                Current = this.jsonFixed = jsonFixed;
                this.Config = config ?? DefaultConfig;
                end = jsonFixed + json.Length;
                deSerialize(ref value);
                if (DeSerializeState == DeSerializeState.Success) return new DeSerializeResult { State = DeSerializeState.Success, MemberMap = MemberMap };
                return new DeSerializeResult { State = DeSerializeState, MemberMap = MemberMap, Json = json, Index = (int)(Current - jsonFixed) };
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
        private DeSerializeResult deSerialize<valueType>(char* json, int length, ref valueType value)
        {
            Config = DefaultConfig;//config ?? 
            //Buffer = buffer;
            end = (jsonFixed = Current = json) + length;
            deSerialize(ref value);
            if (DeSerializeState == DeSerializeState.Success) return new DeSerializeResult { State = DeSerializeState.Success, MemberMap = MemberMap };
            return new DeSerializeResult { State = DeSerializeState, MemberMap = MemberMap, Json = Config.IsErrorJsonNewString ? new string(json, 0, length) : null, Index = (int)(Current - jsonFixed) };
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>解析状态</returns>
        private void deSerialize<valueType>(ref valueType value)
        {
            if (endChar != *(end - 1))
            {
                if (((endChar = *(end - 1)) & 0xff00) == 0)
                {
                    isEndSpace = (bits[(byte)endChar] & AutoCSer.JsonDeSerializer.DeSerializeSpaceBit) == 0;
                    if ((uint)(endChar - '0') < 10) isEndDigital = isEndHex = isEndNumber = true;
                    else
                    {
                        isEndDigital = false;
                        if ((uint)((endChar | 0x20) - 'a') < 6) isEndHex = isEndNumber = true;
                        else
                        {
                            isEndHex = false;
                            isEndNumber = (bits[(byte)endChar] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) == 0;
                        }
                    }
                }
                else isEndSpace = isEndDigital = isEndHex = isEndNumber = false;
            }
            DeSerializeState = DeSerializeState.Success;
            TypeDeSerializer<valueType>.DeSerialize(this, ref value);
            if (DeSerializeState == DeSerializeState.Success)
            {
                if (Current == end || !Config.IsEndSpace) return;
                space();
                if (DeSerializeState == DeSerializeState.Success)
                {
                    if (Current == end) return;
                    DeSerializeState = DeSerializeState.CrashEnd;
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
        public bool TypeDeSerialize<valueType>(ref valueType value)
        {
            TypeDeSerializer<valueType>.DeSerialize(this, ref value);
            return DeSerializeState == DeSerializeState.Success;
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType TypeDeSerialize<valueType>()
        {
            valueType value = default(valueType);
            TypeDeSerializer<valueType>.DeSerialize(this, ref value);
            return DeSerializeState == DeSerializeState.Success ? value : default(valueType);
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
            DeSerializeState = DeSerializeState.Custom;
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
            if (DeSerializeState == DeSerializeState.Success) DeSerializeState = DeSerializeState.Custom;
            return false;
        }
        /// <summary>
        /// 释放 JSON 解析器（单线程模式）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeThreadStatic()
        {
            json = null;
            Config = null;
            //Buffer = null;
            MemberMap = null;
            anonymousTypes.SetEmpty();
        }
        /// <summary>
        /// 释放 JSON 解析器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            YieldPool.Default.Push(this);
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
                    if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeSpaceBit) | *(((byte*)Current) + 1)) != 0)
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
                while (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeSpaceBit) | *(((byte*)Current) + 1)) == 0) ++Current;
                if (*Current != '/' || Current == end) return;
            }
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.UnknownNote;
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
                    DeSerializeState = DeSerializeState.NoteNotRound;
                    return;
                }
                if (endChar == '/')
                {
                    do
                    {
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.NoteNotRound;
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
                            DeSerializeState = DeSerializeState.NoteNotRound;
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
                            DeSerializeState = DeSerializeState.NoteNotRound;
                            return;
                        }
                    }
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.NoteNotRound;
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
            DeSerializeState = DeSerializeState.UnknownNote;
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
                DeSerializeState = DeSerializeState.NotNull;
            }
            return false;
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否 null</returns>
        private bool deSerialize(out bool value)
        {
        START:
            switch (*Current & 7)
            {
                case 'f' & 7:
                    if (*Current == 'f')
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char)
                            && *(long*)(Current + 1) == 'a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48))
                        {
                            value = false;
                            Current += 5;
                            return false;
                        }
                    }
                    else if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char)
                        && *(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                    {
                        value = false;
                        Current += 4;
                        return true;
                    }
                    break;
                case 't' & 7:
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char)
                        && *(long*)(Current) == 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48))
                    {
                        value = true;
                        Current += 4;
                        return false;
                    }
                    break;
                case '0' & 7:
                    if (*Current == '0')
                    {
                        value = false;
                        ++Current;
                        return false;
                    }
                    break;
                case '1' & 7:
                    if (*Current == '1')
                    {
                        value = true;
                        ++Current;
                        return false;
                    }
                    break;
                case '"' & 7:
                case '\'' & 7:
                    if ((*Current == '"' || *Current == '\'') && Quote <= 1)
                    {
                        Quote = *Current;
                        if ((int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                        {
                            ++Current;
                            bool isNull = deSerialize(out value);
                            if (!isNull)
                            {
                                if (Current >= end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotBool;
                                return false;
                            }
                        }
                    }
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (DeSerializeState != DeSerializeState.Success) return value = false;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return value = false;
                    }
                    Quote = (char)1;
                    goto START;
                }
            }
            DeSerializeState = DeSerializeState.NotBool;
            return value = false;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private uint deSerializeUInt32(uint value)
        {
            uint number;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9) return value;
                    value = value * 10 + number;
                    if (++Current == end) return value;
                }
                while (true);
            }
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value = value * 10 + number;
                ++Current;
            }
            return value;
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void deSerializeInt32Next(ref uint value)
        {
            uint number;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9) return;
                    value = value * 10 + number;
                    if (++Current == end) return;
                }
                while (true);
            }
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value = value * 10 + number;
                ++Current;
            }
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        private void deSerializeHex32(ref uint value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    value = 0;
                    DeSerializeState = DeSerializeState.NotHex;
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
                    value = (value << 4) + number;
                    //if ((value & 0xf0000000U) != 0) return;
                }
                while (++Current != end);
            }
            else
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return;
                        number += 10;
                    }
                    value = (value << 4) + number;
                    ++Current;
                    //if ((value & 0xf0000000U) != 0) return;
                }
                while (true);
            }
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private ulong deSerializeUInt64(uint value)
        {
            char* end32 = Current + 8;
            if (end32 > end) end32 = end;
            uint number;
            do
            {
                if ((number = (uint)(*Current - '0')) > 9) return value;
                value = value * 10 + number;
            }
            while (++Current != end32);
            if (Current == end) return value;
            ulong value64 = value;
            if (isEndDigital)
            {
                do
                {
                    if ((number = (uint)(*Current - '0')) > 9) return value64;
                    value64 = value64 * 10 + number;
                    if (++Current == end) return value64;
                }
                while (true);
            }
            while ((number = (uint)(*Current - '0')) < 10)
            {
                value64 = value64 * 10 + number;
                ++Current;
            }
            return value64;
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <returns>数字</returns>
        private ulong deSerializeHex64()
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    DeSerializeState = DeSerializeState.NotHex;
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
        private uint deSerializeHex2()
        {
            uint code = (uint)(*++Current - '0'), number = (uint)(*++Current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            return (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        private uint deSerializeHex4()
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
            DeSerializeState = DeSerializeState.NotNumber;
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
            DeSerializeState = DeSerializeState.NotNumber;
            return NumberType.Error;
        }
        /// <summary>
        /// 查找数字结束位置
        /// </summary>
        /// <param name="numberEnd">数字结束位置</param>
        /// <returns>数字类型</returns>
        private NumberType searchNumber(ref char* numberEnd)
        {
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)Current) + 1)) != 0)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return NumberType.Error;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return NumberType.Error;
                }
                if (*Current == '"' || *Current == '\'')
                {
                    Quote = *Current;
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return NumberType.Error;
                            }
                        }
                    }
                    return NumberType.String;
                }
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)Current) + 1)) != 0) return isNaNPositiveInfinity();
            }
            numberEnd = Current;
            if (isEndNumber)
            {
                while (++numberEnd != end && ((bits[*(byte*)numberEnd] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            else
            {
                while (((bits[*(byte*)++numberEnd] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
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
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)Current) + 1)) != 0)
            {
                if (isNull()) return NumberType.Null;
                space();
                if (DeSerializeState != DeSerializeState.Success) return NumberType.Error;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return NumberType.Error;
                }
                if (*Current == '"' || *Current == '\'')
                {
                    Quote = *Current;
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return NumberType.Error;
                            }
                        }
                    }
                    return NumberType.String;
                }
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)Current) + 1)) != 0) return isNull() ? NumberType.Null : isNaNPositiveInfinity();
            }
            numberEnd = Current;
            if (isEndNumber)
            {
                while (++numberEnd != end && ((bits[*(byte*)numberEnd] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            else
            {
                while (((bits[*(byte*)++numberEnd] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)numberEnd) + 1)) == 0) ;
            }
            return (int)(numberEnd - Current) != 1 || *Current != '-' ? NumberType.Number : isNegativeInfinity();
        }
        ///// <summary>
        ///// 获取数字字符串
        ///// </summary>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //private string getNumberString(char* end)
        //{
        //    int length = (int)(end - Current);
        //    return json == null || json.Length != length ? new string(Current, 0, length) : json;
        //}
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否 null</returns>
        private bool deSerializeDateTime(ref DateTime value)
        {
        START:
            switch (*Current)
            {
                case '"':
                case '\'':
                    if (Quote <= 1)
                    {
                        if ((int)((byte*)end - (byte*)Current) >= 10 * sizeof(char))
                        {
                            char* start = Current;
                            switch (deSerializeDateTimeString(ref value))
                            {
                                case 0:
                                    if (Current < end)
                                    {
                                        if (*Current == *start)
                                        {
                                            ++Current;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        DeSerializeState = DeSerializeState.CrashEnd;
                                        return false;
                                    }
                                    break;
                                case 1:
                                    if (Current < end)
                                    {
                                        if (*Current == *start) ++Current;
                                        else DeSerializeState = DeSerializeState.NotDateTime;
                                    }
                                    else DeSerializeState = DeSerializeState.CrashEnd;
                                    return false;
                            }
                            Current = start;
                            if (JsonSerializer.CustomConfig.DeSerialize(this, ref value))
                            {
                                DeSerializeState = DeSerializeState.Success;
                                return false;
                            }
                        }
                        Quote = (char)1;
                    }
                    break;
                case 'n':
                    if ((int)((byte*)end - (byte*)Current) >= 11 * sizeof(char))
                    {
                        if (((*(long*)(Current + 1) ^ ('e' + ('w' << 16) + ((long)' ' << 32) + ((long)'D' << 48))) | (*(long*)(Current + 5) ^ ('a' + ('t' << 16) + ((long)'e' << 32) + ((long)'(' << 48)))) == 0)
                        {
                            Current += 9;
                            deSerializeDateTimeMillisecond(ref value);
                            return false;
                        }
                    }
                    if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
                    {
                        Current += 4;
                        return true;
                    }
                    Quote = (char)1;
                    break;
            }
            if (Quote == 0)
            {
                char* current = Current;
                space();
                if (current != Current)
                {
                    if (DeSerializeState == DeSerializeState.Success)
                    {
                        if (Current != end)
                        {
                            Quote = (char)1;
                            goto START;
                        }
                        DeSerializeState = DeSerializeState.CrashEnd;
                    }
                    return false;
                }
            }
            JsonSerializer.CustomConfig.DeSerializeNotDateTime(this, ref value);
            return false;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool deSerializeDateTimeMillisecond(ref DateTime value)
        {
            long millisecond = 0;
            CallSerialize(ref millisecond);
            if (DeSerializeState == DeSerializeState.Success)
            {
                if (Current != end)
                {
                    if (*Current == ')')
                    {
                        value = JavascriptLocalMinTime.AddTicks(millisecond * TimeSpan.TicksPerMillisecond);
                        ++Current;
                        return true;
                    }
                    DeSerializeState = DeSerializeState.NotDateTime;
                }
                else DeSerializeState = DeSerializeState.CrashEnd;
            }
            return false;
        }
        /// <summary>
        /// 时间值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int deSerializeDateTimeString(ref DateTime value)
        {
            switch (*++Current & 15)
            {
                case '0' & 15:
                case '1' & 15:
                case '2' & 15:
                case '3' & 15:
                case '4' & 15:
                case '5' & 15:
                case '6' & 15:
                case '7' & 15:
                case '8' & 15:
                case '9' & 15:
                    uint year = (uint)(*Current - '0');
                    if (year < 10)
                    {
                        ++Current;
                        deSerializeInt32Next(ref year);
                        if ((int)((byte*)end - (byte*)Current) >= 5 * sizeof(char))
                        {
                            switch (*Current)
                            {
                                case '/':
                                case '-':
                                    Quote = *Current;
                                    uint month = deSerializeDateTime();
                                    if ((month - 1U) <= (12U - 1U) && *Current == Quote && (int)((byte*)end - (byte*)Current) >= 3 * sizeof(char))
                                    {
                                        uint day = deSerializeDateTime();
                                        if ((day - 1U) <= (31U - 1U) && Current < end)
                                        {
                                            try
                                            {
                                                switch(*Current)
                                                {
                                                    case ' ':
                                                    case 'T':
                                                        if ((int)((byte*)end - (byte*)Current) >= 7 * sizeof(char))
                                                        {
                                                            uint hour = deSerializeDateTime();
                                                            if (hour < 24 && *Current == ':')
                                                            {
                                                                uint minute = deSerializeDateTime();
                                                                if (minute < 60)
                                                                {
                                                                    uint second;
                                                                    if (*Current == ':')
                                                                    {
                                                                        if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char)) return 2;
                                                                        second = deSerializeDateTime();
                                                                        if (second >= 60 || Current >= end) return 2;
                                                                    }
                                                                    else second = 0;

                                                                    long ticks;
                                                                    switch (*Current)
                                                                    {
                                                                        case ' ':
                                                                        case '.':
                                                                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char)) return 2;
                                                                            ticks = deSerializeDateTimeTicks();
                                                                            if (Current >= end && ticks < 0) return 2;
                                                                            break;
                                                                        default: ticks = 0; break;
                                                                    }

                                                                    bool zone;
                                                                    switch (*Current & 3)
                                                                    {
                                                                        case 'Z' & 3:
                                                                            if (*Current == 'Z')
                                                                            {
                                                                                ++Current;
                                                                                value = new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second, DateTimeKind.Utc).AddTicks(ticks);
                                                                                return 0;
                                                                            }
                                                                            goto TICKS;
                                                                        case '+' & 3:
                                                                            if (*Current == '+')
                                                                            {
                                                                                if ((int)((byte*)end - (byte*)Current) < 7 * sizeof(char)) return 2;
                                                                                zone = true;
                                                                                break;
                                                                            }
                                                                            goto TICKS;
                                                                        case '-' & 3:
                                                                            if (*Current == '-')
                                                                            {
                                                                                if ((int)((byte*)end - (byte*)Current) < 7 * sizeof(char)) return 2;
                                                                                zone = false;
                                                                                break;
                                                                            }
                                                                            goto TICKS;
                                                                        default:
                                                                            TICKS:
                                                                            value = new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second).AddTicks(ticks);
                                                                            return 0;
                                                                    }
                                                                    uint zoneHour = deSerializeDateTime();
                                                                    if (zoneHour < 24 && *Current == ':')
                                                                    {
                                                                        uint zoneMinute = deSerializeDateTime();
                                                                        if (zoneMinute < 60)
                                                                        {
                                                                            long zoneTicks = (int)(zoneHour * 60 + zoneMinute) * TimeSpan.TicksPerMinute;
                                                                            if (zone) zoneTicks = -zoneTicks;
                                                                             value = new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second, DateTimeKind.Local)
                                                                                .AddTicks(zoneTicks + ticks + Date.LocalTimeTicks);
                                                                            return 0;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        value = new DateTime((int)year, (int)month, (int)day);
                                                        return 0;
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case '/' & 15:
                    if (*(long*)Current == '/' + ('D' << 16) + ((long)'a' << 32) + ((long)'t' << 48) && *(int*)(Current + 4) == 'e' + ('(' << 16))
                    {
                        Current += 6;
                        if (deSerializeDateTimeMillisecond(ref value) && *Current == '/')
                        {
                            ++Current;
                            return 1;
                        }
                    }
                    break;
            }
            return 2;
        }
        /// <summary>
        /// 时间片段值解析
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private uint deSerializeDateTime()
        {
            uint high = (uint)(*(Current + 1) - '0');
            if (high < 10)
            {
                uint low = (uint)(*(Current + 2) - '0');
                if (low < 10)
                {
                    Current += 3;
                    return high * 10 + low;
                }
                Current += 2;
                return high;
            }
            return uint.MinValue;
        }
        /// <summary>
        /// 时间时钟周期解析
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private long deSerializeDateTimeTicks()
        {
            uint high = (uint)(*(Current + 1) - '0');
            if (high < 10)
            {
                uint low = (uint)(*(Current + 2) - '0');
                if (low < 10)
                {
                    int size = (int)(end - Current);
                    high = high * 10 + low;
                    if (size > 4 && (low = (uint)(*(Current + 3) - '0')) < 10)
                    {
                        high = high * 10 + low;
                        if (size > 5 && (low = (uint)(*(Current + 4) - '0')) < 10)
                        {
                            high = high * 10 + low;
                            if (size > 6 && (low = (uint)(*(Current + 5) - '0')) < 10)
                            {
                                high = high * 10 + low;
                                if (size > 7 && (low = (uint)(*(Current + 6) - '0')) < 10)
                                {
                                    high = high * 10 + low;
                                    if (size > 8 && (low = (uint)(*(Current + 7) - '0')) < 10)
                                    {
                                        Current += 8;
                                        return high * 10 + low;
                                    }
                                    else Current += 7;
                                    return high * 10;
                                }
                                else Current += 6;
                                return high * 100;
                            }
                            else Current += 5;
                            return high * 1000;
                        }
                        else Current += 4;
                        return high * 10000;
                    }
                    else Current += 3;
                    return high * 100000;
                }
                else Current += 2;
                return high * 1000000;
            }
            return long.MinValue;
        }
        ///// <summary>
        ///// 时间解析 /Date(xxx)/
        ///// </summary>
        ///// <param name="timeString"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private unsafe static bool deSerializeTime(string timeString, out DateTime value)
        //{
        //    if (timeString.Length > 8)
        //    {
        //        fixed (char* timeFixed = timeString)
        //        {
        //            if (*(long*)(timeFixed + 1) == 'D' + ('a' << 16) + ((long)'t' << 32) + ((long)'e' << 48))
        //            {
        //                char* end = timeFixed + (timeString.Length - 2);
        //                if (((*(timeFixed + 5) ^ '(') | (*(int*)end ^ (')' + ('/' << 16)))) == 0)
        //                {
        //                    char* start = timeFixed + 6;
        //                    bool isSign;
        //                    if (*start == '-')
        //                    {
        //                        if (timeString.Length == 9)
        //                        {
        //                            value = DateTime.MinValue;
        //                            return false;
        //                        }
        //                        isSign = true;
        //                        ++start;
        //                    }
        //                    else isSign = false;
        //                    uint code = (uint)(*start - '0');
        //                    if (code < 10)
        //                    {
        //                        long millisecond = code;
        //                        while (++start != end)
        //                        {
        //                            if ((code = (uint)(*start - '0')) >= 10)
        //                            {
        //                                value = DateTime.MinValue;
        //                                return false;
        //                            }
        //                            millisecond *= 10;
        //                            millisecond += code;
        //                        }
        //                        value = JavascriptLocalMinTime.AddTicks((isSign ? -millisecond : millisecond) * TimeSpan.TicksPerMillisecond);
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    value = DateTime.MinValue;
        //    return false;
        //}
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        private void deSerialize(ref GuidCreator value)
        {
            if ((int)((byte*)end - (byte*)Current) < 38 * sizeof(char))
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            Quote = *Current;
            value.Byte3 = (byte)deSerializeHex2();
            value.Byte2 = (byte)deSerializeHex2();
            value.Byte1 = (byte)deSerializeHex2();
            value.Byte0 = (byte)deSerializeHex2();
            if (*++Current != '-')
            {
                DeSerializeState = DeSerializeState.NotGuid;
                return;
            }
            value.Byte45 = (ushort)deSerializeHex4();
            if (*++Current != '-')
            {
                DeSerializeState = DeSerializeState.NotGuid;
                return;
            }
            value.Byte67 = (ushort)deSerializeHex4();
            if (*++Current != '-')
            {
                DeSerializeState = DeSerializeState.NotGuid;
                return;
            }
            value.Byte8 = (byte)deSerializeHex2();
            value.Byte9 = (byte)deSerializeHex2();
            if (*++Current != '-')
            {
                DeSerializeState = DeSerializeState.NotGuid;
                return;
            }
            value.Byte10 = (byte)deSerializeHex2();
            value.Byte11 = (byte)deSerializeHex2();
            value.Byte12 = (byte)deSerializeHex2();
            value.Byte13 = (byte)deSerializeHex2();
            value.Byte14 = (byte)deSerializeHex2();
            value.Byte15 = (byte)deSerializeHex2();
            if (*++Current == Quote)
            {
                ++Current;
                return;
            }
            DeSerializeState = DeSerializeState.NotGuid;
        }
        /// <summary>
        /// 临时字符串解析（不处理转义）
        /// </summary>
        /// <returns></returns>
        internal string GetQuoteStringBuffer()
        {
            Quote = *Current;
            if (++Current != end)
            {
                if (stringBuffer.Length == 0) stringBuffer = new string((char)0, stringBufferSize);
                int length = 0;
                fixed (char* bufferFixed = stringBuffer)
                {
                    if (endChar == Quote)
                    {
                        while (*Current != Quote)
                        {
                            if (length != stringBufferSize)
                            {
                                *(bufferFixed + length) = *Current++;
                                ++length;
                            }
                            else return string.Empty;
                        }
                    }
                    else
                    {
                        do
                        {
                            if (Current != end)
                            {
                                if (*Current != Quote)
                                {
                                    if (length != stringBufferSize)
                                    {
                                        *(bufferFixed + length) = *Current++;
                                        ++length;
                                    }
                                    else return string.Empty;
                                }
                                else break;
                            }
                            else return string.Empty;
                        }
                        while (true);
                    }
                    if (length != 0)
                    {
                        ++Current;
                        if (length != stringBufferSize)
                        {
                            fillStringBuffer(bufferFixed, length);
                            *(bufferFixed + 32) = (char)0;
                        }
                        return stringBuffer;
                    }
                }
            }
            return string.Empty;
        }
        ///// <summary>
        ///// 临时字符串解析（不处理转义）
        ///// </summary>
        ///// <returns>长度为0表示失败</returns>
        //private int deSerializeStringBuffer()
        //{
        //    Quote = *Current;
        //    if (++Current != end)
        //    {
        //        if (stringBuffer == null) stringBuffer = new string((char)0, stringBufferSize);
        //        int length = 0;
        //        fixed (char* bufferFixed = stringBuffer)
        //        {
        //            if (endChar == Quote)
        //            {
        //                while (*Current != Quote)
        //                {
        //                    if (length == stringBufferSize) return 0;
        //                    *(bufferFixed + length) = *Current++;
        //                    ++length;
        //                }
        //            }
        //            else
        //            {
        //                do
        //                {
        //                    if (Current == end) return 0;
        //                    if (*Current == Quote) break;
        //                    if (length == stringBufferSize) return 0;
        //                    *(bufferFixed + length) = *Current++;
        //                    ++length;
        //                }
        //                while (true);
        //            }
        //            if (length != 0)
        //            {
        //                ++Current;
        //                if (length != stringBufferSize)
        //                {
        //                    fillStringBuffer(bufferFixed, length);
        //                    *(bufferFixed + 32) = (char)0;
        //                }
        //                return length;
        //            }
        //        }
        //    }
        //    return 0;
        //}
        /// <summary>
        /// 临时字符串填充空格
        /// </summary>
        /// <param name="bufferFixed"></param>
        /// <param name="length"></param>
        private void fillStringBuffer(char* bufferFixed, int length)
        {
            if (length <= 29)
            {
                if ((length & 3) != 0)
                {
                    //*(long*)(bufferFixed + length) = 0x20002000200020;
                    *(long*)(bufferFixed + length) = 0;
                    length = (length + 3) & (60);
                }
                while (length != 32)
                {
                    //*(long*)(bufferFixed + length) = 0x20002000200020;
                    *(long*)(bufferFixed + length) = 0;
                    length += 4;
                }
            }
            else
            {
                //while (length != 32) *(bufferFixed + length++) = ' ';
                while (length != 32) *(bufferFixed + length++) = (char)0;
            }
        }
        /// <summary>
        /// 获取数字字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal string GetStringBuffer(int length)
        {
            if ((uint)(length - 1) <= (32U - 1))
            {
                if (stringBuffer.Length == 0) stringBuffer = new string((char)0, stringBufferSize);
                fixed (char* bufferFixed = stringBuffer)
                {
                    byte* read = (byte*)Current, write = (byte*)bufferFixed, writeEnd = write + (((length << 1) + 6) & (127 - 7));
                    do
                    {
                        *(long*)write = *(long*)read;
                        write += sizeof(long);
                        read += sizeof(long);
                    }
                    while (write != writeEnd);
                    fillStringBuffer(bufferFixed, length);
                    *(bufferFixed + 32) = (char)0;
                }
                return stringBuffer;
            }
            return string.Empty;
        }
        ///// <summary>
        ///// 获取数字字符串
        ///// </summary>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //private int deSerializeStringBuffer(char* end)
        //{
        //    int length = (int)(end - Current);
        //    if ((uint)(length - 1) <= (32U - 1))
        //    {
        //        if (stringBuffer == null) stringBuffer = new string((char)0, stringBufferSize);
        //        fixed (char* bufferFixed = stringBuffer)
        //        {
        //            byte* read = (byte*)Current, write = (byte*)bufferFixed, writeEnd = write + (((length << 1) + 6) & (127 - 7));
        //            do
        //            {
        //                *(long*)write = *(long*)read;
        //                write += sizeof(long);
        //                read += sizeof(long);
        //            }
        //            while (write != writeEnd);
        //            fillStringBuffer(bufferFixed, length);
        //            *(bufferFixed + 32) = (char)0;
        //        }
        //        return length;
        //    }
        //    return 0;
        //}
        /// <summary>
        /// 查找字符串中的转义符
        /// </summary>
        private byte searchEscape()
        {
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote) return 0;
                        if (*Current == '\\') return 1;
                        if (*Current == '\n')
                        {
                            DeSerializeState = DeSerializeState.StringEnter;
                            return 0;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote) return 0;
                    if (*Current == '\\') return 1;
                    if (*Current == '\n')
                    {
                        DeSerializeState = DeSerializeState.StringEnter;
                        return 0;
                    }
                }
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return 0;
                }
            }
            while (true);
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <returns>写入结束位置</returns>
        private char* deSerializeEscape()
        {
            char* write = Current;
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return null;
                }
                *write++ = (char)deSerializeHex4();
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return null;
                }
                *write++ = (char)deSerializeHex2();
            }
            else
            {
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return null;
                }
                *write++ = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
            }
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return null;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return write;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            DeSerializeState = DeSerializeState.StringEnter;
                            return null;
                        }
                    }
                    *write++ = *Current++;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return write;
                    }
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        DeSerializeState = DeSerializeState.StringEnter;
                        return null;
                    }
                }
                *write++ = *Current++;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return null;
                }
            }
            while (true);
        }
        /// <summary>
        /// 获取转义后的字符串长度
        /// </summary>
        /// <returns>字符串长度</returns>
        private int deSerializeEscapeSize()
        {
            char* start = Current;
            int length = 0;
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return 0;
                }
                length += 5;
                Current += 5;
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return 0;
                }
                length += 3;
                Current += 3;
            }
            else
            {
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return 0;
                }
                ++length;
                ++Current;
            }
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return 0;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
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
                            DeSerializeState = DeSerializeState.StringEnter;
                            return 0;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
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
                        DeSerializeState = DeSerializeState.StringEnter;
                        return 0;
                    }
                }
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return 0;
                }
            }
            while (true);
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="write">当前写入位置</param>
        private void deSerializeEscapeUnsafe(char* write)
        {
        NEXT:
            if (*++Current == 'u') *write++ = (char)deSerializeHex4();
            else if (*Current == 'x') *write++ = (char)deSerializeHex2();
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
        private string deSerializeEscape(char* start)
        {
            int size = deSerializeEscapeSize();
            if (size != 0)
            {
                int left = (int)(Current - start);
                string value = AutoCSer.Extensions.StringExtension.FastAllocateString(left + size);
                fixed (char* valueFixed = value)
                {
                    new Span<char>(start, left).CopyTo(new Span<char>(valueFixed, left));
                    deSerializeEscapeUnsafe(valueFixed + left);
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <returns>字符串,失败返回null</returns>
        private string deSerializeString()
        {
            Quote = *Current;
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return null;
            }
            char* start = Current;
            if (searchEscape() == 0) return DeSerializeState == DeSerializeState.Success ? new string(start, 0, (int)(Current++ - start)) : null;
            if (Config.IsTempString)
            {
                char* writeEnd = deSerializeEscape();
                return writeEnd != null ? new string(start, 0, (int)(writeEnd - start)) : null;
            }
            return deSerializeEscape(start);
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                Current += 5;
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                Current += 3;
            }
            else
            {
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                ++Current;
            }
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote) return;
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            DeSerializeState = DeSerializeState.StringEnter;
                            return;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote) return;
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        DeSerializeState = DeSerializeState.StringEnter;
                        return;
                    }
                }
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 解析字符串节点
        /// </summary>
        /// <param name="value"></param>
        private void deSerializeStringNode(ref Node value)
        {
            Quote = *Current;
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            char* start = Current;
            if (searchEscape() == 0)
            {
                if (DeSerializeState == DeSerializeState.Success)
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
                if (DeSerializeState == DeSerializeState.Success)
                {
                    value.SubString.Set(this.json, (int)(start - jsonFixed), (int)(Current - start));
                    value.SetQuoteString((int)(escapeStart - start), Quote, Config.IsTempString);
                    ++Current;
                }
            }
            else
            {
                string newValue = deSerializeEscape(start);
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
        //private void deSerializeListNode(ref LeftArray<Node> value)
        //{
        //    if (++Current == end)
        //    {
        //        DeSerializeState = DeSerializeState.CrashEnd;
        //        return;
        //    }
        //    if (IsFirstArrayValue())
        //    {
        //        do
        //        {
        //            Node node = default(Node);
        //            DeSerialize(ref node);
        //            if (DeSerializeState != DeSerializeState.Success) return;
        //            value.Add(node);
        //        }
        //        while (IsNextArrayValue());
        //    }
        //}
        ///// <summary>
        ///// 解析字典节点
        ///// </summary>
        ///// <param name="value"></param>
        //private void deSerializeDictionaryNode(ref LeftArray<KeyValue<Node, Node>> value)
        //{
        //    if (++Current == end)
        //    {
        //        DeSerializeState = DeSerializeState.CrashEnd;
        //        return;
        //    }
        //    if (IsFirstObject())
        //    {
        //        do
        //        {
        //            Node name = default(Node);
        //            if (*Current == '"' || *Current == '\'') deSerializeStringNode(ref name);
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
        //            if (DeSerializeState != DeSerializeState.Success || SearchColon() == 0) return;
        //            Node node = default(Node);
        //            DeSerialize(ref node);
        //            if (DeSerializeState != DeSerializeState.Success) return;
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
            LeftArray<NodeType> TypeArray = new LeftArray<NodeType>(0);
            NEXTNODE:
            space();
            if (DeSerializeState != DeSerializeState.Success) return;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            switch (*Current & 7)
            {
                case '"' & 7:
                case '\'' & 7:
                    if (*Current == '"' || *Current == '\'')
                    {
                        ignoreString();
                        if (DeSerializeState == DeSerializeState.Success) goto CHECKNODE;
                        return;
                    }
                    goto NUMBER;
                case '{' & 7:
                    if (*Current == '{')
                    {
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if (IsFirstObject())
                        {
                            TypeArray.Add(NodeType.Dictionary);
                            goto DICTIONARYNAME;
                        }
                        if (DeSerializeState == DeSerializeState.Success) goto CHECKNODE;
                        return;
                    }
                    if (*Current == '[')
                    {
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if (IsFirstArrayValue())
                        {
                            TypeArray.Add(NodeType.Array);
                            goto NEXTNODE;
                        }
                        if (DeSerializeState == DeSerializeState.Success) goto CHECKNODE;
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
                            if (DeSerializeState != DeSerializeState.Success) return;
                            if (Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                    if (DeSerializeState == DeSerializeState.Success) goto CHECKNODE;
                    return;
            }
            DeSerializeState = DeSerializeState.UnknownValue;
            return;

            DICTIONARYNAME:
            if (*Current == '\'' || *Current == '"') ignoreString();
            else ignoreName();
            if (DeSerializeState != DeSerializeState.Success || SearchColon() == 0) return;
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
        //    if (DeSerializeState != DeSerializeState.Success) return;
        //    if (Current == end)
        //    {
        //        DeSerializeState = DeSerializeState.CrashEnd;
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
        //                    if (DeSerializeState != DeSerializeState.Success) return;
        //                    if (Current == end)
        //                    {
        //                        DeSerializeState = DeSerializeState.CrashEnd;
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
        //    DeSerializeState = DeSerializeState.UnknownValue;
        //}
        /// <summary>
        /// 忽略字符串
        /// </summary>
        private void ignoreString()
        {
            Quote = *Current;
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            //char* start = Current;
            if (searchEscape() == 0)
            {
                if (DeSerializeState == DeSerializeState.Success) ++Current;
                return;
            }
        NEXT:
            if (*++Current == 'u')
            {
                if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                Current += 5;
            }
            else if (*Current == 'x')
            {
                if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                Current += 3;
            }
            else
            {
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                ++Current;
            }
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            if (endChar == Quote)
            {
                do
                {
                    if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                    {
                        if (*Current == Quote)
                        {
                            ++Current;
                            return;
                        }
                        if (*Current == '\\') goto NEXT;
                        if (*Current == '\n')
                        {
                            DeSerializeState = DeSerializeState.StringEnter;
                            return;
                        }
                    }
                    ++Current;
                }
                while (true);
            }
            do
            {
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return;
                    }
                    if (*Current == '\\') goto NEXT;
                    if (*Current == '\n')
                    {
                        DeSerializeState = DeSerializeState.StringEnter;
                        return;
                    }
                }
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)Current) + 1)) == 0)
            {
                while (++Current != end && ((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) | *(((byte*)Current) + 1)) == 0) ;
                return;
            }
            DeSerializeState = DeSerializeState.NotNumber;
        }
        ///// <summary>
        ///// 忽略数组
        ///// </summary>
        //private void ignoreArray()
        //{
        //    if (++Current == end)
        //    {
        //        DeSerializeState = DeSerializeState.CrashEnd;
        //        return;
        //    }
        //    if (IsFirstArrayValue())
        //    {
        //        do
        //        {
        //            Ignore();
        //            if (DeSerializeState != DeSerializeState.Success) return;
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
            if (DeSerializeState != DeSerializeState.Success) return false;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
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
            if (DeSerializeState != DeSerializeState.Success) return false;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
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
            DeSerializeState = DeSerializeState.NotArrayValue;
            return false;
        }
        /// <summary>
        /// 忽略对象
        /// </summary>
        private void ignoreObject()
        {
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            if (IsFirstObject())
            {
                if (*Current == '\'' || *Current == '"') ignoreString();
                else ignoreName();
                if (DeSerializeState != DeSerializeState.Success || SearchColon() == 0) return;
                Ignore();
                while (DeSerializeState == DeSerializeState.Success && IsNextObject())
                {
                    if (*Current == '\'' || *Current == '"') ignoreString();
                    else ignoreName();
                    if (DeSerializeState != DeSerializeState.Success || SearchColon() == 0) return;
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
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            space();
            if (DeSerializeState != DeSerializeState.Success) return false;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return false;
            }
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
            if (*Current == '}')
            {
                ++Current;
                return false;
            }
            DeSerializeState = DeSerializeState.NotFoundName;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return false;
                }
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                space();
                if (DeSerializeState != DeSerializeState.Success) return false;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return false;
                }
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameStartBit) | *(((byte*)Current) + 1)) == 0) return true;
                DeSerializeState = DeSerializeState.NotFoundName;
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
                if (DeSerializeState != DeSerializeState.Success) return false;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return false;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotObject;
            return false;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private byte* isName(byte* names, ref int index)
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
            else if (AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)Current, names += sizeof(short), length) && (int)((byte*)end - (byte*)Current) >= length)
            {
                Current = (char*)((byte*)Current + length);
                return names + length;
            }
            return null;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* IsName(JsonDeSerializer jsonDeSerializer, byte* names, ref int index)
        {
            return jsonDeSerializer.isName(names, ref index);
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal delegate byte* IsNameDelegate(JsonDeSerializer jsonDeSerializer, byte* names, ref int index);
        /// <summary>
        /// 忽略成员名称
        /// </summary>
        private void ignoreName()
        {
            do
            {
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
            }
            while (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameBit) | *(((byte*)Current) + 1)) == 0);
        }
        /// <summary>
        /// 查找名称直到结束
        /// </summary>
        internal void SearchNameEnd()
        {
            if (DeSerializeState == DeSerializeState.Success)
            {
                while (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                }
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
            if (DeSerializeState != DeSerializeState.Success) return 0;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return 0;
            }
            if (*Current == ':')
            {
                ++Current;
                return 1;
            }
            DeSerializeState = DeSerializeState.NotFoundColon;
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
                if (DeSerializeState != DeSerializeState.Success) return false;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return false;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotObject;
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
                DeSerializeState = DeSerializeState.CrashEnd;
                return (char)0;
            }
            return ((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeNameBit) | *(((byte*)Current) + 1)) == 0 ? *Current : (char)0;
        }
        /// <summary>
        /// 读取下一个字符
        /// </summary>
        /// <returns>字符,结束或者错误返回0</returns>
        internal char NextStringChar()
        {
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return (char)0;
            }
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
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
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return (char)0;
                        }
                        return (char)deSerializeHex4();
                    }
                    if (*Current == 'x')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return (char)0;
                        }
                        return (char)deSerializeHex2();
                    }
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return (char)0;
                    }
                    return *Current < escapeCharSize ? escapeChars[*Current] : *Current;
                }
                if (*Current == '\n')
                {
                    DeSerializeState = DeSerializeState.StringEnter;
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
            if (Quote != 0 && DeSerializeState == DeSerializeState.Success)
            {
                //++Current;
                do
                {
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if (endChar == Quote)
                    {
                        do
                        {
                            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                            {
                                if (*Current == Quote)
                                {
                                    ++Current;
                                    return;
                                }
                                if (*Current == '\\') goto NEXT;
                                if (*Current == '\n')
                                {
                                    DeSerializeState = DeSerializeState.StringEnter;
                                    return;
                                }
                            }
                            ++Current;
                        }
                        while (true);
                    }
                    do
                    {
                        if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                        {
                            if (*Current == Quote)
                            {
                                ++Current;
                                return;
                            }
                            if (*Current == '\\') goto NEXT;
                            if (*Current == '\n')
                            {
                                DeSerializeState = DeSerializeState.StringEnter;
                                return;
                            }
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                    }
                    while (true);
                NEXT:
                    if (*++Current == 'u')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        Current += 5;
                    }
                    else if (*Current == 'x')
                    {
                        if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        Current += 3;
                    }
                    else
                    {
                        if (Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
        /// 获取数组长度
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool searchArraySize<valueType>(ref valueType[] array)
        {
            if (SearchArray(ref array) == 0)
            {
                int count = 1;
                char* read = Current;
                if (endChar == ']')
                {
                    do
                    {
                        if (*read == ',') ++count;
                    }
                    while (*++read != ']');
                }
                else
                {
                    do
                    {
                        if (*read == ',') ++count;
                        if (++read == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return false;
                        }
                    }
                    while (*read != ']');
                }
                array = new valueType[count];
                return true;
            }
            return false;
        }
        /// <summary>
        /// 查找数组起始位置
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">目标数组</param>
        /// <returns></returns>
        internal int SearchArray<valueType>(ref valueType[] value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '[')
            {
                if (*++Current == ']')
                {
                    ++Current;
                    value = EmptyArray<valueType>.Array;
                    return 1;
                }
                space();
                if (DeSerializeState != DeSerializeState.Success) return -1;
                if (Current >= end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return -1;
                }
                if (*Current == ']')
                {
                    ++Current;
                    value = EmptyArray<valueType>.Array;
                    return 1;
                }
                return 0;
            }
            if ((int)((byte*)end - (byte*)Current) >= 4 * sizeof(char) && *(long*)Current == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48))
            {
                value = null;
                Current += 4;
                return -1;
            }
            if (isSpace == 0)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return -1;
                if (Current != end)
                {
                    isSpace = 1;
                    goto START;
                }
            }
            DeSerializeState = DeSerializeState.CrashEnd;
            return -1;
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
            if (DeSerializeState != DeSerializeState.Success) return (char)0;
            if (++Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return (char)0;
            }
            if (*Current == '\'' || *Current == '"')
            {
                Quote = *Current;
                return NextStringChar();
            }
            if (isNull()) return Quote = (char)0;
            DeSerializeState = DeSerializeState.NotString;
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
                if (DeSerializeState != DeSerializeState.Success) return (char)0;
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return (char)0;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotEnumChar;
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
                DeSerializeState = DeSerializeState.CrashEnd;
                return (char)0;
            }
            if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
            {
                if (*Current == Quote)
                {
                    ++Current;
                    return Quote = (char)0;
                }
                if (*Current == '\\' || *Current == '\n')
                {
                    DeSerializeState = DeSerializeState.NotEnumChar;
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
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == Quote)
                    {
                        ++Current;
                        return Quote = (char)0;
                    }
                    if (*Current == '\\' || *Current == '\n')
                    {
                        DeSerializeState = DeSerializeState.NotEnumChar;
                        return (char)0;
                    }
                }
                else if (*Current == ',')
                {
                    do
                    {
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                        DeSerializeState = DeSerializeState.NotEnumChar;
                        return (char)0;
                    }
                    return *Current;
                }
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return (char)0;
                }
            }
            while (true);
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        internal bool IsEnumNumberUnsigned()
        {
            if ((uint)(*Current - '0') < 10) return true;
            space();
            if (DeSerializeState != DeSerializeState.Success) return false;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return false;
            }
            return (uint)(*Current - '0') < 10;
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        internal bool IsEnumNumberSigned()
        {
            if ((uint)(*Current - '0') < 10 || *Current == '-') return true;
            space();
            if (DeSerializeState != DeSerializeState.Success) return false;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
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
                if (DeSerializeState != DeSerializeState.Success) return 0;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return 0;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotObject;
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
            if (DeSerializeState != DeSerializeState.Success) return 1;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
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
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NotSupport<T>(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            DeSerializeState state = JsonSerializer.CustomConfig.NotSupport(jsonDeSerializer, ref value);
            if (state != DeSerializeState.Success) jsonDeSerializer.DeSerializeState = state;
        }

        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructDeSerialize<valueType>(JsonDeSerializer jsonDeSerializer, ref valueType value) where valueType : struct
        {
            TypeDeSerializer<valueType>.DeSerializeStruct(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void TypeDeSerialize<valueType>(JsonDeSerializer jsonDeSerializer, ref valueType value)
        {
            TypeDeSerializer<valueType>.DeSerializeClass(jsonDeSerializer, ref value);
        }

        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="values">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array<valueType>(JsonDeSerializer jsonDeSerializer, ref valueType[] values)
        {
            TypeDeSerializer<valueType>.Array(jsonDeSerializer, ref values);
        }
        /// <summary>
        /// 字典解析
        /// </summary>
        /// <param name="dictionary">目标数据</param>
        private void dictionary<valueType, dictionaryValueType>(ref Dictionary<valueType, dictionaryValueType> dictionary)
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
                        do
                        {
                            valueType key = default(valueType);
                            TypeDeSerializer<valueType>.DeSerialize(this, ref key);
                            if (DeSerializeState != DeSerializeState.Success || SearchColon() == 0) return;
                            dictionaryValueType value = default(dictionaryValueType);
                            TypeDeSerializer<dictionaryValueType>.DeSerialize(this, ref value);
                            if (DeSerializeState != DeSerializeState.Success) return;
                            dictionary.Add(key, value);
                        }
                        while (IsNextObject());
                    }
                }
                else if (IsFirstArrayValue())
                {
                    do
                    {
                        KeyValue<valueType, dictionaryValueType> value = default(KeyValue<valueType, dictionaryValueType>);
                        TypeDeSerializer<KeyValue<valueType, dictionaryValueType>>.DeSerializeValue(this, ref value);
                        if (DeSerializeState != DeSerializeState.Success) return;
                        dictionary.Add(value.Key, value.Value);
                    }
                    while (IsNextArrayValue());
                }
            }
        }
        /// <summary>
        /// 字典解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="dictionary">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Dictionary<valueType, dictionaryValueType>(JsonDeSerializer jsonDeSerializer, ref Dictionary<valueType, dictionaryValueType> dictionary)
        {
            jsonDeSerializer.dictionary<valueType, dictionaryValueType>(ref dictionary);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void nullableEnumDeSerialize<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            if (tryNull()) value = null;
            else
            {
                valueType newValue = value.HasValue ? value.Value : default(valueType);
                TypeDeSerializer<valueType>.DefaultDeSerializer(this, ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableEnumDeSerialize<valueType>(JsonDeSerializer jsonDeSerializer, ref Nullable<valueType> value) where valueType : struct
        {
            jsonDeSerializer.nullableEnumDeSerialize(ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void nullableDeSerialize<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            if (tryNull()) value = null;
            else if (DeSerializeState == DeSerializeState.Success)
            {
                valueType newValue = value.HasValue ? value.Value : default(valueType);
                TypeDeSerializer<valueType>.DeSerialize(this, ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableDeSerialize<valueType>(JsonDeSerializer jsonDeSerializer, ref Nullable<valueType> value) where valueType : struct
        {
            jsonDeSerializer.nullableDeSerialize(ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void keyValuePair<keyType, valueType>(ref KeyValuePair<keyType, valueType> value)
        {
            if (SearchObject())
            {
                KeyValue<keyType, valueType> keyValue = new KeyValue<keyType, valueType>(value.Key, value.Value);
                TypeDeSerializer<KeyValue<keyType, valueType>>.DeSerializeMembers(this, ref keyValue);
                value = new KeyValuePair<keyType, valueType>(keyValue.Key, keyValue.Value);
            }
            else value = new KeyValuePair<keyType, valueType>();
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void KeyValuePair<keyType, valueType>(JsonDeSerializer jsonDeSerializer, ref KeyValuePair<keyType, valueType> value)
        {
            jsonDeSerializer.keyValuePair<keyType, valueType>(ref value);
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        internal static void ListConstructor<valueType, argumentType>(JsonDeSerializer jsonDeSerializer, ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeDeSerializer<argumentType>.ArrayIndex(jsonDeSerializer, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.ListConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        internal static void CollectionConstructor<valueType, argumentType>(JsonDeSerializer jsonDeSerializer, ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeDeSerializer<argumentType>.ArrayIndex(jsonDeSerializer, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.CollectionConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        internal static void EnumerableConstructor<valueType, argumentType>(JsonDeSerializer jsonDeSerializer, ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeDeSerializer<argumentType>.ArrayIndex(jsonDeSerializer, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.EnumerableConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void arrayConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            TypeDeSerializer<argumentType>.Array(this, ref values);
            if (DeSerializeState == DeSerializeState.Success)
            {
                if (values == null) value = default(valueType);
                else value = Emit.ArrayConstructor<valueType, argumentType>.Constructor(values);
            }
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ArrayConstructor<valueType, argumentType>(JsonDeSerializer jsonDeSerializer, ref valueType value)
        {
            jsonDeSerializer.arrayConstructor<valueType, argumentType>(ref value);
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        private void dictionaryConstructor<dictionaryType, keyType, valueType>(ref dictionaryType value)
        {
            KeyValuePair<keyType, valueType>[] values = null;
            int count = TypeDeSerializer<KeyValuePair<keyType, valueType>>.ArrayIndex(this, ref values);
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
        /// 集合构造函数解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DictionaryConstructor<dictionaryType, keyType, valueType>(JsonDeSerializer jsonDeSerializer, ref dictionaryType value)
        {
            jsonDeSerializer.dictionaryConstructor<dictionaryType, keyType, valueType>(ref value);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <param name="value">目标数据</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void baseDeSerialize<valueType, childType>(ref childType value) where childType : valueType
        {
            if (value == null)
            {
                if (SearchObject())
                {
                    if (AutoCSer.Metadata.DefaultConstructor<childType>.Type == Metadata.DefaultConstructorType.None)
                    {
                        CheckNoConstructor(ref value, false);
                        if (value == null) return;
                    }
                    else value = AutoCSer.Metadata.DefaultConstructor<childType>.Constructor();
                    valueType newValue = value;
                    TypeDeSerializer<valueType>.DeSerializeMembers(this, ref newValue);
                }
            }
            else
            {
                valueType newValue = value;
                TypeDeSerializer<valueType>.DeSerializeClass(this, ref newValue);
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
        private bool deSerializeQuoteString(ref SubString value, int escapeIndex, char quote, int isTempString)
        {
            fixed (char* jsonFixed = value.GetFixedBuffer())
            {
                char* start = jsonFixed + value.Start;
                end = start + value.Length;
                Quote = quote;
                Current = start + escapeIndex;
                endChar = *end++;
                if (isTempString == 0)
                {
                    string newValue = deSerializeEscape(start);
                    if (newValue != null)
                    {
                        value.Set(newValue, 0, newValue.Length);
                        return true;
                    }
                }
                else
                {
                    char* writeEnd = deSerializeEscape();
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
        internal static bool DeSerializeQuoteString(ref SubString value, int escapeIndex, char quote, int isTempString)
        {
            JsonDeSerializer deSerializer = YieldPool.Default.Pop() ?? new JsonDeSerializer();
            try
            {
                return deSerializer.deSerializeQuoteString(ref value, escapeIndex, quote, isTempString);
            }
            finally { YieldPool.Default.Push(deSerializer); }
        }
        /// <summary>
        /// 字符串转义解析
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charStream"></param>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <returns>解析是否成功</returns>
        internal bool DeSerializeQuoteString(ref SubString value, CharStream charStream, int escapeIndex, char quote)
        {
            fixed (char* jsonFixed = value.GetFixedBuffer())
            {
                char* start = jsonFixed + value.Start;
                end = start + value.Length;
                Quote = quote;
                Current = start + escapeIndex;
                endChar = *end++;

                int size = deSerializeEscapeSize();
                if (size != 0)
                {
                    int left = (int)(Current - start);
                    char* valueFixed = (char*)charStream.GetBeforeMove((size += left) * sizeof(char));
                    if (left != 0) new Span<char>(start, left).CopyTo(new Span<char>(valueFixed, left));
                    deSerializeEscapeUnsafe(valueFixed + left);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T DeSerialize<T>(string json, DeSerializeConfig config = null)
        {
            T value = AutoCSer.Common.GetDefault<T>();
            return DeSerialize(json, ref value, config).State == DeSerializeState.Success ? value : AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        public static DeSerializeResult DeSerialize<T>(string json, ref T value, DeSerializeConfig config = null)
        {
            if (string.IsNullOrEmpty(json)) return new DeSerializeResult { State = DeSerializeState.NullJson };
            JsonDeSerializer jsonDeSerializer = YieldPool.Default.Pop() ?? new JsonDeSerializer();
            try
            {
                return jsonDeSerializer.deSerialize(json, ref value, config);
            }
            finally { jsonDeSerializer.Free(); }
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T DeSerialize<T>(SubString json, DeSerializeConfig config = null)
        {
            T value = AutoCSer.Common.GetDefault<T>();
            return DeSerialize(ref json, ref value, config).State == DeSerializeState.Success ? value : AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult DeSerialize<T>(SubString json, ref T value, DeSerializeConfig config = null)
        {
            return DeSerialize(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T DeSerialize<T>(ref SubString json, DeSerializeConfig config = null)
        {
            T value = AutoCSer.Common.GetDefault<T>();
            return DeSerialize(ref json, ref value, config).State == DeSerializeState.Success ? value : AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        public static DeSerializeResult DeSerialize<T>(ref SubString json, ref T value, DeSerializeConfig config = null)
        {
            if (string.IsNullOrEmpty(json)) return new DeSerializeResult { State = DeSerializeState.NullJson };
            JsonDeSerializer jsonDeSerializer = YieldPool.Default.Pop() ?? new JsonDeSerializer();
            try
            {
                return jsonDeSerializer.deSerialize(ref json, ref value, config);
            }
            finally { jsonDeSerializer.Free(); }
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">Json 字符串</param>
        /// <param name="length">Json 长度</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DeSerializeResult UnsafeDeSerialize<valueType>(char* json, int length, ref valueType value)
        {
            JsonDeSerializer deSerializer = YieldPool.Default.Pop() ?? new JsonDeSerializer();
            try
            {
                return deSerializer.deSerialize<valueType>(json, length, ref value);//, config, buffer
            }
            finally { deSerializer.Free(); }
        }

        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T ThreadStaticDeSerialize<T>(string json, DeSerializeConfig config = null)
        {
            T value = AutoCSer.Common.GetDefault<T>();
            return ThreadStaticDeSerialize(json, ref value, config).State == DeSerializeState.Success ? value : AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        public static DeSerializeResult ThreadStaticDeSerialize<T>(string json, ref T value, DeSerializeConfig config = null)
        {
            if (string.IsNullOrEmpty(json)) return new DeSerializeResult { State = DeSerializeState.NullJson };
            JsonDeSerializer jsonDeSerializer = ThreadStaticDeSerializer.Get().DeSerializer;
            try
            {
                return jsonDeSerializer.deSerialize(json, ref value, config);
            }
            finally { jsonDeSerializer.freeThreadStatic(); }
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T ThreadStaticDeSerialize<T>(SubString json, DeSerializeConfig config = null)
        {
            T value = AutoCSer.Common.GetDefault<T>();
            return ThreadStaticDeSerialize(ref json, ref value, config).State == DeSerializeState.Success ? value : AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult ThreadStaticDeSerialize<T>(SubString json, ref T value, DeSerializeConfig config = null)
        {
            return ThreadStaticDeSerialize(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>目标数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T ThreadStaticDeSerialize<T>(ref SubString json, DeSerializeConfig config = null)
        {
            T value = AutoCSer.Common.GetDefault<T>();
            return ThreadStaticDeSerialize(ref json, ref value, config).State == DeSerializeState.Success ? value : AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// JSON 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>反序列化状态</returns>
        public static DeSerializeResult ThreadStaticDeSerialize<T>(ref SubString json, ref T value, DeSerializeConfig config = null)
        {
            if (string.IsNullOrEmpty(json)) return new DeSerializeResult { State = DeSerializeState.NullJson };
            JsonDeSerializer jsonDeSerializer = ThreadStaticDeSerializer.Get().DeSerializer;
            try
            {
                return jsonDeSerializer.deSerialize(ref json, ref value, config);
            }
            finally { jsonDeSerializer.freeThreadStatic(); }
        }

        /// <summary>
        /// 解析委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        internal delegate void DeSerializeDelegate<T>(JsonDeSerializer deSerializer, ref T value);

        /// <summary>
        /// 转义字符集合尺寸
        /// </summary>
        private const int escapeCharSize = 128;
        /// <summary>
        /// 转义字符集合
        /// </summary>
        private static AutoCSer.Memory.Pointer escapeCharData;
        /// <summary>
        /// JSON 解析数字
        /// </summary>
        internal const byte DeSerializeNumberBit = 16;
        /// <summary>
        /// JSON 解析键值
        /// </summary>
        internal const byte DeSerializeNameBit = 32;
        /// <summary>
        /// JSON 解析键值开始
        /// </summary>
        internal const byte DeSerializeNameStartBit = 64;
        /// <summary>
        /// JSON 解析空格[ ,\t,\r,\n,160]
        /// </summary>
        internal const byte DeSerializeSpaceBit = 128;
        /// <summary>
        /// JSON 解析转义查找
        /// </summary>
        internal const byte DeSerializeEscapeSearchBit = 8;
        /// <summary>
        /// Javascript 转义位[\r,\n,\\,"]
        /// </summary>
        internal const byte EscapeBit = 4;
        /// <summary>
        /// JSON 解析字符状态位
        /// </summary>
        internal static AutoCSer.Memory.Pointer DeSerializeBits;

        static JsonDeSerializer()
        {
            JavascriptLocalMinTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddTicks(Date.LocalTimeTicks);
            JavascriptLocalMinTimeTicks = JavascriptLocalMinTime.Ticks;

            DeSerializeBits = Unmanaged.GetStaticPointer8(256 + (escapeCharSize * sizeof(char)), false);
            byte* bits = DeSerializeBits.Byte;
            new Span<ulong>(bits, 256 >> 3).Fill(ulong.MaxValue);
            for (char value = '0'; value <= '9'; ++value) bits[value] &= (DeSerializeNumberBit | DeSerializeNameBit) ^ 255;
            for (char value = 'A'; value <= 'F'; ++value) bits[value] &= (DeSerializeNameBit | DeSerializeNameStartBit | DeSerializeNumberBit) ^ 255;
            for (char value = 'a'; value <= 'f'; ++value) bits[value] &= (DeSerializeNameBit | DeSerializeNameStartBit | DeSerializeNumberBit) ^ 255;
            for (char value = 'G'; value <= 'Z'; ++value) bits[value] &= (DeSerializeNameBit | DeSerializeNameStartBit) ^ 255;
            for (char value = 'g'; value <= 'z'; ++value) bits[value] &= (DeSerializeNameBit | DeSerializeNameStartBit) ^ 255;
            bits['\t'] &= (DeSerializeSpaceBit | EscapeBit) ^ 255;
            bits['\r'] &= (DeSerializeSpaceBit | EscapeBit) ^ 255;
            bits['\n'] &= (DeSerializeSpaceBit | DeSerializeEscapeSearchBit | EscapeBit) ^ 255;
            bits[' '] &= DeSerializeSpaceBit ^ 255;
            bits[0xA0] &= DeSerializeSpaceBit ^ 255;
            bits['x'] &= DeSerializeNumberBit ^ 255;
            bits['+'] &= DeSerializeNumberBit ^ 255;
            bits['-'] &= DeSerializeNumberBit ^ 255;
            bits['.'] &= DeSerializeNumberBit ^ 255;
            bits['_'] &= (DeSerializeNameBit | DeSerializeNameStartBit) ^ 255;
            bits['\''] &= (DeSerializeNameStartBit | DeSerializeEscapeSearchBit) ^ 255;
            bits['"'] &= (DeSerializeNameStartBit | DeSerializeEscapeSearchBit | EscapeBit) ^ 255;
            bits['\\'] &= (DeSerializeEscapeSearchBit | EscapeBit) ^ 255;
            bits['\f'] &= EscapeBit ^ 255;
            bits['\b'] &= EscapeBit ^ 255;

            char* escapeCharDataChar = (char*)(bits + 256);
            escapeCharData = new AutoCSer.Memory.Pointer { Data = escapeCharDataChar };
            for (int value = 0; value != escapeCharSize; ++value) escapeCharDataChar[value] = (char)value;
            escapeCharDataChar['0'] = ' ';
            escapeCharDataChar['B'] = escapeCharDataChar['b'] = '\b';
            escapeCharDataChar['F'] = escapeCharDataChar['f'] = '\f';
            escapeCharDataChar['N'] = escapeCharDataChar['n'] = '\n';
            escapeCharDataChar['R'] = escapeCharDataChar['r'] = '\r';
            escapeCharDataChar['T'] = escapeCharDataChar['t'] = '\t';
            escapeCharDataChar['V'] = escapeCharDataChar['v'] = '\v';

            deSerializeMethods = AutoCSer.DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(JsonDeSerializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(DeSerializeMethod), false))
                {
                    deSerializeMethods.Add(method.GetParameters()[0].ParameterType.GetElementType(), method);
                }
            }
        }
    }
}
