using System;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using System.Net;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端
    /// </summary>
    public abstract class ClientBase : CommandBuffer
    {
        /// <summary>
        /// TCP 服务客户端创建器
        /// </summary>
        protected readonly ClientSocketCreator clientCreator;
        /// <summary>
        /// 套接字是否可用
        /// </summary>
        public bool IsSocket
        {
            get
            {
                ClientSocketBase socket = clientCreator.Socket;
                return socket != null && !socket.IsClose;
            }
        }
        /// <summary>
        /// 客户端 TCP 套接字更新访问锁
        /// </summary>
        internal readonly object OnSocketLock = new object();
        /// <summary>
        /// 客户端批量处理等待类型
        /// </summary>
        internal readonly OutputWaitType OutputWaitType;
        /// <summary>
        /// 第一次重建连接休眠毫秒数
        /// </summary>
        internal readonly int FristTryCreateSleep;
        /// <summary>
        /// 重建连接休眠毫秒数
        /// </summary>
        internal readonly int TryCreateSleep;
        /// <summary>
        /// 自定义数据包处理
        /// </summary>
        private readonly Action<SubArray<byte>> onCustomData;
        /// <summary>
        /// 创建 TCP 客户端套接字等待锁
        /// </summary>
        internal AutoCSer.Threading.WaitHandle SocketWait;
        /// <summary>
        /// TCP 客户端套接字事件
        /// </summary>
        private event Action<ClientSocketEventParameter> onSocket;
        /// <summary>
        /// 注册当前服务的 TCP 注册服务名称
        /// </summary>
        internal abstract string TcpRegisterName { get; }
        /// <summary>
        /// 套接字发送数据次数
        /// </summary>
        public abstract int SendCount { get; }
        /// <summary>
        /// 套接字接收数据次数
        /// </summary>
        public abstract int ReceiveCount { get; }
        /// <summary>
        /// 最大超时秒数
        /// </summary>
        internal virtual ushort MaxTimeoutSeconds { get { return 0; } }

#if !NOJIT
        /// <summary>
        /// TCP 组件基类
        /// </summary>
        internal ClientBase() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        internal ClientBase(ServerBaseAttribute attribute, ILog log, Action<SubArray<byte>> onCustomData) 
            : base(attribute, attribute.GetSendBufferSize, attribute.GetReceiveBufferSize, attribute.ClientSendBufferMaxSize, log)
        {
            OutputWaitType = attribute.GetClientOutputWaitType;
            FristTryCreateSleep = Math.Max(attribute.GetClientFirstTryCreateSleep, 10);
            TryCreateSleep = Math.Max(attribute.GetClientTryCreateSleep, 10);
            SocketWait.Set(0);

            this.onCustomData = onCustomData;
            clientCreator = new ClientSocketCreator(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (IsDisposed == 0)
            {
                base.Dispose();
                DisposeSocket();
                SocketWait.Set();
            }
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        internal abstract void DisposeSocket();
        /// <summary>
                                                       /// 创建套接字
                                                       /// </summary>
                                                       /// <param name="clientCreator"></param>
                                                       /// <param name="ipAddress"></param>
                                                       /// <param name="port"></param>
                                                       /// <param name="createVersion"></param>
        internal abstract ClientSocketBase CreateSocketByCreator(ClientSocketCreator clientCreator, IPAddress ipAddress, int port, int createVersion);
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <param name="socket">TCP 调用客户端套接字</param>
        /// <returns></returns>
        internal abstract bool SocketVerifyMethod(ClientSocketSenderBase socket);
        ///// <summary>
        ///// TCP 客户端套接字关闭事件
        ///// </summary>
        ///// <param name="clientSocketCreator"></param>
        ///// <param name="clientSocket"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void OnSocketClosed(ClientSocketCreator clientSocketCreator, ClientSocketBase clientSocket)
        //{
        //    if (onSocket != null) onSocket(new ClientSocketEventParameter( clientSocketCreator, clientSocket, ClientSocketEventParameter.EventType.SocketClosed));
        //}
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="onCheckSocketVersion">TCP 客户端套接字初始化处理</param>
        /// <returns>TCP 客户端套接字初始化处理</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public CheckSocketVersion CreateCheckSocketVersion(Action<ClientSocketEventParameter> onCheckSocketVersion)
        {
            return new CheckSocketVersion(this, onCheckSocketVersion);
        }
        /// <summary>
        /// 设置 TCP 客户端套接字事件
        /// </summary>
        /// <param name="onSocket"></param>
        public void OnSocket(Action<ClientSocketEventParameter> onSocket)
        {
            if (onSocket != null)
            {
                Monitor.Enter(OnSocketLock);
                try
                {
                    this.onSocket += onSocket;
                    OnSetSocket();
                }
                finally { Monitor.Exit(OnSocketLock); }
            }
        }
        /// <summary>
        /// 设置 TCP 客户端套接字事件
        /// </summary>
        internal abstract void OnSetSocket();
        /// <summary>
        /// 删除 TCP 客户端套接字事件
        /// </summary>
        /// <param name="onSocket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void RemoveOnSetSocket(Action<ClientSocketEventParameter> onSocket)
        {
            if (onSocket != null)
            {
                Monitor.Enter(OnSocketLock);
                try
                {
                    this.onSocket -= onSocket;
                }
                finally { Monitor.Exit(OnSocketLock); }
            }
        }
        /// <summary>
        /// 移除 TCP 服务客户端套接字
        /// </summary>
        /// <param name="clientSocketCreator">TCP 服务客户端创建器</param>
        /// <param name="socket"></param>
        /// <param name="type"></param>
        internal void CallOnSocket(ClientSocketCreator clientSocketCreator, ClientSocketBase socket, ClientSocketEventParameter.EventType type)
        {
            if (onSocket != null)
            {
                Monitor.Enter(OnSocketLock);
                try
                {
                    onSocket(new ClientSocketEventParameter(clientSocketCreator, socket, type));
                }
                catch (Exception error)
                {
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
                finally { Monitor.Exit(OnSocketLock); }
            }
        }
        /// <summary>
        /// 设置新的 TCP 服务客户端套接字
        /// </summary>
        /// <param name="clientSocketCreator">TCP 服务客户端创建器</param>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnSetSocketOnly(ClientSocketCreator clientSocketCreator, ClientSocketBase socket)
        {
            onSocket(new ClientSocketEventParameter(clientSocketCreator, socket, ClientSocketEventParameter.EventType.SetSocket));
        }

        /// <summary>
        /// 获取异步回调
        /// </summary>
        /// <param name="callback">回调委托</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public Callback<ReturnValue<outputParameterType>> GetCallback<returnType, outputParameterType>(Action<ReturnValue<returnType>> callback)
#if NOJIT
        where outputParameterType : IReturnParameter
#else
        where outputParameterType : IReturnParameter<returnType>
#endif
        {
            return callback != null ? new CallbackReturnValue<returnType, outputParameterType>(callback) : (Callback<ReturnValue<outputParameterType>>)NullCallbackReturnValue<outputParameterType>.Default;
        }
        /// <summary>
        /// 自定义数据包处理
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CustomData(ref SubArray<byte> data)
        {
            if (onCustomData == null) Log.Info("客户端自定义数据包被丢弃", LogLevel.Info | LogLevel.AutoCSer);
            else
            {
                try
                {
                    onCustomData(data);
                }
                catch (Exception error)
                {
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
            }
        }
    }
}
