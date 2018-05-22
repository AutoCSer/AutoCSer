using System;
using AutoCSer.Extension;

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
        /// <returns>下一个 SQL 队列任务</returns>
        internal override QueueTask RunTask()
        {
            try
            {
                Action();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
            return LinkNext;
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
        /// <returns>下一个 SQL 队列任务</returns>
        internal override QueueTask RunTask()
        {
            try
            {
                Action(Parameter);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
            return LinkNext;
        }
    }
}
