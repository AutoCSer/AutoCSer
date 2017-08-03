using System;
using System.Threading;

namespace AutoCSer.Example.TcpInternalServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Example.TcpInternalServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
                    Console.WriteLine(NoAttribute.TestCase());
                    Console.WriteLine(Static.TestCase());
                    Console.WriteLine(Field.TestCase());
                    Console.WriteLine(Property.TestCase());
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(ClientAsynchronous.TestCase());
#if DOTNET2
#else
#if DOTNET4
#else
                    Console.WriteLine(ClientTaskAsync.TestCase());
#endif
#endif
                    Console.WriteLine(SendOnly.TestCase());
                    Console.WriteLine(Asynchronous.TestCase());
                    Console.WriteLine(KeepCallback.TestCase());
                    Console.WriteLine("Over");
                    Console.ReadKey();
                }
            }
        }
    }
}
