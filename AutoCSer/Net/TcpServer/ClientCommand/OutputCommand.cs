using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal abstract class OutputCommandBase<outputParameterType> : Command
        where outputParameterType : struct
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        internal Callback<ReturnValue<outputParameterType>> Callback;
        /// <summary>
        /// 输出参数
        /// </summary>
        internal ReturnValue<outputParameterType> OutputParameter;
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe void onReceive(ref SubArray<byte> data)
        {
            OutputParameter.Type = GetReturnType(ref data);
            if (OutputParameter.Type == ReturnType.ClientDeSerializeError)
            {
                if ((CommandInfo.CommandFlags & CommandFlags.JsonSerialize) == 0)
                {
                    if (CommandInfo.IsSimpleSerializeOutputParamter)
                    {
                        fixed (byte* dataFixed = data.Array)
                        {
                            byte* start = dataFixed + data.Start, end = start + data.Length;
                            if (SimpleSerialize.TypeDeSerializer<outputParameterType>.DeSerialize(start, ref OutputParameter.Value, end) == end) OutputParameter.Type = ReturnType.Success;
                        }
                    }
                    else if (Socket.DeSerialize(ref data, ref OutputParameter.Value)) OutputParameter.Type = ReturnType.Success;
                }
                else
                {
                    if (Socket.ParseJson(ref data, ref OutputParameter.Value)) OutputParameter.Type = ReturnType.Success;
                }
            }
        }
    }
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal sealed class OutputCommand<outputParameterType> : OutputCommandBase<outputParameterType>
        where outputParameterType : struct
    {
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>下一个命令</returns>
        internal unsafe override CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int) + sizeof(uint))
            {
                CommandBase nextBuild = LinkNext;
                int commandIndex = Socket.CommandPool.Push(this);
                if (commandIndex != 0)
                {
                    byte* write = stream.CurrentData;
                    *(int*)write = CommandInfo.Command;
                    *(uint*)(write + sizeof(int)) = (uint)commandIndex | (uint)(CommandInfo.CommandFlags | CommandFlags.NullData);
                    ++buildInfo.Count;
                    LinkNext = null;
                    stream.ByteSize += sizeof(int) + sizeof(uint);
                    return nextBuild;
                }
                LinkNext = null;
                OutputParameter.Type = ReturnType.ClientBuildError;
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
        /// <param name="callback">异步回调</param>
        /// <param name="outputParameter">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback, ref outputParameterType outputParameter)
        {
            Socket = socket;
            Callback = callback;
            CommandInfo = command;
            OutputParameter.Value = outputParameter;
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="callback">异步回调</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback)
        {
            Socket = socket;
            Callback = callback;
            CommandInfo = command;
            OutputParameter.Value = default(outputParameterType);
        }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        internal override void OnReceive(ref SubArray<byte> data)
        {
            try
            {
                onReceive(ref data);
            }
            catch (Exception error)
            {
                Socket.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
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
            Callback<ReturnValue<outputParameterType>> callback = Callback;
            ClientSocket socket = Socket;
            ReturnValue<outputParameterType> outputParameter = OutputParameter;
            Callback = null;
            Socket = null;
            OutputParameter.Value = default(outputParameterType);
            AutoCSer.Threading.RingPool<OutputCommand<outputParameterType>>.Default.PushNotNull(this);
            try
            {
                callback.Call(ref outputParameter);
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
