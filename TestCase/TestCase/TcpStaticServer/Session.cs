using System;

namespace AutoCSer.TestCase.TcpStaticServer
{
    /// <summary>
    /// TCP 服务客户端识别测试
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = "SessionServer", Host = "127.0.0.1", Port = (int)ServerPort.TcpStaticServer_Session, IsServer = true, IsRememberCommand = false, MaxVerifyDataSize = 1 << 10)]
    internal partial class Session
    {
        /// <summary>
        /// 服务器端写客户端标识测试+服务器端验证函数测试
        /// </summary>
        /// <param name="client">客户端标识[必须定义为第一个参数]</param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticServer.Method(IsVerifyMethod = true)]
        private static bool login(AutoCSer.Net.TcpInternalServer.ServerSocketSender client, string user, string password)
        {
            return Login(client, user, password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal static bool Login(AutoCSer.Net.TcpServer.ServerSocketSender client, string user, string password)
        {
            switch (user)
            {
                case "userA":
                    if (password == "A")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userB":
                    if (password == "B")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userC":
                    if (password == "C")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userD":
                    if (password == "D")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userE":
                    if (password == "E")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userF":
                    if (password == "F")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userG":
                    if (password == "G")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
                case "userH":
                    if (password == "H")
                    {
                        client.ClientObject = user;
                        return true;
                    }
                    break;
            }
            return false;
        }

#if !NoAutoCSer
        /// <summary>
        /// 客户端验证
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        internal static bool Verify(AutoCSer.Net.TcpInternalServer.ClientSocketSender sender)
        {
            return TcpCall.Session.login(sender, "userA", "A");
        }
        /// <summary>
        /// TCP 服务客户端识别测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            using (SessionServer server = new SessionServer()) return server.IsListen && TestClient();
        }
        /// <summary>
        /// TCP 服务客户端识别测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestClient()
        {
            if (TcpCall.GetSession.myName().Value != "userA") return false;
            return true;
        }
#endif
    }
    /// <summary>
    /// TCP 服务客户端识别测试
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = "SessionServer")]
    internal partial class GetSession
    {
        /// <summary>
        /// 服务器端读客户端标识测试
        /// </summary>
        /// <param name="client">客户端标识[必须定义为第一个参数]</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticServer.Method]
        private static string myName(AutoCSer.Net.TcpInternalServer.ServerSocketSender client)
        {
            return MyName(client);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static string MyName(AutoCSer.Net.TcpServer.ServerSocketSender client)
        {
            return (string)client.ClientObject;
        }
    }
}
