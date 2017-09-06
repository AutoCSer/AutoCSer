using System;

namespace AutoCSer.TestCase.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 服务字段与属性支持测试
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpInternalSimpleServer_Member, IsServer = true, IsRememberCommand = false)]
    public partial class Member
    {
        /// <summary>
        /// 测试字段
        /// </summary>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(IsOnlyGetMember = false)]
        [AutoCSer.Net.TcpSimpleServer.Method(IsOnlyGetMember = false)]
        protected int field;
        /// <summary>
        /// 测试属性
        /// </summary>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(IsOnlyGetMember = false)]
        [AutoCSer.Net.TcpSimpleServer.Method(IsOnlyGetMember = false)]
        protected int property
        {
            get { return field; }
            set { field = value; }
        }
        /// <summary>
        /// 只读属性[不支持不可读属性]
        /// </summary>
        [AutoCSer.Net.TcpOpenSimpleServer.Method]
        [AutoCSer.Net.TcpSimpleServer.Method]
        protected int getProperty
        {
            get { return field; }
        }
        /// <summary>
        /// 单索引测试
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method]
        [AutoCSer.Net.TcpSimpleServer.Method]
        protected int this[int index]
        {
            get { return field + index; }
        }
        /// <summary>
        /// 多索引测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenSimpleServer.Method(IsOnlyGetMember = false)]
        [AutoCSer.Net.TcpSimpleServer.Method(IsOnlyGetMember = false)]
        protected int this[int left, int right]
        {
            get { return field - left * right; }
            set { field = value + left * right; }
        }

        /// <summary>
        /// TCP 服务字段与属性支持测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
#if !NoAutoCSer
            Member member = new Member();
            using (Member.TcpInternalSimpleServer server = new Member.TcpInternalSimpleServer(null, null, member))
            {
                if (server.IsListen)
                {
                    using (Member.TcpInternalSimpleClient client = new Member.TcpInternalSimpleClient())
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
