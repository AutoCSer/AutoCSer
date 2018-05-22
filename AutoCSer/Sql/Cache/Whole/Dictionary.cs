using System;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class Dictionary<valueType, modelType, keyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 分组字典关键字获取器
        /// </summary>
        protected readonly Func<valueType, keyType> getKey;
        /// <summary>
        /// 字典缓存
        /// </summary>
        protected readonly Dictionary<RandomKey<keyType>, valueType> dictionary;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 字典缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        public Dictionary(Event.Cache<valueType, modelType> cache, Func<valueType, keyType> getKey, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            this.cache = cache;
            this.getKey = getKey;

            if (isReset)
            {
                dictionary = DictionaryCreator<RandomKey<keyType>>.Create<valueType>(cache.ValueCount);
                foreach (valueType value in cache.Values) dictionary.Add(getKey(value), value);
                cache.OnInserted += onInserted;
                cache.OnUpdated += onUpdated;
                cache.OnDeleted += onDeleted;
            }
            else dictionary = DictionaryCreator<RandomKey<keyType>>.Create<valueType>();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onInserted(valueType value)
        {
            onInserted(value, getKey(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key">数据对象的关键字</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onInserted(valueType value, keyType key)
        {
            dictionary.Add(key, value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        protected void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            keyType key = getKey(value), oldKey = getKey(oldValue);
            if (!key.Equals(oldKey))
            {
                onInserted(cacheValue, key);
                onDeleted(oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">被删除数据的关键字</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(keyType key)
        {
            if (!dictionary.Remove(key))
            {
                cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(getKey(value));
        }
    }
}
