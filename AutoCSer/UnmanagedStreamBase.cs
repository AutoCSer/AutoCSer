using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    //public unsafe abstract partial class UnmanagedStreamBase : IDisposable
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void Write(char value)
        {
            prepSize(sizeof(char));
            *(char*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(char);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public unsafe void Write(string value)
        {
            if (value != null)
            {
                int length = value.Length << 1;
                prepSize(length);
                AutoCSer.Extension.StringExtension.CopyNotNull(value, Data.Byte + ByteSize);
                ByteSize += length;
            }
        }
        ///// <summary>
        ///// 写字符串
        ///// </summary>
        ///// <param name="value">字符串</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public void Write(SubString value)
        //{
        //    Write(ref value);
        //}
        ///// <summary>
        ///// 写字符串
        ///// </summary>
        ///// <param name="value">字符串</param>
        //public unsafe void Write(ref SubString value)
        //{
        //    if (value.Length != 0)
        //    {
        //        int length = value.Length << 1;
        //        prepLength(length);
        //        fixed (char* valueFixed = value.String) AutoCSer.Memory.CopyNotNull(valueFixed + value.StartIndex, Data.Byte + Length, length);
        //        Length += length;
        //    }
        //}
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">写入字符数</param>
        internal unsafe void UnsafeWrite(string value, int index, int count)
        {
            prepSize(count <<= 1);
            fixed (char* valueFixed = value)
            {
                AutoCSer.Memory.CopyNotNull(valueFixed + index, Data.Byte + ByteSize, count);
            }
            ByteSize += count;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="start">字符串起始位置</param>
        /// <param name="count">写入字符数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void Write(char* start, int count)
        {
            if (start != null) WriteNotNull(start, count);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="start">字符串起始位置</param>
        /// <param name="count">写入字符数</param>
        internal unsafe void WriteNotNull(char* start, int count)
        {
            int length = count << 1;
            prepSize(length);
            AutoCSer.Memory.CopyNotNull(start, Data.Byte + ByteSize, length);
            ByteSize += length;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        public unsafe void Write(ref SubString value)
        {
            if (value.Length != 0) WriteNotEmpty(ref value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        internal unsafe void WriteNotEmpty(ref SubString value)
        {
            int length = value.Length << 1;
            prepSize(length);
            fixed (char* valueFixed = value.String) AutoCSer.Memory.CopyNotNull(valueFixed + value.Start, Data.Byte + ByteSize, length);
            ByteSize += length;
        }
    }
}
