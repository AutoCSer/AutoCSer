using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using AutoCSer.Memory;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    internal unsafe static class Serializer
    {
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, bool value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, bool? value)
        {
            stream.Data.Write(value.HasValue ? ((bool)value ? (byte)2 : (byte)1) : (byte)0);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, byte value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, byte? value)
        {
            if (value.HasValue) stream.Data.Write((ushort)value.Value);
            else stream.Data.Write(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, sbyte value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, sbyte? value)
        {
            if (value.HasValue) stream.Data.Write((ushort)(byte)value.Value);
            else stream.Data.Write(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, short value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, short? value)
        {
            if (value.HasValue) stream.Data.Write((uint)(ushort)value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, ushort value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, ushort? value)
        {
            if (value.HasValue) stream.Data.Write((uint)value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, int value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, int? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, uint value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, uint? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, long value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, long? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, ulong value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, ulong? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, float value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, float? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, double value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, double? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, decimal value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, decimal? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, char value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, char? value)
        {
            if (value.HasValue) stream.Data.Write((uint)value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, DateTime value)
        {
            stream.Data.Write(value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, DateTime? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, Guid value)
        {
            stream.Data.Write(ref value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, Guid? value)
        {
            if (value.HasValue) stream.Data.SerializeWriteNullable(value.Value);
            else stream.Data.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(UnmanagedStream stream, string value)
        {
            if (value == null) stream.Write(BinarySerializer.NullValue);
            else if (value.Length == 0) stream.Write(0);
            else
            {
                fixed (char* valueFixed = value) BinarySerializer.Serialize(valueFixed, stream, value.Length);
            }
        }

#if NOJIT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumByte<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, byte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumSByte<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, sbyte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumShort<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, short>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumUShort<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, ushort>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumInt<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, int>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumUInt<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, uint>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumLong<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, long>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void enumULong<valueType>(UnmanagedStream stream, valueType value)
        {
            stream.UnsafeWrite(Emit.EnumCast<valueType, ulong>.ToInt(value));
        }
#endif

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) StructGenericType.Get(type).SimpleSerializeCompile();
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
        /// <summary>
        /// 判断是否可序列化类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsType(Type type)
        {
            return type == typeof(void) || type.IsEnum || serializeMethods.ContainsKey(type);
        }
        static Serializer()
        {
            serializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(Serializer).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(SerializeMethod), false)) serializeMethods.Add(method.GetParameters()[1].ParameterType, method);
            }
        }
    }
}
