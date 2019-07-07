using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 队列任务线程
    /// </summary>
    /// <typeparam name="taskType">任务对象类型</typeparam>
    internal abstract class QueueTaskThread<taskType>
        where taskType : class
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        protected AutoCSer.Threading.Thread.AutoWaitHandle waitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 队列头部
        /// </summary>
        protected taskType head;
        /// <summary>
        /// 队列尾部
        /// </summary>
        protected taskType end;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        protected int queueLock;
        /// <summary>
        /// 队列任务线程
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        internal QueueTaskThread(bool isBackground = true)
        {
            waitHandle.Set(0);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            if (isBackground) threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected abstract void run();
    }
}
