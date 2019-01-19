using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="outputParameterType"></typeparam>
    public abstract class Callback<outputParameterType>
    {
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        public abstract void Call(ref outputParameterType outputParameter);
    }
}
