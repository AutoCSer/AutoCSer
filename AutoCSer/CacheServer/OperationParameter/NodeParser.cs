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
        /// 参数数据包
        /// </summary>
        private readonly byte[] data;
        /// <summary>
        /// 数据起始位置
        /// </summary>
        private readonly byte* dataFixed;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private byte* read;
        /// <summary>
        /// 数据结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 是否已经读取结束
        /// </summary>
        internal bool IsEnd
        {
            get { return read == end; }
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
        /// <param name="data">参数数据包</param>
        /// <param name="dataFixed">数据起始位置</param>
        internal NodeParser(ref SubArray<byte> data, byte* dataFixed)
        {
            read = dataFixed + data.Start;
            this.data = data.Array;
            this.dataFixed = dataFixed;

            end = read + data.Count;
            OperationType = (OperationType)(*(ushort*)(read + Serializer.OperationTypeOffset));
            ValueData = default(ValueData.Data);
            ReturnParameter = default(ReturnParameter);
            IsOperation = false;
            read += Serializer.HeaderSize;
        }
        /// <summary>
        /// 节点解析
        /// </summary>
        /// <param name="loadData">加载数据</param>
        internal NodeParser(ref LoadData loadData)
        {
            data = loadData.Buffer.Array.Array;
            dataFixed = loadData.DataFixed;

            end = read = null;
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
            read = loadData.Start;
            if ((end = loadData.MoveNext()) != null)
            {
                OperationType = (OperationType)(*(ushort*)(read + Serializer.OperationTypeOffset));
                read += Serializer.HeaderSize;
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
            if ((int)(end - read) > IndexIdentity.SerializeSize)
            {
                int index = *(int*)read;
                ServerDataStructure dataStructure = items[index].Get(*(ulong*)(read + sizeof(int)));
                read += IndexIdentity.SerializeSize;
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
            return read <= end;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetValueData(int size)
        {
            ValueData.Int64.Int = (int)(read - dataFixed);
            ValueData.Value = data;
            read += size;
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        internal void SetByteArray()
        {
            int length = *(int*)read;
            ValueData.Value = data;
            ValueData.Int64.Set((int)(read - dataFixed) + sizeof(int), length);
            read += (length + (sizeof(int) + 3)) & (int.MaxValue - 3);
        }
        /// <summary>
        /// 设置参数数据
        /// </summary>
        internal void SetString()
        {
            int length = *(int*)read;
            ValueData.Value = data;
            ValueData.Int64.Set((int)(read - dataFixed) + sizeof(int), length);
            read += length + sizeof(int);
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
            int value = *(int*)read;
            read += sizeof(int);
            return value;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal long ReadLong()
        {
            long value = *(long*)read;
            read += sizeof(long);
            return value;
        }
    }
}
