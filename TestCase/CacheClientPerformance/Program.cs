using System;
using System.Threading;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    static unsafe class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.CacheClientPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/CacheServer/Index.html
");
                    AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.CacheServer.MasterServer), false);
                    serverAttribute.VerifyString = "!2#4%6&8QwErTyAsDfZx";
                    AutoCSer.Net.TcpInternalServer.ServerAttribute fileServerAttribute = AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute);
                    fileServerAttribute.Port -= 1;
                    using (AutoCSer.CacheServer.Client client = new AutoCSer.CacheServer.Client(new AutoCSer.CacheServer.MasterServer.TcpInternalClient(serverAttribute)))
                    using (AutoCSer.CacheServer.Client fileClient = new AutoCSer.CacheServer.Client(new AutoCSer.CacheServer.MasterServer.TcpInternalClient(fileServerAttribute)))
                    {
                        do
                        {
                            new MessageQueue(fileClient).Test();
                            new MesssageDistributor(fileClient).Test();

                            new Dictionary(client, false).Test();
                            new Dictionary(fileClient, true).Test();

                            new Array(client, false).Test();
                            new Array(fileClient, true).Test();

                            new Binary(client, false).Test();
                            new Binary(fileClient, true).Test();

                            new Json(client, false).Test();
                            new Json(fileClient, true).Test();
                        }
                        while (true);
                    }
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
