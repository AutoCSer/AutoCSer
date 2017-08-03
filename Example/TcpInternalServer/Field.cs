using System;

namespace AutoCSer.Example.TcpInternalServer
{
    /// <summary>
    /// 字段支持 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12902)]
    partial class Field
    {
        /// <summary>
        /// 只读字段支持
        /// </summary>
        [AutoCSer.Net.TcpServer.Method]
        int GetField;

        /// <summary>
        /// 可写字段支持
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(IsOnlyGetMember = false)]
        int SetField;

        /// <summary>
        /// 字段支持 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpInternalServer.Field.TcpInternalServer server = new AutoCSer.Example.TcpInternalServer.Field.TcpInternalServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalServer.Field.TcpInternalClient client = new AutoCSer.Example.TcpInternalServer.Field.TcpInternalClient())
                    {
                        server.Value.GetField = 2;
                        AutoCSer.Net.TcpServer.ReturnValue<int> value = client.GetField;
                        if (value.Type != AutoCSer.Net.TcpServer.ReturnType.Success || value.Value != 2)
                        {
                            return false;
                        }

                        server.Value.SetField = 0;
                        client.SetField = 3;
                        if (server.Value.SetField != 3)
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
