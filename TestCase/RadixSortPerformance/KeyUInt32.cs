using System;
using System.Diagnostics;
using AutoCSer.Extension;
using Int = System.UInt32;
using IntKey = AutoCSer.TestCase.RadixSortPerformance.Key.UInt32;

namespace AutoCSer.TestCase.RadixSortPerformance
{
    class KeyUInt32
    {
        /// <summary>
        /// 测试数据类型
        /// </summary>
        private static readonly string type = typeof(IntKey).fullName();
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        private static void random(IntKey[] key1, IntKey[] key2)
        {
            for (int index = key1.Length; index != 0;)
            {
                --index;
                key2[index].Key = key1[index].Key = (uint)AutoCSer.Random.Default.Next();
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        private static void sort(IntKey[] key1, IntKey[] key2)
        {
            random(key1, key2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(key2);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort " + type + "[" + key1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            IntKey[] key11 = key1.getSort(value => value.Key);
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.getSort " + type + "[" + key1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = key1.Length; index != 0;)
            {
                --index;
                if (key11[index].Key != key2[index].Key)
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        private static void sortDesc(IntKey[] key1, IntKey[] key2)
        {
            random(key1, key2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(key2);
            Array.Reverse(key2);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort+Array.Reverse " + type + "[" + key1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            IntKey[] key11 = key1.getSortDesc(value => value.Key);
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.getSortDesc " + type + "[" + key1.Length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = key1.Length; index != 0;)
            {
                --index;
                if (key11[index].Key != key2[index].Key)
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private static void sort(IntKey[] key1, IntKey[] key2, int startIndex, int length)
        {
            random(key1, key2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(key2, startIndex, length);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            IntKey[] key11 = key1.getSort(value => value.Key, startIndex, length);
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.getSort " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = startIndex, endIndex = startIndex + length; index != endIndex; ++index)
            {
                if (key11[index - startIndex].Key != key2[index].Key)
                {
                    Console.WriteLine(type + " sort Error");
                    break;
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private static void sortDesc(IntKey[] key1, IntKey[] key2, int startIndex, int length)
        {
            random(key1, key2);
            long time = System.Diagnostics.Stopwatch.GetTimestamp();
            Array.Sort(key2, startIndex, length);
            Array.Reverse(key2, startIndex, length);
            double milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("Array.Sort+Array.Reverse " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            IntKey[] key11 = key1.getSortDesc(value => value.Key, startIndex, length);
            milliseconds = Date.GetTimestampTimeSpan(time).TotalMilliseconds;
            Console.WriteLine("AutoCSer.getSortDesc " + type + "[" + length.toString() + "] " + milliseconds.ToString() + "ms");
            for (int index = startIndex, endIndex = startIndex + length; index != endIndex; ++index)
            {
                if (key11[index - startIndex].Key != key2[index].Key)
                {
                    Console.WriteLine("Error");
                }
            }
        }
        /// <summary>
        /// 排序测试
        /// </summary>
        /// <param name="count"></param>
        internal static void Test(int count)
        {
            IntKey[] key1 = new IntKey[count], key2 = new IntKey[count];
            sort(key1, key2);
            Console.WriteLine();
            sortDesc(key1, key2);
            Console.WriteLine();
            sort(key1, key2, (count >> 2) - 1, (count >> 1) - 5);
            Console.WriteLine();
            sortDesc(key1, key2, (count >> 2) - 1, (count >> 1) - 5);
            Console.WriteLine();
        }
    }
}
