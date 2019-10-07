using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 队列任务线程
    /// </summary>
    /// <typeparam name="taskType">任务对象类型</typeparam>
    public abstract class QueueTaskThread<taskType>
        where taskType : class
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        internal AutoCSer.Threading.Thread.AutoWaitHandle WaitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        protected readonly System.Threading.Thread threadHandle;
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
        /// <param name="isStart">是否启动线程</param>
        internal QueueTaskThread(bool isBackground = true, bool isStart = true)
        {
            WaitHandle.Set(0);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            if (isBackground) threadHandle.IsBackground = true;
            if(isStart) threadHandle.Start();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected abstract void run();
    }
}
