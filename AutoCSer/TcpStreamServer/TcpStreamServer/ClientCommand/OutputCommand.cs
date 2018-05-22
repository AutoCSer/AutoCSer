using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer.ClientCommand
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
        internal Callback<TcpServer.ReturnValue<outputParameterType>> Callback;
        /// <summary>
        /// 输出参数
        /// </summary>
        internal TcpServer.ReturnValue<outputParameterType> OutputParameter;
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe void onReceive(ref SubArray<byte> data)
        {
            OutputParameter.Type = TcpServer.ClientCommand.Command.GetReturnType(ref data);
            if (OutputParameter.Type == TcpServer.ReturnType.ClientDeSerializeError)
            {
                if ((CommandInfo.CommandFlags & TcpServer.CommandFlags.JsonSerialize) == 0)
                {
                    if (CommandInfo.IsSimpleSerializeOutputParamter)
                    {
                        fixed (byte* dataFixed = data.Array)
                        {
                            byte* start = dataFixed + data.Start, end = start + data.Length;
                            if (SimpleSerialize.TypeDeSerializer<outputParameterType>.DeSerialize(start, ref OutputParameter.Value, end) == end) OutputParameter.Type = TcpServer.ReturnType.Success;
                        }
                    }
                    else if (Socket.DeSerialize(ref data, ref OutputParameter.Value)) OutputParameter.Type = TcpServer.ReturnType.Success;
                }
                else
                {
                    if (Socket.ParseJson(ref data, ref OutputParameter.Value)) OutputParameter.Type = TcpServer.ReturnType.Success;
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
        internal unsafe override TcpServer.ClientCommand.CommandBase Build(ref TcpServer.SenderBuildInfo buildInfo)
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            if ((buildInfo.SendBufferSize - stream.ByteSize) >= sizeof(int))
            {
                *(int*)stream.CurrentData = CommandInfo.Command | (int)(uint)(CommandInfo.CommandFlags | TcpServer.CommandFlags.NullData);
                stream.ByteSize += sizeof(int);
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
            AutoCSer.Threading.RingPool<OutputCommand<outputParameterType>>.Default.PushNotNull(this);
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="outputParameter">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, TcpServer.CommandInfo command, Callback<TcpServer.ReturnValue<outputParameterType>> callback, ref outputParameterType outputParameter)
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
        internal void Set(ClientSocket socket, TcpServer.CommandInfo command, Callback<TcpServer.ReturnValue<outputParameterType>> callback)
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
            if (CommandInfo.TaskType == TcpServer.ClientTaskType.Synchronous) onReceive();
            else
            {
                switch (CommandInfo.TaskType)
                {
                    case TcpServer.ClientTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolOnReceive)) AutoCSer.Threading.LinkTask.Task.Add(this); return;
                    case TcpServer.ClientTaskType.Timeout: AutoCSer.Threading.LinkTask.Task.Add(this); return;
                    case TcpServer.ClientTaskType.TcpTask: TcpServer.ClientCallTask.Task.Add(this); return;
                    case TcpServer.ClientTaskType.TcpQueue: TcpServer.ClientCallQueue.Default.Add(this); return;
                }
            }
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onReceive()
        {
            Callback<TcpServer.ReturnValue<outputParameterType>> callback = Callback;
            ClientSocket socket = Socket;
            TcpServer.ReturnValue<outputParameterType> outputParameter = OutputParameter;
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
