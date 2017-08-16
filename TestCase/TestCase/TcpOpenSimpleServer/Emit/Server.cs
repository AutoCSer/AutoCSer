using System;

namespace AutoCSer.TestCase.TcpOpenSimpleServer.Emit
{
    /// <summary>
    /// TCP 服务测试
    /// </summary>
    class Server : AutoCSer.TestCase.TcpInternalSimpleServer.Emit.Server
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
            using (AutoCSer.Net.TcpOpenSimpleServer.Server server = AutoCSer.Net.TcpOpenSimpleServer.Emit.Server<AutoCSer.TestCase.TcpInternalSimpleServer.Emit.IServer>.Create(new Server()))
            {
                if (server.IsListen)
                {
                    return testCase(AutoCSer.Net.TcpOpenSimpleServer.Emit.Client<AutoCSer.TestCase.TcpInternalSimpleServer.Emit.IServer>.Create());
                }
            }
            return false;
        }
    }
}
