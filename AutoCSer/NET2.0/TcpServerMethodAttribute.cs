using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 调用函数配置
    /// </summary>
    public partial class MethodAttribute
    {
        /// <summary>
        /// 默认为 false 表示不支持 async Task
        /// </summary>
        internal bool IsClientTaskAsync;
    }
}
