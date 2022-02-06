using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 部署客户端任务信息
    /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public abstract TaskType Type { get; }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal abstract DeployResultData Call(Timer timer);
    }
}
