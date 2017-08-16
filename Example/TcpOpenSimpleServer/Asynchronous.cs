using System;
using System.Threading;

namespace AutoCSer.Example.TcpOpenSimpleServer
{
    /// <summary>
    /// 异步回调测试 示例
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = 13305)]
    partial class Asynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        [AutoCSer.Net.TcpOpenSimpleServer.Method]
        void Add(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd)
        {
            onAdd(left + right);
        }

        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpOpenSimpleServer.Asynchronous.TcpOpenSimpleServer server = new AutoCSer.Example.TcpOpenSimpleServer.Asynchronous.TcpOpenSimpleServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient.Asynchronous.TcpOpenSimpleClient client = new AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient.Asynchronous.TcpOpenSimpleClient())
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, 3);
                        if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
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
