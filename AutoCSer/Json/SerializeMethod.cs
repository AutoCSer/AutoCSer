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
        public void CallSerialize(bool value)
        {
            if (Config.IsBoolToInt) CharStream.Write(value ? '1' : '0');
            else CharStream.WriteJsonBool(value);
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
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex);
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
            if (value.HasValue) CharStream.WriteJson((byte)value, Config.IsIntegerToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex);
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
            if (value.HasValue) CharStream.WriteJson((sbyte)value, Config.IsIntegerToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex);
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
            if (value.HasValue) CharStream.WriteJson((short)value, Config.IsIntegerToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex);
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
            if (value.HasValue) CharStream.WriteJson((ushort)value, Config.IsIntegerToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex);
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
            if (value.HasValue) CharStream.WriteJson((int)value, Config.IsIntegerToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex);
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
            if (value.HasValue) CharStream.WriteJson((uint)value, Config.IsIntegerToHex);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex, Config.IsMaxNumberToString);
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
            if (value.HasValue) CharStream.WriteJson((long)value, Config.IsIntegerToHex, Config.IsMaxNumberToString);
            else CharStream.WriteJsonNull();
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
            CharStream.WriteJson(value, Config.IsIntegerToHex, Config.IsMaxNumberToString);
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
            if (value.HasValue) CharStream.WriteJson((ulong)value, Config.IsIntegerToHex, Config.IsMaxNumberToString);
            else CharStream.WriteJsonNull();
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
        public void CallSerialize(float? value)
        {
            if (value.HasValue) CallSerialize(value.Value);
            else CharStream.WriteJsonNull();
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
        public void CallSerialize(double? value)
        {
            if (value.HasValue) CallSerialize(value.Value);
            else CharStream.WriteJsonNull();
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
            CharStream.SimpleWriteNotNull(value.ToString());
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
        public void CallSerialize(char value)
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
        public void CallSerialize(char? value)
        {
            if (value.HasValue) CallSerialize((char)value);
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(DateTime value)
        {
            if (value == DateTime.MinValue && Config.IsDateTimeMinNull)
            {
                CharStream.WriteJsonNull();
                return;
            }
            switch (Config.DateTimeType)
            {
                case DateTimeType.Default: CharStream.WriteJsonString(value); return;
                case DateTimeType.Sql: CharStream.WriteJsonSqlString(value); return;
                case DateTimeType.Javascript: CharStream.WriteJson(value, Config.IsIntegerToHex); return;
                case DateTimeType.ThirdParty: CharStream.WriteJsonOther(value); return;
                case DateTimeType.CustomFormat:
                    if (Config.DateTimeCustomFormat == null) CallSerialize(value.ToString());
                    else CallSerialize(value.ToString(Config.DateTimeCustomFormat));
                    return;
            }
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
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(System.Guid value)
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
        public void CallSerialize(System.Guid? value)
        {
            if (value.HasValue) CallSerialize((System.Guid)value);
            else CharStream.WriteJsonNull();
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
        public void CallSerialize(SubString value)
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
        public void CallSerialize(object value)
        {
            if (value == null) CharStream.WriteJsonNull();
            else if (value.GetType() == typeof(Node)) CallSerialize((Node)value);
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
        public void CallSerialize(Type type)
        {
            if (type == null) CharStream.WriteJsonNull();
            else
            {
                RemoteType remoteType = new RemoteType(type);
                TypeSerializer<RemoteType>.MemberSerialize(this, ref remoteType);
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">JSON节点</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(Node value)
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
                    CallSerialize((int)value.Int64 != 0);
                    return;
                case NodeType.DateTimeTick:
                    CallSerialize(new DateTime(value.Int64, DateTimeKind.Local));
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
