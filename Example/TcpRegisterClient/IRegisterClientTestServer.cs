using System;

namespace AutoCSer.Example.TcpRegisterClient
{
    /// <summary>
    /// 测试服务接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Name = "ITestServer")]
    public interface IRegisterClientTestServer
    {
        /// <summary>
        /// 获取测试数据
        /// </summary>
        /// <returns>测试数据</returns>
        AutoCSer.Net.TcpServer.ReturnValue<int> Get();
    }
}
