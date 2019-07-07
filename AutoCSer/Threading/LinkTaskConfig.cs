using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表任务配置
    /// </summary>
    public abstract class LinkTaskConfigBase
    {
        /// <summary>
        /// 线程切换检测定时器
        /// </summary>
        private Timer timer;
        /// <summary>
        /// 线程切换超时时钟周期
        /// </summary>
        internal long TaskTicks;
        /// <summary>
        /// 是否正在检测线程切换
        /// </summary>
        private int isCheck;
        /// <summary>
        /// 检测线程切换事件
        /// </summary>
        private event Action onCheck;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="threadCount"></param>
        /// <param name="newThreadMilliseconds"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void set(int threadCount, int newThreadMilliseconds)
        {
            if (threadCount != 1)
            {
                onCheck += nullCheck;
                int milliseconds = Math.Max(newThreadMilliseconds, 1);
                TaskTicks = milliseconds * TimeSpan.TicksPerMillisecond;
                timer = new Timer(check, null, milliseconds, milliseconds);
            }
        }
        /// <summary>
        /// 线程切换检测
        /// </summary>
        /// <param name="state"></param>
        private void check(object state)
        {
            if (System.Threading.Interlocked.CompareExchange(ref isCheck, 1, 0) == 0)
            {
                onCheck();
                System.Threading.Interlocked.Exchange(ref isCheck, 0);
            }
        }
        /// <summary>
        /// 检测线程切换事件
        /// </summary>
        /// <param name="onCheck"></param>
        internal void OnCheck(Action onCheck)
        {
            this.onCheck += onCheck;
            this.onCheck -= nullCheck;
        }
        /// <summary>
        /// 判断是否切换线程
        /// </summary>
        /// <param name="currentTaskTicks"></param>
        /// <returns></returns>
        internal bool IsCheck(long currentTaskTicks)
        {
            return currentTaskTicks + TaskTicks <= AutoCSer.Pub.Stopwatch.ElapsedTicks;
        }
        /// <summary>
        /// 线程切换检测
        /// </summary>
        private static void nullCheck() { }
    }
    /// <summary>
    /// 链表任务配置
    /// </summary>
    public sealed class LinkTaskConfig : LinkTaskConfigBase
    {
        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount = AutoCSer.Threading.Pub.CpuCount << 5;
        /// <summary>
        /// 线程切换检测毫秒数量，默认为 10 毫秒
        /// </summary>
        public int NewThreadMilliseconds = 10;
        /// <summary>
        /// 初始化
        /// </summary>
        private void set()
        {
            if (ThreadCount <= 0) ThreadCount = AutoCSer.Threading.Pub.CpuCount << 5;
            set(ThreadCount, NewThreadMilliseconds);
        }

        /// <summary>
        /// 链表任务配置
        /// </summary>
        internal static readonly LinkTaskConfig Default;
        static LinkTaskConfig()
        {
            (Default = ConfigLoader.GetUnion(typeof(LinkTaskConfig)).LinkTaskConfig ?? new LinkTaskConfig()).set();
        }
    }
}
