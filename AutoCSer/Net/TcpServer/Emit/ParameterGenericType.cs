using System;
using System.Reflection;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType
    {
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand"></param>
        /// <param name="onCall"></param>
        /// <returns></returns>
        internal delegate ReturnType WaitCall(CommandInfo identityCommand, ref AutoWaitReturnValue onCall);

        /// <summary>
        /// 获取异步回调
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderWaitGetMethod { get; }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderWaitCallMethod { get; }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderCallOnlyMethod { get; }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderGetMethod { get; }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderGetKeepMethod { get; }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderCallMethod { get; }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal abstract MethodInfo ClientSocketSenderCallKeepMethod { get; }

        /// <summary>
        /// 发送数据
        /// </summary>
        internal abstract MethodInfo ServerSocketSenderPushMethod { get; }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal abstract MethodInfo ServerSocketSenderPushCommandMethod { get; }
    }
}
