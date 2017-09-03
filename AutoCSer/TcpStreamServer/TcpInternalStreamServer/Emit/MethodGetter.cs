using System;
using System.Reflection;
using AutoCSer.Net.TcpStreamServer.Emit;

namespace AutoCSer.Net.TcpInternalStreamServer.Emit
{
    /// <summary>
    /// 获取 TCP 服务函数信息
    /// </summary>
    internal sealed class MethodGetter : Method<ServerAttribute, TcpStreamServer.MethodAttribute, ServerSocketSender>.Getter
    {
        /// <summary>
        /// 获取 TCP 服务函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="serverAttribute"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
        protected override Method<ServerAttribute, TcpStreamServer.MethodAttribute, ServerSocketSender> get(Type type, MethodInfo method, ServerAttribute serverAttribute, TcpStreamServer.MethodAttribute methodAttribute)
        {
            return new Method<ServerAttribute, TcpStreamServer.MethodAttribute, ServerSocketSender>(type, method, serverAttribute, methodAttribute, IsClient);
        }
    }
}
