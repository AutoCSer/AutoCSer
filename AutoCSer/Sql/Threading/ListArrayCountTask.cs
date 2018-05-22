using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 获取单向动态数组匹配记录数量
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class ListArrayCountTask<valueType> : Threading.LinkQueueTaskNode
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
        /// 等待缓存加载
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle wait;
        /// <summary>
        /// 匹配记录数量
        /// </summary>
        private int count;
        /// <summary>
        /// 获取分页记录集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isValue"></param>
        internal ListArrayCountTask(ListArray<valueType> list, Func<valueType, bool> isValue)
        {
            this.list = list;
            this.isValue = isValue;
            wait.Set(0);
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
                wait.Set();
            }
            return LinkNext;
        }
        /// <summary>
        /// 匹配记录数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Wait()
        {
            wait.Wait();
            return count;
        }
    }
}
