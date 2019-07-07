using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AutoCSer.TestCase.TcpServerPerformance;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenServerPerformance
{
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = 12101, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
    public partial class OpenServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if DOTNET2 || DOTNET4
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
#else
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientTaskAsync = true)]
#endif
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 异步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addAsynchronous(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd)
        {
            onAdd(new Add(left, right));
        }

        /// <summary>
        /// 简单计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addSynchronous(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算队列测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addQueue(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpTask, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addTcpTask(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addTimeoutTask(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addThreadPool(int left, int right)
        {
            return new Add(left, right);
        }

        /// <summary>
        /// GC 垃圾回收
        /// </summary>
        [AutoCSer.Net.TcpOpenServer.Method(IsClientAwaiter = false)]
        private void gcCollect()
        {
            GC.Collect();
        }

        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/MethodServer.html
");
#if !NoAutoCSer
                    using (OpenServer.TcpOpenServer server = new OpenServer.TcpOpenServer())
                    {
                        if (server.IsListen)
                        {
                            if (!startProcess("TcpClientPerformance", "AutoCSer.TestCase.TcpOpenClientPerformance")) Console.WriteLine("未找到 TCP 服务性能测试服务 客户端程序");
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
}
