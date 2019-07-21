using System;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围快速排序
    /// </summary>
    internal static class RangeQuickSort
    {
        /// <summary>
        /// 范围排序器(一般用于获取分页)
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        internal struct Sorter<valueType>
        {
            /// <summary>
            /// 待排序数组
            /// </summary>
            public valueType[] Array;
            /// <summary>
            /// 排序比较器
            /// </summary>
            public Func<valueType, valueType, int> Comparer;
            /// <summary>
            /// 跳过数据数量
            /// </summary>
            public int SkipCount;
            /// <summary>
            /// 最后一条记录位置-1
            /// </summary>
            public int GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置-1</param>
            public void Sort(int startIndex, int endIndex)
            {
                do
                {
                    valueType leftValue = Array[startIndex], rightValue = Array[endIndex];
                    int average = (endIndex - startIndex) >> 1;
                    if (average == 0)
                    {
                        if (Comparer(leftValue, rightValue) > 0)
                        {
                            Array[startIndex] = rightValue;
                            Array[endIndex] = leftValue;
                        }
                        break;
                    }
                    average += startIndex;
                    //if (average > getEndIndex) average = getEndIndex;
                    //else if (average < skipCount) average = skipCount;
                    int leftIndex = startIndex, rightIndex = endIndex;
                    valueType value = Array[average];
                    if (Comparer(leftValue, value) <= 0)
                    {
                        if (Comparer(value, rightValue) > 0)
                        {
                            Array[rightIndex] = value;
                            if (Comparer(leftValue, rightValue) <= 0) Array[average] = value = rightValue;
                            else
                            {
                                Array[leftIndex] = rightValue;
                                Array[average] = value = leftValue;
                            }
                        }
                    }
                    else if (Comparer(leftValue, rightValue) <= 0)
                    {
                        Array[leftIndex] = value;
                        Array[average] = value = leftValue;
                    }
                    else
                    {
                        Array[rightIndex] = leftValue;
                        if (Comparer(value, rightValue) <= 0)
                        {
                            Array[leftIndex] = value;
                            Array[average] = value = rightValue;
                        }
                        else Array[leftIndex] = rightValue;
                    }
                    ++leftIndex;
                    --rightIndex;
                    do
                    {
                        while (Comparer(Array[leftIndex], value) < 0) ++leftIndex;
                        while (Comparer(value, Array[rightIndex]) < 0) --rightIndex;
                        if (leftIndex < rightIndex)
                        {
                            leftValue = Array[leftIndex];
                            Array[leftIndex] = Array[rightIndex];
                            Array[rightIndex] = leftValue;
                        }
                        else
                        {
                            if (leftIndex == rightIndex)
                            {
                                ++leftIndex;
                                --rightIndex;
                            }
                            break;
                        }
                    }
                    while (++leftIndex <= --rightIndex);
                    if (rightIndex - startIndex <= endIndex - leftIndex)
                    {
                        if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                        if (leftIndex > GetEndIndex) break;
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                        if (rightIndex < SkipCount) break;
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
    }
}
