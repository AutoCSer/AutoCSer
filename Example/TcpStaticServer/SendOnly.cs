using System;
using System.Threading;

namespace AutoCSer.Example.TcpStaticServer
{
    /// <summary>
    /// 仅发送请求测试 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = ServerName.Example2, Host = "127.0.0.1", Port = 12801, IsServer = true)]
    partial class SendOnly
    {
        /// <summary>
        /// 仅发送请求测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        [AutoCSer.Net.TcpStaticServer.Method(IsClientSendOnly = true, ServerName = ServerName.Example1)]
        static void SetSum1(int left, int right)
        {
            sum = left + right;
            sumWait.Set();
        }
        /// <summary>
        /// 仅发送请求测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        [AutoCSer.Net.TcpStaticServer.Method(IsClientSendOnly = true)]
        static void SetSum2(int left, int right)
        {
            sum = left + right + 2;
            sumWait.Set();
        }

        /// <summary>
        /// 加法求和结果
        /// </summary>
        private static int sum;
        /// <summary>
        /// 加法求和等待事件
        /// </summary>
        private static readonly EventWaitHandle sumWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 仅发送请求测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            sumWait.Reset();
            sum = 0;
            AutoCSer.Example.TcpStaticServer.TcpCall.SendOnly.SetSum1(2, 3);
            sumWait.WaitOne();
            if (sum != 2 + 3)
            {
                return false;
            }

            sumWait.Reset();
            sum = 0;
            AutoCSer.Example.TcpStaticServer.TcpCall.SendOnly.SetSum2(2, 3);
            sumWait.WaitOne();
            if (sum != 2 + 3 + 2)
            {
                return false;
            }

            return true;
        }
    }
}
