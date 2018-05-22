using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取分页记录集合
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    internal sealed class SearchTreeDictionaryPageTask<valueType, sortType> : PageTask<valueType>
        where valueType : class
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 搜索树
        /// </summary>
        private AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
        /// <summary>
        /// 等待缓存加载
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle wait;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="isDesc"></param>
        /// <param name="tree"></param>
        internal SearchTreeDictionaryPageTask(int pageSize, int currentPage, bool isDesc, AutoCSer.SearchTree.Dictionary<sortType, valueType> tree)
                : base(pageSize, currentPage, isDesc)
        {
            this.tree = tree;
            wait.Set(0);
        }
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                count = tree.Count;
                values = isDesc ? tree.GetPageDesc(pageSize, currentPage) : tree.GetPage(pageSize, currentPage);
            }
            finally
            {
                wait.Set();
            }
            return LinkNext;
        }
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType[] Wait(out int count)
        {
            wait.Wait();
            count = this.count;
            return values;
        }
    }
}
