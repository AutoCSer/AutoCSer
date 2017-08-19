using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalSimpleClientPerformance
{
    static unsafe class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleInterfaceServer.html
");
            Client.Start("AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit", test);
        }
        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            int left = Client.Left;
            AutoCSer.TestCase.TcpInternalSimpleServerPerformance.ISimpleServer client = AutoCSer.Net.TcpInternalSimpleServer.Emit.Client<AutoCSer.TestCase.TcpInternalSimpleServerPerformance.ISimpleServer>.Create();
            using (AutoCSer.Net.TcpInternalSimpleServer.Emit.MethodClient methodClient = client as AutoCSer.Net.TcpInternalSimpleServer.Emit.MethodClient)
            {
                Client.Start(TestType.Synchronous, Client.Count);
                for (int right = Client.Count; right != 0;)
                {
                    if (client.Add(left, --right).Value != left + right) ++Client.ErrorCount;
                }
                stop();
            }

            for (int threadCount = 2; threadCount <= 32; threadCount <<= 1)
            {
                Client.ThreadCount = threadCount;
                Client.Start(TestType.Multithreading, Client.Count * threadCount);
                for (int count = threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientThread { Left = left, Right = Client.Count }.Run);
                }
                Console.WriteLine("thread start " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
            }
        }
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void wait()
        {
            Client.WaitHandle.WaitOne();
            sleep();
        }
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void stop()
        {
            Client.Time.Stop();
            sleep();
        }
        /// <summary>
        /// 休息 3 秒
        /// </summary>
        private static void sleep()
        {
            long milliseconds = Math.Max(Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (Client.LoopCount / milliseconds) + "/ms " + (Client.ErrorCount == 0 ? null : (" ERROR[" + Client.ErrorCount.toString() + "]")) + " " + Client.TestType.ToString());

            Console.WriteLine(@"Sleep 3000ms
");
            System.Threading.Thread.Sleep(3000);
        }
    }
}
