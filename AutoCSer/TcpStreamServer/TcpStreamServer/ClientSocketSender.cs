using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// TCP 服务客户端套接字数据发送
    /// </summary>
    public abstract class ClientSocketSender : TcpServer.ClientSocketSenderBase
    {
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
        /// TCP 客户端输出信息队列尾部
        /// </summary>
        internal volatile TcpServer.ClientCommand.CommandBase CommandEnd;
        /// <summary>
        /// 是否有新的 TCP 客户端输出信息
        /// </summary>
        protected volatile int isNewCommand;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        protected int commandQueueLock;
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
            CommandEnd = socket.CommandQueue;
        }
    }
    /// <summary>
    /// TCP 服务客户端套接字数据发送
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    public abstract partial class ClientSocketSender<attributeType> : ClientSocketSender
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// TCP 服务客户端创建器
        /// </summary>
        private readonly TcpServer.ClientSocketCreator<attributeType> clientCreator;
        /// <summary>
        /// 远程表达式客户端检测服务端映射标识
        /// </summary>
        private readonly RemoteExpressionServerNodeIdChecker remoteExpressionServerNodeIdChecker;
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
        internal ClientSocketSender(ClientSocket<attributeType> socket)
            : base(socket)
        {
            clientCreator = socket.ClientCreator;
            if (clientCreator.Attribute.IsRemoteExpression) remoteExpressionServerNodeIdChecker = new RemoteExpressionServerNodeIdChecker { Sender = this };
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        internal override void Check()
        {
            if (IsSocket)
            {
                if (isNewCommand == 0)
                {
                    ClientCommand.CheckCommand command = ClientCommand.CheckCommand.Get(ClientSocket);
                    if (command != null)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref commandQueueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                        if (isNewCommand == 0)
                        {
                            CommandEnd.LinkNext = command;
                            isNewCommand = 1;
                            CommandEnd = command;
                            System.Threading.Interlocked.Exchange(ref commandQueueLock, 0);
                            OutputWaitHandle.Set();
                        }
                        else
                        {
                            System.Threading.Interlocked.Exchange(ref commandQueueLock, 0);
                            AutoCSer.Threading.RingPool<ClientCommand.CheckCommand>.Default.PushNotNull(command);
                        }
                    }
                }
                ClientSocket.CheckTimer.Reset(ClientSocket);
            }
        }
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="command">当前命令</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void push(ClientCommand.Command command)
        {
            //Interlocked.Increment(ref commandCount);
            while (System.Threading.Interlocked.CompareExchange(ref commandQueueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            int isNewCommand = this.isNewCommand;
            CommandEnd.LinkNext = command;
            this.isNewCommand = 1;
            CommandEnd = command;
            System.Threading.Interlocked.Exchange(ref commandQueueLock, 0);
            if (isNewCommand == 0) OutputWaitHandle.Set();
            ClientSocket.ResetCheck();
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
        public TcpServer.ReturnValue<outputParameterType> WaitGet<inputParameterType, outputParameterType>(TcpServer.CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref inputParameter);
                    push(command);
                    TcpServer.ReturnValue<outputParameterType> value = callback.Get();
                    callback = null;
                    return value;
                }
                return new TcpServer.ReturnValue<outputParameterType> { Type = TcpServer.ReturnType.ClientException };
            }
            return new TcpServer.ReturnValue<outputParameterType> { Type = TcpServer.ReturnType.ClientDisposed };
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <returns>保持回调</returns>
        public TcpServer.ReturnValue<outputParameterType> WaitGet<outputParameterType>(TcpServer.CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback);
                    push(command);
                    TcpServer.ReturnValue<outputParameterType> value = callback.Get();
                    callback = null;
                    return value;
                }
                return new TcpServer.ReturnValue<outputParameterType> { Type = TcpServer.ReturnType.ClientException };
            }
            return new TcpServer.ReturnValue<outputParameterType> { Type = TcpServer.ReturnType.ClientDisposed };
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
        public TcpServer.ReturnType WaitGet<inputParameterType, outputParameterType>(TcpServer.CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback
            , ref inputParameterType inputParameter, ref outputParameterType outputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref inputParameter, ref outputParameter);
                    push(command);
                    TcpServer.ReturnType type = callback.Get(out outputParameter);
                    callback = null;
                    return type;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
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
        public TcpServer.ReturnType WaitGet<outputParameterType>(TcpServer.CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> callback, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref outputParameter);
                    push(command);
                    TcpServer.ReturnType type = callback.Get(out outputParameter);
                    callback = null;
                    return type;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
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
        public TcpServer.ReturnType WaitCall<inputParameterType>(TcpServer.CommandInfo identityCommand, ref TcpServer.AutoWaitReturnValue onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall.CallbackHandle, ref inputParameter);
                    push(command);
                    AutoCSer.Net.TcpServer.ReturnType value = onCall.Wait();
                    onCall = null;
                    return value;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <returns>保持回调</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public TcpServer.ReturnType WaitCall(TcpServer.CommandInfo identityCommand, ref TcpServer.AutoWaitReturnValue onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall.CallbackHandle);
                    push(command);
                    AutoCSer.Net.TcpServer.ReturnType value = onCall.Wait();
                    onCall = null;
                    return value;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
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
        internal TcpServer.ReturnType WaitCall<inputParameterType, outputParameterType>(TcpServer.CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<outputParameterType> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="inputParameter">输入参数</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallOnly<inputParameterType>(TcpServer.CommandInfo identityCommand, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.SendOnlyCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.SendOnlyCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.SendOnlyCommand<inputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, ref inputParameter);
                    push(command);
                }
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallOnly(TcpServer.CommandInfo identityCommand)
        {
            if (IsSocket)
            {
                ClientCommand.SendOnlyCommand command = AutoCSer.Threading.RingPool<ClientCommand.SendOnlyCommand>.Default.Pop() ?? new ClientCommand.SendOnlyCommand();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand);
                    push(command);
                }
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
        public TcpServer.ReturnType GetAwaiter<inputParameterType, outputParameterType>(TcpServer.CommandInfo identityCommand, Callback<TcpServer.ReturnValue<outputParameterType>> callback
            , ref inputParameterType inputParameter, ref outputParameterType outputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref inputParameter, ref outputParameter);
                    push(command);
                    return TcpServer.ReturnType.Success;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        public TcpServer.ReturnType GetAwaiter<outputParameterType>(TcpServer.CommandInfo identityCommand, Callback<TcpServer.ReturnValue<outputParameterType>> callback, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref outputParameter);
                    push(command);
                    return TcpServer.ReturnType.Success;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
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
        internal TcpServer.ReturnType GetAwaiter<inputParameterType, outputParameterType>(TcpServer.CommandInfo identityCommand, Callback<TcpServer.ReturnValue<outputParameterType>> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        public TcpServer.ReturnType GetAwaiter<inputParameterType>(TcpServer.CommandInfo identityCommand, TcpServer.Awaiter onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall.Call, ref inputParameter);
                    push(command);
                    return TcpServer.ReturnType.Success;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <returns>保持回调</returns>
        public TcpServer.ReturnType GetAwaiter(TcpServer.CommandInfo identityCommand, TcpServer.Awaiter onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall.Call);
                    push(command);
                    return TcpServer.ReturnType.Success;
                }
                return TcpServer.ReturnType.ClientException;
            }
            return TcpServer.ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        public void Get<inputParameterType, outputParameterType>(TcpServer.CommandInfo identityCommand, ref Callback<TcpServer.ReturnValue<outputParameterType>> callback, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref inputParameter);
                    push(command);
                    callback = null;
                }
            }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        public void Get<outputParameterType>(TcpServer.CommandInfo identityCommand, ref Callback<TcpServer.ReturnValue<outputParameterType>> callback)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback);
                    push(command);
                    callback = null;
                }
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        public void Call<inputParameterType>(TcpServer.CommandInfo identityCommand, Action<TcpServer.ReturnValue> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall ?? NullCallback, ref inputParameter);
                    push(command);
                }
            }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        public void Call(TcpServer.CommandInfo identityCommand, Action<TcpServer.ReturnValue> onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall ?? NullCallback);
                    push(command);
                }
            }
        }
        /// <summary>
        /// 获取远程表达式服务端节点标识
        /// </summary>
        /// <param name="types">表达式服务端节点类型集合</param>
        /// <returns>表达式服务端节点标识集合</returns>
        internal TcpServer.ReturnValue<int[]> GetRemoteExpressionNodeId(RemoteType[] types)
        {
            AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ServerNodeIdChecker.Output> callback = AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ServerNodeIdChecker.Output>.Pop();
            try
            {
                if (IsSocket)
                {
                    ClientCommand.InputOutputCommand<RemoteExpression.ServerNodeIdChecker.Input, RemoteExpression.ServerNodeIdChecker.Output> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<RemoteExpression.ServerNodeIdChecker.Input, RemoteExpression.ServerNodeIdChecker.Output>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<RemoteExpression.ServerNodeIdChecker.Input, RemoteExpression.ServerNodeIdChecker.Output>();
                    if (command != null)
                    {
                        RemoteExpression.ServerNodeIdChecker.Input inputParameter = new RemoteExpression.ServerNodeIdChecker.Input { Types = types };
                        command.Set(ClientSocket, RemoteExpression.ServerNodeIdChecker.Input.CommandInfo, callback, ref inputParameter);
                        push(command);
                        RemoteExpression.ServerNodeIdChecker.Output outputParameter;
                        TcpServer.ReturnType type = callback.Get(out outputParameter);
                        callback = null;
                        return new TcpServer.ReturnValue<int[]> { Type = type, Value = outputParameter.Return };
                    }
                }
                else return new TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientDisposed };
            }
            finally
            {
                if (callback != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ServerNodeIdChecker.Output>.PushNotNull(callback);
            }
            return new TcpServer.ReturnValue<int[]> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <param name="clientNode">客户端远程表达式节点</param>
        /// <returns>返回值类型</returns>
        public TcpServer.ReturnType GetRemoteExpressionClientNode(RemoteExpression.Node node, out RemoteExpression.ClientNode clientNode)
        {
            RemoteExpressionServerNodeIdChecker checker = remoteExpressionServerNodeIdChecker;
            TcpServer.ReturnType returnType = checker.Check(node);
            clientNode = returnType == TcpServer.ReturnType.Success ? new RemoteExpression.ClientNode { Node = node, Checker = checker, ClientNodeId = node.ClientNodeId } : default(RemoteExpression.ClientNode);
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
        private TcpServer.ReturnValue<RemoteExpression.ReturnValue> getRemoteExpression(ref RemoteExpression.ClientNode node)
        {
            AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ReturnValue.Output> callback = AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ReturnValue.Output>.Pop();
            try
            {
                if (IsSocket)
                {
                    ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, RemoteExpression.ReturnValue.Output> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, RemoteExpression.ReturnValue.Output>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, RemoteExpression.ReturnValue.Output>();
                    if (command != null)
                    {
                        command.Set(ClientSocket, RemoteExpression.ClientNode.CommandInfo, callback, ref node);
                        push(command);
                        RemoteExpression.ReturnValue.Output outputParameter;
                        TcpServer.ReturnType type = callback.Get(out outputParameter);
                        callback = null;
                        return new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = type, Value = outputParameter.Return };
                    }
                }
                else return new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientDisposed };
            }
            finally
            {
                if (callback != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<RemoteExpression.ReturnValue.Output>.PushNotNull(callback);
            }
            return new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnType CallRemoteExpression(RemoteExpression.Node node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                TcpServer.ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                return type == TcpServer.ReturnType.Success ? getRemoteExpression(ref clientNode).Type : type;
            }
            return TcpServer.ReturnType.RemoteExpressionNotSupport;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        public TcpServer.ReturnValue<returnType> GetRemoteExpression<returnType>(RemoteExpression.Node<returnType> node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                TcpServer.ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                if (type == TcpServer.ReturnType.Success)
                {
                    TcpServer.ReturnValue<RemoteExpression.ReturnValue> value = getRemoteExpression(ref clientNode);
                    if (value.Type == TcpServer.ReturnType.Success) return ((RemoteExpression.ReturnValue<returnType>)value.Value).Value;
                    return new TcpServer.ReturnValue<returnType> { Type = value.Type };
                }
                return new TcpServer.ReturnValue<returnType> { Type = type };
            }
            return new TcpServer.ReturnValue<returnType> { Type = TcpServer.ReturnType.RemoteExpressionNotSupport };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnValue<RemoteExpression.ReturnValue> GetRemoteExpression(RemoteExpression.ClientNode node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                return node.Checker != remoteExpressionServerNodeIdChecker ? getRemoteExpression(ref node) : new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = TcpServer.ReturnType.RemoteExpressionCheckerError };
            }
            return new TcpServer.ReturnValue<RemoteExpression.ReturnValue> { Type = TcpServer.ReturnType.RemoteExpressionNotSupport };
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        private TcpServer.AwaiterBox<RemoteExpression.ReturnValue> getRemoteExpressionAwaiter(ref RemoteExpression.ClientNode node)
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<RemoteExpression.ClientNode, AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>>();
                if (command != null)
                {
                    AutoCSer.Net.TcpServer.AwaiterBox<RemoteExpression.ReturnValue> awaiter = new AutoCSer.Net.TcpServer.AwaiterBox<RemoteExpression.ReturnValue>();
                    AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue> outputParameter = default(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<RemoteExpression.ReturnValue>);
                    command.Set(ClientSocket, RemoteExpression.ClientNode.CommandInfo, awaiter, ref node, ref outputParameter);
                    push(command);
                    return awaiter;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.AwaiterBox<RemoteExpression.ReturnValue> GetRemoteExpressionAwaiter(RemoteExpression.Node node)
        {
            if (remoteExpressionServerNodeIdChecker != null)
            {
                RemoteExpression.ClientNode clientNode;
                TcpServer.ReturnType type = GetRemoteExpressionClientNode(node, out clientNode);
                if (type == TcpServer.ReturnType.Success) return getRemoteExpressionAwaiter(ref clientNode);
            }
            return null;
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.AwaiterBox<RemoteExpression.ReturnValue> GetRemoteExpressionAwaiter(RemoteExpression.ClientNode node)
        {
            return remoteExpressionServerNodeIdChecker != null && node.Checker != remoteExpressionServerNodeIdChecker ? getRemoteExpressionAwaiter(ref node) : null;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal unsafe override void VirtualBuildOutput()
        {
            TcpServer.ClientCommand.CommandBase head = ClientSocket.CommandQueue, currentCommand;
            SubBuffer.PoolBufferFull Buffer = default(SubBuffer.PoolBufferFull), CopyBuffer = default(SubBuffer.PoolBufferFull), CompressBuffer = default(SubBuffer.PoolBufferFull);
            TcpServer.SenderBuildInfo buildInfo = new TcpServer.SenderBuildInfo { SendBufferSize = clientCreator.CommandClient.SendBufferPool.Size, IsClientAwaiter = clientCreator.Attribute.IsClientAwaiter };
            try
            {
                clientCreator.CommandClient.SendBufferPool.Get(ref Buffer);
                SubArray<byte> sendData = default(SubArray<byte>);
                int bufferLength = Buffer.Length, outputSleep = clientCreator.CommandClient.OutputSleep, currentOutputSleep, minCompressSize = clientCreator.CommandClient.MinCompressSize, isNewCommand;
                SocketError socketError;
                using (UnmanagedStream outputStream = (ClientSocket.OutputSerializer = BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new BinarySerialize.Serializer()).SetTcpServer())
                {
                    do
                    {
                        buildInfo.IsNewBuffer = 0;
                        fixed (byte* dataFixed = Buffer.Buffer)
                        {
                            byte* start = dataFixed + Buffer.StartIndex;
                            currentOutputSleep = outputSleep;
                            RESET:
                            if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                            buildInfo.Clear();
                            outputStream.ByteSize = 0;
                            WAIT:
                            OutputWaitHandle.Wait();
                            if (isClose) return;
                            while (System.Threading.Interlocked.CompareExchange(ref commandQueueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                            isNewCommand = this.isNewCommand;
                            this.isNewCommand = 0;
                            System.Threading.Interlocked.Exchange(ref commandQueueLock, 0);
                            if (isNewCommand == 0) return;
                            LOOP:
                            while ((currentCommand = head.LinkNext) != null)
                            {
                                if (currentCommand.Build(ref buildInfo) != currentCommand) head = currentCommand;
                                if (buildInfo.IsSend != 0) goto SETDATA;
                            }
                            if (this.isNewCommand != 0) goto WAIT;
                            if (!buildInfo.IsClientAwaiter)
                            {
                                if (currentOutputSleep >= 0)
                                {
                                    Thread.Sleep(currentOutputSleep);
                                    if (this.isNewCommand != 0 || outputStream.ByteSize == 0)
                                    {
                                        currentOutputSleep = 0;
                                        goto WAIT;
                                    }
                                }
                                else if (outputStream.ByteSize == 0) goto WAIT;
                            }
                            else
                            {
                                if (currentOutputSleep == int.MinValue) currentOutputSleep = outputSleep;
                                if (currentOutputSleep >= 0) Thread.Sleep(currentOutputSleep);
                                else AutoCSer.Threading.ThreadYield.YieldOnly();
                                if (this.isNewCommand != 0 || outputStream.ByteSize == 0)
                                {
                                    currentOutputSleep = 0;
                                    goto WAIT;
                                }
                            }
                            SETDATA:
                            //buildCommandCount += buildInfo.Count;
                            int outputLength = outputStream.ByteSize;
                            if (outputLength <= bufferLength)
                            {
                                if (outputStream.Data.ByteSize != bufferLength) Memory.CopyNotNull(outputStream.Data.Byte, start, outputLength);
                                sendData.Set(Buffer.Buffer, Buffer.StartIndex, outputLength);
                            }
                            else
                            {
                                outputStream.GetSubBuffer(ref CopyBuffer);
                                sendData.Set(CopyBuffer.Buffer, CopyBuffer.StartIndex, outputLength);
                                if (CopyBuffer.Length <= clientCreator.CommandClient.SendBufferMaxSize)
                                {
                                    Buffer.Free();
                                    CopyBuffer.CopyToClear(ref Buffer);
                                    buildInfo.IsNewBuffer = 1;
                                }
                            }
                            if (outputLength >= minCompressSize && !buildInfo.IsVerifyMethod && AutoCSer.IO.Compression.DeflateCompressor.Get(sendData.Array, sendData.Start, outputLength, ref CompressBuffer, ref sendData, sizeof(int) * 2, sizeof(int) * 2))
                            {
                                int compressionDataSize = sendData.Length;
                                sendData.MoveStart(-(sizeof(int) * 2));
                                fixed (byte* sendDataFixed = sendData.Array)
                                {
                                    byte* dataStart = sendDataFixed + sendData.Start;
                                    *(int*)dataStart = *(int*)-compressionDataSize;
                                    *(int*)(dataStart + sizeof(int)) = outputLength;
                                }
                            }
                            SEND:
                            if (IsSocket)
                            {
                                int count = Socket.Send(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError);
                                sendData.MoveStart(count);
                                ++SendCount;
                                if (sendData.Length == 0)
                                {
                                    if (buildInfo.IsNewBuffer == 0)
                                    {
                                        CompressBuffer.TryFree();
                                        CopyBuffer.Free();
                                        if (head.LinkNext == null)
                                        {
                                            currentOutputSleep = int.MinValue;
                                            goto RESET;
                                        }
                                        if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                        buildInfo.Clear();
                                        outputStream.ByteSize = 0;
                                        //currentOutputSleep = outputSleep;
                                        goto LOOP;
                                    }
                                    CompressBuffer.TryFree();
                                    if (head.LinkNext != null)
                                    {
                                        while (System.Threading.Interlocked.CompareExchange(ref commandQueueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                                        isNewCommand = this.isNewCommand;
                                        this.isNewCommand = 1;
                                        System.Threading.Interlocked.Exchange(ref commandQueueLock, 0);
                                        if (isNewCommand == 0) OutputWaitHandle.Set();
                                    }
                                    goto FIXEDEND;
                                }
                                if (socketError == SocketError.Success && count > 0) goto SEND;
                                buildInfo.IsError = true;
                            }
                            buildInfo.IsNewBuffer = 0;
                            FIXEDEND:;
                        }
                    }
                    while (buildInfo.IsNewBuffer != 0);
                }
            }
            catch (Exception error)
            {
                clientCreator.CommandClient.Log.Add(AutoCSer.Log.LogType.Error, error);
                buildInfo.IsError = true;
            }
            finally
            {
                if (buildInfo.IsError) (ClientSocket as ClientSocket<attributeType>).DisposeSocket();
                Buffer.Free();
                CopyBuffer.TryFree();
                CompressBuffer.TryFree();
                ClientSocket.FreeOutputSerializer();
            }
        }
    }
}
