using System;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenClientPerformance
{
    static unsafe class TcpOpenClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
            TcpInternalClientPerformance.Client.Start("AutoCSer.TestCase.TcpOpenClientPerformance", test);
        }

        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> onAdd = TcpInternalClientPerformance.Client.OnAdd;
            int left = TcpInternalClientPerformance.Client.Left;
            using (AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer.TcpOpenClient client = new AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer.TcpOpenClient())
            {
                tcpClient = client._TcpClient_;
                TcpInternalClientPerformance.Client.SendCount = TcpInternalClientPerformance.Client.ReceiveCount = 0;

                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.Asynchronous, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.addAsynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.Synchronous, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.addSynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.Queue, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.addQueue(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.Timeout, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.addTimeoutTask(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.TcpTask, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.addTcpTask(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //服务端应尽量统一线程模式，混合测试会因为线程切换与 CPU 缓存同步问题大幅降低测试吞吐性能
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.Mixing, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0;)
                {
                    client.addAsynchronous(left, --right, onAdd);
                    client.addSynchronous(left, --right, onAdd);
                    client.addQueue(left, --right, onAdd);
                    client.addTcpTask(left, --right, onAdd);
                    client.addTimeoutTask(left, --right, onAdd);
                }
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

#if DOTNET2
                //.NET 2.0 线程池调度太烂，任务数量 / 10
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ThreadPool, TcpInternalClientPerformance.Client.Count / 10);
                for (int right = TcpInternalClientPerformance.Client.Count / 10; right != 0; client.addThreadPool(left, --right, onAdd)) ;
#else
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ThreadPool, TcpInternalClientPerformance.Client.Count);
                for (int right = TcpInternalClientPerformance.Client.Count; right != 0; client.addThreadPool(left, --right, onAdd)) ;
#endif
                Console.WriteLine("loop end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                GC.Collect();
                client.gcCollect();
                sleep();

#if DOTNET2 || DOTNET4
                int awaitCount = TcpInternalClientPerformance.Client.Count / 10, threadCount = 100;
#else
                int awaitCount = TcpInternalClientPerformance.Client.Count, threadCount = 500;
#endif
                //并发线程较多的时候测试吞吐性能接近与异步模式
                TcpInternalClientPerformance.Client.ThreadCount = threadCount;
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientAwaiter, awaitCount);
                for (int count = TcpInternalClientPerformance.Client.ThreadCount = threadCount, right = awaitCount / threadCount; count != 0; --count)
                {

#if DOTNET2
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientAwaiter { Client = client, Left = left, Right = right }.Run);
#elif DOTNET4
                    new System.Threading.Tasks.Task(new ClientAwaiter { Client = client, Left = left, Right = right }.Run).Start();
#else
                    new ClientAwaiter { Client = client, Left = left, Right = right }.Run();
#endif
                }
                Console.WriteLine("await start " + threadCount.toString() + " end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //在同步上下文环境中的测试吞吐不如普通的同步应答模式
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientAwaiter, TcpInternalClientPerformance.Client.Count / 100);
                for (int right = TcpInternalClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (client.addAwaiter(left, --right).Wait().Result.Value != left + right) ++TcpInternalClientPerformance.Client.ErrorCount;
                }
                TcpInternalClientPerformance.Client.Time.Stop();
                TcpInternalClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("await Result");
                wait();
                sleep();

#if !DOTNET2 && !DOTNET4
                TcpInternalClientPerformance.Client.ThreadCount = 1;
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientAwaiter, TcpInternalClientPerformance.Client.Count / 100);
                new ClientAwaiter { Client = client, Left = left, Right = TcpInternalClientPerformance.Client.Count / 100 }.Run();
                Console.WriteLine("await 1");
                wait();
                sleep();

                //并发线程较多的时候测试吞吐性能可能高于单纯的同步模式
                TcpInternalClientPerformance.Client.ThreadCount = threadCount = 250;
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientTaskAsync, TcpInternalClientPerformance.Client.Count / 10);
                for (int count = TcpInternalClientPerformance.Client.ThreadCount = threadCount, right = TcpInternalClientPerformance.Client.Count / 10 / threadCount; count != 0; --count)
                {
                    new ClientTaskAsync { Client = client, Left = left, Right = right }.Run();
                }
                Console.WriteLine("task start " + threadCount.toString() + " end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //在同步上下文环境中的测试吞吐不如普通的同步应答模式
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientTaskAsync, TcpInternalClientPerformance.Client.Count / 100);
                for (int right = TcpInternalClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (client.addAsync(left, --right).Result.Value != left + right) ++TcpInternalClientPerformance.Client.ErrorCount;
                }
                TcpInternalClientPerformance.Client.Time.Stop();
                TcpInternalClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("task Result");
                wait();
                sleep();

                TcpInternalClientPerformance.Client.ThreadCount = 1;
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientTaskAsync, TcpInternalClientPerformance.Client.Count / 100);
                new ClientTaskAsync { Client = client, Left = left, Right = TcpInternalClientPerformance.Client.Count / 100 }.Run();
                Console.WriteLine("task 1");
                wait();
                sleep();
#endif
                //该框架是为高吞吐的内部服务设计的，所以性能设计上对于客户端异步模式友好，而不利于客户端同步应答模式。
                //当然这种设计主要影响的客户端性能，可能需要多个客户端（多台客户机）同时采用多线程并发模式才能测试出服务端的吞吐性能上限。
                //客户端多线程同步应答模式会造成客户端线程切换问题大幅降低测试吞吐性能，同时会影响服务端批量处理数据的上限。
                TcpInternalClientPerformance.Client.ThreadCount = threadCount = 64;
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientSynchronous, TcpInternalClientPerformance.Client.Count / 10);
                for (int count = threadCount, right = TcpInternalClientPerformance.Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = client, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread start " + threadCount.toString() + " end " + TcpInternalClientPerformance.Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait();
                sleep();

                //客户端单线程同步应答模式，会完全退化为普通的应答请求
                TcpInternalClientPerformance.Client.Start(TcpInternalClientPerformance.TestType.ClientSynchronous, TcpInternalClientPerformance.Client.Count / 100);
                for (int right = TcpInternalClientPerformance.Client.Count / 100; right != 0;)
                {
                    if (client.add(left, --right).Value != left + right) ++TcpInternalClientPerformance.Client.ErrorCount;
                }
                TcpInternalClientPerformance.Client.Time.Stop();
                TcpInternalClientPerformance.Client.WaitHandle.Set();
                Console.WriteLine("thread 1");
                wait();
                sleep();
            }
        }
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        private static AutoCSer.Net.TcpOpenServer.Client<AutoCSer.TestCase.TcpOpenServerPerformance.OpenServer.TcpOpenClient> tcpClient;
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
