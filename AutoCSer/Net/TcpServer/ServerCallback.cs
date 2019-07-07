using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    internal abstract class ServerCallback<callbackType, serverSocketSenderType> : AutoCSer.Threading.Link<callbackType>, IServerKeepCallback
        where callbackType : ServerCallback<callbackType, serverSocketSenderType>
        where serverSocketSenderType : ServerSocketSender
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        protected serverSocketSenderType socket;
        /// <summary>
        /// 会话标识
        /// </summary>
        protected uint commandIndex;
        /// <summary>
        /// 保持回调访问锁
        /// </summary>
        protected int keepLock;
        /// <summary>
        /// 尝试启动创建输出线程
        /// </summary>
        protected bool isBuildOutputThread;

        /// <summary>
        /// 异步回调
        /// </summary>
        protected Func<ReturnValue, bool> onReturnHandle;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<ReturnValue, bool> Set(serverSocketSenderType socket, bool isBuildOutputThread)
        {
            this.socket = socket;
            commandIndex = socket.ServerSocket.CommandIndex;
            this.isBuildOutputThread = isBuildOutputThread;
            return onReturnHandle;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setKeep(serverSocketSenderType socket, bool isBuildOutputThread)
        {
            this.socket = socket;
            this.isBuildOutputThread = isBuildOutputThread;
            socket.AddKeepCallback((commandIndex = socket.ServerSocket.CommandIndex) & Server.CommandIndexAnd, this);
        }
        /// <summary>
        /// 取消保持调用
        /// </summary>
        public abstract void CancelKeep();
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="callbackType">异步回调类型</typeparam>
    /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送类型</typeparam>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal abstract class ServerCallback<callbackType, serverSocketSenderType, outputParameterType, returnType> : AutoCSer.Threading.Link<callbackType>, IServerKeepCallback
        where callbackType : ServerCallback<callbackType, serverSocketSenderType, outputParameterType, returnType>
        where serverSocketSenderType : ServerSocketSender
//#if NOJIT
//        where outputParameterType : IReturnParameter
//#else
//        where outputParameterType : IReturnParameter<returnType>
//#endif
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        protected serverSocketSenderType socket;
        /// <summary>
        /// 服务端输出信息
        /// </summary>
        protected OutputInfo outputInfo;
        /// <summary>
        /// 会话标识
        /// </summary>
        protected uint commandIndex;
        /// <summary>
        /// 保持回调访问锁
        /// </summary>
        protected int keepLock;
        /// <summary>
        /// 输出参数
        /// </summary>
        protected outputParameterType outputParameter;

        /// <summary>
        /// 异步回调
        /// </summary>
        protected Func<ReturnValue<returnType>, bool> onReturnHandle;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Func<ReturnValue<returnType>, bool> Set(serverSocketSenderType socket, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            this.socket = socket;
            this.outputInfo = outputInfo;
            this.outputParameter = outputParameter;
            commandIndex = socket.ServerSocket.CommandIndex;
            return onReturnHandle;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setKeep(serverSocketSenderType socket, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            this.socket = socket;
            this.outputInfo = outputInfo;
            this.outputParameter = outputParameter;
            socket.AddKeepCallback((commandIndex = socket.ServerSocket.CommandIndex) & Server.CommandIndexAnd, this);
            //flags = socket.CommandServerSocket.CommandFlags;
        }
        /// <summary>
        /// 取消保持调用
        /// </summary>
        public abstract void CancelKeep();
    }
}
