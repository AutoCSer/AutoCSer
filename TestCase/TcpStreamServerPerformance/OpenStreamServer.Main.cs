using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.TestCase.TcpStreamServerPerformance
{
    partial class OpenStreamServer
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenStreamServerPerformance.Emit", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceStreamServer.html
");
                    using (AutoCSer.Net.TcpOpenStreamServer.Server server = AutoCSer.Net.TcpOpenStreamServer.Emit.Server<IOpenStreamServer>.Create(new OpenStreamServer()))
                    using (AutoCSer.Net.TcpOpenStreamServer.Server tcpQueueServer = AutoCSer.Net.TcpOpenStreamServer.Emit.Server<ITcpQueueOpenStreamServer>.Create(new OpenStreamServer()))
                    using (AutoCSer.Net.TcpOpenStreamServer.Server queueServer = AutoCSer.Net.TcpOpenStreamServer.Emit.Server<IQueueOpenStreamServer>.Create(new OpenStreamServer()))
                    {
                        if (server.IsListen && tcpQueueServer.IsListen && queueServer.IsListen)
                        {
#if DotNetStandard
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpStreamClientPerformance\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit.dll".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpStreamClientPerformance\bin\Release\netcoreapp2.0\AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit.dll".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit.dll"));
                        if (clientFile.Exists)
                        {
                            ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                            process.UseShellExecute = true;
                            Process.Start(process);
                        }
#else
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpStreamClientPerformance\bin\Debug\AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit.exe".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpStreamClientPerformance\bin\Release\AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit.exe".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit.exe"));
                            if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
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
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
