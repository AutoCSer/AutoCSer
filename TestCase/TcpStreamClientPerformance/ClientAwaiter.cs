using System;
using System.Threading;

namespace AutoCSer.TestCase.TcpInternalStreamClientPerformance
{
    /// <summary>
    /// 客户端同步线程
    /// </summary>
    sealed class ClientAwaiter
    {
        /// <summary>
        /// 测试客户端
        /// </summary>
        internal AutoCSer.TestCase.TcpInternalStreamServerPerformance.InternalStreamServer.TcpInternalStreamClient Client;
        /// <summary>
        /// 
        /// </summary>
        internal int Left;
        /// <summary>
        /// 
        /// </summary>
        internal int Right;
        /// <summary>
        /// 
        /// </summary>
#if DOTNET2 || DOTNET4
        internal void Run()
#else
        internal async void Run()
#endif
        {
            for (int left = Left, right = Right; right != 0;)
            {
#if DOTNET2 || DOTNET4
                if ((Client.addAwaiter(left, --right)).Wait().Result.Value != left + right) ++TcpInternalStreamClientPerformance.Client.ErrorCount;
#else
                if ((await Client.addAwaiter(left, --right)).Value != left + right) ++TcpInternalStreamClientPerformance.Client.ErrorCount;
#endif
            }
            if (Interlocked.Decrement(ref TcpInternalStreamClientPerformance.Client.ThreadCount) == 0)
            {
                TcpInternalStreamClientPerformance.Client.Time.Stop();
                TcpInternalStreamClientPerformance.Client.WaitHandle.Set();
            }
        }
    }
}
