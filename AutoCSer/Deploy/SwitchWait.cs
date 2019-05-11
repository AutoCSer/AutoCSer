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
        private EventWaitHandle WaitHandle;
        /// <summary>
        /// 关闭处理
        /// </summary>
        private Action Exit;
        /// <summary>
        /// 切换进程等待关闭处理
        /// </summary>
        /// <param name="exit">关闭处理</param>
        public SwitchWait(Action exit)
        {
            Exit = exit;
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(wait);
        }
        /// <summary>
        /// 等待关闭处理
        /// </summary>
        private void wait()
        {
            bool createdProcessWait;
            WaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, Assembly.GetEntryAssembly().FullName, out createdProcessWait);
            if (!createdProcessWait)
            {
                WaitHandle.Set();
                Thread.Sleep(1000);
            }
            WaitHandle.Reset();
            WaitHandle.WaitOne();
            Exit();
        }
        /// <summary>
        /// 设置切换进程等待关闭处理
        /// </summary>
        public static void Set()
        {
            using (EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, Assembly.GetEntryAssembly().FullName))
            {
                waitHandle.Set();
            }
        }
    }
}
