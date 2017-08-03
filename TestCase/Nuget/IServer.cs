using System;

namespace AutoCSer.TestCase.Nuget
{
    /// <summary>
    /// TCP 服务接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12104)]
    public interface IServer
    {
        /// <summary>
        /// 加法测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right);
    }
}
