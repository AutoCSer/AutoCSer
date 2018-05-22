using System;
using AutoCSer.Extension;
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
    public partial class SearchTreeDictionary<valueType, modelType, sortType>
        where valueType : class, modelType
        where modelType : class
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 排序关键字获取器
        /// </summary>
        protected readonly Func<valueType, sortType> getSort;
        /// <summary>
        /// 搜索树缓存
        /// </summary>
        protected readonly AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
        /// <summary>
        /// 获取缓存数量
        /// </summary>
        public int Count
        {
            get { return tree.Count; }
        }
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isReset">是否绑定事件与重置数据</param>
        public SearchTreeDictionary(Event.Cache<valueType, modelType> cache, Func<valueType, sortType> getSort, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getSort == null) throw new ArgumentNullException("getSort is null");
            this.cache = cache;
            this.getSort = getSort;

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
            if (!tree.TryAdd(getSort(value), value))
            {
                cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            onDeleted(oldValue);
            onInserted(cacheValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            if (!tree.Remove(getSort(value)))
            {
                cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetArray()
        {
            int count;
            return GetPage(int.MaxValue, 1, out count);
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">数据总数</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页数据集合</returns>
        public valueType[] GetPage(int pageSize, int currentPage, out int count, bool isDesc = false)
        {
            Threading.SearchTreeDictionaryPageTask<valueType, sortType> task = new Threading.SearchTreeDictionaryPageTask<valueType, sortType>(pageSize, currentPage, isDesc, tree);
            cache.SqlTable.AddQueue(task);
            return task.Wait(out count);
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountLess(sortType key)
        {
            return tree.CountLess(ref key);
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountThan(sortType key)
        {
            return tree.CountThan(ref key);
        }
    }
}
