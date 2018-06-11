using System;

namespace AutoCSer.CacheServer.Snapshot.Value
{
    /// <summary>
    /// 数组 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Array<valueType> : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private valueType[] array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="array"></param>
        internal Array(valueType[] array)
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
                    cache.SerializeParameterStart();
                    ValueData.Data<valueType>.SetData(ref cache.Parameter, array[index]);
                    cache.SerializeParameterEnd(OperationParameter.OperationType.SetValue);
                    return true;
                }
            }
            array = null;
            return false;
        }
    }
}
