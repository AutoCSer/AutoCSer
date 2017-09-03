using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalStreamClientPerformance
{
    static unsafe class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceStreamServer.html
");
            Client.Start("AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit", test);
        }
        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            int left = Client.Left;
            AutoCSer.TestCase.TcpInternalStreamServerPerformance.IStreamServer client = AutoCSer.Net.TcpInternalStreamServer.Emit.Client<AutoCSer.TestCase.TcpInternalStreamServerPerformance.IStreamServer>.Create();
            using (AutoCSer.Net.TcpInternalStreamServer.Emit.MethodClient methodClient = client as AutoCSer.Net.TcpInternalStreamServer.Emit.MethodClient)
            {
                Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Synchronous;
                Client.SendCount = Client.ReceiveCount = 0;
                tcpClient = methodClient._TcpClient_;

                Client.Start(ClientTestType.Synchronous, Client.Count / 10);
                int threadCount = 64;
                for (int count = Client.ThreadCount = threadCount, right = Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = client, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                Client.Start(ClientTestType.Synchronous, Client.Count / 100);
                for (int right = Client.Count / 100; right != 0;)
                {
                    if (client.Add(left, --right).Value != left + right) ++Client.ErrorCount;
                }
                Client.Time.Stop();
                Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }

            AutoCSer.TestCase.TcpInternalStreamServerPerformance.ITcpQueueStreamServer tcpQueueClient = AutoCSer.Net.TcpInternalStreamServer.Emit.Client<AutoCSer.TestCase.TcpInternalStreamServerPerformance.ITcpQueueStreamServer>.Create();
            using (AutoCSer.Net.TcpInternalStreamServer.Emit.MethodClient methodClient = tcpQueueClient as AutoCSer.Net.TcpInternalStreamServer.Emit.MethodClient)
            {
                Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.TcpQueue;
                Client.SendCount = Client.ReceiveCount = 0;
                tcpClient = methodClient._TcpClient_;

                Client.Start(ClientTestType.Synchronous, Client.Count / 10);
                int threadCount = 64;
                for (int count = Client.ThreadCount = threadCount, right = Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = tcpQueueClient, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                Client.Start(ClientTestType.Synchronous, Client.Count / 100);
                for (int right = Client.Count / 100; right != 0;)
                {
                    if (tcpQueueClient.Add(left, --right).Value != left + right) ++Client.ErrorCount;
                }
                Client.Time.Stop();
                Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }

            AutoCSer.TestCase.TcpInternalStreamServerPerformance.IQueueStreamServer queueClient = AutoCSer.Net.TcpInternalStreamServer.Emit.Client<AutoCSer.TestCase.TcpInternalStreamServerPerformance.IQueueStreamServer>.Create();
            using (AutoCSer.Net.TcpInternalStreamServer.Emit.MethodClient methodClient = queueClient as AutoCSer.Net.TcpInternalStreamServer.Emit.MethodClient)
            {
                Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Queue;
                Client.SendCount = Client.ReceiveCount = 0;
                tcpClient = methodClient._TcpClient_;

                Client.Start(ClientTestType.Synchronous, Client.Count / 10);
                int threadCount = 64;
                for (int count = Client.ThreadCount = threadCount, right = Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = queueClient, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                Client.Start(ClientTestType.Synchronous, Client.Count / 100);
                for (int right = Client.Count / 100; right != 0;)
                {
                    if (queueClient.Add(left, --right).Value != left + right) ++Client.ErrorCount;
                }
                Client.Time.Stop();
                Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }
        }
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        private static AutoCSer.Net.TcpInternalStreamServer.Client tcpClient;
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void wait()
        {
            Client.WaitHandle.WaitOne();
            long milliseconds = Math.Max(Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (Client.LoopCount / milliseconds) + "/ms send[" + Client.GetSendCount(tcpClient.SendCount).toString() + "] receive[" + Client.GetReceiveCount(tcpClient.ReceiveCount).toString() + "]" + (Client.ErrorCount == 0 ? null : (" ERROR[" + Client.ErrorCount.toString() + "]")) + " " + Client.ServerTaskType.ToString());
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
