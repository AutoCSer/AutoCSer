using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SqlTableCacheServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.SqlTableCacheServer", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
#if !NoAutoCSer
                    Console.WriteLine("http://www.AutoCSer.com/OrmCache/Index.html");
                    bool isServer = false;
                    try
                    {
                        using (AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer.DataReader dataReaderServer = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer.DataReader())
                        using (AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer.DataLog dataLogServer = new AutoCSer.TestCase.SqlTableCacheServer.TcpStaticServer.DataLog())
                        {
                            if (dataReaderServer.IsListen && dataLogServer.IsListen)
                            {
                                isServer = true;
                                Console.WriteLine("数据服务启动成功，正在初始化数据处理 ...");
                                Console.WriteLine("Student " + Student.Loader.Count.toString());
                                Console.WriteLine("Class " + Class.Loader.Cache.Count.toString());
                                Console.WriteLine("Press quit to exit.");
                                while (Console.ReadLine() != "quit") ;
                                return;
                            }
                            Console.WriteLine("数据服务启动失败");
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.ToString());
                        if (isServer) Console.WriteLine("数据库连接失败，请检测 Config.cs 连接字符串等配置是否正确。");
                    }
                    Console.ReadKey();
#endif
#if !DotNetStandard
                }
            }
#endif
        }
    }
}
