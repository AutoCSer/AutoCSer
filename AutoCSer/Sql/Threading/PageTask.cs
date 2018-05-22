using System;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取分页记录集合任务
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public abstract class PageTask<valueType> : Threading.LinkQueueTaskNode
    {
        /// <summary>
        /// 分页记录集合
        /// </summary>
        protected valueType[] values;
        /// <summary>
        /// 分页长度
        /// </summary>
        protected int pageSize;
        /// <summary>
        /// 分页页号
        /// </summary>
        protected int currentPage;
        /// <summary>
        /// 记录总数
        /// </summary>
        protected int count;
        /// <summary>
        /// 是否逆序
        /// </summary>
        protected bool isDesc;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="isDesc"></param>
        internal PageTask(int pageSize, int currentPage, bool isDesc)
        {
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.isDesc = isDesc;
        }
    }
}
