using System;

namespace AutoCSer.TestCase.TcpInternalSimpleServer.Emit
{
    /// <summary>
    /// TCP 服务接口
    /// </summary>
    [AutoCSer.Net.TcpInternalSimpleServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpInternalSimpleServer_Emit, IsServer = true)]
    [AutoCSer.Net.TcpOpenSimpleServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpOpenSimpleServer_Emit)]
    public interface IServer
    {
        /// <summary>
        /// 无参数无返回值调用测试
        /// </summary>
        void Inc();
        /// <summary>
        /// 无参数无返回值调用测试
        /// </summary>
        AutoCSer.Net.TcpServer.ReturnValue IncLong();
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        void Set(int a);
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        AutoCSer.Net.TcpServer.ReturnValue Set(long a);
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        AutoCSer.Net.TcpServer.ReturnValue Set(string a);
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        void Add(int a, int b);
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        AutoCSer.Net.TcpServer.ReturnValue Add(long a, long b);
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        AutoCSer.Net.TcpServer.ReturnValue Add(string a, string b);
        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        int GetInc();
        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<long> GetIncLong();
        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<string> GetIncString();
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        int GetAdd(int a);
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<long> GetAdd(long a);
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<string> GetAdd(string a);
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int GetAdd(int a, int b);
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<long> GetAdd(long a, long b);
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<string> GetAdd(string a, string b);
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        int GetAdd(int a, ref int b, out int c);
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<long> GetAdd(long a, ref long b, out long c);
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.ReturnValue<string> GetAdd(string a, ref string b, out string c);

        /// <summary>
        /// 过期函数测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Net.TcpSimpleServer.Method(IsExpired = true)]
        [AutoCSer.Net.TcpOpenSimpleServer.Method(IsExpired = true)]
        AutoCSer.Net.TcpServer.ReturnValue Expired();
        /// <summary>
        /// 服务端错误返回测试
        /// </summary>
        AutoCSer.Net.TcpServer.ReturnValue ServerDeSerializeError();
        /// <summary>
        /// 服务端异常测试
        /// </summary>
        void ThrowException();
        /// <summary>
        /// 服务端异常测试
        /// </summary>
        AutoCSer.Net.TcpServer.ReturnValue ReturnThrowException();
    }
}
