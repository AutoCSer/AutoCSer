using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    //public unsafe partial class UnmanagedStream
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteNotNull(byte[] data)
        {
            fixed (byte* dataFixed = data) WriteNotEmpty(dataFixed, data.Length);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(byte[] data, int index, int length)
        {
            fixed (byte* dataFixed = data) AutoCSer.Memory.CopyNotNull(dataFixed + index, Data.Byte + ByteSize, length);
            ByteSize += length;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(byte[] data)
        {
            if (data != null && data.Length != 0) WriteNotNull(data);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(SubArray<byte> data)
        {
            Write(ref data);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ref SubArray<byte> data)
        {
            if (data.Length != 0)
            {
                fixed (byte* dataFixed = data.Array) WriteNotEmpty(dataFixed + data.Start, data.Length);
            }
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWriteNotEmpty(ref SubArray<byte> data)
        {
            fixed (byte* dataFixed = data.Array)
            {
                AutoCSer.Memory.CopyNotNull(dataFixed + data.Start, Data.Byte + ByteSize, data.Length);
                ByteSize += data.Length;
            }
        }
    }
}
