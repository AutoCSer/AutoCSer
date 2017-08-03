using System;
using System.Diagnostics;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SearchTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/
");
            for (int index = keyCount; index != 0; keys[index] = index) --index;
            do
            {
                AutoCSer.Threading.ThreadPool.TinyBackground.Start(createDictionary);
                wait.WaitOne();
                if (isError) break;
                long milliseconds = time.ElapsedMilliseconds;
                Console.WriteLine("Dictionary Height[" + dictionary.Height.toString() + "]  Add[" + (keyCount + removeCount).toString() + "] + Remove[" + removeCount.toString() + "] / " + milliseconds.toString() + "ms = " + ((keyCount + removeCount + removeCount) / milliseconds).toString() + " /ms");

                AutoCSer.Threading.ThreadPool.TinyBackground.Start(searchDictionary);
                wait.WaitOne();
                if (isError) break;
                milliseconds = time.ElapsedMilliseconds;
                Console.WriteLine("Dictionary Get " + count.toString() + " / " + milliseconds.toString() + "ms = " + (count / milliseconds).toString() + " /ms");
                Console.WriteLine(@"Sleep 3000ms
");
                System.Threading.Thread.Sleep(3000);


                AutoCSer.Threading.ThreadPool.TinyBackground.Start(createSet);
                wait.WaitOne();
                if (isError) break;
                milliseconds = time.ElapsedMilliseconds;
                Console.WriteLine("Set Height[" + set.Height.toString() + "]  Add[" + (keyCount + removeCount).toString() + "] + Remove[" + removeCount.toString() + "] / " + milliseconds.toString() + "ms = " + ((keyCount + removeCount + removeCount) / milliseconds).toString() + " /ms");

                AutoCSer.Threading.ThreadPool.TinyBackground.Start(searchSet);
                wait.WaitOne();
                if (isError) break;
                milliseconds = time.ElapsedMilliseconds;
                Console.WriteLine("Set Get " + count.toString() + " / " + milliseconds.toString() + "ms = " + (count / milliseconds).toString() + " /ms");
                Console.WriteLine(@"Sleep 3000ms
");
                System.Threading.Thread.Sleep(3000);

            }
            while (true);
            Console.ReadKey();
        }

        /// <summary>
        /// 循环次数
        /// </summary>
        private const int count = 2 << 20;
        /// <summary>
        /// 关键字数量
        /// </summary>
        private const int keyCount = 1 << 20;
        /// <summary>
        /// 关键字集合
        /// </summary>
        private static readonly int[] keys = new int[keyCount];
        /// <summary>
        /// 计时器
        /// </summary>
        private static readonly Stopwatch time = new Stopwatch();
        /// <summary>
        /// 任务等待
        /// </summary>
        private static readonly EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 删除数据次数
        /// </summary>
        private static int removeCount;
        /// <summary>
        /// 是否错误
        /// </summary>
        private static bool isError;

        /// <summary>
        /// 二叉树字典
        /// </summary>
        private static readonly AutoCSer.SearchTree.Dictionary<int, int> dictionary = new AutoCSer.SearchTree.Dictionary<int, int>();
        /// <summary>
        /// 创建字典测试
        /// </summary>
        private static void createDictionary()
        {
            try
            {
                removeCount = 0;
                AutoCSer.Random random = AutoCSer.Random.Default;
                int keyIndex = keyCount;
                time.Restart();
                for (int nextCount = count; nextCount > keyIndex; --nextCount)
                {
                    int index = (int)((uint)random.Next() & (keyCount - 1)), key = keys[index];
                    if (index < keyIndex)
                    {
                        if (!dictionary.TryAdd(ref key, key))
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        if (dictionary.Count != keyCount - keyIndex + 1)
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        keys[index] = keys[--keyIndex];
                        keys[keyIndex] = key;
                    }
                    else
                    {
                        if (!dictionary.Remove(ref key))
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        if (dictionary.Count != keyCount - keyIndex - 1)
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        keys[index] = keys[keyIndex];
                        ++removeCount;
                        keys[keyIndex++] = key;
                    }
                }
                while (keyIndex != 0)
                {
                    int index = (int)((uint)random.Next() % keyIndex), key = keys[index];
                    if (!dictionary.TryAdd(ref key, key))
                    {
                        isError = true;
                        Console.WriteLine("ERROR");
                        return;
                    }
                    if (dictionary.Count != keyCount - keyIndex + 1)
                    {
                        isError = true;
                        Console.WriteLine("ERROR");
                        return;
                    }
                    keys[index] = keys[--keyIndex];
                    keys[keyIndex] = key;
                }
                time.Stop();
                if (dictionary.Count != keyCount)
                {
                    isError = true;
                    Console.WriteLine("ERROR");
                    return;
                }
                for (int index = keyCount; index != 0; )
                {
                    --index;
                    if (dictionary.At(index).Key != index)
                    {
                        isError = true;
                        Console.WriteLine("ERROR");
                        return;
                    }
                }
            }
            finally
            {
                wait.Set();
            }
        }
        /// <summary>
        /// 查找数据测试
        /// </summary>
        private static void searchDictionary()
        {
            try
            {
                time.Restart();
                int value;
                for (int loopCount = count / keyCount; loopCount != 0; --loopCount)
                {
                    for (int index = keyCount; index != 0; )
                    {
                        if (!dictionary.TryGetValue(keys[--index], out value))
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                    }
                }
                time.Stop();
            }
            finally
            {
                dictionary.Clear();
                wait.Set();
            }
        }

        /// <summary>
        /// 二叉树集合
        /// </summary>
        private static readonly AutoCSer.SearchTree.Set<int> set = new AutoCSer.SearchTree.Set<int>();
        /// <summary>
        /// 创建集合测试
        /// </summary>
        private static void createSet()
        {
            try
            {
                removeCount = 0;
                AutoCSer.Random random = AutoCSer.Random.Default;
                int keyIndex = keyCount;
                time.Restart();
                for (int nextCount = count; nextCount > keyIndex; --nextCount)
                {
                    int index = (int)((uint)random.Next() & (keyCount - 1)), key = keys[index];
                    if (index < keyIndex)
                    {
                        if (!set.Add(ref key))
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        if (set.Count != keyCount - keyIndex + 1)
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        keys[index] = keys[--keyIndex];
                        keys[keyIndex] = key;
                    }
                    else
                    {
                        if (!set.Remove(ref key))
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        if (set.Count != keyCount - keyIndex - 1)
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                        keys[index] = keys[keyIndex];
                        ++removeCount;
                        keys[keyIndex++] = key;
                    }
                }
                while (keyIndex != 0)
                {
                    int index = (int)((uint)random.Next() % keyIndex), key = keys[index];
                    if (!set.Add(ref key))
                    {
                        isError = true;
                        Console.WriteLine("ERROR");
                        return;
                    }
                    if (set.Count != keyCount - keyIndex + 1)
                    {
                        isError = true;
                        Console.WriteLine("ERROR");
                        return;
                    }
                    keys[index] = keys[--keyIndex];
                    keys[keyIndex] = key;
                }
                time.Stop();
                if (set.Count != keyCount)
                {
                    isError = true;
                    Console.WriteLine("ERROR");
                    return;
                }
                for (int index = keyCount; index != 0; )
                {
                    --index;
                    if (set.At(index) != index)
                    {
                        isError = true;
                        Console.WriteLine("ERROR");
                        return;
                    }
                }
            }
            finally
            {
                wait.Set();
            }
        }
        /// <summary>
        /// 查找数据测试
        /// </summary>
        private static void searchSet()
        {
            try
            {
                time.Restart();
                for (int loopCount = count / keyCount; loopCount != 0; --loopCount)
                {
                    for (int index = keyCount; index != 0; )
                    {
                        if (!set.Contains(keys[--index]))
                        {
                            isError = true;
                            Console.WriteLine("ERROR");
                            return;
                        }
                    }
                }
                time.Stop();
            }
            finally
            {
                set.Clear();
                wait.Set();
            }
        }
    }
}
