using System;

namespace AutoCSer.CacheServer.Snapshot.Value
{
    /// <summary>
    /// 字典 数据节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Dictionary<keyType, valueType> : Node
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private KeyValue<keyType, valueType>[] array;
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="array"></param>
        internal Dictionary(KeyValue<keyType, valueType>[] array)
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
                cache.SerializeParameterStart();
                ValueData.Data<valueType>.SetData(ref cache.Parameter, array[index].Value);
                cache.SerializeParameterEnd(OperationParameter.OperationType.SetValue);
                ++index;
                return true;
            }
            array = null;
            return false;
        }
    }
}
