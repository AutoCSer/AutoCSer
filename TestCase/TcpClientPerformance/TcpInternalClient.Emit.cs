using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalClientPerformance
{
    static unsafe class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");
            Client.Start("AutoCSer.TestCase.TcpInternalClientPerformance.Emit", test);
        }
        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>, bool> onAdd = Client.OnAddEmit;
            Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpInternalServerPerformance.ServerCustomSerialize>, bool> onCustomSerialize = Client.OnAddEmit;
            int left = Client.Left;
            AutoCSer.TestCase.TcpInternalServerPerformance.IServer client = AutoCSer.Net.TcpInternalServer.Emit.Client<AutoCSer.TestCase.TcpInternalServerPerformance.IServer>.Create();
            using (AutoCSer.Net.TcpInternalServer.Emit.MethodClient methodClient = client as AutoCSer.Net.TcpInternalServer.Emit.MethodClient)
            {
                tcpClient = methodClient._TcpClient_;
                Client.SendCount = Client.ReceiveCount = 0;

                if (Client.IsCreatedProcessWait)
                {
                    Client.Start(TestType.CustomSerialize, Client.Count);
                    using (AutoCSer.Net.TcpServer.KeepCallback customSerializeKeep = client.AddCustomSerialize(onCustomSerialize))
                    {
                        new AutoCSer.TestCase.TcpInternalServerPerformance.ClientCustomSerializeOutput(left, Client.Count, client.AddCustomSerialize).Wait();
                        client.AddCustomSerializeFinally();
                        Console.WriteLine("loop end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                        wait();
                    }
                    sleep();

                    Client.Start(TestType.Register, Client.Count);
                    using (AutoCSer.Net.TcpServer.KeepCallback sendKeep = client.AddRegister(onAdd))
                    {
                        for (int right = Client.Count; right != 0; client.AddRegister(left, --right)) ;
                        Console.WriteLine("loop end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                        wait();
                    }
                    sleep();
                }

                Client.Start(TestType.Asynchronous, Client.Count);
                for (int right = Client.Count; right != 0; client.AddAsynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //该框架是为高吞吐的内部服务设计的，所以性能设计上对于客户端异步模式友好，而不利于客户端同步应答模式。
                //当然这种设计主要影响的客户端性能，可能需要多个客户端（多台客户机）同时采用多线程并发模式才能测试出服务端的吞吐性能上限。
                //客户端多线程同步应答模式会造成客户端线程切换问题大幅降低测试吞吐性能，同时会影响服务端批量处理数据的上限。
                Client.Start(TestType.ClientSynchronous, Client.Count / 10);
                int threadCount = 64;
                for (int count = Client.ThreadCount = threadCount, right = Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = client, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //客户端单线程同步应答模式，会完全退化为普通的应答请求
                Client.Start(TestType.ClientSynchronous, Client.Count / 100);
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
        }
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        private static AutoCSer.Net.TcpInternalServer.Client tcpClient;
        /// <summary>
        /// 等待测试结束
        /// </summary>
        private static void wait()
        {
            Client.WaitHandle.WaitOne();
            long milliseconds = Math.Max(Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (Client.LoopCount / milliseconds) + "/ms send[" + Client.GetSendCount(tcpClient.SendCount).toString() + "] receive[" + Client.GetReceiveCount(tcpClient.ReceiveCount).toString() + "]" + (Client.ErrorCount == 0 ? null : (" ERROR[" + Client.ErrorCount.toString() + "]")) + " " + Client.TestType.ToString());
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
