using System;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉树分页缓存
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class DictionaryPageDescCache<keyType, valueType> : DictionaryPageCacheBase<keyType, valueType>
        where keyType : IComparable<keyType>
    {
        /// <summary>
        /// 二叉树分页缓存
        /// </summary>
        /// <param name="dictionary">二叉树字典</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="arrayPageCount">数组缓存数量</param>
        /// <param name="fifoPageCount">先进先出缓存数量</param>
        internal DictionaryPageDescCache(Dictionary<keyType, valueType> dictionary, int pageSize, int arrayPageCount, int fifoPageCount)
            : base(dictionary, pageSize, arrayPageCount, fifoPageCount)
        {
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">分页号,从 1 开始</param>
        /// <returns>分页数据</returns>
        public valueType[] Get(int page = 1)
        {
            if (page <= 0) page = 1;
            if (page <= ArrayCache.Length)
            {
                valueType[] array = ArrayCache[page - 1].Get(dictionary.Version);
                if (array == null) ArrayCache[page - 1].Set(array = dictionary.GetPageDesc(pageSize, page), dictionary.Version);
                return array;
            }
            RandomKey<int> pageKey = page;
            PageCacheVersion<valueType> cache;
            if (Queue.TryGetValue(ref pageKey, out cache))
            {
                valueType[] array = cache.Get(dictionary.Version);
                if (array != null) return array;
                cache.Set(dictionary.GetPageDesc(pageSize, page), dictionary.Version);
                Queue.Set(ref pageKey, cache);
            }
            else
            {
                cache.Set(dictionary.GetPageDesc(pageSize, page), dictionary.Version);
                Queue.UnsafeAdd(ref pageKey, cache);
                if (Queue.Count > fifoPageCount) Queue.UnsafePopNode();
            }
            return cache.Array;
        }
    }
}
