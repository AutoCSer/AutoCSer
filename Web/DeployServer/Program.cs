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
                switchFile.StartProcessDirectory(args.Length == 0 ? null : args[0]);
                return;
            }
            if (args.Length == 0)
            {
                if ((processWait = AutoCSer.Deploy.Server.TryCreateProcessEventWaitHandle()) != null)
                {
                    using (processWait)
                    {
                        try
                        {
                            AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => AutoCSer.Web.Config.Pub.ConsoleCommand(switchEvent));
                            AutoCSer.Threading.ThreadPool.TinyBackground.Start(processWaitSet);
                            createDeployServer();
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error.ToString());
                        }
                        switchEvent.WaitOne();
                        processWait.Set();
                    }
                    exitEvent.Set();
                }
                //else Console.WriteLine("其它部署服务没有退出");
            }
            else
            {
                AutoCSer.Deploy.Server.SetProcessEventWaitHandle();
            }
            Console.WriteLine("正在退出...");
            Thread.Sleep(2000);
        }
        /// <summary>
        /// 等待进程关闭信号
        /// </summary>
        private static void processWaitSet()
        {
            processWait.WaitOne();
            switchEvent.Set();
        }
        /// <summary>
        /// 进程排他锁
        /// </summary>
        private static EventWaitHandle processWait;
        /// <summary>
        /// 切换服务锁
        /// </summary>
        private static readonly ManualResetEvent exitEvent = new ManualResetEvent(false);
        /// <summary>
        /// 切换服务锁
        /// </summary>
        private static readonly ManualResetEvent switchEvent = new ManualResetEvent(false);
        /// <summary>
        /// 部署服务
        /// </summary>
        private static void createDeployServer()
        {
            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Deploy.Server));
            serverAttribute.Host = AutoCSer.Web.Config.Pub.ServerListenIp;
            serverAttribute.IsServer = true;
            AutoCSer.Deploy.Server serverTarget = new AutoCSer.Deploy.Server();
            serverTarget.BeforeSwitch += () =>
            {
                switchEvent.Set();
                exitEvent.WaitOne();
            };
            serverTarget.SetCustomTask(new ServerCustomTask());
            using (AutoCSer.Deploy.Server.TcpInternalServer server = new AutoCSer.Deploy.Server.TcpInternalServer(serverAttribute, null, serverTarget))
            {
                if (server.IsListen)
                {
                    Console.WriteLine("部署服务 启动成功 " + serverAttribute.Host + ":" + serverAttribute.Port.toString());
                    switchEvent.WaitOne();
                }
                else Console.WriteLine("部署服务 启动失败 " + serverAttribute.Host + ":" + serverAttribute.Port.toString());
            }
        }
    }
}
