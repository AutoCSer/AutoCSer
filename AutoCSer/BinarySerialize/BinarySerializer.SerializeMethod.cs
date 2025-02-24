﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.BinarySerialize;

namespace AutoCSer
{
    /// <summary>
    /// 基本类型序列化函数
    /// </summary>
    internal sealed class SerializeMethod : Attribute { }

    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [SerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(bool value)
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
        public void CallSerialize(bool[] value)
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
        public void CallSerialize(bool? value)
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
        public void CallSerialize(bool?[] value)
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
        public void CallSerialize(byte value)
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
        public void CallSerialize(byte[] value)
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
        public void CallSerialize(LeftArray<byte> value)
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
        public void CallSerialize(byte? value)
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
        public void CallSerialize(byte?[] value)
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
        public void CallSerialize(sbyte value)
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
        public void CallSerialize(sbyte[] value)
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
        public void CallSerialize(sbyte? value)
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
        public void CallSerialize(sbyte?[] value)
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
        public void CallSerialize(short value)
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
        public void CallSerialize(short[] value)
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
        public void CallSerialize(short? value)
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
        public void CallSerialize(short?[] value)
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
        public void CallSerialize(ushort value)
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
        public void CallSerialize(ushort[] value)
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
        public void CallSerialize(ushort? value)
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
        public void CallSerialize(ushort?[] value)
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
        public void CallSerialize(int value)
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
        public void CallSerialize(int[] value)
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
        public void CallSerialize(int? value)
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
        public void CallSerialize(int?[] value)
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
        public void CallSerialize(uint value)
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
        public void CallSerialize(uint[] value)
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
        public void CallSerialize(uint? value)
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
        public void CallSerialize(uint?[] value)
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
        public void CallSerialize(long value)
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
        public void CallSerialize(long[] value)
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
        public void CallSerialize(long? value)
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
        public void CallSerialize(long?[] value)
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
        public void CallSerialize(ulong value)
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
        public void CallSerialize(ulong[] value)
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
        public void CallSerialize(ulong? value)
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
        public void CallSerialize(ulong?[] value)
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
        public void CallSerialize(float value)
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
        public void CallSerialize(float[] value)
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
        public void CallSerialize(float? value)
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
        public void CallSerialize(float?[] value)
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
        public void CallSerialize(double value)
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
        public void CallSerialize(double[] value)
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
        public void CallSerialize(double? value)
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
        public void CallSerialize(double?[] value)
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
        public void CallSerialize(decimal value)
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
        public void CallSerialize(decimal[] value)
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
        public void CallSerialize(decimal? value)
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
        public void CallSerialize(decimal?[] value)
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
        public void CallSerialize(char value)
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
        public void CallSerialize(char[] value)
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
        public void CallSerialize(char? value)
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
        public void CallSerialize(char?[] value)
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
        public void CallSerialize(DateTime value)
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
        public void CallSerialize(DateTime[] value)
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
        public void CallSerialize(DateTime? value)
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
        public void CallSerialize(DateTime?[] value)
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
        public void CallSerialize(Guid value)
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
        public void CallSerialize(Guid[] value)
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
        public void CallSerialize(Guid? value)
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
        public void CallSerialize(Guid?[] value)
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
        public void CallSerialize(string value)
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
        public void CallSerialize(string[] array)
        {
            if (checkPoint(array))
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length);
                foreach (string value in array) arrayMap.Next(value != null);
                arrayMap.End();
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
        public void CallSerialize(SubString value)
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
        public void CallSerialize(Type value)
        {
            if (CheckPoint(value)) TypeSerializer<AutoCSer.Reflection.RemoteType>.StructSerialize(this, new AutoCSer.Reflection.RemoteType(value));
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
