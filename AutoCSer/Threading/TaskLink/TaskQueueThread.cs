using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列线程
    /// </summary>
    /// <typeparam name="T">任务对象类型</typeparam>
    public abstract class TaskQueueThreadBase<T>
        where T : class
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        internal OnceAutoWaitHandle WaitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        protected readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 队列头部
        /// </summary>
        protected T head;
        /// <summary>
        /// 队列尾部
        /// </summary>
        protected T end;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        internal AutoCSer.Threading.SpinLock QueueLock;
        /// <summary>
        /// 队列任务线程
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        internal TaskQueueThreadBase(bool isBackground = true, bool isStart = true)
        {
            WaitHandle.Set(0);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            if (isBackground) threadHandle.IsBackground = true;
            if (isStart) threadHandle.Start();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected abstract void run();
    }
    /// <summary>
    /// 任务队列线程
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskQueueThread<T> : TaskQueueThreadBase<T>
        where T : TaskLinkNode<T>
    {
        /// <summary>
        /// 队列任务线程
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        public TaskQueueThread(bool isBackground = true, bool isStart = true) : base(isBackground, isStart) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(T value)
        {
            QueueLock.EnterYield();
            if (head == null)
            {
                end = value;
                head = value;
                QueueLock.Exit();
                WaitHandle.Set();
            }
            else
            {
                end.LinkNext = value;
                end = value;
                QueueLock.Exit();
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckAdd(T value)
        {
            QueueLock.EnterYield();
            if (value.LinkNext == null && value != end)
            {
                if (head == null)
                {
                    end = value;
                    head = value;
                    QueueLock.Exit();
                    WaitHandle.Set();
                }
                else
                {
                    end.LinkNext = value;
                    end = value;
                    QueueLock.Exit();
                }
                return true;
            }
            QueueLock.Exit();
            return false;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        internal void Add(T head, T end)
        {
            QueueLock.EnterYield();
            if (this.head == null)
            {
                this.end = end;
                this.head = head;
                QueueLock.Exit();
                WaitHandle.Set();
            }
            else
            {
                this.end.LinkNext = head;
                this.end = end;
                QueueLock.Exit();
            }
        }
        /// <summary>
        /// 消息队列读取操作任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.Wait();
                QueueLock.EnterYield();
                T value = head;
                end = null;
                head = null;
                QueueLock.Exit();
                do
                {
                    try
                    {
                        do
                        {
                            value.RunTask(ref value);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.LogHelper.Exception(error);
                    }
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// 低优先级任务队列链表
        /// </summary>
        public sealed class LowPriorityLink
        {
            /// <summary>
            /// 任务队列
            /// </summary>
            private readonly TaskQueueThread<T> queue;
            /// <summary>
            /// 首节点
            /// </summary>
            private T head;
            /// <summary>
            /// 尾节点
            /// </summary>
            private T end;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private AutoCSer.Threading.SpinLock queueLock;
            /// <summary>
            /// 任务队列链表节点
            /// </summary>
            /// <param name="queue">任务队列</param>
            internal LowPriorityLink(TaskQueueThread<T> queue)
            {
                this.queue = queue;
            }
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <param name="node"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Add(T node)
            {
                if (node != null)
                {
                    queueLock.EnterYield();
                    add(node);
                }
            }
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <param name="node"></param>
            private void add(T node)
            {
                if (head == null)
                {
                    head = end = node;
                    queueLock.Exit();
                    queue.Add(this);
                }
                else
                {
                    end.LinkNext = node;
                    end = node;
                    queueLock.Exit();
                }
            }
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            public bool CheckAdd(T node)
            {
                if (node != null)
                {
                    queueLock.EnterYield();
                    if (node.LinkNext == null)
                    {
                        if (node != end)
                        {
                            add(node);
                            return true;
                        }
                        if (head == null)
                        {
                            head = end = node;
                            queueLock.Exit();
                            queue.Add(this);
                            return true;
                        }
                    }
                    queueLock.Exit();
                }
                return false;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            internal void RunTask()
            {
                T node = head, next = head.LinkNext;
                if (next == null)
                {
                    queueLock.EnterYield();
                    head = next = head.LinkNext;
                    queueLock.Exit();
                    node.LinkNext = null;
                    try
                    {
                        node.RunTask();
                    }
                    finally
                    {
                        if (next != null) queue.Add(this);
                    }
                }
                else
                {
                    head = next;
                    node.LinkNext = null;
                    try
                    {
                        node.RunTask();
                    }
                    finally { queue.Add(this); }
                }
            }
        }
        /// <summary>
        /// 添加低优先级任务队列链表
        /// </summary>
        /// <param name="link"></param>
        public virtual void Add(LowPriorityLink link)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 创建低优先级任务队列链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LowPriorityLink CreateLink()
        {
            return new LowPriorityLink(this);
        }
    }
    /// <summary>
    /// 任务队列线程
    /// </summary>
    public class TaskQueueThread : TaskQueueThread<TaskLinkNode>
    {
        /// <summary>
        /// 队列任务线程
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        public TaskQueueThread(bool isBackground = true, bool isStart = true) : base(isBackground, isStart) { }

        /// <summary>
        /// 创建低优先级任务队列链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public new TaskQueueThreadLowPriorityLink CreateLink()
        {
            return new TaskQueueThreadLowPriorityLink(this);
        }
    }
}
