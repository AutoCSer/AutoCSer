using System;
using System.Net.Sockets;
using System.Net.Security;
using AutoCSer.Extension;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using AutoCSer.Net.HttpRegister;
using System.Security.Cryptography.X509Certificates;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字安全流
    /// </summary>
    internal unsafe sealed class SslSocket : SocketBase<SslSocket>, IDisposable
    {
        /// <summary>
        /// HTTP 头部
        /// </summary>
        internal readonly SslHeader Header;
        /// <summary>
        /// 安全网络流
        /// </summary>
        internal SslStream SslStream;
        /// <summary>
        /// 网络流
        /// </summary>
        internal NetworkStream NetworkStream;
        /// <summary>
        /// SSL 客户端 Hello 流
        /// </summary>
        private ServerNameIndication.HelloStream HelloStream;
        /// <summary>
        /// 身份验证完成处理
        /// </summary>
        internal readonly AsyncCallback AuthenticateCallback;
        /// <summary>
        /// 发送数据处理
        /// </summary>
        private readonly AsyncCallback sendCallback;
        /// <summary>
        /// 接受数据处理
        /// </summary>
        private readonly AsyncCallback receiveCallback;
        /// <summary>
        /// HTTP 套接字数据接收器
        /// </summary>
        private SslBoundaryReceiver boundaryReceiver;
        /// <summary>
        /// HTTP 套接字安全流
        /// </summary>
        internal SslSocket()
        {
            IsSsl = true;
            HttpHeader = Header = new SslHeader(this);
            AuthenticateCallback = onAuthenticate;
            sendCallback = onSend;
            receiveCallback = onReceive;
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~SslSocket()
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
            if (Buffer.PoolBuffer.Pool != null)
            {
                Buffer.Free();
                Header.Free();
            }
        }
        /// <summary>
        /// 开始处理新的请求
        /// </summary>
        /// <param name="server">HTTP 服务</param>
        /// <param name="socket">套接字</param>
        internal void Start(SslServer server, ref System.Net.Sockets.Socket socket)
        {
            Socket = socket;
            this.Server = server;
            socket = null;
            try
            {
                SslCertificate certificate = server.Certificate;
                if (certificate != null) SslStream = certificate.CreateSslStream(this);
                else
                {
                    NetworkStream = new NetworkStream(Socket, true);
                    if (HelloStream == null) HelloStream = new ServerNameIndication.HelloStream(this);
                    HelloStream.ReadHello();
                }
                return;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            HeaderError();
        }
        /// <summary>
        /// 身份验证完成处理
        /// </summary>
        /// <param name="result">异步操作状态</param>
        private void onAuthenticate(IAsyncResult result)
        {
            try
            {
                SslStream.EndAuthenticateAsServer(result);
                Header.Receive();
                return;
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            HeaderError();
        }
        /// <summary>
        /// HTTP 头部接收错误
        /// </summary>
        internal void HeaderError()
        {
            if (NetworkStream != null)
            {
                NetworkStream.Dispose();
                NetworkStream = null;
                if (SslStream != null)
                {
                    SslStream.Dispose();
                    SslStream = null;
                }
            }
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
                    Timeout = Config.GetTimeout(Data.Length);
                    if (!SslStream.BeginWrite(data, 0, data.Length, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                    return;
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
                            if (!SslStream.BeginRead(Buffer.Buffer, Buffer.StartIndex, Data.Length, receiveCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                            return;
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
                    ushort timeoutCount;
                    Timeout = Config.GetTimeout(Data.Length, out timeoutCount);
                    if (!SslStream.BeginWrite(Data.Array, Data.Start, Data.Length, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket, timeoutCount);
                    return true;
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
                                        ushort timeoutCount;
                                        Timeout = Config.GetTimeout(Data.Length, out timeoutCount);
                                        if (!SslStream.BeginWrite(Data.Array, Data.Start, Data.Length, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket, timeoutCount);
                                        return true;
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
                        Timeout = Config.GetTimeout(index);
                        if (!SslStream.BeginWrite(buffer.Buffer, buffer.StartIndex, index, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return true;
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
        /// <summary>
        /// 接收数据完成后的回调
        /// </summary>
        /// <param name="result"></param>
        private void onReceive(IAsyncResult result)
        {
            if (!result.CompletedSynchronously) Http.Header.ReceiveTimeout.Cancel(this);
            try
            {
                int count = SslStream.EndRead(result);
                Data.MoveStart(count);
                if (Data.Length == 0)
                {
                    ReceiveSizeLessCount = 0;
                    switch (ReceiveType)
                    {
                        case ReceiveType.Response:
                            if ((Header.ContentLength -= Buffer.Length) <= 0)
                            {
                                if (responseHeader()) return;
                            }
                            else
                            {
                                System.Net.Sockets.Socket socket = Socket;
                                if (socket != null)
                                {
                                    Data.Set(Buffer.Buffer, Buffer.StartIndex, Math.Min(Header.ContentLength, Buffer.Length));
                                    if (!SslStream.BeginRead(Data.Array, Data.Start, Data.Length, receiveCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                    return;
                                }
                            }
                            break;
                        case ReceiveType.GetForm:
                            if (OnGetForm()) return;
                            break;
                    }
                }
                else if ((count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0)) && Date.NowTime.Now <= Timeout)
                {
                    System.Net.Sockets.Socket socket = Socket;
                    if (socket != null)
                    {
                        if (!SslStream.BeginRead(Data.Array, Data.Start, Data.Length, receiveCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return;
                    }
                }
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            HeaderError();
        }
        /// <summary>
        /// 数据发送完成后的回调
        /// </summary>
        /// <param name="result"></param>
        private void onSend(IAsyncResult result)
        {
            if (!result.CompletedSynchronously) Http.Header.ReceiveTimeout.Cancel(this);
            if (result.IsCompleted)
            {
                System.Net.Sockets.Socket socket = Socket;
                if (socket != null)
                {
                    try
                    {
                        SslStream.EndWrite(result);
                        switch (SendType)
                        {
                            case SendType.Next:
                                if (receiveNext()) return;
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
                                        if ((socket = Socket) != null)
                                        {
                                            SendType = SendType.Next;
                                            ushort timeoutCount;
                                            Timeout = Config.GetTimeout(Data.Length, out timeoutCount);
                                            if (!SslStream.BeginWrite(Data.Array, Data.Start, Data.Length, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket, timeoutCount);
                                            return;
                                        }
                                        break;
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
                                if ((socket = Socket) != null)
                                {
                                    Data.Set(Buffer.StartIndex, (int)Math.Min(ResponseSize, Buffer.Length));
                                    if (SendFileStream.Read(Data.Array, Data.Start, Data.Length) == Data.Length)
                                    {
                                        if ((ResponseSize -= Data.Length) == 0) SendType = SendType.Next;
                                        if (!SslStream.BeginWrite(Data.Array, Data.Start, Data.Length, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                                        return;
                                    }
                                }
                                break;
                            case SendType.GetForm:
                                if (getForm()) return;
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
                    }
                }
            }
            HeaderError();
        }
        ///// <summary>
        ///// 获取请求表单数据
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
                        Timeout = Config.GetTimeout(Data.Length);
                        if (!SslStream.BeginWrite(continue100, 0, continue100.Length, sendCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                        return;
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
                    if (!SslStream.BeginRead(Data.Array, Data.Start, Data.Length, receiveCallback, this).CompletedSynchronously) Http.Header.ReceiveTimeout.Push(this, socket);
                    return true;
                case PostType.FormData:
                    if (boundaryReceiver == null) boundaryReceiver = new SslBoundaryReceiver(this);
                    boundaryReceiver.Receive();
                    return true;
                default: return false;
            }
        }
    }
}
