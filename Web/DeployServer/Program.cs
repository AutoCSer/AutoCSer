using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.Web.DeployServer
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo switchFile = AutoCSer.Deploy.Server.GetSwitchFile();
            if (switchFile != null)
            {
                switchFile.StartProcessDirectory();
                return;
            }
            EventWaitHandle processWait = AutoCSer.Deploy.Server.TryCreateProcessEventWaitHandle();
            if (processWait != null)
            {
                using (processWait)
                {
                    try
                    {
                        AutoCSer.Threading.ThreadPool.TinyBackground.Start(() =>
                        {
                            AutoCSer.Web.Config.Pub.ConsoleCommand(() => exitEvent.Set());
                        });

                        AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Deploy.Server));
                        serverAttribute.Host = AutoCSer.Web.Config.Pub.ServerListenIp;
                        serverAttribute.IsServer = true;
                        AutoCSer.Deploy.Server serverTarget = new AutoCSer.Deploy.Server();
                        serverTarget.BeforeSwitch += () => exitEvent.Set();
                        serverTarget.SetCustomTask(new ServerCustomTask());
                        using (AutoCSer.Deploy.Server.TcpInternalServer server = new AutoCSer.Deploy.Server.TcpInternalServer(serverAttribute, null, serverTarget))
                        {
                            if (server.IsListen)
                            {
                                Console.WriteLine("部署服务 启动成功 " + serverAttribute.Host + ":" + serverAttribute.Port.toString());
                                AutoCSer.Diagnostics.ProcessCopyClient.Guard();
                                exitEvent.WaitOne();
                            }
                            else Console.WriteLine("部署服务 启动失败 " + serverAttribute.Host + ":" + serverAttribute.Port.toString());
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                }
            }
            AutoCSer.Diagnostics.ProcessCopyClient.Remove();
            Thread.Sleep(1000);
        }
        /// <summary>
        /// 退出服务锁
        /// </summary>
        private static readonly ManualResetEvent exitEvent = new ManualResetEvent(false);
    }
}
