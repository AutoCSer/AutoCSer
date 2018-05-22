using System;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 数组+搜索树缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    public partial class ArraySearchTreeDictionary<valueType, modelType, sortType>
        where valueType : class, modelType
        where modelType : class
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 数组索引获取器
        /// </summary>
        protected readonly Func<valueType, int> getIndex;
        /// <summary>
        /// 排序关键字获取器
        /// </summary>
        protected readonly Func<valueType, sortType> getSort;
        /// <summary>
        /// 数组+搜索树缓存
        /// </summary>
        protected readonly AutoCSer.SearchTree.Dictionary<sortType, valueType>[] treeArray;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getIndex">数组索引获取器</param>
        /// <param name="arraySize">数组大小</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        public ArraySearchTreeDictionary(Event.Cache<valueType, modelType> cache, Func<valueType, int> getIndex, int arraySize, Func<valueType, sortType> getSort, bool isReset = true)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getIndex == null) throw new ArgumentNullException("getIndex is null");
            if (getSort == null) throw new ArgumentNullException("getSort is null");
            treeArray = new AutoCSer.SearchTree.Dictionary<sortType, valueType>[arraySize];
            this.cache = cache;
            this.getIndex = getIndex;
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
            onInserted(value, getIndex(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="index"></param>
        protected void onInserted(valueType value, int index)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            if (tree == null) treeArray[index] = tree = new AutoCSer.SearchTree.Dictionary<sortType, valueType>();
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
            int index = getIndex(value), oldIndex = getIndex(oldValue);
            if (index == oldIndex)
            {
                sortType sortKey = getSort(value), oldSortKey = getSort(oldValue);
                if (!sortKey.Equals(oldSortKey))
                {
                    AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
                    if (tree != null)
                    {
                        bool isRemove = tree.Remove(oldSortKey), isAdd = tree.TryAdd(sortKey, cacheValue);
                        if (isRemove && isAdd) return;
                    }
                    cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                }
            }
            else
            {
                onInserted(cacheValue, index);
                onDeleted(oldValue, oldIndex);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="index"></param>
        protected void onDeleted(valueType value, int index)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            if (tree != null && tree.Remove(getSort(value))) return;
            cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getIndex(value));
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int GetCount(int index)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            return tree != null ? tree.Count : 0;
        }
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">记录总数</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页记录集合</returns>
        public valueType[] GetPage(int index, int pageSize, int currentPage, out int count, bool isDesc = true)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            if (tree != null)
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
        /// <param name="index">数组索引</param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetArray(int index)
        {
            int count;
            return GetPage(index, int.MaxValue, 1, out count, false);
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountLess(int index, sortType key)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            return tree != null ? tree.CountLess(ref key) : 0;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountThan(int index, sortType key)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            return tree != null ? tree.CountThan(ref key) : 0;
        }
    }
}
