using System;
using System.Reflection;
using AutoCSer.Net.TcpSimpleServer;
using AutoCSer.Net.TcpSimpleServer.Emit;

namespace AutoCSer.Net.TcpInternalSimpleServer.Emit
{
    /// <summary>
    /// 获取 TCP 服务函数信息
    /// </summary>
    internal sealed class MethodGetter : Method<ServerAttribute, MethodAttribute, ServerSocket>.Getter
    {
        /// <summary>
        /// 获取 TCP 服务函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="serverAttribute"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
        protected override Method<ServerAttribute, MethodAttribute, ServerSocket> get(Type type, MethodInfo method, ServerAttribute serverAttribute, MethodAttribute methodAttribute)
        {
            return new Method<ServerAttribute, MethodAttribute, ServerSocket>(type, method, serverAttribute, methodAttribute, IsClient);
        }
    }
}
