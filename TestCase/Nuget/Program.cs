using System;
using System.Threading;

namespace AutoCSer.TestCase.Nuget
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.Nuget", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");

                    using (Server.TcpInternalServer server = new Server.TcpInternalServer())
                    {
                        if (server.IsListen)
                        {
                            using (Server.TcpInternalClient client = new Server.TcpInternalClient())
                            {
                                Console.WriteLine(client.add(2, 3) == 2 + 3);
                            }
                        }
                    }
                    Console.WriteLine("Over");
                    Console.ReadKey();
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
