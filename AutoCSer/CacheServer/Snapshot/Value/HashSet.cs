using System;

namespace AutoCSer.CacheServer.Snapshot.Value
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class HashSet<valueType> : Node
        where valueType : IEquatable<valueType>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private valueType[] array;
        /// <summary>
        /// 哈希表节点
        /// </summary>
        /// <param name="array"></param>
        internal HashSet(valueType[] array)
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
                ValueData.Data<valueType>.SetData(ref cache.Parameter, array[index]);
                cache.SerializeParameter(OperationParameter.OperationType.SetValue);
                ++index;
                return true;
            }
            array = null;
            return false;
        }
    }
}
