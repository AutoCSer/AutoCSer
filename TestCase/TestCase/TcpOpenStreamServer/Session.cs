using System;

namespace AutoCSer.TestCase.TcpOpenStreamServer
{
    /// <summary>
    /// TCP 服务客户端识别测试
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpOpenStreamServer_Session, IsSegmentation = false, MaxVerifyDataSize = 1 << 10)]
    internal partial class Session
    {
        /// <summary>
        /// 服务器端写客户端标识测试+服务器端验证函数测试
        /// </summary>
        /// <param name="client">客户端标识[必须定义为第一个参数]</param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenStreamServer.Method(IsVerifyMethod = true)]
        private bool login(AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender client, string user, string password)
        {
            return TcpStaticStreamServer.Session.Login(client, user, password);
        }
        /// <summary>
        /// 服务器端读客户端标识测试
        /// </summary>
        /// <param name="client">客户端标识[必须定义为第一个参数]</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenStreamServer.Method]
        private string myName(AutoCSer.Net.TcpOpenStreamServer.ServerSocketSender client)
        {
            return TcpStaticStreamServer.GetSession.MyName(client);
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
            /// <param name="sender"></param>
            /// <returns></returns>
            public bool Verify(Session.TcpOpenStreamClient client, AutoCSer.Net.TcpOpenStreamServer.ClientSocketSender sender)
            {
                return client.login(sender, user, password).Value;
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
            using (Session.TcpOpenStreamServer server = new Session.TcpOpenStreamServer())
            {
                if (server.IsListen)
                {
                    using (Session.TcpOpenStreamClient clientA = new Session.TcpOpenStreamClient(null, new veify("userA", "A").Verify))
                    using (Session.TcpOpenStreamClient clientB = new Session.TcpOpenStreamClient(null, new veify("userB", "B").Verify))
                    using (Session.TcpOpenStreamClient clientC = new Session.TcpOpenStreamClient(null, new veify("userC", "C").Verify))
                    using (Session.TcpOpenStreamClient clientD = new Session.TcpOpenStreamClient(null, new veify("userD", "D").Verify))
                    using (Session.TcpOpenStreamClient clientE = new Session.TcpOpenStreamClient(null, new veify("userE", "E").Verify))
                    using (Session.TcpOpenStreamClient clientF = new Session.TcpOpenStreamClient(null, new veify("userF", "F").Verify))
                    using (Session.TcpOpenStreamClient clientG = new Session.TcpOpenStreamClient(null, new veify("userG", "G").Verify))
                    using (Session.TcpOpenStreamClient clientH = new Session.TcpOpenStreamClient(null, new veify("userH", "H").Verify))
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
