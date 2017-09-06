using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace AutoCSer.TestCase.HttpFilePerformance
{
    /// <summary>
    /// 文件服务测试
    /// </summary>
    internal static class HttpFileServer
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.HttpFilePerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/WebView/Index.html
");
                    try
                    {
                        using (AutoCSer.Net.HttpRegister.Server server = AutoCSer.Net.HttpRegister.Server.Create<WebServer>("127.0.0.1", 12200))
                        {
                            if (server == null) Console.WriteLine("HTTP服务启动失败");
                            else
                            {
#if DotNetStandard
                                FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.HttpFilePerformanceClient.dll"));
                                if (clientFile.Exists)
                                {
                                    ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                                    process.UseShellExecute = true;
                                    Process.Start(process);
                                }
#else
                                FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.HttpFilePerformanceClient.exe"));
                                if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
                                else Console.WriteLine("未找到 HTTP 文件测试客户端程序");

                                Console.WriteLine("Press quit to exit.");
                                while (Console.ReadLine() != "quit") ;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                    Console.ReadKey();
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
