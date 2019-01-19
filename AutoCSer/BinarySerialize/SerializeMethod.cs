using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 基本类型序列化函数
    /// </summary>
    internal sealed class SerializeMethod : Attribute { }

    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool value)
        {
            Stream.Write(value ? (int)1 : 0);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool? value)
        {
            Stream.Write((bool)value ? (int)1 : 0);
        }
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(bool?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(byte value)
        {
            Stream.Write((uint)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(byte[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(LeftArray<byte> value)
        {
            if (value.Length == 0) Stream.Write(0);
            else Serialize(Stream, ref value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(byte? value)
        {
            Stream.Write((uint)(byte)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(byte?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(sbyte value)
        {
            Stream.Write((int)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(sbyte[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(sbyte? value)
        {
            Stream.Write((int)(sbyte)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(sbyte?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(short value)
        {
            Stream.Write((int)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(short[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(short? value)
        {
            Stream.Write((int)(short)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(short?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ushort value)
        {
            Stream.Write((uint)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ushort[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ushort? value)
        {
            Stream.Write((uint)(ushort)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ushort?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(int value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(int[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(int? value)
        {
            Stream.Write((int)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(int?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(uint value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(uint[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(uint? value)
        {
            Stream.Write((uint)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(uint?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(long value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(long[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(long? value)
        {
            Stream.Write((long)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(long?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ulong value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ulong[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ulong? value)
        {
            Stream.Write((ulong)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(ulong?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(float value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(float[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(float? value)
        {
            Stream.Write((float)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(float?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(double value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(double[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(double? value)
        {
            Stream.Write((double)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(double?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(decimal value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(decimal[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(decimal? value)
        {
            Stream.Write((decimal)value);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="value">数值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(decimal?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(char value)
        {
            Stream.Write((uint)value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(char[] value)
        {
            if (checkPoint(value))
            {
                fixed (char* valueFixed = value) Serialize(valueFixed, Stream, value.Length);
            }
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(char? value)
        {
            Stream.Write((uint)(char)value);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="value">字符</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(char?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime? value)
        {
            Stream.Write((DateTime)value);
        }
        /// <summary>
        /// 时间序列化
        /// </summary>
        /// <param name="value">时间</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(DateTime?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Guid value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Guid[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Guid? value)
        {
            Stream.Write((Guid)value);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Guid?[] value)
        {
            if (checkPoint(value)) Serialize(Stream, value);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(string value)
        {
            if (value.Length == 0) Stream.Write(0);
            else if (CheckPoint(value))
            {
                fixed (char* valueFixed = value) Serialize(valueFixed, Stream, value.Length);
            }
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="array">字符串数组</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(string[] array)
        {
            if (checkPoint(array))
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length, 0);
                foreach (string value in array) arrayMap.Next(value != null);
                arrayMap.End(Stream);
                foreach (string value in array)
                {
                    if (value != null)
                    {
                        if (value.Length == 0) Stream.Write(0);
                        else if (CheckPoint(value))
                        {
                            fixed (char* valueFixed = value) Serialize(valueFixed, Stream, value.Length);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [SerializeMethod]
        [SerializeMemberMethod]
        [SerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(SubString value)
        {
            Serialize(Stream, ref value);
        }
        /// <summary>
        /// 类型信息序列化
        /// </summary>
        /// <param name="value">类型信息</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Serialize(Type value)
        {
            if (CheckPoint(value)) TypeSerializer<RemoteType>.Serialize(this, new RemoteType(value));
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
            if (serializeMethods.TryGetValue(type, out method))
            {
                serializeMethods.Remove(type);
                return method;
            }
            return null;
        }
    }
}
