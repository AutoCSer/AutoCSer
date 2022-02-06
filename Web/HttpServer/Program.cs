using System;
using System.Threading;
using AutoCSer.Extensions;
using System.IO;

namespace AutoCSer.Web.HttpServer
{
    class Program : AutoCSer.Deploy.SwitchProcess
    {
        private Program(string[] args) : base(args) { }
        /// <summary>
        /// 退出服务进程
        /// </summary>
        private static void exit()
        {
            AutoCSer.Diagnostics.ProcessCopyClient.Remove();
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(() =>
            {
                Thread.Sleep(1000);
                Environment.Exit(-1);
            });
        }
        protected override void onStart()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => AutoCSer.Web.Config.Pub.ConsoleCommand(ExitEvent));
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(createHttpServer);
        }
        private static AutoCSer.Net.HttpRegister.Server.TcpInternalServer server;
        private static void createHttpServer()
        {
            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetTcpRegisterAttribute(typeof(AutoCSer.Net.HttpRegister.Server), false);
            if (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Name == AutoCSer.Deploy.Server.DefaultSwitchDirectoryName) serverAttribute.Port += 10000;
            do
            {
                try
                {
                    if (server != null)
                    {
                        server.Dispose();
                        server = null;
                    }
                    server = new AutoCSer.Net.HttpRegister.Server.TcpInternalServer(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute));
                    if (server.IsListen)
                    {
                        Console.WriteLine("HTTP 服务启动成功 " + serverAttribute.Host + ":" + server.Port.toString());
                        return;
                    }
                    Console.WriteLine("Search 服务启动失败 " + serverAttribute.Host + ":" + server.Port.toString());
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                }
                Thread.Sleep(1000);
            }
            while (true);
        }
        static void Main(string[] args)
        {
            //System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;
            new Program(args).Start();
        }
    }
}
