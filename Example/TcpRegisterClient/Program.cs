using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Example.TcpRegisterClient
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Example.TcpRegisterClient", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/Register.html
");
                    try
                    {
                        AutoCSer.Threading.ThreadPool.TinyBackground.Start(clientThread);
                        AutoCSer.Net.TcpInternalServer.Server server = null;
                        RegisterClientTestServer test = new RegisterClientTestServer();
                        do
                        {
                            AutoCSer.Example.TcpRegisterClient.RegisterClientTestServer.TcpInternalServer newServer = new RegisterClientTestServer.TcpInternalServer(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute), null, test);
                            if (newServer.IsListen)
                            {
                                if (server != null) server.Dispose();
                                server = newServer;
                                Console.WriteLine(@"
Version:" + test.Version.toString());
                            }
                            else Console.WriteLine(@"
测试服务启动失败");
                            Thread.Sleep(5000);
                            if (test.Version == 9)
                            {
                                Console.WriteLine(@"
测试结束");
                                isEnd = true;
                                if (server != null) server.Dispose();
                                Console.ReadKey();
                                return;
                            }
                            ++test.Version;
                        }
                        while (true);
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
        /// <summary>
        /// 客户端测试线程
        /// </summary>
        private static void clientThread()
        {
            using (AutoCSer.Example.TcpRegisterClient.RegisterClientTestServer.TcpInternalClient client = new RegisterClientTestServer.TcpInternalClient(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute)))
            {
                while (!isEnd)
                {
                    AutoCSer.Net.TcpServer.ReturnValue<int> value = client.get();
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) Console.Write(value.Value.toString());
                    else Console.WriteLine(value.Type.ToString());
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// 测试是否结束
        /// </summary>
        private static bool isEnd;
        /// <summary>
        /// 测试 TCP 服务配置
        /// </summary>
        private static readonly AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute;
        static Program()
        {
            serverAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(RegisterClientTestServer), false);
            serverAttribute.TcpRegister = Config.TcpRegisterConfigName;
        }
    }
}
