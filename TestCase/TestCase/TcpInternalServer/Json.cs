using System;

namespace AutoCSer.TestCase.TcpInternalServer
{
    /// <summary>
    /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpInternalServer_Json, IsServer = true, IsRememberCommand = false, IsJsonSerialize = true)]
    internal partial class Json
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        protected static int incValue;
        /// <summary>
        /// 无参数无返回值调用测试
        /// </summary>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected void Inc()
        {
            ++incValue;
        }
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected void Set(int a)
        {
            incValue = a;
        }
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected void Add(int a, int b)
        {
            incValue = a + b;
        }

        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected int inc()
        {
            return ++incValue;
        }
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected int inc(int a)
        {
            return a + 1;
        }
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected int add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected int inc(out int a)
        {
            a = incValue;
            return a + 1;
        }
        /// <summary>
        /// 混合输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected int inc(int a, out int b)
        {
            b = a;
            return a + 1;
        }
        /// <summary>
        /// 混合输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpOpenServer.Method]
        [AutoCSer.Net.TcpServer.Method]
        protected int add(int a, int b, out int c)
        {
            c = b;
            return a + b;
        }

        /// <summary>
        /// TCP 服务 JSON 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
#if !NoAutoCSer
            using (Json.TcpInternalServer server = new Json.TcpInternalServer())
            {
                if (server.IsListen)
                {
                    using (Json.TcpInternalClient client = new Json.TcpInternalClient())
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
