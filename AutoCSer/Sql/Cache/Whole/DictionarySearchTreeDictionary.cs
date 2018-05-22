using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;
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
    public partial class DictionarySearchTreeDictionary<valueType, modelType, keyType, sortType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 分组关键字获取器
        /// </summary>
        protected readonly Func<valueType, keyType> getKey;
        /// <summary>
        /// 排序关键字获取器
        /// </summary>
        protected readonly Func<valueType, sortType> getSort;
        /// <summary>
        /// 字典+搜索树缓存
        /// </summary>
        protected readonly Dictionary<RandomKey<keyType>, AutoCSer.SearchTree.Dictionary<sortType, valueType>> groups;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组关键字获取器</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        public DictionarySearchTreeDictionary
            (Event.Cache<valueType, modelType> cache, Func<valueType, keyType> getKey, Func<valueType, sortType> getSort, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getSort == null) throw new ArgumentNullException("getSort is null");
            this.cache = cache;
            this.getKey = getKey;
            this.getSort = getSort;
            groups = DictionaryCreator<RandomKey<keyType>>.Create<AutoCSer.SearchTree.Dictionary<sortType, valueType>>();

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
        /// <param name="key">数据对象的关键字</param>
        protected void onInserted(valueType value, keyType key)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            if (!groups.TryGetValue(key, out tree)) groups.Add(key, tree = new AutoCSer.SearchTree.Dictionary<sortType, valueType>());
            if (!tree.Set(getSort(value), value)) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
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
            if (key.Equals(oldKey))
            {
                sortType sortKey = getSort(value), oldSortKey = getSort(oldValue);
                if (!sortKey.Equals(oldSortKey))
                {
                    AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
                    if (!groups.TryGetValue(key, out tree) || !tree.Remove(oldSortKey) || !tree.Set(sortKey, cacheValue))
                    {
                        cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                    }
                }
            }
            else
            {
                onInserted(cacheValue, key);
                onDeleted(oldValue, oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">分组关键字</param>
        protected void onDeleted(valueType value, keyType key)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            if (groups.TryGetValue(key, out tree) && tree.Remove(getSort(value)))
            {
                if (tree.Count == 0) groups.Remove(key);
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
            onDeleted(value, getKey(value));
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int GetCount(keyType key)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            return groups.TryGetValue(key, out tree) ? tree.Count : 0;
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">数据总数</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页数据集合</returns>
        public valueType[] GetPage(keyType key, int pageSize, int currentPage, out int count, bool isDesc = true)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            if (groups.TryGetValue(key, out tree))
            {
                Threading.SearchTreeDictionaryPageTask<valueType, sortType> task = new Threading.SearchTreeDictionaryPageTask<valueType, sortType>(pageSize, currentPage, isDesc, tree);
                cache.SqlTable.AddQueue(task);
                return task.Wait(out count);
            }
            count = 0;
            return NullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetArray(keyType key)
        {
            int count;
            return GetPage(key, int.MaxValue, 1, out count, false);
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配委托</param>
        /// <returns>数据集合</returns>
        public LeftArray<valueType> GetFind(keyType key, Func<valueType, bool> isValue)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            if (groups.TryGetValue(key, out tree))
            {
                Threading.SearchTreeDictionaryFindTask<valueType, sortType> task = new Threading.SearchTreeDictionaryFindTask<valueType, sortType>(tree, isValue);
                cache.SqlTable.AddQueue(task);
                return task.Wait();
            }
            return default(LeftArray<valueType>);
        }
    }
}
