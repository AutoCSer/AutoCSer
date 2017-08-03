using System;

namespace AutoCSer.TestCase.TcpInternalServer.Emit
{
    /// <summary>
    /// TCP 服务接口
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpInternalServer_Emit, IsServer = true)]
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = (int)ServerPort.TcpOpenServer_Emit)]
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
        /// 无参数无返回值调用测试（服务端同步）
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        void IncSynchronous();
        /// <summary>
        /// 无参数无返回值调用测试（服务端同步）
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.ReturnValue IncLongSynchronous();
        /// <summary>
        /// 多参数无返回值调用测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        void AddSynchronous(int a, int b);
        /// <summary>
        /// 多参数无返回值调用测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.ReturnValue AddSynchronous(long a, long b);
        /// <summary>
        /// 输出参数测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.ReturnValue<long> GetAddSynchronous(long a, ref long b, out long c);
        /// <summary>
        /// 输出参数测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        [AutoCSer.Net.TcpOpenServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.ReturnValue<string> GetAddSynchronous(string a, ref string b, out string c);

        /// <summary>
        /// 无参数无返回值调用测试（异步）
        /// </summary>
        void IncLong(Func<AutoCSer.Net.TcpServer.ReturnValue, bool> onReturn);
        /// <summary>
        /// 多参数无返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        void Add(long a, long b, Func<AutoCSer.Net.TcpServer.ReturnValue, bool> onReturn);
        /// <summary>
        /// 多参数有返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        void GetAdd(int a, int b, Func<AutoCSer.Net.TcpServer.ReturnValue<Sum>, bool> onReturn);
        /// <summary>
        /// 多参数有返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        void GetAdd(long a, long b, Func<AutoCSer.Net.TcpServer.ReturnValue<long>, bool> onReturn);
        /// <summary>
        /// 多参数有返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        void GetAdd(string a, string b, Func<AutoCSer.Net.TcpServer.ReturnValue<string>, bool> onReturn);

        /// <summary>
        /// 无参数无返回值调用测试（异步回调保持）
        /// </summary>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.KeepCallback IncLongKeepCallback(Func<AutoCSer.Net.TcpServer.ReturnValue, bool> onReturn);
        /// <summary>
        /// 多参数有返回值调用测试（异步回调保持）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        AutoCSer.Net.TcpServer.KeepCallback GetAddKeepCallback(int a, int b, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onReturn);

        /// <summary>
        /// 过期函数测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(IsExpired = true)]
        [AutoCSer.Net.TcpOpenServer.Method(IsExpired = true)]
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
