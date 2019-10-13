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
            //IndexIdentity clientId = new IndexIdentity();
            ClientObject client = DeployInfo.Client;
            Log log = new Log { Identity = Identity };
            bool isLog = true;
            try
            {
                //Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> onLog = Server.GetLog(ref Identity, ref clientId) ?? Timer.onLog;
                AutoCSer.Net.TcpServer.ServerCallback<Log> onLog = client.OnLog ?? AutoCSer.Net.TcpServer.ServerCallback<Log>.Null.Default;
                log.Type = LogType.CreateBakDirectory;
                isLog |= onLog.Callback(log);
                (BakDirectory = new DirectoryInfo(Date.NowTime.Set().ToString("yyyyMMddHHmmss_" + Identity.Index.toString() + "_" + Identity.Identity.toString()))).Create();
                log.Type = LogType.OnCreateBakDirectory;
                isLog |= onLog.Callback(log);
                while (TaskIndex != DeployInfo.Tasks.Length && !IsCancel)
                {
                    Task task = DeployInfo.Tasks.Array[TaskIndex];
                    log.Set(TaskIndex, task.Type);
                    isLog |= onLog.Callback(log);
                    DeployState state = task.Run(this);
                    log.Type = LogType.OnRun;
                    isLog |= onLog.Callback(log);
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
                //if (!(isLog |= onLog(log))) Server.ClearClient(ref clientId);
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
    }
}
