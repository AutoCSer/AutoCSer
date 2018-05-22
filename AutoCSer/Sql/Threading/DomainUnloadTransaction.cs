using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 应用程序卸载事件处理
    /// </summary>
    public sealed class DomainUnloadTransaction : QueueTask
    {
        /// <summary>
        /// 任务执行委托
        /// </summary>
        private Action action;
        /// <summary>
        /// 任务执行
        /// </summary>
        private void run()
        {
            try
            {
                action();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
            finally { AutoCSer.DomainUnload.Unloader.TransactionEnd(); }
        }
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <returns>下一个 SQL 队列任务</returns>
        internal override QueueTask RunTask()
        {
            run();
            return LinkNext;
        }
        /// <summary>
        /// 添加 SQL 队列任务
        /// </summary>
        /// <param name="action"></param>
        public static void AddQueueTask(Action action)
        {
            if (action != null)
            {
                DomainUnloadTransaction value = new DomainUnloadTransaction { action = action };
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(true)) TaskQueue.Default.Add(value);
                throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// 添加 SQL 队列任务
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void AddQueueTask<parameterType>(Action<parameterType> action, parameterType parameter)
        {
            AddQueueTask(action, ref parameter);
        }
        /// <summary>
        /// 添加 SQL 队列任务
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">参数</param>
        public static void AddQueueTask<parameterType>(Action<parameterType> action, ref parameterType parameter)
        {
            if (action != null)
            {
                DomainUnloadTransaction<parameterType> value = new DomainUnloadTransaction<parameterType> { Action = action, Parameter = parameter };
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(true)) TaskQueue.Default.Add(value);
                throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action"></param>
        public static void AddTask(Action action)
        {
            if (action != null)
            {
                DomainUnloadTransaction value = new DomainUnloadTransaction { action = action };
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(true)) new Task(value.run).Start();
                throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void AddTask<parameterType>(Action<parameterType> action, parameterType parameter)
        {
            AddTask(action, ref parameter);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action">任务执行委托</param>
        /// <param name="parameter">参数</param>
        public static void AddTask<parameterType>(Action<parameterType> action, ref parameterType parameter)
        {
            if (action != null)
            {
                DomainUnloadTransaction<parameterType> value = new DomainUnloadTransaction<parameterType> { Action = action, Parameter = parameter };
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(true)) new Task(value.Run).Start();
                throw new InvalidOperationException();
            }
        }
    }
    /// <summary>
    /// 应用程序卸载事件处理
    /// </summary>
    /// <typeparam name="parameterType"></typeparam>
    internal sealed class DomainUnloadTransaction<parameterType> : QueueTask
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
        /// 任务执行
        /// </summary>
        internal void Run()
        {
            try
            {
                Action(Parameter);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
            finally { AutoCSer.DomainUnload.Unloader.TransactionEnd(); }
        }
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <returns>下一个 SQL 队列任务</returns>
        internal override QueueTask RunTask()
        {
            Run();
            return LinkNext;
        }
    }
}
