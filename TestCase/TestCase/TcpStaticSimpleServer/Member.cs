using System;

namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
    /// <summary>
    /// TCP服务字段与属性支持测试
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = "SimpleMemberServer", Host = "127.0.0.1", Port = (int)ServerPort.TcpStaticSimpleServer_Member, IsServer = true, IsRememberCommand = false)]
    public partial class Member
    {
        /// <summary>
        /// 测试字段
        /// </summary>
        [AutoCSer.Net.TcpStaticSimpleServer.Method(IsOnlyGetMember = false)]
        internal static int field;
#if !NoAutoCSer
        /// <summary>
        /// TCP 服务字段与属性支持测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            using (SimpleMemberServer server = new SimpleMemberServer()) return server.IsListen && TestClient();
        }
        /// <summary>
        /// TCP 服务字段与属性支持测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestClient()
        {
            TcpCallSimple.Member.field = 1;
            if (field != 1) return false;

            field = 2;
            if (TcpCallSimple.Member.field != 2) return false;

            field = 3;
            if (TcpCallSimple.MemberProperty.getProperty != 3) return false;

            TcpCallSimple.MemberProperty.property = 5;
            if (field != 5) return false;

            field = 6;
            if (TcpCallSimple.MemberProperty.property != 6) return false;

            return true;
        }
#endif
    }
    /// <summary>
    /// TCP服务字段与属性支持测试
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = "SimpleMemberServer")]
    public partial class MemberProperty
    {
        /// <summary>
        /// 测试属性
        /// </summary>
        [AutoCSer.Net.TcpStaticSimpleServer.Method(IsOnlyGetMember = false)]
        private static int property
        {
            get { return Member.field; }
            set { Member.field = value; }
        }
        /// <summary>
        /// 只读属性[不支持不可读属性]
        /// </summary>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int getProperty
        {
            get { return Member.field; }
        }
    }
}
