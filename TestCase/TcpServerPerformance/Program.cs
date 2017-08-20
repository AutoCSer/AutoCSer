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
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
#if NoAutoCSer
#else
                    using (InternalServer.TcpInternalServer server = new InternalServer.TcpInternalServer())
                    {
                        if (server.IsListen)
                        {
#if DotNetStandard
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpClientPerformance\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.TcpInternalClientPerformance.dll".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpClientPerformance\bin\Release\netcoreapp2.0\AutoCSer.TestCase.TcpInternalClientPerformance.dll".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalClientPerformance.dll"));
                        if (clientFile.Exists)
                        {
                            ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                            process.UseShellExecute = true;
                            Process.Start(process);
                        }
#else
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Debug\AutoCSer.TestCase.TcpInternalClientPerformance.exe".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpClientPerformance\bin\Release\AutoCSer.TestCase.TcpInternalClientPerformance.exe".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalClientPerformance.exe"));
                            if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
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
#if DotNetStandard
#else
                }
            }
#endif
        }
    }
}
