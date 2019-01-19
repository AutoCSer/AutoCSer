using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Data.Common;
using System.Runtime.CompilerServices;

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
    public sealed partial class QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType>
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
                                counter.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                            }
                        }
                        else values.Add(getDictionaryKey(cacheValue), counter.Add(cacheValue));
                    }
                    else if (oldValues != null)
                    {
                        if (oldValues.Remove(getDictionaryKey(value))) counter.Remove(cacheValue);
                        else counter.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
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
                counter.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 获取字典缓存
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key">关键字</param>
        /// <returns>字典缓存</returns>
        private Dictionary<RandomKey<dictionaryKeyType>, valueType> getDictionary(ref DbConnection connection, keyType key)
        {
            Dictionary<RandomKey<dictionaryKeyType>, valueType> values = queueCache.Get(ref key, null);
            if (values == null)
            {
                values = DictionaryCreator<RandomKey<dictionaryKeyType>>.Create<valueType>();
                foreach (valueType value in counter.SqlTable.SelectQueue(ref connection, getWhere(key), counter.MemberMap))
                {
                    values.Add(getDictionaryKey(value), counter.Add(value));
                }
                queueCache[key] = values;
            }
            return values;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class GetTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType> queue;
            /// <summary>
            /// 返回值
            /// </summary>
            private valueType value;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 字典关键字
            /// </summary>
            private dictionaryKeyType dictionaryKey;
            /// <summary>
            /// 等待缓存加载
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="key">关键字</param>
            /// <param name="dictionaryKey">字典关键字</param>
            internal GetTask(QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType> queue, keyType key, dictionaryKeyType dictionaryKey)
            {
                this.queue = queue;
                this.key = key;
                this.dictionaryKey = dictionaryKey;
                wait.Set(0);
            }
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    value = queue.get(ref connection, key, dictionaryKey);
                }
                finally
                {
                    wait.Set();
                }
                return LinkNext;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal valueType Wait()
            {
                wait.Wait();
                return value;
            }
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key">关键字</param>
        /// <param name="dictionaryKey">字典关键字</param>
        /// <returns>匹配数据</returns>
        private valueType get(ref DbConnection connection, keyType key, dictionaryKeyType dictionaryKey)
        {
            Dictionary<RandomKey<dictionaryKeyType>, valueType> values = getDictionary(ref connection, key);
            if (values != null)
            {
                valueType value;
                if (values.TryGetValue(dictionaryKey, out value)) return value;
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="dictionaryKey">字典关键字</param>
        /// <param name="nullValue">失败返回值</param>
        /// <returns>匹配数据</returns>
        public valueType Get(keyType key, dictionaryKeyType dictionaryKey, valueType nullValue)
        {
            GetTask task = new GetTask(this, key, dictionaryKey);
            counter.SqlTable.AddQueue(task);
            return task.Wait() ?? nullValue;
        }
    }
}
