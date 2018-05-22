using System;

namespace AutoCSer.CacheServer.Snapshot
{
    /// <summary>
    /// 缓存快照节点
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// 当前处理索引位置
        /// </summary>
        protected int index;
        /// <summary>
        /// 缓存快照序列化
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        internal abstract bool Serialize(Cache cache);
    }
}
