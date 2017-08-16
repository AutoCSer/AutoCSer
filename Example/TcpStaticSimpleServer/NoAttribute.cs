using System;

namespace AutoCSer.Example.TcpStaticSimpleServer
{
    /// <summary>
    /// 无需 TCP 远程函数申明配置 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = ServerName.Example1, IsAttribute = false)]
    partial class NoAttribute
    {
        /// <summary>
        /// 无需 TCP 远程函数申明配置测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpStaticSimpleServer.Method]
        static int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 无需 TCP 远程函数申明配置测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            AutoCSer.Net.TcpServer.ReturnValue<int> sum = AutoCSer.Example.TcpStaticSimpleServer.TcpCallSimple.NoAttribute.Add(2, 3);
            return sum.Type == AutoCSer.Net.TcpServer.ReturnType.Success && sum.Value == 2 + 3;
        }
    }
}
