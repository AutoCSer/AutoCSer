using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取单向动态数组匹配记录数量
    /// </summary>
    public sealed class ListArrayCountAwaiter<valueType> : CountAwaiter
    {
        /// <summary>
        /// 单向动态数组
        /// </summary>
        private ListArray<valueType> list;
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private Func<valueType, bool> isValue;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isValue"></param>
        internal ListArrayCountAwaiter(ListArray<valueType> list, Func<valueType, bool> isValue)
        {
            this.list = list;
            this.isValue = isValue;
        }
        /// <summary>
        /// 重置缓存
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                count = new SubArray<valueType>(list).GetCount(isValue);
            }
            finally
            {
                if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
            }
            return LinkNext;
        }
    }
}
