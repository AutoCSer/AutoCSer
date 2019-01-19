using System;
using AutoCSer.Extension;
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
        private LeftArray<ReturnType> outputParameters;
        /// <summary>
        /// 输出参数访问锁
        /// </summary>
        internal object OutputLock;
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
        private LeftArray<ReturnType> currentOutputParameters;
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
                    catch (Exception error) { Socket.Log.Add(AutoCSer.Log.LogType.Error, error); }
                }
                else
                {
                    int isOutput = 1;
                    Monitor.Enter(OutputLock);
                    try
                    {
                        outputParameters.Add(value);
                        isOutput = this.isOutput;
                        this.isOutput = 1;
                    }
                    catch (Exception error)
                    {
                        Socket.Log.Add(AutoCSer.Log.LogType.Error, error);
                    }
                    finally { Monitor.Exit(OutputLock); }
                    if (isOutput == 0)
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
                Monitor.Enter(OutputLock);
                do
                {
                    currentOutputParameters.Exchange(ref outputParameters);
                    Monitor.Exit(OutputLock);
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
                            Socket.Log.Add(AutoCSer.Log.LogType.Error, error);
                        }
                    }
                    while (++index != count);
                    currentOutputParameters.Length = 0;

                    Monitor.Enter(OutputLock);
                    if (outputParameters.Length == 0)
                    {
                        isOutput = 0;
                        Monitor.Exit(OutputLock);
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
            onReceive();
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
            if (command.TaskType != ClientTaskType.Synchronous) OutputLock = new object();
            KeepCallback = new KeepCallback(this);
            Socket = socket;
            Callback = onCall;
            CommandInfo = command;
        }
    }
}
