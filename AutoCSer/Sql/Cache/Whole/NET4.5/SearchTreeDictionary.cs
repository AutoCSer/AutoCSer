using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 搜索树缓存
    /// </summary>
    public partial class SearchTreeDictionary<valueType, modelType, sortType>
    {
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Threading.Awaiter<KeyValue<valueType[], int>> GetArrayAwaiter()
        {
            return GetPageAwaiter(int.MaxValue, 1);
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页数据集合</returns>
        public Threading.Awaiter<KeyValue<valueType[], int>> GetPageAwaiter(int pageSize, int currentPage, bool isDesc = false)
        {
            Threading.SearchTreeDictionaryPageAwaiter<valueType, sortType> task = new Threading.SearchTreeDictionaryPageAwaiter<valueType, sortType>(pageSize, currentPage, isDesc, tree);
            cache.SqlTable.AddQueue(task);
            return task;
        }
    }
}
