using System;
/*ulong;long;uint;int;DateTime*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展操作
    /// </summary>
    public static unsafe partial class SubArrayExtension
    {
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> distinct(this SubArray<ulong> array)
        {
            if (array.Length <= 1) return array;
            return new SubArray<ulong> { Array = array.Array, Start = array.Start, Length = FixedArraySortGroup.Distinct(array.Array, array.Start, array.Length) - array.Start };
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] distinct<valueType>(this SubArray<ulong> array, Func<ulong, valueType> getValue)
        {
            return array.Length == 0 ? EmptyArray<valueType>.Array : FixedArraySortGroup.Distinct(array.Array, getValue, array.Start, array.Length);
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> distinct<valueType>(this SubArray<valueType> array, Func<valueType, ulong> getValue)
        {
            ulong[] newValues = array.getArray(getValue);
            newValues.sort(0, newValues.Length);
            return newValues.distinct();
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <returns>分组数量</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue<ulong, int>[] SortGroupCount(this SubArray<ulong> array)
        {
            return array.Length == 0 ? EmptyArray<KeyValue<ulong, int>>.Array : FixedArraySortGroup.SortGroupCount(array.Array, array.Start, array.Length);
        }
        /// <summary>
        /// 数据排序分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<SubArray<valueType>> SortGroup<valueType>(this SubArray<valueType> array, Func<valueType, ulong> getValue)
        {
            return array.Length == 0 ? new LeftArray<SubArray<valueType>>(0) : FixedArraySortGroup.SortGroup(array.Array, getValue, array.Start, array.Length);
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合数量</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int SortGroupCount<valueType>(this SubArray<valueType> array, Func<valueType, ulong> getValue)
        {
            return array.Length == 0 ? 0 : FixedArraySortGroup.SortGroupCount(array.Array, getValue, array.Start, array.Length);
        }
    }
}