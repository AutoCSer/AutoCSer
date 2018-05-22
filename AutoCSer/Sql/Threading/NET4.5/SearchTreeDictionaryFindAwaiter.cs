using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 查找匹配记录集合
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    internal sealed class SearchTreeDictionaryFindAwaiter<valueType, sortType> : FindAwaiter<valueType>
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
        /// <param name="isValue"></param>
        /// <param name="tree"></param>
        internal SearchTreeDictionaryFindAwaiter(Func<valueType, bool> isValue, AutoCSer.SearchTree.Dictionary<sortType, valueType> tree) : base(isValue)
        {
            this.tree = tree;
        }
        /// <summary>
        /// 查找匹配记录集合
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                Value = tree.GetFind(isValue);
            }
            finally
            {
                if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
            }
            return LinkNext;
        }
    }
}
