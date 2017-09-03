using System;

namespace AutoCSer.TestCase.TcpOpenStreamServer.Emit
{
    /// <summary>
    /// TCP 服务测试
    /// </summary>
    class Server : AutoCSer.TestCase.TcpInternalStreamServer.Emit.Server
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
            using (AutoCSer.Net.TcpOpenStreamServer.Server server = AutoCSer.Net.TcpOpenStreamServer.Emit.Server<AutoCSer.TestCase.TcpInternalStreamServer.Emit.IServer>.Create(new Server()))
            {
                if (server.IsListen)
                {
                    return testCase(AutoCSer.Net.TcpOpenStreamServer.Emit.Client<AutoCSer.TestCase.TcpInternalStreamServer.Emit.IServer>.Create());
                }
            }
            return false;
        }
    }
}
