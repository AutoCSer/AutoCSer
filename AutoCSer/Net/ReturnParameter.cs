using System;

namespace AutoCSer.Net
{
#if NOJIT
    /// <summary>
    /// 返回参数
    /// </summary>
    public interface IReturnParameter
    {
        /// <summary>
        /// 返回值
        /// </summary>
        object ReturnObject { get; set; }
    }
#else
    /// <summary>
    /// 返回参数
    /// </summary>
    /// <typeparam name="valueType">返回参数类型</typeparam>
    public interface IReturnParameter<valueType>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        valueType Return { get; set; }
    }
#endif
}
