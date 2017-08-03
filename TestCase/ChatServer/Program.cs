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
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.ChatServer", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
#if NoAutoCSer
#else
                using (processWait)
                using (Server.TcpOpenServer server = new Server.TcpOpenServer())
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\ChatClient\bin\Debug\AutoCSer.TestCase.ChatClient.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\ChatClient\bin\Release\AutoCSer.TestCase.ChatClient.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.ChatClient.exe"));
                        if (clientFile.Exists)
                        {
                            Process.Start(clientFile.FullName, "user1");
                            Process.Start(clientFile.FullName, "user2");
                            Process.Start(clientFile.FullName, "user3");
                            Process.Start(clientFile.FullName, "user4");
                        }
                        else Console.WriteLine("未找到 群聊 客户端程序");
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
            }
        }
    }
}
