using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// SQL 队列任务
    /// </summary>
    public abstract class QueueTask : AutoCSer.Threading.Link<QueueTask>
    {
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <returns>下一个 SQL 队列任务</returns>
        internal abstract QueueTask RunTask();
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action"></param>
        public static void Add(Action action)
        {
            if (action != null) TaskQueue.Background.Add(new QueueTaskAction { Action = action });
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Add<parameterType>(Action<parameterType> action, parameterType parameter)
        {
            Add(action, ref parameter);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">参数</param>
        public static void Add<parameterType>(Action<parameterType> action, ref parameterType parameter)
        {
            if (action != null) TaskQueue.Background.Add(new QueueTaskAction<parameterType> { Action = action, Parameter = parameter });
        }
    }
}
