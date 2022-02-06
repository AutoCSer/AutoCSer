using System;
using AutoCSer.Threading;
using System.Reflection;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType : AutoCSer.Metadata.GenericTypeBase
    {
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientCallMethod { get; }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal abstract MethodInfo ClientGetMethod { get; }

        /// <summary>
        /// 发送数据
        /// </summary>
        internal abstract MethodInfo ServerSocketSendParameterMethod { get; }
        /// <summary>
        /// 反序列化
        /// </summary>
        internal abstract MethodInfo ServerSocketDeSerializeMethod { get; }
    }
}
