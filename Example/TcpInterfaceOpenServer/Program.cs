using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceOpenServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if NETCOREAPP2_0
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInterfaceOpenServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(SendOnly.TestCase());
                    Console.WriteLine(Asynchronous.TestCase());
                    Console.WriteLine(KeepCallback.TestCase());
                    Console.WriteLine(Inherit.TestCase());
                    Console.WriteLine("Over");
                    Console.ReadKey();
#if NETCOREAPP2_0
#else
                }
            }
#endif
        }
    }
}
