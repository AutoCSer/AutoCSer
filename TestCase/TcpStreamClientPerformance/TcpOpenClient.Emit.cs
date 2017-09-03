using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenStreamClientPerformance
{
    static unsafe class TcpOpenClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceStreamServer.html
");
            TcpInternalStreamClientPerformance.Client.Start("AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit", test);
        }

        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            int left = TcpInternalStreamClientPerformance.Client.Left;
            AutoCSer.TestCase.TcpStreamServerPerformance.IOpenStreamServer client = AutoCSer.Net.TcpOpenStreamServer.Emit.Client<AutoCSer.TestCase.TcpStreamServerPerformance.IOpenStreamServer>.Create();
            using (AutoCSer.Net.TcpOpenStreamServer.Emit.MethodClient methodClient = client as AutoCSer.Net.TcpOpenStreamServer.Emit.MethodClient)
            {
                tcpClient = methodClient._TcpClient_;
                TcpInternalStreamClientPerformance.Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Synchronous;
                TcpInternalStreamClientPerformance.Client.SendCount = TcpInternalStreamClientPerformance.Client.ReceiveCount = 0;

                TcpInternalStreamClientPerformance.Client.Start(TcpInternalStreamClientPerformance.ClientTestType.Synchronous, TcpInternalStreamClientPerformance.Client.Count / 10);
                int threadCount = 64;
                for (int count = TcpInternalStreamClientPerformance.Client.ThreadCount = threadCount, right = TcpInternalStreamClientPerformance.Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = client, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalStreamClientPerformance.Client.Start(TcpInternalStreamClientPerformance.ClientTestType.Synchronous, TcpInternalStreamClientPerformance.Client.Count / 100);
                for (int right = TcpInternalStreamClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (client.Add(left, --right).Value != left + right) ++TcpInternalStreamClientPerformance.Client.ErrorCount;
                }
                TcpInternalStreamClientPerformance.Client.Time.Stop();
                TcpInternalStreamClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }
            AutoCSer.TestCase.TcpStreamServerPerformance.ITcpQueueOpenStreamServer tcpQueueClient = AutoCSer.Net.TcpOpenStreamServer.Emit.Client<AutoCSer.TestCase.TcpStreamServerPerformance.ITcpQueueOpenStreamServer>.Create();
            using (AutoCSer.Net.TcpOpenStreamServer.Emit.MethodClient methodClient = tcpQueueClient as AutoCSer.Net.TcpOpenStreamServer.Emit.MethodClient)
            {
                tcpClient = methodClient._TcpClient_;
                TcpInternalStreamClientPerformance.Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.TcpQueue;
                TcpInternalStreamClientPerformance.Client.SendCount = TcpInternalStreamClientPerformance.Client.ReceiveCount = 0;

                TcpInternalStreamClientPerformance.Client.Start(TcpInternalStreamClientPerformance.ClientTestType.Synchronous, TcpInternalStreamClientPerformance.Client.Count / 10);
                int threadCount = 64;
                for (int count = TcpInternalStreamClientPerformance.Client.ThreadCount = threadCount, right = TcpInternalStreamClientPerformance.Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = tcpQueueClient, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalStreamClientPerformance.Client.Start(TcpInternalStreamClientPerformance.ClientTestType.Synchronous, TcpInternalStreamClientPerformance.Client.Count / 100);
                for (int right = TcpInternalStreamClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (tcpQueueClient.Add(left, --right).Value != left + right) ++TcpInternalStreamClientPerformance.Client.ErrorCount;
                }
                TcpInternalStreamClientPerformance.Client.Time.Stop();
                TcpInternalStreamClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }
            AutoCSer.TestCase.TcpStreamServerPerformance.IQueueOpenStreamServer queueClient = AutoCSer.Net.TcpOpenStreamServer.Emit.Client<AutoCSer.TestCase.TcpStreamServerPerformance.IQueueOpenStreamServer>.Create();
            using (AutoCSer.Net.TcpOpenStreamServer.Emit.MethodClient methodClient = queueClient as AutoCSer.Net.TcpOpenStreamServer.Emit.MethodClient)
            {
                tcpClient = methodClient._TcpClient_;
                TcpInternalStreamClientPerformance.Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Queue;
                TcpInternalStreamClientPerformance.Client.SendCount = TcpInternalStreamClientPerformance.Client.ReceiveCount = 0;

                TcpInternalStreamClientPerformance.Client.Start(TcpInternalStreamClientPerformance.ClientTestType.Synchronous, TcpInternalStreamClientPerformance.Client.Count / 10);
                int threadCount = 64;
                for (int count = TcpInternalStreamClientPerformance.Client.ThreadCount = threadCount, right = TcpInternalStreamClientPerformance.Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = queueClient, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalStreamClientPerformance.Client.Start(TcpInternalStreamClientPerformance.ClientTestType.Synchronous, TcpInternalStreamClientPerformance.Client.Count / 100);
                for (int right = TcpInternalStreamClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (queueClient.Add(left, --right).Value != left + right) ++TcpInternalStreamClientPerformance.Client.ErrorCount;
                }
                TcpInternalStreamClientPerformance.Client.Time.Stop();
                TcpInternalStreamClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }
        }
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        private static AutoCSer.Net.TcpOpenStreamServer.Client tcpClient;
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void wait()
        {
            TcpInternalStreamClientPerformance.Client.WaitHandle.WaitOne();
            long milliseconds = Math.Max(TcpInternalStreamClientPerformance.Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(TcpInternalStreamClientPerformance.Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (TcpInternalStreamClientPerformance.Client.LoopCount / milliseconds) + "/ms send[" + TcpInternalStreamClientPerformance.Client.GetSendCount(tcpClient.SendCount).toString() + "] receive[" + TcpInternalStreamClientPerformance.Client.GetReceiveCount(tcpClient.ReceiveCount).toString() + "]" + (TcpInternalStreamClientPerformance.Client.ErrorCount == 0 ? null : (" ERROR[" + TcpInternalStreamClientPerformance.Client.ErrorCount.toString() + "]")) + " " + TcpInternalStreamClientPerformance.Client.ServerTaskType.ToString());
        }
        /// <summary>
        /// 休息 3 秒
        /// </summary>
        private static void sleep()
        {
            Console.WriteLine(@"Sleep 3000ms
");
            System.Threading.Thread.Sleep(3000);
        }
    }
}
