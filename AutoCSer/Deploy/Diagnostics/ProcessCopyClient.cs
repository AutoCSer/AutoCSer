using System;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程复制重启服务客户端
    /// </summary>
    public static partial class ProcessCopyClient
    {
        /// <summary>
        /// 守护进程删除客户端调用
        /// </summary>
        public static void Remove()
        {
            isGuard = false;
#if NoAutoCSer
            throw new Exception();
#else
            try
            {
                client.remove(ProcessCopyer.Default);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.AutoCSer, error);
            }
#endif
        }
        /// <summary>
        /// 进程复制重启客户端调用
        /// </summary>
        /// <returns>是否成功</returns>
        internal static bool Copy()
        {
#if NoAutoCSer
            throw new Exception();
#else
            try
            {
                client.copy(ProcessCopyer.Default);
                return true;
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.AutoCSer, error);
            }
#endif
            return false;
        }
        /// <summary>
        /// 守护进程客户端调用
        /// </summary>
        public static void Guard()
        {
            isGuard = true;
#if NoAutoCSer
            throw new Exception();
#else
            guard();
#endif
        }
#if !NoAutoCSer
        /// <summary>
        /// 守护进程客户端调用
        /// </summary>
        private static void guard()
        {
            try
            {
                if (client.guard(ProcessCopyer.Default).Type == AutoCSer.Net.TcpServer.ReturnType.Success) return;
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, "守护进程客户端调用失败");
            callGuardTask();
        }
        /// <summary>
        /// 守护进程客户端调用
        /// </summary>
        private static void callGuard()
        {
            if (isGuard)
            {
                try
                {
                    if (client.guard(ProcessCopyer.Default).Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                    {
                        AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Info, "守护进程客户端调用成功");
                        return;
                    }
                }
                catch { }
                AutoCSer.Threading.TimerTask.Default.Add(callGuard, Date.NowTime.Now.AddMinutes(1));
            }
            else Interlocked.Exchange(ref isGuardTask, 0);
        }
        /// <summary>
        /// 守护任务调用
        /// </summary>
        private static void callGuardTask()
        {
            if (Interlocked.CompareExchange(ref isGuardTask, 1, 0) == 0)
            {
                AutoCSer.Threading.TimerTask.Default.Add(callGuard, Date.NowTime.Now.AddMinutes(1));
            }
        }
        /// <summary>
        /// 是否守护任务调用状态
        /// </summary>
        private static int isGuardTask;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private static void onNewSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            if (parameter.Type == AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket && isGuard) guard();
        }
        /// <summary>
        /// 进程复制重启服务客户端
        /// </summary>
        private static readonly ProcessCopyServer.TcpInternalClient client = new ProcessCopyServer.TcpInternalClient();
#endif
        /// <summary>
        /// 是否守护状态
        /// </summary>
        private static bool isGuard;

        static ProcessCopyClient()
        {
#if !NoAutoCSer
            client._TcpClient_.CreateCheckSocketVersion(onNewSocket);
#endif
        }
    }
}
