using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取记录
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LazyOrderArrayAtAwaiter<valueType> : Awaiter<valueType>
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
        /// 索引位置
        /// </summary>
        private int index;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="index"></param>
        /// <param name="array"></param>
        /// <param name="sorter"></param>
        internal LazyOrderArrayAtAwaiter(int index, AutoCSer.Sql.Cache.LazyOrderArray<valueType> array, Func<LeftArray<valueType>, LeftArray<valueType>> sorter)
        {
            this.array = array;
            this.sorter = sorter;
            this.index = index;
        }
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                Value = array.At(sorter, index);
            }
            finally
            {
                if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
            }
            return LinkNext;
        }
    }
}
