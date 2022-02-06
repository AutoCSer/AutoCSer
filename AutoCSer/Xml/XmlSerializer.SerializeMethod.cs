using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Xml;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 基本转换类型
    /// </summary>
    internal sealed class SerializeMethod : Attribute { }
}
namespace AutoCSer
{
    /// <summary>
    /// XML序列化
    /// </summary>
    public unsafe sealed partial class XmlSerializer
    {
        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(bool value)
        {
            if (value)
            {
                *(long*)CharStream.GetBeforeMove(4 * sizeof(char)) = 'T' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48);
            }
            else
            {
                byte* chars = (byte*)CharStream.GetBeforeMove(5 * sizeof(char));
                *(long*)chars = 'F' + ('a' << 16) + ((long)'l' << 32) + ((long)'s' << 48);
                *(char*)(chars + sizeof(long)) = 'e';
            }
        }
        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(bool? value)
        {
            if (value.HasValue) CallSerialize((bool)value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(byte value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(byte? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((byte)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(sbyte value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(sbyte? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((sbyte)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(short value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(short? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((short)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ushort value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ushort? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((ushort)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(int value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(int? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((int)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(uint value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(uint? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((uint)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(long value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(long? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((long)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ulong value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ulong? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString((ulong)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(float value)
        {
            CharStream.WriteJsonInfinity(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(float? value)
        {
            if (value.HasValue) CharStream.WriteJsonInfinity(value.Value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(double value)
        {
            CharStream.WriteJsonInfinity(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(double? value)
        {
            if (value.HasValue) CharStream.WriteJsonInfinity(value.Value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(decimal value)
        {
            CharStream.SimpleWrite(value.ToString());
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(decimal? value)
        {
            if (value.HasValue) CallSerialize(value.Value);
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(char value)
        {
            if (((bits[(byte)value] & XmlDeSerializer.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch ((byte)value)
                {
                    case (byte)'\t':
                        *(long*)CharStream.GetBeforeMove(4 * sizeof(char)) = '&' + ('#' << 16) + ((long)'9' << 32) + ((long)';' << 48);
                        return;
                    case (byte)'\n':
                        byte* data10 = CharStream.GetBeforeMove(5 * sizeof(char));
                        *(long*)data10 = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'0' << 48);
                        *(char*)(data10 + sizeof(long)) = ';';
                        return;
                    case (byte)'\r':
                        byte* data13 = CharStream.GetBeforeMove(5 * sizeof(char));
                        *(long*)data13 = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'3' << 48);
                        *(char*)(data13 + sizeof(long)) = ';';
                        return;
                    case (byte)' ':
                        byte* data32 = CharStream.GetBeforeMove(5 * sizeof(char));
                        *(long*)data32 = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'2' << 48);
                        *(char*)(data32 + sizeof(long)) = ';';
                        return;
                    case (byte)'&':
                        byte* data38 = CharStream.GetBeforeMove(5 * sizeof(char));
                        *(long*)data38 = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                        *(char*)(data38 + sizeof(long)) = ';';
                        return;
                    case (byte)'<':
                        *(long*)CharStream.GetBeforeMove(4 * sizeof(char)) = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        return;
                    case (byte)'>':
                        *(long*)CharStream.GetBeforeMove(4 * sizeof(char)) = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        return;
                }
            }
            CharStream.Write(value);
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(char? value)
        {
            if (value.HasValue) CallSerialize((char)value);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(DateTime value)
        {
            CharStream.PrepCharSize(AutoCSer.Date.MillisecondStringSize);
            Date.ToMillisecondString(value, CharStream);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(DateTime? value)
        {
            if (value.HasValue) CallSerialize((DateTime)value);
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(Guid value)
        {
            new GuidCreator { Value = value }.ToString((char*)CharStream.GetBeforeMove(36 * sizeof(char)));
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(Guid? value)
        {
            if (value.HasValue) CallSerialize((Guid)value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(string value)
        {
            if (value != null)
            {
                if (value.Length == 0) emptyString();
                else
                {
                    fixed (char* valueFixed = value) serialize(valueFixed, value.Length);
                }
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(SubString value)
        {
            if (value.Length == 0)
            {
                if (value.String != null) emptyString();
            }
            else
            {
                fixed (char* valueFixed = value.GetFixedBuffer()) serialize(valueFixed + value.Start, value.Length);
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(object value)
        {
            if (value != null && value.GetType() == typeof(Node)) CallSerialize((Node)value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">XML节点</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(Node value)
        {
            switch (value.Type)
            {
                case NodeType.String:
                    CallSerialize(value.String);
                    return;
                case NodeType.EncodeString:
                    CharStream.Write(value.String);
                    return;
                case NodeType.TempString:
                    CharStream.Write(value.String);
                    return;
                case NodeType.ErrorString:
                    return;
                case NodeType.Node:
                    foreach (KeyValue<SubString, Node> node in value.Nodes)
                    {
                        fixed (char* nameFixed = node.Key.GetFixedBuffer())
                        {
                            nameStart(nameFixed + node.Key.Start, node.Key.Length);
                            CallSerialize(node.Value);
                            nameEnd(nameFixed + node.Key.Start, node.Key.Length);
                        }
                    }
                    return;
            }
        }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> serializeMethods;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetSerializeMethod(Type type)
        {
            MethodInfo method;
            return serializeMethods.TryGetValue(type, out method) ? method : null;
        }
    }
}
