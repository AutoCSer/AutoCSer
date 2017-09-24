using System;
using System.Threading.Tasks;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块成员
    /// </summary>
    public partial struct Member<valueType>
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        public async Task<MemberValue<valueType>> ValueAsync()
        {
            switch (state)
            {
                case MemberState.Unknown:
                    if (Size == 0) return new MemberValue<valueType> { State = state = MemberState.Remote };
                    Server.TcpInternalClient client = ClientPool.Get(Index);
                    if (client == null) return new MemberValue<valueType> { State = MemberState.NoClient };
                    SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
                    SubBuffer.Pool.GetBuffer(ref buffer, Size);
                    try
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer> clientBuffer = await client.readAwaiter(new ClientBuffer { Buffer = new SubArray<byte>(buffer.StartIndex, Size, buffer.Buffer), IsClient = true }, Index);
                        onRead(ref clientBuffer);
                    }
                    finally { buffer.Free(); }
                    break;
            }
            return new MemberValue<valueType> { Value = value, State = state };
        }
        /// <summary>
        /// 设置远程对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="blockIndex">磁盘块编号</param>
        /// <param name="bufferSize">序列化缓冲区大小</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(valueType value, int blockIndex, SubBuffer.Size bufferSize = SubBuffer.Size.Kilobyte4)
        {
            if (value == null)
            {
                SetNull();
                return true;
            }
            Server.TcpInternalClient client = ClientPool.Get(blockIndex);
            if (client != null)
            {
                BinarySerialize.Serializer serializer = BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new BinarySerialize.Serializer();
                SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
                SubBuffer.Pool.GetPool(bufferSize).Get(ref buffer);
                try
                {
                    int size;
                    AppendBuffer appendBuffer = getAppendBuffer(ref value, serializer, ref buffer, out size);
                    appendBuffer.BlockIndex = (ushort)blockIndex;
                    AutoCSer.Net.TcpServer.ReturnValue<ulong> index = await client.appendAwaiter(appendBuffer);
                    if (index.Type == Net.TcpServer.ReturnType.Success && index.Value != 0)
                    {
                        Index = index.Value;
                        Size = size;
                        value = Value;
                        state = MemberState.Remote;
                        return true;
                    }
                }
                finally
                {
                    buffer.Free();
                    serializer.Free();
                }
            }
            return false;
        }
    }
}
