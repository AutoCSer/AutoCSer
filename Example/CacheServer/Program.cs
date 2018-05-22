using System;
using System.Threading;

namespace AutoCSer.Example.CacheServer
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.CacheServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/CacheServer/Index.html
");
                    using (AutoCSer.CacheServer.MasterServer.TcpInternalServer server = new AutoCSer.CacheServer.MasterServer.TcpInternalServer())
                    {
                        if (server.IsListen)
                        {
                            using (AutoCSer.CacheServer.MasterClient client = new AutoCSer.CacheServer.MasterClient())
                            {
                                Console.WriteLine(ValueArray.TestCase(client));
                                Console.WriteLine(ValueDictionary.TestCase(client));
                                Console.WriteLine(ValueSearchTreeDictionary.TestCase(client));
                                Console.WriteLine(HashSet.TestCase(client));
                                Console.WriteLine(Link.TestCase(client));

                                Console.WriteLine(Array.TestCase(client));
                                Console.WriteLine(Dictionary.TestCase(client));
                                Console.WriteLine(SearchTreeDictionary.TestCase(client));
                                Console.WriteLine("Over");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("缓存服务启动失败");
                            Console.ReadKey();
                        }
                    }
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
