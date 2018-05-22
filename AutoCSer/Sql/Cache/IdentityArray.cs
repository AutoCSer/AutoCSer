using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// 缓存专用分段二维数组参数
    /// </summary>
    internal static class IdentityArray
    {
        /// <summary>
        /// 数组长度的有效二进制位数
        /// </summary>
        internal const int ArrayShift = 15;
        /// <summary>
        /// 数组长度
        /// </summary>
        internal const int ArraySize = 1 << ArrayShift;
        /// <summary>
        /// 数组索引计算 And 值
        /// </summary>
        internal const int ArraySizeAnd = ArraySize - 1;
    }
    /// <summary>
    /// 缓存专用分段二维数组
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct IdentityArray<valueType> where valueType : class
    {
        /// <summary>
        /// 二维数组
        /// </summary>
        internal valueType[][] Arrays;
        /// <summary>
        /// 有效数组数量
        /// </summary>
        private int arrayCount;
        /// <summary>
        /// 当前数组容量
        /// </summary>
        internal int Length;
        /// <summary>
        /// 获取或者设置数据(不检测位置有效性)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal valueType this[int index]
        {
            get { return Arrays[index >> IdentityArray.ArrayShift][index & IdentityArray.ArraySizeAnd]; }
            set { Arrays[index >> IdentityArray.ArrayShift][index & IdentityArray.ArraySizeAnd] = value; }
        }
        /// <summary>
        /// 数据枚举
        /// </summary>
        internal IEnumerable<valueType> Values
        {
            get
            {
                int count = arrayCount;
                foreach (valueType[] array in Arrays)
                {
                    foreach (valueType value in array)
                    {
                        if (value != null) yield return value;
                    }
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 缓存专用分段二维数组
        /// </summary>
        /// <param name="size"></param>
        internal IdentityArray(int size)
        {
            int newArrayCount = (size + IdentityArray.ArraySizeAnd) >> IdentityArray.ArrayShift;
            Arrays = new valueType[Math.Max(newArrayCount, 4)][];
            for (arrayCount = 0; arrayCount < newArrayCount; Arrays[arrayCount++] = new valueType[IdentityArray.ArraySize]) ;
            Length = arrayCount << IdentityArray.ArrayShift;
        }
        /// <summary>
        /// 增加数组扩展容量到指定数量
        /// </summary>
        /// <param name="size"></param>
        internal void ToSize(int size)
        {
            int newArrayCount = (size + IdentityArray.ArraySizeAnd) >> IdentityArray.ArrayShift;
            if (Arrays == null) Arrays = new valueType[Math.Max(newArrayCount, 4)][];
            else if (Arrays.Length < newArrayCount)
            {
                valueType[][] newArrays = new valueType[Math.Max(Arrays.Length << 1, newArrayCount)][];
                Array.Copy(Arrays, 0, newArrays, 0, arrayCount);
                Arrays = newArrays;
            }
            while (arrayCount < newArrayCount) Arrays[arrayCount++] = new valueType[IdentityArray.ArraySize];
            Length = arrayCount << IdentityArray.ArrayShift;
        }
        /// <summary>
        /// 获取并清除数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType GetRemove(int index)
        {
            valueType[] array = Arrays[index >> IdentityArray.ArrayShift];
            valueType value = array[index &= IdentityArray.ArraySizeAnd];
            array[index] = null;
            return value;
        }
    }
}
