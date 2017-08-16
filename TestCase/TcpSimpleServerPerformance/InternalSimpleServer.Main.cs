using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.TestCase.TcpInternalSimpleServerPerformance
{
    static class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalSimpleServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleInterfaceServer.html
");
                using (processWait)
                using (AutoCSer.Net.TcpInternalSimpleServer.Server server = AutoCSer.Net.TcpInternalSimpleServer.Emit.Server<ISimpleServer>.Create(new InternalSimpleServer()))
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Debug\AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Release\AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.exe"));
                        if (clientFile.Exists) Process.Start(clientFile.FullName);
                        else Console.WriteLine("未找到 TCP 内部应答服务性能测试服务 客户端程序");
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                    else
                    {
                        Console.WriteLine("TCP 内部应答服务性能测试服务 启动失败");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
