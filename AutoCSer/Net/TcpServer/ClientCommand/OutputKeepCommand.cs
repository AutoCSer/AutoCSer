﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal abstract class OutputKeepCommandBase<outputParameterType> : KeepCommand
        where outputParameterType : struct
    {
        /// <summary>
        /// 输出参数集合
        /// </summary>
        private LeftArray<ReturnValue<outputParameterType>> outputParameters = new LeftArray<ReturnValue<outputParameterType>>(0);
        /// <summary>
        /// 输出参数访问锁
        /// </summary>
        internal AutoCSer.Threading.SleepFlagSpinLock OutputLock;
        /// <summary>
        /// 输出参数
        /// </summary>
        internal outputParameterType OutputParameter;
        /// <summary>
        /// 是否正在输出
        /// </summary>
        private int isOutput;
        /// <summary>
        /// 异步回调
        /// </summary>
        internal Callback<ReturnValue<outputParameterType>> Callback;
        /// <summary>
        /// 当前输出参数集合
        /// </summary>
        private LeftArray<ReturnValue<outputParameterType>> currentOutputParameters = new LeftArray<ReturnValue<outputParameterType>>(0);
        ///// <summary>
        ///// 终止保持回调
        ///// </summary>
        ///// <param name="identity">保持回调序号</param>
        //internal override void CancelKeep(int identity)
        //{
        //    if (Interlocked.CompareExchange(ref keepCallbackIdentity, identity + 1, identity) == identity)
        //    {
        //        Callback = null;
        //        Socket.CancelKeep(this);
        //    }
        //}
        /// <summary>
        /// 终止保持回调
        /// </summary>
        /// <param name="commandIndex">命令会话标识</param>
        internal override void CancelKeep(int commandIndex)
        {
            Callback = null;
            Socket.CancelKeep(this, commandIndex);
        }
        /// <summary>
        /// 接收数据反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outputParameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe void onReceive(ref SubArray<byte> data, ref ReturnValue<outputParameterType> outputParameter)
        {
            outputParameter.Type = GetReturnType(ref data);
            if (outputParameter.Type == ReturnType.ClientDeSerializeError)
            {
                outputParameter.Value = OutputParameter;
                if ((CommandInfo.CommandFlags & CommandFlags.JsonSerialize) == 0)
                {
                    if (CommandInfo.SimpleSerializeOutputParamter == 0)
                    {
                        if (Socket.DeSerialize(ref data, ref outputParameter.Value)) outputParameter.Type = ReturnType.Success;
                    }
                    else
                    {
                        fixed (byte* dataFixed = data.GetFixedBuffer())
                        {
                            byte* start = dataFixed + data.Start, end = start + data.Length;
                            if (SimpleSerialize.TypeDeSerializer<outputParameterType>.DeSerialize(start, ref outputParameter.Value, end) == end) outputParameter.Type = ReturnType.Success;
                        }
                    }
                }
                else
                {
                    if (Socket.DeSerializeJson(ref data, ref outputParameter.Value)) outputParameter.Type = ReturnType.Success;
                }
            }
            else if ((byte)outputParameter.Type < (byte)ReturnType.Success) KeepCallback.Cancel();
        }
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        internal override void OnReceive(ref SubArray<byte> data)
        {
            Callback<ReturnValue<outputParameterType>> callback = Callback;
            if (callback != null)
            {
                ReturnValue<outputParameterType> outputParameter = new ReturnValue<outputParameterType>();
                if (CommandInfo.TaskType == ClientTaskType.Synchronous)
                {
                    try
                    {
                        onReceive(ref data, ref outputParameter);
                    }
                    catch (Exception error)
                    {
                        Socket.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    finally { callback.Call(ref outputParameter); }
                }
                else
                {
                    try
                    {
                        onReceive(ref data, ref outputParameter);
                    }
                    catch (Exception error)
                    {
                        outputParameter.Type = ReturnType.ClientException;
                        outputParameter.Value = default(outputParameterType);
                        Socket.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    int isOutput = 1;
                    Exception exception = null;
                    OutputLock.Enter();
                    try
                    {
                        if (outputParameters.FreeCount == 0) OutputLock.SleepFlag = 1;
                        outputParameters.Add(outputParameter);
                        isOutput = this.isOutput;
                        this.isOutput = 1;
                    }
                    catch (Exception error) { exception = error; }
                    finally
                    {
                        OutputLock.ExitSleepFlag();
                        if (exception != null) Socket.Log.Exception(exception, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    if (isOutput == 0)
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
            }
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        private void onReceive()
        {
            Callback<ReturnValue<outputParameterType>> callback = Callback;
            if (callback != null)
            {
                OutputLock.Enter();
                do
                {
                    currentOutputParameters.Exchange(ref outputParameters);
                    OutputLock.Exit();
                    ReturnValue<outputParameterType>[] outputParameterArray = currentOutputParameters.Array;
                    int index = 0, count = currentOutputParameters.Length;
                    do
                    {
                        try
                        {
                            do
                            {
                                callback.Call(ref outputParameterArray[index]);
                            }
                            while (++index != count);
                            break;
                        }
                        catch (Exception error)
                        {
                            Socket.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                        }
                    }
                    while (++index != count);
                    if(AutoCSer.DynamicArray<outputParameterType>.IsClearArray) System.Array.Clear(outputParameterArray, 0, count);
                    currentOutputParameters.Length = 0;

                    OutputLock.Enter();
                    if (outputParameters.Length == 0)
                    {
                        isOutput = 0;
                        OutputLock.Exit();
                        return;
                    }
                }
                while (true);
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
            try
            {
                onReceive();
            }
            catch (Exception error)
            {
                Socket.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
        }
    }
    /// <summary>
    /// 客户端命令
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal sealed class OutputKeepCommand<outputParameterType> : OutputKeepCommandBase<outputParameterType>
        where outputParameterType : struct
    {
        ///// <summary>
        ///// 获取客户端命令
        ///// </summary>
        ///// <param name="socket">TCP客户端命令流处理套接字</param>
        ///// <param name="command">命令信息</param>
        ///// <param name="callback">异步回调</param>
        ///// <param name="outputParameter">输出参数</param>
        ///// <returns>客户端命令</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal bool Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback, ref outputParameterType outputParameter)
        //{
        //    if (callback != null && command.TaskType != ClientTaskType.Synchronous)
        //    {
        //        OutputLock = new object();
        //        CurrentOutputLock = new object();
        //    }
        //    if (socket.NewIndex(this) && SetKeepCallback())
        //    {
        //        Socket = socket;
        //        Callback = callback;
        //        CommandInfo = command;
        //        OutputParameter = outputParameter;
        //        return true;
        //    }
        //    AutoCSer.Threading.RingPool<OutputKeepCommand<outputParameterType>>.Default.PushNotNull(this);
        //    return false;
        //}
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
            KeepCallback = new KeepCallback(this);
            Socket = socket;
            Callback = callback;
            CommandInfo = command;
            OutputParameter = outputParameter;
        }
        ///// <summary>
        ///// 获取客户端命令
        ///// </summary>
        ///// <param name="socket">TCP客户端命令流处理套接字</param>
        ///// <param name="command">命令信息</param>
        ///// <param name="callback">异步回调</param>
        ///// <returns>客户端命令</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal bool Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback)
        //{
        //    if (callback != null && command.TaskType != ClientTaskType.Synchronous)
        //    {
        //        OutputLock = new object();
        //        CurrentOutputLock = new object();
        //    }
        //    if (socket.NewIndex(this) && SetKeepCallback())
        //    {
        //        Socket = socket;
        //        Callback = callback;
        //        CommandInfo = command;
        //        OutputParameter = default(outputParameterType);
        //        return true;
        //    }
        //    AutoCSer.Threading.RingPool<OutputKeepCommand<outputParameterType>>.Default.PushNotNull(this);
        //    return false;
        //}
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="callback">异步回调</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ClientSocket socket, CommandInfo command, Callback<ReturnValue<outputParameterType>> callback)
        {
            KeepCallback = new KeepCallback(this);
            Socket = socket;
            Callback = callback;
            CommandInfo = command;
            OutputParameter = default(outputParameterType);
        }
    }
}
