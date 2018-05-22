using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取分页记录集合
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LazyOrderArrayPageAwaiter<valueType> : Threading.PageAwaiter<valueType>
        where valueType : class
    {
        /// <summary>
        /// 延时排序缓存数组
        /// </summary>
        private AutoCSer.Sql.Cache.LazyOrderArray<valueType> array;
        /// <summary>
        /// 排序器
        /// </summary>
        private Func<LeftArray<valueType>, LeftArray<valueType>> sorter;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="isDesc"></param>
        /// <param name="array"></param>
        /// <param name="sorter"></param>
        internal LazyOrderArrayPageAwaiter(int pageSize, int currentPage, bool isDesc, AutoCSer.Sql.Cache.LazyOrderArray<valueType> array, Func<LeftArray<valueType>, LeftArray<valueType>> sorter)
                    : base(pageSize, currentPage, isDesc)
        {
            this.array = array;
            this.sorter = sorter;
        }
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                Value.Key = isDesc ? array.GetPageDesc(sorter, pageSize, currentPage, out Value.Value) : array.GetPage(sorter, pageSize, currentPage, out Value.Value);
            }
            finally
            {
                if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
            }
            return LinkNext;
        }
    }
}
