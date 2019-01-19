using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 单向动态数组扩展操作
    /// </summary>
    public static class ListArray
    {
        /// <summary>
        /// 转换数组子串
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="array">单向动态数组</param>
        /// <returns>数组子串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static LeftArray<valueType> ToLeftArray<valueType>(this ListArray<valueType> array)
        {
            return new LeftArray<valueType>(array);
        }
    }
}
