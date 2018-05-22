using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 数组+搜索树缓存
    /// </summary>
    public partial class ArraySearchTreeDictionary<valueType, modelType, sortType>
    {
        /// <summary>
        /// 获取分页记录集合 awaiter
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页记录集合 + 记录总数</returns>
        public Threading.Awaiter<KeyValue<valueType[], int>> GetPageAwaiter(int index, int pageSize, int currentPage, bool isDesc = true)
        {
            AutoCSer.SearchTree.Dictionary<sortType, valueType> tree = treeArray[index];
            if (tree != null)
            {
                Threading.SearchTreeDictionaryPageAwaiter<valueType, sortType> task = new Threading.SearchTreeDictionaryPageAwaiter<valueType, sortType>(pageSize, currentPage, isDesc, tree);
                cache.SqlTable.AddQueue(task);
                return task;
            }
            return new Threading.PageAwaiter<valueType>.NullValue();
        }
        /// <summary>
        /// 获取数据集合 awaiter
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <returns>记录集合 + 记录总数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Threading.Awaiter<KeyValue<valueType[], int>> GetArrayAwaiter(int index)
        {
            return GetPageAwaiter(index, int.MaxValue, 1, false);
        }
    }
}
