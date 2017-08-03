using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
            string userName;
            if (args.length() == 0)
            {
                Console.WriteLine("请输出用户名称");
                userName = Console.ReadLine();
            }
            else userName = args[0];
            using (Client client = new Client(userName))
            {
                while (true)
                {
                    Console.WriteLine("Press quit to exit.");
                    string message = Console.ReadLine();
                    if (message == "quit") return;
                    if (client.IsLogin && message.Length != 0) client.Send(message);
                }
            }
        }
    }
}
