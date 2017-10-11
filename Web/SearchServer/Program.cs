using System;
using System.Threading;

namespace AutoCSer.Web.SearchServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Html.Cache[0] != null);
            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetTcpStaticRegisterAttribute(typeof(AutoCSer.Web.SearchServer.Server)); 
            do
            {
                try
                {
                    try
                    {
                        Console.WriteLine(TcpCall.Server.StopListen(AutoCSer.Web.SearchServer.Server.CurrentProcessId).Type.ToString());
                    }
                    catch(Exception error)
                    {
                        Console.WriteLine(error.ToString());
                    }
                    using (AutoCSer.Web.SearchServer.TcpStaticServer.SearchServer server = new AutoCSer.Web.SearchServer.TcpStaticServer.SearchServer(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute)))
                    {
                        if (server.IsListen)
                        {
                            AutoCSer.Web.SearchServer.Server.CurrentServer = server;
                            Console.WriteLine("Search 服务启动成功");
                            AutoCSer.Diagnostics.ProcessCopyClient.Guard();
                            AutoCSer.Web.Config.Pub.ConsoleCommand();
                            AutoCSer.Diagnostics.ProcessCopyClient.Remove();
                            return;
                        }
                        Console.WriteLine("Search 服务启动失败");
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
    }
}
