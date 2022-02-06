using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 批量处理等待类型
    /// </summary>
    public enum OutputWaitType
    {
#if DOTNET2 || UNITY3D
        /// <summary>
        /// 不等待，适应于低并发场景，串行效率最大化
        /// </summary>
#else
        /// <summary>
        /// Thread.Yield()，适应于高并发场景以减少套接字调用次数，对串行效率有一些影响
        /// </summary>
#endif
        ThreadYield,
        /// <summary>
        /// 不等待，适应于低并发场景，串行效率最大化
        /// </summary>
        DontWait,
        /// <summary>
        /// Thread.Sleep(0)，适应于延时要求不高只要求并行吞吐量的场景以减少套接字调用次数，严重影响串行效率
        /// </summary>
        ThreadSleep,
    }
}
