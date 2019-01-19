using System;
using AutoCSer.Net.TcpOpenServer;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Example.TcpOpenServer
{
    /// <summary>
    /// ref / out 参数测试 示例
    /// </summary>
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = 13004, IsRemoteExpression = true)]
    partial class RefOut
    {
        /// <summary>
        /// ref / out 参数测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="product">乘积</param>
        /// <returns>和</returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, ref int right, out int product)
        {
            product = left * right;
            int sum = left + right;
            right <<= 1;
            return sum;
        }

        /// <summary>
        /// ref / out 参数 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer server = new AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenServer.TcpClient.RefOut.TcpOpenClient client = new AutoCSer.Example.TcpOpenServer.TcpClient.RefOut.TcpOpenClient())
                    {
                        int right = 3, product;
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, ref right, out product);
                        if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3 || right != (3 << 1) || product != 2 * 3)
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }
            //AutoCSer.Net.TcpOpenServer.ServerAttribute attribute1 = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenServer.RefOut", typeof(AutoCSer.Example.TcpOpenServer.RefOut));
            //AutoCSer.Net.TcpOpenServer.ServerAttribute attribute2 = AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpOpenServer.ServerAttribute>.MemberwiseClone(attribute1);
            //++attribute2.Port;
            //using (AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer server1 = new AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer(attribute1))
            //using (AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer server2 = new AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer(attribute2))
            //{
            //    if (server1.IsListen && server2.IsListen)
            //    {
            //        using (AutoCSer.Example.TcpOpenServer.TcpClient.RefOut.TcpOpenClient client = new AutoCSer.Example.TcpOpenServer.TcpClient.RefOut.TcpOpenClient(null, new ClientLoadRoute(attribute1, attribute2)))
            //        {
            //            client._TcpClient_.Sender;

            //        }
            //    }
            //}
            return false;
        }

        //internal sealed class ClientLoadRoute : AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenServer.ClientSocketSender>
        //{
        //    private AutoCSer.Net.TcpOpenServer.ServerAttribute attribute1;
        //    private AutoCSer.Net.TcpOpenServer.ServerAttribute attribute2;
        //    internal ClientLoadRoute(AutoCSer.Net.TcpOpenServer.ServerAttribute attribute1, AutoCSer.Net.TcpOpenServer.ServerAttribute attribute2)
        //    {
        //        this.attribute1 = attribute1;
        //        this.attribute2 = attribute2;

        //    }
        //    public override Net.TcpOpenServer.ClientSocketSender Sender
        //    {
        //        get
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }

        //    public override ClientSocketBase Socket
        //    {
        //        get
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }

        //    public override void OnDisposeSocket(ClientSocketBase socket)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override void OnSetSocket()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override bool OnSocketVerifyMethod(ClientSocketBase socket)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
