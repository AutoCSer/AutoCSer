using System;
using System.Threading;

namespace AutoCSer.Example.TcpInternalStreamServer
{
    /// <summary>
    /// 同步函数客户端 async / await 测试 示例
    /// </summary>
    [AutoCSer.Net.TcpInternalStreamServer.Server(Host = "127.0.0.1", Port = 13607)]
    partial class ClientTaskAsync
    {
        /// <summary>
        /// 同步函数客户端 async / await 测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
#if DOTNET2 || DOTNET4
        [AutoCSer.Net.TcpStreamServer.Method]
#else
        [AutoCSer.Net.TcpStreamServer.Method(IsClientTaskAsync = true, IsClientAwaiter = true)]
#endif
        int Add(int left, int right)
        {
            return left + right;
        }

#if !DOTNET2 && !DOTNET4
        /// <summary>
        /// 同步函数客户端 async / await 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync.TcpInternalStreamServer server = new AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync.TcpInternalStreamServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync.TcpInternalStreamClient client = new AutoCSer.Example.TcpInternalStreamServer.ClientTaskAsync.TcpInternalStreamClient())
                    {
                        #region 同步代理调用
                        AutoCSer.Net.TcpServer.ReturnValue<int> sum = client.Add(2, 3);
                        if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
                        {
                            return false;
                        }
                        #endregion

                        #region async 同步调用
                        sum = client.AddAsync(2, 3).Result;
                        if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
                        {
                            return false;
                        }
                        #endregion

                        #region Awaiter.Wait()
                        sum = client.AddAwaiter(2, 3).Wait().Result;
                        if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
                        {
                            return false;
                        }
                        #endregion

                        return true;
                    }
                }
            }
            return false;
        }
#endif
    }
}

