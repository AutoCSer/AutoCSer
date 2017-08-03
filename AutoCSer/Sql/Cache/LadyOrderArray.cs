using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// 延时排序缓存数组
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public sealed class LadyOrderArray<valueType>
        where valueType : class
    {
        /// <summary>
        /// 数据数组
        /// </summary>
        private LeftArray<valueType> array;
        /// <summary>
        /// 是否存在被删除数据
        /// </summary>
        private bool isDelete;
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
            array.Add(value);
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
            int index = System.Array.IndexOf(array.Array, value, 0, array.Length);
            if (index != -1)
            {
                array.Array[index] = null;
                isDelete = true;
            }
        }
    }
}
