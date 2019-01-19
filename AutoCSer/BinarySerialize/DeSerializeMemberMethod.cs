using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 基本类型反序列化函数
    /// </summary>
    internal sealed class DeSerializeMemberMethod : Attribute { }
    /// <summary>
    /// 基本类型反序列化函数
    /// </summary>
    internal sealed class DeSerializeMemberMapMethod : Attribute { }

    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public unsafe sealed partial class DeSerializer
    {
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref bool value)
        {
            value = *(bool*)Read++;
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref bool[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref bool? value)
        {
            if (*Read == 0) value = null;
            else value = *Read == 2;
            ++Read;
        }
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref bool?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref byte value)
        {
            value = *(byte*)Read++;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref byte[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref byte? value)
        {
            if (*(Read + sizeof(byte)) == 0) value = *(byte*)Read;
            else value = null;
            Read += sizeof(ushort);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref byte?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref sbyte value)
        {
            value = *(sbyte*)Read++;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref sbyte[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref sbyte? value)
        {
            if (*(Read + sizeof(byte)) == 0) value = *(sbyte*)Read;
            else value = null;
            Read += sizeof(ushort);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref sbyte?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref short value)
        {
            value = *(short*)Read;
            Read += sizeof(short);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref short[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref short? value)
        {
            if (*(ushort*)(Read + sizeof(ushort)) == 0) value = *(short*)Read;
            else value = null;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref short?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ushort value)
        {
            value = *(ushort*)Read;
            Read += sizeof(ushort);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ushort[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ushort? value)
        {
            if (*(ushort*)(Read + sizeof(ushort)) == 0) value = *(ushort*)Read;
            else value = null;
            Read += sizeof(int);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ushort?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref int? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(int*)(Read += sizeof(int));
                Read += sizeof(int);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref int[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref int?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref uint? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(uint*)(Read += sizeof(int));
                Read += sizeof(uint);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref uint[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref uint?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref long? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(long*)(Read += sizeof(int));
                Read += sizeof(long);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref long[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref long?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ulong? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(ulong*)(Read += sizeof(int));
                Read += sizeof(ulong);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ulong[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref ulong?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref float? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(float*)(Read += sizeof(int));
                Read += sizeof(float);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref float[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref float?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref double? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(double*)(Read += sizeof(int));
                Read += sizeof(double);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref double[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref double?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref decimal? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(decimal*)(Read += sizeof(int));
                Read += sizeof(decimal);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref decimal[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="value">数值</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref decimal?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref char value)
        {
            value = *(char*)Read;
            Read += sizeof(char);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref char[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref char? value)
        {
            if (*(ushort*)(Read + sizeof(char)) == 0) value = *(char*)Read;
            else value = null;
            Read += sizeof(int);
        }
        /// <summary>
        /// 字符反序列化
        /// </summary>
        /// <param name="value">字符</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref char?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref DateTime? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(DateTime*)(Read += sizeof(int));
                Read += sizeof(DateTime);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref DateTime[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 时间反序列化
        /// </summary>
        /// <param name="value">时间</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref DateTime?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref Guid? value)
        {
            if (*(int*)Read == 0)
            {
                value = *(Guid*)(Read += sizeof(int));
                Read += sizeof(Guid);
            }
            else
            {
                Read += sizeof(int);
                value = null;
            }
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref Guid[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// Guid反序列化
        /// </summary>
        /// <param name="value">Guid</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref Guid?[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref string value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="value">字符串</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref string[] value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }
        /// <summary>
        /// 类型信息反序列化
        /// </summary>
        /// <param name="value">类型信息</param>
        [DeSerializeMemberMethod]
        [DeSerializeMemberMapMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void memberDeSerialize(ref Type value)
        {
            if (*(int*)Read == Serializer.NullValue)
            {
                Read += sizeof(int);
                value = null;
            }
            else deSerialize(ref value);
        }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> memberDeSerializeMethods;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetMemberDeSerializeMethod(Type type)
        {
            MethodInfo method;
            return memberDeSerializeMethods.TryGetValue(type, out method) ? method : null;
        }
        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> memberMapDeSerializeMethods;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetMemberMapDeSerializeMethod(Type type)
        {
            MethodInfo method;
            return memberMapDeSerializeMethods.TryGetValue(type, out method) ? method : null;
        }
    }
}
