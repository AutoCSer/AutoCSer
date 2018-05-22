using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取分页记录集合
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    internal sealed class SearchTreeDictionaryPageAwaiter<valueType, sortType> : Threading.PageAwaiter<valueType>
        where valueType : class
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 搜索树
        /// </summary>
        private AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="isDesc"></param>
        /// <param name="tree"></param>
        internal SearchTreeDictionaryPageAwaiter(int pageSize, int currentPage, bool isDesc, AutoCSer.SearchTree.Dictionary<sortType, valueType> tree)
            : base(pageSize, currentPage, isDesc)
        {
            this.tree = tree;
        }
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                Value.Key = isDesc ? tree.GetPageDesc(pageSize, currentPage) : tree.GetPage(pageSize, currentPage);
                Value.Value = tree.Count;
            }
            finally
            {
                if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
            }
            return LinkNext;
        }
    }
}
