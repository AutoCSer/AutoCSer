using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceServer
{
    /// <summary>
    /// 异步回调测试
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12602)]
    public interface IAsynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        void Add(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd);
    }
    /// <summary>
    /// 异步回调测试 示例
    /// </summary>
    class Asynchronous : IAsynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        public void Add(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd)
        {
            onAdd(left + right);
        }

        /// <summary>
        /// 加法求和等待事件
        /// </summary>
        private static readonly EventWaitHandle sumWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IAsynchronous>.Create(new Asynchronous()))
            {
                if (server.IsListen)
                {
                    IAsynchronous client = AutoCSer.Net.TcpInternalServer.Emit.Client<IAsynchronous>.Create();
                    using (client as IDisposable)
                    {
                        sumWait.Reset();
                        int sum = 0;
                        client.Add(2, 3, value =>
                        {
                            if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) sum = value.Value;
                            sumWait.Set();
                            return false;
                        });
                        sumWait.WaitOne();
                        return sum == 2 + 3;
                    }
                }
            }
            return false;
        }
    }
}
