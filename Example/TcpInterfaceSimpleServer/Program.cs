using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceSimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Example.TcpInterfaceSimpleServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleInterfaceServer.html
");
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(Inherit.TestCase());
                    Console.WriteLine("Over");
                    Console.ReadKey();
                }
            }
        }
    }
}
