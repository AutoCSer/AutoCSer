using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpOpenSimpleServerPerformance
{
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = 12105, SendBufferSize = SubBuffer.Size.Byte256, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsJsonSerialize = true)]
    public partial class OpenSimpleServer
    {
        /// <summary>
        /// 异步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        [AutoCSer.Net.TcpOpenSimpleServer.Method]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addAsynchronous(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd)
        {
            onAdd(left + right);
        }

        /// <summary>
        /// 简单计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int addSynchronous(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 计算队列测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int addQueue(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpTask)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int addTcpTask(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int addTimeoutTask(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int addThreadPool(int left, int right)
        {
            return left + right;
        }

        static void Main(string[] args)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
#else
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenSimpleServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
#endif
                    Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleMethodServer.html
");
#if !NoAutoCSer
                    using (OpenSimpleServer.TcpOpenSimpleServer server = new OpenSimpleServer.TcpOpenSimpleServer())
                    {
                        if (server.IsListen)
                        {
                            if (!startProcess("TcpSimpleClientPerformance", "AutoCSer.TestCase.TcpOpenSimpleClientPerformance")) Console.WriteLine("未找到 TCP 服务性能测试服务 客户端程序");
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