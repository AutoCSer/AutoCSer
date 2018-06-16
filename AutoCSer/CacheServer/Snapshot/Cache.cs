using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Snapshot
{
    /// <summary>
    /// 缓存快照
    /// </summary>
    internal sealed unsafe class Cache : IDisposable
    {
        /// <summary>
        /// 数据结构定义集合
        /// </summary>
        private ServerDataStructure[] dataStructures;
        /// <summary>
        /// 数据结构数组集合
        /// </summary>
        private DataStructureItem[] array;
        /// <summary>
        /// 缓存序列化数据流
        /// </summary>
        private readonly UnmanagedStream stream;
        /// <summary>
        /// 参数数据
        /// </summary>
        internal ValueData.Data Parameter;
        /// <summary>
        /// 数据结构定义集合索引
        /// </summary>
        private int dataStructureIndex;
        /// <summary>
        /// 缓存序列化步骤
        /// </summary>
        private byte step;
        /// <summary>
        /// 缓存快照历史节点集合
        /// </summary>
        private LeftArray<KeyValue<Node, int>> historyNodes;
        /// <summary>
        /// 缓存快照
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="isLoadArraySize"></param>
        internal Cache(AutoCSer.CacheServer.CacheManager cache, bool isLoadArraySize)
        {
            if (cache.Array.Length != 0)
            {
                dataStructures = cache.DataStructures.Values
                    .getLeftArray(value => new ServerDataStructure(value))
                    .GetSortDesc(value => value.Identity.Index);
                if (isLoadArraySize)
                {
                    array = cache.Array.copy();
                    cache.FreeIndexs.SortDesc();
                    step = 1;
                }
                else step = 2;
                stream = new UnmanagedStream(64 << 10);
            }
            else step = 4;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
        }
        /// <summary>
        /// 获取下一个序列化数据大小
        /// </summary>
        /// <returns></returns>
        internal int NextSize()
        {
            switch (step)
            {
                case 0:
                    NODE:
                    while (!historyNodes[historyNodes.Length - 1].Key.Serialize(this))
                    {
                        if (--historyNodes.Length == 0)
                        {
                            ++dataStructureIndex;
                            step = 2;
                            goto NEXT;
                        }
                    }
                    break;
                case 1: loadArraySize(); break;
                case 2:
                    NEXT:
                    if (dataStructureIndex == dataStructures.Length)
                    {
                        step = 4;
                        if (array != null) loadIndexIdentity();
                        else return 0;
                    }
                    else
                    {
                        dataStructures[dataStructureIndex].Serialize(stream);
                        step = 3;
                    }
                    break;
                case 3:
                    stream.ByteSize = OperationParameter.Serializer.HeaderSize + IndexIdentity.SerializeSize;
                    historyNodes.Length = 0;
                    historyNodes.Add(new KeyValue<Node, int>(dataStructures[dataStructureIndex].Node, OperationParameter.Serializer.HeaderSize + IndexIdentity.SerializeSize));
                    step = 0;
                    goto NODE;
                case 4: return 0;
            }
            return stream.ByteSize;
        }
        /// <summary>
        /// 重建缓存数组大小
        /// </summary>
        private void loadArraySize()
        {
            byte* write = stream.GetPrepSizeCurrent(OperationParameter.Serializer.HeaderSize + sizeof(int));
            *(int*)write = OperationParameter.Serializer.HeaderSize + sizeof(int);
            *(uint*)(write + OperationParameter.Serializer.OperationTypeOffset) = (ushort)OperationParameter.OperationType.LoadArraySize;
            *(int*)(write + OperationParameter.Serializer.HeaderSize) = array.Length;
            stream.ByteSize += OperationParameter.Serializer.HeaderSize + sizeof(int);
            step = 2;
        }
        /// <summary>
        /// 加载服务端数据结构索引标识信息
        /// </summary>
        private void loadIndexIdentity()
        {
            stream.ByteSize = 0;
            int size = OperationParameter.Serializer.HeaderSize + sizeof(int) + sizeof(ulong) * array.Length;
            byte* write = stream.GetPrepSizeCurrent(size);
            *(int*)write = size;
            *(uint*)(write + OperationParameter.Serializer.OperationTypeOffset) = (ushort)OperationParameter.OperationType.LoadIndexIdentity;
            *(int*)(write + OperationParameter.Serializer.HeaderSize) = array.Length;
            write += OperationParameter.Serializer.HeaderSize + sizeof(int) + array.Length * sizeof(ulong);
            foreach (DataStructureItem item in array)
            {
                write -= sizeof(ulong);
                *(ulong*)write = item.Identity;
            }
            stream.ByteSize += size;
        }
        /// <summary>
        /// 序列化结束处理
        /// </summary>
        /// <param name="operationType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serializeEnd(OperationParameter.OperationType operationType)
        {
            byte* write = stream.Data.Byte;
            *(int*)write = stream.ByteSize;
            *(uint*)(write + OperationParameter.Serializer.OperationTypeOffset) = (ushort)operationType;
        }
        /// <summary>
        /// 参数数据序列化以后添加缓存快照历史节点
        /// </summary>
        /// <param name="node"></param>
        internal void CreateNode(Node node)
        {
            stream.ByteSize = historyNodes[historyNodes.Length - 1].Value;
            Parameter.Serialize(stream);
            historyNodes.Add(new KeyValue<Node, int>(node, stream.ByteSize));
            serializeEnd(OperationParameter.OperationType.GetOrCreateNode);
        }
        /// <summary>
        /// 参数数据序列化
        /// </summary>
        /// <param name="operationType"></param>
        internal void SerializeParameter(OperationParameter.OperationType operationType)
        {
            stream.ByteSize = historyNodes[historyNodes.Length - 1].Value;
            Parameter.Serialize(stream);
            serializeEnd(operationType);
        }
        /// <summary>
        /// 参数数据序列化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeParameterStart()
        {
            stream.ByteSize = historyNodes[historyNodes.Length - 1].Value;
            Parameter.Serialize(stream);
        }
        /// <summary>
        /// 参数数据序列化
        /// </summary>
        /// <param name="operationType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SerializeParameterEnd(OperationParameter.OperationType operationType)
        {
            Parameter.Serialize(stream);
            serializeEnd(operationType);
        }
        /// <summary>
        /// 序列化数据复制
        /// </summary>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(UnmanagedStream stream)
        {
            stream.WriteNotEmpty(this.stream.Data.Byte, this.stream.ByteSize);
        }
        /// <summary>
        /// 序列化数据复制
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TryCopyTo(UnmanagedStream stream)
        {
            if (stream.FreeSize - sizeof(int) >= this.stream.ByteSize)
            {
                stream.WriteNotEmpty(this.stream.Data.Byte, this.stream.ByteSize);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 序列化数据复制
        /// </summary>
        /// <param name="write"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(byte* write)
        {
            AutoCSer.Memory.CopyNotNull(stream.Data.Byte, write, stream.ByteSize);
        }
    }
}
