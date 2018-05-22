using System;
using System.Threading;
using AutoCSer.Net.TcpServer;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="inputParameterType">输入参数类型</typeparam>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal sealed class InputOutputCommand<inputParameterType, outputParameterType> : OutputCommandBase<outputParameterType>
        where inputParameterType : struct
        where outputParameterType : struct
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
        internal unsafe override TcpServer.ClientCommand.CommandBase Build(ref SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            int streamLength = stream.ByteSize;
            if (streamLength == 0 || (buildInfo.SendBufferSize - stream.ByteSize) >= CommandInfo.MaxDataSize)
            {
                stream.PrepLength(sizeof(int) * 3);
                stream.ByteSize += sizeof(int) * 2;
                if ((CommandInfo.CommandFlags & CommandFlags.JsonSerialize) == 0) Socket.Serialize(CommandInfo, ref InputParameter);
                else Socket.JsonSerialize(ref InputParameter);
                int dataLength = stream.ByteSize - streamLength - sizeof(int) * 2;
                if (dataLength <= Socket.MaxInputSize)
                {
                    ulong markData = Socket.Sender.SendMarkData;
                    byte* write = stream.Data.Byte + streamLength;
                    buildInfo.IsVerifyMethod |= CommandInfo.IsVerifyMethod;
                    *(int*)write = CommandInfo.Command | (int)(uint)CommandInfo.CommandFlags;
                    *(int*)(write + sizeof(int)) = dataLength;
                    if (markData != 0) TcpServer.CommandBase.Mark(write + sizeof(int) * 2, markData, dataLength);
                    CommandInfo.CheckMaxDataSize(Math.Max(dataLength + sizeof(int) * 2, stream.LastPrepSize - streamLength));
                }
                else
                {
                    stream.ByteSize = streamLength;
                    OutputParameter.Type = ReturnType.ClientBuildError;
                    IsBuildError = true;
                    setTask();
                }
                return LinkNext;
            }
            buildInfo.isFullSend = 1;
            return this;
        }
        /// <summary>
        /// 释放 TCP 客户端命令
        /// </summary>
        protected override void free()
        {
            LinkNext = null;
            AutoCSer.Threading.RingPool<InputOutputCommand<inputParameterType, outputParameterType>>.Default.PushNotNull(this);
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        /// <param name="outputParameter">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback, ref inputParameterType inputParameter, ref outputParameterType outputParameter)
        {
            Socket = socket;
            Callback = callback;
            CommandInfo = command;
            InputParameter = inputParameter;
            OutputParameter.Value = outputParameter;
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback, ref inputParameterType inputParameter)
        {
            Socket = socket;
            Callback = callback;
            CommandInfo = command;
            InputParameter = inputParameter;
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
            InputParameter = default(inputParameterType);
            Callback = null;
            Socket = null;
            OutputParameter.Value = default(outputParameterType);
            if ((Interlocked.Increment(ref FreeLock) & 1) == 0) free();
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
