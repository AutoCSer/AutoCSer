using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 捕获异常任务
    /// </summary>
    public abstract class CatchTask : DoubleLink<CatchTask>
    {
        /// <summary>
        /// 异常处理委托
        /// </summary>
        protected readonly Action<AggregateException> onException;
        /// <summary>
        /// 调用文件路径
        /// </summary>
        public readonly string CallerFilePath;
        /// <summary>
        /// 所在文件行数
        /// </summary>
        public readonly int CallerLineNumber;
        /// <summary>
        /// 调用成员名称
        /// </summary>
        public readonly string CallerMemberName;
        /// <summary>
        /// 捕获异常线程
        /// </summary>
        /// <param name="onException">异常处理委托</param>
        /// <param name="callerFilePath">调用文件路径</param>
        /// <param name="callerLineNumber">所在文件行数</param>
        /// <param name="callerMemberName">调用成员名称</param>
        protected CatchTask(Action<AggregateException> onException, string callerFilePath, int callerLineNumber, string callerMemberName)
        {
            CallerLineNumber = callerLineNumber;
            CallerFilePath = callerFilePath;
            CallerMemberName = callerMemberName;

            this.onException = onException;
            TaskLink.PushNotNull(this);
        }

        /// <summary>
        /// 异常回调
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="onException"></param>
        protected static void checkException(AggregateException exception, Action<AggregateException> onException)
        {
            if (exception != null)
            {
                if (onException == null) AutoCSer.LogHelper.Exception(exception, null, LogLevel.Exception | LogLevel.AutoCSer);
                else
                {
                    try
                    {
                        onException(exception);
                    }
                    catch
                    {
                        AutoCSer.LogHelper.Exception(exception, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                }
            }
        }
        /// <summary>
        /// 未释放任务集合
        /// </summary>
        internal static YieldLink TaskLink;
        /// <summary>
        /// 枚举所有未释放任务
        /// </summary>
        public static IEnumerable<CatchTask> Tasks
        {
            get
            {
                CatchTask end = TaskLink.End;
                while (end != null)
                {
                    yield return end;
                    end = end.DoubleLinkPrevious;
                }
            }
        }
        /// <summary>
        /// 添加监视任务
        /// </summary>
        /// <param name="task">线程任务委托</param>
        /// <param name="onException">异常处理委托</param>
        /// <param name="callerFilePath">调用文件路径</param>
        /// <param name="callerLineNumber">所在文件行数</param>
        /// <param name="callerMemberName">调用成员名称</param>
        public static void Add(Task task, Action<AggregateException> onException = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string callerMemberName = null)
        {
            TaskAwaiter taskAwaiter = task.GetAwaiter();
            if (taskAwaiter.IsCompleted) checkException(task.Exception, onException);
            else
            {
                CatchTaskOnly catchTask = new CatchTaskOnly(onException, callerFilePath ?? string.Empty, callerLineNumber, callerMemberName ?? string.Empty, task);
                taskAwaiter.OnCompleted(catchTask.OnCompleted);
            }
        }
        /// <summary>
        /// 添加监视任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">线程任务委托</param>
        /// <param name="onException">异常处理委托</param>
        /// <param name="callerFilePath">调用文件路径</param>
        /// <param name="callerLineNumber">所在文件行数</param>
        /// <param name="callerMemberName">调用成员名称</param>
        public static void Add<T>(Task<T> task, Action<AggregateException> onException = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string callerMemberName = null)
        {
            TaskAwaiter<T> taskAwaiter = task.GetAwaiter();
            if (taskAwaiter.IsCompleted) checkException(task.Exception, onException);
            else
            {
                CatchTask<T> catchTask = new CatchTask<T>(onException, callerFilePath ?? string.Empty, callerLineNumber, callerMemberName ?? string.Empty, task);
                taskAwaiter.OnCompleted(catchTask.OnCompleted);
            }
        }
    }
    /// <summary>
    /// 捕获异常任务
    /// </summary>
    internal sealed class CatchTaskOnly : CatchTask
    {
        /// <summary>
        /// 任务
        /// </summary>
        private readonly Task task;
        /// <summary>
        /// 捕获异常线程
        /// </summary>
        /// <param name="onException">异常处理委托</param>
        /// <param name="callerFilePath">调用文件路径</param>
        /// <param name="callerLineNumber">所在文件行数</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="task">线程任务委托</param>
        internal CatchTaskOnly(Action<AggregateException> onException, string callerFilePath, int callerLineNumber, string callerMemberName, Task task)
            : base(onException, callerFilePath, callerLineNumber, callerMemberName)
        {
            this.task = task;
        }
        /// <summary>
        /// 任务完成检测
        /// </summary>
        internal void OnCompleted()
        {
            TaskAwaiter TaskAwaiter = task.GetAwaiter();
            if (TaskAwaiter.IsCompleted)
            {
                if (TaskLink.PopNotNull(this)) checkException(task.Exception, onException);
            }
            else TaskAwaiter.OnCompleted(OnCompleted);
        }
    }
    /// <summary>
    /// 捕获异常任务
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    internal sealed class CatchTask<T> : CatchTask
    {
        /// <summary>
        /// 任务
        /// </summary>
        private readonly Task<T> task;
        /// <summary>
        /// 捕获异常线程
        /// </summary>
        /// <param name="onException">异常处理委托</param>
        /// <param name="callerFilePath">调用文件路径</param>
        /// <param name="callerLineNumber">所在文件行数</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="task">线程任务委托</param>
        internal CatchTask(Action<AggregateException> onException, string callerFilePath, int callerLineNumber, string callerMemberName, Task<T> task)
            : base(onException, callerFilePath, callerLineNumber, callerMemberName)
        {
            this.task = task;
        }
        /// <summary>
        /// 任务完成检测
        /// </summary>
        internal void OnCompleted()
        {
            TaskAwaiter<T> TaskAwaiter = task.GetAwaiter();
            if (TaskAwaiter.IsCompleted)
            {
                if (TaskLink.PopNotNull(this)) checkException(task.Exception, onException);
            }
            else TaskAwaiter.OnCompleted(OnCompleted);
        }
    }
}
