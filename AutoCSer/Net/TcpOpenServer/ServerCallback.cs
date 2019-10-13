using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal sealed class ServerCallback : TcpServer.ServerCallbackBase<ServerSocketSender>
    {
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket">异步套接字</param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        internal ServerCallback(ServerSocketSender socket, bool isBuildOutputThread) : base(socket, isBuildOutputThread) { }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public override bool Callback(TcpServer.ReturnValue returnValue)
        {
            ServerSocketSender socket = this.socket;
            this.socket = null;
            return socket != null && socket.TryPush(TcpServer.Server.GetCommandIndex(commandIndex, returnValue.Type), isBuildOutputThread);
        }
    }
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal sealed class ServerCallbackKeep : TcpServer.ServerCallbackBase<ServerSocketSender>
    {
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket">异步套接字</param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        internal ServerCallbackKeep(ServerSocketSender socket, bool isBuildOutputThread) : base(socket, isBuildOutputThread)
        {
            socket.AddKeepCallback(commandIndex & TcpServer.Server.CommandIndexAnd, this);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public override bool Callback(TcpServer.ReturnValue returnValue)
        {
            ServerSocketSender sender = socket;
            if (sender != null && sender.IsSocket)
            {
                TcpServer.ServerOutput.ReturnTypeOutput output = sender.TryGetOutput(TcpServer.Server.GetCommandIndex(commandIndex, returnValue.Type));
                if (output != null)
                {
                    if (returnValue.Type == TcpServer.ReturnType.Success)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallback);
                        if (socket == null) System.Threading.Interlocked.Exchange(ref keepLock, 0);
                        else
                        {
                            bool isHead = sender.Outputs.IsPushHead(output);
                            System.Threading.Interlocked.Exchange(ref keepLock, 0);
                            if (isHead) sender.TryBuildOutput(isBuildOutputThread);
                            return true;
                        }
                    }
                    else
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallback);
                        if (socket == null) System.Threading.Interlocked.Exchange(ref keepLock, 0);
                        else
                        {
                            bool isHead = sender.Outputs.IsPushHead(output);
                            socket = null;
                            System.Threading.Interlocked.Exchange(ref keepLock, 0);
                            if (isHead) sender.TryBuildOutput(isBuildOutputThread);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 取消保持调用
        /// </summary>
        internal override void CancelKeep()
        {
            Callback(new TcpServer.ReturnValue { Type = TcpServer.ReturnType.CancelKeep });
            socket = null;
        }
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal sealed class ServerCallback<outputParameterType, returnType> : TcpServer.ServerCallbackBase<ServerSocketSender, outputParameterType, returnType>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        internal ServerCallback(ServerSocketSender socket, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter) : base(socket, outputInfo, ref outputParameter) { }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public override bool Callback(TcpServer.ReturnValue<returnType> returnValue)
        {
            TcpServer.ReturnValue<outputParameterType> outputParameter = new TcpServer.ReturnValue<outputParameterType> { Type = returnValue.Type };
            if (returnValue.Type == TcpServer.ReturnType.Success)
            {
#if NOJIT
                this.outputParameter.ReturnObject = returnValue.Value;
#else
                SetReturn(ref this.outputParameter, returnValue.Value);
#endif
                outputParameter.Value = this.outputParameter;
            }
            return socket.TryPush(commandIndex, outputInfo, ref outputParameter);
        }
#if !NOJIT
        /// <summary>
        /// 设置返回值委托
        /// </summary>
        internal static readonly TcpInternalServer.ServerCallback<outputParameterType, returnType>.SetReturnValue SetReturn = TcpInternalServer.ServerCallback<outputParameterType, returnType>.SetReturn;
#endif
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal sealed class ServerCallbackKeep<outputParameterType, returnType> : TcpServer.ServerCallbackBase<ServerSocketSender, outputParameterType, returnType>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        internal ServerCallbackKeep(ServerSocketSender socket, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter) : base(socket, outputInfo, ref outputParameter)
        {
            socket.AddKeepCallback(commandIndex & TcpServer.Server.CommandIndexAnd, this);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public override bool Callback(TcpServer.ReturnValue<returnType> returnValue)
        {
            ServerSocketSender sender = socket;
            if (sender != null && sender.IsSocket)
            {
                if (returnValue.Type == TcpServer.ReturnType.Success)
                {
                    outputParameterType outputParameter = this.outputParameter;
#if NOJIT
                    outputParameter.ReturnObject = returnValue.Value;
#else
                    ServerCallback<outputParameterType, returnType>.SetReturn(ref outputParameter, returnValue.Value);
#endif
                    TcpServer.ServerOutput.Output<outputParameterType> output = sender.TryGetOutput<outputParameterType>(commandIndex, outputInfo, ref outputParameter);
                    if (output != null)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallback);
                        if (socket == null) System.Threading.Interlocked.Exchange(ref keepLock, 0);
                        else
                        {
                            bool isHead = sender.Outputs.IsPushHead(output);
                            System.Threading.Interlocked.Exchange(ref keepLock, 0);
                            if (isHead) sender.TryBuildOutput(outputInfo.IsBuildOutputThread);
                            return true;
                        }
                    }
                }
                else
                {
                    TcpServer.ServerOutput.ReturnTypeOutput output = sender.TryGetOutput(TcpServer.Server.GetCommandIndex(commandIndex, returnValue.Type));
                    if (output != null)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallback);
                        if (socket == null) System.Threading.Interlocked.Exchange(ref keepLock, 0);
                        else
                        {
                            bool isHead = sender.Outputs.IsPushHead(output);
                            socket = null;
                            System.Threading.Interlocked.Exchange(ref keepLock, 0);
                            if (isHead) sender.TryBuildOutput(outputInfo.IsBuildOutputThread);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 取消保持调用
        /// </summary>
        internal override void CancelKeep()
        {
            Callback(new TcpServer.ReturnValue<returnType> { Type = TcpServer.ReturnType.CancelKeep });
            socket = null;
        }
    }
}
