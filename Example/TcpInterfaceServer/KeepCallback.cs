using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceServer
{
    /// <summary>
    /// 异步回调注册测试
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12603)]
    public interface IKeepCallback
    {
        /// <summary>
        /// 异步回调注册测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="count">回调次数</param>
        /// <param name="onAdd">加法计算回调委托</param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.KeepCallback Add(int left, int right, int count, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd);
    }
    /// <summary>
    /// 异步回调注册测试 示例
    /// </summary>
    class KeepCallback : IKeepCallback
    {
        /// <summary>
        /// 异步回调注册测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="count">回调次数</param>
        /// <param name="onAdd">加法计算回调委托</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback Add(int left, int right, int count, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd)
        {
            while (count != 0)
            {
                onAdd(left + right);
                --count;
            }
            return null;
        }

        /// <summary>
        /// 加法求和等待事件
        /// </summary>
        private static readonly EventWaitHandle sumWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 异步回调注册测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IKeepCallback>.Create(new KeepCallback()))
            {
                if (server.IsListen)
                {
                    IKeepCallback client = AutoCSer.Net.TcpInternalServer.Emit.Client<IKeepCallback>.Create();
                    using (client as IDisposable)
                    {
                        sumWait.Reset();
                        int count = 4, successCount = count;
                        using (AutoCSer.Net.TcpServer.KeepCallback keepCallback =
                        client.Add(2, 3, count, value =>
                        {
                            if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success && value.Value == 2 + 3) --successCount;
                            if (--count == 0) sumWait.Set();
                            return false;
                        }))
                        {
                            sumWait.WaitOne();
                            return successCount == 0;
                        }
                    }
                }
            }
            return false;
        }
    }
}
