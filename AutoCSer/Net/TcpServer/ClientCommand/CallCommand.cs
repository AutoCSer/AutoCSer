using System;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    internal abstract class CallCommandBase : Command
    {
        /// <summary>
        /// 回调委托
        /// </summary>
        internal Action<ReturnValue> Callback;
        /// <summary>
        /// 任务返回类型
        /// </summary>
        internal ReturnType ReturnType;
    }
    /// <summary>
    /// 客户端命令
    /// </summary>
    internal sealed class CallCommand : CallCommandBase
    {
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>下一个命令</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.Data.CurrentIndex) >= sizeof(int) + sizeof(uint))
            {
                CommandBase nextBuild = LinkNext;
                int commandIndex = Socket.CommandPool.Push(this);
                if (commandIndex != 0)
                {
                    stream.Data.Write(CommandInfo.Command, (uint)commandIndex | (uint)(CommandInfo.CommandFlags | CommandFlags.NullData));
                    ++buildInfo.Count;
                    LinkNext = null;
                    return nextBuild;
                }
                LinkNext = null;
                ReturnType = ReturnType.ClientBuildError;
                setTask();
                return nextBuild;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="onCall">回调委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Action<ReturnValue> onCall)
        {
            Socket = socket;
            Callback = onCall;
            CommandInfo = command;
        }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        internal override void OnReceive(ref SubArray<byte> data)
        {
            ReturnType = GetReturnType(ref data);
            setTask();
        }
        /// <summary>
        /// 设置回调任务
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setTask()
        {
            if (CommandInfo.TaskType == ClientTaskType.Synchronous) onReceive();
            else
            {
                switch (CommandInfo.TaskType)
                {
                    case ClientTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolOnReceive)) AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                    case ClientTaskType.Timeout: AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                    case ClientTaskType.TcpTask: ClientCallThreadArray.Default.CurrentThread.Add(this); return;
                    case ClientTaskType.TcpQueue: ClientCallQueue.Default.Add(this); return;
                }
            }
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onReceive()
        {
            Action<ReturnValue> callback = Callback;
            ClientSocket socket = Socket;
            Callback = null;
            Socket = null;
            AutoCSer.Threading.RingPool<CallCommand>.Default.PushNotNull(this);
            callback(new ReturnValue { Type = ReturnType });
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        internal override void onReceiveTask()
        {
            onReceive();
        }
        /// <summary>
        /// 系统线程池接收数据回调处理
        /// </summary>
        /// <param name="state"></param>
        private void threadPoolOnReceive(object state)
        {
            ClientSocket socket = Socket;
            try
            {
                onReceive();
            }
            catch (Exception error)
            {
                socket.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
        }
    }
}
