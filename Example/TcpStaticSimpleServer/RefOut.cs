﻿using System;

namespace AutoCSer.Example.TcpStaticSimpleServer
{
    /// <summary>
    /// ref / out 参数测试 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = ServerName.Example1, Host = "127.0.0.1", Port = 13200, IsServer = true, IsSegmentation = true, IsRemoteExpression = true)]
    partial class RefOut
    {
        /// <summary>
        /// ref / out 参数测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="product">乘积</param>
        /// <returns>和</returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        static int Add1(int left, ref int right, out int product)
        {
            product = left * right;
            int sum = left + right;
            right <<= 1;
            return sum;
        }
        /// <summary>
        /// ref / out 参数测试
        /// </summary>
        /// <param name="left">加法左值</param>
        /// <param name="right">加法右值</param>
        /// <param name="product">乘积</param>
        /// <returns>和</returns>
        [AutoCSer.Net.TcpStaticSimpleServer.Method(ServerName = ServerName.Example2)]
        static int Add2(int left, ref int right, out int product)
        {
            product = left * right;
            int sum = left + right;
            right <<= 2;
            return sum;
        }

        /// <summary>
        /// ref / out 参数 测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            int right = 3, product;
            AutoCSer.Net.TcpServer.ReturnValue<int> sum = AutoCSer.Example.TcpStaticSimpleServer.TcpCallSimple.RefOut.Add1(2, ref right, out product);
            if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3 || right != (3 << 1) || product != 2 * 3)
            {
                return false;
            }

            right = 3;
            product = 0;
            sum = AutoCSer.Example.TcpStaticSimpleServer.TcpCallSimple.RefOut.Add2(2, ref right, out product);
            if (sum.Type != AutoCSer.Net.TcpServer.ReturnType.Success || sum.Value != 2 + 3 || right != (3 << 2) || product != 2 * 3)
            {
                return false;
            }

            return true;
        }
    }
}
