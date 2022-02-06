using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    public abstract class ServerCallbackBase
    {
        /// <summary>
        /// 套接字是否有效
        /// </summary>
        public virtual bool IsSocket { get { return false; } }
        /// <summary>
        /// 套接字最后接收数据时间
        /// </summary>
        public virtual DateTime LastReceiveTime { get { return default(DateTime); } }
        /// <summary>
        /// 取消保持调用
        /// </summary>
        internal virtual void CancelKeep() { }
    }
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    public abstract class ServerCallback : ServerCallbackBase
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public abstract bool Callback(ReturnValue returnValue);
#if !NOJIT
        /// <summary>
        /// 回调委托隐式转换为客户端异步回调
        /// </summary>
        /// <param name="callback">回调委托</param>
        /// <returns>客户端异步回调</returns>
        public static implicit operator ServerCallback(Action<ReturnValue> callback)
        {
            return new Client(callback);
        }
        /// <summary>
        /// 客户端异步回调
        /// </summary>
        internal sealed class Client : ServerCallback
        {
            /// <summary>
            /// 回调委托
            /// </summary>
            private readonly Action<ReturnValue> callback;
            /// <summary>
            /// 客户端异步回调
            /// </summary>
            /// <param name="callback">回调委托</param>
            internal Client(Action<ReturnValue> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// 回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <returns></returns>
            public override bool Callback(ReturnValue returnValue)
            {
                callback(returnValue);
                return true;
            }
        }
#endif
    }
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送类型</typeparam>
    internal abstract class ServerCallbackBase<serverSocketSenderType> : ServerCallback
        where serverSocketSenderType : ServerSocketSender
    {
        /// <summary>
        /// 异步套接字
        /// </summary>
        protected serverSocketSenderType socket;
        /// <summary>
        /// 套接字是否有效
        /// </summary>
        public override bool IsSocket
        {
            get
            {
                serverSocketSenderType socket = this.socket;
                return socket != null && socket.IsSocket;
            }
        }
        /// <summary>
        /// 套接字最后接收数据时间
        /// </summary>
        public override DateTime LastReceiveTime
        {
            get
            {
                serverSocketSenderType socket = this.socket;
                if (socket != null) return socket.ServerSocket.LastReceiveTime;
                return base.LastReceiveTime;
            }
        }
        /// <summary>
        /// 保持回调访问锁
        /// </summary>
        protected AutoCSer.Threading.SpinLock keepLock;
        /// <summary>
        /// 会话标识
        /// </summary>
        protected readonly uint commandIndex;
        /// <summary>
        /// 尝试启动创建输出线程
        /// </summary>
        protected bool isBuildOutputThread
        {
            get { return (commandIndex & (1U << 31)) != 0; }
        }
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket">异步套接字</param>
        /// <param name="isBuildOutputThread">尝试启动创建输出线程</param>
        protected ServerCallbackBase(serverSocketSenderType socket, bool isBuildOutputThread)
        {
            this.socket = socket;
            commandIndex = socket.ServerSocket.CommandIndex & Server.CommandIndexAnd;
            if (isBuildOutputThread) commandIndex |= 1U << 31;
        }
    }
    /// <summary>
    /// TCP 服务器端异步调用
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public abstract class ServerCallback<returnType> : ServerCallbackBase
    {
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否成功加入回调队列</returns>
        public abstract bool Callback(ReturnValue<returnType> returnValue);
#if !NOJIT
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal bool CallbackReturn(returnType returnValue)
        {
            return Callback(returnValue);
        }
#endif
        /// <summary>
        /// 空回调
        /// </summary>
        public sealed class Null : ServerCallback<returnType>
        {
            /// <summary>
            /// 空回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <returns></returns>
            public override bool Callback(ReturnValue<returnType> returnValue)
            {
                return true;
            }
            /// <summary>
            /// 空回调
            /// </summary>
            public static readonly Null Default = new Null();
        }
        /// <summary>
        /// TCP 服务器端异步调用委托包装
        /// </summary>
        public sealed class Func : ServerCallback<returnType>
        {
            /// <summary>
            /// 回调委托
            /// </summary>
            private readonly Func<ReturnValue<returnType>, bool> callback;
            /// <summary>
            /// TCP 服务器端异步调用委托包装
            /// </summary>
            /// <param name="callback">回调委托</param>
            public Func(Func<ReturnValue<returnType>, bool> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// 异步回调
            /// </summary>
            /// <param name="returnValue">返回值</param>
            /// <returns>是否成功加入回调队列</returns>
            public override bool Callback(ReturnValue<returnType> returnValue)
            {
                return callback(returnValue);
            }
            /// <summary>
            /// 回调委托隐式转换为 TCP 服务器端异步调用委托包装
            /// </summary>
            /// <param name="callback">回调委托</param>
            /// <returns>TCP 服务器端异步调用委托包装</returns>
            public static implicit operator Func(Func<ReturnValue<returnType>, bool> callback)
            {
                return new Func(callback);
            }
        }

#if !NOJIT
        /// <summary>
        /// 回调委托隐式转换为客户端异步回调
        /// </summary>
        /// <param name="callback">回调委托</param>
        /// <returns>客户端异步回调</returns>
        public static implicit operator ServerCallback<returnType>(Action<ReturnValue<returnType>> callback)
        {
            return new Client(callback);
        }
        /// <summary>
        /// 客户端异步回调
        /// </summary>
        internal sealed class Client : ServerCallback<returnType>
        {
            /// <summary>
            /// 回调委托
            /// </summary>
            private readonly Action<ReturnValue<returnType>> callback;
            /// <summary>
            /// 客户端异步回调
            /// </summary>
            /// <param name="callback">回调委托</param>
            internal Client(Action<ReturnValue<returnType>> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// 回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <returns></returns>
            public override bool Callback(ReturnValue<returnType> returnValue)
            {
                callback(returnValue);
                return true;
            }
        }
#endif
    }
    /// <summary>
    /// 异步回调
    /// </summary>
    /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送类型</typeparam>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    /// <typeparam name="returnType">返回值类型</typeparam>
    internal abstract class ServerCallbackBase<serverSocketSenderType, outputParameterType, returnType> : ServerCallback<returnType>
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
        /// 套接字是否有效
        /// </summary>
        public override bool IsSocket
        {
            get
            {
                serverSocketSenderType socket = this.socket;
                return socket != null && socket.IsSocket;
            }
        }
        /// <summary>
        /// 套接字最后接收数据时间
        /// </summary>
        public override DateTime LastReceiveTime
        {
            get
            {
                serverSocketSenderType socket = this.socket;
                if (socket != null) return socket.ServerSocket.LastReceiveTime;
                return base.LastReceiveTime;
            }
        }
        /// <summary>
        /// 服务端输出信息
        /// </summary>
        protected readonly OutputInfo outputInfo;
        /// <summary>
        /// 会话标识
        /// </summary>
        protected readonly uint commandIndex;
        /// <summary>
        /// 保持回调访问锁
        /// </summary>
        protected AutoCSer.Threading.SpinLock keepLock;
        /// <summary>
        /// 输出参数
        /// </summary>
        protected outputParameterType outputParameter;
        /// <summary>
        /// TCP 服务器端异步调用
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter"></param>
        protected ServerCallbackBase(serverSocketSenderType socket, TcpServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
        {
            this.socket = socket;
            this.outputInfo = outputInfo;
            this.outputParameter = outputParameter;
            commandIndex = socket.ServerSocket.CommandIndex;
        }
    }
}
