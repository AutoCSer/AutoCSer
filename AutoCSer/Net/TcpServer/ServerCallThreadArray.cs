using System;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务端任务处理线程集合
    /// </summary>
    internal sealed class ServerCallThreadArray : AutoCSer.Threading.TaskSwitchThreadArray<ServerCallThread, ServerCallBase>
    {
        /// <summary>
        /// TCP 服务端任务处理线程集合
        /// </summary>
        /// <param name="config"></param>
        private ServerCallThreadArray(AutoCSer.Threading.TaskSwitchThreadConfig config = null) : base(config ?? DefaultConfig) { }
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <returns></returns>
        public override ServerCallThread CreateThread() { return new ServerCallThread(this); }

        /// <summary>
        /// TCP 服务端任务处理线程集合
        /// </summary>
        internal static readonly ServerCallThreadArray Default = (ServerCallThreadArray)AutoCSer.Configuration.Common.Get(typeof(ServerCallThreadArray)) ?? new ServerCallThreadArray((AutoCSer.Threading.TaskSwitchThreadConfig)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Threading.TaskSwitchThreadConfig), ServerCallBase.TaskSwitchThreadConfigName));
    }
}
