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
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalSimpleServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleInterfaceServer.html
");
                    using (AutoCSer.Net.TcpInternalSimpleServer.Server server = AutoCSer.Net.TcpInternalSimpleServer.Emit.Server<ISimpleServer>.Create(new InternalSimpleServer()))
                    {
                        if (server.IsListen)
                        {
#if DotNetStandard
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpSimpleClientPerformance\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.dll".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpSimpleClientPerformance\bin\Release\netcoreapp2.0\AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.dll".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.dll"));
                        if (clientFile.Exists)
                        {
                            ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                            process.UseShellExecute = true;
                            Process.Start(process);
                        }
#else
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Debug\AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.exe".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Release\AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.exe".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalSimpleClientPerformance.Emit.exe"));
                            if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
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
#if DotNetStandard
#else
                }
            }
#endif
        }
    }
}
