using System;
using System.Diagnostics;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.TestCase.TcpInternalStreamClientPerformance
{
    static unsafe class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodStreamServer.html
");
            Client.Start("AutoCSer.TestCase.TcpInternalStreamClientPerformance", test);
        }
        /// <summary>
        /// 测试
        /// </summary>
        private static void test()
        {
            Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add>> onAdd = Client.OnAdd;
            int left = Client.Left;
            using (AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer.TcpInternalStreamClient client = new AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamTcpQueueServer.TcpInternalStreamClient())
            {
                Client.SendCount = Client.ReceiveCount = 0;
                Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.TcpQueue;

                Client.Start(ClientTestType.Asynchronous, Client.Count);
                for (int right = Client.Count; right != 0; client.addAsynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();
            }
            using (AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer.TcpInternalStreamClient client = new AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamQueueServer.TcpInternalStreamClient())
            {
                Client.SendCount = Client.ReceiveCount = 0;
                Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Queue;

                Client.Start(ClientTestType.Asynchronous, Client.Count);
                for (int right = Client.Count; right != 0; client.addAsynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();
            }
            using (AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer.TcpInternalStreamClient client = new AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer.TcpInternalStreamClient())
            {
                Client.SendCount = Client.ReceiveCount = 0;
                Client.ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Synchronous;

                Client.Start(ClientTestType.Asynchronous, Client.Count);
                for (int right = Client.Count; right != 0; client.addAsynchronous(left, --right, onAdd)) ;
                Console.WriteLine("loop end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

#if DOTNET2 || DOTNET4
                int awaitCount = Client.Count / 10, threadCount = 200;
#else
                int awaitCount = Client.Count, threadCount = 1000;
#endif
                //并发线程较多的时候测试吞吐性能接近与异步模式
                Client.ThreadCount = threadCount;
                Client.Start(ClientTestType.Awaiter, awaitCount);
                for (int count = threadCount, right = awaitCount / threadCount; count != 0; --count)
                {

#if DOTNET2
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientAwaiter { Client = client, Left = left, Right = right }.Run);
#elif DOTNET4
                    new System.Threading.Tasks.Task(new ClientAwaiter { Client = client, Left = left, Right = right }.Run).Start();
#else
                    new ClientAwaiter { Client = client, Left = left, Right = right }.Run();
#endif
                }
                Console.WriteLine("await start " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

                //在同步上下文环境中的测试吞吐不如普通的同步应答模式
                Client.Start(ClientTestType.Awaiter, Client.Count / 100);
                for (int right = Client.Count / 100; right != 0;)
                {
                    if (client.addAwaiter(left, --right).Wait().Result != left + right) ++Client.ErrorCount;
                }
                Client.Time.Stop();
                Client.WaitHandle.Set();
                Console.WriteLine("await Result");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

#if !DOTNET2 && !DOTNET4
                Client.ThreadCount = 1;
                Client.Start(ClientTestType.Awaiter, Client.Count / 100);
                new ClientAwaiter { Client = client, Left = left, Right = Client.Count / 100 }.Run();
                Console.WriteLine("await 1");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

                //并发线程较多的时候测试吞吐性能可能高于单纯的同步模式
                Client.ThreadCount = threadCount = 250;
                Client.Start(ClientTestType.TaskAsync, Client.Count / 10);
                for (int count = threadCount, right = Client.Count / 10 / threadCount; count != 0; --count)
                {
                    new ClientTaskAsync { Client = client, Left = left, Right = right }.Run();
                }
                Console.WriteLine("task start " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

                //在同步上下文环境中的测试吞吐不如普通的同步应答模式
                Client.Start(ClientTestType.TaskAsync, Client.Count / 100);
                for (int right = Client.Count / 100; right != 0;)
                {
                    if (client.addAsync(left, --right).Result.Value != left + right) ++Client.ErrorCount;
                }
                Client.Time.Stop();
                Client.WaitHandle.Set();
                Console.WriteLine("task Result");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

                Client.ThreadCount = 1;
                Client.Start(ClientTestType.TaskAsync, Client.Count / 100);
                new ClientTaskAsync { Client = client, Left = left, Right = Client.Count / 100 }.Run();
                Console.WriteLine("task 1");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();
#endif
                //该框架是为高吞吐的内部服务设计的，所以性能设计上对于客户端异步模式友好，而不利于客户端同步应答模式。
                //当然这种设计主要影响的客户端性能，可能需要多个客户端（多台客户机）同时采用多线程并发模式才能测试出服务端的吞吐性能上限。
                //客户端多线程同步应答模式会造成客户端线程切换问题大幅降低测试吞吐性能，同时会影响服务端批量处理数据的上限。
                Client.ThreadCount = threadCount = 64;
                Client.Start(ClientTestType.Synchronous, Client.Count / 10);
                for (int count = threadCount, right = Client.Count / 10 / threadCount; count != 0; --count)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(new ClientSynchronous { Client = client, Left = left, Right = right }.Run);
                }
                Console.WriteLine("thread start " + threadCount.toString() + " end " + Client.Time.ElapsedMilliseconds.toString() + "ms");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();

                //客户端单线程同步应答模式，会完全退化为普通的应答请求
                Client.Start(ClientTestType.Synchronous, Client.Count / 100);
                for (int right = Client.Count / 100; right != 0;)
                {
                    if (client.add(left, --right).Value != left + right) ++Client.ErrorCount;
                }
                Client.Time.Stop();
                Client.WaitHandle.Set();
                Console.WriteLine("thread 1");
                wait(client._TcpClient_.SendCount, client._TcpClient_.ReceiveCount);
                sleep();
            }
        }
        /// <summary>
        /// 等待测试结束
        /// </summary>
        /// <param name="sendCount"></param>
        /// <param name="receiveCount"></param>
        private static void wait(int sendCount, int receiveCount)
        {
            Client.WaitHandle.WaitOne();
            long milliseconds = Math.Max(Client.Time.ElapsedMilliseconds, 1);
            Console.WriteLine(Client.LoopCount.toString() + " / " + milliseconds.toString() + "ms = " + (Client.LoopCount / milliseconds) + "/ms send[" + Client.GetSendCount(sendCount).toString() + "] receive[" + Client.GetReceiveCount(receiveCount).toString() + "]" + (Client.ErrorCount == 0 ? null : (" ERROR[" + Client.ErrorCount.toString() + "]")) + " " + Client.ServerTaskType.ToString() + " + " + Client.TestType.ToString());
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
