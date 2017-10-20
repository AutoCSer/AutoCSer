using System;
using System.Threading;

namespace AutoCSer.Example.TcpStaticStreamServer
{
    class Program
    {
        //[AutoCSer.Metadata.TestMethod]
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpStaticStreamServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    using (AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer.StreamExample1 server1 = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer.StreamExample1())
                    using (AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer.StreamExample2 server2 = new AutoCSer.Example.TcpStaticStreamServer.TcpStaticStreamServer.StreamExample2())
                    {
                        Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodStreamServer.html
");
                        if (server1.IsListen && server2.IsListen)
                        {
                            Console.WriteLine(NoAttribute.TestCase());
                            Console.WriteLine(Static.TestCase());
                            Console.WriteLine(Field.TestCase());
                            Console.WriteLine(Property.TestCase());
                            Console.WriteLine(RefOut.TestCase());
                            Console.WriteLine(ClientAsynchronous.TestCase());
                            Console.WriteLine(SendOnly.TestCase());
                            Console.WriteLine(Expression.TestCase());
                            Console.WriteLine("Over");
                        }
                        else Console.WriteLine("示例服务启动失败");
                        Console.ReadKey();
                    }
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
