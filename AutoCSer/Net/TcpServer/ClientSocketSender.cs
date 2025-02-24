﻿using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端套接字数据发送
    /// </summary>
    public abstract partial class ClientSocketSender : ClientSocketSenderBase
    {
        /// <summary>
        /// 远程表达式客户端检测服务端映射标识
        /// </summary>
        private readonly RemoteExpressionServerNodeIdChecker remoteExpressionServerNodeIdChecker;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        internal readonly ClientSocket ClientSocket;
        /// <summary>
        /// 套接字是否有效
        /// </summary>
        public bool IsSocket
        {
            get { return ClientSocket.Socket == Socket; }
        }
        /// <summary>
        /// TCP 客户端输出信息链表
        /// </summary>
        internal ClientCommand.Command.YieldQueue Outputs;
        ///// <summary>
        ///// 命令总数
        ///// </summary>
        //protected int commandCount;
        ///// <summary>
        ///// 已创建输出的命令总数
        ///// </summary>
        //protected int buildCommandCount;
        ///// <summary>
        ///// 未处理命令数量
        ///// </summary>
        //public int CurrentCommandCount
        //{
        //    get
        //    {
        //        return commandCount - buildCommandCount;
        //    }
        //}
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal ClientSocketSender() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务客户端套接字</param>
        internal ClientSocketSender(ClientSocket socket)
            : base(socket)
        {
            ClientSocket = socket;
            Outputs = new ClientCommand.Command.YieldQueue(new ClientCommand.MergeCommand { Socket = socket, CommandInfo = ClientCommand.KeepCommand.KeepCallbackCommandInfo });

            if (clientCreator.Attribute.IsRemoteExpression) remoteExpressionServerNodeIdChecker = new RemoteExpressionServerNodeIdChecker { Sender = this };
        }

        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public KeepCallback GetKeep<outputParameterType>(CommandInfo identityCommand, ref Callback<ReturnValue<outputParameterType>> callback)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputKeepCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputKeepCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputKeepCommand<outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback);
                push(command);
                callback = null;
                return command.KeepCallback;
            }
            return null;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void Get<outputParameterType>(CommandInfo identityCommand, ref Callback<ReturnValue<outputParameterType>> callback)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback);
                push(command);
                callback = null;
            }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <returns>保持回调</returns>
        public ReturnValue<outputParameterType> WaitGet<outputParameterType>(CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback);
                push(command);
                ReturnValue<outputParameterType> value = callback.Get();
                callback = null;
                return value;
            }
            return new ReturnValue<outputParameterType> { Type = ReturnType.ClientDisposed };
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public KeepCallback GetKeep<inputParameterType, outputParameterType>(CommandInfo identityCommand, ref Callback<ReturnValue<outputParameterType>> callback, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputKeepCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputKeepCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputKeepCommand<inputParameterType, outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref inputParameter);
                push(command);
                callback = null;
                return command.KeepCallback;
            }
            return null;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void Get<inputParameterType, outputParameterType>(CommandInfo identityCommand, ref Callback<ReturnValue<outputParameterType>> callback, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref inputParameter);
                push(command);
                callback = null;
            }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        public ReturnValue<outputParameterType> WaitGet<inputParameterType, outputParameterType>(CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref inputParameter);
                push(command);
                ReturnValue<outputParameterType> value = callback.Get();
                callback = null;
                return value;
            }
            return new ReturnValue<outputParameterType> { Type = ReturnType.ClientDisposed };
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public ReturnType WaitGet<outputParameterType>(CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref outputParameter);
                push(command);
                ReturnType type = callback.Get(out outputParameter);
                callback = null;
                return type;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public ReturnType WaitGet<inputParameterType, outputParameterType>(CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback
            , ref inputParameterType inputParameter, ref outputParameterType outputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref inputParameter, ref outputParameter);
                push(command);
                ReturnType type = callback.Get(out outputParameter);
                callback = null;
                return type;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void Call<inputParameterType>(CommandInfo identityCommand, Action<ReturnValue> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                command.Set(ClientSocket, identityCommand, onCall ?? NullCallback, ref inputParameter);
                push(command);
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public KeepCallback CallKeep<inputParameterType>(CommandInfo identityCommand, Action<ReturnValue> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            ReturnValue returnValue;
            if (IsSocket)
            {
                ClientCommand.InputKeepCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputKeepCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputKeepCommand<inputParameterType>();
                command.Set(ClientSocket, identityCommand, onCall ?? NullCallback, ref inputParameter);
                push(command);
                return command.KeepCallback;
            }
            returnValue.Type = ReturnType.ClientDisposed;
            return null;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void Call(CommandInfo identityCommand, Action<ReturnValue> onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                command.Set(ClientSocket, identityCommand, onCall ?? NullCallback);
                push(command);
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public KeepCallback CallKeep(CommandInfo identityCommand, Action<ReturnValue> onCall)
        {
            ReturnValue returnValue;
            if (IsSocket)
            {
                ClientCommand.CallKeepCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallKeepCommand>.Default.Pop() ?? new ClientCommand.CallKeepCommand();
                command.Set(ClientSocket, identityCommand, onCall ?? NullCallback);
                push(command);
                return command.KeepCallback;
            }
            returnValue.Type = ReturnType.ClientDisposed;
            return null;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public ReturnType WaitCall<inputParameterType>(CommandInfo identityCommand, ref AutoWaitReturnValue onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                command.Set(ClientSocket, identityCommand, onCall.CallbackHandle, ref inputParameter);
                push(command);
                AutoCSer.Net.TcpServer.ReturnType value = onCall.Wait();
                onCall = null;
                return value;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用（用于代码生成编译）
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        internal ReturnType WaitCall<inputParameterType, outputParameterType>(CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public ReturnType WaitCall(CommandInfo identityCommand, ref AutoWaitReturnValue onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                command.Set(ClientSocket, identityCommand, onCall.CallbackHandle);
                push(command);
                AutoCSer.Net.TcpServer.ReturnType value = onCall.Wait();
                onCall = null;
                return value;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="inputParameter">输入参数</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallOnly<inputParameterType>(CommandInfo identityCommand, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.SendOnlyCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.SendOnlyCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.SendOnlyCommand<inputParameterType>();
                command.Set(ClientSocket, identityCommand, ref inputParameter);
                push(command);
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallOnly(CommandInfo identityCommand)
        {
            if (IsSocket)
            {
                ClientCommand.SendOnlyCommand command = AutoCSer.Threading.RingPool<ClientCommand.SendOnlyCommand>.Default.Pop() ?? new ClientCommand.SendOnlyCommand();
                command.Set(ClientSocket, identityCommand);
                push(command);
            }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        public ReturnType GetAwaiter<inputParameterType, outputParameterType>(CommandInfo identityCommand, Callback<ReturnValue<outputParameterType>> callback
            , ref inputParameterType inputParameter, ref outputParameterType outputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref inputParameter, ref outputParameter);
                push(command);
                return ReturnType.Success;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        public ReturnType GetAwaiter<outputParameterType>(CommandInfo identityCommand, Callback<ReturnValue<outputParameterType>> callback, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                command.Set(ClientSocket, identityCommand, callback, ref outputParameter);
                push(command);
                return ReturnType.Success;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        public ReturnType GetAwaiter<inputParameterType>(CommandInfo identityCommand, Awaiter onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                command.Set(ClientSocket, identityCommand, onCall.Call, ref inputParameter);
                push(command);
                return ReturnType.Success;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用（用于代码生成编译）
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        internal ReturnType GetAwaiter<inputParameterType, outputParameterType>(CommandInfo identityCommand, Callback<ReturnValue<outputParameterType>> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <returns>保持回调</returns>
        public ReturnType GetAwaiter(CommandInfo identityCommand, Awaiter onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                command.Set(ClientSocket, identityCommand, onCall.Call);
                push(command);
                return ReturnType.Success;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="command">当前命令</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void push(ClientCommand.Command command)
        {
            if (commandCount - buildCommandCount >= queueCommandSize)
            {
                AutoCSer.Threading.ThreadYield.YieldOnly();
                if (commandCount - buildCommandCount >= queueCommandSize)
                {
                    Thread.Sleep(0);
                    if (commandCount - buildCommandCount >= queueCommandSize) Thread.Sleep(1);
                }
            }
            Interlocked.Increment(ref commandCount);
            if (Outputs.IsPushHead(command)) OutputWaitHandle.Set();
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        internal override bool Check()
        {
            if (IsSocket)
            {
                if (Outputs.IsEmpty && OutputWaitHandle.IsWait != 0)
                {
                    ClientCommand.CheckCommand command = ClientCommand.CheckCommand.Get(ClientSocket);
                    if (command != null)
                    {
                        if (Outputs.TryPushHead(command))
                        {
                            OutputWaitHandle.Set();
                            Interlocked.Increment(ref commandCount);
                        }
                        else AutoCSer.Threading.RingPool<ClientCommand.CheckCommand>.Default.PushNotNull(command);
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CustomData(byte[] data)
        {
            if (IsSocket)
            {
                if (data == null) data = EmptyArray<byte>.Array;
                ClientCommand.CustomDataCommand command = AutoCSer.Threading.RingPool<ClientCommand.CustomDataCommand>.Default.Pop() ?? new ClientCommand.CustomDataCommand(ClientSocket);
                command.Set(ClientSocket, data);
                push(command);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CustomData(ref SubArray<byte> data)
        {
            if (IsSocket)
            {
                ClientCommand.CustomDataCommand command = AutoCSer.Threading.RingPool<ClientCommand.CustomDataCommand>.Default.Pop() ?? new ClientCommand.CustomDataCommand(ClientSocket);
                command.Set(ClientSocket, ref data);
                push(command);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取消保持回调
        /// </summary>
        /// <param name="commandIndex"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CancelKeep(int commandIndex)
        {
            if (IsSocket)
            {
                ClientCommand.CancelKeepCommand command = AutoCSer.Threading.RingPool<ClientCommand.CancelKeepCommand>.Default.Pop() ?? new ClientCommand.CancelKeepCommand();
                command.Set(ClientSocket, commandIndex);
                push(command);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取远程表达式服务端节点标识
        /// </summary>
        /// <param name="types">表达式服务端节点类型集合</param>
        /// <returns>表达式服务端节点标识集合</returns>
        internal ReturnValue<int[]> GetRemoteExpressionNodeId(AutoCSer.Reflection.RemoteType[] types)
        {
            AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ServerNodeIdChecker.Output> callback = AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ServerNodeIdChecker.Output>.Pop();
            try
            {
                if (IsSocket)
                {
                    ClientCommand.InputOutputCommand<RemoteExpression.ServerNodeIdChecker.Input, RemoteExpression.ServerNodeIdChecker.Output> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<RemoteExpression.ServerNodeIdChecker.Input, RemoteExpression.ServerNodeIdChecker.Output>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<RemoteExpression.ServerNodeIdChecker.Input, RemoteExpression.ServerNodeIdChecker.Output>();
                    RemoteExpression.ServerNodeIdChecker.Input inputParameter = new RemoteExpression.ServerNodeIdChecker.Input { Types = types };
                    command.Set(ClientSocket, RemoteExpression.ServerNodeIdChecker.Input.CommandInfo, callback, ref inputParameter);
                    push(command);
                    RemoteExpression.ServerNodeIdChecker.Output outputParameter;
                    ReturnType type = callback.Get(out outputParameter);
                    callback = null;
                    return new ReturnValue<int[]> { Type = type, Value = outputParameter.Return };
                }
                return new ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientDisposed };
            }
            finally
            {
                if (callback != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ServerNodeIdChecker.Output>.PushNotNull(callback);
            }
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <param name="clientNode">客户端远程表达式节点</param>
        /// <returns>返回值类型</returns>
        public ReturnType GetRemoteExpressionClientNode(RemoteExpression.Node node, out RemoteExpression.ClientNode clientNode)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            ReturnType returnType = checker.Check(node);
            clientNode = returnType == ReturnType.Success ? new RemoteExpression.ClientNode { Node = node, Checker = checker, ClientNodeId = node.ClientNodeId } : default(RemoteExpression.ClientNode);
            return returnType;
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>客户端远程表达式节点</returns>
        public RemoteExpression.ClientNode GetRemoteExpressionClientNode(RemoteExpression.Node node)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            if (checker.Check(node) == TcpServer.ReturnType.Success) return new RemoteExpression.ClientNode { Node = node, Checker = checker, ClientNodeId = node.ClientNodeId };
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取客户端远程表达式参数节点
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="node">远程表达式参数节点</param>
        /// <returns>客户端远程表达式参数节点</returns>
        public RemoteExpression.ClientNode<returnType> GetRemoteExpressionClientNodeParameter<returnType>(RemoteExpression.Node<returnType> node)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            if (checker.Check(node) == TcpServer.ReturnType.Success) return new RemoteExpression.ClientNode<returnType> { Node = node, Checker = checker };
            throw new InvalidCastException();
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        private ReturnValue<RemoteExpression.ReturnValue> getRemoteExpression(ref RemoteExpression.ClientNode node)
        {
            AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ReturnValue.Output> callback = AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ReturnValue.Output>.Pop();
            try
            {
                if (IsSocket)
                {
                    ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, RemoteExpression.ReturnValue.Output> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, RemoteExpression.ReturnValue.Output>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, RemoteExpression.ReturnValue.Output>();
                    command.Set(ClientSocket, RemoteExpression.ClientNode.CommandInfo, callback, ref node);
                    push(command);
                    RemoteExpression.ReturnValue.Output outputParameter;
                    ReturnType type = callback.Get(out outputParameter);
                    callback = null;
                    return new ReturnValue<RemoteExpression.ReturnValue> { Type = type, Value = outputParameter.Return };
                }
                return new ReturnValue<RemoteExpression.ReturnValue> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientDisposed };
            }
            finally
            {
                if (callback != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ReturnValue.Output>.PushNotNull(callback);
            }
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnType CallRemoteExpression(RemoteExpression.Node node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                return type == ReturnType.Success ? getRemoteExpression(ref clientNode).Type : type;
            }
            return ReturnType.RemoteExpressionNotSupport;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        public ReturnValue<returnType> GetRemoteExpression<returnType>(RemoteExpression.Node<returnType> node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                if (type == ReturnType.Success)
                {
                    ReturnValue<RemoteExpression.ReturnValue> value = getRemoteExpression(ref clientNode);
                    if (value.Type == ReturnType.Success) return (value.Value as RemoteExpression.ReturnValue<returnType>).Value;
                    return new ReturnValue<returnType> { Type = value.Type };
                }
                return new ReturnValue<returnType> { Type = type };
            }
            return new ReturnValue<returnType> { Type = ReturnType.RemoteExpressionNotSupport };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<RemoteExpression.ReturnValue> GetRemoteExpression(RemoteExpression.ClientNode node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                return node.Checker != remoteExpressionServerNodeIdChecker ? getRemoteExpression(ref node) : new ReturnValue<RemoteExpression.ReturnValue> { Type = ReturnType.RemoteExpressionCheckerError };
            }
            return new ReturnValue<RemoteExpression.ReturnValue> { Type = ReturnType.RemoteExpressionNotSupport };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        private AwaiterBox<RemoteExpression.ReturnValue> getRemoteExpressionAwaiter(ref RemoteExpression.ClientNode node)
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>>();
                AutoCSer.Net.TcpServer.AwaiterBox<RemoteExpression.ReturnValue> awaiter = new AutoCSer.Net.TcpServer.AwaiterBox<RemoteExpression.ReturnValue>();
                AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue> outputParameter = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>);
                command.Set(ClientSocket, RemoteExpression.ClientNode.CommandInfo, awaiter, ref node, ref outputParameter);
                push(command);
                return awaiter;
            }
            return null;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AwaiterBox<RemoteExpression.ReturnValue> GetRemoteExpressionAwaiter(RemoteExpression.Node node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                if (type == ReturnType.Success) return getRemoteExpressionAwaiter(ref clientNode);
            }
            return null;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AwaiterBox<RemoteExpression.ReturnValue> GetRemoteExpressionAwaiter(RemoteExpression.ClientNode node)
        {
            return remoteExpressionServerNodeIdChecker != null && node.Checker != remoteExpressionServerNodeIdChecker ? getRemoteExpressionAwaiter(ref node) : null;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal unsafe void BuildOutput()
        {
            ClientCommand.CommandBase head = null, end = null;
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull), copyBuffer = default(SubBuffer.PoolBufferFull), compressBuffer = default(SubBuffer.PoolBufferFull);
            SenderBuildInfo buildInfo = new SenderBuildInfo { SendBufferSize = clientCreator.CommandClient.SendBufferPool.Size };
            try
            {
                clientCreator.CommandClient.SendBufferPool.Get(ref buffer);
                SubArray<byte> sendData = new SubArray<byte>();
                int bufferLength = buffer.Length, minCompressSize = clientCreator.CommandClient.MinCompressSize, buildCount;
                OutputWaitType outputWaitType = clientCreator.CommandClient.OutputWaitType, currentOutputWaitType;
                SocketError socketError;
                using (UnmanagedStream outputStream = (ClientSocket.OutputSerializer = BinarySerializer.YieldPool.Default.Pop() ?? new BinarySerializer()).SetTcpServer())
                {
                    do
                    {
                        buildInfo.IsNewBuffer = 0;
                        fixed (byte* dataFixed = buffer.GetFixedBuffer())
                        {
                            byte* start = dataFixed + buffer.StartIndex;
                        RESET:
                            currentOutputWaitType = outputWaitType;
                            if (outputStream.Data.Byte != start) outputStream.Reset(start, buffer.Length);
                            buildInfo.Clear();
                            outputStream.Data.CurrentIndex = ClientCommand.Command.StreamStartIndex;
                            WAIT:
                            OutputWaitHandle.Wait();
                            if (isClose || (head = Outputs.GetClear(out end)) == null) return;
                            LOOP:
                            buildCount = 0;
                            do
                            {
                                head = head.Build(ref buildInfo);
                                ++buildCount;
                                if (buildInfo.IsSend != 0)
                                {
                                    addBuildCommandCount(buildCount);
                                    buildCount = 0;
                                    goto SETDATA;
                                }
                            }
                            while (head != null);
                            addBuildCommandCount(buildCount);
                            buildCount = 0;
                            if (!Outputs.IsEmpty) goto WAIT;
                            switch (currentOutputWaitType)
                            {
                                case OutputWaitType.ThreadYield:
                                    AutoCSer.Threading.ThreadYield.YieldOnly();
                                    if (!Outputs.IsEmpty)
                                    {
                                        currentOutputWaitType = OutputWaitType.DontWait;
                                        goto WAIT;
                                    }
                                    break;
                                case OutputWaitType.ThreadSleep:
                                    System.Threading.Thread.Sleep(0);
                                    if (!Outputs.IsEmpty)
                                    {
                                        currentOutputWaitType = OutputWaitType.DontWait;
                                        goto WAIT;
                                    }
                                    break;
                            }
                            if (buildInfo.Count == 0)
                            {
                                currentOutputWaitType = outputWaitType;
                                goto WAIT;
                            }
                        SETDATA:
                            //buildCommandCount += buildInfo.Count;
                            int outputLength = outputStream.Data.CurrentIndex, dataLength = outputLength - ClientCommand.Command.StreamStartIndex, compressionDataSize = 0;
                            if (outputLength <= bufferLength)
                            {
                                if (outputStream.Data.ByteSize != bufferLength)
                                {
                                    AutoCSer.Memory.Common.CopyNotNull(outputStream.Data.Byte + ClientCommand.Command.StreamStartIndex, start + ClientCommand.Command.StreamStartIndex, dataLength);
                                }
                                sendData.Set(buffer.Buffer, buffer.StartIndex + ClientCommand.Command.StreamStartIndex, dataLength);
                            }
                            else
                            {
                                outputStream.Data.GetSubBuffer(ref copyBuffer, ClientCommand.Command.StreamStartIndex);
                                sendData.Set(copyBuffer.Buffer, copyBuffer.StartIndex + ClientCommand.Command.StreamStartIndex, dataLength);
                                if (copyBuffer.Length <= clientCreator.CommandClient.SendBufferMaxSize)
                                {
                                    buffer.Free();
                                    copyBuffer.CopyToClear(ref buffer);
                                    buildInfo.IsNewBuffer = 1;
                                }
                            }
                            if (buildInfo.Count == 1)
                            {
                                if ((dataLength -= (sizeof(uint) + sizeof(int) * 2)) >= minCompressSize)
                                {
                                    SubArray<byte> oldSendData = sendData;
                                    if (AutoCSer.IO.Compression.DeflateCompressor.Get(sendData.Array, sendData.Start + (sizeof(uint) + sizeof(int) * 2), dataLength, ref compressBuffer, ref sendData, sizeof(uint) + sizeof(int) * 3, sizeof(uint) + sizeof(int) * 3))
                                    {
                                        compressionDataSize = sendData.Length;
                                        sendData.MoveStart(-(sizeof(uint) + sizeof(int) * 3));
                                        fixed (byte* sendDataFixed = sendData.GetFixedBuffer(), oldSendDataFixed = oldSendData.GetFixedBuffer())
                                        {
                                            byte* dataStart = sendDataFixed + sendData.Start, oldDataStart = oldSendDataFixed + oldSendData.Start;
                                            *(int*)dataStart = *(int*)oldDataStart;
                                            *(uint*)(dataStart + sizeof(int)) = *(uint*)(oldDataStart + sizeof(uint));
                                            *(int*)(dataStart + sizeof(uint) + sizeof(int)) = -compressionDataSize;
                                            *(int*)(dataStart + (sizeof(uint) + sizeof(int) * 2)) = dataLength;
                                            if (SendMarkData != 0)
                                            {
                                                CommandBuffer.Mark64(dataStart + (sizeof(uint) + sizeof(int) * 3), SendMarkData, (compressionDataSize + 3) & (int.MaxValue - 3));
                                            }
                                        }
                                    }
                                }
                                if (compressionDataSize == 0 && SendMarkData != 0)
                                {
                                    fixed (byte* sendDataFixed = sendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + sendData.Start;
                                        int markSize = sendData.Length - (sizeof(uint) + sizeof(int) * 2 - 3);
                                        if (markSize > 3) CommandBuffer.Mark32(dataStart + (sizeof(uint) + sizeof(int) * 2), SendMarkData, markSize & (int.MaxValue - 3));
                                    }
                                }
                            }
                            else
                            {
                                if (dataLength >= minCompressSize)
                                {
                                    if (AutoCSer.IO.Compression.DeflateCompressor.Get(sendData.Array, sendData.Start, dataLength, ref compressBuffer, ref sendData, sizeof(uint) + sizeof(int) * 2, 0))
                                    {
                                        compressionDataSize = sendData.Length;
                                        sendData.MoveStart(-(sizeof(uint) + sizeof(int) * 2));
                                        fixed (byte* sendDataFixed = sendData.GetFixedBuffer())
                                        {
                                            byte* dataStart = sendDataFixed + sendData.Start;
                                            *(int*)dataStart = Server.MergeCommandIndex;
                                            *(uint*)(dataStart + sizeof(int)) = (uint)-compressionDataSize;
                                            *(int*)(dataStart + (sizeof(uint) + sizeof(int))) = dataLength;
                                            if (SendMarkData != 0) CommandBuffer.Mark32(dataStart + (sizeof(uint) + sizeof(int) * 2), SendMarkData, (compressionDataSize + 3) & (int.MaxValue - 3));
                                        }
                                    }
                                }
                                if (compressionDataSize == 0)
                                {
                                    dataLength = sendData.Length;
                                    sendData.MoveStart(-ClientCommand.Command.StreamStartIndex);
                                    fixed (byte* sendDataFixed = sendData.GetFixedBuffer())
                                    {
                                        byte* dataStart = sendDataFixed + sendData.Start;
                                        *(int*)dataStart = Server.MergeCommandIndex;
                                        *(uint*)(dataStart + sizeof(int)) = (uint)dataLength;
                                        if (SendMarkData != 0) CommandBuffer.Mark64(dataStart + ClientCommand.Command.StreamStartIndex, SendMarkData, (dataLength + 3) & (int.MaxValue - 3));
                                    }
                                }
                            }
                        SEND:
                            if (IsSocket)
                            {
                                int count = Socket.Send(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError);
                                sendData.MoveStart(count);
                                ++OutputWaitHandle.Reserved;
                                if (sendData.Length == 0)
                                {
                                    if (buildInfo.IsNewBuffer == 0)
                                    {
                                        compressBuffer.TryFree();
                                        copyBuffer.Free();
                                        if (head == null) goto RESET;
                                        if (outputStream.Data.Byte != start) outputStream.Reset(start, buffer.Length);
                                        buildInfo.Clear();
                                        outputStream.Data.CurrentIndex = ClientCommand.Command.StreamStartIndex;
                                        currentOutputWaitType = outputWaitType;
                                        goto LOOP;
                                    }
                                    compressBuffer.TryFree();
                                    if (head != null && Outputs.IsPushHead(ref head, end)) OutputWaitHandle.Set();
                                    goto FIXEDEND;
                                }
                                if (socketError == SocketError.Success && count > 0) goto SEND;
                                buildInfo.IsError = true;
                            }
                            buildInfo.IsNewBuffer = 0;
                        FIXEDEND: ;
                        }
                    }
                    while (buildInfo.IsNewBuffer != 0);
                }
            }
            catch (Exception error)
            {
                clientCreator.CommandClient.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                buildInfo.IsError = true;
            }
            finally
            {
                if (buildInfo.IsError) ClientSocket.DisposeSocket();
                buffer.Free();
                copyBuffer.TryFree();
                compressBuffer.TryFree();
                ClientSocket.FreeOutputSerializer();
                if (head == null) head = Outputs.GetClear(out end);
                else Outputs.GetToEndClear(ref end);
                if (ClientSocket.CommandPool != null) head = ClientSocket.CommandPool.Free(head, end, ClientCommand.KeepCommand.CommandPoolIndex);
                if (head != null) ClientCommand.CommandBase.CancelLink(head);
            }
        }
    }
}
