using System;
using System.Net.Sockets;
using AutoCSer.Extension;
using System.Threading;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字
    /// </summary>
    internal unsafe sealed class Socket : SocketBase<Socket>, IDisposable
    {
        /// <summary>
        /// HTTP 头部
        /// </summary>
        internal SocketHeader Header;
#if DOTNET2
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private AsyncCallback onReceiveAsyncCallback;
        /// <summary>
        /// 发送数据异步回调
        /// </summary>
        private AsyncCallback onSendAsyncCallback;
#else
        /// <summary>
        /// 接收数据套接字异步事件对象
        /// </summary>
        private SocketAsyncEventArgs receiveAsyncEventArgs;
        /// <summary>
        /// 发送数据套接字异步事件对象
        /// </summary>
        private SocketAsyncEventArgs sendAsyncEventArgs;
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        internal int ReceiveAsyncLock;
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        private int sendAsyncLock;
#endif
        /// <summary>
        /// HTTP 套接字数据接收器
        /// </summary>
        private SocketBoundaryReceiver boundaryReceiver;
        /// <summary>
        /// HTTP 套接字
        /// </summary>
        internal Socket()
        {
            HttpHeader = Header = new SocketHeader(this);
#if DOTNET2
            onReceiveAsyncCallback = onReceive;
            onSendAsyncCallback = onSend;
#else
            receiveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
            sendAsyncEventArgs = SocketAsyncEventArgsPool.Get();
            receiveAsyncEventArgs.Completed += onReceive;
            sendAsyncEventArgs.Completed += onSend;
#endif
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~Socket()
        {
            if (Buffer.PoolBuffer.Pool != null)
            {
                Buffer.Free();
                Header.Free();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
#if DOTNET2
            if (onReceiveAsyncCallback != null)
#else
            if (receiveAsyncEventArgs != null)
#endif
            {
#if DOTNET2
                onReceiveAsyncCallback = onSendAsyncCallback = null;
#else
                receiveAsyncEventArgs.Completed -= onReceive;
                sendAsyncEventArgs.Completed -= onSend;
                SocketAsyncEventArgsPool.PushNotNull(ref receiveAsyncEventArgs);
                SocketAsyncEventArgsPool.PushNotNull(ref sendAsyncEventArgs);
#endif
                Buffer.Free();
                Header.Free();
#if !DOTNET2
                if (boundaryReceiver == null) boundaryReceiver.Free();
#endif
            }
        }
        /// <summary>
        /// 开始处理新的请求
        /// </summary>
        /// <param name="server">HTTP 服务</param>
        /// <param name="socket">套接字</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Start(Server server, ref System.Net.Sockets.Socket socket)
        {
            Socket = socket;
            this.Server = server;
            socket = null;
#if !DOTNET2
            sendAsyncEventArgs.SocketError = SocketError.Success;
#endif
            Header.Receive();
        }
        /// <summary>
        /// HTTP 头部接收错误
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void HeaderError()
        {
            DisposeSocket();
            DomainServer = null;
            Free();
            if (Pool.PushNotNull(this) == 0) Dispose();
        }
        /// <summary>
        /// 接收数据下一个请求数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool receiveNext()
        {
            if (Header.IsKeepAlive != 0)
            {
                Free();
                Header.ReceiveNext();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="state">错误状态</param>
        internal void ResponseError(ResponseState state)
        {
            if (DomainServer != null)
            {
                Response response = DomainServer.GetErrorResponseData(state, Header.IsGZip);
                if (response != null)
                {
                    if (state != ResponseState.NotFound404 || Header.Method != MethodType.GET)
                    {
                        Header.Flag &= HeaderFlag.All ^ HeaderFlag.IsKeepAlive;
                    }
                    if (responseHeader(ref response)) return;
                }
            }
            byte[] data = errorResponseDatas[(int)state];
            System.Net.Sockets.Socket socket = Socket;
            if (data != null && socket != null)
            {
                try
                {
                    SendType = state == ResponseState.NotFound404 && Header.Method == MethodType.GET ? SendType.Next : SendType.Close;
                    Data.Set(data, 0, data.Length);
                    Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                    SocketError socketError;
                    IAsyncResult async = socket.BeginSend(data, 0, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return;
                    }
#else
                    while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                    sendAsyncEventArgs.SetBuffer(data, 0, Data.Length);
                    if (socket.SendAsync(sendAsyncEventArgs))
                    {
                        Http.Header.ReceiveTimeout.Push(this, socket);
                        Interlocked.Exchange(ref sendAsyncLock, 0);
                        return;
                    }
                    Interlocked.Exchange(ref sendAsyncLock, 0);
                    if (onSend()) return;
#endif
                }
                catch (Exception error)
                {
                    Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
                }
            }
            HeaderError();
        }
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="identity">操作标识</param>
        /// <param name="state">错误状态</param>
        /// <returns>是否匹配会话标识</returns>
        public override bool ResponseError(long identity, ResponseState state)
        {
            if (Interlocked.CompareExchange(ref this.Identity, identity + 1, identity) == identity)
            {
                ResponseError(state);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="state">错误状态</param>
        internal override void ResponseErrorIdentity(ResponseState state)
        {
            ++this.Identity;
            ResponseError(state);
        }
        /// <summary>
        /// 输出 HTTP 响应数据
        /// </summary>
        /// <param name="identity">HTTP 操作标识</param>
        /// <param name="response">HTTP 响应数据</param>
        /// <returns>是否匹配会话标识</returns>
        public override bool Response(long identity, ref Response response)
        {
            if (Interlocked.CompareExchange(ref this.Identity, identity + 1, identity) == identity)
            {
                this.response(ref response);
                return true;
            }
            Http.Response.Push(ref response);
            return false;
        }
        /// <summary>
        /// 输出HTTP响应数据
        /// </summary>
        /// <param name="response">HTTP响应数据</param>
        internal override void ResponseIdentity(ref Response response)
        {
            ++this.Identity;
            this.response(ref response);
        }
        /// <summary>
        /// 输出 HTTP 响应数据
        /// </summary>
        /// <param name="response">HTTP 响应数据</param>
        private void response(ref Response response)
        {
            bool isHeaderError = false;
            try
            {
                CheckNotChanged304(ref response);
                if (Header.Method == MethodType.POST && (Flag & SocketFlag.IsLoadForm) == 0)
                {
                    Header.IgnoreContentLength();
                    if (Header.ContentLength == 0)
                    {
                        isHeaderError = true;
                        if (responseHeader(ref response)) return;
                    }
                    else
                    {
                        System.Net.Sockets.Socket socket = Socket;
                        if (socket == null) Http.Response.Push(ref response);
                        else
                        {
                            this.HttpResponse = response;
                            ReceiveType = ReceiveType.Response;
                            Data.Set(Buffer.Buffer, Buffer.StartIndex, Math.Min(Header.ContentLength, Buffer.Length));
                            response = null;
                            Timeout = Config.GetTimeout(Header.ContentLength);
#if DOTNET2
                                SocketError socketError;
                                IAsyncResult async = socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex, Data.Length, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                                if (socketError == SocketError.Success)
                                {
                                    if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                    return;
                                }
#else
                            receiveAsyncEventArgs.SocketError = SocketError.Success;
                            while (Interlocked.CompareExchange(ref ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                            receiveAsyncEventArgs.SetBuffer(Buffer.Buffer, Buffer.StartIndex, Data.Length);
                            if (socket.ReceiveAsync(receiveAsyncEventArgs))
                            {
                                Http.Header.ReceiveTimeout.Push(this, socket);
                                while (Interlocked.CompareExchange(ref ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                                return;
                            }
                            Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                            isHeaderError = true;
                            if (onReceive()) return;
#endif
                        }
                    }
                }
                else
                {
                    isHeaderError = true;
                    if (responseHeader(ref response)) return;
                }
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error);
            }
            if (isHeaderError) HeaderError();
            else ResponseError(ResponseState.ServerError500);
        }
        /// <summary>
        /// HTTP 响应头部输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool responseHeader()
        {
            Response response = this.HttpResponse;
            this.HttpResponse = null;
            return responseHeader(ref response);
        }
        /// <summary>
        /// HTTP 响应头部输出
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool responseHeader(ref Response response)
        {
            try
            {
                if (!response.IsFile)
                {
                    ResponseError(ResponseState.NotFound404);
                    return true;
                }
                System.Net.Sockets.Socket socket;
                ResponseFlag responseFlag = response.Flag;
                if (response.Body.Length != 0 && Header.IsKeepAlive != 0 && (responseFlag & ResponseFlag.HeaderSize) != 0 && Header.IsRange == 0 && Header.Method != MethodType.HEAD)
                {
                    if ((socket = Socket) == null) return false;
                    Data = response.Body;
                    Data.MoveStart(-response.HeaderSize);
                    SendType = SendType.Next;
                    Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                    SocketError socketError;
                    IAsyncResult async = socket.BeginSend(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return true;
                    }
                    return false;
#else
                    while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                    sendAsyncEventArgs.SetBuffer(Data.Array, Data.Start, Data.Length);
                    if (socket.SendAsync(sendAsyncEventArgs))
                    {
                        Http.Header.ReceiveTimeout.Push(this, socket);
                        Interlocked.Exchange(ref sendAsyncLock, 0);
                        return true;
                    }
                    Interlocked.Exchange(ref sendAsyncLock, 0);
                    if (--sendDepth == 0)
                    {
                        sendDepth = maxSendDepth;
                        OnSendTask.Task.Add(this);
                        return true;
                    }
                    return onSend();
#endif
                }
                ResponseSize = response.BodySize;
                fixed (byte* headerBufferFixed = Header.Buffer.Buffer)
                {
                    byte* responseSizeFixed = headerBufferFixed + (Header.Buffer.StartIndex + Http.Header.ReceiveBufferSize);
                    RangeLength responseSizeIndex, bodySizeIndex = new RangeLength(), rangeStartIndex = new RangeLength(), rangeEndIndex = new RangeLength();
                    ResponseState state = response.ResponseState;
                    if (Header.IsRange != 0 && (responseFlag & ResponseFlag.IsPool) != 0)
                    {
                        if (Header.IsFormatRange != 0 || Header.FormatRange(ResponseSize))
                        {
                            if (state == ResponseState.Ok200)
                            {
                                long rangeStart = Header.RangeStart, rangeEnd = Header.RangeEnd;
                                rangeStartIndex = Number.ToBytes((ulong)rangeStart, responseSizeFixed + 20 * 2);
                                rangeEndIndex = Number.ToBytes((ulong)rangeEnd, responseSizeFixed + 20 * 3);
                                bodySizeIndex = Number.ToBytes((ulong)ResponseSize, responseSizeFixed + 20);
                                response.State = state = ResponseState.PartialContent206;
                                ResponseSize = Header.RangeSize;
                            }
                        }
                        else
                        {
                            ResponseSize = 0;
                            ResponseError(ResponseState.RangeNotSatisfiable416);
                            return true;
                        }
                    }
                    if ((ulong)ResponseSize < 10)
                    {
                        *responseSizeFixed = (byte)((int)ResponseSize + '0');
                        responseSizeIndex = new RangeLength(0, 1);
                    }
                    else responseSizeIndex = Number.ToBytes((ulong)ResponseSize, responseSizeFixed);
                    ResponseStateAttribute stateAttribute = EnumAttribute<ResponseState, ResponseStateAttribute>.Array((byte)state);
                    if (stateAttribute == null) stateAttribute = EnumAttribute<ResponseState, ResponseStateAttribute>.Array((byte)ResponseState.ServerError500);
                    int index = httpVersionSize + stateAttribute.Text.Length + contentLengthSize + responseSizeIndex.Length + 2 + 2;
                    if (state == ResponseState.PartialContent206)
                    {
                        index += rangeSize + rangeStartIndex.Length + rangeEndIndex.Length + bodySizeIndex.Length + 2 + 2;
                    }
                    Cookie cookie = null;
                    SubBuffer.PoolBufferFull buffer = GetBuffer(index = GetResponseHeaderIndex(response, index, ref cookie));
                    fixed (byte* bufferFixed = buffer.Buffer)
                    {
                        byte* bufferStart = bufferFixed + buffer.StartIndex, write = bufferStart + httpVersionSize;
                        writeHttpVersion(bufferStart);
                        stateAttribute.Write(write);
                        writeContentLength(write += stateAttribute.Text.Length);
                        Memory.SimpleCopyNotNull64(responseSizeFixed + responseSizeIndex.Start, write += contentLengthSize, responseSizeIndex.Length);
                        *(short*)(write += responseSizeIndex.Length) = 0x0a0d;
                        write += sizeof(short);
                        if (state == ResponseState.PartialContent206)
                        {
                            writeRange(write);
                            Memory.SimpleCopyNotNull64(responseSizeFixed + (rangeStartIndex.Start + 20 * 2), write += rangeSize, rangeStartIndex.Length);
                            *(write += rangeStartIndex.Length) = (byte)'-';
                            Memory.SimpleCopyNotNull64(responseSizeFixed + (rangeEndIndex.Start + 20 * 3), ++write, rangeEndIndex.Length);
                            *(write += rangeEndIndex.Length) = (byte)'/';
                            Memory.SimpleCopyNotNull64(responseSizeFixed + (bodySizeIndex.Start + 20), ++write, bodySizeIndex.Length);
                            *(short*)(write += bodySizeIndex.Length) = 0x0a0d;
                            write += sizeof(short);
                        }
                        index = (int)(CreateResponseHeader(response, cookie, write, index) - bufferStart);
                        //                    if (checkIndex != index)
                        //                    {
                        //                        Server.RegisterServer.TcpServer.Log.add(Log.Type.Fatal, "responseHeader checkIndex[" + checkIndex.toString() + "] != index[" + index.toString() + @"]
                        //" + System.Text.Encoding.ASCII.GetString(buffer.Buffer, buffer.StartIndex, index));
                        //                    }
                        if (ResponseSize != 0)
                        {
                            switch (response.Type)
                            {
                                case ResponseType.ByteArray:
                                    if (buffer.Length - index >= (int)ResponseSize)
                                    {
                                        System.Buffer.BlockCopy(response.Body.Array, state == ResponseState.PartialContent206 ? (int)Header.RangeStart : 0, buffer.Buffer, buffer.StartIndex + index, (int)ResponseSize);
                                        index += (int)ResponseSize;
                                        ResponseSize = 0;
                                    }
                                    break;
                                case ResponseType.SubByteArray:
                                    if (Header.IsKeepAlive != 0 && (responseFlag & ResponseFlag.CanHeaderSize) != 0 && index <= response.Body.Start && Header.IsRange == 0)
                                    {
                                        if ((socket = Socket) == null) return false;
                                        fixed (byte* bodyFixed = response.Body.Array) Memory.CopyNotNull(bufferStart, bodyFixed + response.Body.Start - index, index);
                                        response.SetHeaderSize(index);

                                        Data = response.Body;
                                        Data.MoveStart(-response.HeaderSize);
                                        SendType = SendType.Next;
                                        Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                                    SocketError socketError;
                                    IAsyncResult async = socket.BeginSend(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                                    if (socketError == SocketError.Success)
                                    {
                                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                        return true;
                                    }
                                    return false;
#else
                                        while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                                        sendAsyncEventArgs.SetBuffer(Data.Array, Data.Start, Data.Length);
                                        if (socket.SendAsync(sendAsyncEventArgs))
                                        {
                                            Http.Header.ReceiveTimeout.Push(this, socket);
                                            Interlocked.Exchange(ref sendAsyncLock, 0);
                                            return true;
                                        }
                                        Interlocked.Exchange(ref sendAsyncLock, 0);
                                        if (--sendDepth == 0)
                                        {
                                            sendDepth = maxSendDepth;
                                            OnSendTask.Task.Add(this);
                                            return true;
                                        }
                                        return onSend();
#endif
                                    }
                                    goto COPY;
                                case ResponseType.SubBuffer:
                                    COPY:
                                    if (buffer.Length - index >= (int)ResponseSize)
                                    {
                                        System.Buffer.BlockCopy(response.Body.Array, state == ResponseState.PartialContent206 ? response.Body.Start + (int)Header.RangeStart : response.Body.Start, buffer.Buffer, buffer.StartIndex + index, (int)ResponseSize);
                                        index += (int)ResponseSize;
                                        ResponseSize = 0;
                                    }
                                    break;
                            }
                        }
                    }
                    if ((socket = Socket) != null)
                    {
                        if (ResponseSize == 0) SendType = SendType.Next;
                        else
                        {
                            this.HttpResponse = response;
                            SendType = SendType.Body;
                            response = null;
                        }
                        Data.Set(buffer.Buffer, buffer.StartIndex, index);
                        Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                    SocketError socketError;
                    IAsyncResult async = socket.BeginSend(Data.Array, Data.Start, index, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return true;
                    }
                    return false;
#else
                        while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                        sendAsyncEventArgs.SetBuffer(Data.Array, Data.Start, index);
                        if (socket.SendAsync(sendAsyncEventArgs))
                        {
                            Http.Header.ReceiveTimeout.Push(this, socket);
                            Interlocked.Exchange(ref sendAsyncLock, 0);
                            return true;
                        }
                        Interlocked.Exchange(ref sendAsyncLock, 0);
                        if (--sendDepth == 0)
                        {
                            sendDepth = maxSendDepth;
                            OnSendTask.Task.Add(this);
                            return true;
                        }
                        return onSend();
#endif
                    }
                }
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error);
            }
            finally { Http.Response.Push(ref response); }
            return false;
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
                if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Cancel(this);
                if (onReceiveAsync(async)) return;
#else
                Http.Header.ReceiveTimeout.Cancel(this);
                if (onReceive()) return;
#endif
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            HeaderError();
        }
#if DOTNET2
        /// <summary>
        /// 数据接收完成后的处理
        /// </summary>
        /// <param name="async">异步回调参数</param>
        /// <returns></returns>
        private bool onReceiveAsync(IAsyncResult async)
#else
        /// <summary>
        /// 数据接收完成后的处理
        /// </summary>
        /// <returns></returns>
        private bool onReceive()
#endif
        {
#if DOTNET2
            System.Net.Sockets.Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
            if (socket == Socket)
            {
                SocketError socketError;
                int count = socket.EndReceive(async, out socketError);
                if (socketError == SocketError.Success)
                {
#else
        START:
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                    Data.MoveStart(count);
                    if (Data.Length == 0)
                    {
                        ReceiveSizeLessCount = 0;
                        switch (ReceiveType)
                        {
                            case ReceiveType.Response:
                                if ((Header.ContentLength -= Buffer.Length) <= 0) return responseHeader();
                                Data.Set(Buffer.Buffer, Buffer.StartIndex, Math.Min(Header.ContentLength, Buffer.Length));
#if DOTNET2
                                if (socket == Socket)
                                {
                                    async = socket.BeginReceive(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                                    if (socketError == SocketError.Success)
                                    {
                                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                        return true;
                                    }
                                }
                                return false;
#else
                            System.Net.Sockets.Socket socket = Socket;
                            if (socket == null) return false;
                            while (Interlocked.CompareExchange(ref ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                            receiveAsyncEventArgs.SetBuffer(Data.Array, Data.Start, Data.Length);
                            if (socket.ReceiveAsync(receiveAsyncEventArgs))
                            {
                                Http.Header.ReceiveTimeout.Push(this, socket);
                                Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                                return true;
                            }
                            Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                            goto START;
#endif
                            case ReceiveType.GetForm: return OnGetForm();
                        }
                    }
                    if ((count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0)) && Date.NowTime.Now <= Timeout)
                    {
#if DOTNET2
                        if (socket == Socket)
                        {
                            async = socket.BeginReceive(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                return true;
                            }
                        }
#else
                    System.Net.Sockets.Socket socket = Socket;
                    if (socket == null) return false;
                    while (Interlocked.CompareExchange(ref ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                    receiveAsyncEventArgs.SetBuffer(Data.Start, Data.Length);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
                        Http.Header.ReceiveTimeout.Push(this, socket);
                        Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                        return true;
                    }
                    Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                    goto START;
#endif
                }
#if DOTNET2
                }
#endif
            }
            return false;
        }
#if DOTNET2
        /// <summary>
        /// 数据发送完成后的回调
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onSend(IAsyncResult async)
#else
        /// <summary>
        /// 数据发送完成后的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onSend(object sender, SocketAsyncEventArgs async)
#endif
        {
            try
            {
#if DOTNET2
                if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Cancel(this);
                if (onSendAsync(async)) return;
#else
                Http.Header.ReceiveTimeout.Cancel(this);
                if (onSend()) return;
#endif
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            HeaderError();
        }
#if !DOTNET2
        /// <summary>
        /// 最大同步发送数据深度
        /// </summary>
        private const byte maxSendDepth = 32;
        /// <summary>
        /// 可同步发送数据深度
        /// </summary>
        private byte sendDepth = maxSendDepth;
        /// <summary>
        /// 数据发送完成后的任务处理
        /// </summary>
        /// <param name="currentTaskTicks"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Socket SingleRunTask(ref long currentTaskTicks)
        {
            Socket nextTask = NextTask;
            NextTask = null;
            try
            {
                if(onSend()) return nextTask;
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            HeaderError();
            return nextTask;
        }
#endif
#if DOTNET2
        /// <summary>
        /// 数据发送完成后的处理
        /// </summary>
        /// <param name="async">异步回调参数</param>
        /// <returns></returns>
        private bool onSendAsync(IAsyncResult async)
#else
        /// <summary>
        /// 数据发送完成后的处理
        /// </summary>
        /// <returns></returns>
        private bool onSend()
#endif
        {
#if DOTNET2
            System.Net.Sockets.Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
            if (socket == Socket)
            {
                SocketError socketError;
                int count = socket.EndSend(async, out socketError);
                if (socketError == SocketError.Success)
                {
#else
        START:
            if (sendAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = sendAsyncEventArgs.BytesTransferred;
                System.Net.Sockets.Socket socket;
#endif
                    Data.MoveStart(count);
                    if (Data.Length == 0)
                    {
                        SendSizeLessCount = 0;
                        //isShutdown = true;
                        switch (SendType)
                        {
                            case SendType.Next:
                                if (receiveNext()) return true;
                                break;
                            case SendType.Body:
                                switch (HttpResponse.Type)
                                {
                                    case ResponseType.ByteArray:
                                        Data.Array = HttpResponse.Body.Array;
                                        Data.SetFull();
                                        goto SENDDATA;
                                    case ResponseType.SubByteArray:
                                    case ResponseType.SubBuffer:
                                        Data = HttpResponse.Body;
                                    SENDDATA:
#if DOTNET2
                                        if (socket != Socket) return false;
#else
                                    if ((socket = Socket) == null) return false;
#endif
                                        SendType = SendType.Next;
                                        Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                                        async = socket.BeginSend(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                                        if (socketError == SocketError.Success)
                                        {
                                            if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                            return true;
                                        }
                                        break;
#else
                                    while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                                    sendAsyncEventArgs.SetBuffer(Data.Array, Data.Start, Data.Length);
                                    if (socket.SendAsync(sendAsyncEventArgs))
                                    {
                                        Http.Header.ReceiveTimeout.Push(this, socket);
                                        Interlocked.Exchange(ref sendAsyncLock, 0);
                                        return true;
                                    }
                                    Interlocked.Exchange(ref sendAsyncLock, 0);
                                    goto START;
#endif
                                    case ResponseType.File:
                                        SendFileStream = new FileStream(HttpResponse.BodyFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, Http.Header.BufferPool.Size, FileOptions.SequentialScan);
                                        if (HttpResponse.State == ResponseState.PartialContent206) SendFileStream.Seek(Header.RangeStart, SeekOrigin.Begin);
                                        SendType = SendType.File;
                                        Buffer.ToSubByteArray(ref Data);
                                        Timeout = Config.GetTimeout(ResponseSize);
                                        goto SENDFILE;
                                }
                                break;
                            case SendType.File:
                            SENDFILE:
#if DOTNET2
                                if (socket != Socket) return false;
#else
                            if ((socket = Socket) == null) return false;
#endif
                                Data.Set(Buffer.StartIndex, (int)Math.Min(ResponseSize, Buffer.Length));
                                if (SendFileStream.Read(Data.Array, Data.Start, Data.Length) == Data.Length)
                                {
                                    if ((ResponseSize -= Data.Length) == 0) SendType = SendType.Next;
#if DOTNET2
                                    async = socket.BeginSend(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                                    if (socketError == SocketError.Success)
                                    {
                                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                        return true;
                                    }
#else
                                while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                                sendAsyncEventArgs.SetBuffer(Data.Start, Data.Length);
                                if (socket.SendAsync(sendAsyncEventArgs))
                                {
                                    Http.Header.ReceiveTimeout.Push(this, socket);
                                    Interlocked.Exchange(ref sendAsyncLock, 0);
                                    return true;
                                }
                                Interlocked.Exchange(ref sendAsyncLock, 0);
                                goto START;
#endif
                                }
                                break;
                            case SendType.GetForm:
                                if (getForm()) return true;
                                break;
                        }
                    }
                    else if ((count >= TcpServer.Server.MinSocketSize || (count > 0 && SendSizeLessCount++ == 0)) && Date.NowTime.Now <= Timeout)
                    {
#if DOTNET2
                        if (socket == Socket)
                        {
                            async = socket.BeginSend(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                return true;
                            }
                        }
#else
                    if ((socket = Socket) != null)
                    {
                        while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                        sendAsyncEventArgs.SetBuffer(Data.Start, Data.Length);
                        if (socket.SendAsync(sendAsyncEventArgs))
                        {
                            Http.Header.ReceiveTimeout.Push(this, socket);
                            Interlocked.Exchange(ref sendAsyncLock, 0);
                            return true;
                        }
                        Interlocked.Exchange(ref sendAsyncLock, 0);
                        goto START;
                    }
#endif
                    }
#if DOTNET2
                }
#endif
            }
            return false;
        }
        ///// <summary>
        ///// 获取请求表单数据[TRY+1]
        ///// </summary>
        ///// <param name="identity">HTTP操作标识</param>
        ///// <param name="page">WEB 页面</param>
        ///// <param name="type">获取请求表单数据回调类型</param>
        ///// <returns>是否匹配会话标识</returns>
        //internal override bool GetForm(long identity, AutoCSer.WebView.Page page, GetFormType type)
        //{
        //    if (Interlocked.CompareExchange(ref this.Identity, identity + 1, identity) == identity)
        //    {
        //        GetFormPage = page;
        //        GetFormType = type;
        //        tryGetForm();
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// 获取请求表单数据
        /// </summary>
        /// <param name="page">WEB 页面</param>
        /// <param name="type">获取请求表单数据回调类型</param>
        internal override void GetForm(AutoCSer.WebView.Page page, GetFormType type)
        {
            ++this.Identity;
            GetFormPage = page;
            GetFormType = type;

            Flag |= SocketFlag.GetForm | SocketFlag.IsLoadForm;
            try
            {
                if (Header.Is100Continue == 0)
                {
                    if (getForm()) return;
                }
                else
                {
                    System.Net.Sockets.Socket socket = Socket;
                    if (socket != null)
                    {
                        SendType = SendType.GetForm;
                        Data.Set(continue100, 0, continue100.Length);
                        Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                            SocketError socketError;
                            IAsyncResult async = socket.BeginSend(continue100, 0, Data.Length, SocketFlags.None, out socketError, onSendAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                return;
                            }
#else
                        while (Interlocked.CompareExchange(ref sendAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                        sendAsyncEventArgs.SetBuffer(continue100, 0, Data.Length);
                        if (socket.SendAsync(sendAsyncEventArgs))
                        {
                            Http.Header.ReceiveTimeout.Push(this, socket);
                            Interlocked.Exchange(ref sendAsyncLock, 0);
                            return;
                        }
                        Interlocked.Exchange(ref sendAsyncLock, 0);
                        if (sendAsyncEventArgs.SocketError == SocketError.Success && sendAsyncEventArgs.BytesTransferred == Data.Length && getForm()) return;
#endif
                    }
                }
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error);
            }
            HeaderError();
        }
        /// <summary>
        /// 获取请求表单数据
        /// </summary>
        /// <returns></returns>
        private bool getForm()
        {
            switch (Header.PostType)
            {
                case PostType.Json:
                case PostType.Form:
                case PostType.Xml:
                case PostType.Data:
                    FormBuffer = GetBuffer(Header.ContentLength + 1);
                    Data.Set(FormBuffer.Buffer, FormBuffer.StartIndex, FormBufferReceiveEndIndex = Header.ContentLength);
                    Header.CopyToForm(ref Data);
                    if (Data.Length == 0) return OnGetForm();
                    System.Net.Sockets.Socket socket = Socket;
                    if (socket == null) return false;
                    ReceiveType = ReceiveType.GetForm;
                    Timeout = Config.GetTimeout(Data.Length);
#if DOTNET2
                    SocketError socketError;
                    IAsyncResult async = socket.BeginReceive(Data.Array, Data.Start, Data.Length, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return true;
                    }
                    return false;
#else
                    receiveAsyncEventArgs.SocketError = SocketError.Success;
                    while (Interlocked.CompareExchange(ref ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                    receiveAsyncEventArgs.SetBuffer(Data.Array, Data.Start, Data.Length);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
                        Http.Header.ReceiveTimeout.Push(this, socket);
                        Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                        return true;
                    }
                    Interlocked.Exchange(ref ReceiveAsyncLock, 0);
                    return onReceive();
#endif
                case PostType.FormData:
                    if (boundaryReceiver == null) boundaryReceiver = new SocketBoundaryReceiver(this);
                    boundaryReceiver.Receive();
                    return true;
                default: return false;
            }
        }
    }
}
