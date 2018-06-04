using System;
namespace AutoCSer.CacheServer.Snapshot
{
    /// <summary>
    /// 无需或者不能创建快照的节点
    /// </summary>
    internal sealed class NoSerialize : Node
    {
        /// <summary>
        /// 缓存快照序列化
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        internal override bool Serialize(Cache cache)
        {
            return false;
        }
        /// <summary>
        /// 无需或者不能创建快照的节点
        /// </summary>
        internal static readonly NoSerialize Default = new NoSerialize();
    }
}
