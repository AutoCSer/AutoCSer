using System;
using AutoCSer.Net;
using System.IO;
using AutoCSer.Extensions;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 定时任务
    /// </summary>
    internal sealed class Timer
    {
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
        internal DeployResultData Start()
        {
            if (IsCancel) return DeployState.Canceled;
            ClientObject client = DeployInfo.Client;
            Log log = new Log { DeployIndex = DeployInfo.Index };
            bool isLog = true;
            try
            {
                AutoCSer.Net.TcpServer.ServerCallback<Log> onLog = client.OnLog ?? AutoCSer.Net.TcpServer.ServerCallback<Log>.Null.Default;
                byte[] data = null;
                while (TaskIndex != DeployInfo.Tasks.Length && !IsCancel)
                {
                    ClientTask.Task task = DeployInfo.Tasks.Array[TaskIndex];
                    log.Set(TaskIndex, task.Type);
                    isLog |= onLog.Callback(log);
                    DeployResultData state = task.Call(this);
                    log.Type = LogType.OnRun;
                    isLog |= onLog.Callback(log);
                    if (state.State != DeployState.Success) return state;
                    if (state.Data != null) data = state.Data;
                    ++TaskIndex;
                }
                return new DeployResultData { State = DeployState.Success, Data = data };
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                Error = error;
            }
            finally
            {
                //Server.Clear(ref Identity);
                if (Error == null) log.Type = LogType.Completed;
                else
                {
                    AutoCSer.LogHelper.Exception(Error, null, LogLevel.Exception | LogLevel.AutoCSer);
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
            if (BakDirectory == null) (BakDirectory = new DirectoryInfo(Server.GetBakDirectoryName(DeployInfo.Index))).Create();
            DirectoryInfo bakDirectory = new DirectoryInfo(BakDirectory.fullName() + TaskIndex.toString());
            bakDirectory.Create();
            return bakDirectory;
        }
    }
}
