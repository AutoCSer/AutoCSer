using System;

namespace AutoCSer.Example.TcpOpenSimpleServer
{
    /// <summary>
    /// 支持公共函数 示例
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = 13301, IsAttribute = false, MemberFilters = AutoCSer.Metadata.MemberFilters.Instance)]
    partial class Static
    {
        /// <summary>
        /// 支持公共函数
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpOpenSimpleServer.Method]
        public int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 支持公共函数测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpOpenSimpleServer.Static.TcpOpenSimpleServer server = new AutoCSer.Example.TcpOpenSimpleServer.Static.TcpOpenSimpleServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient.Static.TcpOpenSimpleClient client = new AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient.Static.TcpOpenSimpleClient())
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, 3);
                        return sum.Type == AutoCSer.Net.TcpServer.ReturnType.Success && sum.Value == 2 + 3;
                    }
                }
            }
            return false;
        }
    }
}
