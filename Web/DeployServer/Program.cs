using System;
using System.Threading;

namespace AutoCSer.Web.DeployServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Web.DeployServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Deploy.Server)); 
                        serverAttribute.Host = AutoCSer.Web.Config.Pub.ServerListenIp;
                        serverAttribute.IsServer = true;
                        using (AutoCSer.Deploy.Server.TcpInternalServer server = new AutoCSer.Deploy.Server.TcpInternalServer(serverAttribute))
                        {
                            if (server.IsListen)
                            {
                                Console.WriteLine("部署服务 启动成功");
                                AutoCSer.Diagnostics.ProcessCopyClient.Guard();
                                AutoCSer.Web.Config.Pub.ConsoleCommand();
                                AutoCSer.Diagnostics.ProcessCopyClient.Remove();
                                return;
                            }
                            Console.WriteLine("部署服务 启动失败");
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
