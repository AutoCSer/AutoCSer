using System;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// 远程调用链目标成员配置
    /// </summary>
    public partial class RemoteMemberAttribute
    {
        /// <summary>
        /// 无意义，用于兼容 .NET 4.5 及以上版本
        /// </summary>
        public bool IsAwait = false;
    }
}
