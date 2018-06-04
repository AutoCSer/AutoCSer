using System;

namespace AutoCSer.CacheServer.Snapshot
{
    /// <summary>
    /// 服务端数据结构定义信息
    /// </summary>
    internal sealed class ServerDataStructure : AutoCSer.CacheServer.ServerDataStructureBase
    {
        /// <summary>
        /// 缓存节点
        /// </summary>
        internal readonly Node Node;
        /// <summary>
        /// 服务端数据结构定义信息
        /// </summary>
        /// <param name="value"></param>
        internal ServerDataStructure(AutoCSer.CacheServer.ServerDataStructure value) : base(value.CacheName, value.Identity, value.NodeData)
        {
            Node = value.Node.CreateSnapshot();
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        internal unsafe void Serialize(UnmanagedStream stream)
        {
            stream.ByteSize = 0;
            OperationParameter.Serializer operationSerializer = new OperationParameter.Serializer(stream);
            Identity.UnsafeSerialize(stream);
            fixed (char* nameFixed = CacheName) AutoCSer.BinarySerialize.Serializer.Serialize(nameFixed, stream, CacheName.Length);
            int startIndex = stream.ByteSize;
            stream.Write(ref NodeData);
            stream.SerializeFillWithStartIndex(startIndex);
            operationSerializer.End(OperationParameter.OperationType.GetOrCreateDataStructure);
        }
    }
}
