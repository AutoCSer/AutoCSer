using System;

namespace AutoCSer.TestCase.TcpOpenSimpleServer
{
    /// <summary>
    /// TCP 服务字段与属性支持测试
    /// </summary>
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpOpenSimpleServer_Member, IsSegmentation = false)]
    public partial class Member : TcpInternalSimpleServer.Member
    {
        /// <summary>
        /// TCP 服务字段与属性支持测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal new static bool TestCase()
        {
#if !NoAutoCSer
            Member member = new Member();
            using (Member.TcpOpenSimpleServer server = new Member.TcpOpenSimpleServer(null, null, member))
            {
                if (server.IsListen)
                {
                    using (Member.TcpOpenSimpleClient client = new Member.TcpOpenSimpleClient())
                    {
                        client.field = 1;
                        if (member.field != 1) return false;

                        member.field = 2;
                        if (client.field != 2) return false;

                        member.field = 3;
                        if (client.getProperty != 3) return false;

                        if (client[1] != 4) return false;

                        client.property = 5;
                        if (member.property != 5) return false;

                        member.property = 6;
                        if (client.property != 6) return false;

                        client[2, 3] = 7;
                        if (member[2, 3] != 7) return false;

                        member[3, 5] = 8;
                        if (client[3, 5] != 8) return false;

                        return true;
                    }
                }
            }
#endif
            return false;
        }
    }
}
