using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// SQL 队列任务
    /// </summary>
    public abstract class QueueTask : AutoCSer.Threading.TaskLinkNode
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action"></param>
        public static void Add(Action action)
        {
            if (action != null) new QueueTaskAction { Action = action }.AddBackgroundQueueTaskLinkThread();
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
            if (action != null) new QueueTaskAction<parameterType> { Action = action, Parameter = parameter }.AddBackgroundQueueTaskLinkThread();
        }

        /// <summary>
        /// 添加 SQL 队列任务
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddQueueTaskLinkThread()
        {
            queueTaskLinkThread.Add(this);
        }
        /// <summary>
        /// 添加 SQL 队列任务
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddBackgroundQueueTaskLinkThread()
        {
            backgroundQueueTaskLinkThread.Add(this);
        }
        /// <summary>
        /// SQL 队列处理
        /// </summary>
        private static readonly AutoCSer.Threading.TaskQueueThread queueTaskLinkThread = new AutoCSer.Threading.TaskQueueThread(false);
        /// <summary>
        /// SQL 队列处理
        /// </summary>
        private static readonly AutoCSer.Threading.TaskQueueThread backgroundQueueTaskLinkThread = new AutoCSer.Threading.TaskQueueThread(true);
    }
}
