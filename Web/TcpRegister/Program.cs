using System;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace AutoCSer.Web.TcpRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Web.TcpRegister", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ServerAttribute readerServerAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.ReaderServer));
                        AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.Server)); 
                        AutoCSer.Net.TcpRegister.ReaderServer reader = AutoCSer.Net.TcpRegister.ReaderServer.Create();
                        using (AutoCSer.Net.TcpRegister.Server.TcpInternalServer registerServer = new AutoCSer.Net.TcpRegister.Server.TcpInternalServer(serverAttribute, null, reader.Server))
                        using (AutoCSer.Net.TcpRegister.ReaderServer.TcpInternalServer registerReaderServer = new AutoCSer.Net.TcpRegister.ReaderServer.TcpInternalServer(readerServerAttribute, null, reader))
                        {
                            if (registerServer.IsListen && registerReaderServer.IsListen)
                            {
                                Console.WriteLine("TCP 注册服务启动成功");
                                AutoCSer.Threading.ThreadPool.TinyBackground.Start(processCopy);

                                FileInfo httpServer = new FileInfo(@"..\..\..\HttpServer\bin\Release\AutoCSer.Web.HttpServer.exe");
                                if (httpServer.Exists)
                                {
                                    ProcessStartInfo start = new ProcessStartInfo(httpServer.FullName);
                                    start.WorkingDirectory = httpServer.Directory.FullName;
                                    Process.Start(start);
                                }

                                FileInfo searchServer = new FileInfo(@"..\..\..\SearchServer\bin\Release\AutoCSer.Web.SearchServer.exe");
                                if (searchServer.Exists)
                                {
                                    ProcessStartInfo start = new ProcessStartInfo(searchServer.FullName);
                                    start.WorkingDirectory = searchServer.Directory.FullName;
                                    Process.Start(start);
                                }

                                FileInfo deployServer = new FileInfo(@"..\..\..\DeployServer\bin\Release\AutoCSer.Web.DeployServer.exe");
                                if (deployServer.Exists)
                                {
                                    ProcessStartInfo start = new ProcessStartInfo(deployServer.FullName);
                                    start.WorkingDirectory = deployServer.Directory.FullName;
                                    Process.Start(start);
                                }

                                AutoCSer.Web.Config.Pub.ConsoleCommand();
                                isExit = true;
                                if (processCopyServer != null) processCopyServer.Dispose();
                                return;
                            }
                        }
                        Console.WriteLine("TCP 注册服务启动失败");
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                    Console.ReadKey();
                }
            }
        }
        /// <summary>
        /// 是否退出进程
        /// </summary>
        private static bool isExit;
        /// <summary>
        /// 进程复制重启服务
        /// </summary>
        private static AutoCSer.Diagnostics.ProcessCopyServer.TcpInternalServer processCopyServer;
        /// <summary>
        /// 启动进程复制重启服务
        /// </summary>
        private static void processCopy()
        {
            do
            {
                try
                {
                    AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.Diagnostics.ProcessCopyServer), false);
                    serverAttribute.VerifyString = AutoCSer.Web.Config.Pub.TcpVerifyString;
                    AutoCSer.Diagnostics.ProcessCopyServer.TcpInternalServer server = new AutoCSer.Diagnostics.ProcessCopyServer.TcpInternalServer(serverAttribute);
                    if(server.IsListen)
                    {
                        Console.WriteLine("进程复制重启服务启动成功");
                        processCopyServer = server;
                        return;
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                }
                Thread.Sleep(1000);
            }
            while (!isExit);
        }
    }
}
