using System;
using AutoCSer.Net;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 定时任务
    /// </summary>
    internal sealed class Timer
    {
        /// <summary>
        /// 部署信息索引标识
        /// </summary>
        internal IndexIdentity Identity;
        /// <summary>
        /// 部署服务
        /// </summary>
        internal Server Server;
        /// <summary>
        /// 备份目录
        /// </summary>
        internal DirectoryInfo BakDirectory;
        /// <summary>
        /// 错误异常
        /// </summary>
        internal Exception Error;
        /// <summary>
        /// 部署信息
        /// </summary>
        internal DeployInfo DeployInfo;
        /// <summary>
        /// 当前任务标识
        /// </summary>
        internal int TaskIndex;
        /// <summary>
        /// 是否已经取消定时任务
        /// </summary>
        internal bool IsCancel;
        /// <summary>
        /// 启动定时任务
        /// </summary>
        internal void StartTimer()
        {
            Start();
        }
        /// <summary>
        /// 启动定时任务
        /// </summary>
        internal DeployState Start()
        {
            if (IsCancel) return DeployState.Canceled;
            IndexIdentity clientId = new IndexIdentity();
            Log log = new Log { Identity = Identity };
            bool isLog = true;
            try
            {
                Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> onLog = Server.GetLog(ref Identity, ref clientId) ?? Timer.onLog;
                log.Type = LogType.CreateBakDirectory;
                isLog |= onLog(log);
                (BakDirectory = new DirectoryInfo(Date.NowTime.Set().ToString("yyyyMMddHHmmss_" + Identity.Index.toString() + "_" + Identity.Identity.toString()))).Create();
                log.Type = LogType.OnCreateBakDirectory;
                isLog |= onLog(log);
                while (TaskIndex != DeployInfo.Tasks.Length && !IsCancel)
                {
                    Task task = DeployInfo.Tasks.Array[TaskIndex];
                    log.Set(TaskIndex, task.Type);
                    isLog |= onLog(log);
                    DeployState state = task.Run(this);
                    log.Type = LogType.OnRun;
                    isLog |= onLog(log);
                    if (state != DeployState.Success) return state;
                    ++TaskIndex;
                }
                return DeployState.Success;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                Error = error;
            }
            finally
            {
                Server.Clear(ref Identity);
                if (Error == null) log.Type = LogType.Completed;
                else
                {
                    AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, Error);
                    log.Type = LogType.Error;
                }
                if (!(isLog |= onLog(log))) Server.ClearClient(ref clientId);
            }
            return DeployState.Exception;
        }
        /// <summary>
        /// 创建备份目录
        /// </summary>
        /// <returns></returns>
        internal DirectoryInfo CreateBakDirectory()
        {
            DirectoryInfo bakDirectory = new DirectoryInfo(BakDirectory.fullName() + TaskIndex.toString());
            bakDirectory.Create();
            return bakDirectory;
        }
        /// <summary>
        /// 模拟部署任务状态更新回调
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private static bool onLog(AutoCSer.Net.TcpServer.ReturnValue<Log> log)
        {
            return true;
        }
    }
}
