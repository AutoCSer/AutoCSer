using System;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列 列表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="counterKeyType">缓存统计关键字类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public partial class QueueList<valueType, modelType, counterKeyType, keyType>
        : QueueExpression<valueType, modelType, counterKeyType, keyType, ListArray<valueType>>
        where valueType : class, modelType
        where modelType : class
        where counterKeyType : IEquatable<counterKeyType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 先进先出优先队列 列表缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public QueueList(Event.Cache<valueType, modelType, counterKeyType> counter
            , Expression<Func<modelType, keyType>> getKey, Func<keyType, Expression<Func<modelType, bool>>> getWhere, int maxCount = 0)
            : base(counter, getKey, maxCount, getWhere)
        {
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
            ListArray<valueType> values = queueCache.Get(ref key, null);
            if (values != null) values.Add(counter.Add(value));
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
                ListArray<valueType> values;
                if (queueCache.Remove(ref key, out values))
                {
                    foreach (valueType removeValue in values) counter.Remove(removeValue);
                }
            }
            else
            {
                keyType oldKey = getKey(oldValue);
                if (!key.Equals(oldKey))
                {
                    ListArray<valueType> values = queueCache.Get(ref key, null), oldValues = queueCache.Get(ref oldKey, null);
                    if (values != null)
                    {
                        if (oldValues != null)
                        {
                            values.Add(cacheValue);
                            if (!oldValues.Remove(cacheValue)) counter.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                        }
                        else values.Add(counter.Add(cacheValue));
                    }
                    else if (oldValues != null)
                    {
                        if (oldValues.Remove(cacheValue)) counter.Remove(cacheValue);
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
            ListArray<valueType> values = queueCache.Get(ref key, null);
            if (values != null && !values.Remove(value)) counter.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
        /// <summary>
        /// 读取数据库数据列表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key">关键字</param>
        /// <returns>数据列表</returns>
        private ListArray<valueType> createList(ref DbConnection connection, keyType key)
        {
            ListArray<valueType> list = new ListArray<valueType>(counter.SqlTable.SelectQueue(ref connection, getWhere(key), counter.MemberMap));
            if (list != null)
            {
                if (list.Length != 0)
                {
                    int index = 0, count = list.Length;
                    valueType[] array = list.Array;
                    foreach (valueType value in array)
                    {
                        array[index] = counter.Add(value);
                        if (++index == count) break;
                    }
                }
            }
            else list = new ListArray<valueType>();
            queueCache[key] = list;
            if (queueCache.Count > maxCount)
            {
                foreach (valueType value in queueCache.UnsafePopValue()) counter.Remove(value);
            }
            return list;
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private ListArray<valueType> getList(ref DbConnection connection, keyType key)
        {
            return queueCache.Get(ref key, null) ?? createList(ref connection, key);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class GetArrayTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private QueueList<valueType, modelType, counterKeyType, keyType> queue;
            /// <summary>
            /// 返回值
            /// </summary>
            private valueType[] value;
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
            internal GetArrayTask(QueueList<valueType, modelType, counterKeyType, keyType> queue, keyType key)
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
                    value = new LeftArray<valueType>(queue.getList(ref connection, key)).GetArray();
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
            internal valueType[] Wait()
            {
                wait.Wait();
                return value;
            }
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        public valueType[] GetArray(keyType key)
        {
            GetArrayTask task = new GetArrayTask(this, key);
            counter.SqlTable.AddQueue(task);
            return task.Wait();
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数据集合</returns>
        public LeftArray<valueType> GetFindArray(keyType key, Func<valueType, bool> isValue)
        {
            valueType[] array = GetArray(key);
            int index = 0;
            foreach (valueType value in array)
            {
                if (isValue(value)) array[index++] = value;
            }
            return new LeftArray<valueType>(index, array);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class FindTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private QueueList<valueType, modelType, counterKeyType, keyType> queue;
            /// <summary>
            /// 数据匹配器
            /// </summary>
            private Func<valueType, bool> isValue;
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
            /// <param name="isValue"></param>
            internal FindTask(QueueList<valueType, modelType, counterKeyType, keyType> queue, keyType key, Func<valueType, bool> isValue)
            {
                this.queue = queue;
                this.key = key;
                this.isValue = isValue;
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
                    value = new LeftArray<valueType>(queue.getList(ref connection, key)).FirstOrDefault(isValue);
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
        /// 获取一个匹配数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止数据库与锁操作</param>
        /// <returns>匹配数据,失败返回null</returns>
        public valueType FirstOrDefault(keyType key, Func<valueType, bool> isValue)
        {
            if (isValue == null) throw new ArgumentNullException();
            FindTask task = new FindTask(this, key, isValue);
            counter.SqlTable.AddQueue(task);
            return task.Wait();
        }
    }
}
