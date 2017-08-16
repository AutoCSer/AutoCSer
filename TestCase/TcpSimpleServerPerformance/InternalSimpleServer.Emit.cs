using System;

namespace AutoCSer.TestCase.TcpInternalSimpleServerPerformance
{
    /// <summary>
    /// TCP 内部应答服务性能测试服务
    /// </summary>
    sealed class InternalSimpleServer : ISimpleServer
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
