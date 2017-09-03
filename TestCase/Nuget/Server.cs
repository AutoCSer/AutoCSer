using System;

namespace AutoCSer.TestCase.Nuget
{
    /// <summary>
    /// TCP 服务测试
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12120)]
    sealed partial class Server
    {
        /// <summary>
        /// 加法测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method]
        private int add(int left, int right)
        {
            return left + right;
        }
    }
}
