using System;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 计数缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public abstract class Cache<valueType, modelType, keyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 缓存计数器
        /// </summary>
        protected readonly Event.Cache<valueType, modelType, keyType> counter;
        /// <summary>
        /// 计数缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        protected Cache(Event.Cache<valueType, modelType, keyType> counter)
        {
            if (counter == null) throw new ArgumentNullException();
            this.counter = counter;
        }
    }
}
