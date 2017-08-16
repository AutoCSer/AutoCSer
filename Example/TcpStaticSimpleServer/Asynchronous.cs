using System;
using System.Threading;

namespace AutoCSer.Example.TcpStaticSimpleServer
{
    /// <summary>
    /// 异步回调测试 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = ServerName.Example2, Host = "127.0.0.1", Port = 13201, IsServer = true)]
    partial class Asynchronous
    {
        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="onAdd">加法计算回调委托</param>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        static void Add(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onAdd)
        {
            onAdd(left + right);
        }

        /// <summary>
        /// 异步回调测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            AutoCSer.Net.TcpServer.ReturnValue<int> sum = AutoCSer.Example.TcpStaticSimpleServer.TcpCallSimple.Asynchronous.Add(2, 3);
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            return true;
        }
    }
}
