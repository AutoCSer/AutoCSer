using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe class UnmanagedStream : UnmanagedStreamBase
    {
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="size"></param>
        public UnmanagedStream(int size = UnmanagedPool.TinySize) : base(size) { }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        internal UnmanagedStream(ref Pointer data) : base(ref data) { }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        internal UnmanagedStream(Pointer data) : base(ref data) { }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, byte value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, sbyte value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, short value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, ushort value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, int value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, uint value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, long value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStream unmanagedStream, ulong value)
        {
            unmanagedStream.Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(byte value)
        {
            PrepSize(1);
            Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(sbyte value)
        {
            PrepSize(1);
            Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(short value)
        {
            *(short*)GetBeforeMove(sizeof(short)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ushort value)
        {
            *(ushort*)GetBeforeMove(sizeof(ushort)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(int value)
        {
            *(int*)GetBeforeMove(sizeof(int)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(uint value)
        {
            *(uint*)GetBeforeMove(sizeof(uint)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(long value)
        {
            *(long*)GetBeforeMove(sizeof(long)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ulong value)
        {
            *(ulong*)GetBeforeMove(sizeof(ulong)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(DateTime value)
        {
            *(DateTime*)GetBeforeMove(sizeof(DateTime)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(float value)
        {
            *(float*)GetBeforeMove(sizeof(float)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(double value)
        {
            *(double*)GetBeforeMove(sizeof(double)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(decimal value)
        {
            *(decimal*)GetBeforeMove(sizeof(decimal)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(Guid value)
        {
            *(Guid*)GetBeforeMove(sizeof(Guid)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ref Guid value)
        {
            *(Guid*)GetBeforeMove(sizeof(Guid)) = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value"></param>
        public void Write(byte[] value)
        {
            if (value.Length != 0) value.AsSpan().CopyTo(new Span<byte>(GetBeforeMove(value.Length), value.Length));
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value"></param>
        public void Write(ref SubArray<byte> value)
        {
            if (value.Length != 0) value.CopyTo(new Span<byte>(GetBeforeMove(value.Length), value.Length));
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(SubArray<byte> value)
        {
            Write(ref value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteNotEmpty(byte* data, int length)
        {
#if DEBUG
            if (length < 0) throw new Exception(length.toString() + " < 0");
#endif
            AutoCSer.Memory.Common.CopyNotNull(data, GetBeforeMove(length), length);
        }
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <returns>字节数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte[] GetArray()
        {
            return Data.GetArray();
        }
    }
}
