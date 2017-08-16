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
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.TestCase.TcpOpenSimpleServerPerformance", out createdProcessWait);
            if (createdProcessWait)
            {
                Console.WriteLine(@"http://www.AutoCSer.com/TcpServer/SimpleMethodServer.html
");
#if NoAutoCSer
#else
                using (processWait)
                using (OpenSimpleServer.TcpOpenSimpleServer server = new OpenSimpleServer.TcpOpenSimpleServer())
                {
                    if (server.IsListen)
                    {
#if DEBUG
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Debug\AutoCSer.TestCase.TcpOpenSimpleClientPerformance.exe".pathSeparator()));
#else
                        FileInfo clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"..\..\..\TcpSimpleClientPerformance\bin\Release\AutoCSer.TestCase.TcpOpenSimpleClientPerformance.exe".pathSeparator()));
#endif
                        if (!clientFile.Exists) clientFile = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, @"AutoCSer.TestCase.TcpOpenSimpleClientPerformance.exe"));
                        if (clientFile.Exists) Process.Start(clientFile.FullName);
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
            }
        }
    }
}
