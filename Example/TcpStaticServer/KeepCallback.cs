using System;
using System.Threading;

namespace AutoCSer.Example.TcpStaticServer
{
    /// <summary>
    /// 异步回调注册测试 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = ServerName.Example1)]
    partial class KeepCallback
    {
        /// <summary>
        /// 异步回调注册测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="count">回调次数</param>
        /// <param name="onAdd">加法计算回调委托</param>
        [AutoCSer.Net.TcpStaticServer.KeepCallbackMethod]
        static void Add(int left, int right, int count, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd)
        {
            while (count != 0)
            {
                onAdd(left + right);
                --count;
            }
        }

        /// <summary>
        /// 加法求和等待事件
        /// </summary>
        private static readonly EventWaitHandle sumWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 异步回调注册测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            sumWait.Reset();
            int count = 4, successCount = count;
            using (AutoCSer.Net.TcpServer.KeepCallback keepCallback =
            AutoCSer.Example.TcpStaticServer.TcpCall.KeepCallback.Add(2, 3, count, value =>
            {
                if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success && value.Value == 2 + 3) --successCount;
                if (--count == 0) sumWait.Set();
            }))
            {
                sumWait.WaitOne();
                return successCount == 0;
            }
        }
    }
}
