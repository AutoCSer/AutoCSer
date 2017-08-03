using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组列表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    /// <typeparam name="memberKeyType"></typeparam>
    /// <typeparam name="targetType"></typeparam>
    public sealed class MemberDictionaryListWhere<valueType, modelType, keyType, memberKeyType, targetType>
        : MemberDictionaryList<valueType, modelType, keyType, memberKeyType, targetType>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
        where memberKeyType : struct, IEquatable<memberKeyType>
        where targetType : class
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private readonly Func<valueType, bool> isValue;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getMemberKey">分组列表关键字获取器</param>
        /// <param name="getValue"></param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getTargets"></param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        public MemberDictionaryListWhere(Event.Cache<valueType, modelType> cache
            , Func<modelType, keyType> getKey, Func<modelType, memberKeyType> getMemberKey
            , Func<keyType, targetType> getValue, Expression<Func<targetType, Dictionary<RandomKey<memberKeyType>, ListArray<valueType>>>> member
            , Func<IEnumerable<targetType>> getTargets, Func<valueType, bool> isValue, bool isRemoveEnd)
            : base(cache, getKey, getMemberKey, getValue, member, getTargets, isRemoveEnd, false)
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
        private new void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (isValue(value))
            {
                if (isValue(oldValue)) base.onUpdated(cacheValue, value, oldValue, memberMap);
                else base.onInserted(cacheValue);
            }
            else if (isValue(oldValue)) base.onDeleted(cacheValue, getKey(oldValue), getMemberKey(oldValue));
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
