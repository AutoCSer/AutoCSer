using System;
using AutoCSer.Net.TcpStreamServer.Emit;
using System.Reflection;

namespace AutoCSer.Net.TcpOpenStreamServer.Emit
{
    /// <summary>
    /// 获取 TCP 服务函数信息
    /// </summary>
    internal sealed class MethodGetter : Method<ServerAttribute, MethodAttribute, ServerSocketSender>.Getter
    {
        /// <summary>
        /// 获取 TCP 服务函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="serverAttribute"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
        protected override Method<ServerAttribute, MethodAttribute, ServerSocketSender> get(Type type, MethodInfo method, ServerAttribute serverAttribute, MethodAttribute methodAttribute)
        {
            return new Method<ServerAttribute, MethodAttribute, ServerSocketSender>(type, method, serverAttribute, methodAttribute, IsClient);
        }
    }
}
