using System;
using System.Threading;

namespace AutoCSer.Example.TcpOpenSimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Example.TcpOpenSimpleServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleMethodServer.html
");
                    Console.WriteLine(NoAttribute.TestCase());
                    Console.WriteLine(Static.TestCase());
                    Console.WriteLine(Field.TestCase());
                    Console.WriteLine(Property.TestCase());
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(Asynchronous.TestCase());
                    Console.WriteLine("Over");
                    Console.ReadKey();
                }
            }
        }
    }
}
