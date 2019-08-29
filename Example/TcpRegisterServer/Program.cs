using System;
using System.Threading;
using AutoCSer.Extension;
using System.IO;
using System.Diagnostics;

namespace AutoCSer.Example.TcpRegisterServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Example.TcpRegisterServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/Register.html
");
                    try
                    {
                        using (AutoCSer.Net.TcpRegister.Server.TcpInternalServer registerServer = new AutoCSer.Net.TcpRegister.Server.TcpInternalServer())//null, null, reader.Server
                        {
                            if (registerServer.IsListen)
                            {
                                if (!startProcess("TcpRegisterClient", "AutoCSer.Example.TcpRegisterClient")) Console.WriteLine("未找到 TCP 注册服务客户端测试程序");

                                Console.WriteLine("Press quit to exit.");
                                while (Console.ReadLine() != "quit") ;
                            }
                        }
                        Console.WriteLine("TCP 注册服务启动失败");
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
