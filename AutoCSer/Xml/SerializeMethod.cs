using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 基本转换类型
    /// </summary>
    internal sealed class SerializeMethod : Attribute { }

    /// <summary>
    /// XML序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool value)
        {
            if (value)
            {
                *(long*)CharStream.GetPrepSizeCurrent(4) = 'T' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48);
                CharStream.ByteSize += 4 * sizeof(char);
            }
            else
            {
                byte* chars = (byte*)CharStream.GetPrepSizeCurrent(5);
                *(long*)chars = 'F' + ('a' << 16) + ((long)'l' << 32) + ((long)'s' << 48);
                *(char*)(chars + sizeof(long)) = 'e';
                CharStream.ByteSize += 5 * sizeof(char);
            }
        }
        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool? value)
        {
            if (value.HasValue) Serialize((bool)value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(byte value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(byte? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((byte)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(sbyte value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(sbyte? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((sbyte)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(short value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(short? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((short)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ushort value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ushort? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((ushort)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(int value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(int? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((int)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(uint value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(uint? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((uint)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(long value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(long? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((long)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ulong value)
        {
            AutoCSer.Extension.Number.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ulong? value)
        {
            if (value.HasValue) AutoCSer.Extension.Number.ToString((ulong)value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(float value)
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
        public void Serialize(float? value)
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
        public void Serialize(double value)
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
        public void Serialize(double? value)
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
        public void Serialize(decimal value)
        {
            CharStream.SimpleWriteNotNull(value.ToString());
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(decimal? value)
        {
            if (value.HasValue) Serialize(value.Value);
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(char value)
        {
            if (((bits[(byte)value] & Parser.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch ((byte)value)
                {
                    case (byte)'\t':
                        *(long*)CharStream.GetPrepSizeCurrent(4) = '&' + ('#' << 16) + ((long)'9' << 32) + ((long)';' << 48);
                        CharStream.ByteSize += 4 * sizeof(char);
                        return;
                    case (byte)'\n':
                        byte* data10 = (byte*)CharStream.GetPrepSizeCurrent(5);
                        *(long*)data10 = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'0' << 48);
                        *(char*)(data10 + sizeof(long)) = ';';
                        CharStream.ByteSize += 5 * sizeof(char);
                        return;
                    case (byte)'\r':
                        byte* data13 = (byte*)CharStream.GetPrepSizeCurrent(5);
                        *(long*)data13 = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'3' << 48);
                        *(char*)(data13 + sizeof(long)) = ';';
                        CharStream.ByteSize += 5 * sizeof(char);
                        return;
                    case (byte)' ':
                        byte* data32 = (byte*)CharStream.GetPrepSizeCurrent(5);
                        *(long*)data32 = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'2' << 48);
                        *(char*)(data32 + sizeof(long)) = ';';
                        CharStream.ByteSize += 5 * sizeof(char);
                        return;
                    case (byte)'&':
                        byte* data38 = (byte*)CharStream.GetPrepSizeCurrent(5);
                        *(long*)data38 = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                        *(char*)(data38 + sizeof(long)) = ';';
                        CharStream.ByteSize += 5 * sizeof(char);
                        return;
                    case (byte)'<':
                        *(long*)CharStream.GetPrepSizeCurrent(4) = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        CharStream.ByteSize += 4 * sizeof(char);
                        return;
                    case (byte)'>':
                        *(long*)CharStream.GetPrepSizeCurrent(4) = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        CharStream.ByteSize += 4 * sizeof(char);
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
        public void Serialize(char? value)
        {
            if (value.HasValue) Serialize((char)value);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime value)
        {
            CharStream.PrepLength(AutoCSer.Date.MillisecondStringSize);
            Date.ToMillisecondString(value, CharStream);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime? value)
        {
            if (value.HasValue) Serialize((DateTime)value);
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Guid value)
        {
            new GuidCreator { Value = value }.ToString(CharStream.GetPrepSizeCurrent(36));
            CharStream.ByteSize += 36 * sizeof(char);
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Guid? value)
        {
            if (value.HasValue) Serialize((Guid)value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(string value)
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
        public void Serialize(SubString value)
        {
            if (value.Length == 0)
            {
                if (value.String != null) emptyString();
            }
            else
            {
                fixed (char* valueFixed = value.String) serialize(valueFixed + value.Start, value.Length);
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(object value)
        {
            if (value != null && value.GetType() == typeof(Node)) Serialize((Node)value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">XML节点</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Node value)
        {
            switch (value.Type)
            {
                case NodeType.String:
                    Serialize(value.String);
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
                        fixed (char* nameFixed = node.Key.String)
                        {
                            nameStart(nameFixed + node.Key.Start, node.Key.Length);
                            Serialize(node.Value);
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
