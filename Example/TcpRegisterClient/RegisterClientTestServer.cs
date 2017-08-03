using System;

namespace AutoCSer.Example.TcpRegisterClient
{
    /// <summary>
    /// 测试服务
    /// </summary>
    sealed class RegisterClientTestServer : IRegisterClientTestServer
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        internal int Version;
        /// <summary>
        /// 获取测试数据
        /// </summary>
        /// <returns>测试数据</returns>
        public AutoCSer.Net.TcpServer.ReturnValue<int> Get()
        {
            return Version;
        }
    }
}
