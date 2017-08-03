using System;
using System.IO;
using System.Reflection;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Web
{
    class Program
    {
        static void Main(string[] args)
        {
#if DOTNET2
            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetTcpRegisterAttribute(typeof(AutoCSer.Net.HttpRegister.Server));
            AutoCSer.Net.HostPort serverListen = new AutoCSer.Net.HostPort { Host = AutoCSer.Web.Config.Pub.ServerListenIp, Port = 80 };

            HttpDomain main = new HttpDomain
            {
                ServerType = typeof(WebServer),
                Domains = new AutoCSer.Net.HttpRegister.Domain[]
                {
                    new AutoCSer.Net.HttpRegister.Domain { Host = serverListen, DomainName = AutoCSer.Web.Config.Web.MainDomain },
                }
            };
            HttpDomain file = new HttpDomain
            {
                ServerType = typeof(FileServer),
                Domains = new AutoCSer.Net.HttpRegister.Domain[]
                {
                    new AutoCSer.Net.HttpRegister.Domain { Host = serverListen, DomainName = AutoCSer.Web.Config.Web.StaticFileDomain },
                }
            };
            HttpDomain location = new HttpDomain
            {
                ServerType = typeof(LocationServer),
                Domains = AutoCSer.Web.Config.Web.LocationDomains.getArray(domain => new AutoCSer.Net.HttpRegister.Domain { Host = serverListen, DomainName = domain })
            };
            do
            {
                try
                {
                    using (AutoCSer.Net.HttpRegister.Server.TcpInternalClient httpClient = new AutoCSer.Net.HttpRegister.Server.TcpInternalClient(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute)))
                    {
                        location.Stop(httpClient);
                        file.Stop(httpClient);
                        main.Stop(httpClient);
                        if (main.Start(httpClient) && file.Start(httpClient) && location.Start(httpClient)) return;
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                }
                Thread.Sleep(1000);
            }
            while (true);
#else
            Console.WriteLine(@"由于 www.AutoCSer.com 部署于 .NET 2.0 环境，当前项目仅用于编译 TypeScript");
            Console.ReadKey();
#endif
        }
    }
}
