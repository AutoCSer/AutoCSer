using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.TcpInternalSimpleServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Name = InternalSimpleServer.ServerName, Host = "127.0.0.1", Port = 12104, SendBufferSize = SubBuffer.Size.Byte256, IsRememberCommand = false, CheckSeconds = 0)]
    public partial class InternalSimpleServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "TcpInternalSimpleServerPerformance";

        /// <summary>
        /// 异步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        [AutoCSer.Net.TcpSimpleServer.Method]
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
        [AutoCSer.Net.TcpSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
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
        [AutoCSer.Net.TcpSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue)]
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
        [AutoCSer.Net.TcpSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpTask)]
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
        [AutoCSer.Net.TcpSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout)]
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
        [AutoCSer.Net.TcpSimpleServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int addThreadPool(int left, int right)
        {
            return left + right;
        }
    }
}
