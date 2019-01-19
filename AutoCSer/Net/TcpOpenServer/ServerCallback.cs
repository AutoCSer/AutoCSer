using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal sealed class ServerCallback : TcpServer.ServerCallback<ServerCallback, ServerSocketSender>
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="isKeep">是否保持回调</param>
        internal ServerCallback(byte isKeep)
        {
            if (isKeep == 0) onReturnHandle = onReturn;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        private bool onReturn(TcpServer.ReturnValue returnValue)
        {
            ServerSocketSender socket = this.socket;
            uint commandIndex = this.commandIndex;
            this.socket = null;
            AutoCSer.Threading.RingPool<ServerCallback>.Default.PushNotNull(this);
            return socket.TryPush(TcpServer.Server.GetCommandIndex(commandIndex, returnValue.Type), isBuildOutputThread);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        private bool onlyCallback(TcpServer.ReturnValue returnValue)
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
                            //sender.Outputs.Push(output);
                            //AutoCSer.Threading.Interlocked.keepLock = 0;
                            //sender.TryBuildOutput();
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
                            //sender.Outputs.Push(output);
                            //socket = null;
                            //AutoCSer.Threading.Interlocked.keepLock = 0;
                            //sender.TryBuildOutput();
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
        public override void CancelKeep()
        {
            onlyCallback(new TcpServer.ReturnValue { Type = TcpServer.ReturnType.CancelKeep });
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<TcpServer.ReturnValue, bool> SetKeep(ServerSocketSender socket, bool isBuildOutputThread)
        {
            setKeep(socket, isBuildOutputThread);
            return onlyCallback;
        }
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal sealed class ServerCallback<outputParameterType, returnType> : TcpServer.ServerCallback<ServerCallback<outputParameterType, returnType>, ServerSocketSender, outputParameterType, returnType>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="isKeep">是否保持回调</param>
        internal ServerCallback(byte isKeep)
        {
            if (isKeep == 0) onReturnHandle = onReturn;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        private bool onReturn(TcpServer.ReturnValue<returnType> returnValue)
        {
            TcpServer.ReturnValue<outputParameterType> outputParameter = new TcpServer.ReturnValue<outputParameterType> { Type = returnValue.Type };
            if (returnValue.Type == TcpServer.ReturnType.Success)
            {
#if NOJIT
                this.outputParameter.ReturnObject = returnValue.Value;
#else
                setReturn(ref this.outputParameter, returnValue.Value);
                //this.outputParameter.Return = returnValue.Value;
#endif
                outputParameter.Value = this.outputParameter;
            }
            ServerSocketSender socket = this.socket;
            TcpServer.OutputInfo outputInfo = this.outputInfo;
            uint commandIndex = this.commandIndex;
            this.outputParameter = default(outputParameterType);
            this.socket = null;
            AutoCSer.Threading.RingPool<ServerCallback<outputParameterType, returnType>>.Default.PushNotNull(this);
            return socket.TryPush(commandIndex, outputInfo, ref outputParameter);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        private bool onlyCallback(TcpServer.ReturnValue<returnType> returnValue)
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
                    setReturn(ref outputParameter, returnValue.Value);
                    //outputParameter.Return = returnValue.Value;
#endif
                    TcpServer.ServerOutput.Output<outputParameterType> output = sender.TryGetOutput<outputParameterType>(commandIndex, outputInfo, ref outputParameter);
                    if (output != null)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref keepLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TcpServerKeepCallback);
                        if (socket == null) System.Threading.Interlocked.Exchange(ref keepLock, 0);
                        else
                        {
                            //sender.Outputs.Push(output);
                            //AutoCSer.Threading.Interlocked.keepLock = 0;
                            //sender.TryBuildOutput();
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
                            //sender.Outputs.Push(output);
                            //socket = null;
                            //AutoCSer.Threading.Interlocked.keepLock = 0;
                            //sender.TryBuildOutput();
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
        public override void CancelKeep()
        {
            onlyCallback(new TcpServer.ReturnValue<returnType> { Type = TcpServer.ReturnType.CancelKeep });
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<TcpServer.ReturnValue<returnType>, bool> SetKeep(ServerSocketSender socket, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            setKeep(socket, outputInfo, ref outputParameter);
            return onlyCallback;
        }
#if !NOJIT
        /// <summary>
        /// 设置返回值委托
        /// </summary>
        private static readonly TcpInternalServer.ServerCallback<outputParameterType, returnType>.SetReturnValue setReturn = TcpInternalServer.ServerCallback<outputParameterType, returnType>.SetReturn;
#endif
    }
}
