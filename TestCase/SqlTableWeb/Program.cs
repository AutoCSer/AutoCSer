using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SqlTableWeb
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.SqlTableWeb", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
            Console.WriteLine("http://www.AutoCSer.com/WebView/Index.html");
            try
            {
#if DotNetStandard
#if DEBUG
                FileInfo dataServerFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\SqlTableCacheServer\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.SqlTableCacheServer.dll".pathSeparator()));
#else
                FileInfo dataServerFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\SqlTableCacheServer\bin\Release\netcoreapp2.0\AutoCSer.TestCase.SqlTableCacheServer.dll".pathSeparator()));
#endif
                ProcessStartInfo process = new ProcessStartInfo("dotnet", dataServerFile.FullName);
                process.UseShellExecute = true;
                Process.Start(process);
#else
                Process.Start(new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.SqlTableCacheServer.exe")).FullName);
#endif

                WebConfig webConfig = new WebConfig();
                using (AutoCSer.Net.HttpRegister.Server server = AutoCSer.Net.HttpRegister.Server.Create<WebServer>(webConfig.MainHostPort))
                {
                    if (server == null) Console.WriteLine("HTTP服务启动失败");
                    else
                    {
                        Console.WriteLine("HTTP服务启动成功");
                        Thread.Sleep(1000);
#if DotNetStandard
                        Console.WriteLine("http://" + webConfig.MainDomain + "/");
#else
                                Process.Start("http://" + webConfig.MainDomain + "/");
#endif
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                        return;
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
