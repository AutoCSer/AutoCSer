using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.CacheServerPerformance
{
    static class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.CacheServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/CacheServer/Index.html
");
                    CacheServer.MasterServerConfig fileConfig = new CacheServer.MasterServerConfig { FileName = "PerformanceTest", IsIgnoreFileEndError = true };
                    FileInfo file = new FileInfo(fileConfig.FileName + ".amc");
                    if (file.Exists) file.Delete();

                    AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.CacheServer.MasterServer), false);
                    serverAttribute.VerifyString = "!2#4%6&8QwErTyAsDfZx";
                    AutoCSer.Net.TcpInternalServer.ServerAttribute fileServerAttribute = AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute);
                    fileServerAttribute.Port -= 1;
                    using (AutoCSer.CacheServer.MasterServer.TcpInternalServer server = new AutoCSer.CacheServer.MasterServer.TcpInternalServer(serverAttribute))
                    using (AutoCSer.CacheServer.MasterServer.TcpInternalServer fileServer = new AutoCSer.CacheServer.MasterServer.TcpInternalServer(fileServerAttribute, null, new AutoCSer.CacheServer.MasterServer(fileConfig)))
                    {
                        if (server.IsListen && fileServer.IsListen)
                        {
#if DotNetStandard
#if DEBUG
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\CacheClientPerformance\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.CacheClientPerformance.dll".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\CacheClientPerformance\bin\Release\netcoreapp2.0\AutoCSer.TestCase.CacheClientPerformance.dll".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.CacheClientPerformance.dll"));
                            if (clientFile.Exists)
                            {
                                ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                                process.UseShellExecute = true;
                                Process.Start(process);
                            }
#else
#if DEBUG
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\CacheClientPerformance\bin\Debug\AutoCSer.TestCase.CacheClientPerformance.exe".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\CacheClientPerformance\bin\Release\AutoCSer.TestCase.CacheClientPerformance.exe".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.CacheClientPerformance.exe"));
                            if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
                            else Console.WriteLine("未找到缓存服务性能测试服务 客户端程序");
                            Console.WriteLine("Press quit to exit.");
                            while (Console.ReadLine() != "quit") ;
                        }
                        else
                        {
                            Console.WriteLine("缓存服务性能测试服务 启动失败");
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
