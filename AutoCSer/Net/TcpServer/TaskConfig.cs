using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 任务处理配置
    /// </summary>
    public sealed class TaskConfig : AutoCSer.Threading.LinkTaskConfigBase
    {
        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount = AutoCSer.Threading.Pub.CpuCount;
        /// <summary>
        /// 线程切换检测毫秒数量，默认为 10 毫秒
        /// </summary>
        public int NewThreadMilliseconds = 10;
        /// <summary>
        /// 初始化
        /// </summary>
        private void set()
        {
            if (ThreadCount <= 0) ThreadCount = AutoCSer.Threading.Pub.CpuCount;
            set(ThreadCount, NewThreadMilliseconds);
        }

        /// <summary>
        /// TCP 任务处理配置
        /// </summary>
        internal static readonly TaskConfig Default;
        static TaskConfig()
        {
            (Default = ConfigLoader.GetUnion(typeof(TaskConfig)).TaskConfig ?? new TaskConfig()).set();
        }
    }
}
