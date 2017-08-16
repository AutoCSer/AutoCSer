using System;

namespace AutoCSer.Example.TcpInternalSimpleServer
{
    /// <summary>
    /// 字段支持 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Host = "127.0.0.1", Port = 13102)]
    partial class Field
    {
        /// <summary>
        /// 只读字段支持
        /// </summary>
        [AutoCSer.Net.TcpSimpleServer.Method]
        int GetField;

        /// <summary>
        /// 可写字段支持
        /// </summary>
        [AutoCSer.Net.TcpSimpleServer.Method(IsOnlyGetMember = false)]
        int SetField;

        /// <summary>
        /// 字段支持 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpInternalSimpleServer.Field.TcpInternalSimpleServer server = new AutoCSer.Example.TcpInternalSimpleServer.Field.TcpInternalSimpleServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalSimpleServer.Field.TcpInternalSimpleClient client = new AutoCSer.Example.TcpInternalSimpleServer.Field.TcpInternalSimpleClient())
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
