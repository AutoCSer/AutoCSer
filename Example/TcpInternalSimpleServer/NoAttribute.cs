using System;

namespace AutoCSer.Example.TcpInternalSimpleServer
{
    /// <summary>
    /// 无需 TCP 远程函数申明配置 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Host = "127.0.0.1", Port = 13100, IsAttribute = false, MemberFilters = AutoCSer.Metadata.MemberFilters.Instance)]
    partial class NoAttribute
    {
        /// <summary>
        /// 无需 TCP 远程函数申明配置测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpSimpleServer.Method]
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
            using (AutoCSer.Example.TcpInternalSimpleServer.NoAttribute.TcpInternalSimpleServer server = new AutoCSer.Example.TcpInternalSimpleServer.NoAttribute.TcpInternalSimpleServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalSimpleServer.NoAttribute.TcpInternalSimpleClient client = new AutoCSer.Example.TcpInternalSimpleServer.NoAttribute.TcpInternalSimpleClient())
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, 3);
                        if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
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
