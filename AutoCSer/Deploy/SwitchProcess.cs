using AutoCSer.Extension;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 切换进程
    /// </summary>
    public abstract class SwitchProcess
    {
        /// <summary>
        /// 切换服务锁
        /// </summary>
        public readonly ManualResetEvent ExitEvent;
        /// <summary>
        /// 切换进程等待关闭处理
        /// </summary>
        private readonly AutoCSer.Deploy.SwitchWait switchWait;
        /// <summary>
        /// 是否自动调用守护操作
        /// </summary>
        protected virtual bool autoGuard { get { return true; } }
        /// <summary>
        /// 切换进程（默认规则）
        /// </summary>
        /// <param name="arguments"></param>
        protected SwitchProcess(string[] arguments)
            : this(arguments.Length == 0 ? null : arguments[0], arguments.Length != 0)
        {
        }
        /// <summary>
        /// 切换进程
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="isOnlySet"></param>
        /// <param name="deployServerFileName"></param>
        /// <param name="switchDirectoryName"></param>
        protected SwitchProcess(string arguments, bool isOnlySet = false, string deployServerFileName = null, string switchDirectoryName = Server.DefaultSwitchDirectoryName)
        {
            FileInfo SwitchFile = Server.GetSwitchFile(deployServerFileName, switchDirectoryName);
            if (SwitchFile != null)
            {
                SwitchFile.StartProcessDirectory(arguments);
                return;
            }
            if (isOnlySet) SwitchWait.Set();
            else
            {
                initialize();
                ExitEvent = new ManualResetEvent(false);
                switchWait = new SwitchWait(exit);
            }
        }
        /// <summary>
        /// 初始化操作
        /// </summary>
        protected virtual void initialize() { }
        /// <summary>
        /// 退出
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void exit()
        {
            ExitEvent.Set();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        public void Start()
        {
            if (switchWait != null)
            {
#if !MONO
                Win32.Kernel32.SetErrorMode(Win32.ErrorMode.SEM_NOGPFAULTERRORBOX | Win32.ErrorMode.SEM_NOOPENFILEERRORBOX);
#endif
                onStart();
                if (autoGuard) AutoCSer.Diagnostics.ProcessCopyClient.Guard();
                ExitEvent.WaitOne();
                AutoCSer.Diagnostics.ProcessCopyClient.Remove();
                onExit();
            }
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        protected abstract void onStart();
        /// <summary>
        /// 退出运行
        /// </summary>
        protected virtual void onExit()
        {
            Environment.Exit(0);
        }
    }
}
