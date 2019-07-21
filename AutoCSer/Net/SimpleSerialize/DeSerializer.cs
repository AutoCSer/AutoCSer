using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Net.SimpleSerialize
{

    /// <summary>
    /// 简单反序列化
    /// </summary>
    internal unsafe sealed partial class DeSerializer
    {
#if NOJIT
        /// <summary>
        /// 当前读取位置
        /// </summary>
        internal byte* Read;
        /// <summary>
        /// 结束位置
        /// </summary>
        internal byte* End;
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref bool value)
        {
            value = *(bool*)Read++;
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
            if (*Read == 0) value = null;
            else value = *Read == 2;
            ++Read;
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
            Read += sizeof(byte);
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
            if (*(Read + sizeof(byte)) == 0) value = *(byte*)Read;
            else value = null;
            Read += sizeof(ushort);
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
            value = *(sbyte*)Read;
            Read += sizeof(sbyte);
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
            if (*(Read + sizeof(byte)) == 0) value = *(sbyte*)Read;
            else value = null;
            Read += sizeof(ushort);
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
            value = *(short*)Read;
            Read += sizeof(short);
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
            if (*(ushort*)(Read + sizeof(ushort)) == 0) value = *(short*)Read;
            else value = null;
            Read += sizeof(int);
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
            Read += sizeof(ushort);
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
            if (*(ushort*)(Read + sizeof(ushort)) == 0) value = *(ushort*)Read;
            else value = null;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref int? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(int*)(Read + sizeof(int));
                Read += sizeof(int) * 2;
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref uint value)
        {
            value = *(uint*)Read;
            Read += sizeof(uint);
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
            if (*(int*)Read == 0)
            {
                value = *(uint*)(Read + sizeof(int));
                Read += sizeof(uint) * 2;
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref long? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(long*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(long));
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref ulong? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(ulong*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(ulong));
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref float? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(float*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(float));
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref double? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(double*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(double));
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref decimal? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(decimal*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(decimal));
            }
            else
            {
                value = null;
                Read += sizeof(int);
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
            Read += sizeof(char);
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
            if (*(ushort*)(Read + sizeof(char)) == 0) value = *(char*)Read;
            else value = null;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref DateTime? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(DateTime*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(DateTime));
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMethod]
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref Guid? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(Guid*)(Read + sizeof(int));
                Read += (sizeof(int) + sizeof(Guid));
            }
            else
            {
                value = null;
                Read += sizeof(int);
            }
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="value">字符串</param>

        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(ref string value)
        {
            if (Read != null)
            {
                if (*(int*)Read == BinarySerialize.Serializer.NullValue)
                {
                    value = null;
                    Read += sizeof(int);
                    return;
                }
                int length = *(int*)Read;
                if ((length & 1) == 0)
                {
                    if (length != 0)
                    {
                        int ReadLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                        if (ReadLength <= (int)(End - Read))
                        {
                            value = new string((char*)(Read + sizeof(int)), 0, length >> 1);
                            Read += ReadLength;
                            return;
                        }
                    }
                    else
                    {
                        value = string.Empty;
                        Read += sizeof(int);
                        return;
                    }
                }
                else
                {
                    length >>= 1;
                    int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                    if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Read))
                    {
                        value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                        fixed (char* valueFixed = value) Read = BinarySerialize.DeSerializer.DeSerialize(Read, End, valueFixed, length, lengthSize);
                        return;
                    }
                }
                Read = null;
            }
        }

        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void enumByte<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, byte>.FromInt(*Read++);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void enumSByte<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, sbyte>.FromInt(*(sbyte*)Read++);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void enumShort<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, short>.FromInt(*(short*)Read);
            Read += sizeof(short);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void enumUShort<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, ushort>.FromInt(*(ushort*)Read);
            Read += sizeof(ushort);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void EnumInt<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, int>.FromInt(*(int*)Read);
            Read += sizeof(int);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void EnumUInt<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, uint>.FromInt(*(uint*)Read);
            Read += sizeof(uint);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void EnumLong<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, long>.FromInt(*(long*)Read);
            Read += sizeof(long);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void EnumULong<valueType>(ref valueType value)
        {
            value = Emit.EnumCast<valueType, ulong>.FromInt(*(ulong*)Read);
            Read += sizeof(ulong);
        }
#else
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">逻辑值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref bool value)
        {
            value = *(bool*)data;
            return data + 1;
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">逻辑值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref bool? value)
        {
            if (*data == 0) value = null;
            else value = *data == 2;
            return data + 1;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref byte value)
        {
            value = *(byte*)data;
            return data + sizeof(byte);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref byte? value)
        {
            if (*(data + sizeof(byte)) == 0) value = *(byte*)data;
            else value = null;
            return data + sizeof(ushort);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref sbyte value)
        {
            value = *(sbyte*)data;
            return data + sizeof(sbyte);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref sbyte? value)
        {
            if (*(data + sizeof(byte)) == 0) value = *(sbyte*)data;
            else value = null;
            return data + sizeof(ushort);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref short value)
        {
            value = *(short*)data;
            return data + sizeof(short);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref short? value)
        {
            if (*(ushort*)(data + sizeof(ushort)) == 0) value = *(short*)data;
            else value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref ushort value)
        {
            value = *(ushort*)data;
            return data + sizeof(ushort);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref ushort? value)
        {
            if (*(ushort*)(data + sizeof(ushort)) == 0) value = *(ushort*)data;
            else value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref int value)
        {
            value = *(int*)data;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref int? value)
        {
            if (*(int*)data == 0)
            {
                value = *(int*)(data + sizeof(int));
                return data + sizeof(int) * 2;
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref uint value)
        {
            value = *(uint*)data;
            return data + sizeof(uint);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref uint? value)
        {
            if (*(int*)data == 0)
            {
                value = *(uint*)(data + sizeof(int));
                return data + sizeof(uint) * 2;
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref long value)
        {
            value = *(long*)data;
            return data + sizeof(long);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref long? value)
        {
            if (*(int*)data == 0)
            {
                value = *(long*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(long));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref ulong value)
        {
            value = *(ulong*)data;
            return data + sizeof(ulong);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref ulong? value)
        {
            if (*(int*)data == 0)
            {
                value = *(ulong*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(ulong));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref float value)
        {
            value = *(float*)data;
            return data + sizeof(float);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref float? value)
        {
            if (*(int*)data == 0)
            {
                value = *(float*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(float));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref double value)
        {
            value = *(double*)data;
            return data + sizeof(double);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref double? value)
        {
            if (*(int*)data == 0)
            {
                value = *(double*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(double));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref decimal value)
        {
            value = *(decimal*)data;
            return data + sizeof(decimal);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref decimal? value)
        {
            if (*(int*)data == 0)
            {
                value = *(decimal*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(decimal));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">字符</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref char value)
        {
            value = *(char*)data;
            return data + sizeof(char);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">字符</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref char? value)
        {
            if (*(ushort*)(data + sizeof(char)) == 0) value = *(char*)data;
            else value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref DateTime value)
        {
            value = *(DateTime*)data;
            return data + sizeof(DateTime);
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">时间</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref DateTime? value)
        {
            if (*(int*)data == 0)
            {
                value = *(DateTime*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(DateTime));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref Guid value)
        {
            value = *(Guid*)data;
            return data + sizeof(Guid);
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">Guid</param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* data, ref Guid? value)
        {
            if (*(int*)data == 0)
            {
                value = *(Guid*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(Guid));
            }
            value = null;
            return data + sizeof(int);
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value">字符串</param>
        /// <param name="end"></param>
        /// <returns></returns>
        [DeSerializeMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static byte* deSerialize(byte* start, ref string value, byte* end)
        {
            if (start != null)
            {
                if (*(int*)start == BinarySerialize.Serializer.NullValue)
                {
                    value = null;
                    return start + sizeof(int);
                }
                int length = *(int*)start;
                if ((length & 1) == 0)
                {
                    if (length != 0)
                    {
                        int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                        if (dataLength <= (int)(end - start))
                        {
                            value = new string((char*)(start + sizeof(int)), 0, length >> 1);
                            return start + dataLength;
                        }
                    }
                    else
                    {
                        value = string.Empty;
                        return start + sizeof(int);
                    }
                }
                else
                {
                    length >>= 1;
                    int lengthSize = (length <= byte.MaxValue ? 1 : (length <= ushort.MaxValue ? sizeof(ushort) : sizeof(int)));
                    if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(end - start))
                    {
                        value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                        fixed (char* valueFixed = value) return BinarySerialize.DeSerializer.DeSerialize(start, end, valueFixed, length, lengthSize);
                    }
                }
            }
            return null;
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
                if (type != null) StructGenericType.Get(type).SimpleDeSerializeCompile();
            }
        }
        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> deSerializeMethods;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetDeSerializeMethod(Type type)
        {
            MethodInfo method;
            return deSerializeMethods.TryGetValue(type, out method) ? method : null;
        }
        static DeSerializer()
        {
            deSerializeMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
#if NOJIT
            foreach (MethodInfo method in typeof(DeSerializer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
#else
            foreach (MethodInfo method in typeof(DeSerializer).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
#endif
            {
                Type parameterType = null;
                if (method.IsDefined(typeof(DeSerializeMethod), false))
                {
#if NOJIT
                    if (parameterType == null) parameterType = method.GetParameters()[0].ParameterType.GetElementType();
#else
                    if (parameterType == null) parameterType = method.GetParameters()[1].ParameterType.GetElementType();
#endif
                    deSerializeMethods.Add(parameterType, method);
                }
            }
        }
    }
}
