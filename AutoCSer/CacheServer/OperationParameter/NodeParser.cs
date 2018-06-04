using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 参数解析
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct NodeParser
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        internal readonly CacheManager Cache;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal readonly Buffer Buffer;
        /// <summary>
        /// 数据起始位置
        /// </summary>
        private readonly byte* dataFixed;
        /// <summary>
        /// 返回调用委托
        /// </summary>
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> OnReturn;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        internal byte* Read;
        /// <summary>
        /// 数据结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 是否已经读取结束
        /// </summary>
        internal bool IsEnd
        {
            get { return Read == end; }
        }
        /// <summary>
        /// 参数数据
        /// </summary>
        internal ValueData.Data ValueData;
        /// <summary>
        /// 返回值参数
        /// </summary>
        internal ReturnParameter ReturnParameter;
        /// <summary>
        /// 操作类型
        /// </summary>
        internal OperationType OperationType;
        /// <summary>
        /// 是否操作了数据
        /// </summary>
        internal bool IsOperation;
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="dataFixed">数据起始位置</param>
        internal NodeParser(CacheManager cache, Buffer buffer, byte* dataFixed)
        {
            Read = dataFixed + buffer.Array.Start;
            Buffer = buffer;
            Cache = cache;
            OnReturn = null;
            this.dataFixed = dataFixed;

            end = Read + buffer.Array.Count;
            OperationType = (OperationType)(*(ushort*)(Read + Serializer.OperationTypeOffset));
            ValueData = default(ValueData.Data);
            ReturnParameter = default(ReturnParameter);
            IsOperation = false;
            Read += Serializer.HeaderSize;
        }
        /// <summary>
        /// 节点解析
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="loadData">加载数据</param>
        internal NodeParser(CacheManager cache, ref LoadData loadData)
        {
            Cache = cache;
            Buffer = loadData.Buffer;
            dataFixed = loadData.DataFixed;

            end = Read = null;
            OnReturn = null;
            OperationType = default(OperationType);
            ValueData = default(ValueData.Data);
            ReturnParameter = default(ReturnParameter);
            IsOperation = false;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        internal bool Load(ref LoadData loadData)
        {
            Read = loadData.Start;
            if ((end = loadData.MoveNext()) != null)
            {
                OperationType = (OperationType)(*(ushort*)(Read + Serializer.OperationTypeOffset));
                Read += Serializer.HeaderSize;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取服务端数据结构定义信息
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        internal ServerDataStructure Get(DataStructureItem[] items)
        {
            if ((int)(end - Read) > IndexIdentity.SerializeSize)
            {
                int index = *(int*)Read;
                ServerDataStructure dataStructure = items[index].Get(*(ulong*)(Read + sizeof(int)));
                Read += IndexIdentity.SerializeSize;
                return dataStructure;
            }
            return null;
        }
        /// <summary>
        /// 加载参数数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool LoadValueData()
        {
            ValueData.Load(ref this);
            return Read <= end;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetValueData(int size)
        {
            ValueData.Int64.Int = (int)(Read - dataFixed);
            ValueData.Value = Buffer.Array.Array;
            Read += size;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        internal void SetByteArray()
        {
            int length = *(int*)Read;
            ValueData.Value = Buffer.Array.Array;
            ValueData.Int64.Set((int)(Read - dataFixed) + sizeof(int), length);
            Read += (length + (sizeof(int) + 3)) & (int.MaxValue - 3);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        internal void SetString()
        {
            int length = *(int*)Read;
            ValueData.Value = Buffer.Array.Array;
            ValueData.Int64.Set((int)(Read - dataFixed) + sizeof(int), length);
            Read += length + sizeof(int);
        }
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetValueData(int errorValue)
        {
            return ValueData.Type == CacheServer.ValueData.DataType.Int ? ValueData.Int64.Int : errorValue;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int ReadInt()
        {
            int value = *(int*)Read;
            Read += sizeof(int);
            return value;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal long ReadLong()
        {
            long value = *(long*)Read;
            Read += sizeof(long);
            return value;
        }
        /// <summary>
        /// 获取数据包
        /// </summary>
        /// <param name="headerSize"></param>
        /// <returns></returns>
        internal byte[] CreateReadPacket(int headerSize)
        {
            SubArray<byte> data = Buffer.Array;
            int start = data.Start + headerSize;
            byte[] packet = new byte[(int)(Read - dataFixed) - start];
            System.Buffer.BlockCopy(data.Array, start, packet, 0, packet.Length);
            return packet;
        }
        /// <summary>
        /// 调用返回委托
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturn()
        {
            if (OnReturn != null)
            {
                OnReturn(ReturnParameter);
                OnReturn = null;
            }
        }
        /// <summary>
        /// 设置返回调用委托
        /// </summary>
        /// <param name="onReturn"></param>
        /// <param name="isReturnStream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetOnReturn(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onReturn, bool isReturnStream)
        {
            OnReturn = onReturn;
            ReturnParameter.IsDeSerializeStream = isReturnStream;
        }
    }
}
