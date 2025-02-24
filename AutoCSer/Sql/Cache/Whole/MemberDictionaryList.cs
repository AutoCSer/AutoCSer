﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
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
    public class MemberDictionaryList<valueType, modelType, keyType, memberKeyType, targetType>
        : Member<valueType, modelType, keyType, targetType, Dictionary<RandomKey<memberKeyType>, ListArray<valueType>>>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
        where memberKeyType : struct, IEquatable<memberKeyType>
        where targetType : class
    {
        /// <summary>
        /// 关键字获取器
        /// </summary>
        protected readonly Func<modelType, memberKeyType> getMemberKey;
        /// <summary>
        /// 移除数据并使用最后一个数据移动到当前位置
        /// </summary>
        protected readonly bool isRemoveEnd;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getMemberKey"></param>
        /// <param name="getValue"></param>
        /// <param name="member"></param>
        /// <param name="getTargets"></param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public MemberDictionaryList(Event.Cache<valueType, modelType> cache
            , Func<modelType, keyType> getKey, Func<modelType, memberKeyType> getMemberKey
            , Func<keyType, targetType> getValue, Expression<Func<targetType, Dictionary<RandomKey<memberKeyType>, ListArray<valueType>>>> member
            , Func<IEnumerable<targetType>> getTargets, bool isRemoveEnd, bool isReset)
            : base(cache, getKey, getValue, member, getTargets)
        {
            if (getMemberKey == null) throw new ArgumentNullException();
            this.getMemberKey = getMemberKey;
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
        private void onInserted(valueType value, keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Debug(typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString(), LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
            else
            {
                memberKeyType memberKey = getMemberKey(value);
                Dictionary<RandomKey<memberKeyType>, ListArray<valueType>> dictionary = getMember(target);
                ListArray<valueType> list;
                if (dictionary == null)
                {
                    setMember(target, dictionary = DictionaryCreator<RandomKey<memberKeyType>>.Create<ListArray<valueType>>());
                    dictionary.Add(memberKey, list = new ListArray<valueType>());
                }
                else if (!dictionary.TryGetValue(memberKey, out list)) dictionary.Add(memberKey, list = new ListArray<valueType>());
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
            keyType key = getKey(value), oldKey = getKey(oldValue);
            if (!key.Equals(oldKey))
            {
                onInserted(cacheValue, key);
                onDeleted(cacheValue, oldKey, getMemberKey(oldValue));
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除的数据关键字</param>
        /// <param name="memberKey"></param>
        protected void onDeleted(valueType value, keyType key, memberKeyType memberKey)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Debug(typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString(), LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
            else
            {
                Dictionary<RandomKey<memberKeyType>, ListArray<valueType>> dictionary = getMember(target);
                if (dictionary != null)
                {
                    ListArray<valueType> list;
                    if (dictionary.TryGetValue(memberKey, out list))
                    {
                        int index = Array.LastIndexOf(list.Array.Array, value, list.Array.Length - 1);
                        if (index != -1)
                        {
                            if (list.Array.Length != 1)
                            {
                                if (isRemoveEnd) list.Array.TryRemoveAtToEnd(index);
                                else list.RemoveAt(index);
                            }
                            else dictionary.Remove(memberKey);
                            return;
                        }
                    }
                }
                cache.SqlTable.Log.Fatal(typeof(valueType).FullName + " 缓存同步错误", LogLevel.Fatal | LogLevel.AutoCSer);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getKey(value), getMemberKey(value));
        }
    }
}
