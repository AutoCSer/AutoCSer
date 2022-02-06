using System;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端任务处理线程集合
    /// </summary>
    internal sealed class ClientCallThreadArray : AutoCSer.Threading.TaskSwitchThreadArray<ClientCallThread, ClientCommand.CommandBase>
    {
        /// <summary>
        /// TCP 客户端任务处理线程集合
        /// </summary>
        /// <param name="config"></param>
        private ClientCallThreadArray(AutoCSer.Threading.TaskSwitchThreadConfig config = null) : base(config ?? DefaultConfig) { }
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <returns></returns>
        public override ClientCallThread CreateThread() { return new ClientCallThread(this); }

        /// <summary>
        /// TCP 客户端任务处理线程集合
        /// </summary>
        internal static readonly ClientCallThreadArray Default = (ClientCallThreadArray)AutoCSer.Configuration.Common.Get(typeof(ClientCallThreadArray)) ?? new ClientCallThreadArray((AutoCSer.Threading.TaskSwitchThreadConfig)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Threading.TaskSwitchThreadConfig), "ClientCallThread"));
    }
}
