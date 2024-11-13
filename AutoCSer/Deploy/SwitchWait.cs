using System;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 切换进程等待关闭处理
    /// </summary>
    public sealed class SwitchWait
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        private EventWaitHandle waitHandle;
        /// <summary>
        /// 关闭处理
        /// </summary>
        private readonly Action exit;
        /// <summary>
        /// 名称前缀，可用于区分环境版本
        /// </summary>
        private readonly string prefix;
        /// <summary>
        /// 切换进程等待关闭处理
        /// </summary>
        /// <param name="exit">关闭处理</param>
        /// <param name="prefix">名称前缀，可用于区分环境版本</param>
        public SwitchWait(Action exit, string prefix = null)
        {
            this.exit = exit;
            this.prefix = prefix;
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(wait);
        }
        /// <summary>
        /// 等待关闭处理
        /// </summary>
        private void wait()
        {
            bool createdProcessWait;
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, prefix + Assembly.GetEntryAssembly().FullName, out createdProcessWait);
            if (!createdProcessWait)
            {
                waitHandle.Set();
                Thread.Sleep(1000);
            }
            waitHandle.Reset();
            waitHandle.WaitOne();
            exit();
        }
        /// <summary>
        /// 设置切换进程等待关闭处理
        /// </summary>
        /// <param name="prefix">名称前缀，可用于区分环境版本</param>
        public static void Set(string prefix = null)
        {
            using (EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, prefix + Assembly.GetEntryAssembly().FullName))
            {
                waitHandle.Set();
            }
        }
    }
}
