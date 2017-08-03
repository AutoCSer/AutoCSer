using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    static class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");
                using (processWait)
                using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IServer>.Create(new InternalServer()))
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Debug\AutoCSer.TestCase.TcpInternalClientPerformance.Emit.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Release\AutoCSer.TestCase.TcpInternalClientPerformance.Emit.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalClientPerformance.Emit.exe"));
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
            }
        }
    }
}
