using AutoCSer.Net.TcpServer;
using System;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 保存进程守护信息集合到缓存文件
    /// </summary>
    internal sealed  class ProcessCopySaveCache : ServerCallBase
    {
        /// <summary>
        /// 进程复制重启服务
        /// </summary>
        private readonly ProcessCopyServer server;
        /// <summary>
        /// 保存进程守护信息集合到缓存文件
        /// </summary>
        /// <param name="server">进程复制重启服务</param>
        internal ProcessCopySaveCache(ProcessCopyServer server)
        {
            this.server = server;
        }
        /// <summary>
        /// 保存进程守护信息集合到缓存文件
        /// </summary>
        public override void RunTask()
        {
            server.SaveCache();
        }
    }
}
