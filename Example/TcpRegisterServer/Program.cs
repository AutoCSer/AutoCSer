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
                        AutoCSer.Net.TcpRegister.ReaderServer reader = AutoCSer.Net.TcpRegister.ReaderServer.Create();
                        using (AutoCSer.Net.TcpRegister.Server.TcpInternalServer registerServer = new AutoCSer.Net.TcpRegister.Server.TcpInternalServer(null, null, reader.Server))
                        using (AutoCSer.Net.TcpRegister.ReaderServer.TcpInternalServer registerReaderServer = new AutoCSer.Net.TcpRegister.ReaderServer.TcpInternalServer(null, null, reader))
                        //using (AutoCSer.Net.TcpRegister.DefaultServer server = AutoCSer.Net.TcpRegister.DefaultServer.Create())
                        {
                            if (registerServer.IsListen && registerReaderServer.IsListen)
                            {
#if DotNetStandard
#if DEBUG
                                FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpRegisterClient\bin\Debug\netcoreapp2.0\AutoCSer.Example.TcpRegisterClient.dll".pathSeparator()));
#else
                                FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpRegisterClient\bin\Release\netcoreapp2.0\AutoCSer.Example.TcpRegisterClient.dll".pathSeparator()));
#endif
                                Console.WriteLine(clientFile.FullName);
                                if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.Example.TcpRegisterClient.dll"));
                                if (clientFile.Exists)
                                {
                                    ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                                    process.UseShellExecute = true;
                                    Process.Start(process);
                                }
#else
#if DEBUG
                                FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpRegisterClient\bin\Debug\AutoCSer.Example.TcpRegisterClient.exe".pathSeparator()));
#else
                                FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpRegisterClient\bin\Release\AutoCSer.Example.TcpRegisterClient.exe".pathSeparator()));
#endif
                                if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.Example.TcpRegisterClient.exe"));
                                if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
                                else Console.WriteLine("未找到 TCP 注册服务客户端测试程序");

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
    }
}
