using System;
using AutoCSer.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeployInfo
    {
        /// <summary>
        /// 部署信息索引编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// 部署服务端标识
        /// </summary>
        internal IndexIdentity ClientId;
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
        /// 设置部署服务端标识
        /// </summary>
        /// <param name="clientId">部署服务端标识</param>
        /// <returns>部署信息索引编号</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Set(ref IndexIdentity clientId)
        {
            ClientId = clientId;
            return Identity;
        }
        /// <summary>
        /// 清除部署信息
        /// </summary>
        internal void Clear()
        {
            if (Timer != null)
            {
                Timer.IsCancel = true;
                Timer = null;
            }
            Files = null;
            Tasks.ClearOnlyLength();
            ++Identity;
        }
        /// <summary>
        /// 清除部署信息
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Clear(int identity)
        {
            if (Identity == identity)
            {
                Clear();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置文件数据源
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool SetFiles(int identity, byte[][] files)
        {
            if (Identity == identity)
            {
                Files = files;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="task"></param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int AddTask(int identity, Task task)
        {
            if (Identity == identity)
            {
                int index = Tasks.Length;
                Tasks.Add(task);
                return index;
            }
            return -1;
        }
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="time"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal DeployState Start(int identity, DateTime time, Timer timer)
        {
            if (Identity == identity)
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
            return DeployState.IdentityError;
        }
        /// <summary>
        /// 获取部署服务端标识
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool GetClientId(int identity, ref IndexIdentity clientId)
        {
            if (Identity == identity)
            {
                clientId = ClientId;
                return true;
            }
            return false;
        }
    }
}
