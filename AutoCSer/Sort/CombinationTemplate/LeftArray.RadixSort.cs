using System;
/*Type:ulong;long;uint;int;DateTime*/
/*Compare:;Desc*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组子串扩展操作
    /// </summary>
    public static partial class LeftArray
    {
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <returns>排序后的数组子串</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> Sort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array)
        {
            if (array.Length > 1) FixedArraySort.Sort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] GetSort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array)
        {
            return array.Length > 1 ? FixedArraySort.GetSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length) : array.GetArray();
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组子串</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetSort/*Compare[0]*//*Compare[0]*/<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey)
        {
            return array.Length > 1 ? FixedArraySort.GetSort/*Compare[0]*//*Compare[0]*/(array.Array, getKey, 0, array.Length) : array.GetArray();
        }
    }
}
