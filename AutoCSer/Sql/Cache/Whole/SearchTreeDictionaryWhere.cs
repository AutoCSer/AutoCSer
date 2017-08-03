using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 搜索树缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    public class SearchTreeDictionaryWhere<valueType, modelType, sortType> : SearchTreeDictionary<valueType, modelType, sortType>
        where valueType : class, modelType
        where modelType : class
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private readonly Func<valueType, bool> isValue;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        public SearchTreeDictionaryWhere
            (Event.Cache<valueType, modelType> cache, Func<valueType, sortType> getSort, Func<valueType, bool> isValue)
            : base(cache, getSort, false)
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (isValue(oldValue)) onDeleted(oldValue);
            if (isValue(value)) onInserted(cacheValue);
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
