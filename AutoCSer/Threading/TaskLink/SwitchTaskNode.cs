using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 超时切换任务节点接口
    /// </summary>
    public interface ISwitchTaskNode
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        long SwitchTimestamp { get; set; }
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        ISwitchTaskNode NextSwitchTask { get; set; }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next">下一个任务节点</param>
        /// <param name="currentTaskTimestamp">线程切换检测时间</param>
        void RunTask(ref ISwitchTaskNode next, ref long currentTaskTimestamp);
    }
    /// <summary>
    /// 超时切换任务节点接口
    /// </summary>
    /// <typeparam name="T">任务节点类型</typeparam>
    public abstract class SwitchTaskNode<T> : TaskLinkNode<T>, ISwitchTaskNode
        where T : SwitchTaskNode<T>
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long SwitchTaskTimestamp;
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        long ISwitchTaskNode.SwitchTimestamp
        {
            get { return SwitchTaskTimestamp; }
            set { SwitchTaskTimestamp = value; }
        }
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        ISwitchTaskNode ISwitchTaskNode.NextSwitchTask
        {
            get { return LinkNext; }
            set { LinkNext = (T)value; }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next">下一个任务节点</param>
        /// <param name="currentTaskTimestamp">线程切换检测时间</param>
        void ISwitchTaskNode.RunTask(ref ISwitchTaskNode next, ref long currentTaskTimestamp)
        {
            next = LinkNext;
            currentTaskTimestamp = SwitchTaskTimestamp;
            LinkNext = null;
            RunTask();
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        /// <param name="currentTaskTimestamp"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(ref T next, ref long currentTaskTimestamp)
        {
            next = LinkNext;
            currentTaskTimestamp = SwitchTaskTimestamp;
            LinkNext = null;
            RunTask();
        }
    }
}
