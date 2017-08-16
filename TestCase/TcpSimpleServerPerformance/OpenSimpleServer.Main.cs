using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.TestCase.TcpServerPerformance
{
    partial class OpenSimpleServer
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenSimpleServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleInterfaceServer.html
");
                using (processWait)
                using (AutoCSer.Net.TcpOpenSimpleServer.Server server = AutoCSer.Net.TcpOpenSimpleServer.Emit.Server<IOpenSimpleServer>.Create(new OpenSimpleServer()))
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Debug\AutoCSer.TestCase.TcpOpenSimpleClientPerformance.Emit.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Release\AutoCSer.TestCase.TcpOpenSimpleClientPerformance.Emit.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenSimpleClientPerformance.exe.Emit"));
                        if (clientFile.Exists) Process.Start(clientFile.FullName);
                        else Console.WriteLine("未找到 TCP 应答服务性能测试服务 客户端程序");
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                    else
                    {
                        Console.WriteLine("TCP 应答服务性能测试服务 启动失败");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
