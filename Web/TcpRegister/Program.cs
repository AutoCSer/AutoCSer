using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AutoCSer.Extension;

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
                        AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.Server)); 
                        using (AutoCSer.Net.TcpRegister.Server.TcpInternalServer registerServer = new AutoCSer.Net.TcpRegister.Server.TcpInternalServer(serverAttribute))
                        {
                            if (registerServer.IsListen)
                            {
                                Console.WriteLine("TCP 注册服务启动成功 " + serverAttribute.Host + ":" + registerServer.Port.toString());
                                AutoCSer.Threading.ThreadPool.TinyBackground.Start(processCopy);

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
                        Console.WriteLine("进程复制重启服务启动成功 " + serverAttribute.Host + ":" + server.Port.toString());
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
