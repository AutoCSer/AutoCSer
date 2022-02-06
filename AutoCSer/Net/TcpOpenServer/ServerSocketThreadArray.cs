using System;
using System.Threading;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 服务端套接字任务线程集合
    /// </summary>
    internal sealed class ServerSocketThreadArray : AutoCSer.Threading.TaskSwitchThreadArray<ServerSocketThread, ServerSocket>
    {
        /// <summary>
        /// TCP 服务端套接字任务线程集合
        /// </summary>
        /// <param name="config"></param>
        private ServerSocketThreadArray(AutoCSer.Threading.TaskSwitchThreadConfig config = null) : base(config ?? DefaultConfig) { }
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <returns></returns>
        public override ServerSocketThread CreateThread() { return new ServerSocketThread(this); }

        /// <summary>
        /// TCP 服务端套接字任务线程集合
        /// </summary>
        internal static readonly ServerSocketThreadArray Default = (ServerSocketThreadArray)AutoCSer.Configuration.Common.Get(typeof(ServerSocketThreadArray)) ?? new ServerSocketThreadArray((AutoCSer.Threading.TaskSwitchThreadConfig)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Threading.TaskSwitchThreadConfig), "ServerSocketThread"));
    }
}
