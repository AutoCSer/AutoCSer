using System;
using System.Reflection;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// 输入+输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType2
    {
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal abstract MethodInfo ClientGetMethod { get; }
    }
}
