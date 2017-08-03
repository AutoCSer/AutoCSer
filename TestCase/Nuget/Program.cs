using System;
using System.Threading;

namespace AutoCSer.TestCase.Nuget
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.Nuget", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/InterfaceServer.html
");

                using (processWait)
                using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IServer>.Create(new Server()))
                {
                    if (server.IsListen)
                    {

                        IServer client = AutoCSer.Net.TcpInternalServer.Emit.Client<IServer>.Create();
                        using (client as IDisposable)
                        {
                            Console.WriteLine(client.Add(2, 3) == 2 + 3);
                        }
                    }
                }
                Console.ReadKey();
            }
        }
    }
}
