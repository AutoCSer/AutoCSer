using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端数据结构定义信息
    /// </summary>
    internal sealed partial class ClientDataStructure
    {
        /// <summary>
        /// 客户端
        /// </summary>
        internal readonly Client Client;
        /// <summary>
        /// 缓存名称标识
        /// </summary>
        internal readonly string CacheName;
        /// <summary>
        /// 数据结构定义节点类型
        /// </summary>
        internal readonly Type NodeType;
        /// <summary>
        /// 数据结构定义节点
        /// </summary>
        internal readonly DataStructure.Abstract.Node Node;
        /// <summary>
        /// 数据节点
        /// </summary>
        private readonly DataStructure.Abstract.Node valueNode;
        /// <summary>
        /// 服务端数据结构索引标识
        /// </summary>
        internal IndexIdentity Identity;
        /// <summary>
        /// 数据结构定义信息
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="nodeType">数据结构定义节点类型</param>
        /// <param name="node">数据结构定义节点</param>
        internal ClientDataStructure(Client client, string cacheName, Type nodeType, DataStructure.Abstract.Node node)
        {
            Client = client;
            CacheName = cacheName;
            NodeType = nodeType;
            Node = node;
            if ((valueNode = node.CreateValueNode()) != null && valueNode is DataStructure.Abstract.IValueNode && Identity.Set(client.GetOrCreate(this))) Node.Parameter.Value = this;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        internal unsafe void SerializeOperationParameter(UnmanagedStream stream)
        {
            OperationParameter.Serializer operationSerializer = new OperationParameter.Serializer(stream);
            Identity.UnsafeSerialize(stream);
            fixed (char* nameFixed = CacheName) AutoCSer.BinarySerialize.Serializer.Serialize(nameFixed, stream, CacheName.Length);
            int startIndex = stream.ByteSize;
            valueNode.SerializeDataStructure(stream);
            stream.SerializeFillWithStartIndex(startIndex);
            operationSerializer.End(OperationParameter.OperationType.GetOrCreateDataStructure);
        }
    }
}
