using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 自增 ID 整表排序树缓存
    /// </summary>
    public partial class IdentityTree<valueType, modelType, memberCacheType>
    {
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        private sealed class PageAwaiter : Threading.PageAwaiter<valueType>
        {
            /// <summary>
            /// 自增 ID 整表排序树缓存
            /// </summary>
            private IdentityTree<valueType, modelType, memberCacheType> tree;
            /// <summary>
            /// 获取分页记录集合
            /// </summary>
            /// <param name="tree"></param>
            /// <param name="pageSize"></param>
            /// <param name="currentPage"></param>
            /// <param name="isDesc"></param>
            internal PageAwaiter(IdentityTree<valueType, modelType, memberCacheType> tree, int pageSize, int currentPage, bool isDesc)
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
                    Value.Key = isDesc ? tree.getPageDesc(pageSize, currentPage, out Value.Value) : tree.getPage(pageSize, currentPage, out Value.Value);
                }
                finally
                {
                    if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
                }
                return LinkNext;
            }
        }
        /// <summary>
        /// 获取分页记录集合 awaiter
        /// </summary>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页记录集合 + 记录总数</returns>
        public Threading.Awaiter<KeyValue<valueType[], int>> GetPageAwaiter(int pageSize, int currentPage, bool isDesc = true)
        {
            PageAwaiter task = new PageAwaiter(this, pageSize, currentPage, isDesc);
            SqlTable.AddQueue(task);
            return task;
        }
    }
}
