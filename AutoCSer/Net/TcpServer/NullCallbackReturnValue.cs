using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 异步空回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal sealed class NullCallbackReturnValue<outputParameterType> : Callback<ReturnValue<outputParameterType>>
    {
        /// <summary>
        /// 异步空回调
        /// </summary>
        private NullCallbackReturnValue() { }
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        public override void Call(ref ReturnValue<outputParameterType> outputParameter) { }
        /// <summary>
        /// 异步空回调
        /// </summary>
        internal static readonly NullCallbackReturnValue<outputParameterType> Default = new NullCallbackReturnValue<outputParameterType>();
    }
}
