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
                            if (!startProcess("TcpStreamClientPerformance", "AutoCSer.TestCase.TcpOpenStreamClientPerformance.Emit")) Console.WriteLine("未找到 TCP 服务性能测试服务 客户端程序");
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
        private static bool startProcess(string directoryName, string fileName)
        {
            fileName +=
#if DotNetStandard
 ".dll";
#else
 ".exe";
#endif
            FileInfo fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, (
#if !DOTNET45
@"..\" +
#endif

 @"..\..\..\" + directoryName + @"\bin\" +

#if DEBUG
 "Debug"
#else
 "Release"
#endif

#if DotNetStandard
 + @"\netcoreapp2.0"
#elif DOTNET2
 + @"\DotNet2"
#elif DOTNET4
 + @"\DotNet4"
#endif

 + @"\" + fileName
            ).pathSeparator()));
#if DotNetStandard
            Console.WriteLine(fileInfo.FullName);
            if (!fileInfo.Exists) fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, fileName));
            if (fileInfo.Exists)
            {
                ProcessStartInfo process = new ProcessStartInfo("dotnet", fileInfo.FullName);
                process.UseShellExecute = true;
                Process.Start(process);
                return true;
            }
#else
            if (!fileInfo.Exists) fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, fileName));
            if (fileInfo.Exists)
            {
                Process.Start(fileInfo.FullName);
                return true;
            }
#endif
            return false;
        }
    }
}
