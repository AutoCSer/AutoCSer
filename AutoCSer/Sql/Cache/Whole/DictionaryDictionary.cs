using System;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="groupKeyType">分组关键字类型</typeparam>
    /// <typeparam name="keyType">字典关键字类型</typeparam>
    public class DictionaryDictionary<valueType, modelType, groupKeyType, keyType>
        where valueType : class, modelType
        where modelType : class
        where groupKeyType : IEquatable<groupKeyType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 分组关键字获取器
        /// </summary>
        protected readonly Func<valueType, groupKeyType> getGroupKey;
        /// <summary>
        /// 字典关键字获取器
        /// </summary>
        protected readonly Func<valueType, keyType> getKey;
        /// <summary>
        /// 分组数据
        /// </summary>
        protected readonly Dictionary<RandomKey<groupKeyType>, Dictionary<RandomKey<keyType>, valueType>> groups;
        /// <summary>
        /// 分组字典缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getGroupKey">分组关键字获取器</param>
        /// <param name="getKey">字典关键字获取器</param>
        /// <param name="isReset">是否初始化数据</param>
        public DictionaryDictionary(Event.Cache<valueType, modelType> cache
            , Func<valueType, groupKeyType> getGroupKey, Func<valueType, keyType> getKey, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getGroupKey == null) throw new ArgumentNullException("getGroupKey is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            this.cache = cache;
            this.getGroupKey = getGroupKey;
            this.getKey = getKey;

            if (isReset)
            {
                groups = DictionaryCreator<RandomKey<groupKeyType>>.Create<Dictionary<RandomKey<keyType>, valueType>>(cache.ValueCount);
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
            onInserted(value, getGroupKey(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key"></param>
        protected void onInserted(valueType value, groupKeyType key)
        {
            Dictionary<RandomKey<keyType>, valueType> values;
            if (!groups.TryGetValue(key, out values)) groups.Add(key, values = DictionaryCreator<RandomKey<keyType>>.Create<valueType>());
            values.Add(getKey(value), value);
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
            groupKeyType groupKey = getGroupKey(value), oldGroupKey = getGroupKey(oldValue);
            if (groupKey.Equals(oldGroupKey))
            {
                keyType key = getKey(value), oldKey = getKey(oldValue);
                if (!key.Equals(oldKey))
                {
                    Dictionary<RandomKey<keyType>, valueType> dictionary;
                    if (groups.TryGetValue(groupKey, out dictionary) && dictionary.Remove(oldKey))
                    {
                        dictionary.Add(key, cacheValue);
                    }
                    else cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                }
            }
            else
            {
                onInserted(cacheValue, groupKey);
                onDeleted(oldValue, oldGroupKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="groupKey">分组关键字</param>
        protected void onDeleted(valueType value, groupKeyType groupKey)
        {
            Dictionary<RandomKey<keyType>, valueType> dictionary;
            if (groups.TryGetValue(groupKey, out dictionary) && dictionary.Remove(getKey(value)))
            {
                if (dictionary.Count == 0) groups.Remove(groupKey);
            }
            else cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getGroupKey(value));
        }
        /// <summary>
        /// 获取关键字集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ICollection<RandomKey<keyType>> GetKeys(groupKeyType key)
        {
            Dictionary<RandomKey<keyType>, valueType> dictionary;
            if (groups.TryGetValue(key, out dictionary)) return dictionary.Keys;
            return NullValue<RandomKey<keyType>>.Array;
        }
    }
}
