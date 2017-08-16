using System;
using System.Threading;

namespace AutoCSer.TestCase.TcpInternalSimpleClientPerformance
{
    /// <summary>
    /// 客户端同步线程
    /// </summary>
    sealed class ClientThread
    {
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
            using (AutoCSer.TestCase.TcpInternalSimpleServerPerformance.InternalSimpleServer.TcpInternalSimpleClient client = new AutoCSer.TestCase.TcpInternalSimpleServerPerformance.InternalSimpleServer.TcpInternalSimpleClient())
            {
                int errorCount = 0;
                for (int left = Left, right = Right; right != 0;)
                {
                    if (client.addSynchronous(left, --right).Value != left + right) ++errorCount;
                }
                Interlocked.Add(ref TcpInternalSimpleClientPerformance.Client.ErrorCount, errorCount);
                if (Interlocked.Decrement(ref TcpInternalSimpleClientPerformance.Client.ThreadCount) == 0)
                {
                    TcpInternalSimpleClientPerformance.Client.Time.Stop();
                    TcpInternalSimpleClientPerformance.Client.WaitHandle.Set();
                }
            }
        }
    }
}
