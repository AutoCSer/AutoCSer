using System;
/*ulong,ULong,GetSize,FillSize;long,Long,GetSize,FillSize;uint,UInt,GetSize,FillSize;int,Int,GetSize,FillSize;ushort,UShort,GetSize4,FillSize2;short,Short,GetSize4,FillSize2;byte,Byte,GetSize4,FillSize4;sbyte,SByte,GetSize4,FillSize4*/

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
        internal static void EnumULongMember<T>(BinarySerializer binarySerializer, T value) where T : struct, IConvertible
        {
            binarySerializer.Stream.Data.Write(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="collection">枚举集合序列化</param>
        private unsafe void structEnumULongCollection<T, CT>(CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            int count = collection.Count;
            if (count > 0)
            {
                int writeCount = count, size = GetSize(count * sizeof(ulong) + sizeof(int));
                byte* start = Stream.GetBeforeMove(size), write = start + sizeof(int);
                *(int*)start = count;
                foreach (T value in collection)
                {
                    *(ulong*)write = AutoCSer.Metadata.EnumCast<T, ulong>.ToInt(value);
                    write += sizeof(ulong);
                    if (--writeCount == 0)
                    {
                        FillSize(write, count);
                        return;
                    }
                }
                FillSize(write, count -= writeCount);
                *(int*)start = count;
                Stream.Data.CurrentIndex -= size - GetSize(count * sizeof(ulong) + sizeof(int));
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
        internal static void StructEnumULongCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            binarySerializer.structEnumULongCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CT"></typeparam>
        /// <param name="binarySerializer">x</param>
        /// <param name="collection">枚举集合序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassEnumULongCollection<T, CT>(BinarySerializer binarySerializer, CT collection)
            where CT : System.Collections.Generic.ICollection<T>
        {
            if (binarySerializer.CheckPoint(collection)) binarySerializer.structEnumULongCollection<T, CT>(collection);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组序列化</param>
        private unsafe void enumULongArray<T>(T[] array)
        {
            if (checkPoint(array))
            {
                byte* write = Stream.GetBeforeMove(GetSize(array.Length * sizeof(ulong) + sizeof(int)));
                *(int*)write = array.Length;
                write += sizeof(int);
                foreach (T value in array)
                {
                    *(ulong*)write = AutoCSer.Metadata.EnumCast<T, ulong>.ToInt(value);
                    write += sizeof(ulong);
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
        internal static void EnumULongArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.enumULongArray(array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void EnumULongArrayMember<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            if (array == null) binarySerializer.Stream.Write(NullValue);
            else binarySerializer.enumULongArray(array);
        }
    }
}
