using System;

namespace AutoCSer.Example.TcpInterfaceServer
{
    /// <summary>
    /// 客户端异步 await 测试 服务端接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12607, CommandIdentityEnmuType = typeof(ClientAwaiterCommand))]
    public interface IClientAwaiterServer
    {
        /// <summary>
        /// 异步测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        int Add(int left, int right);
        /// <summary>
        /// 异步测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        void Add2(int left, int right);
    }
    /// <summary>
    /// 客户端异步 await 测试 客户端端接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12607, CommandIdentityEnmuType = typeof(ClientAwaiterCommand))]
    public interface IClientAwaiter
    {
        /// <summary>
        /// 异步 await 测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.Emit.Awaiter<int> Add(int left, int right);
        /// <summary>
        /// 异步 await 测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.Awaiter Add2(int left, int right);
    }
    /// <summary>
    /// 客户端 await 回调测试 命令映射枚举
    /// </summary>
    public enum ClientAwaiterCommand
    {
        /// <summary>
        /// 异步 await 测试
        /// </summary>
        Add,
        /// <summary>
        /// 异步 await 测试
        /// </summary>
        Add2
    }
    /// <summary>
    /// 客户端异步 await 测试 示例
    /// </summary>
    class ClientAwaiter : IClientAwaiterServer
    {
        /// <summary>
        /// 客户端异步 await 测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        public int Add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 计算和
        /// </summary>
        private static int sum2;
        /// <summary>
        /// 异步测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        public void Add2(int left, int right)
        {
            sum2 = left + right;
        }

        /// <summary>
        /// 客户端异步 await 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            return TestCaseAsync().Result;
        }
        /// <summary>
        /// 客户端异步 await 测试
        /// </summary>
        /// <returns></returns>
        internal static async System.Threading.Tasks.Task<bool> TestCaseAsync()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IClientAwaiterServer>.Create(new ClientAwaiter()))
            {
                if (server.IsListen)
                {
                    IClientAwaiter client = AutoCSer.Net.TcpInternalServer.Emit.Client<IClientAwaiter>.Create();
                    using (client as IDisposable)
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = await client.Add(2, 3);
                        if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
                        {
                            return false;
                        }

                        sum2 = 0;
                        AutoCSer.Net.TcpServer.ReturnValue returnValue = await client.Add2(3, 5);
                        if (returnValue.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum2 != 3 + 5)
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
