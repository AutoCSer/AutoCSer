using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 参数类型
    /// </summary>
    internal sealed class SimpleParameterType
    {
        /// <summary>
        /// 判断是否有效输入参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsInputParameter(ParameterInfo parameter)
        {
            if (parameter.IsOut) return false;
            Type type = parameter.ParameterType;
            return type != typeof(TcpInternalSimpleServer.ServerSocket) && type != typeof(TcpOpenSimpleServer.ServerSocket);
        }
    }
}
