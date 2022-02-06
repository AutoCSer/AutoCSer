using System;
/*ulong,ULong,GetSize;long,Long,GetSize;uint,UInt,GetSize;int,Int,GetSize;ushort,UShort,GetSize4;short,Short,GetSize4;byte,Byte,GetSize4;sbyte,SByte,GetSize4*/

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
        internal static void EnumULongMember<T>(BinaryDeSerializer deSerializer, ref T value)
        {
            value = AutoCSer.Metadata.EnumCast<T, ulong>.FromInt(*(ulong*)deSerializer.Read);
            deSerializer.Read += sizeof(ulong);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
        private void enumULongArray<T>(ref T[] array)
        {
            int length = deSerializeArray(ref array);
            if (length != 0)
            {
                int dataLength = AutoCSer.BinarySerializer.GetSize(length * sizeof(ulong) + (sizeof(int)));
                if (dataLength <= (int)(End - Read))
                {
                    if (createArray(ref array, length))
                    {
                        byte* data = Read + sizeof(int);
                        for (int index = 0; index != array.Length; data += sizeof(ulong)) array[index++] = AutoCSer.Metadata.EnumCast<T, ulong>.FromInt(*(ulong*)data);
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
        private void enumULongArrayMember<T>(ref T[] array)
        {
            if (*(int*)Read == BinarySerializer.NullValue)
            {
                Read += sizeof(int);
                array = null;
            }
            else enumULongArray(ref array);
        }
        /// <summary>
        /// 枚举数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumULongArrayMember<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumULongArrayMember(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumULongArray<T>(BinaryDeSerializer deSerializer, ref T[] array)
        {
            deSerializer.enumULongArray(ref array);
        }
    }
}
