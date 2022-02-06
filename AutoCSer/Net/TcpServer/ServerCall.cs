using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCallBase : AutoCSer.Threading.SwitchTaskNode<ServerCallBase>
    {
        /// <summary>
        /// TCP 服务端任务线程超时切换配置名称
        /// </summary>
        public const string TaskSwitchThreadConfigName = "ServerCallThread";
        /// <summary>
        /// 添加任务队列（不允许添加重复的任务实例，否则可能造成严重后果）
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="server">TCP 服务</param>
        /// <returns>是否添加成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CheckCall(ServerTaskType taskType, ServerBase server = null)
        {
            switch (taskType)
            {
                case ServerTaskType.ThreadPool: return System.Threading.ThreadPool.QueueUserWorkItem(ThreadPoolCall) || AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.CheckAdd(this);
                case ServerTaskType.Timeout: return AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.CheckAdd(this);
                case ServerTaskType.TcpTask: return ServerCallThreadArray.Default.CurrentThread.CheckAdd(this);
                case ServerTaskType.TcpQueue: return ServerCallQueue.Default.CheckAdd(this);
                case ServerTaskType.TcpQueueLink: return ServerCallQueue.DefaultLink.CheckAdd(this);
                case ServerTaskType.Queue: return server.CallQueue.CheckAdd(this);
                case ServerTaskType.QueueLink: return server.CallQueueLink.CheckAdd(this);
            }
            return false;
        }
        /// <summary>
        /// 添加任务队列（不允许添加重复的任务实例，否则可能造成严重后果）
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="callQueueIndex">独占 TCP 服务器端同步调用队列编号</param>
        /// <param name="server">TCP 服务</param>
        /// <returns>是否添加成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CheckCall(ServerTaskType taskType, byte callQueueIndex, ServerBase server)
        {
            switch (taskType)
            {
                case ServerTaskType.Queue: return server.CallQueueArray[callQueueIndex].Key.CheckAdd(this);
                case ServerTaskType.QueueLink: return server.CallQueueArray[callQueueIndex].Value.CheckAdd(this);
            }
            return false;
        }
    }
    /// <summary>
    /// TCP 服务器端同步调用
    /// </summary>
    public abstract class ServerCall : ServerCallBase
    {
        /// <summary>
        /// 会话标识
        /// </summary>
        public uint CommandIndex;

        /// <summary>
        /// 获取会话标识
        /// </summary>
        /// <param name="serverCall"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static uint GetCommandIndex(ServerCall serverCall)
        {
            return serverCall.CommandIndex;
        }
    }
}
