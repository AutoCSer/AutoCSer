using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="inputParameterType">输入参数类型</typeparam>
    internal sealed class InputCommand<inputParameterType> : CallCommandBase
        where inputParameterType : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        internal inputParameterType InputParameter;
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= CommandInfo.MaxDataSize)
            {
                int streamLength = stream.ByteSize;
                stream.PrepLength(sizeof(uint) + sizeof(int) * 3);
                CommandBase nextBuild = LinkNext;
                stream.ByteSize += sizeof(uint) + sizeof(int) * 2;
                if ((CommandInfo.CommandFlags & CommandFlags.JsonSerialize) == 0) Socket.Serialize(CommandInfo, ref InputParameter);
                else Socket.JsonSerialize(ref InputParameter);
                int dataLength = stream.ByteSize - streamLength - (sizeof(int) * 2 + sizeof(uint));
                if (dataLength <= Socket.MaxInputSize)
                {
                    int commandIndex = Socket.CommandPool.Push(this);
                    if (commandIndex != 0)
                    {
                        byte* write = stream.Data.Byte + streamLength;
                        buildInfo.IsVerifyMethod |= CommandInfo.IsVerifyMethod;
                        ++buildInfo.Count;
                        *(int*)write = CommandInfo.Command;
                        *(uint*)(write + sizeof(int)) = (uint)commandIndex | (uint)CommandInfo.CommandFlags;
                        *(int*)(write + (sizeof(uint) + sizeof(int))) = dataLength;
                        CommandInfo.CheckMaxDataSize(Math.Max(dataLength + (sizeof(int) * 2 + sizeof(uint)), stream.LastPrepSize - streamLength));
                        LinkNext = null;
                        return nextBuild;
                    }
                }
                stream.ByteSize = streamLength;
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
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Action<ReturnValue> onCall, ref inputParameterType inputParameter)
        {
            Socket = socket;
            Callback = onCall;
            CommandInfo = command;
            InputParameter = inputParameter;
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
                    case ClientTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolOnReceive)) AutoCSer.Threading.LinkTask.Task.Add(this); return;
                    case ClientTaskType.Timeout: AutoCSer.Threading.LinkTask.Task.Add(this); return;
                    case ClientTaskType.TcpTask: ClientCallTask.Task.Add(this); return;
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
            InputParameter = default(inputParameterType);
            Callback = null;
            Socket = null;
            AutoCSer.Threading.RingPool<InputCommand<inputParameterType>>.Default.PushNotNull(this);
            try
            {
                callback(new ReturnValue { Type = ReturnType });
            }
            catch (Exception error)
            {
                socket.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
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
            onReceive();
        }
    }
}
