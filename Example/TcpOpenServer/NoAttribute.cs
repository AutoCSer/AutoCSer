using System;

namespace AutoCSer.Example.TcpOpenServer
{
    /// <summary>
    /// 无需 TCP 远程函数申明配置 示例
    /// </summary>
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = 13000, IsAttribute = false, MemberFilters = AutoCSer.Metadata.MemberFilters.Instance)]
    partial class NoAttribute
    {
        /// <summary>
        /// 无需 TCP 远程函数申明配置测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpOpenServer.Method]
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
            using (AutoCSer.Example.TcpOpenServer.NoAttribute.TcpOpenServer server = new AutoCSer.Example.TcpOpenServer.NoAttribute.TcpOpenServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenServer.TcpClient.NoAttribute.TcpOpenClient client = new AutoCSer.Example.TcpOpenServer.TcpClient.NoAttribute.TcpOpenClient())
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, 3);
                        if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
                        {
                            return false;
                        }

                        #region Awaiter.Wait()
                        sum = client.AddAwaiter(2, 3).Wait().Result;
                        if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
                        {
                            return false;
                        }
                        #endregion

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
