using System;

namespace AutoCSer.TestCase.TcpStaticSimpleServer
{
    /// <summary>
    /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = "SimpleJsonServer", Host = "127.0.0.1", Port = (int)ServerPort.TcpStaticSimpleServer_Json, IsServer = true, IsRememberCommand = false, IsJsonSerialize = true)]
    internal partial class Json
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        internal static int IncValue;
        /// <summary>
        /// 无参数无返回值调用测试
        /// </summary>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static void Inc()
        {
            ++IncValue;
        }
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static void Set(int a)
        {
            IncValue = a;
        }
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static void Add(int a, int b)
        {
            IncValue = a + b;
        }

        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int inc()
        {
            return ++IncValue;
        }
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int inc(int a)
        {
            return a + 1;
        }
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int add(int a, int b)
        {
            return a + b;
        }
#if !NoAutoCSer
        /// <summary>
        /// TCP 服务 JSON 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            using (SimpleJsonServer server = new SimpleJsonServer()) return server.IsListen && TestClient();
        }
        /// <summary>
        /// TCP 服务 JSON 序列化测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestClient()
        {
            IncValue = 0;
            TcpCallSimple.Json.Inc();
            if (IncValue != 1) return false;

            TcpCallSimple.Json.Set(3);
            if (IncValue != 3) return false;

            TcpCallSimple.Json.Add(2, 3);
            if (IncValue != 5) return false;

            if (TcpCallSimple.Json.inc().Value != 6) return false;
            if (TcpCallSimple.Json.inc(8).Value != 9) return false;
            if (TcpCallSimple.Json.add(10, 13).Value != 23) return false;

            IncValue = 15;
            int outValue;
            if (TcpCallSimple.JsonOut.inc(out outValue).Value != 16 && outValue != 15) return false;
            if (TcpCallSimple.JsonOut.inc(20, out outValue).Value != 21 && outValue != 20) return false;
            if (TcpCallSimple.JsonOut.add(30, 33, out outValue).Value != 63 && outValue != 33) return false;
            return true;
        }
#endif
    }
    /// <summary>
    /// TCP服务JSON序列化支持测试，必须指定[IsJsonSerialize = true]，否则默认为二进制序列化
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = "SimpleJsonServer", Host = "127.0.0.1", Port = (int)ServerPort.TcpStaticSimpleServer_Json, IsServer = true, IsJsonSerialize = true)]
    internal partial class JsonOut
    {
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int inc(out int a)
        {
            a = Json.IncValue;
            return a + 1;
        }
        /// <summary>
        /// 混合输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int inc(int a, out int b)
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
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        private static int add(int a, int b, out int c)
        {
            c = b;
            return a + b;
        }
    }
}
