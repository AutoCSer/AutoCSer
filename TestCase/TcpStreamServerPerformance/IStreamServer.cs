using System;

namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalStreamServer.Server(Host = "127.0.0.1", Port = 12114, SendBufferSize = SubBuffer.Size.Kilobyte64, ReceiveBufferSize = SubBuffer.Size.Kilobyte64, CheckSeconds = 0)]
    public interface IStreamServer
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
