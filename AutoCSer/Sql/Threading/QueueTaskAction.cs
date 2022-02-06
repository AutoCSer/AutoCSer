using System;
using AutoCSer.Extensions;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 任务信息
    /// </summary>
    internal sealed class QueueTaskAction : QueueTask
    {
        /// <summary>
        /// 任务执行委托
        /// </summary>
        internal Action Action;
        /// <summary>
        /// 运行任务
        /// </summary>
        public override void RunTask()
        {
            Action();
        }
    }
    /// <summary>
    /// 任务信息
    /// </summary>
    /// <typeparam name="parameterType"></typeparam>
    internal sealed class QueueTaskAction<parameterType> : QueueTask
    {
        /// <summary>
        /// 任务执行委托
        /// </summary>
        internal Action<parameterType> Action;
        /// <summary>
        /// 参数
        /// </summary>
        internal parameterType Parameter;
        /// <summary>
        /// 运行任务
        /// </summary>
        public override void RunTask()
        {
            Action(Parameter);
        }
    }
}
