using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase : IDisposable
    {
        /// <summary>
        /// 数据指针
        /// </summary>
        internal Pointer.Size Data;
        /// <summary>
        /// 当前数据长度
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal int ByteSize;
        /// <summary>
        /// 当前写入位置
        /// </summary>
        public byte* CurrentData
        {
            get { return Data.Byte + ByteSize; }
        }
        /// <summary>
        /// 最后一次预增目标数据长度
        /// </summary>
        internal int LastPrepSize;
        /// <summary>
        /// 是否非托管内存数据
        /// </summary>
        internal bool IsUnmanaged;
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="length">容器初始尺寸</param>
        protected UnmanagedStreamBase(int length)
        {
            if (length <= 0) length = UnmanagedPool.TinySize;
            Data.Set(Unmanaged.Get(length, false), length);
            IsUnmanaged = true;
        }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        /// <param name="dataLength">容器初始尺寸</param>
        internal UnmanagedStreamBase(byte* data, int dataLength)
        {
            //if (data == null) throw new ArgumentNullException();
            //if (dataLength <= 0) throw new IndexOutOfRangeException("dataLength[" + dataLength.toString() + "] <= 0");
            Data.Set(data, dataLength);
        }
        /// <summary>
        /// 析构释放资源
        /// </summary>
        ~UnmanagedStreamBase()
        {
            if (IsUnmanaged) Unmanaged.Free(ref Data);
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        public virtual void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 释放数据容器
        /// </summary>
        public virtual void Close()
        {
            if (IsUnmanaged)
            {
                Unmanaged.Free(ref Data);
                IsUnmanaged = false;
            }
            ByteSize = LastPrepSize = 0;
            Data.SetNull();
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public virtual void Clear()
        {
            ByteSize = LastPrepSize = 0;
        }
        /// <summary>
        /// 设置容器尺寸
        /// </summary>
        /// <param name="length">容器尺寸</param>
        protected void setStreamLength(int length)
        {
            if (length < UnmanagedPool.TinySize) length = UnmanagedPool.TinySize;
            void* newData = Unmanaged.Get(length, false);
            AutoCSer.Memory.CopyNotNull(Data.Data, newData, ByteSize);
            if (IsUnmanaged) Unmanaged.Free(ref Data);
            Data.Set(newData, length);
            IsUnmanaged = true;
        }
        /// <summary>
        /// 预增数据流长度
        /// </summary>
        /// <param name="length">增加长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void prepSize(int length)
        {
            if ((LastPrepSize = length + ByteSize) > Data.ByteSize) setStreamLength(Math.Max(LastPrepSize, Data.ByteSize << 1));
        }
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
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="length">数据字节长度</param>
        internal virtual void Reset(byte* data, int length)
        {
            if (IsUnmanaged)
            {
                Unmanaged.Free(ref this.Data);
                IsUnmanaged = false;
            }
            this.Data.Set(data, length);
            this.ByteSize = LastPrepSize = 0;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="length">数据字节长度</param>
        internal virtual void Reset(int length = UnmanagedPool.TinySize)
        {
            if (length != Data.ByteSize)
            {
                if (IsUnmanaged)
                {
                    Unmanaged.Free(ref this.Data);
                    IsUnmanaged = false;
                }
                if (length <= 0) length = UnmanagedPool.TinySize;
                Data.Set(Unmanaged.Get(length, false), length);
                IsUnmanaged = true;
            }
        }

        /// <summary>
        /// 内存数据流转换
        /// </summary>
        /// <param name="stream">内存数据流</param>
        internal virtual void From(UnmanagedStreamBase stream)
        {
            IsUnmanaged = stream.IsUnmanaged;
            Data = stream.Data;
            ByteSize = stream.ByteSize;
            LastPrepSize = stream.LastPrepSize;
            stream.IsUnmanaged = false;
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override unsafe string ToString()
        {
            return new string(Data.Char, 0, ByteSize >> 1);
        }
    }
}
