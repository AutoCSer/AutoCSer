﻿using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceStreamServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInterfaceStreamServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceStreamServer.html
");
                    Console.WriteLine(RefOut.TestCase());
                    Console.WriteLine(SendOnly.TestCase());
#if !DOTNET2 && !DOTNET4
                    Console.WriteLine(ClientAwaiter.TestCase());
#endif
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
