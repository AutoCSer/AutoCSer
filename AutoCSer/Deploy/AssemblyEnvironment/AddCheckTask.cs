using AutoCSer.Net.TcpServer;
using System;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 添加程序集环境检测任务
    /// </summary>
    internal sealed class AddCheckTask :ServerCallBase
    {
        /// <summary>
        /// 程序集环境检测服务
        /// </summary>
        private readonly CheckServer server;
        /// <summary>
        /// 程序集环境检测任务
        /// </summary>
        private readonly CheckTask task;
        /// <summary>
        /// 程序集环境检测任务编号
        /// </summary>
        internal int TaskId;
        /// <summary>
        /// 添加程序集环境检测任务
        /// </summary>
        /// <param name="server">程序集环境检测服务</param>
        /// <param name="task">程序集环境检测任务</param>
        internal AddCheckTask(CheckServer server, CheckTask task)
        {
            this.server = server;
            this.task = task;
        }
        /// <summary>
        /// 添加程序集环境检测任务
        /// </summary>
        public override void RunTask()
        {
            TaskId = server.Add(task);
            task.WaitHandle.Set();
        }
    }
}
