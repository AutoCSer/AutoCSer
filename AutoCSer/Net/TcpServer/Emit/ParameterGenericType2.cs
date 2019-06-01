using System;
using System.Reflection;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 输入+输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType2
    {
        /// <summary>
        /// 获取异步回调
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderWaitGetMethod { get; }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderGetMethod { get; }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderGetKeepMethod { get; }
    }
}
