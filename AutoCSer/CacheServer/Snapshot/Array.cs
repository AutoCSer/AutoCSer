using System;

namespace AutoCSer.CacheServer.Snapshot
{
    /// <summary>
    /// 数组节点
    /// </summary>
    internal sealed class Array : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private Node[] array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="array"></param>
        internal Array(Node[] array)
        {
            this.array = array;
            index = array.Length;
        }
        /// <summary>
        /// 缓存快照序列化
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        internal override bool Serialize(Cache cache)
        {
            while (index != 0)
            {
                if (array[--index] != null)
                {
                    cache.Parameter.Set(index);
                    cache.CreateNode(array[index]);
                    return true;
                }
            }
            array = null;
            return false;
        }
    }
}
