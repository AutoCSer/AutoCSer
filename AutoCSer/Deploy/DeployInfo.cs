using System;
using AutoCSer.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal sealed class DeployInfo
    {
        /// <summary>
        /// 客户端信息
        /// </summary>
        internal readonly ClientObject Client;
        /// <summary>
        /// 发布编号
        /// </summary>
        internal readonly int Index;
        /// <summary>
        /// 文件数据源
        /// </summary>
        internal byte[][] Files;
        /// <summary>
        /// 启动的定时任务
        /// </summary>
        internal Timer Timer;
        /// <summary>
        /// 任务集合
        /// </summary>
        internal LeftArray<Task> Tasks;
        /// <summary>
        /// 部署信息
        /// </summary>
        /// <param name="client">客户端信息</param>
        /// <param name="index">发布编号</param>
        internal DeployInfo(ClientObject client, int index)
        {
            Client = client;
            Index = index;
        }
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal DeployState Start(DateTime time, Timer timer)
        {
            if (Tasks.Length != 0 && Timer == null)
            {
                (Timer = timer).DeployInfo = this;
                if (time == default(DateTime)) return timer.Start();
                AutoCSer.Threading.TimerTask.Default.Add(timer.StartTimer, time);
                return DeployState.Success;
            }
            return DeployState.Canceled;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int AddTask(Task task)
        {
            int index = Tasks.Length;
            Tasks.Add(task);
            return index;
        }
    }
}
