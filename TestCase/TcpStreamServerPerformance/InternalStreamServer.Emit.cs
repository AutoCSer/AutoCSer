using System;

namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    sealed class InternalStreamServer : IStreamServer, ITcpQueueStreamServer, IQueueStreamServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
        {
            return left + right;
        }
    }
}
