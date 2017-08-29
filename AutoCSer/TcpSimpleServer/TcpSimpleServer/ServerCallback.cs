using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal abstract class ServerCallback<callbackType, serverSocketType> : AutoCSer.Threading.Link<callbackType>
        where callbackType : ServerCallback<callbackType, serverSocketType>
        where serverSocketType : ServerSocket
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        protected serverSocketType socket;
        /// <summary>
        /// 异步回调
        /// </summary>
        protected Func<TcpServer.ReturnValue, bool> onReturnHandle;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<TcpServer.ReturnValue, bool> Set(serverSocketType socket)
        {
            this.socket = socket;
            return onReturnHandle;
        }
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="callbackType">异步回调类型</typeparam>
    /// <typeparam name="serverSocketType">TCP 服务套接字数据发送类型</typeparam>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal abstract class ServerCallback<callbackType, serverSocketType, outputParameterType, returnType> : AutoCSer.Threading.Link<callbackType>
        where callbackType : ServerCallback<callbackType, serverSocketType, outputParameterType, returnType>
        where serverSocketType : ServerSocket
        //#if NOJIT
        //        where outputParameterType : IReturnParameter
        //#else
        //        where outputParameterType : IReturnParameter<returnType>
        //#endif
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        protected serverSocketType socket;
        /// <summary>
        /// 服务端输出信息
        /// </summary>
        protected TcpSimpleServer.OutputInfo outputInfo;
        /// <summary>
        /// 输出参数
        /// </summary>
        protected outputParameterType outputParameter;

        /// <summary>
        /// 异步回调
        /// </summary>
        protected Func<TcpServer.ReturnValue<returnType>, bool> onReturnHandle;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<TcpServer.ReturnValue<returnType>, bool> Set(serverSocketType socket, TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            this.socket = socket;
            this.outputInfo = outputInfo;
            this.outputParameter = outputParameter;
            return onReturnHandle;
        }
    }
}
