using System;
using AutoCSer.Metadata;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType">缓存数据类型</typeparam>
    public abstract class Queue<valueType, modelType, counterKeyType, keyType, cacheValueType>
        : Cache<valueType, modelType, counterKeyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where counterKeyType : IEquatable<counterKeyType>
        where cacheValueType : class
    {
        /// <summary>
        /// 缓存关键字获取器
        /// </summary>
        protected readonly Func<modelType, keyType> getKey;
        /// <summary>
        /// 数据集合
        /// </summary>
        protected readonly FifoPriorityQueue<keyType, cacheValueType> queueCache = new FifoPriorityQueue<keyType, cacheValueType>();
        /// <summary>
        /// 缓存默认最大容器大小
        /// </summary>
        protected readonly int maxCount;
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        protected Queue(Event.Cache<valueType, modelType, counterKeyType> counter, Func<modelType, keyType> getKey, int maxCount)
            : base(counter)
        {
            if (getKey == null) throw new ArgumentNullException();
            this.getKey = getKey;
            this.maxCount = maxCount <= 0 ? ConfigLoader.Config.CacheMaxCount : maxCount;
        }
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        protected Queue(Event.Cache<valueType, modelType, counterKeyType> counter, Expression<Func<modelType, keyType>> getKey, int maxCount)
            : base(counter)
        {
            if (getKey == null) throw new ArgumentNullException();
            counter.SqlTable.SetSelectMember(getKey);
            this.getKey = getKey.Compile();
            this.maxCount = maxCount <= 0 ? ConfigLoader.Config.CacheMaxCount : maxCount;
        }
    }
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed partial class Queue<valueType, modelType, keyType>
        : Queue<valueType, modelType, keyType, keyType, valueType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 数据获取器
        /// </summary>
        private readonly Table<valueType, modelType, keyType>.GetValue getValue;
        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public valueType this[keyType key]
        {
            get
            {
                GetTask task = new GetTask(this, key);
                counter.SqlTable.AddQueue(task);
                return task.Wait();
            }
        }
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getValue">数据获取器,禁止数据库与锁操作</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public Queue(Event.Cache<valueType, modelType, keyType> counter
            , Table<valueType, modelType, keyType>.GetValue getValue, int maxCount = 0)
            : base(counter, counter.GetKey, maxCount)
        {
            if (getValue == null) throw new ArgumentNullException();
            this.getValue = getValue;

            //counter.OnReset += reset;
            counter.SqlTable.OnInserted += onInserted;
            counter.OnUpdated += onUpdated;
            counter.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onInserted(valueType value)
        {
            onInserted(value, getKey(value));
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        /// <param name="key">关键字</param>
        private void onInserted(valueType value, keyType key)
        {
            queueCache[key] = counter.Add(value);
            if (queueCache.Count > maxCount) counter.Remove(queueCache.UnsafePopValue());
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
            if (cacheValue != null)
            {
                keyType key = getKey(value), oldKey = getKey(oldValue);
                if (!key.Equals(oldKey))
                {
                    valueType removeValue;
                    if (queueCache.Remove(ref oldKey, out removeValue)) queueCache.Set(ref key, cacheValue);
                    else onInserted(cacheValue, key);
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onDeleted(valueType value)
        {
            keyType key = getKey(value);
            queueCache.Remove(ref key, out value);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class GetTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private Queue<valueType, modelType, keyType> queue;
            /// <summary>
            /// 返回值
            /// </summary>
            private valueType value;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 等待缓存加载
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="key"></param>
            internal GetTask(Queue<valueType, modelType, keyType> queue, keyType key)
            {
                this.queue = queue;
                this.key = key;
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
                    value = queue.get(ref connection, key);
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
        /// 获取缓存数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private valueType get(ref DbConnection connection, keyType key)
        {
            valueType value = queueCache.Get(ref key, null);
            if (value != null) return value;
            if (getKey == counter.GetKey)
            {
                value = counter.Get(key);
                if (value != null) return value;
            }
            if ((value = getValue(ref connection, key, counter.MemberMap)) != null) onInserted(value);
            return value;
        }
    }
}
