using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块成员
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct Member<valueType> where valueType : class
    {
        /// <summary>
        /// 磁盘块索引位置
        /// </summary>
        internal ulong Index;
        /// <summary>
        /// 数据长度
        /// </summary>
        internal int Size;
        /// <summary>
        /// 成员状态
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private MemberState state;
        /// <summary>
        /// TCP 返回值类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private Net.TcpServer.ReturnType tcpReturnType;
        /// <summary>
        /// 反序列化状态
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private AutoCSer.BinarySerialize.DeSerializeState deSerializeState;
        /// <summary>
        /// 当前数据
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private valueType value;
        /// <summary>
        /// 目标对象
        /// </summary>
        public MemberValue<valueType> Value
        {
            get
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
                            AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer> clientBuffer = client.read(new ClientBuffer { Buffer = new SubArray<byte>(buffer.StartIndex, Size, buffer.Buffer), IsClient = true }, Index);
                            onRead(ref clientBuffer);
                        }
                        finally { buffer.Free(); }
                        break;
                }
                return new MemberValue<valueType> { Value = value, State = state };
            }
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="clientBuffer">客户端缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onRead(ref AutoCSer.Net.TcpServer.ReturnValue<ClientBuffer> clientBuffer)
        {
            if (clientBuffer.Type == Net.TcpServer.ReturnType.Success)
            {
                if ((state = clientBuffer.Value.State) == MemberState.Remote)
                {
                    deSerializeState = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<valueType>(ref clientBuffer.Value.Buffer, ref value).State;
                    if (deSerializeState != BinarySerialize.DeSerializeState.Success)
                    {
                        value = null;
                        state = MemberState.DeSerializeError;
                    }
                }
            }
            else
            {
                tcpReturnType = clientBuffer.Type;
                state = MemberState.TcpError;
            }
        }
        /// <summary>
        /// 磁盘块成员
        /// </summary>
        /// <param name="value">磁盘块索引位置</param>
        /// <returns>磁盘块成员</returns>
        public static implicit operator Member<valueType>(MemberIndex value)
        {
            return new Member<valueType> { Index = value.Index, Size = value.Size };
        }
        /// <summary>
        /// 磁盘块索引位置
        /// </summary>
        /// <param name="value">磁盘块成员</param>
        /// <returns>磁盘块索引位置</returns>
        public static implicit operator MemberIndex(Member<valueType> value)
        {
            return new MemberIndex { Index = value.Index, Size = value.Size };
        }
        ///// <summary>
        ///// 清除数据与状态（仅保留磁盘块索引位置）
        ///// </summary>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public void ClearValueState()
        //{
        //    value = null;
        //    state = MemberState.Unknown;
        //    tcpReturnType = Net.TcpServer.ReturnType.Success;
        //    deSerializeState = BinarySerialize.DeSerializeState.Success;
        //}
        /// <summary>
        /// 设置为 null
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNull()
        {
            value = null;
            Size = 0;
            state = MemberState.Remote;
        }
        /// <summary>
        /// 设置为本地对象
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(valueType value)
        {
            this.value = value;
            Size = 0;
            state = MemberState.Local;
        }
        /// <summary>
        /// 设置远程对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="blockIndex">磁盘块编号</param>
        /// <param name="bufferSize">序列化缓冲区大小</param>
        /// <returns></returns>
        public unsafe bool Set(valueType value, int blockIndex, SubBuffer.Size bufferSize = SubBuffer.Size.Kilobyte4)
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
                    AutoCSer.Net.TcpServer.ReturnValue<ulong> index = client.append(appendBuffer);
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
        /// <summary>
        /// 获取添加数据缓冲区
        /// </summary>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe AppendBuffer getAppendBuffer(ref valueType value, BinarySerialize.Serializer serializer, ref SubBuffer.PoolBufferFull buffer, out int size)
        {
            fixed (byte* bufferFixed = buffer.Buffer)
            {
                byte* start = bufferFixed + buffer.StartIndex;
                serializer.SerializeNotNull(value, start, buffer.PoolBuffer.Pool.Size, ClientConfig.BinarySerializeConfig);
                size = serializer.Stream.ByteSize;
                if (serializer.Stream.Data.Data == start) return new AppendBuffer { Buffer = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = size }, Index = size == Size ? Index : 0 };
                else return new AppendBuffer { Buffer = new SubArray<byte> { Array = serializer.Stream.GetArray(), Length = size }, Index = size == Size ? Index : 0 };
            }
        }
    }
}
