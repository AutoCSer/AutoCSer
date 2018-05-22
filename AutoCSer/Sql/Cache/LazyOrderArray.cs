using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// 延时排序缓存数组
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public sealed class LazyOrderArray<valueType>
        where valueType : class
    {
        /// <summary>
        /// 数据数组
        /// </summary>
        internal LeftArray<valueType> Array;
        /// <summary>
        /// 数据是否已经排序
        /// </summary>
        private bool isSorted = true;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Insert(valueType value)
        {
            Array.Add(value);
            isSorted = false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Update(valueType value)
        {
            isSorted = false;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        internal void Delete(valueType value)
        {
            if (Array.RemoveToEnd(value)) isSorted = false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sorter"></param>
        /// <param name="index">分页大小</param>
        /// <returns>数据对象</returns>
        internal valueType At(Func<LeftArray<valueType>, LeftArray<valueType>> sorter, int index)
        {
            if (!isSorted) Array = sorter(Array);
            return (uint)index < Array.Length ? Array.Array[index] : null;
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="sorter"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">数据总数</param>
        /// <returns>分页数据集合</returns>
        internal valueType[] GetPage(Func<LeftArray<valueType>, LeftArray<valueType>> sorter, int pageSize, int currentPage, out int count)
        {
            if (!isSorted) Array = sorter(Array);
            return Array.GetPage(pageSize, currentPage, out count);
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="sorter"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">数据总数</param>
        /// <returns>分页数据集合</returns>
        internal valueType[] GetPageDesc(Func<LeftArray<valueType>, LeftArray<valueType>> sorter, int pageSize, int currentPage, out int count)
        {
            if (!isSorted) Array = sorter(Array);
            return Array.GetPageDesc(pageSize, currentPage, out count);
        }
    }
}
