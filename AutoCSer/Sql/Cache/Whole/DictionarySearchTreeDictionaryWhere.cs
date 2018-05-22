using System;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 字典+搜索树缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">分组关键字类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    public class DictionarySearchTreeDictionaryWhere<valueType, modelType, keyType, sortType> : DictionarySearchTreeDictionary<valueType, modelType, keyType, sortType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 缓存值判定
        /// </summary>
        private readonly Func<valueType, bool> isValue;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组关键字获取器</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isValue">缓存值判定</param>
        public DictionarySearchTreeDictionaryWhere(Event.Cache<valueType, modelType> cache, Func<valueType, keyType> getKey, Func<valueType, sortType> getSort, Func<valueType, bool> isValue)
            : base(cache, getKey, getSort, false)
        {
            if (isValue == null) throw new ArgumentNullException();
            this.isValue = isValue;

            foreach (valueType value in cache.Values) onInserted(value);
            cache.OnInserted += onInserted;
            cache.OnUpdated += onUpdated;
            cache.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onInserted(valueType value)
        {
            if (isValue(value)) base.onInserted(value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        protected new void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (isValue(value))
            {
                if (isValue(oldValue)) base.onUpdated(cacheValue, value, oldValue, memberMap);
                else base.onInserted(cacheValue);
            }
            else if (isValue(oldValue)) onDeleted(oldValue, getKey(oldValue));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onDeleted(valueType value)
        {
            if (isValue(value)) base.onDeleted(value);
        }
    }
}
