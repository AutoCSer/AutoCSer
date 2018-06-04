using System;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 数据节点
    /// </summary>
    internal abstract class Node : Cache.Node
    {
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved() { }
    }
}
