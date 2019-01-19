using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCallBase : AutoCSer.Threading.ILinkTask
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        public long TaskTicks;
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        long AutoCSer.Threading.ILinkTask.LinkTaskTicks
        {
            get { return TaskTicks; }
            set { TaskTicks = value; }
        }
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        internal AutoCSer.Threading.ILinkTask NextTask;
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        AutoCSer.Threading.ILinkTask AutoCSer.Threading.ILinkTask.NextLinkTask
        {
            get { return NextTask; }
            set { NextTask = value; }
        }
        /// <summary>
        /// 调用处理
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public abstract void Call();
        /// <summary>
        /// 系统线程池调用处理
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void threadPoolCall(object state)
        {
            try
            {
                Call();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// TCP 服务器端同步调用
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerCall SingleRunTask()
        {
            ServerCall value = new UnionType { Value = NextTask }.ServerCall;
            NextTask = null;
            Call();
            return value;
        }
        /// <summary>
        /// TCP 服务器端同步调用
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerCall SingleRunTask(ref long currentTaskTicks)
        {
            ServerCall value = new UnionType { Value = NextTask }.ServerCall;
            currentTaskTicks = TaskTicks;
            NextTask = null;
            Call();
            return value;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        AutoCSer.Threading.ILinkTask AutoCSer.Threading.ILinkTask.SingleRunLinkTask()
        {
            AutoCSer.Threading.ILinkTask value = NextTask;
            NextTask = null;
            Call();
            return value;
        }
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCall : ServerCallBase
    {
        /// <summary>
        /// 回话标识
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public uint CommandIndex;
    }
}
