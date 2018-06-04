using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class ServerDataStructureBase
    {
        /// <summary>
        /// 缓存名称标识
        /// </summary>
        internal readonly string CacheName;
        /// <summary>
        /// 数据结构索引标识
        /// </summary>
        internal IndexIdentity Identity;
        /// <summary>
        /// 节点序列化数据
        /// </summary>
        internal SubArray<byte> NodeData;
        /// <summary>
        /// 服务端数据结构定义信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="identity">数据结构索引标识</param>
        /// <param name="nodeData">节点序列化数据</param>
        internal ServerDataStructureBase(string cacheName, IndexIdentity identity, SubArray<byte> nodeData)
        {
            CacheName = cacheName;
            Identity = identity;
            NodeData = nodeData;
        }
    }
    /// <summary>
    /// 服务端数据结构定义信息
    /// </summary>
    internal sealed class ServerDataStructure : ServerDataStructureBase
    {
        /// <summary>
        /// 缓存节点
        /// </summary>
        internal readonly Cache.Node Node;
        /// <summary>
        /// 返回类型
        /// </summary>
        internal readonly ReturnType ReturnType;
        /// <summary>
        /// 服务端数据结构定义信息
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="dataStructureBuffer">服务端数据结构定义数据</param>
        internal unsafe ServerDataStructure(CacheManager cache, Buffer buffer, ref DataStructureBuffer dataStructureBuffer) : base(dataStructureBuffer.CacheName, dataStructureBuffer.Identity, dataStructureBuffer.Data)
        {
            ReturnType = ReturnType.ServerDataStructureCreateError;
            fixed (byte* dataFixed = NodeData.Array)
            {
                byte* start = dataFixed + NodeData.Start;
                DataStructureParser nodeParser = new DataStructureParser(start, start + NodeData.Length);
                Type nodeType = nodeParser.Parse();
                if (nodeType != null)
                {
                    Cache.NodeInfo nodeInfo = (Cache.NodeInfo)nodeType.GetField(Cache.Node.NodeInfoFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
                    if (nodeInfo != null)
                    {
                        if (cache.IsFile || !nodeInfo.IsCacheFile || cache.SlaveServer != null)
                        {
                            OperationParameter.NodeParser nodeParameter = new OperationParameter.NodeParser(cache, buffer, dataFixed);
                            nodeParameter.Read += IndexIdentity.SerializeSize;
                            Node = nodeInfo.CallConstructor(ref nodeParameter);
                            NodeData.Length = (int)(nodeParser.Read - start);
                            byte[] data = NodeData.GetArray();
                            NodeData.Set(data, 0, data.Length);
                        }
                        else ReturnType = ReturnType.CacheNodeNeedFile;
                    }
                    else ReturnType = ReturnType.NotFoundCacheNodeInfo;
                }
                else ReturnType = ReturnType.CacheNodeParseError;
            }
        }
        /// <summary>
        /// 判断节点序列化数据是否匹配
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool IsNodeData(ref SubArray<byte> data)
        {
            if (data.Length >= NodeData.Length)
            {
                data.Length = NodeData.Length;
                return NodeData.equal(ref data);
            }
            return false;
        }
    }
}
