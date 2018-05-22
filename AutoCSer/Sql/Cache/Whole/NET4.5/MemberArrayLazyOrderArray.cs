using System;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组列表 延时排序缓存
    /// </summary>
    public partial class MemberArrayLazyOrderArray<valueType, modelType, keyType, targetType>
    {
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="index">数组索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页数据集合</returns>
        public Threading.Awaiter<KeyValue<valueType[], int>> GetPageAwaiter(keyType key, int index, int pageSize, int currentPage, bool isDesc = false)
        {
            LazyOrderArray<valueType> array = getCache(key, index);
            if (array != null)
            {
                Threading.LazyOrderArrayPageAwaiter<valueType> task = new Threading.LazyOrderArrayPageAwaiter<valueType>(pageSize, currentPage, isDesc, array, sorter);
                cache.SqlTable.AddQueue(task);
                return task;
            }
            return new Threading.PageAwaiter<valueType>.NullValue();
        }
    }
}
