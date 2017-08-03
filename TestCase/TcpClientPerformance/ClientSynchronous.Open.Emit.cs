using System;
using System.Threading;

namespace AutoCSer.TestCase.TcpOpenClientPerformance
{
    /// <summary>
    /// 客户端同步线程
    /// </summary>
    sealed class ClientSynchronous
    {
        /// <summary>
        /// 测试客户端
        /// </summary>
        internal AutoCSer.TestCase.TcpServerPerformance.IOpenServer Client;
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
        internal void Run()
        {
            for (int left = Left, right = Right; right != 0;)
            {
                if (Client.Add(left, --right).Value != left + right) ++TcpInternalClientPerformance.Client.ErrorCount;
            }
            if (Interlocked.Decrement(ref TcpInternalClientPerformance.Client.ThreadCount) == 0)
            {
                TcpInternalClientPerformance.Client.Time.Stop();
                TcpInternalClientPerformance.Client.WaitHandle.Set();
            }
        }
    }
}
