using System;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal sealed class ServerCallback : TcpSimpleServer.ServerCallback<ServerCallback, ServerSocket>
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        internal ServerCallback()
        {
            onReturnHandle = onReturn;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        private bool onReturn(TcpServer.ReturnValue returnValue)
        {
            ServerSocket socket = this.socket;
            this.socket = null;
            AutoCSer.Threading.RingPool<ServerCallback>.Default.PushNotNull(this);
            return socket.SendAsync(returnValue.Type);
        }
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal sealed class ServerCallback<outputParameterType, returnType> : TcpSimpleServer.ServerCallback<ServerCallback<outputParameterType, returnType>, ServerSocket, outputParameterType, returnType>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<returnType>
#endif
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        internal ServerCallback()
        {
            onReturnHandle = onReturn;
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
            ServerSocket socket = this.socket;
            TcpSimpleServer.OutputInfo outputInfo = this.outputInfo;
            this.outputParameter = default(outputParameterType);
            this.socket = null;
            AutoCSer.Threading.RingPool<ServerCallback<outputParameterType, returnType>>.Default.PushNotNull(this);
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
    internal sealed class ServerCallback<outputParameterType> : TcpSimpleServer.ServerCallback<ServerCallback<outputParameterType>, ServerSocket, outputParameterType, bool>
#if NOJIT
        where outputParameterType : struct, IReturnParameter
#else
        where outputParameterType : struct, IReturnParameter<bool>
#endif
    {
        /// <summary>
        /// 验证函数异步回调
        /// </summary>
        internal ServerCallback()
        {
            onReturnHandle = onReturn;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        private bool onReturn(TcpServer.ReturnValue<bool> returnValue)
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
            ServerSocket socket = this.socket;
            TcpSimpleServer.OutputInfo outputInfo = this.outputInfo;
            this.outputParameter = default(outputParameterType);
            this.socket = null;
            AutoCSer.Threading.RingPool<ServerCallback<outputParameterType>>.Default.PushNotNull(this);
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
