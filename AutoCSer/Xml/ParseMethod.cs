using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 解析类型
    /// </summary>
    internal sealed class ParseMethod : Attribute { }

    /// <summary>
    /// XML 解析器
    /// </summary>
    public unsafe sealed partial class Parser
    {
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref bool value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((*current | 0x20) == 'f')
                {
                    if ((*(long*)(current + 1) | 0x20002000200020L) == ('a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48)))
                    {
                        current += 5;
                        value = false;
                        SearchValueEnd();
                        return;
                    }
                }
                else
                {
                    if ((*(long*)current | 0x20002000200020L) == ('t' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48)))
                    {
                        current += 4;
                        value = true;
                        SearchValueEnd();
                        return;
                    }
                    if ((uint)(*current - '0') < 2)
                    {
                        value = *current++ != '0';
                        SearchValueEnd();
                        return;
                    }
                }
                State = ParseState.NotBool;
            }
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref bool? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((*current | 0x20) == 'f')
                {
                    if ((*(long*)(current + 1) | 0x0020002000200020L) == ('a' + ('l' << 16) + ((long)'s' << 32) + ((long)'e' << 48)))
                    {
                        current += 5;
                        value = false;
                        SearchValueEnd();
                        return;
                    }
                }
                else
                {
                    if ((*(long*)(current) | 0x0020002000200020L) == ('t' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48)))
                    {
                        current += 4;
                        value = true;
                        SearchValueEnd();
                        return;
                    }
                    if ((uint)(*current - '0') < 2)
                    {
                        value = *current++ != '0';
                        SearchValueEnd();
                        return;
                    }
                    if (*current == '<')
                    {
                        value = null;
                        return;
                    }
                }
                State = ParseState.NotBool;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref byte value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref byte? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = (byte)number;
                        }
                        else value = 0;
                    }
                    else value = (byte)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref sbyte value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref sbyte? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = sign == '-' ? (sbyte)-(int)number : (sbyte)(byte)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (sbyte)-(int)parseUInt32(number) : (sbyte)(byte)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ushort value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ushort? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = (ushort)number;
                        }
                        else value = 0;
                    }
                    else value = (ushort)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref short value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref short? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = sign == '-' ? (short)-(int)number : (short)(ushort)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (short)-(int)parseUInt32(number) : (short)(ushort)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref uint value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref uint? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = number;
                        }
                        else value = 0;
                    }
                    else value = parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref int value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref int? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = sign == '-' ? -(int)number : (int)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(int)parseUInt32(number) : (int)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ulong value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref ulong? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            value = parseHex64();
                            if (State != ParseState.Success) return;
                        }
                        else value = 0;
                    }
                    else value = parseUInt64(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref long value)
        {
            searchValue();
            ParseNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref long? value)
        {
            searchValue();
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            value = sign == '-' ? -(long)parseHex64() : (long)parseHex64();
                            if (State != ParseState.Success) return;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(long)parseUInt64(number) : (long)parseUInt64(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref float value)
        {
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize != 0)
                {
                    switch (valueSize)
                    {
                        case 3:
                            if (isNaN())
                            {
                                value = float.NaN;
                                return;
                            }
                            break;
                        case 8:
                            if (isPositiveInfinity())
                            {
                                value = float.PositiveInfinity;
                                return;
                            }
                            break;
                        case 9:
                            if (isNegativeInfinity())
                            {
                                value = float.NegativeInfinity;
                                return;
                            }
                            break;
                    }
                    string number = new string(valueStart, 0, valueSize);
                    if (float.TryParse(number, out value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = ParseState.NotNumber;
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
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize == 0)
                {
                    value = null;
                    getValueEnd();
                }
                else
                {
                    switch (valueSize)
                    {
                        case 3:
                            if (isNaN())
                            {
                                value = float.NaN;
                                return;
                            }
                            break;
                        case 8:
                            if (isPositiveInfinity())
                            {
                                value = float.PositiveInfinity;
                                return;
                            }
                            break;
                        case 9:
                            if (isNegativeInfinity())
                            {
                                value = float.NegativeInfinity;
                                return;
                            }
                            break;
                    }
                    string numberString = new string(valueStart, 0, valueSize);
                    float number;
                    if (float.TryParse(numberString, out number))
                    {
                        value = number;
                        getValueEnd();
                    }
                    else State = ParseState.NotNumber;
                }
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
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize != 0)
                {
                    switch (valueSize)
                    {
                        case 3:
                            if (isNaN())
                            {
                                value = double.NaN;
                                return;
                            }
                            break;
                        case 8:
                            if (isPositiveInfinity())
                            {
                                value = double.PositiveInfinity;
                                return;
                            }
                            break;
                        case 9:
                            if (isNegativeInfinity())
                            {
                                value = double.NegativeInfinity;
                                return;
                            }
                            break;
                    }
                    string number = new string(valueStart, 0, valueSize);
                    if (double.TryParse(number, out value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = ParseState.NotNumber;
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
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize == 0)
                {
                    value = null;
                    getValueEnd();
                }
                else
                {
                    switch (valueSize)
                    {
                        case 3:
                            if (isNaN())
                            {
                                value = double.NaN;
                                return;
                            }
                            break;
                        case 8:
                            if (isPositiveInfinity())
                            {
                                value = double.PositiveInfinity;
                                return;
                            }
                            break;
                        case 9:
                            if (isNegativeInfinity())
                            {
                                value = double.NegativeInfinity;
                                return;
                            }
                            break;
                    }
                    string numberString = new string(valueStart, 0, valueSize);
                    double number;
                    if (double.TryParse(numberString, out number))
                    {
                        value = number;
                        getValueEnd();
                    }
                    else State = ParseState.NotNumber;
                }
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
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize != 0)
                {
                    string number = new string(valueStart, 0, valueSize);
                    if (decimal.TryParse(number, out value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = ParseState.NotNumber;
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
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize == 0)
                {
                    value = null;
                    getValueEnd();
                }
                else
                {
                    string numberString = new string(valueStart, 0, valueSize);
                    decimal number;
                    if (decimal.TryParse(numberString, out number))
                    {
                        value = number;
                        getValueEnd();
                    }
                    else State = ParseState.NotNumber;
                }
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
            getValue();
            if (valueSize == 1)
            {
                value = *valueStart;
                getValueEnd();
                return;
            }
            if ((IsCData | (*valueStart ^ '&')) == 0)
            {
                decodeChar(ref value);
                if (State == ParseState.Success) getValueEnd();
                return;
            }
            State = ParseState.NotChar;
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref char? value)
        {
            getValue();
            if (State == ParseState.Success)
            {
                switch (valueSize)
                {
                    case 0:
                        value = null;
                        getValueEnd();
                        return;
                    case 1:
                        value = *valueStart;
                        getValueEnd();
                        return;
                    default:
                        if ((IsCData | (*valueStart ^ '&')) == 0)
                        {
                            char charValue = (char)0;
                            decodeChar(ref charValue);
                            if (State == ParseState.Success)
                            {
                                value = charValue;
                                getValueEnd();
                            }
                            return;
                        }
                        break;
                }
                State = ParseState.NotChar;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref DateTime value)
        {
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize != 0)
                {
                    string number = new string(valueStart, 0, valueSize);
                    if (DateTime.TryParse(number, out value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = ParseState.NotDateTime;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref DateTime? value)
        {
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize == 0)
                {
                    value = null;
                    getValueEnd();
                }
                else
                {
                    string numberString = new string(valueStart, 0, valueSize);
                    DateTime number;
                    if (DateTime.TryParse(numberString, out number))
                    {
                        value = number;
                        getValueEnd();
                    }
                    else State = ParseState.NotDateTime;
                }
            }
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref Guid value)
        {
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize == 36)
                {
                    GuidCreator guid = new GuidCreator();
                    parse(ref guid);
                    value = guid.Value;
                    getValueEnd();
                }
                else State = ParseState.NotGuid;
            }
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref Guid? value)
        {
            getValue();
            if (State == ParseState.Success)
            {
                if (valueSize == 36)
                {
                    GuidCreator guid = new GuidCreator();
                    parse(ref guid);
                    value = guid.Value;
                    getValueEnd();
                }
                else if (valueSize == 0)
                {
                    value = null;
                    getValueEnd();
                }
                else State = ParseState.NotGuid;
            }
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref string value)
        {
            space();
            if (State != ParseState.Success) return;
            if (*current == '<')
            {
                if (*(current + 1) == '!')
                {
                    searchCData2();
                    if (State == ParseState.Success) value = valueSize == 0 ? string.Empty : new string(valueStart, 0, valueSize);
                }
                else value = string.Empty;
            }
            else
            {
                valueStart = current;
                valueSize = 0;
                do
                {
                    if (*current == '<')
                    {
                        int length = (int)(endSpace() - valueStart);
                        if (valueSize == 0) value = new string(valueStart, 0, length);
                        else
                        {
                            fixed (char* valueFixed = value = AutoCSer.Extension.StringExtension.FastAllocateString(length - valueSize))
                            {
                                decodeString(valueFixed, valueFixed + value.Length);
                            }
                        }
                        return;
                    }
                    if (*current == '&')
                    {
                        do
                        {
                            ++valueSize;
                            if (*++current == ';') break;
                            if (*current == '<')
                            {
                                State = ParseState.DecodeError;
                                return;
                            }
                        }
                        while (true);
                    }
                    ++current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref SubString value)
        {
            space();
            if (State != ParseState.Success) return;
            if (*current == '<')
            {
                if (*(current + 1) == '!')
                {
                    searchCData2();
                    if (State == ParseState.Success)
                    {
                        if (valueSize == 0) value.Set(string.Empty, 0, 0);
                        else value.Set(xml, (int)(valueStart - xmlFixed), valueSize);
                    }
                }
                else value.Set(string.Empty, 0, 0);
            }
            else
            {
                valueStart = current;
                valueSize = 0;
                do
                {
                    if (*current == '<')
                    {
                        int length = (int)(endSpace() - valueStart);
                        if (valueSize == 0) value.Set(xml, (int)(valueStart - xmlFixed), length);
                        else if (Config.IsTempString)
                        {
                            value.Set(xml, (int)(valueStart - xmlFixed), length - valueSize);
                            while (*valueStart != '&') ++valueStart;
                            decodeString(valueStart, xmlFixed + value.Start + value.Length);
                        }
                        else
                        {
                            string decodeValue = AutoCSer.Extension.StringExtension.FastAllocateString(length - valueSize);
                            fixed (char* valueFixed = decodeValue) decodeString(valueFixed, valueFixed + decodeValue.Length);
                            value.Set(decodeValue, 0, decodeValue.Length);
                        }
                        return;
                    }
                    if (*current == '&')
                    {
                        do
                        {
                            ++valueSize;
                            if (*++current == ';') break;
                            if (*current == '<')
                            {
                                State = ParseState.DecodeError;
                                return;
                            }
                        }
                        while (true);
                    }
                    ++current;
                }
                while (true);
            }
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
            if (State == ParseState.Success) value = node;
            //IgnoreValue();
        }
        /// <summary>
        /// XML节点解析
        /// </summary>
        /// <param name="value">数据</param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Parse(ref Node value)
        {
            space();
            if (State != ParseState.Success) return;
            if (*current == '<')
            {
                char code = *(current + 1);
                if (((bits[code & 0xff] & targetStartCheckBit) | (code & 0xff00)) == 0)
                {
                    if (code == '/')
                    {
                        value.SetString(string.Empty);
                        return;
                    }
                    if (code == '!')
                    {
                        searchCData2();
                        if (State == ParseState.Success) value.SetString(xml, (int)(valueStart - xmlFixed), valueSize);
                        return;
                    }
                    State = ParseState.NotFoundTagStart;
                    return;
                }
                char* nameStart;
                LeftArray<KeyValue<SubString, Node>> nodes = default(LeftArray<KeyValue<SubString, Node>>);
                KeyValue<Range, Range>[] attributes;
                int nameSize = 0;
                do
                {
                    nameStart = getName(ref nameSize);
                    if (State != ParseState.Success) return;
                    if (nameStart == null)
                    {
                        value.SetNode(ref nodes);
                        return;
                    }
                    nodes.PrepLength(1);
                    nodes.Array[nodes.Length].Key.Set(xml, (int)(nameStart - xmlFixed), nameSize);
                    attributes = Config.IsAttribute && this.attributes.Length != 0 ? this.attributes.GetArray() : null;
                    if (isTagEnd == 0)
                    {
                        Parse(ref nodes.Array[nodes.Length].Value);
                        if (State != ParseState.Success || CheckNameEnd(nameStart, nameSize) == 0) return;
                    }
                    if (attributes != null) nodes.Array[nodes.Length].Value.SetAttribute(xml, attributes);
                    ++nodes.Length;
                }
                while (true);
            }
            else
            {
                valueStart = current;
                value.Type = NodeType.String;
                do
                {
                    if (*current == '<')
                    {
                        value.String.Set(xml, (int)(valueStart - xmlFixed), (int)(endSpace() - valueStart));
                        if (Config.IsTempString && value.Type == NodeType.EncodeString) value.Type = NodeType.TempString;
                        return;
                    }
                    if (*current == '&')
                    {
                        value.Type = NodeType.EncodeString;
                        while (*++current != ';')
                        {
                            if (*current == '<')
                            {
                                State = ParseState.DecodeError;
                                return;
                            }
                        }
                    }
                    ++current;
                }
                while (true);
            }
        }

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
