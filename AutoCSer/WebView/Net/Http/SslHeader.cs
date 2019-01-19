using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 头部
    /// </summary>
    internal unsafe sealed class SslHeader : Header
    {
        /// <summary>
        /// HTTP 套接字
        /// </summary>
        private SslSocket httpSocket;
        /// <summary>
        /// 接受头部换行数据
        /// </summary>
        private AsyncCallback receiveCallback;
        /// <summary>
        /// HTTP 头部
        /// </summary>
        /// <param name="socket">HTTP 套接字</param>
        internal SslHeader(SslSocket socket)
            : base()
        {
            httpSocket = socket;
            receiveCallback = onReceive;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
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
                ReceiveSizeLessCount = 0;
                if (!httpSocket.SslStream.BeginRead(Buffer.Buffer, Buffer.StartIndex, ReceiveBufferSize, receiveCallback, this).CompletedSynchronously) ReceiveTimeout.Push(httpSocket, socket);
                return;
            }
            catch (Exception error)
            {
                httpSocket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
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
                IsKeepAliveReceive = true;
                if (!httpSocket.SslStream.BeginRead(Buffer.Buffer, Buffer.StartIndex + receiveCount, ReceiveBufferSize - receiveCount, receiveCallback, this).CompletedSynchronously) ReceiveTimeout.Push(httpSocket, socket);
                return;
            }
            catch (Exception error)
            {
                httpSocket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            httpSocket.HeaderError();
        }
        /// <summary>
        /// 接受头部换行数据
        /// </summary>
        /// <param name="result">异步操作状态</param>
        private unsafe void onReceive(IAsyncResult result)
        {
            if (IsKeepAliveReceive)
            {
                if (!result.CompletedSynchronously) KeepAliveReceiveTimeout.Cancel(httpSocket);
                IsKeepAliveReceive = false;
            }
            else if (!result.CompletedSynchronously) ReceiveTimeout.Cancel(httpSocket);
            try
            {
                int count = httpSocket.SslStream.EndRead(result);
                if (count > 0)
                {
                    receiveCount += count;
                    if (searchEnd()) return;
                    if (receiveCount != ReceiveBufferSize && (count >= TcpServer.Server.MinSocketSize || ReceiveSizeLessCount++ == 0))
                    {
                        if (!httpSocket.SslStream.BeginRead(Buffer.Buffer, Buffer.StartIndex + receiveCount, ReceiveBufferSize - receiveCount, receiveCallback, this).CompletedSynchronously) ReceiveTimeout.Push(httpSocket, socket);
                    }
                }
            }
            catch (Exception error)
            {
                httpSocket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            httpSocket.HeaderError();
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
