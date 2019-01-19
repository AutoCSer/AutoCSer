using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Counter.Event
{
    /// <summary>
    /// 缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public abstract class Cache<valueType, modelType, keyType> : Copy<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 缓存关键字获取器
        /// </summary>
        internal readonly Func<modelType, keyType> GetKey;
        /// <summary>
        /// 缓存数据
        /// </summary>
        private Dictionary<RandomKey<keyType>, KeyValue<valueType, int>> dictionary;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存值</returns>
        public valueType this[keyType key]
        {
            get
            {
                return Get(key);
            }
        }
        /// <summary>
        /// 缓存计数
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="group">数据分组</param>
        protected Cache(Sql.Table<valueType, modelType> sqlTool, int group, Expression<Func<modelType, keyType>> getKey)
            : this(sqlTool, group, getKey == null ? null : getKey.Compile())
        {
            sqlTool.SetSelectMember(getKey);
        }
        /// <summary>
        /// 缓存计数
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="group">数据分组</param>
        protected Cache(Sql.Table<valueType, modelType> sqlTool, int group, Func<modelType, keyType> getKey)
            : base(sqlTool, group)
        {
            if (getKey == null) throw new ArgumentNullException();
            GetKey = getKey;
            dictionary = DictionaryCreator<RandomKey<keyType>>.Create<KeyValue<valueType, int>>();

            sqlTool.IsOnlyQueue = true;
            sqlTool.OnUpdated += onUpdated;
            sqlTool.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType Get(keyType key)
        {
            KeyValue<valueType, int> valueCount;
            return dictionary.TryGetValue(key, out valueCount) ? valueCount.Key : null;
        }
        /// <summary>
        /// 更新记录事件 [缓存数据 + 更新后的数据 + 更新前的数据 + 更新数据成员]
        /// </summary>
        public event OnCacheUpdated OnUpdated;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            KeyValue<valueType, int> cacheValue;
            keyType key = GetKey(value);
            if (dictionary.TryGetValue(key, out cacheValue)) update(cacheValue.Key, value, oldValue, memberMap);
            if (OnUpdated != null) OnUpdated(cacheValue.Key, value, oldValue, memberMap);
        }
        /// <summary>
        /// 删除记录事件
        /// </summary>
        public event Action<valueType> OnDeleted;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            KeyValue<valueType, int> cacheValue;
            keyType key = GetKey(value);
            if (dictionary.TryGetValue(key, out cacheValue))
            {
                dictionary.Remove(GetKey(value));
                if (OnDeleted != null) OnDeleted(cacheValue.Key);
            }
        }
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>缓存数据</returns>
        internal valueType Add(valueType value)
        {
            KeyValue<valueType, int> valueCount;
            keyType key = GetKey(value);
            if (dictionary.TryGetValue(key, out valueCount))
            {
                ++valueCount.Value;
                dictionary[key] = valueCount;
                return valueCount.Key;
            }
            valueType copyValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<modelType>.Copy(copyValue, value, MemberMap);
            dictionary.Add(key, new KeyValue<valueType, int>(copyValue, 0));
            return copyValue;
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        internal void Remove(valueType value)
        {
            KeyValue<valueType, int> valueCount;
            keyType key = GetKey(value);
            if (dictionary.TryGetValue(key, out valueCount))
            {
                if (valueCount.Value == 0) dictionary.Remove(key);
                else
                {
                    --valueCount.Value;
                    dictionary[key] = valueCount;
                }
            }
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }

        /// <summary>
        /// 创建先进先出优先队列缓存
        /// </summary>
        /// <param name="getValue">数据获取器,禁止数据库与锁操作</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        /// <returns></returns>
        public Queue<valueType, modelType, keyType> CreateQueue(Table<valueType, modelType, keyType>.GetValue getValue, int maxCount = 0)
        {
            return new Queue<valueType, modelType, keyType>(this, getValue, maxCount);
        }
    }
}
