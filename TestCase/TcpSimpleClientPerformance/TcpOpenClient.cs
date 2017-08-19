using System;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenSimpleClientPerformance
{
    static unsafe class TcpOpenClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleMethodServer.html
");
            TcpInternalSimpleClientPerformance.Client.Start("AutoCSer.TestCase.TcpOpenSimpleClientPerformance", test);
        }

        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            int left = TcpInternalSimpleClientPerformance.Client.Left;
            using (AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer.TcpOpenSimpleClient client = new AutoCSer.TestCase.TcpOpenSimpleServerPerformance.OpenSimpleServer.TcpOpenSimpleClient())
            {
                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.Synchronous, TcpInternalSimpleClientPerformance.Client.Count);
                for (int right = TcpInternalSimpleClientPerformance.Client.Count; right != 0;)
                {
                    if (client.addAsynchronous(left, --right).Value != left + right) ++TcpInternalSimpleClientPerformance.Client.ErrorCount;
                }
                stop();

                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.Asynchronous, TcpInternalSimpleClientPerformance.Client.Count);
                for (int right = TcpInternalSimpleClientPerformance.Client.Count; right != 0;)
                {
                    if (client.addAsynchronous(left, --right).Value != left + right) ++TcpInternalSimpleClientPerformance.Client.ErrorCount;
                }
                stop();

                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.Queue, TcpInternalSimpleClientPerformance.Client.Count);
                for (int right = TcpInternalSimpleClientPerformance.Client.Count; right != 0;)
                {
                    if (client.addQueue(left, --right).Value != left + right) ++TcpInternalSimpleClientPerformance.Client.ErrorCount;
                }
                stop();

                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.Timeout, TcpInternalSimpleClientPerformance.Client.Count);
                for (int right = TcpInternalSimpleClientPerformance.Client.Count; right != 0;)
                {
                    if (client.addTimeoutTask(left, --right).Value != left + right) ++TcpInternalSimpleClientPerformance.Client.ErrorCount;
                }
                stop();

                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.TcpTask, TcpInternalSimpleClientPerformance.Client.Count);
                for (int right = TcpInternalSimpleClientPerformance.Client.Count; right != 0;)
                {
                    if (client.addTcpTask(left, --right).Value != left + right) ++TcpInternalSimpleClientPerformance.Client.ErrorCount;
                }
                stop();

                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.ThreadPool, TcpInternalSimpleClientPerformance.Client.Count);
                for (int right = TcpInternalSimpleClientPerformance.Client.Count; right != 0;)
                {
                    if (client.addThreadPool(left, --right).Value != left + right) ++TcpInternalSimpleClientPerformance.Client.ErrorCount;
                }
                stop();
            }
            for (int threadCount = 2; threadCount <= 32; threadCount <<= 1)
            {
                TcpInternalSimpleClientPerformance.Client.ThreadCount = threadCount;
                TcpInternalSimpleClientPerformance.Client.Start(TcpInternalSimpleClientPerformance.TestType.Multithreading, TcpInternalSimpleClientPerformance.Client.Count * threadCount);
                for (int count = threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientThread { Left = left, Right = TcpInternalSimpleClientPerformance.Client.Count }.Run);
                }
                Console.WriteLine("thread start " + threadCount.toString() + " end " + TcpInternalSimpleClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
            }
        }
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void wait()
        {
            TcpInternalSimpleClientPerformance.Client.WaitHandle.WaitOne();
            sleep();
        }
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void stop()
        {
            TcpInternalSimpleClientPerformance.Client.Time.Stop();
            sleep();
        }
        /// <summary>
        /// 休息 3 秒
        /// </summary>
        private static void sleep()
        {
            long milliseconds = Math.Max(TcpInternalSimpleClientPerformance.Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(TcpInternalSimpleClientPerformance.Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (TcpInternalSimpleClientPerformance.Client.LoopCount / milliseconds) + "/ms " + (TcpInternalSimpleClientPerformance.Client.ErrorCount == 0 ? null : (" ERROR[" + TcpInternalSimpleClientPerformance.Client.ErrorCount.toString() + "]")) + " " + TcpInternalSimpleClientPerformance.Client.TestType.ToString());

            Console.WriteLine(@"Sleep 3000ms
");
            System.Threading.Thread.Sleep(3000);
        }
    }
}
