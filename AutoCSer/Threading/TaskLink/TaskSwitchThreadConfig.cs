using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 超时切换任务线程配置
    /// </summary>
    public sealed class TaskSwitchThreadConfig
    {
        /// <summary>
        /// 线程数量，最小值为 1，默认为 CPU 线程数量 AutoCSer.Common.ProcessorCount
        /// </summary>
        public int ThreadCount;
        /// <summary>
        /// 获取线程数量
        /// </summary>
        internal int GetThreadCount
        {
            get { return ThreadCount <= 0 ? AutoCSer.Common.ProcessorCount : ThreadCount; }
        }
        /// <summary>
        /// 线程切换超时毫秒数，默认最小值为 1
        /// </summary>
        public int SwitchThreadMilliseconds;
        /// <summary>
        /// 获取线程切换超时毫秒数
        /// </summary>
        internal int GetSwitchThreadMilliseconds { get { return Math.Max(SwitchThreadMilliseconds, 1); } }
    }
}
