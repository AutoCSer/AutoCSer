using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端套接字数据发送
    /// </summary>
    public abstract class ClientSocketSenderBase : Sender
    {
        ///// <summary>
        ///// TCP 服务客户端套接字
        ///// </summary>
        //internal readonly ClientSocketBase ClientSocket;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal AutoCSer.Threading.AutoWaitHandle OutputWaitHandle;
        /// <summary>
        /// 发送变换数据
        /// </summary>
        internal ulong SendMarkData;
        /// <summary>
        /// 套接字发送数据次数
        /// </summary>
        internal int SendCount;
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal ClientSocketSenderBase() : base(null) { }
#endif
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务客户端套接字</param>
        internal ClientSocketSenderBase(ClientSocketBase socket)
            : base(socket.Socket)
        {
            //ClientSocket = socket;
            OutputWaitHandle.Set(0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Close()
        {
            if (!isClose)
            {
                isClose = true;
                OutputWaitHandle.Set();
            }
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        internal abstract void Check();
        /// <summary>
        /// 创建输出
        /// </summary>
        internal virtual void VirtualBuildOutput()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 默认空回调委托
        /// </summary>
        /// <param name="value"></param>
        private static void nullCallback(ReturnValue value) { }
        /// <summary>
        /// 默认空回调委托
        /// </summary>
        internal static readonly Action<ReturnValue> NullCallback = nullCallback;
    }
}
