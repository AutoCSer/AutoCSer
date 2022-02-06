using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase : IDisposable
    {
        /// <summary>
        /// 数据指针
        /// </summary>
        internal Pointer Data;
        /// <summary>
        /// 空闲字节数量
        /// </summary>
        public int FreeSize
        {
            get { return Data.FreeSize; }
        }
        /// <summary>
        /// 当前数据操作位置
        /// </summary>
        public byte* Current
        {
            get { return Data.Current; }
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
        /// 保留
        /// </summary>
        internal bool Reserve;
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="size">容器初始尺寸</param>
        protected UnmanagedStreamBase(int size)
        {
            if (size <= 0) size = UnmanagedPool.TinySize;
            Data = Unmanaged.GetPointer(size, false);
            IsUnmanaged = true;
        }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        internal UnmanagedStreamBase(ref Pointer data)
        {
#if DEBUG
            data.DebugCheck();
#endif
            Data = data;
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
                Data.CurrentIndex = 0;
            }
            else Data.SetNull();
            LastPrepSize = 0;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public virtual void Clear()
        {
            Data.CurrentIndex = 0;
            LastPrepSize = 0;
        }
        /// <summary>
        /// 设置容器字节尺寸
        /// </summary>
        /// <param name="size">容器字节尺寸</param>
        protected void setStreamSize(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
#endif
            if (size < UnmanagedPool.TinySize) size = UnmanagedPool.TinySize;
            Pointer newData = Unmanaged.GetPointer(size, false);
            Data.CopyTo(ref newData);
            if (IsUnmanaged) Unmanaged.Free(ref Data);
            Data = newData;
            IsUnmanaged = true;
        }
        /// <summary>
        /// 预增数据流字节长度
        /// </summary>
        /// <param name="size">增加字节长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal void PrepSize(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
            if ((long)size + Data.CurrentIndex > int.MaxValue) throw new Exception(size.toString() + " + " + Data.CurrentIndex.toString() + " > int.MaxValue");
#endif
            if ((LastPrepSize = size + Data.CurrentIndex) > Data.ByteSize)
            {
                long size2 = (long)Data.ByteSize << 1;
                if (size2 <= int.MaxValue) setStreamSize(Math.Max(LastPrepSize, (int)size2));
                else setStreamSize(int.MaxValue);
            }
        }
        /// <summary>
        /// 预增数据流字节长度
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="size">增加字节长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PrepSize(UnmanagedStreamBase unmanagedStream, int size)
        {
            unmanagedStream.PrepSize(size);
        }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* GetPrepSizeCurrent(int size)
        {
            PrepSize(size);
            return Data.Current;
        }
        /// <summary>
        /// 增加数据流字节长度并返回增加之前的位置
        /// </summary>
        /// <param name="size">增加字节长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal byte* GetBeforeMove(int size)
        {
            PrepSize(size);
            return Data.GetBeforeMove(size);
        }
        /// <summary>
        /// 增加流长度并返回增加后的流长度
        /// </summary>
        /// <param name="length">增加长度</param>
        /// <returns>增加后的流长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int AddSize(int length)
        {
            PrepSize(length);
            return Data.CurrentIndex += length;
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeMoveSize(UnmanagedStreamBase stream, int size)
        {
            stream.Data.UnsafeMoveSize(size);
        }
        /// <summary>
        /// 移动当前位置
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void MoveSize(int size)
        {
            Data.MoveSize(size);
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="data">数据</param>
        internal virtual void Reset(ref Pointer data)
        {
            if (IsUnmanaged)
            {
                Unmanaged.Free(ref Data);
                IsUnmanaged = false;
            }
            Data = data;
            LastPrepSize = 0;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Reset(void* data, int size)
        {
            AutoCSer.Memory.Pointer buffer = new AutoCSer.Memory.Pointer(data, size);
            Reset(ref buffer);
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="size">数据字节长度</param>
        internal virtual void Reset(int size = UnmanagedPool.TinySize)
        {
            if (size <= 0) size = UnmanagedPool.TinySize;
            if (size != Data.ByteSize)
            {
                if (IsUnmanaged)
                {
                    Unmanaged.Free(ref this.Data);
                    IsUnmanaged = false;
                }
                Data = Unmanaged.GetPointer(size, false);
                IsUnmanaged = true;
            }
            Data.CurrentIndex = LastPrepSize = 0;
        }
        /// <summary>
        /// 内存数据流转换
        /// </summary>
        /// <param name="stream">内存数据流</param>
        internal virtual void From(UnmanagedStreamBase stream)
        {
            IsUnmanaged = stream.IsUnmanaged;
            Data = stream.Data;
            LastPrepSize = stream.LastPrepSize;
            stream.IsUnmanaged = false;
        }

        /// <summary>
        /// 写入 64 字节数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(ulong value0, ulong value1, ulong value2, ulong value3)
        {
            byte* data = Data.GetBeforeMove(sizeof(ulong) * 4);
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            *(ulong*)(data + sizeof(ulong) * 2) = value2;
            *(ulong*)(data + sizeof(ulong) * 3) = value3;
        }
        /// <summary>
        /// 写入 64 字节数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, ulong value2, ulong value3)
        {
            unmanagedStream.UnsafeWrite(value0, value1, value2, value3);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(ulong value0, ulong value1, ulong value2, ulong value3, int size)
        {
            byte* data = Data.GetBeforeMove(size);
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            *(ulong*)(data + sizeof(ulong) * 2) = value2;
            *(ulong*)(data + sizeof(ulong) * 3) = value3;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, ulong value2, ulong value3, int size)
        {
            unmanagedStream.UnsafeWrite(value0, value1, value2, value3, size);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(ulong value0, ulong value1, ulong value2, int size)
        {
            byte* data = Data.GetBeforeMove(size);
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
            *(ulong*)(data + sizeof(ulong) * 2) = value2;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, ulong value2, int size)
        {
            unmanagedStream.UnsafeWrite(value0, value1, value2, size);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(ulong value0, ulong value1, int size)
        {
            byte* data = Data.GetBeforeMove(size);
            *(ulong*)data = value0;
            *(ulong*)(data + sizeof(ulong)) = value1;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value0, ulong value1, int size)
        {
            unmanagedStream.UnsafeWrite(value0, value1, size);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeWrite(ulong value, int size)
        {
            *(ulong*)(Data.GetBeforeMove(size)) = value;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="unmanagedStream"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void UnsafeWrite(UnmanagedStreamBase unmanagedStream, ulong value, int size)
        {
            unmanagedStream.UnsafeWrite(value, size);
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override unsafe string ToString()
        {
#if DEBUG
            Data.DebugCheck();
#endif
            return new string(Data.Char, 0, Data.CurrentIndex >> 1);
        }
    }
}
