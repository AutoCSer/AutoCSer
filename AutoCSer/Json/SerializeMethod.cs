using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 基本类型序列化函数配置
    /// </summary>
    internal sealed class SerializeMethod : Attribute { }
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool value)
        {
            CharStream.WriteJsonBool(value);
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
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex);
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
            if (value.HasValue) CharStream.WriteJson((byte)value, Config.IsNumberToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex);
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
            if (value.HasValue) CharStream.WriteJson((sbyte)value, Config.IsNumberToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex);
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
            if (value.HasValue) CharStream.WriteJson((short)value, Config.IsNumberToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex);
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
            if (value.HasValue) CharStream.WriteJson((ushort)value, Config.IsNumberToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex);
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
            if (value.HasValue) CharStream.WriteJson((int)value, Config.IsNumberToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex);
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
            if (value.HasValue) CharStream.WriteJson((uint)value, Config.IsNumberToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex, Config.IsMaxNumberToString);
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
            if (value.HasValue) CharStream.WriteJson((long)value, Config.IsNumberToHex, Config.IsMaxNumberToString);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsNumberToHex, Config.IsMaxNumberToString);
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
            if (value.HasValue) CharStream.WriteJson((ulong)value, Config.IsNumberToHex, Config.IsMaxNumberToString);
            else CharStream.WriteJsonNull();
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
            if (Config.IsInfinityToNaN) CharStream.WriteJson(value);
            else CharStream.WriteJsonInfinity(value);
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
            if (value.HasValue) Serialize(value.Value);
            else CharStream.WriteJsonNull();
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
            if (Config.IsInfinityToNaN) CharStream.WriteJson(value);
            else CharStream.WriteJsonInfinity(value);
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
            if (value.HasValue) Serialize(value.Value);
            else CharStream.WriteJsonNull();
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
            if (value.HasValue) CharStream.SimpleWriteNotNull(((decimal)value).ToString());
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(char value)
        {
            CharStream.WriteJson(value, Config.NullChar);
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
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime value)
        {
            if (value == DateTime.MinValue && Config.IsDateTimeMinNull)
            {
                CharStream.WriteJsonNull();
                return;
            }
            if (Config.IsDateTimeToString)
            {
                if (Config.IsDateTimeOther) CharStream.WriteJsonOther(value);
                else CharStream.WriteJsonString(value);
            }
            else CharStream.WriteJson(value, Config.IsNumberToHex);
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
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(System.Guid value)
        {
            CharStream.WriteJson(ref value);
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(System.Guid? value)
        {
            if (value.HasValue) Serialize((System.Guid)value);
            else CharStream.WriteJsonNull();
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
            if (value == null) CharStream.WriteJsonNull();
            else
            {
                fixed (char* valueFixed = value) CharStream.WriteJson(valueFixed, value.Length, Config.NullChar);
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
            if (value.String == null) CharStream.WriteJsonNull();
            else
            {
                fixed (char* valueFixed = value.String) CharStream.WriteJson(valueFixed + value.Start, value.Length, Config.NullChar);
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
            if (value == null) CharStream.WriteJsonNull();
            else if (value.GetType() == typeof(Node)) Serialize((Node)value);
            else if (Config.IsObject)
            {
                Type type = value.GetType();
                if (type == typeof(object)) CharStream.WriteJsonObject();
                //else SerializeMethodCache.GetObject(type)(this, value);
                else GenericType.Get(type).JsonSerializeObjectDelegate(this, value);
            }
            else CharStream.WriteJsonObject();
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="type">类型</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Type type)
        {
            if (type == null) CharStream.WriteJsonNull();
            else TypeSerializer<RemoteType>.Serialize(this, new RemoteType(type));
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">JSON节点</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Node value)
        {
            serialize(ref value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void serialize(ref Node value)
        {
            switch (value.Type)
            {
                case NodeType.Dictionary:
                    CharStream.Write('{');
                    if ((int)value.Int64 != 0)
                    {
                        KeyValue<Node, Node>[] array = value.DictionaryArray;
                        for (int index = 0; index != (int)value.Int64; ++index)
                        {
                            if (index != 0) CharStream.Write(',');
                            serialize(ref array[index].Key);
                            CharStream.Write(':');
                            serialize(ref array[index].Value);
                        }
                    }
                    CharStream.Write('}');
                    return;
                case NodeType.Array:
                    CharStream.Write('[');
                    if ((int)value.Int64 != 0)
                    {
                        Node[] array = value.ListArray;
                        for (int index = 0; index != (int)value.Int64; ++index)
                        {
                            if (index != 0) CharStream.Write(',');
                            serialize(ref array[index]);
                            CharStream.Write(':');
                            serialize(ref array[index]);
                        }
                    }
                    CharStream.Write(']');
                    return;
                case NodeType.String:
                    {
                        SubString subString = value.SubString;
                        fixed (char* valueFixed = subString.String) CharStream.WriteJson(valueFixed + subString.Start, subString.Length, Config.NullChar);
                    }
                    return;
                case NodeType.QuoteString:
                case NodeType.ErrorQuoteString:
                    CharStream.PrepLength(value.SubString.Length + 2);
                    CharStream.UnsafeWrite((char)value.Int64);
                    CharStream.Write(ref value.SubString);
                    CharStream.UnsafeWrite((char)value.Int64);
                    return;
                case NodeType.NumberString:
                    if ((int)value.Int64 == 0) CharStream.Write(ref value.SubString);
                    else
                    {
                        CharStream.PrepLength(value.SubString.Length + 2);
                        CharStream.UnsafeWrite((char)value.Int64);
                        CharStream.Write(ref value.SubString);
                        CharStream.UnsafeWrite((char)value.Int64);
                    }
                    return;
                case NodeType.Bool:
                    Serialize((int)value.Int64 != 0);
                    return;
                case NodeType.DateTimeTick:
                    Serialize(new DateTime(value.Int64, DateTimeKind.Local));
                    return;
                case NodeType.NaN:
                    CharStream.WriteJsonNaN();
                    return;
                case NodeType.PositiveInfinity:
                    if (Config.IsInfinityToNaN) CharStream.WriteJsonNaN();
                    else CharStream.WritePositiveInfinity();
                    return;
                case NodeType.NegativeInfinity:
                    if (Config.IsInfinityToNaN) CharStream.WriteJsonNaN();
                    else CharStream.WriteNegativeInfinity();
                    return;
                default:
                    CharStream.WriteJsonNull();
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
