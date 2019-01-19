using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// await 模拟返回值
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
	public struct AwaiterResult<valueType>
	{
        /// <summary>
        /// 返回值
        /// </summary>
        public valueType Result;
	}
}
