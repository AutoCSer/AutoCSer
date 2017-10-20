using System;

namespace AutoCSer.Example.TcpInternalServer
{
    /// <summary>
    /// ref / out 参数测试 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12904, IsRemoteExpression = true)]
    partial class RefOut
    {
        /// <summary>
        /// ref / out 参数测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="product">乘积</param>
        /// <returns>和</returns>
        [AutoCSer.Net.TcpServer.Method]
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
            using (AutoCSer.Example.TcpInternalServer.RefOut.TcpInternalServer server = new AutoCSer.Example.TcpInternalServer.RefOut.TcpInternalServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalServer.RefOut.TcpInternalClient client = new AutoCSer.Example.TcpInternalServer.RefOut.TcpInternalClient())
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
            return false;
        }
    }
}
