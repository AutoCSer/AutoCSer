using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    /// <typeparam name="serverSocketType">TCP 服务套接字数据发送类型</typeparam>
    internal abstract class ServerCallback<serverSocketType> : TcpServer.ServerCallback
        where serverSocketType : ServerSocket
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        protected serverSocketType socket;
        /// <summary>
        /// 套接字是否有效
        /// </summary>
        public override bool IsSocket
        {
            get
            {
                serverSocketType socket = this.socket;
                return socket != null && socket.Socket != null;
            }
        }
        /// <summary>
                 /// TCP 服务器端异步调用
                 /// </summary>
                 /// <param name="socket">异步套接字</param>
        protected ServerCallback(serverSocketType socket)
        {
            this.socket = socket;
        }
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="serverSocketType">TCP 服务套接字数据发送类型</typeparam>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal abstract class ServerCallback<serverSocketType, outputParameterType, returnType> : TcpServer.ServerCallback<returnType>
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
        protected readonly serverSocketType socket;
        /// <summary>
        /// 套接字是否有效
        /// </summary>
        public override bool IsSocket
        {
            get
            {
                serverSocketType socket = this.socket;
                return socket != null && socket.Socket != null;
            }
        }
        /// <summary>
                 /// 服务端输出信息
                 /// </summary>
        protected readonly TcpSimpleServer.OutputInfo outputInfo;
        /// <summary>
        /// 输出参数
        /// </summary>
        protected outputParameterType outputParameter;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        protected ServerCallback(serverSocketType socket, TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            this.socket = socket;
            this.outputInfo = outputInfo;
            this.outputParameter = outputParameter;
        }
    }
}
