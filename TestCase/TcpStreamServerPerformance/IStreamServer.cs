using System;

namespace AutoCSer.TestCase.TcpInternalStreamServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalStreamServer.Server(Host = "127.0.0.1", Port = 12114, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Synchronous, SendBufferSize = SubBuffer.Size.Kilobyte64, ReceiveBufferSize = SubBuffer.Size.Kilobyte64, CheckSeconds = 0)]
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
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalStreamServer.Server(Host = "127.0.0.1", Port = 12115, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.TcpQueue, SendBufferSize = SubBuffer.Size.Kilobyte64, ReceiveBufferSize = SubBuffer.Size.Kilobyte64, CheckSeconds = 0)]
    public interface ITcpQueueStreamServer : IStreamServer
    {
    }
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalStreamServer.Server(Host = "127.0.0.1", Port = 12116, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Queue, SendBufferSize = SubBuffer.Size.Kilobyte64, ReceiveBufferSize = SubBuffer.Size.Kilobyte64, CheckSeconds = 0)]
    public interface IQueueStreamServer : IStreamServer
    {
    }
}
