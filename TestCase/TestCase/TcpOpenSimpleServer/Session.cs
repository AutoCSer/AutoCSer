using System;

namespace AutoCSer.TestCase.TcpOpenSimpleServer
{
    /// <summary>
    /// TCP 服务客户端识别测试
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpOpenSimpleServer_Session, IsSegmentation = false, MaxVerifyDataSize = 1 << 10)]
    internal partial class Session
    {
        /// <summary>
        /// 服务器端写客户端标识测试+服务器端验证函数测试
        /// </summary>
        /// <param name="client">客户端标识[必须定义为第一个参数]</param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(IsVerifyMethod = true)]
        private bool login(AutoCSer.Net.TcpOpenSimpleServer.ServerSocket client, string user, string password)
        {
            return TcpStaticSimpleServer.Session.Login(client, user, password);
        }
        /// <summary>
        /// 服务器端读客户端标识测试
        /// </summary>
        /// <param name="client">客户端标识[必须定义为第一个参数]</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method]
        private string myName(AutoCSer.Net.TcpOpenSimpleServer.ServerSocket client)
        {
            return TcpStaticSimpleServer.GetSession.MyName(client);
        }

        /// <summary>
        /// 客户端验证
        /// </summary>
        private sealed class veify
        {
            /// <summary>
            /// 
            /// </summary>
            private string user;
            /// <summary>
            /// 
            /// </summary>
            private string password;
            /// <summary>
            /// 客户端验证
            /// </summary>
            /// <param name="user"></param>
            /// <param name="password"></param>
            public veify(string user, string password)
            {
                this.user = user;
                this.password = password;
            }
#if !NoAutoCSer
            /// <summary>
            /// 客户端验证
            /// </summary>
            /// <param name="client"></param>
            /// <returns></returns>
            public bool Verify(Session.TcpOpenSimpleClient client)
            {
                return client.login(user, password).Value;
            }
#endif
        }
        /// <summary>
        /// TCP服务客户端识别测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
#if !NoAutoCSer
            using (Session.TcpOpenSimpleServer server = new Session.TcpOpenSimpleServer())
            {
                if (server.IsListen)
                {
                    using (Session.TcpOpenSimpleClient clientA = new Session.TcpOpenSimpleClient(null, new veify("userA", "A").Verify))
                    using (Session.TcpOpenSimpleClient clientB = new Session.TcpOpenSimpleClient(null, new veify("userB", "B").Verify))
                    using (Session.TcpOpenSimpleClient clientC = new Session.TcpOpenSimpleClient(null, new veify("userC", "C").Verify))
                    using (Session.TcpOpenSimpleClient clientD = new Session.TcpOpenSimpleClient(null, new veify("userD", "D").Verify))
                    using (Session.TcpOpenSimpleClient clientE = new Session.TcpOpenSimpleClient(null, new veify("userE", "E").Verify))
                    using (Session.TcpOpenSimpleClient clientF = new Session.TcpOpenSimpleClient(null, new veify("userF", "F").Verify))
                    using (Session.TcpOpenSimpleClient clientG = new Session.TcpOpenSimpleClient(null, new veify("userG", "G").Verify))
                    using (Session.TcpOpenSimpleClient clientH = new Session.TcpOpenSimpleClient(null, new veify("userH", "H").Verify))
                    {
                        if (clientA.myName().Value != "userA") return false;
                        if (clientB.myName().Value != "userB") return false;
                        if (clientC.myName().Value != "userC") return false;
                        if (clientD.myName().Value != "userD") return false;
                        if (clientE.myName().Value != "userE") return false;
                        if (clientF.myName().Value != "userF") return false;
                        if (clientG.myName().Value != "userG") return false;
                        if (clientH.myName().Value != "userH") return false;
                    }
                    return true;
                }
            }
#endif
            return false;
        }
    }
}
