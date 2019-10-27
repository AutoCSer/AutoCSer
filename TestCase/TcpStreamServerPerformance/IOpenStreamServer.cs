using System;

namespace AutoCSer.TestCase.TcpStreamServerPerformance
{
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12117, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true, QueueCommandSize = 1 << 16)]
    public interface IOpenStreamServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenStreamServer.Method]
        AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right);
    }
}
