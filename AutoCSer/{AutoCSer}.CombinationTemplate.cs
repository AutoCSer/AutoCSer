//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumLongMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, long>.FromInt(*(long*)deSerializer.Read);
            deSerializer.Read += sizeof(long);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumLongArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize(length * sizeof(long) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(long)) array[index++] = AutoCSer.Metadata.EnumCast<T, long>.FromInt(*(long*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumLongArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumLongArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumLongArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumLongArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumLongArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumLongArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUIntMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, uint>.FromInt(*(uint*)deSerializer.Read);
            deSerializer.Read += sizeof(uint);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumUIntArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize(length * sizeof(uint) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(uint)) array[index++] = AutoCSer.Metadata.EnumCast<T, uint>.FromInt(*(uint*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumUIntArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumUIntArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUIntArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumUIntArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUIntArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumUIntArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumIntMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, int>.FromInt(*(int*)deSerializer.Read);
            deSerializer.Read += sizeof(int);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumIntArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize(length * sizeof(int) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(int)) array[index++] = AutoCSer.Metadata.EnumCast<T, int>.FromInt(*(int*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumIntArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumIntArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumIntArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumIntArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumIntArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumIntArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUShortMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, ushort>.FromInt(*(ushort*)deSerializer.Read);
            deSerializer.Read += sizeof(ushort);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumUShortArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize4(length * sizeof(ushort) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(ushort)) array[index++] = AutoCSer.Metadata.EnumCast<T, ushort>.FromInt(*(ushort*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumUShortArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumUShortArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUShortArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumUShortArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUShortArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumUShortArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumShortMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, short>.FromInt(*(short*)deSerializer.Read);
            deSerializer.Read += sizeof(short);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumShortArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize4(length * sizeof(short) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(short)) array[index++] = AutoCSer.Metadata.EnumCast<T, short>.FromInt(*(short*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumShortArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumShortArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumShortArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumShortArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumShortArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumShortArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumByteMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, byte>.FromInt(*(byte*)deSerializer.Read);
            deSerializer.Read += sizeof(byte);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumByteArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize4(length * sizeof(byte) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(byte)) array[index++] = AutoCSer.Metadata.EnumCast<T, byte>.FromInt(*(byte*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumByteArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumByteArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumByteArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumByteArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumByteArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumByteArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public unsafe sealed partial class BinaryDeSerializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="value">枚举值反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumSByteMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, sbyte>.FromInt(*(sbyte*)deSerializer.Read);
            deSerializer.Read += sizeof(sbyte);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumSByteArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize4(length * sizeof(sbyte) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(sbyte)) array[index++] = AutoCSer.Metadata.EnumCast<T, sbyte>.FromInt(*(sbyte*)data);
                        Read += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void enumSByteArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumSByteArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumSByteArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumSByteArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumSByteArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumSByteArray(ref array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumLongMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumLongCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize(count * sizeof(long) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(long*)write = AutoCSer.Metadata.EnumCast<T, long>.ToInt(value);
                    write += sizeof(long);
                    if (--writeCount == 0)
                    {
                        FillSize(write, count);
                        return;
                    }
                }
                FillSize(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize(count * sizeof(long) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumLongCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumLongCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumLongCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumLongCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumLongArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize(array.Length * sizeof(long) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(long*)write = AutoCSer.Metadata.EnumCast<T, long>.ToInt(value);
                    write += sizeof(long);
                }
                FillSize(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumLongArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumLongArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumLongArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumLongArray(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUIntMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumUIntCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize(count * sizeof(uint) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(uint*)write = AutoCSer.Metadata.EnumCast<T, uint>.ToInt(value);
                    write += sizeof(uint);
                    if (--writeCount == 0)
                    {
                        FillSize(write, count);
                        return;
                    }
                }
                FillSize(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize(count * sizeof(uint) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumUIntCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumUIntCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumUIntCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumUIntCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumUIntArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize(array.Length * sizeof(uint) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(uint*)write = AutoCSer.Metadata.EnumCast<T, uint>.ToInt(value);
                    write += sizeof(uint);
                }
                FillSize(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUIntArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumUIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumUIntArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumUIntArray(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumIntMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumIntCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize(count * sizeof(int) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(int*)write = AutoCSer.Metadata.EnumCast<T, int>.ToInt(value);
                    write += sizeof(int);
                    if (--writeCount == 0)
                    {
                        FillSize(write, count);
                        return;
                    }
                }
                FillSize(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize(count * sizeof(int) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumIntCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumIntCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumIntCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumIntCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumIntArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize(array.Length * sizeof(int) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(int*)write = AutoCSer.Metadata.EnumCast<T, int>.ToInt(value);
                    write += sizeof(int);
                }
                FillSize(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumIntArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumIntArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumIntArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumIntArray(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUShortMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumUShortCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize4(count * sizeof(ushort) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(ushort*)write = AutoCSer.Metadata.EnumCast<T, ushort>.ToInt(value);
                    write += sizeof(ushort);
                    if (--writeCount == 0)
                    {
                        FillSize2(write, count);
                        return;
                    }
                }
                FillSize2(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize4(count * sizeof(ushort) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumUShortCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumUShortCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumUShortCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumUShortCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumUShortArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize4(array.Length * sizeof(ushort) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(ushort*)write = AutoCSer.Metadata.EnumCast<T, ushort>.ToInt(value);
                    write += sizeof(ushort);
                }
                FillSize2(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumUShortArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumUShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumUShortArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumUShortArray(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumShortMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumShortCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize4(count * sizeof(short) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(short*)write = AutoCSer.Metadata.EnumCast<T, short>.ToInt(value);
                    write += sizeof(short);
                    if (--writeCount == 0)
                    {
                        FillSize2(write, count);
                        return;
                    }
                }
                FillSize2(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize4(count * sizeof(short) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumShortCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumShortCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumShortCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumShortCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumShortArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize4(array.Length * sizeof(short) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(short*)write = AutoCSer.Metadata.EnumCast<T, short>.ToInt(value);
                    write += sizeof(short);
                }
                FillSize2(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumShortArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumShortArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumShortArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumShortArray(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumByteMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumByteCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize4(count * sizeof(byte) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(byte*)write = AutoCSer.Metadata.EnumCast<T, byte>.ToInt(value);
                    write += sizeof(byte);
                    if (--writeCount == 0)
                    {
                        FillSize4(write, count);
                        return;
                    }
                }
                FillSize4(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize4(count * sizeof(byte) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumByteCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumByteCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumByteCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumByteCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumByteArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize4(array.Length * sizeof(byte) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(byte*)write = AutoCSer.Metadata.EnumCast<T, byte>.ToInt(value);
                    write += sizeof(byte);
                }
                FillSize4(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumByteArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumByteArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumByteArray(array);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public unsafe sealed partial class BinarySerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">枚举值序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumSByteMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumSByteCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize4(count * sizeof(sbyte) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(sbyte*)write = AutoCSer.Metadata.EnumCast<T, sbyte>.ToInt(value);
                    write += sizeof(sbyte);
                    if (--writeCount == 0)
                    {
                        FillSize4(write, count);
                        return;
                    }
                }
                FillSize4(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize4(count * sizeof(sbyte) + sizeof(int));
            }
            else Stream.Write((int)0);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumSByteCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumSByteCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumSByteCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumSByteCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumSByteArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize4(array.Length * sizeof(sbyte) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(sbyte*)write = AutoCSer.Metadata.EnumCast<T, sbyte>.ToInt(value);
                    write += sizeof(sbyte);
                }
                FillSize4(write, array.Length);
            }
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumSByteArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumSByteArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumSByteArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumSByteArray(array);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumLongDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberSigned())
            {
                long intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        long intValue = enumInts.Long[index];
                        intValue |= enumInts.Long[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.Long[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumLongDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(long), false);
            long* data = enumInts.Long;
            foreach (T value in enumValues) *(long*)data++ = AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUIntDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberUnsigned())
            {
                uint intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        uint intValue = enumInts.UInt[index];
                        intValue |= enumInts.UInt[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.UInt[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumUIntDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(uint), false);
            uint* data = enumInts.UInt;
            foreach (T value in enumValues) *(uint*)data++ = AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumIntDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberSigned())
            {
                int intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        int intValue = enumInts.Int[index];
                        intValue |= enumInts.Int[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.Int[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumIntDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(int), false);
            int* data = enumInts.Int;
            foreach (T value in enumValues) *(int*)data++ = AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUShortDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberUnsigned())
            {
                ushort intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ushort intValue = enumInts.UShort[index];
                        intValue |= enumInts.UShort[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.UShort[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumUShortDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(ushort), false);
            ushort* data = enumInts.UShort;
            foreach (T value in enumValues) *(ushort*)data++ = AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumShortDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberSigned())
            {
                short intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        short intValue = enumInts.Short[index];
                        intValue |= enumInts.Short[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.Short[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumShortDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(short), false);
            short* data = enumInts.Short;
            foreach (T value in enumValues) *(short*)data++ = AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumByteDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberUnsigned())
            {
                byte intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        byte intValue = enumInts.Byte[index];
                        intValue |= enumInts.Byte[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.Byte[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumByteDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(byte), false);
            byte* data = enumInts.Byte;
            foreach (T value in enumValues) *(byte*)data++ = AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value);
        }
    }
}

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumSByteDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberSigned())
            {
                sbyte intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        sbyte intValue = enumInts.SByte[index];
                        intValue |= enumInts.SByte[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.SByte[index];
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumSByteDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(sbyte), false);
            sbyte* data = enumInts.SByte;
            foreach (T value in enumValues) *(sbyte*)data++ = AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref long[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref uint[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref int[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref ushort[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref short[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref byte[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref sbyte[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref bool[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref DateTime[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(long[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (long value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(uint[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (uint value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(int[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (int value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ushort[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (ushort value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(short[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (short value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(byte[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (byte value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(sbyte[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (sbyte value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(bool[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (bool value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(DateTime[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (DateTime value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumLongDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberSigned())
            {
                long intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        long intValue = enumInts.Long[index];
                        intValue |= enumInts.Long[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.Long[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumLongDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(long), false);
            long* data = enumInts.Long;
            foreach (T value in enumValues) *(long*)data++ = AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUIntDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberUnsigned())
            {
                uint intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        uint intValue = enumInts.UInt[index];
                        intValue |= enumInts.UInt[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.UInt[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumUIntDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(uint), false);
            uint* data = enumInts.UInt;
            foreach (T value in enumValues) *(uint*)data++ = AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumIntDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberSigned())
            {
                int intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        int intValue = enumInts.Int[index];
                        intValue |= enumInts.Int[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.Int[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumIntDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(int), false);
            int* data = enumInts.Int;
            foreach (T value in enumValues) *(int*)data++ = AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUShortDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberUnsigned())
            {
                ushort intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ushort intValue = enumInts.UShort[index];
                        intValue |= enumInts.UShort[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.UShort[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumUShortDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(ushort), false);
            ushort* data = enumInts.UShort;
            foreach (T value in enumValues) *(ushort*)data++ = AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumShortDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberSigned())
            {
                short intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        short intValue = enumInts.Short[index];
                        intValue |= enumInts.Short[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.Short[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumShortDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(short), false);
            short* data = enumInts.Short;
            foreach (T value in enumValues) *(short*)data++ = AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumByteDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberUnsigned())
            {
                byte intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        byte intValue = enumInts.Byte[index];
                        intValue |= enumInts.Byte[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.Byte[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumByteDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(byte), false);
            byte* data = enumInts.Byte;
            foreach (T value in enumValues) *(byte*)data++ = AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumSByteDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberSigned())
            {
                sbyte intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        sbyte intValue = enumInts.SByte[index];
                        intValue |= enumInts.SByte[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.SByte[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumSByteDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(sbyte), false);
            sbyte* data = enumInts.SByte;
            foreach (T value in enumValues) *(sbyte*)data++ = AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value);
        }
    }
}

#endif