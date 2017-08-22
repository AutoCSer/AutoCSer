using System;

namespace AutoCSer.Example.TcpRegisterClient
{
    /// <summary>
    /// 测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Name = "ITestServer")]
    public sealed partial class RegisterClientTestServer
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        internal int Version;
        /// <summary>
        /// 获取测试数据
        /// </summary>
        /// <returns>测试数据</returns>
        [AutoCSer.Net.TcpServer.Method]
        private int get()
        {
            return Version;
        }
    }
}
