using System;

namespace AutoCSer.TestCase.TcpServerPerformance
{
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    partial class OpenServer : IOpenServer
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
        /// <summary>
        /// 简单计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        public void AddAsynchronous(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd)
        {
            onAdd(new Add(left, right));
        }
	}
}
