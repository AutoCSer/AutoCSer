using System;
using System.Threading;

namespace AutoCSer.Example.TcpStaticServer
{
    /// <summary>
    /// 同步函数客户端异步测试 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = ServerName.Example1)]
    partial class ClientAsynchronous
    {
        /// <summary>
        /// 同步函数客户端异步测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        [AutoCSer.Net.TcpStaticServer.Method(IsClientAsynchronous = true, IsClientSynchronous = true)]
        static int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 加法求和等待事件
        /// </summary>
        private static readonly EventWaitHandle sumWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 同步函数客户端异步测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            #region 同步代理调用
            AutoCSer.Net.TcpServer.ReturnValue<int> sum = AutoCSer.Example.TcpStaticServer.TcpCall.ClientAsynchronous.Add(2, 3);
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            #region Awaiter.Wait()
            sum = AutoCSer.Example.TcpStaticServer.TcpCall.ClientAsynchronous.AddAwaiter(2, 3).Wait().Result;
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            #region 异步代理调用
            sum = default(AutoCSer.Net.TcpServer.ReturnValue<int>);
            sumWait.Reset();
            AutoCSer.Example.TcpStaticServer.TcpCall.ClientAsynchronous.Add(2, 3, value =>
            {
                sum = value;
                sumWait.Set();
            });
            sumWait.WaitOne();
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}
