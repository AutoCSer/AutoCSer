using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.ChatServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
#if !NoAutoCSer
                    using (Server.TcpOpenServer server = new Server.TcpOpenServer())
                    {
                        if (server.IsListen)
                        {
                            if (!startProcess("ChatClient", "AutoCSer.TestCase.ChatClient")) Console.WriteLine("未找到 群聊 客户端程序");
                            Console.WriteLine("Press quit to exit.");
                            while (Console.ReadLine() != "quit") ;
                        }
                        else
                        {
                            Console.WriteLine("群聊服务 启动失败");
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
                ProcessStartInfo process = new ProcessStartInfo("dotnet", fileInfo.FullName + " user1");
                process.UseShellExecute = true;
                Process.Start(process);
                process.Arguments = fileInfo.FullName + " user2";
                Process.Start(process);
                process.Arguments = fileInfo.FullName + " user3";
                Process.Start(process);
                process.Arguments = fileInfo.FullName + " user4";
                Process.Start(process);
                return true;
            }
#else
            if (!fileInfo.Exists) fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, fileName));
            if (fileInfo.Exists)
            {
                Process.Start(fileInfo.FullName, "user1");
                Process.Start(fileInfo.FullName, "user2");
                Process.Start(fileInfo.FullName, "user3");
                Process.Start(fileInfo.FullName, "user4");
                return true;
            }
#endif
            return false;
        }
    }
}
