using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取记录
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LazyOrderArrayAtTask<valueType> : Threading.LinkQueueTaskNode
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
        /// 索引位置
        /// </summary>
        private int index;
        /// <summary>
        /// 数据
        /// </summary>
        private valueType value;
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="index"></param>
        /// <param name="array"></param>
        /// <param name="sorter"></param>
        internal LazyOrderArrayAtTask(int index, AutoCSer.Sql.Cache.LazyOrderArray<valueType> array, Func<LeftArray<valueType>, LeftArray<valueType>> sorter)
        {
            this.array = array;
            this.sorter = sorter;
            this.index = index;
            wait.Set(0);
        }
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                value = array.At(sorter, index);
            }
            finally
            {
                wait.Set();
            }
            return LinkNext;
        }
        /// <summary>
        /// 获取数据记录
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType Wait()
        {
            wait.Wait();
            return value;
        }
    }
}
