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
                            if (!startProcess("TcpStreamClientPerformance", "AutoCSer.TestCase.TcpOpenStreamClientPerformance")) Console.WriteLine("未找到 TCP 服务性能测试服务 客户端程序");
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
        private static bool startProcess(string directoryName, string fileName)
        {
            fileName +=
#if DotNetStandard
 ".dll";
#else
 ".exe";
#endif
            FileInfo fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, (
#if !DOTNET45
@"..\" +
#endif

 @"..\..\..\" + directoryName + @"\bin\" +

#if DEBUG
 "Debug"
#else
 "Release"
#endif

#if DotNetStandard
 + @"\netcoreapp2.0"
#elif DOTNET2
 + @"\DotNet2"
#elif DOTNET4
 + @"\DotNet4"
#endif

 + @"\" + fileName
            ).pathSeparator()));
#if DotNetStandard
            Console.WriteLine(fileInfo.FullName);
            if (!fileInfo.Exists) fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, fileName));
            if (fileInfo.Exists)
            {
                ProcessStartInfo process = new ProcessStartInfo("dotnet", fileInfo.FullName);
                process.UseShellExecute = true;
                Process.Start(process);
                return true;
            }
#else
            if (!fileInfo.Exists) fileInfo = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, fileName));
            if (fileInfo.Exists)
            {
                Process.Start(fileInfo.FullName);
                return true;
            }
#endif
            return false;
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
