using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Example.TcpInterfaceServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(SendOnly.TestCase());
                    Console.WriteLine(Asynchronous.TestCase());
                    Console.WriteLine(KeepCallback.TestCase());
                    Console.WriteLine(Inherit.TestCase());
                    Console.WriteLine("Over");
                    Console.ReadKey();
                }
            }
        }
    }
}
