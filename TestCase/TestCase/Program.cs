using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using AutoCSer.Extension;

namespace AutoCSer.TestCase
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
                    Console.WriteLine(@"http://www.AutoCSer.com/
");

                    Type errorType = typeof(Program);
#if NoAutoCSer
#else
                    FileInfo blockFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, "blockFile" + AutoCSer.DiskBlock.File.ExtensionName));
                    if (blockFile.Exists && blockFile.Length >= 1 << 20) blockFile.Delete();
                    using (AutoCSer.DiskBlock.Server.TcpInternalServer fileBlockServer = AutoCSer.DiskBlock.Server.Create(new AutoCSer.DiskBlock.File(blockFile.FullName, 0)))
                    using (AutoCSer.TestCase.TcpStaticServer.JsonServer jsonServer = new AutoCSer.TestCase.TcpStaticServer.JsonServer())
                    using (AutoCSer.TestCase.TcpStaticServer.MemberServer memberServer = new AutoCSer.TestCase.TcpStaticServer.MemberServer())
                    using (AutoCSer.TestCase.TcpStaticServer.SessionServer sessionServer = new AutoCSer.TestCase.TcpStaticServer.SessionServer())
                    using (AutoCSer.TestCase.TcpStaticSimpleServer.SimpleJsonServer jsonSimpleServer = new AutoCSer.TestCase.TcpStaticSimpleServer.SimpleJsonServer())
                    using (AutoCSer.TestCase.TcpStaticSimpleServer.SimpleMemberServer memberSimpleServer = new AutoCSer.TestCase.TcpStaticSimpleServer.SimpleMemberServer())
                    using (AutoCSer.TestCase.TcpStaticSimpleServer.SimpleSessionServer sessionSimpleServer = new AutoCSer.TestCase.TcpStaticSimpleServer.SimpleSessionServer())
                    {
                        if (fileBlockServer.IsListen && jsonServer.IsListen && memberServer.IsListen && sessionServer.IsListen && jsonSimpleServer.IsListen && memberSimpleServer.IsListen && sessionSimpleServer.IsListen)
                        {
#endif
                            do
                            {
                                if (!Json.TestCase()) { errorType = typeof(Json); break; }
                                if (!Xml.TestCase()) { errorType = typeof(Xml); break; }
                                if (!BinarySerialize.TestCase()) { errorType = typeof(BinarySerialize); break; }
                                if (!SimpleSerialize.TestCase()) { errorType = typeof(SimpleSerialize); break; }
#if NOJIT
#else
                                if (!TcpInternalServer.Emit.Server.TestCase()) { errorType = typeof(TcpInternalServer.Emit.Server); break; }
                                if (!TcpOpenServer.Emit.Server.TestCase()) { errorType = typeof(TcpOpenServer.Emit.Server); break; }
                                if (!TcpInternalSimpleServer.Emit.Server.TestCase()) { errorType = typeof(TcpInternalSimpleServer.Emit.Server); break; }
                                if (!TcpOpenSimpleServer.Emit.Server.TestCase()) { errorType = typeof(TcpOpenSimpleServer.Emit.Server); break; }
#endif
#if NoAutoCSer
#else
                                if (!TcpInternalServer.Session.TestCase()) { errorType = typeof(TcpInternalServer.Session); break; }
                                if (!TcpInternalServer.Member.TestCase()) { errorType = typeof(TcpInternalServer.Member); break; }
                                if (!TcpInternalServer.Json.TestCase()) { errorType = typeof(TcpInternalServer.Json); break; }
                                if (!TcpStaticServer.Session.TestClient()) { errorType = typeof(TcpStaticServer.Session); break; }
                                if (!TcpStaticServer.Member.TestClient()) { errorType = typeof(TcpStaticServer.Member); break; }
                                if (!TcpStaticServer.Json.TestClient()) { errorType = typeof(TcpStaticServer.Json); break; }
                                if (!TcpOpenServer.Session.TestCase()) { errorType = typeof(TcpOpenServer.Session); break; }
                                if (!TcpOpenServer.Member.TestCase()) { errorType = typeof(TcpOpenServer.Member); break; }
                                if (!TcpOpenServer.Json.TestCase()) { errorType = typeof(TcpOpenServer.Json); break; }
                                if (!TcpInternalSimpleServer.Session.TestCase()) { errorType = typeof(TcpInternalSimpleServer.Session); break; }
                                if (!TcpInternalSimpleServer.Member.TestCase()) { errorType = typeof(TcpInternalSimpleServer.Member); break; }
                                if (!TcpInternalSimpleServer.Json.TestCase()) { errorType = typeof(TcpInternalSimpleServer.Json); break; }
                                if (!TcpStaticSimpleServer.Session.TestClient()) { errorType = typeof(TcpStaticSimpleServer.Session); break; }
                                if (!TcpStaticSimpleServer.Member.TestClient()) { errorType = typeof(TcpStaticSimpleServer.Member); break; }
                                if (!TcpStaticSimpleServer.Json.TestClient()) { errorType = typeof(TcpStaticSimpleServer.Json); break; }
                                if (!TcpOpenSimpleServer.Session.TestCase()) { errorType = typeof(TcpOpenSimpleServer.Session); break; }
                                if (!TcpOpenSimpleServer.Member.TestCase()) { errorType = typeof(TcpOpenSimpleServer.Member); break; }
                                if (!TcpOpenSimpleServer.Json.TestCase()) { errorType = typeof(TcpOpenSimpleServer.Json); break; }

                                if (!DiskBlock.File.TestCase()) { errorType = typeof(DiskBlock.File); break; }
#endif
                                Console.Write('.');
                                if (testCount++ == 1) AutoCSer.Threading.ThreadPool.TinyBackground.Start(check);
                            }
                            while (true);
#if NoAutoCSer
#else
                        }
                    }
#endif
                    testCount = 0;
                    Console.WriteLine(errorType.FullName + " ERROR");
                    Console.ReadKey();
                }
            }
        }
        private static int testCount = 1;
        private static void check()
        {
            do
            {
                int count = testCount;
                System.Threading.Thread.Sleep(5000);
                if (count == testCount)
                {
                    Console.WriteLine("STOP");
                }
            }
            while (testCount != 0);
        }
    }
}
