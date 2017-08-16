using System;

namespace AutoCSer.Example.TcpOpenSimpleServer
{
    /// <summary>
    /// 无需 TCP 远程函数申明配置 示例
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = 13300, IsAttribute = false, MemberFilters = AutoCSer.Metadata.MemberFilters.Instance)]
    partial class NoAttribute
    {
        /// <summary>
        /// 无需 TCP 远程函数申明配置测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpOpenSimpleServer.Method]
        int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 无需 TCP 远程函数申明配置测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpOpenSimpleServer.NoAttribute.TcpOpenSimpleServer server = new AutoCSer.Example.TcpOpenSimpleServer.NoAttribute.TcpOpenSimpleServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient.NoAttribute.TcpOpenSimpleClient client = new AutoCSer.Example.TcpOpenSimpleServer.TcpSimpleClient.NoAttribute.TcpOpenSimpleClient())
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
