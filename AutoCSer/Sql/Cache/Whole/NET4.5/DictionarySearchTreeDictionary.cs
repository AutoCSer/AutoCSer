using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 字典+搜索树缓存
    /// </summary>
    public partial class DictionarySearchTreeDictionary<valueType, modelType, keyType, sortType>
    {
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页数据集合</returns>
        public Threading.Awaiter<KeyValue<valueType[], int>> GetPageAwaiter(keyType key, int pageSize, int currentPage, bool isDesc = true)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            if (groups.TryGetValue(key, out tree))
            {
                Threading.SearchTreeDictionaryPageAwaiter<valueType, sortType> task = new Threading.SearchTreeDictionaryPageAwaiter<valueType, sortType>(pageSize, currentPage, isDesc, tree);
                cache.SqlTable.AddQueue(task);
                return task;
            }
            return new Threading.PageAwaiter<valueType>.NullValue();
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Threading.Awaiter<KeyValue<valueType[], int>> GetArrayAwaiter(keyType key)
        {
            return GetPageAwaiter(key, int.MaxValue, 1, false);
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配委托</param>
        /// <returns>数据集合</returns>
        public Threading.Awaiter<LeftArray<valueType>> GetFindAwaiter(keyType key, Func<valueType, bool> isValue)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
            if (groups.TryGetValue(key, out tree))
            {
                Threading.SearchTreeDictionaryFindAwaiter<valueType, sortType> task = new Threading.SearchTreeDictionaryFindAwaiter<valueType, sortType>(isValue, tree);
                cache.SqlTable.AddQueue(task);
                return task;
            }
            return new Threading.FindAwaiter<valueType>.NullValue();
        }
    }
}
