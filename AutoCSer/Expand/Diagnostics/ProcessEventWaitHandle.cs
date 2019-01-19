using System;
using System.Threading;
using AutoCSer.Extension;
using AutoCSer.Reflection;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程独占锁处理
    /// </summary>
    public static class ProcessEventWaitHandle
    {
        /// <summary>
        /// 申请进程独占锁
        /// </summary>
        /// <param name="LockName"></param>
        /// <returns></returns>
        public static EventWaitHandle TryEnter(string LockName = null)
        {
            if (LockName == null)
            {
                System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (mainAssembly == null) throw new ArgumentNullException("LockName is null");
                LockName = mainAssembly.FullName;
            }
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, LockName, out createdProcessWait);
            if (createdProcessWait) return processWait;
            if (processWait != null) ((IDisposable)processWait).Dispose();
            return null;
        }
        /// <summary>
        /// 申请进程独占锁
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="LockName"></param>
        public static void TryEnter(Action Action, string LockName = null)
        {
            EventWaitHandle WaitHandle = TryEnter(LockName);
            if (WaitHandle != null)
            {
                using (WaitHandle)
                {
                    try
                    {
                        Action();
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                }
            }
        }
    }
}
