using System;

namespace AutoCSer.TestCase.TcpOpenServer.Emit
{
    /// <summary>
    /// TCP 服务测试
    /// </summary>
    class Server : AutoCSer.TestCase.TcpInternalServer.Emit.Server
    {
        /// <summary>
        /// TCP 服务测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal new static bool TestCase()
        {
            using (AutoCSer.Net.TcpOpenServer.Server server = AutoCSer.Net.TcpOpenServer.Emit.Server<AutoCSer.TestCase.TcpInternalServer.Emit.IServer>.Create(new Server()))
            {
                if (server.IsListen)
                {
                    return testCase(AutoCSer.Net.TcpOpenServer.Emit.Client<AutoCSer.TestCase.TcpInternalServer.Emit.IServer>.Create());
                }
            }
            return false;
        }
    }
}
