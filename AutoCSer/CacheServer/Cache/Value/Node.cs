using System;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 数据节点
    /// </summary>
    internal abstract class Node : Cache.Node
    {
        /// <summary>
        /// 构造参数
        /// </summary>
        internal byte[] ConstructorParameter = NullValue<byte>.Array;
        /// <summary>
        /// 缓存节点
        /// </summary>
        /// <param name="parent"></param>
        protected Node(Cache.Node parent) : base(parent) { }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="parser"></param>
        internal override void CreateShortPath(ref OperationParameter.NodeParser parser)
        {
            parser.Cache.CreateShortPath(this, ref parser);
        }
    }
}
