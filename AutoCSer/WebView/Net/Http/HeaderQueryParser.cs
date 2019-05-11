using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Threading;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 查询解析器
    /// </summary>
    internal unsafe sealed class HeaderQueryParser : AutoCSer.Threading.Link<HeaderQueryParser>
    {
        /// <summary>
        /// HTTP请求头部
        /// </summary>
        private Header header;
        /// <summary>
        /// 解析状态
        /// </summary>
        private HeaderQueryParseState state;
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        private byte* bufferStart;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        private byte* current;
        /// <summary>
        /// 解析结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 当前处理位置
        /// </summary>
        private BufferIndex* queryIndex;
        /// <summary>
        /// 最后处理位置
        /// </summary>
        private BufferIndex* queryEndIndex;
        /// <summary>
        /// 查询解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="header">HTTP请求头部</param>
        /// <param name="value">目标数据</param>
        /// <returns>解析状态</returns>
        private HeaderQueryParseState parse<valueType>(Header header, ref valueType value)
        {
            this.header = header;
            state = HeaderQueryParseState.Success;
            fixed (byte* bufferFixed = header.Buffer.Buffer)
            {
                bufferStart = bufferFixed + header.Buffer.StartIndex;
                queryIndex = (BufferIndex*)(bufferStart + Header.QueryStartIndex);
                queryEndIndex = queryIndex + (header.QueryCount << 1);
                queryIndex -= 2;
                HeaderQueryTypeParser<valueType>.Parse(this, ref value);
            }
            return state;
        }
        /// <summary>
        /// 释放查询解析器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void free()
        {
            header = null;
            YieldPool.Default.PushNotNull(this);
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        private void parseHex32(ref uint value)
        {
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    state = HeaderQueryParseState.NotHex;
                    return;
                }
                number += 10;
            }
            value = number;
            if (++current == end) return;
            do
            {
                if ((number = (uint)(*current - '0')) > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return;
                    number += 10;
                }
                value <<= 4;
                value += number;
            }
            while (++current != end);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private uint parseUInt32(uint value)
        {
            uint number;
            do
            {
                if ((number = (uint)(*current - '0')) > 9) return value;
                value *= 10;
                value += number;
                if (++current == end) return value;
            }
            while (true);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>解析状态</returns>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref bool value)
        {
            BufferIndex* indexs = queryIndex + 1;
            switch (indexs->Length)
            {
                case 0:
                    value = false;
                    return;
                case 4:
                    current = bufferStart + indexs->StartIndex;
                    if (*(int*)current == ('t' + ('r' << 8) + ('u' << 16) + ('e' << 24))) value = true;
                    else state = HeaderQueryParseState.NotBool;
                    return;
                case 5:
                    current = bufferStart + indexs->StartIndex;
                    if ((*current | 0x20) == 'f' && *(int*)(current + 1) == ('a' + ('l' << 8) + ('s' << 16) + ('e' << 24))) value = false;
                    else state = HeaderQueryParseState.NotBool;
                    return;
                default:
                    byte byteValue = (byte)(*(bufferStart + indexs->StartIndex) - '0');
                    if (byteValue < 10) value = byteValue != 0;
                    else state = HeaderQueryParseState.NotBool;
                    return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref byte value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = (byte)number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref sbyte value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            int sign = 0;
            if (*current == '-')
            {
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
                    return;
                }
                sign = 1;
            }
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref ushort value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = (ushort)number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref short value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            int sign = 0;
            if (*current == '-')
            {
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
                    return;
                }
                sign = 1;
            }
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref uint value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref int value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            int sign = 0;
            if (*current == '-')
            {
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
                    return;
                }
                sign = 1;
            }
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
                    return;
                }
                parseHex32(ref number);
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            value = sign == 0 ? (int)parseUInt32(number) : -(int)parseUInt32(number);
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <returns>数字</returns>
        private ulong parseHex64()
        {
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                {
                    state = HeaderQueryParseState.NotHex;
                    return 0;
                }
                number += 10;
            }
            if (++current == end) return number;
            uint high = number;
            byte* end32 = current + 7;
            if (end32 > end) end32 = end;
            do
            {
                if ((number = (uint)(*current - '0')) > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5) return high;
                    number += 10;
                }
                high <<= 4;
                high += number;
            }
            while (++current != end32);
            if (current == end) return high;
            byte* start = current;
            ulong low = number;
            do
            {
                if ((number = (uint)(*current - '0')) > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                    {
                        return low | (ulong)high << ((int)((byte*)current - (byte*)start) << 1);
                    }
                    number += 10;
                }
                low <<= 4;
                low += number;
            }
            while (++current != end);
            return low | (ulong)high << ((int)((byte*)current - (byte*)start) << 1);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        private ulong parseUInt64(uint value)
        {
            byte* end32 = current + 8;
            if (end32 > end) end32 = end;
            uint number;
            do
            {
                if ((number = (uint)(*current - '0')) > 9) return value;
                value *= 10;
                value += number;
            }
            while (++current != end32);
            if (current == end) return value;
            ulong value64 = value;
            do
            {
                if ((number = (uint)(*current - '0')) > 9) return value64;
                value64 *= 10;
                value64 += number;
                if (++current == end) return value64;
            }
            while (true);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref ulong value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref long value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0)
            {
                value = 0;
                return;
            }
            current = bufferStart + indexs->StartIndex;
            end = current + indexs->Length;
            int sign = 0;
            if (*current == '-')
            {
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
                    return;
                }
                sign = 1;
            }
            uint number = (uint)(*current - '0');
            if (number > 9)
            {
                state = HeaderQueryParseState.NotNumber;
                return;
            }
            if (++current == end)
            {
                value = sign == 0 ? (long)(int)number : -(long)(int)number;
                return;
            }
            if (number == 0)
            {
                if (*current != 'x')
                {
                    value = 0;
                    return;
                }
                if (++current == end)
                {
                    state = HeaderQueryParseState.NotNumber;
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
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <returns>unescape解码后的字符串</returns>
        private string unescapeAscii()
        {
            byte* start = current;
            while (*start != '%')
            {
                if (++start == end) return AutoCSer.Memory.ToString(current, (int)(end - current));
            }
            byte* write = start;
        NEXT:
            if (*++start == 'u')
            {
                uint code = (uint)(*++start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code <<= 12;
                code += (number << 8);
                if ((number = (uint)(*++start - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code += (number << 4);
                number = (uint)(*++start - '0');
                code += (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
                *write++ = (byte)code;
            }
            else
            {
                uint code = (uint)(*start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                *write++ = (byte)code;
            }
            while (++start < end)
            {
                if (*start == '%') goto NEXT;
                *write++ = *start;
            }
            return AutoCSer.Memory.ToString(current, (int)(write - current));
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref float value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = 0;
            else
            {
                current = bufferStart + indexs->StartIndex;
                end = current + indexs->Length;
                if (!float.TryParse(unescapeAscii(), out value)) state = HeaderQueryParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref double value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = 0;
            else
            {
                current = bufferStart + indexs->StartIndex;
                end = current + indexs->Length;
                if (!double.TryParse(unescapeAscii(), out value)) state = HeaderQueryParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref decimal value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = 0;
            else
            {
                current = bufferStart + indexs->StartIndex;
                end = current + indexs->Length;
                if (!decimal.TryParse(unescapeAscii(), out value)) state = HeaderQueryParseState.NotNumber;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref DateTime value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = DateTime.MinValue;
            else
            {
                current = bufferStart + indexs->StartIndex;
                end = current + indexs->Length;
                if (!DateTime.TryParse(unescapeAscii(), out value)) state = HeaderQueryParseState.NotDateTime;
            }
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private uint parseHex2()
        {
            uint code = (uint)(*++current - '0'), number = (uint)(*++current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            return (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        private uint parseHex4()
        {
            uint code = (uint)(*++current - '0'), number = (uint)(*++current - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code <<= 12;
            code += (number << 8);
            if ((number = (uint)(*++current - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code += (number << 4);
            number = (uint)(*++current - '0');
            return code + (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref Guid value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = new Guid();
            else if (end - current != 36) state = HeaderQueryParseState.NotGuid;
            else
            {
                current = bufferStart + indexs->StartIndex;
                end = current + indexs->Length;
                GuidCreator guid = new GuidCreator();
                guid.Byte3 = (byte)parseHex2();
                guid.Byte2 = (byte)parseHex2();
                guid.Byte1 = (byte)parseHex2();
                guid.Byte0 = (byte)parseHex2();
                if (*++current != '-')
                {
                    state = HeaderQueryParseState.NotGuid;
                    return;
                }
                guid.Byte45 = (ushort)parseHex4();
                if (*++current != '-')
                {
                    state = HeaderQueryParseState.NotGuid;
                    return;
                }
                guid.Byte67 = (ushort)parseHex4();
                if (*++current != '-')
                {
                    state = HeaderQueryParseState.NotGuid;
                    return;
                }
                guid.Byte8 = (byte)parseHex2();
                guid.Byte9 = (byte)parseHex2();
                if (*++current != '-')
                {
                    state = HeaderQueryParseState.NotGuid;
                    return;
                }
                guid.Byte10 = (byte)parseHex2();
                guid.Byte11 = (byte)parseHex2();
                guid.Byte12 = (byte)parseHex2();
                guid.Byte13 = (byte)parseHex2();
                guid.Byte14 = (byte)parseHex2();
                guid.Byte15 = (byte)parseHex2();
                value = guid.Value;
            }
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <returns>unescape解码后的字符串</returns>
        private string unescapeUtf8()
        {
            byte* start = current, escape = null;
            uint escapeCode = 0, unicode = 0;
            do
            {
                if ((*start & 0x80) == 0)
                {
                    if (*start == '%')
                    {
                        if (escape == null) escape = start;
                        if (*++start == 'u')
                        {
                            unicode = 1;
                            break;
                        }
                        uint code = (uint)(*start++ - '0');
                        escapeCode |= code > 9 ? 8 : code;
                    }
                }
                else escapeCode = 8;
            }
            while (++start < end);
            if (unicode != 0 || (escapeCode & 8) == 0)
            {
                if (escape == null) AutoCSer.Memory.ToString(current, (int)(end - current));
                int length = (int)(++escape - current);
                for (start = escape + (*escape == 'u' ? 5 : 2); start < end; ++length)
                {
                    if (*start == '%') start += *(start + 1) == 'u' ? 6 : 3;
                    else ++start;
                }
                string value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                fixed (char* valueFixed = value)
                {
                    start = current;
                    char* write = valueFixed, writeEnd = valueFixed + length;
                    do
                    {
                        if (*start == '%')
                        {
                            if (*++start == 'u')
                            {
                                uint code = (uint)(*++start - '0'), number = (uint)(*++start - '0');
                                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                                if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                                code <<= 12;
                                code += (number << 8);
                                if ((number = (uint)(*++start - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                                code += (number << 4);
                                number = (uint)(*++start - '0');
                                code += (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
                                *write++ = (char)code;
                            }
                            else
                            {
                                uint code = (uint)(*start - '0'), number = (uint)(*++start - '0');
                                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                                *write++ = (char)code;
                            }
                        }
                        else *write = (char)*start;
                        ++start;
                    }
                    while (++write != writeEnd);
                }
                return value;
            }
            if (escape != null)
            {
                byte* write = escape;
            NEXT:
                unicode = (uint)(*++escape - '0');
                escapeCode = (uint)(*++escape - '0');
                if (unicode > 9) unicode = ((unicode - ('A' - '0')) & 0xffdfU) + 10;
                unicode = (escapeCode > 9 ? (((escapeCode - ('A' - '0')) & 0xffdfU) + 10) : escapeCode) + (unicode << 4);
                *write++ = (byte)unicode;
                while (++escape < end)
                {
                    if (*escape == '%') goto NEXT;
                    *write++ = *escape;
                }
                end = write;
            }
            return Encoding.UTF8.GetString(header.Buffer.Buffer, header.Buffer.StartIndex + (int)(current - bufferStart), (int)(end - current));
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref string value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = string.Empty;
            else value = header.UnescapeUtf8(bufferStart, indexs->StartIndex, indexs->Length);
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [HeaderQueryParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Parse(ref SubString value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value.Set(string.Empty, 0, 0);
            else value = header.UnescapeUtf8(bufferStart, indexs->StartIndex, indexs->Length);
        }

        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal byte* IsName(byte* names, ref int index)
        {
            if ((queryIndex += 2) == queryEndIndex)
            {
                index = -1;
                return names;
            }
            int length = *(short*)names;
            if (queryIndex->Length == (short)length && Memory.SimpleEqualNotNull(bufferStart + queryIndex->StartIndex, names += sizeof(short), length))
            {
                return names + length;
            }
            current = bufferStart + queryIndex->StartIndex;
            end = current + queryIndex->Length;
            return null;
        }
        /// <summary>
        /// 是否存在未结束的查询
        /// </summary>
        /// <returns>是否存在未结束的查询</returns>
        internal bool IsQuery()
        {
            if ((queryIndex += 2) == queryEndIndex) return false;
            current = bufferStart + queryIndex->StartIndex;
            end = current + queryIndex->Length;
            return true;
        }
        /// <summary>
        /// 获取当前名称字符
        /// </summary>
        /// <returns>当前名称字符,结束返回0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte GetName()
        {
            return current == end ? (byte)0 : *current++;
        }

        /// <summary>
        /// 枚举 JSON 解析器
        /// </summary>
        private static Json.Parser enumJsonParser;
        /// <summary>
        /// 枚举类型解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal unsafe void parseEnum<valueType>(ref valueType value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = default(valueType);
            else
            {
                current = bufferStart + indexs->StartIndex;
                *(current + indexs->Length) = *(current - 1) = (byte)'"';
                Json.Parser parser = Interlocked.Exchange(ref enumJsonParser, null);
                if (parser == null)
                {
                    parser = Json.Parser.YieldPool.Default.Pop() ?? new Json.Parser();
                    parser.SetEnum();
                }
                if (!parser.ParseEnum(header.UnescapeUtf8(bufferStart, indexs->StartIndex - 1, indexs->Length + 2), ref value)) state = HeaderQueryParseState.Unknown;
                if ((parser = Interlocked.Exchange(ref enumJsonParser, parser)) != null) parser.Free();
            }
        }
        ///// <summary>
        ///// 未知类型解析函数信息
        ///// </summary>
        //private static readonly MethodInfo parseEnumMethod = typeof(HeaderQueryParser).GetMethod("parseEnum", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 未知类型解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void unknown<valueType>(ref valueType value)
        {
            BufferIndex* indexs = queryIndex + 1;
            if (indexs->Length == 0) value = default(valueType);
            else if (!AutoCSer.Json.Parser.ParseNotEmpty(header.UnescapeUtf8(bufferStart, indexs->StartIndex, indexs->Length), ref value)) state = HeaderQueryParseState.Unknown;
        }
        ///// <summary>
        ///// 未知类型解析函数信息
        ///// </summary>
        //private static readonly MethodInfo unknownMethod = typeof(HeaderQueryParser).GetMethod("unknown", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 查询解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="header">HTTP请求头部</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool Parse<valueType>(Header header, ref valueType value)
        {
            HeaderQueryParser parser = YieldPool.Default.Pop() ?? new HeaderQueryParser();
            try
            {
                return parser.parse<valueType>(header, ref value) == HeaderQueryParseState.Success;
            }
            finally { parser.free(); }
        }

        /// <summary>
        /// 获取解析函数信息
        /// </summary>
        /// <param name="memberType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetParseMemberMethod(Type memberType)
        {
            //return getParseMethod(memberType) ?? ((memberType.IsEnum ? parseEnumMethod : unknownMethod).MakeGenericMethod(memberType));
            return getParseMethod(memberType) ?? (memberType.IsEnum ? AutoCSer.WebView.Metadata.GenericType.Get(memberType).HttpHeaderQueryParseEnumMethod : AutoCSer.WebView.Metadata.GenericType.Get(memberType).HttpHeaderQueryParseUnknownMethod);
        }
#if !NOJIT
        /// <summary>
        /// 创建解析委托函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">成员名称</param>
        /// <param name="memberType">成员类型</param>
        /// <param name="generator"></param>
        /// <returns>解析委托函数</returns>
        internal static DynamicMethod CreateDynamicMethod(Type type, string name, Type memberType, out ILGenerator generator)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("HttpHeaderQueryParser" + name, null, new Type[] { typeof(HeaderQueryParser), type.MakeByRefType() }, type, true);
            generator = dynamicMethod.GetILGenerator();
            LocalBuilder loadMember = generator.DeclareLocal(memberType);
            generator.DeclareLocal(memberType);
            generator.initobjShort(memberType, loadMember);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloca_S, loadMember);
            generator.call(GetParseMemberMethod(memberType));

            generator.Emit(OpCodes.Ldarg_1);
            if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldloc_0);
            return dynamicMethod;
        }
#endif
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
        private static MethodInfo getParseMethod(Type type)
        {
            MethodInfo method;
            return parseMethods.TryGetValue(type, out method) ? method : null;
        }

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) RuntimeHelpers.RunClassConstructor(typeof(HeaderQueryTypeParser<>).MakeGenericType(type).TypeHandle);
            }
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            if (count == 0) enumJsonParser = null;
        }
        static HeaderQueryParser()
        {
            parseMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(HeaderQueryParser).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(HeaderQueryParseMethodAttribute), false))
                {
                    parseMethods.Add(method.GetParameters()[0].ParameterType.GetElementType(), method);
                }
            }
            Pub.ClearCaches += clearCache;
        }
    }
}
