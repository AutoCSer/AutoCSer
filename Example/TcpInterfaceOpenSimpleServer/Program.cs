using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceOpenSimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInterfaceOpenSimpleServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleInterfaceServer.html
");
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(Inherit.TestCase());
                    Console.WriteLine(Expression.TestCase());
                    Console.WriteLine("Over");
                    Console.ReadKey();
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
