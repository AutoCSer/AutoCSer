using System;
using System.Threading;

namespace AutoCSer.Example.TcpOpenServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
                    Console.WriteLine(NoAttribute.TestCase());
                    Console.WriteLine(Static.TestCase());
                    Console.WriteLine(Field.TestCase());
                    Console.WriteLine(Property.TestCase());
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(ClientAsynchronous.TestCase());
#if !DOTNET2 && !DOTNET4
                    Console.WriteLine(ClientTaskAsync.TestCase());
#endif
                    Console.WriteLine(SendOnly.TestCase());
                    Console.WriteLine(Asynchronous.TestCase());
                    Console.WriteLine(KeepCallback.TestCase());
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
