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
        internal ValueData.Data ReturnParameter;
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
            ReturnParameter = default(ValueData.Data);
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
            ReturnParameter = default(ValueData.Data);
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
        /// 检测构造参数是否相同
        /// </summary>
        /// <param name="constructorParameter">构造参数</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckConstructorParameter(byte[] constructorParameter)
        {
            return (int)(end - Read) == constructorParameter.Length && AutoCSer.Memory.EqualNotNull(constructorParameter, Read, constructorParameter.Length);
        }
        /// <summary>
        /// 获取构造参数数据包
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        internal byte[] CreateConstructorParameter(byte* start)
        {
            byte[] constructorParameter = new byte[(int)(end - start)];
            System.Buffer.BlockCopy(Buffer.Array.Array, (int)(start - dataFixed), constructorParameter, 0, constructorParameter.Length);
            return constructorParameter;
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
        /// 设置操作类型
        /// </summary>
        /// <param name="operationType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetOperationType(OperationType operationType)
        {
            *(uint*)(dataFixed + Buffer.Array.Start + Serializer.OperationTypeOffset) = (ushort)operationType;
            IsOperation = true;
        }
        /// <summary>
        /// 调用返回委托
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturn()
        {
            if (OnReturn != null)
            {
                OnReturn(new ReturnParameter(ref ReturnParameter));
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
            ReturnParameter.IsReturnDeSerializeStream = isReturnStream;
        }
        /// <summary>
        /// 调用返回委托
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnReturnDistributionMessage()
        {
            Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool> onReturn = ReturnParameter.Value as Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool>;
            if (onReturn != null)
            {
                onReturn(new IdentityReturnParameter(ref ReturnParameter));
                ReturnParameter.Value = null;
            }
        }
        /// <summary>
        /// 设置返回调用委托
        /// </summary>
        /// <param name="onReturn"></param>
        /// <param name="isReturnStream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetOnReturn(Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool> onReturn, bool isReturnStream)
        {
            ReturnParameter.Value = onReturn;
            ReturnParameter.IsReturnDeSerializeStream = isReturnStream;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetOperationReturnParameter()
        {
            ReturnParameter.ReturnParameterSet(true);
            IsOperation = true;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetOperationReturnParameterFalse()
        {
            ReturnParameter.ReturnParameterSet(false);
            IsOperation = true;
        }
        /// <summary>
        /// 修改操作数据
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="read"></param>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        internal void UpdateOperation<valueType>(byte* read, valueType value, OperationType operationType)
        {
            Read = read;
            AutoCSer.CacheServer.OperationUpdater.Data<valueType>.UpdateOperationData(ref this, value);
            byte* write = dataFixed + Buffer.Array.Start;
            Buffer.Array.Length = *(int*)write = (int)(Read - write);
            *(uint*)(write + Serializer.OperationTypeOffset) = (ushort)operationType;
            IsOperation = true;
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(ulong value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.ULong;
            *(ulong*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(ulong);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(long value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Long;
            *(long*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(ulong);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(uint value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.UInt;
            *(uint*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(int value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Int;
            *(int*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(ushort value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.UShort + ((uint)value << 16);
            Read += sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(short value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Short + ((uint)(ushort)value << 16);
            Read += sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(byte value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Byte + ((uint)value << 16);
            Read += sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(sbyte value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.SByte + ((uint)(byte)value << 16);
            Read += sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(double value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Double;
            *(double*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(ulong);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(float value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Float;
            *(float*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(uint);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UpdateOperation(decimal value)
        {
            *(uint*)Read = (byte)CacheServer.ValueData.DataType.Decimal;
            *(decimal*)(Read + sizeof(uint)) = value;
            Read += sizeof(uint) + sizeof(decimal);
        }
    }
}
