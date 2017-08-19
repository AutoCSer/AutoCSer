using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenClientPerformance
{
    static unsafe class TcpOpenClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");
            TcpInternalClientPerformance.Client.Start("AutoCSer.TestCase.TcpOpenClientPerformance.Emit", test);
        }

        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>, bool> onAdd = TcpInternalClientPerformance.Client.OnAddEmit;
            int left = TcpInternalClientPerformance.Client.Left;
            AutoCSer.TestCase.TcpServerPerformance.IOpenServer client = AutoCSer.Net.TcpOpenServer.Emit.Client<AutoCSer.TestCase.TcpServerPerformance.IOpenServer>.Create();
            using (AutoCSer.Net.TcpOpenServer.Emit.MethodClient methodClient = client as AutoCSer.Net.TcpOpenServer.Emit.MethodClient)
            {
                tcpClient = methodClient._TcpClient_;
                TcpInternalClientPerformance.Client.SendCount = TcpInternalClientPerformance.Client.ReceiveCount = 0;

                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.Asynchronous, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.AddAsynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //该框架是为高吞吐的内部服务设计的，所以性能设计上对于客户端异步模式友好，而不利于客户端同步应答模式。
                //当然这种设计主要影响的客户端性能，可能需要多个客户端（多台客户机）同时采用多线程并发模式才能测试出服务端的吞吐性能上限。
                //客户端多线程同步应答模式会造成客户端线程切换问题大幅降低测试吞吐性能，同时会影响服务端批量处理数据的上限。
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientSynchronous, TcpInternalClientPerformance.Client.Count / 10);
                int threadCount = 64;
                for (int count = TcpInternalClientPerformance.Client.ThreadCount = threadCount, right = TcpInternalClientPerformance.Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = client, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //客户端单线程同步应答模式，会完全退化为普通的应答请求
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientSynchronous, TcpInternalClientPerformance.Client.Count / 100);
                for (int right = TcpInternalClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (client.Add(left, --right).Value != left + right) ++TcpInternalClientPerformance.Client.ErrorCount;
                }
                TcpInternalClientPerformance.Client.Time.Stop();
                TcpInternalClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("thread 1 end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();
            }
        }
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        private static AutoCSer.Net.TcpOpenServer.Client tcpClient;
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void wait()
        {
            TcpInternalClientPerformance.Client.WaitHandle.WaitOne();
            long milliseconds = Math.Max(TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(TcpInternalClientPerformance.Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (TcpInternalClientPerformance.Client.LoopCount / milliseconds) + "/ms send[" + TcpInternalClientPerformance.Client.GetSendCount(tcpClient.SendCount).toString() + "] receive[" + TcpInternalClientPerformance.Client.GetReceiveCount(tcpClient.ReceiveCount).toString() + "]" + (TcpInternalClientPerformance.Client.ErrorCount == 0 ? null : (" ERROR[" + TcpInternalClientPerformance.Client.ErrorCount.toString() + "]")) + " " + TcpInternalClientPerformance.Client.TestType.ToString());
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
