using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Xml;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 解析类型
    /// </summary>
    internal sealed class DeSerializeMethod : Attribute { }
}
namespace AutoCSer
{
    /// <summary>
    /// XML 解析器
    /// </summary>
    public unsafe sealed partial class XmlDeSerializer
    {
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref bool value)
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
                State = DeSerializeState.NotBool;
            }
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref bool? value)
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
                State = DeSerializeState.NotBool;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref byte value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref byte? value)
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
                            deSerializeHex32(ref number);
                            if (State != DeSerializeState.Success) return;
                            value = (byte)number;
                        }
                        else value = 0;
                    }
                    else value = (byte)deSerializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref sbyte value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref sbyte? value)
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
                            deSerializeHex32(ref number);
                            if (State != DeSerializeState.Success) return;
                            value = sign == '-' ? (sbyte)-(int)number : (sbyte)(byte)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (sbyte)-(int)deSerializeUInt32(number) : (sbyte)(byte)deSerializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref ushort value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref ushort? value)
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
                            deSerializeHex32(ref number);
                            if (State != DeSerializeState.Success) return;
                            value = (ushort)number;
                        }
                        else value = 0;
                    }
                    else value = (ushort)deSerializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref short value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref short? value)
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
                            deSerializeHex32(ref number);
                            if (State != DeSerializeState.Success) return;
                            value = sign == '-' ? (short)-(int)number : (short)(ushort)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (short)-(int)deSerializeUInt32(number) : (short)(ushort)deSerializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref uint value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref uint? value)
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
                            deSerializeHex32(ref number);
                            if (State != DeSerializeState.Success) return;
                            value = number;
                        }
                        else value = 0;
                    }
                    else value = deSerializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref int value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref int? value)
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
                            deSerializeHex32(ref number);
                            if (State != DeSerializeState.Success) return;
                            value = sign == '-' ? -(int)number : (int)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(int)deSerializeUInt32(number) : (int)deSerializeUInt32(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref ulong value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref ulong? value)
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
                            value = deSerializeHex64();
                            if (State != DeSerializeState.Success) return;
                        }
                        else value = 0;
                    }
                    else value = deSerializeUInt64(number);
                    SearchValueEnd();
                    return;
                }
                if (number == '<' - '0')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref long value)
        {
            searchValue();
            DeSerializeNumber(ref value);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref long? value)
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
                            value = sign == '-' ? -(long)deSerializeHex64() : (long)deSerializeHex64();
                            if (State != DeSerializeState.Success) return;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(long)deSerializeUInt64(number) : (long)deSerializeUInt64(number);
                    SearchValueEnd();
                    return;
                }
                if (sign == '<')
                {
                    value = null;
                    SearchValueEnd();
                    return;
                }
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref float value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref float? value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                    else State = DeSerializeState.NotNumber;
                }
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref double value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref double? value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                    else State = DeSerializeState.NotNumber;
                }
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref decimal value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                State = DeSerializeState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref decimal? value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                    else State = DeSerializeState.NotNumber;
                }
            }
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref char value)
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
                if (State == DeSerializeState.Success) getValueEnd();
                return;
            }
            State = DeSerializeState.NotChar;
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref char? value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                            if (State == DeSerializeState.Success)
                            {
                                value = charValue;
                                getValueEnd();
                            }
                            return;
                        }
                        break;
                }
                State = DeSerializeState.NotChar;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref DateTime value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                State = DeSerializeState.NotDateTime;
            }
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref DateTime? value)
        {
            getValue();
            if (State == DeSerializeState.Success)
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
                    else State = DeSerializeState.NotDateTime;
                }
            }
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref Guid value)
        {
            getValue();
            if (State == DeSerializeState.Success)
            {
                if (valueSize == 36)
                {
                    GuidCreator guid = new GuidCreator();
                    deSerialize(ref guid);
                    value = guid.Value;
                    getValueEnd();
                }
                else State = DeSerializeState.NotGuid;
            }
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref Guid? value)
        {
            getValue();
            if (State == DeSerializeState.Success)
            {
                if (valueSize == 36)
                {
                    GuidCreator guid = new GuidCreator();
                    deSerialize(ref guid);
                    value = guid.Value;
                    getValueEnd();
                }
                else if (valueSize == 0)
                {
                    value = null;
                    getValueEnd();
                }
                else State = DeSerializeState.NotGuid;
            }
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref string value)
        {
            space();
            if (State != DeSerializeState.Success) return;
            if (*current == '<')
            {
                if (*(current + 1) == '!')
                {
                    searchCData2();
                    if (State == DeSerializeState.Success) value = valueSize == 0 ? string.Empty : new string(valueStart, 0, valueSize);
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
                            fixed (char* valueFixed = value = AutoCSer.Extensions.StringExtension.FastAllocateString(length - valueSize))
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
                                State = DeSerializeState.DecodeError;
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
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref SubString value)
        {
            space();
            if (State != DeSerializeState.Success) return;
            if (*current == '<')
            {
                if (*(current + 1) == '!')
                {
                    searchCData2();
                    if (State == DeSerializeState.Success)
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
                            string decodeValue = AutoCSer.Extensions.StringExtension.FastAllocateString(length - valueSize);
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
                                State = DeSerializeState.DecodeError;
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
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref object value)
        {
            Node node = default(Node);
            CallDeSerialize(ref node);
            if (State == DeSerializeState.Success) value = node;
            //IgnoreValue();
        }
        /// <summary>
        /// XML节点解析
        /// </summary>
        /// <param name="value">数据</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallDeSerialize(ref Node value)
        {
            space();
            if (State != DeSerializeState.Success) return;
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
                        if (State == DeSerializeState.Success) value.SetString(xml, (int)(valueStart - xmlFixed), valueSize);
                        return;
                    }
                    State = DeSerializeState.NotFoundTagStart;
                    return;
                }
                char* nameStart;
                LeftArray<KeyValue<SubString, Node>> nodes = new LeftArray<KeyValue<SubString, Node>>(0);
                KeyValue<Range, Range>[] attributes;
                int nameSize = 0;
                do
                {
                    nameStart = getName(ref nameSize);
                    if (State != DeSerializeState.Success) return;
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
                        CallDeSerialize(ref nodes.Array[nodes.Length].Value);
                        if (State != DeSerializeState.Success || CheckNameEnd(nameStart, nameSize) == 0) return;
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
                                State = DeSerializeState.DecodeError;
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
        private static readonly Dictionary<Type, MethodInfo> deSerializeMethods;
        /// <summary>
        /// 获取基本类型解析函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>解析函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetDeSerializeMethod(Type type)
        {
            MethodInfo method;
            return deSerializeMethods.TryGetValue(type, out method) ? method : null;
        }
    }
}