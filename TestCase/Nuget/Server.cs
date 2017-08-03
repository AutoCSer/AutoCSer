using System;

namespace AutoCSer.TestCase.Nuget
{
    /// <summary>
    /// TCP 服务测试
    /// </summary>
    class Server : IServer
    {
        /// <summary>
        /// 加法测试
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
