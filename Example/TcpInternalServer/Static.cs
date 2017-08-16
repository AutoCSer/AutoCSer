using System;

namespace AutoCSer.Example.TcpInternalServer
{
    /// <summary>
    /// 支持公共函数 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12901, IsAttribute = false, MemberFilters = AutoCSer.Metadata.MemberFilters.Instance)]
    partial class Static
    {
        /// <summary>
        /// 支持公共函数
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpServer.Method]
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
            using (AutoCSer.Example.TcpInternalServer.Static.TcpInternalServer server = new AutoCSer.Example.TcpInternalServer.Static.TcpInternalServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalServer.Static.TcpInternalClient client = new AutoCSer.Example.TcpInternalServer.Static.TcpInternalClient())
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
