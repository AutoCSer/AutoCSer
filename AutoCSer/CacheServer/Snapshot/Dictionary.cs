using System;

namespace AutoCSer.CacheServer.Snapshot
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    internal sealed class Dictionary<keyType> : Node
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private KeyValue<keyType, Node>[] array;
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="array"></param>
        internal Dictionary(KeyValue<keyType, Node>[] array)
        {
            this.array = array;
        }
        /// <summary>
        /// 缓存快照序列化
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        internal override bool Serialize(Cache cache)
        {
            if (index != array.Length)
            {
                ValueData.Data<keyType>.SetData(ref cache.Parameter, array[index].Key);
                cache.CreateNode(array[index].Value);
                ++index;
                return true;
            }
            array = null;
            return false;
        }
    }
}
