using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    public static unsafe partial class Memory
    {
        /// <summary>
        /// 填充整数(用Buffer.BlockCopy可能比指针快)
        /// </summary>
        /// <param name="src">串起始地址</param>
        /// <param name="count">整数数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Clear(ulong* src, int count)
        {
            if (src == null) throw new ArgumentNullException();
            if (count <= 0) throw new IndexOutOfRangeException();
            ClearUnsafe(src, count);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool equal(this byte[] left, byte[] right)
        {
            if (left == null) return right == null;
            return right != null && (left.Equals(right) || EqualNotNull(left, right));
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool EqualNotNull(byte[] left, byte[] right)
        {
            return left.Length == right.Length && EqualNotNull(left, right, left.Length);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool EqualNotNull(byte[] left, byte[] right, int count)
        {
            fixed (byte* leftFixed = left, rightFixed = right) return EqualNotNull(leftFixed, rightFixed, count);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool EqualNotNull(byte[] left, void* right, int count)
        {
            fixed (byte* leftFixed = left) return EqualNotNull(leftFixed, (byte*)right, count);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否相等</returns>
        public static bool equal(this SubArray<byte> left, ref SubArray<byte> right)
        {
            if (left.Length == 0) return right.Length == 0 && !((left.Array == null) ^ (right.Array == null));
            if (right.Length == left.Length)
            {
                if (Object.ReferenceEquals(left.Array, right.Array) && left.Start == right.Start) return true;
                fixed (byte* leftFixed = left.Array, rightFixed = right.Array)
                {
                    return Memory.EqualNotNull(leftFixed + left.Start, rightFixed + right.Start, left.Length);
                }
            }
            return false;
        }
        /// <summary>
        /// 查找字节
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int indexOfNotNull(this byte[] data, byte value)
        {
            return data.Length > 0 ? IndexOf(data, value) : -1;
        }
        /// <summary>
        /// 查找字节
        /// </summary>
        /// <param name="data">长度不为 0 的字节数组</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int IndexOf(byte[] data, byte value)
        {
            fixed (byte* dataFixed = data)
            {
                byte* valueData = Find(dataFixed, dataFixed + data.Length, value);
                return valueData != null ? (int)(valueData - dataFixed) : -1;
            }
        }
        /// <summary>
        /// 查找字节,数据长度不能为0
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null,长度不为 0</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* Find(byte* start, byte* end, byte value)
        {
            do
            {
                if (*start == value) return start;
            }
            while (++start != end);
            return null;
        }
        /// <summary>
        /// 查找最后一个字节,数据长度不能为0
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte* FindLast(byte* start, byte* end, byte value)
        {
            do
            {
                if (*--end == value) return end;
            }
            while (start != end);
            return null;
        }
        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string toStringNotNull(this byte[] data)
        {
            if (data.Length == 0) return string.Empty;
            fixed (byte* dataFixed = data) return ToString(dataFixed, data.Length);
        }
        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string toStringNotNull(this byte[] data, int start, int count)
        {
            if (count == 0) return string.Empty;
            fixed (byte* dataFixed = data) return ToString(dataFixed + start, count);
        }
        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static string ToString(byte* data, int count)
        {
            string value = AutoCSer.Extension.StringExtension.FastAllocateString(count);
            byte* end = data + count;
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                do
                {
                    *write++ = (char)*data++;
                }
                while (data != end);
            }
            return value;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void toLowerNotNull(this byte[] value)
        {
            fixed (byte* valueFixed = value) ToLowerNotNull(valueFixed, valueFixed + value.Length);
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ToLowerNotNull(byte* start, byte* end)
        {
            while (start != end)
            {
                if ((uint)(*start - 'A') < 26) *start |= 0x20;
                ++start;
            }
        }
        /// <summary>
        /// 复制字节数组(不足8字节按8字节算)
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SimpleCopyNotNull64(byte[] source, byte* destination, int length)
        {
            fixed (byte* sourceFixed = source) SimpleCopyNotNull64(sourceFixed, destination, length);
        }
        /// <summary>
        /// 填充二进制位
        /// </summary>
        /// <param name="data">数据起始位置,不能为null</param>
        /// <param name="start">起始二进制位,不能越界</param>
        /// <param name="count">二进制位数量,不能越界</param>
        internal static void FillBits(byte* data, int start, int count)
        {
            data += (start >> 6) << 3;
            if ((start &= ((sizeof(ulong) << 3) - 1)) != 0)
            {
                int high = (sizeof(ulong) << 3) - start;
                if ((count -= high) >= 0)
                {
                    *(ulong*)data |= ulong.MaxValue << start;
                    data += sizeof(ulong);
                }
                else
                {
                    *(ulong*)data |= (ulong.MaxValue >> (start - count)) << start;
                    return;
                }
            }
            if ((start = count >> 6) != 0) Fill((ulong*)data, ulong.MaxValue, start);
            if ((count = -count & ((sizeof(ulong) << 3) - 1)) != 0) *(ulong*)(data + (start << 3)) |= ulong.MaxValue >> count;
        }
        /// <summary>
        /// 计算 32 位 HASH 值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int getHashCode(this byte[] data)
        {
            if (data == null || data.Length == 0) return 0;
            fixed (byte* dataFixed = data) return GetHashCode(dataFixed, data.Length);
        }
    }
}
