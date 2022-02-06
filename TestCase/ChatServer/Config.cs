using System;
using System.Collections.Generic;

namespace AutoCSer.TestCase.ChatServer
{
    /// <summary>
    /// 服务端项目配置
    /// </summary>
    internal sealed class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// TCP 任务处理配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.Net.TcpServer.ServerCallBase.TaskSwitchThreadConfigName)]
        internal static AutoCSer.Threading.TaskSwitchThreadConfig TcpTaskConfig
        {
            get { return new AutoCSer.Threading.TaskSwitchThreadConfig { ThreadCount = 1 }; }
        }
    }
}
