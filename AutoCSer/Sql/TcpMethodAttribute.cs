using System;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Sql
{
    /// <summary>
    /// TCP 调用函数配置
    /// </summary>
    public sealed class TcpMethodAttribute : AutoCSer.Net.TcpStaticServer.MethodAttribute
    {
        /// <summary>
        /// 参数设置
        /// </summary>
        internal override ParameterFlags GetParameterFlags
        {
            get { return ParameterFlags.SerializeBox; }
        }
    }
}
