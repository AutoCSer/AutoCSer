using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceServer
{
    /// <summary>
    /// 客户端异步回调测试 服务端接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12606, CommandIdentityEnmuType = typeof(ClientAsynchronousCommand))]
    public interface IClientAsynchronousServer
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        int Add(int left, int right);
    }
    /// <summary>
    /// 客户端异步回调测试 客户端端接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12606, CommandIdentityEnmuType = typeof(ClientAsynchronousCommand))]
    public interface IClientAsynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> onAdd);
    }
    /// <summary>
    /// 客户端异步回调测试 命令映射枚举
    /// </summary>
    public enum ClientAsynchronousCommand
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        Add
    }
    /// <summary>
    /// 客户端异步回调测试 示例
    /// </summary>
    class ClientAsynchronous : IClientAsynchronousServer
    {
        /// <summary>
        /// 客户端异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        public int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 加法求和等待事件
        /// </summary>
        private static readonly EventWaitHandle sumWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        /// <summary>
        /// 客户端异步回调测试 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IClientAsynchronousServer>.Create(new ClientAsynchronous()))
            {
                if (server.IsListen)
                {
                    IClientAsynchronous client = AutoCSer.Net.TcpInternalServer.Emit.Client<IClientAsynchronous>.Create();
                    using (client as IDisposable)
                    {
                        sumWait.Reset();
                        int sum = 0;
                        client.Add(2, 3, value =>
                        {
                            if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) sum = value.Value;
                            sumWait.Set();
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
