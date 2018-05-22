using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取分页记录集合
    /// </summary>
    internal abstract class PageAwaiter<valueType> : Awaiter<KeyValue<valueType[], int>>
    {
        /// <summary>
        /// 空分页记录集合
        /// </summary>
        internal new sealed class NullValue : PageAwaiter<valueType>
        {
            /// <summary>
            /// 空分页记录集合
            /// </summary>
            internal NullValue() : base(0, 0, false)
            {
                Value.Key = NullValue<valueType>.Array;
                continuation = Pub.EmptyAction;
                isCompleted = true;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="connection"></param>
            /// <returns></returns>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 分页长度
        /// </summary>
        protected int pageSize;
        /// <summary>
        /// 分页页号
        /// </summary>
        protected int currentPage;
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
        internal PageAwaiter(int pageSize, int currentPage, bool isDesc)
        {
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.isDesc = isDesc;
        }
    }
}
