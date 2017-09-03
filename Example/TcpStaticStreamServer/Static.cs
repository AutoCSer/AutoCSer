using System;

namespace AutoCSer.Example.TcpStaticStreamServer
{
    /// <summary>
    /// 支持公共函数 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticStreamServer.Server(Name = ServerName.Example1, IsAttribute = false, MemberFilters = AutoCSer.Metadata.MemberFilters.Static)]
    partial class Static
    {
        /// <summary>
        /// 支持公共函数
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <returns></returns>
        //[AutoCSer.Net.TcpStaticStreamServer.Method]
        public static int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 支持公共函数测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            AutoCSer.Net.TcpServer.ReturnValue<int> sum = AutoCSer.Example.TcpStaticStreamServer.TcpCallStream.Static.Add(2, 3);
            if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }

            #region Awaiter.Wait()
            sum = AutoCSer.Example.TcpStaticStreamServer.TcpCallStream.Static.AddAwaiter(2, 3).Wait().Result;
            if (sum.Type != Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}
