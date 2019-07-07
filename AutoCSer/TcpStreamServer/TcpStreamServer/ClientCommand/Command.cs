using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer.ClientCommand
{
    /// <summary>
    /// TCP 客户端命令
    /// </summary>
    internal abstract class Command : TcpServer.ClientCommand.CommandBase
    {
        /// <summary>
        /// TCP 客户端套接字
        /// </summary>
        internal ClientSocket Socket;
        /// <summary>
        /// 释放 TCP 客户端访问锁
        /// </summary>
        internal int FreeLock;
        /// <summary>
        /// 是否创建输出错误
        /// </summary>
        internal bool IsBuildError;
        /// <summary>
        /// 释放 TCP 客户端命令
        /// </summary>
        protected abstract void free();
        /// <summary>
        /// 释放 TCP 客户端命令
        /// </summary>
        internal void Free()
        {
            if ((Interlocked.Increment(ref FreeLock) & 1) == 0) free();
        }
    }
}
