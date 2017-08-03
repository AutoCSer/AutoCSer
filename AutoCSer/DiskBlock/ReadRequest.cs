using System;
using System.Runtime.CompilerServices;
using System.IO;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 数据读取请求
    /// </summary>
    internal sealed unsafe class ReadRequest : AutoCSer.Threading.Link<ReadRequest>
    {
        /// <summary>
        /// 索引位置
        /// </summary>
        internal long Index;
        /// <summary>
        /// 字节数量
        /// </summary>
        internal int Size;
        /// <summary>
        /// 获取数据回调委托
        /// </summary>
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer>, bool> OnRead;
        /// <summary>
        /// 数据写入请求
        /// </summary>
        internal WriteRequest WriteRequest;
        /// <summary>
        /// 读取数据错误处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReadRequest Error()
        {
            if (WriteRequest == null) OnRead(new ClientBuffer { State = MemberState.ServerException });
            else WriteRequest.AppendWrite();
            return LinkNext;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="block"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        internal ReadRequest Read(BlockBase block, FileStream fileStream)
        {
            HashBytes hashData;
            if (WriteRequest == null)
            {
                if (block.IndexCache.TryGetValue(ref Index, out hashData))
                {
                    byte[] data = hashData.SubArray.Array;
                    if (data.Length == Size + sizeof(int)) OnRead(new ClientBuffer { Buffer = new SubArray<byte> { Array = data, Start = sizeof(int), Length = Size }, State = MemberState.Remote });
                    else OnRead(new ClientBuffer { State = MemberState.SizeError });
                }
                else
                {
                    byte[] data = read(block, fileStream);
                    if (data == null) OnRead(new ClientBuffer { State = MemberState.SizeError });
                    else OnRead(new ClientBuffer { Buffer = new SubArray<byte> { Array = data, Start = sizeof(int), Length = Size }, State = MemberState.Remote });
                }
            }
            else if (block.IndexCache.TryGetValue(ref Index, out hashData)) WriteRequest.OnCache(hashData.SubArray.Array);
            else
            {
                SubArray<byte> data = WriteRequest.SubArray;
                if (block.DataCache != null)
                {
                    ulong index = block.GetCacheIndex(data);
                    if (index != 0)
                    {
                        WriteRequest.OnCache(index);
                        return LinkNext;
                    }
                }
                if (isData(fileStream, ref data)) WriteRequest.OnCache();
                else WriteRequest.AppendWrite();
            }
            return LinkNext;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="block"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        private byte[] read(BlockBase block, FileStream fileStream)
        {
            byte[] data = new byte[Size + sizeof(int)];
            fileStream.Seek(Index, SeekOrigin.Begin);
            if (data.Length <= BlockBase.CheckSize)
            {
                if (((fileStream.Read(data, 0, data.Length) ^ data.Length) | (BitConverter.ToInt32(data, 0) ^ Size)) != 0) return null;
            }
            else
            {
                if (((fileStream.Read(data, 0, BlockBase.CheckSize) ^ BlockBase.CheckSize) | (BitConverter.ToInt32(data, 0) ^ Size)) != 0) return null;
                int readSize = data.Length - BlockBase.CheckSize;
                if (fileStream.Read(data, BlockBase.CheckSize, readSize) != readSize) return null;
            }
            block.SetCache(Index, data);
            return data;
        }
        /// <summary>
        /// 判断数据是否匹配
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool isData(FileStream fileStream, ref SubArray<byte> data)
        {
            fileStream.Seek(Index, SeekOrigin.Begin);
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            BlockBase.DefaultBufferPool.Get(ref buffer);
            try
            {
                byte[] bufferArray = buffer.Buffer;
                int size = data.Length;
                fixed (byte* dataFixed = data.Array, bufferFixed = bufferArray)
                {
                    byte* dataStart = dataFixed + data.Start, bufferStart = bufferFixed + buffer.StartIndex;
                    while (size > BlockBase.CheckSize)
                    {
                        if (fileStream.Read(bufferArray, buffer.StartIndex, BlockBase.CheckSize) != BlockBase.CheckSize) return false;
                        if (!AutoCSer.Memory.EqualNotNull(dataStart, bufferStart, BlockBase.CheckSize)) return false;
                        dataStart += BlockBase.CheckSize;
                        size -= BlockBase.CheckSize;
                    }
                    return fileStream.Read(bufferArray, buffer.StartIndex, size) == size && AutoCSer.Memory.EqualNotNull(dataStart, bufferStart, size);
                }
            }
            finally { buffer.Free(); }
        }
    }
}
