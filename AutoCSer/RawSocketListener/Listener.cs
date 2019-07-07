using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.RawSocketListener
{
    /// <summary>
    /// 原始套接字监听
    /// </summary>
    public sealed class Listener : IDisposable
    {
        /// <summary>
        /// 默认获取的数据包的字节数(默认为以太网)
        /// </summary>
        private const int defaultBufferSize = 1500;
        /// <summary>
        /// 缓冲区池
        /// </summary>
        internal static readonly AutoCSer.SubBuffer.Pool BufferPool = AutoCSer.SubBuffer.Pool.GetPool(SubBuffer.Size.Kilobyte128);

        /// <summary>
        /// 监听套接字
        /// </summary>
        private Socket socket;
#if DOTNET2

        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private AsyncCallback onReceiveAsyncCallback;
        /// <summary>
        /// 套接字错误
        /// </summary>
        private SocketError socketError;
#else
        /// <summary>
        /// 异步套接字操作
        /// </summary>
        private SocketAsyncEventArgs async;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        private int receiveAsyncLock;
#endif
#endif
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly AutoCSer.Log.ILog log;
        /// <summary>
        /// 监听地址
        /// </summary>
        private readonly IPEndPoint ipEndPoint;
        /// <summary>
        /// 队列任务
        /// </summary>
        private readonly QueueTask queueTask;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        private BufferCount buffer;
        /// <summary>
        /// 数据包字节数
        /// </summary>
        private readonly int packetSize;
        /// <summary>
        /// 缓冲区最大可用索引
        /// </summary>
        private readonly int maxBufferIndex;
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// 缓冲区结束位置
        /// </summary>
        private int bufferEndIndex;
        /// <summary>
        /// 是否处理错误状态
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private byte isDisposed;
        /// <summary>
        /// 原始套接字监听
        /// </summary>
        /// <param name="ipAddress">监听地址</param>
        /// <param name="onPacket">数据包处理委托</param>
        /// <param name="packetSize">数据包字节数</param>
        /// <param name="log">日志处理</param>
        public Listener(IPAddress ipAddress, Action<Buffer> onPacket, int packetSize = defaultBufferSize, AutoCSer.Log.ILog log = null)
        {
            if (onPacket == null) throw new ArgumentNullException();
            if (packetSize <= 0) packetSize = defaultBufferSize;
            else if (packetSize > BufferPool.Size) packetSize = BufferPool.Size;
            this.packetSize = packetSize;
            maxBufferIndex = BufferPool.Size - packetSize;
            this.log = log ?? AutoCSer.Log.Pub.Log;
            ipEndPoint = new IPEndPoint(ipAddress, 0);
#if DOTNET2
            onReceiveAsyncCallback = onReceive;
#else
            async = AutoCSer.Net.SocketAsyncEventArgsPool.Get();
            async.UserToken = this;
            async.Completed += onReceive;
#endif
            queueTask = new QueueTask(onPacket, this.log);
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(start);
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        public void Dispose()
        {
            isDisposed = 1;
            closeSocket();
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void closeSocket()
        {
            if (socket != null)
            {
#if DotNetStandard
                AutoCSer.Net.TcpServer.CommandBase.CloseClientNotNull(socket);
#else
                socket.Dispose();
#endif
                socket = null;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void free()
        {
            if (buffer != null)
            {
                buffer.Free();
                buffer = null;
            }
#if !DOTNET2
            if (async != null)
            {
                async.Completed -= onReceive;
                AutoCSer.Net.SocketAsyncEventArgsPool.PushNotNull(ref async);
            }
#endif
            queueTask.Dispose();
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        private void start()
        {
            while (isDisposed == 0)
            {
                try
                {
                    closeSocket();
                    IPAddress ipAddress = ipEndPoint.Address;
                    //在发送时必须提供完整的IP标头，所接收的数据报在返回时会保持其IP标头和选项不变。
                    socket = new Socket(ipAddress.AddressFamily, SocketType.Raw, ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ? ProtocolType.IPv6 : ProtocolType.IP);
                    socket.Blocking = false;
                    socket.Bind(ipEndPoint);
                    socket.SetSocketOption(ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ? SocketOptionLevel.IPv6 : SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
                    //byte[] optionIn = new byte[] { 1, 0, 0, 0 }, optionOut = new byte[4];
                    //socket.IOControl(IOControlCode.ReceiveAll, optionIn, optionOut);
                    byte[] optionIn = new byte[] { 1, 0, 0, 0 };
                    socket.IOControl(IOControlCode.ReceiveAll, optionIn, null);
                    if (receive())
                    {
                        IsError = false;
                        return;
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception error)
                {
                    if (!IsError) this.log.Add(Log.LogType.Error, error, "监听初始化失败，可能需要管理员权限。");
                }
                IsError = true;
            }
            free();
        }
        /// <summary>
        /// 继续接收数据
        /// </summary>
        /// <returns>是否接收成功</returns>
        private unsafe bool receive()
        {
#if DOTNET2
            if (buffer == null)
            {
                buffer = new BufferCount();
                bufferIndex = buffer.Buffer.StartIndex;
                bufferEndIndex = bufferIndex + BufferPool.Size;
            }
            socket.BeginReceive(buffer.Buffer.Buffer, bufferIndex, bufferEndIndex - bufferIndex, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
            return socketError == SocketError.Success;
#else
            START:
            if (buffer == null)
            {
                buffer = new BufferCount();
                bufferIndex = buffer.Buffer.StartIndex;
                bufferEndIndex = bufferIndex + BufferPool.Size;
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                async.SetBuffer(buffer.Buffer.Buffer, bufferIndex, BufferPool.Size);
                if (socket.ReceiveAsync(async))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
            }
            else
            {
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                async.SetBuffer(bufferIndex, bufferEndIndex - bufferIndex);
                if (socket.ReceiveAsync(async))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
            }
#if !DotNetStandard
            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
            if (async.SocketError == SocketError.Success)
            {
                onReceive(async.BytesTransferred);
                goto START;
            }
            return false;
#endif
        }
#if DOTNET2
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onReceive(IAsyncResult async)
#else
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private unsafe void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
            try
            {
#if DOTNET2
                Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
                if (socket == this.socket)
                {
                    int count = socket.EndReceive(async, out socketError);
                    if (count >= 0 && socketError == SocketError.Success)
                    {
                        onReceive(count);
                        if (receive()) return;
                    }
                }
#else
                if (async.SocketError == SocketError.Success)
                {
                    onReceive(async.BytesTransferred);
                    if (receive()) return;
                }
#endif
            }
            catch (Exception error)
            {
                this.log.Add(Log.LogType.Error, error);
            }
            if (isDisposed == 0) start();
            else free();
        }
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="count">接收数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onReceive(int count)
        {
            if (count != 0)
            {
                queueTask.Add(new Buffer(this.buffer, bufferIndex, count));
                bufferIndex += (count + 3) & (int.MaxValue - 3);
                if (bufferIndex - this.buffer.Buffer.StartIndex > maxBufferIndex) this.buffer = null;
            }
        }
    }
}
