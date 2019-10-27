using System;
using System.Threading;

namespace AutoCSer.Example.TcpInterfaceServer
{
    /// <summary>
    /// 异步回调测试 服务端接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12605, CommandIdentityEnmuType = typeof(ServerAsynchronousCommand))]
    public interface IServerAsynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        void Add(int left, int right, AutoCSer.Net.TcpServer.ServerCallback<int> onAdd);
    }
    /// <summary>
    /// 异步回调测试 客户端端接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12605, CommandIdentityEnmuType = typeof(ServerAsynchronousCommand))]
    public interface IServerAsynchronousClient
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right);
    }
    /// <summary>
    /// 异步回调测试 命令映射枚举
    /// </summary>
    public enum ServerAsynchronousCommand
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        Add
    }
    /// <summary>
    /// 异步回调测试 + 客户端同步 示例
    /// </summary>
    class ServerAsynchronous : IServerAsynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        public void Add(int left, int right, AutoCSer.Net.TcpServer.ServerCallback<int> onAdd)
        {
            onAdd.Callback(left + right);
        }

        /// <summary>
        /// 异步回调 + 客户端同步 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IServerAsynchronous>.Create(new ServerAsynchronous()))
            {
                if (server.IsListen)
                {
                    IServerAsynchronousClient client = AutoCSer.Net.TcpInternalServer.Emit.Client<IServerAsynchronousClient>.Create();
                    using (client as IDisposable)
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, 3);
                        return sum.Type == Net.TcpServer.ReturnType.Success && sum.Value == 2 + 3;
                    }
                }
            }
            return false;
        }
    }
}
