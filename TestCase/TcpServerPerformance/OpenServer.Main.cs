using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.TestCase.TcpServerPerformance
{
    partial class OpenServer
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");
                using (processWait)
                using (AutoCSer.Net.TcpOpenServer.Server server = AutoCSer.Net.TcpOpenServer.Emit.Server<IOpenServer>.Create(new OpenServer()))
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Debug\AutoCSer.TestCase.TcpOpenClientPerformance.Emit.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Release\AutoCSer.TestCase.TcpOpenClientPerformance.Emit.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenClientPerformance.exe.Emit"));
                        if (clientFile.Exists) Process.Start(clientFile.FullName);
                        else Console.WriteLine("未找到 TCP 服务性能测试服务 客户端程序");
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                    else
                    {
                        Console.WriteLine("TCP 服务性能测试服务 启动失败");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
