using System;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// TCP 内部服务端套接字任务处理配置
    /// </summary>
    public sealed class ServerSocketTaskConfig : AutoCSer.Threading.LinkTaskConfigBase
    {
        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount = Math.Max(AutoCSer.Threading.Pub.CpuCount >> 1, 1);
        /// <summary>
        /// 线程切换检测毫秒数量，默认为 10 毫秒
        /// </summary>
        public int NewThreadMilliseconds = 10;
        /// <summary>
        /// 初始化
        /// </summary>
        private void set()
        {
            if (ThreadCount <= 0) ThreadCount = 1;
            set(ThreadCount, NewThreadMilliseconds);
        }

        /// <summary>
        /// TCP 内部服务端套接字任务处理配置
        /// </summary>
        internal static readonly ServerSocketTaskConfig Default;
        static ServerSocketTaskConfig()
        {
            (Default = ConfigLoader.GetUnion(typeof(ServerSocketTaskConfig)).ServerSocketTaskConfig ?? new ServerSocketTaskConfig()).set();
        }
    }
}
