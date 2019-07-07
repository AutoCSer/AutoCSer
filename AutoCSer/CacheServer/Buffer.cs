using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 数据缓冲区
    /// </summary>
    internal sealed class Buffer : AutoCSer.Threading.Link<Buffer>, IDisposable
    {
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        internal BufferCount BufferCount;
        /// <summary>
        /// 数据
        /// </summary>
        internal SubArray<byte> Array;
        /// <summary>
        /// 引用计数
        /// </summary>
        private int referenceCount;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Buffer() { }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="size">字节数量</param>
        internal Buffer(int size)
        {
            Array.Set(new byte[size], 0, size);
        }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="bufferCount">数据缓冲区计数</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">字节数量</param>
        internal Buffer(BufferCount bufferCount, int index, int count)
        {
            referenceCount = 1;
            this.BufferCount = bufferCount;
            Array.Set(bufferCount.Buffer.Buffer, index, count);
        }
        /// <summary>
        /// 增加引用计数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Reference()
        {
            Interlocked.Increment(ref referenceCount);
        }
        /// <summary>
        /// 释放引用计数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeReference()
        {
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                BufferCount.Free();
                BufferCount = null;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (BufferCount != null)
            {
                BufferCount.Free();
                BufferCount = null;
            }
        }
        /// <summary>
        /// 设置服务端数据结构索引标识
        /// </summary>
        /// <param name="identity">服务端数据结构索引标识</param>
        internal unsafe void SetIdentity(ref IndexIdentity identity)
        {
            fixed (byte* bufferFixed = Array.Array) identity.UnsafeSerialize(bufferFixed + (Array.Start + OperationParameter.Serializer.HeaderSize));
        }
        /// <summary>
        /// 设置服务端数据结构索引标识
        /// </summary>
        /// <param name="identity">服务端数据结构索引标识</param>
        internal unsafe void SetIdentity(IndexIdentity identity)
        {
            fixed (byte* bufferFixed = Array.Array) identity.UnsafeSerialize(bufferFixed + (Array.Start + OperationParameter.Serializer.HeaderSize));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref SubArray<byte> array)
        {
            Array = array;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Buffer Copy(ref SubBuffer.PoolBufferFull buffer, ref int index)
        {
            System.Buffer.BlockCopy(Array.Array, Array.Start, buffer.Buffer, buffer.StartIndex + index, Array.Length);
            index += Array.Length;
            FreeReference();
            return LinkNext;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="bigBuffer"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Buffer Copy(byte[] bigBuffer)
        {
            System.Buffer.BlockCopy(Array.Array, Array.Start, bigBuffer, sizeof(int), Array.Length);
            FreeReference();
            return LinkNext;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(UnmanagedStream stream)
        {
            stream.Write(ref Array);
            FreeReference();
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TryCopyTo(UnmanagedStream stream)
        {
            if (stream.FreeSize - sizeof(int) > Array.Length)
            {
                stream.Write(ref Array);
                FreeReference();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 释放数据缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal static void DisposeLink(Buffer buffer)
        {
            while (buffer != null)
            {
                buffer.FreeReference();
                buffer = buffer.LinkNext;
            }
        }
    }
}
