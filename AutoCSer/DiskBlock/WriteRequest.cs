using System;
using System.Runtime.CompilerServices;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 数据写入请求
    /// </summary>
    internal unsafe class WriteRequest : AutoCSer.Threading.Link<WriteRequest>
    {
        /// <summary>
        /// 添加数据回调委托
        /// </summary>
        internal AutoCSer.Net.TcpServer.ServerCallback<ulong> OnWrite;
        /// <summary>
        /// 写入数据
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 字节数量
        /// </summary>
        internal int Size;
        /// <summary>
        /// 数据
        /// </summary>
        internal SubArray<byte> SubArray
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex, Length = Size + sizeof(int) }; }
        }
        /// <summary>
        /// 默认磁盘块索引位置
        /// </summary>
        internal ulong Index;
        /// <summary>
        /// 数据写入请求
        /// </summary>
        internal WriteRequest() { }
        /// <summary>
        /// 头部写入请求
        /// </summary>
        /// <param name="index"></param>
        internal WriteRequest(int index)
        {
            this.OnWrite = AutoCSer.Net.TcpServer.ServerCallback<ulong>.Null.Default;
            SubBuffer.Pool.GetPool(AutoCSer.Memory.BufferSize.Byte256).Get(ref Buffer);
            fixed (byte* bufferFixed = Buffer.GetFixedBuffer())
            {
                byte* start = bufferFixed + Buffer.StartIndex;
                *(int*)start = Common.PuzzleValue;
                *(int*)(start + sizeof(int)) = (int)AutoCSer.IO.FileHead.DiskBlockFile;
                *(int*)(start + sizeof(int) * 2) = index;
            }
            Size = sizeof(int) * 2;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="onWrite">添加数据回调委托</param>
        internal WriteRequest(ref AppendBuffer buffer, AutoCSer.Net.TcpServer.ServerCallback<ulong> onWrite)
        {
            this.OnWrite = onWrite;
            SubBuffer.Pool.GetBuffer(ref Buffer, (Size = buffer.Buffer.Length) + sizeof(int));
            System.Buffer.BlockCopy(buffer.Buffer.Array, buffer.Buffer.Start, Buffer.Buffer, Buffer.StartIndex + sizeof(int), Size);
            fixed (byte* bufferFixed = Buffer.GetFixedBuffer()) *(int*)(bufferFixed + Buffer.StartIndex) = Size;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal WriteRequest Write(File file)
        {
            Index = file.Write(ref Buffer, Size);
            Buffer.Free();
            return LinkNext;
        }
        /// <summary>
        /// 读取数据错误处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal WriteRequest Error()
        {
            Buffer.Free();
            OnWrite.Callback(0);
            return LinkNext;
        }
        /// <summary>
        /// 添加数据回调处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal WriteRequest OnFlush()
        {
            OnWrite.Callback(Index);
            return LinkNext;
        }
        /// <summary>
        /// 缓存处理
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnCache(ulong index)
        {
            Buffer.Free();
            OnWrite.Callback(index);
        }
        /// <summary>
        /// 缓存处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnCache()
        {
            Buffer.Free();
            OnWrite.Callback(Index);
        }
        /// <summary>
        /// 缓存处理
        /// </summary>
        /// <param name="data"></param>
        internal void OnCache(byte[] data)
        {
            if (data.Length == Size + sizeof(int))
            {
                bool isCache;
                fixed (byte* bufferFixed = Buffer.GetFixedBuffer()) isCache = AutoCSer.Memory.Common.EqualNotNull(data, bufferFixed + Buffer.StartIndex, Size + sizeof(int));
                if (isCache)
                {
                    Buffer.Free();
                    OnWrite.Callback(Index);
                    return;
                }
            }
            AppendWrite();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        internal virtual void AppendWrite() 
        {
            throw new InvalidOperationException();
        }
    }
}
