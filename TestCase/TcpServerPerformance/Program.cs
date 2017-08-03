using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    static class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
#if NoAutoCSer
#else
                using (processWait)
                using (InternalServer.TcpInternalServer server = new InternalServer.TcpInternalServer())
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Debug\AutoCSer.TestCase.TcpInternalClientPerformance.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Release\AutoCSer.TestCase.TcpInternalClientPerformance.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalClientPerformance.exe"));
                        if (clientFile.Exists) Process.Start(clientFile.FullName);
                        else Console.WriteLine("未找到 TCP 内部服务性能测试服务 客户端程序");
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                    else
                    {
                        Console.WriteLine("TCP 内部服务性能测试服务 启动失败");
                        Console.ReadKey();
                    }
                }
#endif
            }
        }
    }
}
