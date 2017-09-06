using System;

namespace AutoCSer.TestCase.TcpOpenStreamServer
{
    /// <summary>
    /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
    /// </summary>
    [AutoCSer.Net.TcpOpenStreamServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpOpenStreamServer_Json, IsSegmentation = false, IsJsonSerialize = true)]
    internal partial class Json : TcpInternalStreamServer.Json
    {
        /// <summary>
        /// TCP 服务 JSON 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal new static bool TestCase()
        {
#if !NoAutoCSer
            using (Json.TcpOpenStreamServer server = new Json.TcpOpenStreamServer())
            {
                if (server.IsListen)
                {
                    using (Json.TcpOpenStreamClient client = new Json.TcpOpenStreamClient())
                    {
                        incValue = 0;
                        client.Inc();
                        if (incValue != 1) return false;

                        client.Set(3);
                        if (incValue != 3) return false;

                        client.Add(2, 3);
                        if (incValue != 5) return false;

                        if (client.inc().Value != 6) return false;
                        if (client.inc(8).Value != 9) return false;
                        if (client.add(10, 13).Value != 23) return false;

                        incValue = 15;
                        int outValue;
                        if (client.inc(out outValue).Value != 16 && outValue != 15) return false;
                        if (client.inc(20, out outValue).Value != 21 && outValue != 20) return false;
                        if (client.add(30, 33, out outValue).Value != 63 && outValue != 33) return false;
                        return true;
                    }
                }
            }
#endif
            return false;
        }
    }
}
