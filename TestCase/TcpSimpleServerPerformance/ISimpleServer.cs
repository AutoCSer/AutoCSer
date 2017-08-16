using System;

namespace AutoCSer.TestCase.TcpInternalSimpleServerPerformance
{
    /// <summary>
    /// TCP 内部应答服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Host = "127.0.0.1", Port = 12106, SendBufferSize = SubBuffer.Size.Byte256, CheckSeconds = 0)]
    public interface ISimpleServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right);
    }
}
