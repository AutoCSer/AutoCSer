using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
    static class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalStreamServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceStreamServer.html
");
                    using (AutoCSer.Net.TcpInternalStreamServer.Server server = AutoCSer.Net.TcpInternalStreamServer.Emit.Server<IStreamServer>.Create(new InternalStreamServer()))
                    using (AutoCSer.Net.TcpInternalStreamServer.Server tcpQueueServer = AutoCSer.Net.TcpInternalStreamServer.Emit.Server<ITcpQueueStreamServer>.Create(new InternalStreamServer()))
                    using (AutoCSer.Net.TcpInternalStreamServer.Server queueServer = AutoCSer.Net.TcpInternalStreamServer.Emit.Server<IQueueStreamServer>.Create(new InternalStreamServer()))
                    {
                        if (server.IsListen && tcpQueueServer.IsListen && queueServer.IsListen)
                        {
#if DotNetStandard
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpStreamClientPerformance\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit.dll".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpStreamClientPerformance\bin\Release\netcoreapp2.0\AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit.dll".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit.dll"));
                        if (clientFile.Exists)
                        {
                            ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                            process.UseShellExecute = true;
                            Process.Start(process);
                        }
#else
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpStreamClientPerformance\bin\Debug\AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit.exe".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpStreamClientPerformance\bin\Release\AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit.exe".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpInternalStreamClientPerformance.Emit.exe"));
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
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
