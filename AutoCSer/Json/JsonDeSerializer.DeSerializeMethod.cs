using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Json;
using AutoCSer.Memory;

namespace AutoCSer.Json
{
    /// <summary>
    /// 基本类型解析函数配置
    /// </summary>
    internal sealed class DeSerializeMethod : Attribute { }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>解析状态</returns>
        [AutoCSer.Json.DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref bool value)
        {
            Quote = (char)0;
            deSerialize(out value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>解析状态</returns>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref bool? value)
        {
            Quote = (char)0;
            bool boolValue;
            if (deSerialize(out boolValue)) value = null;
            else value = boolValue;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref byte value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            deSerializeHex32(ref number);
                            value = (byte)number;
                        }
                        else value = (byte)deSerializeUInt32(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = (byte)number;
                return;
            }
            value = (byte)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref byte? value)
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            deSerializeHex32(ref number);
                            value = (byte)number;
                        }
                        else value = (byte)deSerializeUInt32(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = (byte)number;
                return;
            }
            value = (byte)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref sbyte value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                deSerializeHex32(ref number);
                                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                            }
                            else value = sign == 0 ? (sbyte)(byte)deSerializeUInt32(number) : (sbyte)-(int)deSerializeUInt32(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            value = sign == 0 ? (sbyte)(byte)deSerializeUInt32(number) : (sbyte)-(int)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref sbyte? value)
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
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                deSerializeHex32(ref number);
                                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                            }
                            else value = sign == 0 ? (sbyte)(byte)deSerializeUInt32(number) : (sbyte)-(int)deSerializeUInt32(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = sign == 0 ? (sbyte)(byte)number : (sbyte)-(int)number;
                return;
            }
            value = sign == 0 ? (sbyte)(byte)deSerializeUInt32(number) : (sbyte)-(int)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref ushort value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            deSerializeHex32(ref number);
                            value = (ushort)number;
                        }
                        else value = (ushort)deSerializeUInt32(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = (ushort)number;
                return;
            }
            value = (ushort)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref ushort? value)
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            deSerializeHex32(ref number);
                            value = (ushort)number;
                        }
                        else value = (ushort)deSerializeUInt32(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = (ushort)number;
                return;
            }
            value = (ushort)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref short value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                deSerializeHex32(ref number);
                                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                            }
                            else value = sign == 0 ? (short)(ushort)deSerializeUInt32(number) : (short)-(int)deSerializeUInt32(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            value = sign == 0 ? (short)(ushort)deSerializeUInt32(number) : (short)-(int)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref short? value)
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
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                deSerializeHex32(ref number);
                                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                            }
                            else value = sign == 0 ? (short)(ushort)deSerializeUInt32(number) : (short)-(int)deSerializeUInt32(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = sign == 0 ? (short)(ushort)number : (short)-(int)number;
                return;
            }
            value = sign == 0 ? (short)(ushort)deSerializeUInt32(number) : (short)-(int)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref uint value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            deSerializeHex32(ref number);
                            value = number;
                        }
                        else value = deSerializeUInt32(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = number;
                return;
            }
            value = deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref uint? value)
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            deSerializeHex32(ref number);
                            value = number;
                        }
                        else value = deSerializeUInt32(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = number;
                return;
            }
            value = deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref int value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                deSerializeHex32(ref number);
                                value = sign == 0 ? (int)number : -(int)number;
                            }
                            else value = sign == 0 ? (int)deSerializeUInt32(number) : -(int)deSerializeUInt32(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            value = sign == 0 ? (int)deSerializeUInt32(number) : -(int)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref int? value)
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
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                deSerializeHex32(ref number);
                                value = sign == 0 ? (int)number : -(int)number;
                            }
                            else value = sign == 0 ? (int)deSerializeUInt32(number) : -(int)deSerializeUInt32(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote) ++Current;
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                deSerializeHex32(ref number);
                value = sign == 0 ? (int)number : -(int)number;
                return;
            }
            value = sign == 0 ? (int)deSerializeUInt32(number) : -(int)deSerializeUInt32(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref ulong value)
        {
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                if ((number = (uint)(*Current - '0')) > 9)
                {
                    if (*Current == '"' || *Current == '\'')
                    {
                        Quote = *Current;
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            value = deSerializeHex64();
                        }
                        else value = deSerializeUInt64(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                value = deSerializeHex64();
                return;
            }
            value = deSerializeUInt64(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref ulong? value)
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.NotNumber;
                                return;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            value = deSerializeHex64();
                        }
                        else value = deSerializeUInt64(number);
                        if (DeSerializeState == DeSerializeState.Success)
                        {
                            if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                            else if (*Current == Quote) ++Current;
                            else DeSerializeState = DeSerializeState.NotNumber;
                        }
                        return;
                    }
                    DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                value = deSerializeHex64();
                return;
            }
            value = deSerializeUInt64(number);
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref long value)
        {
            int sign = 0;
            uint number = (uint)(*Current - '0');
            if (number > 9)
            {
                if (*Current == '-')
                {
                    if (++Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        if (*Current == '"' || *Current == '\'')
                        {
                            Quote = *Current;
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                value = (long)deSerializeHex64();
                            }
                            else value = (long)deSerializeUInt64(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote)
                                {
                                    if (sign != 0) value = -value;
                                    ++Current;
                                }
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                value = (long)deSerializeHex64();
                if (sign != 0) value = -value;
                return;
            }
            value = (long)deSerializeUInt64(number);
            if (sign != 0) value = -value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref long? value)
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
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    if ((number = (uint)(*Current - '0')) > 9)
                    {
                        DeSerializeState = DeSerializeState.NotNumber;
                        return;
                    }
                    sign = 1;
                }
                else
                {
                    space();
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
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
                                DeSerializeState = DeSerializeState.CrashEnd;
                                return;
                            }
                            if ((number = (uint)(*Current - '0')) > 9)
                            {
                                if (*Current != '-')
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                if ((number = (uint)(*Current - '0')) > 9)
                                {
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                sign = 1;
                            }
                            if (++Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
                                    DeSerializeState = DeSerializeState.NotNumber;
                                    return;
                                }
                                if (++Current == end)
                                {
                                    DeSerializeState = DeSerializeState.CrashEnd;
                                    return;
                                }
                                value = (long)deSerializeHex64();
                            }
                            else value = (long)deSerializeUInt64(number);
                            if (DeSerializeState == DeSerializeState.Success)
                            {
                                if (Current == end) DeSerializeState = DeSerializeState.CrashEnd;
                                else if (*Current == Quote)
                                {
                                    if (sign != 0) value = -value;
                                    ++Current;
                                }
                                else DeSerializeState = DeSerializeState.NotNumber;
                            }
                            return;
                        }
                        if (*Current != '-')
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
                            return;
                        }
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if ((number = (uint)(*Current - '0')) > 9)
                        {
                            DeSerializeState = DeSerializeState.NotNumber;
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
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                long hexValue = (long)deSerializeHex64();
                value = sign == 0 ? hexValue : -hexValue;
                return;
            }
            long value64 = (long)deSerializeUInt64(number);
            value = sign == 0 ? value64 : -value64;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref float value)
        {
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberType.Number:
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref value))
                    {
                        Current = end;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.String:
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
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
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref float? value)
        {
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberType.Number:
                    float valueNumber = default(float);
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref valueNumber))
                    {
                        value = valueNumber;
                        Current = end;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.String:
                    float valueString = default(float);
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref valueString))
                    {
                        value = valueString;
                        Current = end + 1;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
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
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref double value)
        {
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberType.Number:
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref value))
                    {
                        Current = end;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.String:
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
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
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref double? value)
        {
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberType.Number:
                    double valueNumber = default(double);
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref valueNumber))
                    {
                        value = valueNumber;
                        Current = end;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.String:
                    double valueString = default(double);
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref valueString))
                    {
                        value = valueString;
                        Current = end + 1;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
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
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref decimal value)
        {
            char* end = null;
            switch (searchNumber(ref end))
            {
                case NumberType.Number:
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref value))
                    {
                        Current = end;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.String:
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref value))
                    {
                        Current = end + 1;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.NaN:
                case NumberType.PositiveInfinity:
                case NumberType.NegativeInfinity:
                    DeSerializeState = DeSerializeState.NotNumber;
                    return;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref decimal? value)
        {
            char* end = null;
            switch (searchNumberNull(ref end))
            {
                case NumberType.Number:
                    decimal valueNumber = default(decimal);
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref valueNumber))
                    {
                        value = valueNumber;
                        Current = end;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.String:
                    decimal valueString = default(decimal);
                    if (JsonSerializer.CustomConfig.DeSerialize(this, new ReadOnlySpan<char>(Current, (int)(end - Current)), ref valueString))
                    {
                        value = valueString;
                        Current = end + 1;
                    }
                    else DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.NaN:
                case NumberType.PositiveInfinity:
                case NumberType.NegativeInfinity:
                    DeSerializeState = DeSerializeState.NotNumber;
                    return;
                case NumberType.Null: value = null; return;
            }
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref char value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == '\\')
                    {
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if (*Current == 'u')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                            {
                                DeSerializeState = DeSerializeState.NotChar;
                                return;
                            }
                            value = (char)deSerializeHex4();
                        }
                        else if (*Current == 'x')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                            {
                                DeSerializeState = DeSerializeState.NotChar;
                                return;
                            }
                            value = (char)deSerializeHex2();
                        }
                        else value = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
                    }
                    else
                    {
                        if (*Current == Quote)
                        {
                            DeSerializeState = DeSerializeState.NotChar;
                            return;
                        }
                        value = *Current;
                    }
                }
                else value = *Current;
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotChar;
        }
        /// <summary>
        /// 字符解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref char? value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                Quote = *Current;
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                if (((bits[*(byte*)Current] & AutoCSer.JsonDeSerializer.DeSerializeEscapeSearchBit) | *(((byte*)Current) + 1)) == 0)
                {
                    if (*Current == '\\')
                    {
                        if (++Current == end)
                        {
                            DeSerializeState = DeSerializeState.CrashEnd;
                            return;
                        }
                        if (*Current == 'u')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 5 * sizeof(char))
                            {
                                DeSerializeState = DeSerializeState.NotChar;
                                return;
                            }
                            value = (char)deSerializeHex4();
                        }
                        else if (*Current == 'x')
                        {
                            if ((int)((byte*)end - (byte*)Current) < 3 * sizeof(char))
                            {
                                DeSerializeState = DeSerializeState.NotChar;
                                return;
                            }
                            value = (char)deSerializeHex2();
                        }
                        else value = *Current < escapeCharSize ? escapeChars[*Current] : *Current;
                    }
                    else
                    {
                        if (*Current == Quote)
                        {
                            DeSerializeState = DeSerializeState.NotChar;
                            return;
                        }
                        value = *Current;
                    }
                }
                else value = *Current;
                if (++Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
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
                    if (DeSerializeState != DeSerializeState.Success) return;
                    if (Current == end)
                    {
                        DeSerializeState = DeSerializeState.CrashEnd;
                        return;
                    }
                    isSpace = 1;
                    goto START;
                }
            }
            DeSerializeState = DeSerializeState.NotChar;
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref DateTime value)
        {
            Quote = (char)0;
            if (deSerializeDateTime(ref value)) value = DateTime.MinValue;
        }
        /// <summary>
        /// 时间解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref DateTime? value)
        {
            Quote = (char)0;
            DateTime dateTime = default(DateTime);
            if (deSerializeDateTime(ref dateTime)) value = null;
            else value = dateTime;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref System.Guid value)
        {
            if (*Current == '\'' || *Current == '"')
            {
                GuidCreator guid = new GuidCreator();
                deSerialize(ref guid);
                value = guid.Value;
                return;
            }
            space();
            if (DeSerializeState != DeSerializeState.Success) return;
            if (Current == end)
            {
                DeSerializeState = DeSerializeState.CrashEnd;
                return;
            }
            if (*Current == '\'' || *Current == '"')
            {
                GuidCreator guid = new GuidCreator();
                deSerialize(ref guid);
                value = guid.Value;
                return;
            }
            DeSerializeState = DeSerializeState.NotGuid;
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref System.Guid? value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                GuidCreator guid = new GuidCreator();
                deSerialize(ref guid);
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotGuid;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref string value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
            {
                value = deSerializeString();
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotString;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref SubString value)
        {
            byte isSpace = 0;
        START:
            if (*Current == '"' || *Current == '\'')
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
                        if (this.json == null) value = new string(start, 0, (int)(Current++ - start));
                        else value.Set(this.json, (int)(start - jsonFixed), (int)(Current++ - start));
                    }
                    return;
                }
                if (Config.IsTempString && this.json != null)
                {
                    char* writeEnd = deSerializeEscape();
                    if (writeEnd != null) value.Set(this.json, (int)(start - jsonFixed), (int)(writeEnd - start));
                }
                else
                {
                    string newValue = deSerializeEscape(start);
                    if (newValue != null) value.Set(newValue, 0, newValue.Length);
                }
                return;
            }
            if (*(long*)(Current) == 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48) && (int)((byte*)end - (byte*)Current) >= 4 * sizeof(char))
            {
                value.SetEmpty();
                Current += 4;
                return;
            }
            if (isSpace == 0)
            {
                space();
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.NotString;
        }
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref object value)
        {
            Node node = default(Node);
            CallSerialize(ref node);
            if (DeSerializeState == DeSerializeState.Success)
            {
                if (node.Type == NodeType.Null) value = null;
                else value = node;
            }
        }
        /// <summary>
        /// 类型解析
        /// </summary>
        /// <param name="type">类型</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref Type type)
        {
            byte isSpace = 0;
        START:
            if (*Current == '{')
            {
                AutoCSer.Reflection.RemoteType remoteType = default(AutoCSer.Reflection.RemoteType);
                TypeDeSerializer<AutoCSer.Reflection.RemoteType>.DeSerializeValue(this, ref remoteType);
                if (DeSerializeState != DeSerializeState.Success) return;
                if (!remoteType.TryGet(out type)) DeSerializeState = DeSerializeState.ErrorType;
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
                if (DeSerializeState != DeSerializeState.Success) return;
                if (Current == end)
                {
                    DeSerializeState = DeSerializeState.CrashEnd;
                    return;
                }
                isSpace = 1;
                goto START;
            }
            DeSerializeState = DeSerializeState.ErrorType;
        }
        /// <summary>
        /// JSON节点解析
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref Node value)
        {
            LeftArray<Node> nodeArray = new LeftArray<Node>(0), nameArray = new LeftArray<Node>(0);
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
                        deSerializeStringNode(ref value);
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
                        value.SetDictionary();
                        if (IsFirstObject())
                        {
                            nodeArray.Add(value);
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
                        value.SetList();
                        if (IsFirstArrayValue())
                        {
                            nodeArray.Add(value);
                            value = default(Node);
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
                            CallSerialize(ref millisecond);
                            if (DeSerializeState != DeSerializeState.Success) return;
                            if (Current == end)
                            {
                                DeSerializeState = DeSerializeState.CrashEnd;
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
            DeSerializeState = DeSerializeState.UnknownValue;
            return;

        DICTIONARYNAME:
            if (*Current == '"' || *Current == '\'') deSerializeStringNode(ref value);
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
            if (DeSerializeState != DeSerializeState.Success || SearchColon() == 0) return;
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