using System;

namespace AutoCSer.TestCase.TcpStreamServerPerformance
{
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12117, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Synchronous, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
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
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12118, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.TcpQueue, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
    public interface ITcpQueueOpenStreamServer : IOpenStreamServer
    {
    }
    /// <summary>
    /// TCP 服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = 12119, ServerTaskType = AutoCSer.Net.TcpStreamServer.ServerTaskType.Queue, SendBufferSize = SubBuffer.Size.Kilobyte8, ReceiveBufferSize = SubBuffer.Size.Kilobyte8, CheckSeconds = 0, IsAutoClient = true, IsSegmentation = false, MinCompressSize = 0, IsServerBuildOutputThread = true, IsJsonSerialize = true)]
    public interface IQueueOpenStreamServer : IOpenStreamServer
    {
    }
}
