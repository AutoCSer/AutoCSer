using System;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace AutoCSer.TestCase.SqlTableWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.SqlTableWeb", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    Console.WriteLine("http://www.AutoCSer.com/WebView/Index.html");
                    try
                    {
                        Process.Start(new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.SqlTableCacheServer.exe")).FullName);
                        WebConfig webConfig = new WebConfig();
                        using (AutoCSer.Net.HttpRegister.Server server = AutoCSer.Net.HttpRegister.Server.Create<WebServer>(webConfig.MainHostPort))
                        {
                            if (server == null) Console.WriteLine("HTTP服务启动失败");
                            else
                            {
                                Console.WriteLine("HTTP服务启动成功");
                                Thread.Sleep(1000);
                                Process.Start("http://" + webConfig.MainDomain + "/");

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
                }
            }
        }
    }
}
