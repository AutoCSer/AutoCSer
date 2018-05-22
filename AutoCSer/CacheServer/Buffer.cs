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
        private volatile BufferCount bufferCount;
        /// <summary>
        /// 数据
        /// </summary>
        internal SubArray<byte> Array;
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
            this.bufferCount = bufferCount;
            Array.Set(bufferCount.Buffer.Buffer, index, count);
            Interlocked.Increment(ref bufferCount.Count);
        }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="buffer">数据</param>
        internal Buffer(ref SubArray<byte> buffer)
        {
            Array = buffer;
        }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        internal Buffer(Buffer buffer)
        {
            bufferCount = buffer.bufferCount;
            Array = buffer.Array;
            Interlocked.Increment(ref bufferCount.Count);
        }
        /// <summary>
        /// 复制数据缓冲区
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Buffer Copy()
        {
            return bufferCount != null ? new Buffer(this) : new Buffer(ref Array);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Dispose()
        {
            BufferCount bufferCount = Interlocked.Exchange(ref this.bufferCount, null);
            if (bufferCount != null) bufferCount.Free();
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
            Dispose();
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
            Dispose();
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
            Dispose();
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
                Dispose();
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
                buffer.Dispose();
                buffer = buffer.LinkNext;
            }
        }
    }
}
