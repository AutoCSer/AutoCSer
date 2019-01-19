using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe partial class UnmanagedStream : UnmanagedStreamBase 
    {
        ///// <summary>
        ///// 原始偏移位置
        ///// </summary>
        //protected int offset;
        ///// <summary>
        ///// 相对于原始偏移位置的数据长度
        ///// </summary>
        //internal int OffsetLength
        //{
        //    get { return offset + ByteSize; }
        //}
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        /// <param name="dataSize">容器初始字节数</param>
        public UnmanagedStream(byte* data, int dataSize) : base(data, dataSize) { }
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
        /// <summary>
        /// 增加流长度并返回增加后的流长度
        /// </summary>
        /// <param name="length">增加长度</param>
        /// <returns>增加后的流长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int AddSize(int length)
        {
            prepSize(length);
            return ByteSize += length;
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
        public void Write(byte value)
        {
            if (ByteSize == Data.ByteSize) setStreamLength(ByteSize << 1);
            Data.Byte[ByteSize++] = value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(sbyte value)
        {
            if (ByteSize == Data.ByteSize) setStreamLength(ByteSize << 1);
            Data.Byte[ByteSize++] = (byte)value;
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(short value)
        {
            prepSize(sizeof(short));
            *(short*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(short);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ushort value)
        {
            prepSize(sizeof(ushort));
            *(ushort*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(ushort);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(int value)
        {
            prepSize(sizeof(int));
            *(int*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(int);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(uint value)
        {
            prepSize(sizeof(uint));
            *(uint*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(uint);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(long value)
        {
            prepSize(sizeof(long));
            *(long*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(long);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ulong value)
        {
            prepSize(sizeof(ulong));
            *(ulong*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(ulong);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(DateTime value)
        {
            prepSize(sizeof(DateTime));
            *(DateTime*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(DateTime);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(float value)
        {
            prepSize(sizeof(float));
            *(float*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(float);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(double value)
        {
            prepSize(sizeof(double));
            *(double*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(double);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(decimal value)
        {
            prepSize(sizeof(decimal));
            *(decimal*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(decimal);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(Guid value)
        {
            prepSize(sizeof(Guid));
            *(Guid*)(Data.Byte + ByteSize) = value;
            ByteSize += sizeof(Guid);
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
        /// 二进制序列化填充空白字符
        /// </summary>
        /// <param name="fillSize">字节数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFill(int fillSize)
        {
            switch (fillSize)
            {
                case 1:
                    *(Data.Byte + ByteSize) = 0;
                    ++ByteSize;
                    return;
                case 2:
                    *(ushort*)(Data.Byte + ByteSize) = 0;
                    ByteSize += 2;
                    return;
                case 3:
                    byte* data = Data.Byte + ByteSize;
                    *data = 0;
                    *(ushort*)(data + 1) = 0;
                    ByteSize += 3;
                    return;
            }
        }
        /// <summary>
        /// 补白对齐 4 字节
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeFillWithStartIndex(int startIndex)
        {
            switch ((ByteSize - startIndex) & 3)
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
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] GetArray()
        {
            if (ByteSize == 0) return NullValue<byte>.Array;
            byte[] data = new byte[ByteSize];
            AutoCSer.Memory.CopyNotNull(this.Data.Data, data, ByteSize);
            return data;
        }
    }
}
