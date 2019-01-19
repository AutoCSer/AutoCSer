using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 基本类型序列化函数
    /// </summary>
    internal sealed class SerializeMemberMethod : Attribute { }
    /// <summary>
    /// 基本类型序列化函数
    /// </summary>
    internal sealed class SerializeMemberMapMethod : Attribute { }

    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(bool value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(bool[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(bool? value)
        {
            if (value.HasValue) Stream.UnsafeWrite((bool)value ? (byte)2 : (byte)1);
            else Stream.UnsafeWrite((byte)0);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(bool?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(byte value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(byte[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(byte? value)
        {
            if (value.HasValue) Stream.UnsafeWrite((ushort)(byte)value);
            else Stream.UnsafeWrite(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(byte?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(sbyte value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(sbyte[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(sbyte? value)
        {
            if (value.HasValue) Stream.UnsafeWrite((ushort)(byte)(sbyte)value);
            else Stream.UnsafeWrite(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(sbyte?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(short value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(short[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(short? value)
        {
            if (value.HasValue) Stream.UnsafeWrite((uint)(ushort)(short)value);
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(short?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ushort value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ushort[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ushort? value)
        {
            if (value.HasValue) Stream.UnsafeWrite((uint)(ushort)value);
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ushort?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(int value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(int[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(int? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(int*)(data + sizeof(int)) = (int)value;
                Stream.ByteSize += sizeof(int) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(int?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(uint value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(uint[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(uint? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(uint*)(data + sizeof(int)) = (uint)value;
                Stream.ByteSize += sizeof(uint) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(uint?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(long value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(long[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(long? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(long*)(data + sizeof(int)) = (long)value;
                Stream.ByteSize += sizeof(long) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(long?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ulong value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ulong[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ulong? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(ulong*)(data + sizeof(int)) = (ulong)value;
                Stream.ByteSize += sizeof(ulong) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(ulong?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(float value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(float[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(float? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(float*)(data + sizeof(int)) = (float)value;
                Stream.ByteSize += sizeof(float) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(float?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(double value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(double[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(double? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(double*)(data + sizeof(int)) = (double)value;
                Stream.ByteSize += sizeof(double) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(double?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(decimal value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(decimal[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(decimal? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(decimal*)(data + sizeof(int)) = (decimal)value;
                Stream.ByteSize += sizeof(decimal) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(decimal?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(char value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(char[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(char? value)
        {
            if (value.HasValue) Stream.UnsafeWrite((uint)(char)value);
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(char?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(DateTime value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(DateTime[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(DateTime? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(DateTime*)(data + sizeof(int)) = (DateTime)value;
                Stream.ByteSize += sizeof(DateTime) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(DateTime?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(Guid value)
        {
            Stream.UnsafeWrite(value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(Guid[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(Guid? value)
        {
            if (value.HasValue)
            {
                byte* data = Stream.CurrentData;
                *(int*)data = 0;
                *(Guid*)(data + sizeof(int)) = (Guid)value;
                Stream.ByteSize += sizeof(Guid) + sizeof(int);
            }
            else Stream.UnsafeWrite(NullValue);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(Guid?[] value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(string value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="array">字符串数组</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(string[] array)
        {
            if (array == null) Stream.Write(NullValue);
            else Serialize(array);
        }
        /// <summary>
        /// 类型信息序列化
        /// </summary>
        /// <param name="value">类型信息</param>
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberSerialize(Type value)
        {
            if (value == null) Stream.Write(NullValue);
            else Serialize(value);
        }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> memberSerializeMethods;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetMemberSerializeMethod(Type type)
        {
            MethodInfo method;
            return memberSerializeMethods.TryGetValue(type, out method) ? method : null;
        }
        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> memberMapSerializeMethods;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetMemberMapSerializeMethod(Type type)
        {
            MethodInfo method;
            return memberMapSerializeMethods.TryGetValue(type, out method) ? method : null;
        }
    }
}
