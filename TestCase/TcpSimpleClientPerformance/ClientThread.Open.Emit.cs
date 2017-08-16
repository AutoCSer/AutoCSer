using System;
using System.Threading;

namespace AutoCSer.TestCase.TcpOpenSimpleClientPerformance
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
            AutoCSer.TestCase.TcpServerPerformance.IOpenSimpleServer client = AutoCSer.Net.TcpOpenSimpleServer.Emit.Client<AutoCSer.TestCase.TcpServerPerformance.IOpenSimpleServer>.Create();
            using (AutoCSer.Net.TcpOpenSimpleServer.Emit.MethodClient methodClient = client as AutoCSer.Net.TcpOpenSimpleServer.Emit.MethodClient)
            {
                int errorCount = 0;
                for (int left = Left, right = Right; right != 0;)
                {
                    if (client.Add(left, --right).Value != left + right) ++errorCount;
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
