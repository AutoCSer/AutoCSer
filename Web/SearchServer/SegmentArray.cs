using System;
using System.Runtime.InteropServices;
using AutoCSer.Extension;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 分段数组
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    [StructLayout(LayoutKind.Auto)]
    internal struct SegmentArray<valueType>
    {
        /// <summary>
        /// 数组集合
        /// </summary>
        private valueType[][] arrays;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal valueType this[int index]
        {
            get
            {
                return arrays[index >> maxBits][index & arrayAnd];
            }
            set
            {
                arrays[index >> maxBits][index & arrayAnd] = value;
            }
        }
        /// <summary>
        /// 数组集合位置
        /// </summary>
        private int arrayIndex;
        /// <summary>
        /// 当前数组
        /// </summary>
        private valueType[] array;
        /// <summary>
        /// 当前数组分配位置
        /// </summary>
        private int currentIndex;
        /// <summary>
        /// 当前数组起始位置
        /// </summary>
        private int baseIndex;
        /// <summary>
        /// 数组最大长度
        /// </summary>
        private int maxArraySize;
        /// <summary>
        /// 数组最大二进制位长度
        /// </summary>
        private int maxBits;
        /// <summary>
        /// 数组索引最大值
        /// </summary>
        private int arrayAnd;
        /// <summary>
        /// 分段数组
        /// </summary>
        /// <param name="minBits">数组初始二进制位长度</param>
        /// <param name="maxBits">数组最大二进制位长度</param>
        internal SegmentArray(int minBits, int maxBits = 16)
        {
            arrays = new valueType[][] { array = new valueType[1 << minBits] };
            currentIndex = arrayIndex = 1;
            baseIndex = 0;
            maxArraySize = 1 << (this.maxBits = maxBits);
            arrayAnd = maxArraySize - 1;
        }
        /// <summary>
        /// 获取数组索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int GetIndex(valueType value)
        {
            if (currentIndex == array.Length)
            {
                if (currentIndex == maxArraySize)
                {
                    if (arrayIndex == arrays.Length) arrays = arrays.copyNew(arrayIndex << 1);
                    arrays[arrayIndex++] = array = new valueType[maxArraySize];
                    baseIndex += maxArraySize;
                }
                else array = array.copyNew(currentIndex << 1);
            }
            array[currentIndex] = value;
            return baseIndex + currentIndex++;
        }
    }
}
