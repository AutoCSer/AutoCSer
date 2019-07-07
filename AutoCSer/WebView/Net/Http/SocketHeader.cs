using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 头部
    /// </summary>
    internal unsafe sealed class SocketHeader : Header
    {
        /// <summary>
        /// HTTP 套接字
        /// </summary>
        private Socket httpSocket;
#if DOTNET2
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private AsyncCallback onReceiveAsyncCallback;
#else
        /// <summary>
        /// 接收数据套接字异步事件对象
        /// </summary>
        private SocketAsyncEventArgs receiveAsyncEventArgs;
#endif
        ///// <summary>
        ///// HTTP 头部是否接收完成
        ///// </summary>
        //private byte isHeader;
        /// <summary>
        /// HTTP 头部
        /// </summary>
        /// <param name="socket">HTTP 套接字</param>
        internal SocketHeader(Socket socket)
            : base()
        {
            httpSocket = socket;
#if DOTNET2
            onReceiveAsyncCallback = onReceive;
#else
            receiveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
            receiveAsyncEventArgs.Completed += onReceive;
            receiveAsyncEventArgs.SetBuffer(Buffer.Buffer, Buffer.StartIndex, ReceiveBufferSize);
#endif
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
#if !DOTNET2
            receiveAsyncEventArgs.Completed -= onReceive;
            SocketAsyncEventArgsPool.PushNotNull(ref receiveAsyncEventArgs);
#endif
            Buffer.Free();
        }
        /// <summary>
        /// 开始接收数据
        /// </summary>
        internal void Receive()
        {
            try
            {
                socket = httpSocket.Socket;
                Flag = HeaderFlag.None;
                receiveIndex = receiveCount = HeaderEndIndex = 0;
                IsKeepAliveReceive = false;
#if DOTNET2
                SocketError socketError;
                IAsyncResult async = socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex, ReceiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) ReceiveTimeout.Push(httpSocket, socket);
                    return;
                }
#else
                receiveAsyncEventArgs.SocketError = SocketError.Success;
                while (Interlocked.CompareExchange(ref httpSocket.ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                receiveAsyncEventArgs.SetBuffer(Buffer.StartIndex, ReceiveBufferSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
                    ReceiveTimeout.Push(httpSocket, socket);
                    Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                    return;
                }
                Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                if (onReceive()) return;
#endif
            }
            catch (Exception error)
            {
                httpSocket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            //if (isHeader == 0) 
                httpSocket.HeaderError();
        }
        /// <summary>
        /// 接收数据下一个请求数据
        /// </summary>
        internal void ReceiveNext()
        {
            try
            {
                int receiveSize = receiveCount - receiveIndex;
                if (receiveSize == 0) receiveIndex = receiveCount = HeaderEndIndex = 0;
                else
                {
                    HeaderEndIndex = receiveIndex;
                    if (searchEnd()) return;
                    fixed (byte* bufferFixed = Buffer.Buffer)
                    {
                        byte* start = bufferFixed + Buffer.StartIndex;
                        Memory.CopyNotNull(start + receiveIndex, start, receiveCount = receiveSize);
                    }
                    HeaderEndIndex -= receiveIndex;
                    receiveIndex = 0;
                }
                //isHeader = 0;
                IsKeepAliveReceive = true;
#if DOTNET2
                SocketError socketError;
                IAsyncResult async = socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex + receiveCount, ReceiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) KeepAliveReceiveTimeout.Push(httpSocket, socket);
                    return;
                }
#else
                while (Interlocked.CompareExchange(ref httpSocket.ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                receiveAsyncEventArgs.SetBuffer(Buffer.StartIndex + receiveCount, ReceiveBufferSize - receiveCount);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
                    KeepAliveReceiveTimeout.Push(httpSocket, socket);
                    Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                    return;
                }
                Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                if (onReceive()) return;
#endif
            }
            catch (Exception error)
            {
                httpSocket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            //if (isHeader == 0) 
            httpSocket.HeaderError();
        }
#if DOTNET2
        /// <summary>
        /// 接收数据完成后的回调
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onReceive(IAsyncResult async)
#else
        /// <summary>
        /// 接收数据完成后的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
            try
            {
#if DOTNET2
                if (IsKeepAliveReceive)
                {
                    if (!async.CompletedSynchronously) KeepAliveReceiveTimeout.Cancel(httpSocket);
                    IsKeepAliveReceive = false;
                }
                else if (!async.CompletedSynchronously) ReceiveTimeout.Cancel(httpSocket);
                if (onReceiveAsync(async)) return;
#else
                if (IsKeepAliveReceive)
                {
                    KeepAliveReceiveTimeout.Cancel(httpSocket);
                    IsKeepAliveReceive = false;
                }
                else ReceiveTimeout.Cancel(httpSocket);
                if (onReceive()) return;
#endif
            }
            catch (Exception error)
            {
                httpSocket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            //if (isHeader == 0) 
                httpSocket.HeaderError();
        }
#if DOTNET2
        /// <summary>
        /// 接收数据完成后的处理
        /// </summary>
        /// <param name="async">异步回调参数</param>
        /// <returns></returns>
        private bool onReceiveAsync(IAsyncResult async)
#else
        /// <summary>
        /// 接收数据完成后的处理
        /// </summary>
        /// <returns></returns>
        private bool onReceive()
#endif
        {
#if DOTNET2
            SocketError socketError;
            int count = socket.EndReceive(async, out socketError);
            if (count > 0 && socketError == SocketError.Success)
#else
        START:
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
                if (count > 0)
#endif
            {
                receiveCount += count;
                if (searchEnd()) return true;
                if (receiveCount != ReceiveBufferSize && (count >= TcpServer.Server.MinSocketSize || ReceiveSizeLessCount++ == 0))
                {
#if DOTNET2
                    async = socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex + receiveCount, ReceiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) ReceiveTimeout.Push(httpSocket, socket);
                        return true;
                    }
#else
                        while (Interlocked.CompareExchange(ref httpSocket.ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                        receiveAsyncEventArgs.SetBuffer(Buffer.StartIndex + receiveCount, ReceiveBufferSize - receiveCount);
                        if (socket.ReceiveAsync(receiveAsyncEventArgs))
                        {
                            ReceiveTimeout.Push(httpSocket, socket);
                            Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                            return true;
                        }
                        Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                        goto START;
#endif
                }
            }
#if !DOTNET2
            }
#endif
            return false;
        }
        /// <summary>
        /// 搜索头部结束位置
        /// </summary>
        /// <returns></returns>
        private bool searchEnd()
        {
            int searchEndIndex = receiveCount - sizeof(int);
            if (HeaderEndIndex <= searchEndIndex)
            {
                fixed (byte* dataFixed = Buffer.Buffer)
                {
                    byte* bufferStart = dataFixed + Buffer.StartIndex, start = bufferStart + HeaderEndIndex, searchEnd = bufferStart + searchEndIndex, end = bufferStart + receiveCount;
                    *end = 13;
                    do
                    {
                        while (*start != 13) ++start;
                        if (start <= searchEnd)
                        {
                            if (*(int*)start == 0x0a0d0a0d)
                            {
                                HeaderEndIndex = (int)(start - bufferStart);
                                ReceiveSizeLessCount = 0;
                                if (parse())
                                {
                                    if (httpSocket.DomainServer == null || !isKeepAliveDomainServer)
                                    {
                                        httpSocket.DomainServer = httpSocket.Server.RegisterServer.GetServer(bufferStart + HostIndex.StartIndex, HostIndex.Length);
                                        if (httpSocket.DomainServer == null) return false;
                                    }
                                    receiveIndex = HeaderEndIndex + sizeof(int);
                                    httpSocket.Request();
                                }
                                else if ((Flag & HeaderFlag.IsRangeError) == 0) httpSocket.HeaderError();
                                else httpSocket.ResponseError(ResponseState.RangeNotSatisfiable416);
                                return true;
                            }
                            ++start;
                        }
                        else
                        {
                            HeaderEndIndex = (int)(start - bufferStart);
                            return false;
                        }
                    }
                    while (true);
                }
            }
            return false;
        }
    }
}
