using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 内存数据段扩展操作
    /// </summary>
    internal static unsafe class Span
    {
        /// <summary>
        /// 转换为数组子串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static SubArray<byte> AsSpan(this byte[] value)
        {
            return new SubArray<byte>(value);
        }
        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyTo(this Span<byte> source, Span<byte> destination)
        {
            AutoCSer.Memory.Common.CopyNotNull(source.Data, destination.Data, source.Size);
        }
        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Clear(this Span<byte> source)
        {
            AutoCSer.Memory.Common.Clear((byte*)source.Data, source.Size);
        }
        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        internal static void CopyTo(this SubArray<byte> source, Span<byte> destination)
        {
            fixed (byte* sourceFixed = source.GetFixedBuffer())
            {
                CopyTo(new Span<byte>(sourceFixed + source.Start, source.Length), destination);
            }
        }
        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyTo(this Span<byte> source, SubArray<byte> destination)
        {
            fixed (byte* destinationFixed = destination.GetFixedBuffer())
            {
                CopyTo(source, new Span<byte>(destinationFixed + destination.Start, destination.Length));
            }
        }

        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyTo(this Span<char> source, Span<char> destination)
        {
#if DEBUG
            if (((long)source.Size << 1) > int.MaxValue) throw new Exception(source.Size.toString() + " * 2 > int.MaxValue");
            if (source.Size > destination.Size) throw new Exception(source.Size.toString() + " > " + destination.Size.toString());
#endif
            AutoCSer.Memory.Common.CopyNotNull(source.Data, destination.Data, source.Size << 1);
        }

        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyTo(this Span<int> source, Span<int> destination)
        {
#if DEBUG
            if (((long)source.Size << 2) > int.MaxValue) throw new Exception(source.Size.toString() + " * 4 > int.MaxValue");
            if (source.Size > destination.Size) throw new Exception(source.Size.toString() + " > " + destination.Size.toString());
#endif
            AutoCSer.Memory.Common.CopyNotNull(source.Data, destination.Data, source.Size << 2);
        }
        /// <summary>
        /// 复制内存数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        internal static void CopyTo(this SubArray<int> source, Span<int> destination)
        {
            fixed (int* sourceFixed = source.GetFixedBuffer())
            {
                CopyTo(new Span<int>(sourceFixed + source.Start, source.Length), destination);
            }
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Fill(this Span<ulong> source, ulong value)
        {
            AutoCSer.Memory.Common.Fill((ulong*)source.Data, value, source.Size);
        }
    }
    /// <summary>
    /// 内存数据段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe struct Span<T>
    {
        /// <summary>
        /// 数据起始位置
        /// </summary>
        internal readonly void* Data;
        /// <summary>
        /// 数据长度
        /// </summary>
        internal readonly int Size;
        /// <summary>
        /// 内存数据段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        internal Span(void* data, int size)
        {
#if DEBUG
            if (size < 0) throw new Exception(size.toString() + " < 0");
#endif
            Data = data;
            Size = size;
        }
    }
}
