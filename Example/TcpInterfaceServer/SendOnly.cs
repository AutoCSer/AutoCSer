using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceServer
{
    /// <summary>
    /// 仅发送请求测试
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12601)]
    public interface ISendOnly
    {
        /// <summary>
        /// 仅发送请求测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        [AutoCSer.Net.TcpServer.Method(IsClientSendOnly = true)]
        void SetSum(int left, int right);
    }
    /// <summary>
    /// 仅发送请求测试 示例
    /// </summary>
    class SendOnly : ISendOnly
    {
        /// <summary>
        /// 仅发送请求测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        public void SetSum(int left, int right)
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
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<ISendOnly>.Create(new SendOnly()))
            {
                if (server.IsListen)
                {
                    ISendOnly client = AutoCSer.Net.TcpInternalServer.Emit.Client<ISendOnly>.Create();
                    using (client as IDisposable)
                    {
                        sumWait.Reset();
                        sum = 0;
                        client.SetSum(2, 3);
                        sumWait.WaitOne();
                        return sum == 2 + 3;
                    }
                }
            }
            return false;
        }
    }
}
