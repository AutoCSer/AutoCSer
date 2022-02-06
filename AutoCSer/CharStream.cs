using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 内存字符流
    /// </summary>
    //public sealed unsafe partial class CharStream : UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(char value)
        {
            *(char*)CurrentData = value;
            ByteSize += sizeof(char);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        public void WriteNotNull(string value)
        {
            int length = value.Length << 1;
            prepSize(length);
            AutoCSer.Extension.StringExtension.CopyNotNull(value, Data.Byte + ByteSize);
            ByteSize += length;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(string value)
        {
            AutoCSer.Extension.StringExtension.CopyNotNull(value, Data.Byte + ByteSize);
            ByteSize += value.Length << 1;
        }
        /// <summary>
        /// 写字符串(无需预增数据流)
        /// </summary>
        /// <param name="value">字符串,长度必须>0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeSimpleWrite(string value)
        {
            AutoCSer.Extension.StringExtension.SimpleCopyNotNull(value, Data.Byte + ByteSize);
            ByteSize += value.Length << 1;
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="start">字符串起始位置,不能为null</param>
        /// <param name="count">写入字符数，必须>0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeSimpleWriteNotNull(char* start, int count)
        {
            AutoCSer.Extension.StringExtension.SimpleCopyNotNull64(start, (char*)(Data.Byte + ByteSize), count);
            ByteSize += count << 1;
        }
    }
}
