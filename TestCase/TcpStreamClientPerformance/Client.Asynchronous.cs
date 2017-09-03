using System;

namespace AutoCSer.TestCase.TcpInternalStreamClientPerformance
{
    /// <summary>
    /// TCP 客户端操作
    /// </summary>
    internal unsafe static partial class Client
    {
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        internal static void OnAdd(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add> value)
        {
            int right;
            if (value.Value.CheckSum(Left, out right) != 0 || !addMap.SetWhenNullUnsafe(right))
            {
                ++ErrorCount;
            }
            if (--WaitCount == 0)
            {
                Time.Stop();
                WaitHandle.Set();
            }
        }
    }
}
