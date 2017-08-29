using System;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 服务端输出信息
    /// </summary>
    public sealed class OutputInfo
    {
        /// <summary>
        /// 输出参数编号
        /// </summary>
        public int OutputParameterIndex;
        /// <summary>
        /// 是否简单序列化输出参数
        /// </summary>
        public bool IsSimpleSerializeOutputParamter;
    }
}
