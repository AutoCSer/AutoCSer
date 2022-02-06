using System;
using AutoCSer.Extensions;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.ClientCommand
{
    /// <summary>
    /// 客户端命令
    /// </summary>
    internal abstract class CallKeepCommandBase : KeepCommand
    {
        /// <summary>
        /// 输出参数集合
        /// </summary>
        private LeftArray<ReturnType> outputParameters = new LeftArray<ReturnType>(0);
        /// <summary>
        /// 输出参数访问锁
        /// </summary>
        internal AutoCSer.Threading.SleepFlagSpinLock OutputLock;
        /// <summary>
        /// 是否正在输出
        /// </summary>
        private int isOutput;
        /// <summary>
        /// 回调委托
        /// </summary>
        internal Action<ReturnValue> Callback;
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
        /// 当前输出参数集合
        /// </summary>
        private LeftArray<ReturnType> currentOutputParameters = new LeftArray<ReturnType>(0);
        /// <summary>
        /// 接收数据回调处理
        /// </summary>
        /// <param name="data">输出数据</param>
        internal override void OnReceive(ref SubArray<byte> data)
        {
            Action<ReturnValue> callback = Callback;
            if (callback != null)
            {
                ReturnType value = GetReturnType(ref data);
                if ((byte)value < (byte)ReturnType.Success) KeepCallback.Cancel();
                if (CommandInfo.TaskType == ClientTaskType.Synchronous)
                {
                    try
                    {
                        callback(new ReturnValue { Type = value });
                    }
                    catch (Exception error) { Socket.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer); }
                }
                else
                {
                    int isOutput = 1;
                    Exception exception = null;
                    OutputLock.Enter();
                    try
                    {
                        if (outputParameters.FreeCount == 0) OutputLock.SleepFlag = 1;
                        outputParameters.Add(value);
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
            Action<ReturnValue> callback = Callback;
            if (callback != null)
            {
                OutputLock.Enter();
                do
                {
                    currentOutputParameters.Exchange(ref outputParameters);
                    OutputLock.Exit();
                    ReturnType[] outputParameterArray = currentOutputParameters.Array;
                    int index = 0, count = currentOutputParameters.Length;
                    do
                    {
                        try
                        {
                            do
                            {
                                callback(new ReturnValue { Type = outputParameterArray[index] });
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
    internal sealed class CallKeepCommand : CallKeepCommandBase
    {
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="socket">TCP客户端命令流处理套接字</param>
        /// <param name="command">命令信息</param>
        /// <param name="onCall">回调委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(ClientSocket socket, CommandInfo command, Action<ReturnValue> onCall)
        {
            KeepCallback = new KeepCallback(this);
            Socket = socket;
            Callback = onCall;
            CommandInfo = command;
        }
    }
}
