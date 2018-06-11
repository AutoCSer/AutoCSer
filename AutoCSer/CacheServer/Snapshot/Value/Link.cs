using System;

namespace AutoCSer.CacheServer.Snapshot.Value
{
    /// <summary>
    /// 链表 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Link<valueType> : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private valueType[] array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="array"></param>
        internal Link(valueType[] array)
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
                cache.Parameter.Set(-1);
                cache.SerializeParameterStart();
                ValueData.Data<valueType>.SetData(ref cache.Parameter, array[index]);
                cache.SerializeParameterEnd(OperationParameter.OperationType.InsertAfter);
                ++index;
                return true;
            }
            array = null;
            return false;
        }
    }
}
