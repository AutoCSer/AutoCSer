using System;
using System.Runtime.CompilerServices;

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
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> OnWrite;
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
            this.OnWrite = onWriteHead;
            SubBuffer.Pool.GetPool(SubBuffer.Size.Byte256).Get(ref Buffer);
            fixed (byte* bufferFixed = Buffer.Buffer)
            {
                byte* start = bufferFixed + Buffer.StartIndex;
                *(int*)start = Pub.PuzzleValue;
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
        internal WriteRequest(ref AppendBuffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> onWrite)
        {
            this.OnWrite = onWrite;
            SubBuffer.Pool.GetBuffer(ref Buffer, (Size = buffer.Buffer.Length) + sizeof(int));
            System.Buffer.BlockCopy(buffer.Buffer.Array, buffer.Buffer.Start, Buffer.Buffer, Buffer.StartIndex + sizeof(int), Size);
            fixed (byte* bufferFixed = Buffer.Buffer) *(int*)(bufferFixed + Buffer.StartIndex) = Size;
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
            OnWrite(0);
            return LinkNext;
        }
        /// <summary>
        /// 添加数据回调处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal WriteRequest OnFlush()
        {
            OnWrite(Index);
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
            OnWrite(index);
        }
        /// <summary>
        /// 缓存处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnCache()
        {
            Buffer.Free();
            OnWrite(Index);
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
                fixed (byte* bufferFixed = Buffer.Buffer) isCache = AutoCSer.Memory.EqualNotNull(data, bufferFixed + Buffer.StartIndex, Size + sizeof(int));
                if (isCache)
                {
                    Buffer.Free();
                    OnWrite(Index);
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

        /// <summary>
        /// 添加数据头部回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool onWriteHead(AutoCSer.Net.TcpServer.ReturnValue<ulong> value)
        {
            return true;
        }
    }
}
