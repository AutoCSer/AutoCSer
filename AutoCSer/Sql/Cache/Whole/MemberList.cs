using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoCSer.Extension;
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
    /// <typeparam name="targetType">目标数据类型</typeparam>
    public class MemberList<valueType, modelType, keyType, targetType> : Member<valueType, modelType, keyType, targetType, ListArray<valueType>>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where targetType : class
    {
        /// <summary>
        /// 移除数据并使用最后一个数据移动到当前位置
        /// </summary>
        protected readonly bool isRemoveEnd;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getValue">获取目标对象委托</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getTargets">获取缓存目标对象集合</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public MemberList(Event.Cache<valueType, modelType> cache, Func<modelType, keyType> getKey
            , Func<keyType, targetType> getValue, Expression<Func<targetType, ListArray<valueType>>> member
            , Func<IEnumerable<targetType>> getTargets, bool isRemoveEnd = false, bool isReset = true)
            : base(cache, getKey, getValue, member, getTargets)
        {
            this.isRemoveEnd = isRemoveEnd;

            if (isReset)
            {
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
            onInserted(value, getKey(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key"></param>
        protected void onInserted(valueType value, keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                ListArray<valueType> list = getMember(target);
                if (list == null) setMember(target, list = new ListArray<valueType>());
                list.Add(value);
            }
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
            keyType oldKey = getKey(oldValue), newKey = getKey(value);
            if (!newKey.Equals(oldKey))
            {
                onInserted(cacheValue, newKey);
                onDeleted(cacheValue, oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除的数据关键字</param>
        protected void onDeleted(valueType value, keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                ListArray<valueType> list = getMember(target);
                if (list != null)
                {
                    int index = Array.LastIndexOf(list.Array, value, list.Length - 1);
                    if (index != -1)
                    {
                        if (isRemoveEnd) list.RemoveAtEnd(index);
                        else list.RemoveAt(index);
                        return;
                    }
                }
                cache.SqlTable.Log.add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getKey(value));
        }
    }
}
