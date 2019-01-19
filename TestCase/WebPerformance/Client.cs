using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// HTTP 测试客户端
    /// </summary>
    internal abstract class Client : IDisposable
    {
        /// <summary>
        /// 客户端任务池
        /// </summary>
        public abstract class Task<clientType> : IDisposable
            where clientType : Client
        {
            /// <summary>
            /// 客户端集合
            /// </summary>
            protected clientType[] clients;
            /// <summary>
            /// 空闲事件
            /// </summary>
            protected EventWaitHandle freeWait;
            /// <summary>
            /// 客户端集合访问锁
            /// </summary>
            protected readonly object clientLock = new object();
            /// <summary>
            /// 空闲客户端数量
            /// </summary>
            protected int clientIndex;
            /// <summary>
            /// 当前实例数量
            /// </summary>
            protected int clientCount;
            /// <summary>
            /// 请求任务数量
            /// </summary>
            protected int taskCount;
            /// <summary>
            /// 保持连接最大次数
            /// </summary>
            internal int KeepAliveCount;
            /// <summary>
            /// 错误数量
            /// </summary>
            internal int ErrorCount;
            /// <summary>
            /// 拒绝服务次数
            /// </summary>
            internal int RefusedCount;
            /// <summary>
            /// 客户端任务池
            /// </summary>
            /// <param name="maxClientCount">最大实例数量</param>
            /// <param name="isKeepAlive">保持连接最大次数</param>
            public Task(int maxClientCount, int keepAliveCount)
            {
                clients = new clientType[maxClientCount];
                KeepAliveCount = keepAliveCount;
                freeWait = new EventWaitHandle(true, EventResetMode.ManualReset);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                Monitor.Enter(clientLock);
                try
                {
                    if (clientIndex != 0)
                    {
                        foreach (clientType client in clients)
                        {
                            client.Dispose();
                            if (--clientIndex == 0) break;
                        }
                    }
                }
                finally { Monitor.Exit(clientLock); }
            }
            /// <summary>
            /// 等待空闲
            /// </summary>
            public void Wait()
            {
                freeWait.WaitOne();
            }
            /// <summary>
            /// 关闭客户端
            /// </summary>
            public void CloseClient()
            {
                Monitor.Enter(clientLock);
                for (int index = 0; index != clientIndex; ++index) clients[index].CloseSocket();
                Monitor.Exit(clientLock);
            }
        }

        /// <summary>
        /// 收发数据缓冲区字节数
        /// </summary>
        protected const int bufferSize = 2 << 10;
        /// <summary>
        /// 关闭套接字0超时设置
        /// </summary>
        protected static readonly LingerOption lingerOption = new LingerOption(true, 0);
        /// <summary>
        /// 异步发送操作
        /// </summary>
        protected SocketAsyncEventArgs sendAsyncEventArgs;
        /// <summary>
        /// 异步接收操作
        /// </summary>
        protected SocketAsyncEventArgs recieveAsyncEventArgs;
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        protected byte[] receiveBuffer;
        /// <summary>
        /// 套接字
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// 保持连接最大次数
        /// </summary>
        protected int keepAliveCount;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        protected volatile int receiveAsyncLock;
#endif
        /// <summary>
        /// 异步操作类型
        /// </summary>
        protected ClientSocketAsyncType asyncType;
        /// <summary>
        /// 客户端
        /// </summary>
        protected Client()
        {
            sendAsyncEventArgs = SocketAsyncEventArgsPool.Get();
            recieveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
            recieveAsyncEventArgs.SetBuffer(receiveBuffer = new byte[bufferSize], 0, bufferSize);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void CloseSocket()
        {
            keepAliveCount = 0;
#if DotNetStandard
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch { }
            finally { socket.Dispose(); }
#else
            socket.Dispose();
#endif
        }
    }
}
