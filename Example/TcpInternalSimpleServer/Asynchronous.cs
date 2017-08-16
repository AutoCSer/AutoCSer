using System;
using System.Threading;

namespace AutoCSer.Example.TcpInternalSimpleServer
{
    /// <summary>
    /// 异步回调测试 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Host = "127.0.0.1", Port = 13105)]
    partial class Asynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        [AutoCSer.Net.TcpSimpleServer.Method]
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
            using (AutoCSer.Example.TcpInternalSimpleServer.Asynchronous.TcpInternalSimpleServer server = new AutoCSer.Example.TcpInternalSimpleServer.Asynchronous.TcpInternalSimpleServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalSimpleServer.Asynchronous.TcpInternalSimpleClient client = new AutoCSer.Example.TcpInternalSimpleServer.Asynchronous.TcpInternalSimpleClient())
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
