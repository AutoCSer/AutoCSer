using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCallBase : AutoCSer.Threading.TimestampTaskLinkNode<ServerCallBase>
    {
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCall : ServerCallBase
    {
        /// <summary>
        /// 回话标识
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public uint CommandIndex;
    }
}
