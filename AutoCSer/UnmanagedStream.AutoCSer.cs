using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe partial class UnmanagedStream
    {
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="size"></param>
        public UnmanagedStream(int size = UnmanagedPool.TinySize) : base(size) { }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index">复制起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void GetSubBuffer(ref SubBuffer.PoolBufferFull buffer, int index)
        {
            SubBuffer.Pool.GetBuffer(ref buffer, ByteSize);
            fixed (byte* dataFixed = buffer.Buffer) AutoCSer.Memory.CopyNotNull(Data.Byte + index, dataFixed + (buffer.StartIndex + index), ByteSize - index);
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void GetSubBuffer(ref SubBuffer.PoolBufferFull buffer)
        {
            SubBuffer.Pool.GetBuffer(ref buffer, ByteSize);
            fixed (byte* dataFixed = buffer.Buffer) AutoCSer.Memory.CopyNotNull(Data.Byte, dataFixed + buffer.StartIndex, ByteSize);
        }
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
        /// 写入数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteNotEmpty(byte* data, int length)
        {
            prepSize(length);
            AutoCSer.Memory.CopyNotNull(data, Data.Byte + ByteSize, length);
            ByteSize += length;
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
