using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using AutoCSer.Extension;

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
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpInternalSimpleServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleMethodServer.html
");
#if !NoAutoCSer
                    using (InternalSimpleServer.TcpInternalSimpleServer server = new InternalSimpleServer.TcpInternalSimpleServer())
                    {
                        if (server.IsListen)
                        {
                            if (!startProcess("TcpSimpleClientPerformance", "AutoCSer.TestCase.TcpInternalSimpleClientPerformance")) Console.WriteLine("未找到 TCP 内部应答服务性能测试服务 客户端程序");
                            Console.WriteLine("Press quit to exit.");
                            while (Console.ReadLine() != "quit") ;
                        }
                        else
                        {
                            Console.WriteLine("TCP 内部应答服务性能测试服务 启动失败");
                            Console.ReadKey();
                        }
                    }
#endif
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
