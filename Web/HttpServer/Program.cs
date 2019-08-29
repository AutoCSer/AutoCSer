using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Web.HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;
            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetTcpRegisterAttribute(typeof(AutoCSer.Net.HttpRegister.Server)); 
            byte isStopListen = 0;
            do
            {
                try
                {
                    using (AutoCSer.Net.HttpRegister.Server serverValue = new AutoCSer.Net.HttpRegister.Server())
                    {
                        serverValue.OnLoadCacheDomain += () =>
                        {
                            if (isStopListen == 0)
                            {
                                isStopListen = 1;
                                try
                                {
                                    using (AutoCSer.Net.HttpRegister.Server.TcpInternalClient client = new AutoCSer.Net.HttpRegister.Server.TcpInternalClient(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute)))
                                    {
                                        client.stopListen(false);
                                        client.stopListen(true);
                                    }
                                }
                                catch { }
                            }
                        };
                        serverValue.OnStopListen += exit;
                        using (AutoCSer.Net.HttpRegister.Server.TcpInternalServer server = new AutoCSer.Net.HttpRegister.Server.TcpInternalServer(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute), null, serverValue))
                        {
                            if (server.IsListen)
                            {
                                Console.WriteLine("HTTP 服务启动成功 " + serverAttribute.Host + ":" + server.Port.toString());
                                AutoCSer.Diagnostics.ProcessCopyClient.Guard();
                                AutoCSer.Web.Config.Pub.ConsoleCommand();
                                AutoCSer.Diagnostics.ProcessCopyClient.Remove();
                                return;
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                }
                Thread.Sleep(1000);
            }
            while (true);
        }
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
    }
}
