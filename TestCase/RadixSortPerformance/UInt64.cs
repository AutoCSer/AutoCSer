﻿using System;
using System.Diagnostics;
using AutoCSer.Extensions;
using Int = System.UInt64;

namespace AutoCSer.TestCase.RadixSortPerformance
{
    class UInt64
    {
        /// <summary>
        /// 测试数据类型
        /// </summary>
        private static readonly string type = typeof(Int).fullName();
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        private static void random(Int[] number1, Int[] number2)
        {
            for (int index = number1.Length; index != 0;)
            {
                --index;
                number2[index] = number1[index] = AutoCSer.Random.Default.NextULong();
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        private static void sort(Int[] number1, Int[] number2)
        {
            random(number1, number2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(number2);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort " + type + "[" + number1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            number1.sort();
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.sort " + type + "[" + number1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = number1.Length; index != 0;)
            {
                --index;
                if (number1[index] != number2[index])
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        private static void sortDesc(Int[] number1, Int[] number2)
        {
            random(number1, number2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(number2);
            Array.Reverse(number2);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort+Array.Reverse " + type + "[" + number1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            number1.sortDesc();
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.sortDesc " + type + "[" + number1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = number1.Length; index != 0;)
            {
                --index;
                if (number1[index] != number2[index])
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private static void sort(Int[] number1, Int[] number2, int startIndex, int length)
        {
            random(number1, number2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(number2, startIndex, length);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            number1.sort(startIndex, length);
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.sort " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = startIndex, endIndex = startIndex + length; index != endIndex; ++index)
            {
                if (number1[index] != number2[index])
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private static void sortDesc(Int[] number1, Int[] number2, int startIndex, int length)
        {
            random(number1, number2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(number2, startIndex, length);
            Array.Reverse(number2, startIndex, length);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort+Array.Reverse " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            number1.sortDesc(startIndex, length);
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.sortDesc " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = startIndex, endIndex = startIndex + length; index != endIndex; ++index)
            {
                if (number1[index] != number2[index])
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="count"></param>
        internal static void Test(int count)
        {
            Int[] number1 = new Int[count], number2 = new Int[count];
            sort(number1, number2);
            Console.WriteLine();
            sortDesc(number1, number2);
            Console.WriteLine();
            sort(number1, number2, (count >> 2) - 1, (count >> 1) - 5);
            Console.WriteLine();
            sortDesc(number1, number2, (count >> 2) - 1, (count >> 1) - 5);
            Console.WriteLine();
        }
    }
}
