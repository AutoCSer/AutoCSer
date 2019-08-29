using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 基本类型反序列化函数
    /// </summary>
    internal sealed class DeSerializeMethod : Attribute { }

    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public unsafe sealed partial class DeSerializer
    {
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref bool value)
        {
            value = *(bool*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref bool[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref bool? value)
        {
            value = *(bool*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref bool?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length >> 1)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref byte value)
        {
            value = *(byte*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref byte[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (((length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref LeftArray<byte> value)
        {
            int length = *(int*)Read;
            if (length == 0)
            {
                value.Length = 0;
                Read += sizeof(int);
            }
            else
            {
                if (((length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    byte[] array = new byte[length];
                    Read = DeSerialize(Read + sizeof(int), array);
                    value.Set(array, length);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref byte? value)
        {
            value = *(byte*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref byte?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref sbyte value)
        {
            value = (sbyte)*(int*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref sbyte[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (((length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref sbyte? value)
        {
            value = (sbyte)*(int*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref sbyte?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize((sbyte*)Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref short value)
        {
            value = (short)*(int*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref short[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length * sizeof(short)) + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref short? value)
        {
            value = (short)*(int*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref short?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ushort value)
        {
            value = *(ushort*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ushort[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (((length * sizeof(ushort) + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ushort? value)
        {
            value = *(ushort*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ushort?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void deSerialize(ref int value)
        {
            value = *(int*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref int[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((length + 1) * sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref int? value)
        {
            value = *(int*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref int?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref uint value)
        {
            value = *(uint*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref uint[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((length + 1) * sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref uint? value)
        {
            value = *(uint*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref uint?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref long value)
        {
            value = *(long*)Read;
            Read += sizeof(long);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref long[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(long) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref long? value)
        {
            value = *(long*)Read;
            Read += sizeof(long);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref long?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ulong value)
        {
            value = *(ulong*)Read;
            Read += sizeof(ulong);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ulong[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(ulong) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ulong? value)
        {
            value = *(ulong*)Read;
            Read += sizeof(ulong);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ulong?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref float value)
        {
            value = *(float*)Read;
            Read += sizeof(float);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref float[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(float) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref float? value)
        {
            value = *(float*)Read;
            Read += sizeof(float);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref float?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref double value)
        {
            value = *(double*)Read;
            Read += sizeof(double);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref double[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(double) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref double? value)
        {
            value = *(double*)Read;
            Read += sizeof(double);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref double?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref decimal value)
        {
            value = *(decimal*)Read;
            Read += sizeof(decimal);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref decimal[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(decimal) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref decimal? value)
        {
            value = *(decimal*)Read;
            Read += sizeof(decimal);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref decimal?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref char value)
        {
            value = *(char*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref char[] value)
        {
            int length = deSerializeArray(ref value);
            if ((length & 1) == 0)
            {
                if (length != 0)
                {
                    int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                    if (dataLength <= (int)(End - Read))
                    {
                        if (createArray(ref value, length >> 1))
                        {
                            fixed (char* valueFixed = value) AutoCSer.Memory.CopyNotNull(Read + sizeof(int), valueFixed, length);
                            Read += dataLength;
                        }
                        return;
                    }
                }
                else return;
            }
            else
            {
                length >>= 1;
                int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        fixed (char* valueFixed = value)
                        {
                            byte* read = DeSerialize(Read, End, valueFixed, length, lengthSize);
                            if (read != null)
                            {
                                Read = read;
                                return;
                            }
                        }
                    }
                    else return;
                }
            }
            State = DeSerializeState.IndexOutOfRange;
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref char? value)
        {
            value = *(char*)Read;
            Read += sizeof(int);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref char?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref DateTime value)
        {
            value = *(DateTime*)Read;
            Read += sizeof(DateTime);
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref DateTime[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(DateTime) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref DateTime? value)
        {
            value = *(DateTime*)Read;
            Read += sizeof(DateTime);
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref DateTime?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref Guid value)
        {
            value = *(Guid*)Read;
            Read += sizeof(Guid);
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref Guid[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if (length * sizeof(Guid) + sizeof(int) <= (int)(End - Read))
                {
                    if (createArray(ref value, length)) Read = DeSerialize(Read + sizeof(int), value);
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref Guid? value)
        {
            value = *(Guid*)Read;
            Read += sizeof(Guid);
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref Guid?[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                if ((((length + (31 + 32)) >> 5) << 2) <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        Read = DeSerialize(Read + sizeof(int), value);
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        ///// <summary>
        ///// 字符串反序列化
        ///// </summary>
        ///// <param name="value">字符串</param>
        //[DeSerializeMethod]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        //private void deSerialize(ref string value)
        //{
        //    if (CheckPoint(ref value))
        //    {
        //        int length = *(int*)Read;
        //        if ((length & 1) == 0)
        //        {
        //            if (length != 0)
        //            {
        //                int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
        //                if (dataLength <= (int)(end - Read))
        //                {
        //                    value = new string((char*)(Read + sizeof(int)), 0, length >> 1);
        //                    if (isReferenceMember) points.Add((int)(start - Read), value);
        //                    Read += dataLength;
        //                }
        //                else State = DeSerializeState.IndexOutOfRange;
        //            }
        //            else
        //            {
        //                value = string.Empty;
        //                Read += sizeof(int);
        //            }
        //        }
        //        else
        //        {
        //            int dataLength = ((length >>= 1) + (3 + sizeof(int))) & (int.MaxValue - 3);
        //            if (dataLength <= (int)(end - Read))
        //            {
        //                value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
        //                if (isReferenceMember) points.Add((int)(start - Read), value);
        //                fixed (char* valueFixed = value)
        //                {
        //                    char* write = valueFixed;
        //                    byte* readStart = Read + sizeof(int), readEnd = readStart + length;
        //                    do
        //                    {
        //                        *write++ = (char)*readStart++;
        //                    }
        //                    while (readStart != readEnd);
        //                }
        //                Read += dataLength;
        //            }
        //            else State = DeSerializeState.IndexOutOfRange;
        //        }
        //    }
        //}
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref string value)
        {
            if (CheckPoint(ref value))
            {
                int length = *(int*)Read;
                if ((length & 1) == 0)
                {
                    if (length != 0)
                    {
                        int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                        if (dataLength <= (int)(End - Read))
                        {
                            value = new string((char*)(Read + sizeof(int)), 0, length >> 1);
                            if (isReferenceMember) points.Set((int)(start - Read), value);
                            Read += dataLength;
                        }
                        else State = DeSerializeState.IndexOutOfRange;
                    }
                    else
                    {
                        value = string.Empty;
                        Read += sizeof(int);
                    }
                }
                else
                {
                    length >>= 1;
                    int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                    if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                    {
                        value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                        if (isReferenceMember) points.Set((int)(start - Read), value);
                        fixed (char* valueFixed = value)
                        {
                            byte* read = DeSerialize(Read, End, valueFixed, length, lengthSize);
                            if (read != null)
                            {
                                Read = read;
                                return;
                            }
                        }
                    }
                    State = DeSerializeState.IndexOutOfRange;
                }
            }
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref string[] value)
        {
            int length = deSerializeArray(ref value);
            if (length != 0)
            {
                int mapLength = ((length + (31 + 32)) >> 5) << 2;
                if (mapLength <= (int)(End - Read))
                {
                    if (createArray(ref value, length))
                    {
                        DeSerializeArrayMap arrayMap = new DeSerializeArrayMap(Read + sizeof(int));
                        Read += mapLength;
                        for (int index = 0; index != value.Length; ++index)
                        {
                            if (arrayMap.Next() == 0) value[index] = null;
                            else
                            {
                                deSerialize(ref value[index]);
                                if (State != DeSerializeState.Success) return;
                            }
                        }
                        if (Read > End) State = DeSerializeState.IndexOutOfRange;
                    }
                }
                else State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        [DeSerializeMethod]
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(ref SubString value)
        {
            //string stringValue = null;
            //if ((Read = DeSerialize(Read, end, ref stringValue)) == null) State = DeSerializeState.IndexOutOfRange;
            //else value.Set(stringValue, 0, stringValue.Length);
            int length = *(int*)Read;
            if ((length & 1) == 0)
            {
                if (length != 0)
                {
                    int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                    if (dataLength <= (int)(End - Read))
                    {
                        length >>= 1;
                        value.Set(new string((char*)(Read + sizeof(int)), 0, length), 0, length);
                        Read += dataLength;
                    }
                    else State = DeSerializeState.IndexOutOfRange;
                }
                else
                {
                    value.Set(string.Empty, 0, 0);
                    Read += sizeof(int);
                }
            }
            else
            {
                length >>= 1;
                int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                {
                    value.Set(AutoCSer.Extension.StringExtension.FastAllocateString(length), 0, length);
                    fixed (char* valueFixed = value.String)
                    {
                        byte* read = DeSerialize(Read, End, valueFixed, length, lengthSize);
                        if (read != null)
                        {
                            Read = read;
                            return;
                        }
                    }
                }
                State = DeSerializeState.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 类型信息反序列化
        /// </summary>
        /// <param name="value">类型信息</param>
        [DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref Type value)
        {
            if (CheckPoint(ref value))
            {
                RemoteType remoteType = default(RemoteType);
                int point = (int)(start - Read);
                TypeDeSerializer<RemoteType>.DeSerialize(this, ref remoteType);
                if (State == DeSerializeState.Success)
                {
                    if (remoteType.TryGet(out value))
                    {
                        if (isReferenceMember) points.Set(ref point, value);
                    }
                    else State = DeSerializeState.ErrorType;
                }
            }
        }

        /// <summary>
        /// 基本类型反序列化函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> deSerializeMethods;
        /// <summary>
        /// 获取基本类型反序列化函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>反序列化函数</returns>
        internal static MethodInfo GetDeSerializeMethod(Type type)
        {
            MethodInfo method;
            if (deSerializeMethods.TryGetValue(type, out method))
            {
                deSerializeMethods.Remove(type);
                return method;
            }
            return null;
        }
    }
}
