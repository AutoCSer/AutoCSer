using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组列表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    public partial class DictionaryList<valueType, modelType, keyType>
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
        /// 分组数据
        /// </summary>
        protected readonly Dictionary<RandomKey<keyType>, ListArray<valueType>> groups;
        /// <summary>
        /// 移除数据并使用最后一个数据移动到当前位置
        /// </summary>
        protected readonly bool isRemoveEnd;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public DictionaryList(Event.Cache<valueType, modelType> cache, Func<valueType, keyType> getKey, bool isRemoveEnd = false, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            this.cache = cache;
            this.getKey = getKey;
            this.isRemoveEnd = isRemoveEnd;
            groups = DictionaryCreator<RandomKey<keyType>>.Create<ListArray<valueType>>();

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
            ListArray<valueType> list;
            keyType key = getKey(value);
            if (!groups.TryGetValue(key, out list)) groups.Add(key, list = new ListArray<valueType>());
            list.Add(value);
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
            keyType oldKey = getKey(oldValue);
            if (!getKey(value).Equals(oldKey))
            {
                onInserted(cacheValue);
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
            ListArray<valueType> list;
            if (groups.TryGetValue(key, out list))
            {
                int index = Array.LastIndexOf(list.Array, value, list.Length - 1);
                if (index != -1)
                {
                    if (list.Length != 1)
                    {
                        if (isRemoveEnd) list.RemoveAtEnd(index);
                        else list.RemoveAt(index);
                    }
                    else groups.Remove(key);
                    return;
                }
            }
            cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
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
        /// <summary>
        /// 获取匹配数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>匹配数据数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int Count(keyType key)
        {
            ListArray<valueType> list;
            return groups.TryGetValue(key, out list) ? list.Length : 0;
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public int Count(keyType key, Func<valueType, bool> isValue)
        {
            ListArray<valueType> list;
            if (groups.TryGetValue(key, out list))
            {
                if (isValue == null) throw new ArgumentNullException();
                return new LeftArray<valueType>(list).GetCount(isValue);
                //Threading.ListArrayCountTask<valueType> task = new Threading.ListArrayCountTask<valueType>(list, isValue);
                //cache.SqlTable.AddQueue(task);
                //return task.Wait();
            }
            return 0;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetArray(keyType key)
        {
            ListArray<valueType> list;
            return groups.TryGetValue(key, out list) ? new SubArray<valueType>(list).GetArray() : NullValue<valueType>.Array;
        }
    }
}
