using System;
using AutoCSer.TestCase.TcpServerPerformance;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalStreamServer.Server(Name = InternalStreamServer.ServerName, Host = "127.0.0.1", Port = 12108, SendBufferSize = SubBuffer.Size.Kilobyte64, ReceiveBufferSize = SubBuffer.Size.Kilobyte64, IsRememberCommand = false, CheckSeconds = 0)]
    public partial class InternalStreamServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "TcpInternalStreamServerPerformance";

        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if DOTNET2 || DOTNET4
        [AutoCSer.Net.TcpStreamServer.Method]
#else
        [AutoCSer.Net.TcpStreamServer.Method(IsClientTaskAsync = true)]
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
        [AutoCSer.Net.TcpStreamServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected Add addAsynchronous(int left, int right)
        {
            return new Add(left, right);
        }
    }
}
