using System;

namespace AutoCSer.TestCase.TcpServerPerformance
{
    /// <summary>
    /// TCP 应答服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = 12107, SendBufferSize = SubBuffer.Size.Byte256, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsJsonSerialize = true)]
    public interface IOpenSimpleServer
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
