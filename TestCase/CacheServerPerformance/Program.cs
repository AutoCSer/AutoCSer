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
                    DirectoryInfo messageQueueDirectory = new DirectoryInfo("MessageQueue");
                    if (messageQueueDirectory.Exists)
                    {
                        foreach (DirectoryInfo directory in messageQueueDirectory.GetDirectories()) directory.Delete(true);
                    }
                    CacheServer.MasterServerConfig fileConfig = new CacheServer.MasterServerConfig { FileName = "PerformanceTest", IsIgnoreFileEndError = true };
                    deleteFile(fileConfig.FileName + ".amc");
                    deleteFile(fileConfig.FileName + ".amcs");

                    AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.CacheServer.MasterServer), false);
                    serverAttribute.VerifyString = "!2#4%6&8QwErTyAsDfZx";
                    AutoCSer.Net.TcpInternalServer.ServerAttribute fileServerAttribute = AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute);
                    fileServerAttribute.Port -= 1;
                    using (AutoCSer.CacheServer.MasterServer.TcpInternalServer server = new AutoCSer.CacheServer.MasterServer.TcpInternalServer(serverAttribute))
                    using (AutoCSer.CacheServer.MasterServer.TcpInternalServer fileServer = new AutoCSer.CacheServer.MasterServer.TcpInternalServer(fileServerAttribute, null, new AutoCSer.CacheServer.MasterServer(fileConfig)))
                    {
                        if (server.IsListen && fileServer.IsListen)
                        {
                            if (!startProcess("CacheClientPerformance", "AutoCSer.TestCase.CacheClientPerformance")) Console.WriteLine("未找到缓存服务性能测试服务 客户端程序");
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
        /// <summary>
        /// 删除历史文件
        /// </summary>
        /// <param name="fileName"></param>
        private static void deleteFile(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            if (file.Exists) file.Delete();
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
