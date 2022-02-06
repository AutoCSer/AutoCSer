using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    //public unsafe partial class UnmanagedStream : UnmanagedStreamBase 
    {
        /// <summary>
        /// 预增数据流长度
        /// </summary>
        /// <param name="length">增加长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void PrepLength(int length)//virtual
        {
            prepSize(length);
        }
        /// <summary>
        /// 预增数据流长度
        /// </summary>
        /// <param name="size">增加长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* GetPrepSizeCurrent(int size)
        {
            prepSize(size);
            return Data.Byte + ByteSize;
        }
        ///// <summary>
        ///// 预增数据流结束
        ///// </summary>
        //public virtual void PrepLength() { }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(bool value)
        {
            if (ByteSize == Data.ByteSize) setStreamLength(ByteSize << 1);
            Data.Byte[ByteSize++] = (byte)(value ? 1 : 0);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(bool value)
        {
            Data.Byte[ByteSize++] = (byte)(value ? 1 : 0);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(byte value)
        {
            Data.Byte[ByteSize++] = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(sbyte value)
        {
            Data.Byte[ByteSize++] = (byte)value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(short value)
        {
            *(short*)CurrentData = value;
            ByteSize += sizeof(short);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(ushort value)
        {
            *(ushort*)CurrentData = value;
            ByteSize += sizeof(ushort);
        }
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
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(int value)
        {
            *(int*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(int);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(uint value)
        {
            *(uint*)CurrentData = value;
            ByteSize += sizeof(uint);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(long value)
        {
            *(long*)CurrentData = value;
            ByteSize += sizeof(long);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void UnsafeWrite(ulong value)
        {
            *(ulong*)CurrentData = value;
            ByteSize += sizeof(ulong);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(DateTime value)
        {
            *(long*)CurrentData = *(long*)&value;
            ByteSize += sizeof(long);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(float value)
        {
            *(float*)CurrentData = value;
            ByteSize += sizeof(float);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(double value)
        {
            *(double*)CurrentData = value;
            ByteSize += sizeof(double);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(decimal value)
        {
            *(decimal*)CurrentData = value;
            ByteSize += sizeof(decimal);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(Guid value)
        {
            *(Guid*)CurrentData = value;
            ByteSize += sizeof(Guid);
        }
        /// <summary>
        /// 增加当前数据长度并且补白对齐 4 字节
        /// </summary>
        /// <param name="size">增加数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFillByteSize(int size)
        {
            ByteSize += size;
            switch (size & 3)
            {
                case 1:
                    byte* data = Data.Byte + ByteSize;
                    *data = 0;
                    *(ushort*)(data + 1) = 0;
                    ByteSize += 3;
                    return;
                case 2:
                    *(ushort*)(Data.Byte + ByteSize) = 0;
                    ByteSize += 2;
                    return;
                case 3:
                    *(Data.Byte + ByteSize) = 0;
                    ++ByteSize;
                    return;
            }
        }
    }
}
