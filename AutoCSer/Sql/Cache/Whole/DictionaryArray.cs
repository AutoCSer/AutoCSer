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
    public class DictionaryArray<valueType, modelType, keyType>
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
        internal DictionaryArray<RandomKey<keyType>, valueType> Array;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get { return Array.Count; } }
        /// <summary>
        /// 字典缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        public DictionaryArray(Event.Cache<valueType, modelType> cache, Func<valueType, keyType> getKey, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            this.cache = cache;
            this.getKey = getKey;

            if (isReset)
            {
                Array.Reset();
                foreach (valueType value in cache.Values) onInserted(value);
                cache.OnInserted += onInserted;
                cache.OnUpdated += onUpdated;
                cache.OnDeleted += onDeleted;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onInserted(valueType value)
        {
            Array.Add(getKey(value), value);
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
                Array.Add(key, cacheValue);
                Array.Remove(oldKey, out value);
                if (value == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            Array.Remove(getKey(value), out value);
            if (value == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
    }
}
