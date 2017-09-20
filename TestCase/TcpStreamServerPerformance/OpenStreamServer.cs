using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AutoCSer.TestCase.TcpServerPerformance;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenStreamServerPerformance
{
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12111, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Synchronous, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
    public partial class OpenStreamServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if DOTNET2 || DOTNET4
        [AutoCSer.Net.TcpOpenStreamServer.Method]
#else
        [AutoCSer.Net.TcpOpenStreamServer.Method(IsClientTaskAsync = true)]
#endif
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected int add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 简单计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenStreamServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected Add addAsynchronous(int left, int right)
        {
            return new Add(left, right);
        }

        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenStreamServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodStreamServer.html
");
#if !NoAutoCSer
                    using (OpenStreamServer.TcpOpenStreamServer synchronousServer = new OpenStreamServer.TcpOpenStreamServer())
                    using (OpenStreamTcpQueueServer.TcpOpenStreamServer tcpQueueServer = new OpenStreamTcpQueueServer.TcpOpenStreamServer())
                    using (OpenStreamQueueServer.TcpOpenStreamServer queueServer = new OpenStreamQueueServer.TcpOpenStreamServer())
                    {
                        if (synchronousServer.IsListen && tcpQueueServer.IsListen&& queueServer.IsListen)
                        {
#if DotNetStandard
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpStreamClientPerformance\bin\Debug\netcoreapp2.0\AutoCSer.TestCase.TcpOpenStreamClientPerformance.dll".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\..\TcpStreamClientPerformance\bin\Release\netcoreapp2.0\AutoCSer.TestCase.TcpOpenStreamClientPerformance.dll".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenStreamClientPerformance.dll"));
                        if (clientFile.Exists)
                        {
                            ProcessStartInfo process = new ProcessStartInfo("dotnet", clientFile.FullName);
                            process.UseShellExecute = true;
                            Process.Start(process);
                        }
#else
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpStreamClientPerformance\bin\Debug\AutoCSer.TestCase.TcpOpenStreamClientPerformance.exe".pathSeparator()));
#else
                            FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpStreamClientPerformance\bin\Release\AutoCSer.TestCase.TcpOpenStreamClientPerformance.exe".pathSeparator()));
#endif
                            if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenStreamClientPerformance.exe"));
                            if (clientFile.Exists) Process.Start(clientFile.FullName);
#endif
                            else Console.WriteLine("未找到 TCP 服务性能测试服务 客户端程序");
                            Console.WriteLine("Press quit to exit.");
                            while (Console.ReadLine() != "quit") ;
                        }
                        else
                        {
                            Console.WriteLine("TCP 服务性能测试服务 启动失败");
                            Console.ReadKey();
                        }
                    }
#endif
#if !DotNetStandard
                }
            }
#endif
        }
    }
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12112, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.TcpQueue, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
    public partial class OpenStreamTcpQueueServer : OpenStreamServer
    {
    }
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12113, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Queue, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
    public partial class OpenStreamQueueServer : OpenStreamServer
    {
    }
}
