using System;
/*Type:ulong;long;uint;int;DateTime*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组子串扩展操作
    /// </summary>
    public static unsafe partial class SubArray
    {
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> distinct(this SubArray</*Type[0]*/ulong/*Type[0]*/> array)
        {
            if (array.Length <= 1) return array;
            return new SubArray</*Type[0]*/ulong/*Type[0]*/> { Array = array.Array, Start = array.Start, Length = FixedArraySortGroup.Distinct(array.Array, array.Start, array.Length) - array.Start };
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] distinct<valueType>(this SubArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, valueType> getValue)
        {
            return array.Length == 0 ? NullValue<valueType>.Array : FixedArraySortGroup.Distinct(array.Array, getValue, array.Start, array.Length);
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> distinct<valueType>(this SubArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = array.getArray(getValue);
            newValues.sort(0, newValues.Length);
            return newValues.distinct();
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <returns>分组数量</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue</*Type[0]*/ulong/*Type[0]*/, int>[] SortGroupCount(this SubArray</*Type[0]*/ulong/*Type[0]*/> array)
        {
            return array.Length == 0 ? NullValue<KeyValue</*Type[0]*/ulong/*Type[0]*/, int>>.Array : FixedArraySortGroup.SortGroupCount(array.Array, array.Start, array.Length);
        }
        /// <summary>
        /// 数据排序分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<SubArray<valueType>> SortGroup<valueType>(this SubArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            return array.Length == 0 ? default(LeftArray<SubArray<valueType>>) : FixedArraySortGroup.SortGroup(array.Array, getValue, array.Start, array.Length);
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合数量</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int SortGroupCount<valueType>(this SubArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            return array.Length == 0 ? 0 : FixedArraySortGroup.SortGroupCount(array.Array, getValue, array.Start, array.Length);
        }
    }
}