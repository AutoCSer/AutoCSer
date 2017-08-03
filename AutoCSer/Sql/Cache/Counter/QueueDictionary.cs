using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列 字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="dictionaryKeyType">字典关键字类型</typeparam>
    public sealed class QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType>
        : QueueExpression<valueType, modelType, counterKeyType, keyType, Dictionary<RandomKey<dictionaryKeyType>, valueType>>
        where valueType : class, modelType
        where modelType : class
        where counterKeyType : IEquatable<counterKeyType>
        where keyType : IEquatable<keyType>
        where dictionaryKeyType : IEquatable<dictionaryKeyType>
    {
        /// <summary>
        /// 缓存字典关键字获取器
        /// </summary>
        private readonly Func<valueType, dictionaryKeyType> getDictionaryKey;
        /// <summary>
        /// 先进先出优先队列 字典缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="getDictionaryKey">缓存字典关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public QueueDictionary(Event.Cache<valueType, modelType, counterKeyType> counter
            , Expression<Func<modelType, keyType>> getKey, Func<keyType, Expression<Func<modelType, bool>>> getWhere
            , Func<valueType, dictionaryKeyType> getDictionaryKey
            , int maxCount = 0)
            : base(counter, getKey, maxCount, getWhere)
        {
            if (getDictionaryKey == null) throw new ArgumentNullException();
            this.getDictionaryKey = getDictionaryKey;

            //counter.OnReset += reset;
            counter.SqlTable.OnInserted += onInserted;
            counter.OnUpdated += onUpdated;
            counter.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            keyType key = getKey(value);
            Dictionary<RandomKey<dictionaryKeyType>, valueType> values = queueCache.Get(ref key, null);
            if (values != null) values.Add(getDictionaryKey(value), counter.Add(value));
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue">缓存数据</param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        private void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            keyType key = getKey(value);
            if (cacheValue == null)
            {
                Dictionary<RandomKey<dictionaryKeyType>, valueType> values;
                if (queueCache.Remove(ref key, out values))
                {
                    foreach (valueType removeValue in values.Values) counter.Remove(removeValue);
                }
            }
            else
            {
                keyType oldKey = getKey(oldValue);
                if (key.Equals(oldKey))
                {
                    Dictionary<RandomKey<dictionaryKeyType>, valueType> values = queueCache.Get(ref key, null);
                    if (values != null)
                    {
                        dictionaryKeyType dictionaryKey = getDictionaryKey(cacheValue), oldDictionaryKey = getDictionaryKey(oldValue);
                        if (!dictionaryKey.Equals(oldDictionaryKey))
                        {
                            values.Add(dictionaryKey, cacheValue);
                            values.Remove(oldDictionaryKey);
                        }
                    }
                }
                else
                {
                    Dictionary<RandomKey<dictionaryKeyType>, valueType> values = queueCache.Get(ref key, null);
                    Dictionary<RandomKey<dictionaryKeyType>, valueType> oldValues = queueCache.Get(ref oldKey, null);
                    if (values != null)
                    {
                        if (oldValues != null)
                        {
                            values.Add(getDictionaryKey(cacheValue), cacheValue);
                            if (!oldValues.Remove(getDictionaryKey(oldValue)))
                            {
                                counter.SqlTable.Log.add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                            }
                        }
                        else values.Add(getDictionaryKey(cacheValue), counter.Add(cacheValue));
                    }
                    else if (oldValues != null)
                    {
                        if (oldValues.Remove(getDictionaryKey(value))) counter.Remove(cacheValue);
                        else counter.SqlTable.Log.add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                    }
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            keyType key = getKey(value);
            Dictionary<RandomKey<dictionaryKeyType>, valueType> values = queueCache.Get(ref key, null);
            if (values != null && !values.Remove(getDictionaryKey(value)))
            {
                counter.SqlTable.Log.add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
    }
}
