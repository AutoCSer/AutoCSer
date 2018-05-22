using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取分页记录集合
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LazyOrderArrayPageTask<valueType> : Threading.PageTask<valueType>
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
        /// 等待缓存加载
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle wait;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="isDesc"></param>
        /// <param name="array"></param>
        /// <param name="sorter"></param>
        internal LazyOrderArrayPageTask(int pageSize, int currentPage, bool isDesc, AutoCSer.Sql.Cache.LazyOrderArray<valueType> array, Func<LeftArray<valueType>, LeftArray<valueType>> sorter)
                    : base(pageSize, currentPage, isDesc)
        {
            this.array = array;
            this.sorter = sorter;
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
                values = isDesc ? array.GetPageDesc(sorter, pageSize, currentPage, out count) : array.GetPage(sorter, pageSize, currentPage, out count);
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
