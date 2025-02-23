﻿using System;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static partial class QuickSort
    {
        /// <summary>
        /// 排序器
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct Sorter<valueType>
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
                    int leftIndex = startIndex, rightIndex = endIndex;
                    valueType value = Array[average += startIndex];
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
                        if (startIndex < rightIndex) Sort(startIndex, rightIndex);
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex) Sort(leftIndex, endIndex);
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Sort<valueType>(valueType[] values, Func<valueType, valueType, int> comparer)
        {
            if (values != null && values.Length > 1)
            {
                new Sorter<valueType> { Array = values, Comparer = comparer }.Sort(0, values.Length - 1);
            }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Sort<valueType>(valueType[] values, Func<valueType, valueType, int> comparer, int startIndex, int count)
        {
            FormatRange range = new FormatRange(values.length(), startIndex, count);
            if (range.GetCount > 1)
            {
                new Sorter<valueType> { Array = values, Comparer = comparer }.Sort(range.SkipCount, range.EndIndex - 1);
            }
        }
    }
}
