using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 查找匹配记录集合
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="sortType">排序关键字类型</typeparam>
    internal sealed class SearchTreeDictionaryFindTask<valueType, sortType> : Threading.LinkQueueTaskNode
        where valueType : class
        where sortType : IComparable<sortType>
    {
        /// <summary>
        /// 搜索树
        /// </summary>
        private AutoCSer.SearchTree.Dictionary<sortType, valueType> tree;
        /// <summary>
        /// 数据匹配委托
        /// </summary>
        private Func<valueType, bool> isValue;
        /// <summary>
        /// 等待缓存加载
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle wait;
        /// <summary>
        /// 匹配数据集合
        /// </summary>
        private LeftArray<valueType> array;
        /// <summary>
        /// 获取匹配记录集合
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="isValue"></param>
        internal SearchTreeDictionaryFindTask(AutoCSer.SearchTree.Dictionary<sortType, valueType> tree, Func<valueType, bool> isValue)
        {
            this.tree = tree;
            this.isValue = isValue;
            wait.Set(0);
        }
        /// <summary>
        /// 查找匹配记录集合
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            try
            {
                array = tree.GetFind(isValue);
            }
            finally
            {
                wait.Set();
            }
            return LinkNext;
        }
        /// <summary>
        /// 获取匹配记录集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal LeftArray<valueType> Wait()
        {
            wait.Wait();
            return array;
        }
    }
}
