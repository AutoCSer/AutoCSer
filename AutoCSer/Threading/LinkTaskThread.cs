﻿using System;
using System.Diagnostics;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表任务线程
    /// </summary>
    /// <typeparam name="T">任务对象类型</typeparam>
    //internal abstract class TaskSwitchThread<T>
        where T : class
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        protected OnceAutoWaitHandle waitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 链表头部
        /// </summary>
        protected T head;
        /// <summary>
        /// 当前处理任务时钟周期
        /// </summary>
        protected long currentTaskTimestamp;
        /// <summary>
        /// 线程切换超时时钟周期
        /// </summary>
        protected long noSwitchTimestamp;
        /// <summary>
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        /// <param name="taskTimestamp">线程切换超时时钟周期</param>
        internal TaskSwitchThread(long taskTimestamp)
        {
            //maxTaskCount = taskCount;
            this.noSwitchTimestamp = currentTaskTimestamp = long.MaxValue - taskTimestamp;
            waitHandle.Set(0);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected abstract void run();

        /// <summary>
        /// 当前任务处理索引
        /// </summary>
        protected static int taskIndex;
        /// <summary>
        /// 是否已经创建所有任务
        /// </summary>
        protected static bool isAllTask;
    }
}
