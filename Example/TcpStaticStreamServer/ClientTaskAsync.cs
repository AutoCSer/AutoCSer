using System;
using System.Threading;

namespace AutoCSer.Example.TcpStaticStreamServer
{
    /// <summary>
    /// 同步函数客户端 async / await 测试 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticStreamServer.Server(Name = ServerName.Example1)]
    partial class ClientTaskAsync
    {
        /// <summary>
        /// 同步函数客户端 async / await 测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
#if DOTNET2 || DOTNET4
        [AutoCSer.Net.TcpStaticStreamServer.Method]
#else
        [AutoCSer.Net.TcpStaticStreamServer.Method(IsClientTaskAsync = true)]
#endif
        static int Add(int left, int right)
        {
            return left + right;
        }

#if !DOTNET2 && !DOTNET4
        /// <summary>
        /// 同步函数客户端 async / await 测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            #region 同步代理调用
            AutoCSer.Net.TcpServer.ReturnValue<int> sum = AutoCSer.Example.TcpStaticStreamServer.TcpCallStream.ClientTaskAsync.Add(2, 3);
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            #region Awaiter.Wait()
            sum = AutoCSer.Example.TcpStaticStreamServer.TcpCallStream.ClientTaskAsync.AddAwaiter(2, 3).Wait().Result;
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            #region async 同步调用
            sum = AutoCSer.Example.TcpStaticStreamServer.TcpCallStream.ClientTaskAsync.AddAsync(2, 3).Result;
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            return true;
        }
#endif
    }
}

