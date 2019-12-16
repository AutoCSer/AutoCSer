using AutoCSer.Net.TcpServer;
using System;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程复制重启服务队列任务
    /// </summary>
    internal sealed class ProcessCopyerServerCall : ServerCallBase
    {
        /// <summary>
        /// 调用类型
        /// </summary>
        internal enum CallType
        {
            /// <summary>
            /// 删除守护进程
            /// </summary>
            Remove,
            /// <summary>
            /// 守护启动进程
            /// </summary>
            Start,
        }
        /// <summary>
        /// 进程复制重启服务
        /// </summary>
        private readonly ProcessCopyServer server;
        /// <summary>
        /// 进程文件复制
        /// </summary>
        private readonly ProcessCopyer processCopyer;
        /// <summary>
        /// 调用类型
        /// </summary>
        private readonly CallType type;
        /// <summary>
        /// 删除进程文件复制
        /// </summary>
        /// <param name="processCopyer">进程文件复制</param>
        /// <param name="type">调用类型</param>
        internal ProcessCopyerServerCall(ProcessCopyer processCopyer, CallType type)
        {
            server = processCopyer.Server;
            this.processCopyer = processCopyer;
            this.type = type;
            processCopyer.Server = null;
        }
        /// <summary>
        /// 删除进程文件复制
        /// </summary>
        public override void RunTask()
        {
            switch (type)
            {
                case CallType.Remove: server.Remove(processCopyer); return;
                case CallType.Start: processCopyer.Start(); return;
            }
        }
    }
}
