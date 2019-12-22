using System;
using AutoCSer.Net;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 程序集环境检测服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = CheckServer.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.DeployAssemblyEnvironmentCheck, MinCompressSize = 1024, ClientWaitConnectedMilliseconds = 1000)]
    public partial class CheckServer : AutoCSer.Net.TcpInternalServer.TimeVerifyServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "DeployAssemblyEnvironmentCheck";

        /// <summary>
        /// 程序集环境检测任务集合
        /// </summary>
        private readonly Dictionary<int, CheckTask> tasks = DictionaryCreator.CreateInt<CheckTask>();
        /// <summary>
        /// 获取程序集环境检测任务
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false, ClientWaitConnected = true, ClientTimeoutSeconds = 1)]
        private CheckTask get(long tick, int taskId)
        {
            CheckTask task;
            return tick == AutoCSer.Date.StartTime.Ticks && tasks.TryGetValue(taskId, out task) ? task : null;
        }
        /// <summary>
        /// 设置程序集环境检测结果
        /// </summary>
        /// <param name="result"></param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false, ClientWaitConnected = true, ClientTimeoutSeconds = 1)]
        private void setResult(CheckResult result)
        {
            CheckTask task;
            if (result.Tick == AutoCSer.Date.StartTime.Ticks && tasks.TryGetValue(result.TaskId, out task))
            {
                tasks.Remove(result.TaskId);
                task.Result = result;
                task.WaitHandle.Set();
            }
        }
        /// <summary>
        /// 添加程序集环境检测任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        internal int Add(CheckTask task)
        {
            int taskId = Interlocked.Increment(ref CheckServer.taskId);
            tasks.Add(taskId, task);
            return taskId;
        }

        /// <summary>
        /// 程序集环境检测任务编号
        /// </summary>
        private static int taskId;
    }
}
