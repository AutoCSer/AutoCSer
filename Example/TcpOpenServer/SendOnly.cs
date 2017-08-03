using System;
using System.Threading;

namespace AutoCSer.Example.TcpOpenServer
{
    /// <summary>
    /// 仅发送请求测试 示例
    /// </summary>
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = 13006)]
    partial class SendOnly
    {
        /// <summary>
        /// 仅发送请求测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        [AutoCSer.Net.TcpOpenServer.Method(IsClientSendOnly = true)]
        void SetSum(int left, int right)
        {
            sum = left + right;
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
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpOpenServer.SendOnly.TcpOpenServer server = new AutoCSer.Example.TcpOpenServer.SendOnly.TcpOpenServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenServer.TcpClient.SendOnly.TcpOpenClient client = new AutoCSer.Example.TcpOpenServer.TcpClient.SendOnly.TcpOpenClient())
                    {
                        sumWait.Reset();
                        sum = 0;
                        client.SetSum(2, 3);
                        sumWait.WaitOne();
                        if (sum != 2 + 3)
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
