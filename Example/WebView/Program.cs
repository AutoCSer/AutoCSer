using System;
using System.Diagnostics;
using System.Threading;

namespace AutoCSer.Example.WebView
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.WebView", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/WebView/Index.html
");
                    try
                    {
                        WebConfig webConfig = new WebConfig();
                        using (AutoCSer.Net.HttpRegister.Server server = AutoCSer.Net.HttpRegister.Server.Create<WebServer>(webConfig.MainHostPort))
                        {
                            if (server == null) Console.WriteLine("HTTP服务启动失败");
                            else
                            {
                                System.Net.ServicePointManager.Expect100Continue = false;
                                Console.WriteLine(Ajax.Post.TestCase());
                                Console.WriteLine(Ajax.Get.TestCase());
                                Console.WriteLine(Ajax.RefOut.TestCase());
                                Console.WriteLine(Ajax.Asynchronous.TestCase());
                                Console.WriteLine(Ajax.Name.TestCase());
                                Console.WriteLine(Ajax.BoxSerialize.TestCase());
                                Console.WriteLine(Call.TestCase());
                                Console.WriteLine(CallAsynchronous.TestCase());
                                Console.WriteLine(CallBoxSerialize.TestCase());
                                Console.WriteLine(CallName.TestCase());
                                Console.WriteLine(Location.TestCase());
                                Console.WriteLine(Upload.TestCase());
                                Console.WriteLine(File.TestCase());

#if DotNetStandard
                                Console.WriteLine("http://" + webConfig.MainDomain + "/");
#else
                                Process.Start("http://" + webConfig.MainDomain + "/");
#endif
                                Console.WriteLine("press quit to exit.");
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
