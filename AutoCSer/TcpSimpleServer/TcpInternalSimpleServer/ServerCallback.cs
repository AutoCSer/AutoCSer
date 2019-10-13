using System;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal sealed class ServerCallback : TcpSimpleServer.ServerCallback<ServerSocket>
    {
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket">异步套接字</param>
        internal ServerCallback(ServerSocket socket) : base(socket) { }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public override bool Callback(TcpServer.ReturnValue returnValue)
        {
            ServerSocket socket = this.socket;
            this.socket = null;
            return socket != null && socket.SendAsync(returnValue.Type);
        }
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal sealed class ServerCallback<outputParameterType, returnType> : TcpSimpleServer.ServerCallback<ServerSocket, outputParameterType, returnType>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        internal ServerCallback(ServerSocket socket, TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter) : base(socket, outputInfo, ref outputParameter) { }
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
                setReturn(ref this.outputParameter, returnValue.Value);
#endif
                outputParameter.Value = this.outputParameter;
            }
            return socket.SendAsync(outputInfo, ref outputParameter);
        }
#if !NOJIT
        /// <summary>
        /// 设置返回值委托
        /// </summary>
        private static readonly TcpInternalServer.ServerCallback<outputParameterType, returnType>.SetReturnValue setReturn = TcpInternalServer.ServerCallback<outputParameterType, returnType>.SetReturn;
#endif
    }
    /// <summary>
    /// 验证函数异步回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    internal sealed class ServerCallback<outputParameterType> : TcpSimpleServer.ServerCallback<ServerSocket, outputParameterType, bool>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<bool>
#endif
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        internal ServerCallback(ServerSocket socket, TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter) : base(socket, outputInfo, ref outputParameter) { }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public override bool Callback(TcpServer.ReturnValue<bool> returnValue)
        {
            TcpServer.ReturnValue<outputParameterType> outputParameter = new TcpServer.ReturnValue<outputParameterType> { Type = returnValue.Type };
            if (returnValue.Type == TcpServer.ReturnType.Success)
            {
#if NOJIT
                this.outputParameter.ReturnObject = returnValue.Value;
#else
                setReturn(ref this.outputParameter, returnValue.Value);
#endif
                outputParameter.Value = this.outputParameter;
            }
            if (returnValue.Value) socket.SetVerifyMethod();
            return socket.SendAsync(outputInfo, ref outputParameter);
        }
#if !NOJIT
        /// <summary>
        /// 设置返回值委托
        /// </summary>
        private static readonly TcpInternalServer.ServerCallback<outputParameterType, bool>.SetReturnValue setReturn = TcpInternalServer.ServerCallback<outputParameterType, bool>.SetReturn;
#endif
    }
}
