using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Net;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Threading;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// HTML 标题获取客户端
    /// </summary>
    internal sealed class HttpClient : SocketTimeoutLink
    {
        /// <summary>
        /// 请求域名缓冲区池
        /// </summary>
        private static readonly AutoCSer.SubBuffer.Pool hostBufferPool = AutoCSer.SubBuffer.Pool.GetPool(SubBuffer.Size.Byte256);
        /// <summary>
        /// HTTP服务版本号
        /// </summary>
        private static readonly byte[] httpVersion = (@" HTTP/1.1
Connection: Close
User-Agent: Mozilla/5.0 (" + AutoCSer.Pub.HttpSpiderUserAgent + @")
Host: ").getBytes();
        /// <summary>
        /// HASH重定向名称
        /// </summary>
        private static readonly byte[] googleEscapedFragment = ("?_escaped_fragment_=").getBytes();
        /// <summary>
        /// 关闭套接字0超时设置
        /// </summary>
        private static readonly LingerOption lingerOption = new LingerOption(true, 0);

        /// <summary>
        /// HTML 标题获取客户端任务池
        /// </summary>
        private readonly HttpTask task;
        /// <summary>
        /// 安全连接证书验证
        /// </summary>
        private RemoteCertificateValidationCallback validateCertificate;
        /// <summary>
        /// 安全连接完成处理
        /// </summary>
        private AsyncCallback sslCallback;
        /// <summary>
        /// 请求域名缓冲区
        /// </summary>
        private SubBuffer.PoolBufferFull hostBuffer;
        /// <summary>
        /// 收发数据缓冲区
        /// </summary>
        private SubBuffer.PoolBufferFull buffer;
#if DOTNET2
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private AsyncCallback socketCallback;
        /// <summary>
        /// 套接字错误
        /// </summary>
        private SocketError socketError = SocketError.Success;
        /// <summary>
        /// HTTP 服务端 IP 地址
        /// </summary>
        private IPEndPoint ipEndPoint;
#else
        /// <summary>
        /// 异步套接字操作
        /// </summary>
        private SocketAsyncEventArgs socketAsync;
#if !DotNetStandard
        /// <summary>
        /// .NET 底层线程安全 BUG 处理锁
        /// </summary>
        private int asyncLock;
#endif
#endif
        /// <summary>
        /// Uri 与回调函数信息
        /// </summary>
        private Uri uri;
        /// <summary>
        /// 安全连接
        /// </summary>
        private SslStream sslStream;
        /// <summary>
        /// HTTP响应编码
        /// </summary>
        private Encoding responseEncoding;
        /// <summary>
        /// HTML页面编码
        /// </summary>
        private Encoding htmlEncoding;
        /// <summary>
        /// 收发数据缓冲区字节长度
        /// </summary>
        private readonly int bufferSize;
        /// <summary>
        /// 当前剩余搜索字节数
        /// </summary>
        private int currentSearchSize;
        /// <summary>
        /// 请求域名字节长度
        /// </summary>
        private int hostSize;
        /// <summary>
        /// 数据缓冲区有效位置
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// 当前处理位置
        /// </summary>
        private int currentIndex;
        /// <summary>
        /// 输出内容字节长度
        /// </summary>
        private int contentLength;
        /// <summary>
        /// 最后一次分段字节长度
        /// </summary>
        private int chunkedLength;
        /// <summary>
        /// 安全连接错误信息
        /// </summary>
        private SslPolicyErrors? sslPolicyErrors;
        /// <summary>
        /// HTTP响应内容类型
        /// </summary>
        private BufferIndex contentType;
        /// <summary>
        /// HTML标题
        /// </summary>
        private BufferIndex title;
        /// <summary>
        /// 套接字异步操作类型
        /// </summary>
        private SocketAsyncType socketAsyncType;
        /// <summary>
        /// 是否安全连接
        /// </summary>
        private bool isHttps;
        /// <summary>
        /// 是否压缩数据
        /// </summary>
        private bool isGzip;
        /// <summary>
        /// 是否已经解析头部
        /// </summary>
        private bool isHeader;
        /// <summary>
        /// 是否存在HTML节点
        /// </summary>
        private bool isHtml;
        /// <summary>
        /// 是否分段传输
        /// </summary>
        private bool isChunked;
        /// <summary>
        /// 是否关闭连接
        /// </summary>
        private bool isCloseConnection;
        /// <summary>
        /// 是否位置压缩编码类型
        /// </summary>
        private bool isUnknownEncoding;
        /// <summary>
        /// 是否重定向
        /// </summary>
        private bool isLocation;
        /// <summary>
        /// HTML 标题获取客户端
        /// </summary>
        /// <param name="task">HTML 标题获取客户端任务池</param>
        internal HttpClient(HttpTask task)
        {
            this.task = task;
            bufferSize = task.BufferSize - sizeof(int);
            task.BufferPool.Get(ref buffer);
            hostBufferPool.Get(ref hostBuffer);
#if DOTNET2
            socketCallback = onSocket;
#else
            socketAsync = AutoCSer.Net.SocketAsyncEventArgsPool.Get();
            socketAsync.Completed += onSocket;
            socketAsync.SetBuffer(buffer.Buffer, buffer.StartIndex, bufferSize);
#endif
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
#if !DOTNET2
            if (socketAsync != null)
            {
                socketAsync.Completed -= onSocket;
                AutoCSer.Net.SocketAsyncEventArgsPool.PushNotNull(ref socketAsync);
            }
#endif
            buffer.TryFree();
            hostBuffer.TryFree();
        }
        /// <summary>
        /// 获取HTML标题回调
        /// </summary>
        /// <param name="title">HTML标题</param>
        private void callback(string title)
        {
            try
            {
                Uri uri = this.uri;
                if (sslStream != null)
                {
                    sslStream.Dispose();
                    sslStream = null;
                }
                //DisposeSocket();
                if (Socket != null)
                {
#if DotNetStandard
                    AutoCSer.Net.TcpServer.CommandBase.CloseClientNotNull(Socket);
#else
                    Socket.Dispose();
#endif
                    Socket = null;
                }
                this.uri = null;
                if (task.Push(this) != 0) Free();
                uri.Callback(title);
            }
            catch (Exception error)
            {
                task.Log.Add(Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 获取HTML标题回调
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="encoding">HTML标题编码</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void callback(ref SubBuffer.PoolBufferFull buffer, Encoding encoding)
        {
            callback(encoding.GetString(buffer.Buffer, buffer.StartIndex + title.StartIndex, title.Length - title.StartIndex));
        }
        /// <summary>
        /// 获取 HTML 标题
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        internal unsafe bool Get(Uri uri)
        {
            string uriString = uri.UriString;
            this.uri = uri;
            if (uriString == null) return get(ref uri.UriBytes, 0, false);
            AutoCSer.SubBuffer.Pool pool = AutoCSer.SubBuffer.Pool.GetPool(uriString.Length);
            SubBuffer.PoolBufferFull buffer = default(SubBuffer.PoolBufferFull);
            pool.Get(ref buffer);
            try
            {
                fixed (char* uriFixed = uriString)
                fixed (byte* dataFixed = buffer.Buffer)
                {
                    AutoCSer.Extension.StringExtension.WriteBytesNotNull(uriFixed, uriString.Length, dataFixed + buffer.StartIndex);
                }
                SubArray<byte> uriArray = new SubArray<byte>(buffer.StartIndex, uriString.Length, buffer.Buffer);
                return get(ref uriArray, 0, false);
            }
            finally { buffer.Free(); }
        }
        /// <summary>
        /// 获取HTML标题
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="defaultPort">默认请求端口</param>
        /// <param name="isLocation">是否重定向请求</param>
        /// <returns></returns>
        private unsafe bool get(ref SubArray<byte> uri, int defaultPort, bool isLocation)
        {
            fixed (byte* uriFixed = uri.Array)
            {
                byte* start = uriFixed + uri.StartIndex;
                int hostIndex = 0;
                if ((*(int*)start | 0x20202020) == 'h' + ('t' << 8) + ('t' << 16) + ('p' << 24))
                {
                    int code = *(int*)(start + sizeof(int));
                    if ((code & 0xffffff) == (':' + ('/' << 8) + ('/' << 16))) hostIndex = 7;
                    else if ((code | 0x20) == 's' + (':' << 8) + ('/' << 16) + ('/' << 24)) hostIndex = 8;
                }
                if (hostIndex != 0)
                {
                    byte* end = start + uri.Length, nameStart = (start += hostIndex);
                    while (start != end && *start != '/' && *start != '?' && *start != '#' && *start != ':') ++start;
                    int hostSize = (int)(start - nameStart);
                    if (hostSize <= hostBuffer.Length)
                    {
                        SubArray<byte> host = new SubArray<byte>((int)(nameStart - uriFixed), hostSize, uri.Array);
                        IPAddress[] ips = DomainIPAddress.Get(ref host);
                        if (ips.length() != 0)
                        {
                            int port;
                            if (start != end && *start == ':')
                            {
                                port = 0;
                                while (++start != end)
                                {
                                    byte value = *start;
                                    if ((value -= (byte)'0') < 10)
                                    {
                                        port *= 10;
                                        port += value;
                                    }
                                    else break;
                                }
                                if (start != end && *start != '/' && *start != '?' && *start != '#') port = 0;
                            }
                            else port = defaultPort == 0 ? (hostIndex == 8 ? 443 : 80) : defaultPort;
                            if (port != 0)
                            {
                                SubArray<byte> path = default(SubArray<byte>), hash = default(SubArray<byte>);
                                for (nameStart = start; start != end && *start != '?' && *start != '#'; ++start) ;
                                if (start == end) path.Set(uri.Array, (int)(nameStart - uriFixed), (int)(start - nameStart));
                                else
                                {
                                    if (*start == '?')
                                    {
                                        while (++start != end && *start != '#') ;
                                        path.Set(uri.Array, (int)(nameStart - uriFixed), (int)(start - nameStart));
                                    }
                                    else
                                    {
                                        path.Set(uri.Array, (int)(nameStart - uriFixed), (int)(start - nameStart));
                                        if (++start != end || *start == '!') hash.Set(uri.Array, (int)(++start - uriFixed), (int)(end - start));
                                    }
                                }
                                int index = sizeof(int) * 2 + path.Length + httpVersion.Length + hostSize, hashCount = 0;
                                if (hash.Length != 0)
                                {
                                    index += googleEscapedFragment.Length + hash.Length;
                                    for (start = uriFixed + hash.StartIndex, end = start + hash.Length; start != end; ++start)
                                    {
                                        if ((uint)(*start - '0') >= 10 && (uint)((*start | 0x20) - 'a') >= 26) ++hashCount;
                                    }
                                    index += hashCount << 1;
                                }
                                if (index < buffer.Length)
                                {
                                    fixed (byte* bufferFixed = buffer.Buffer)
                                    {
                                        byte* bufferStart = bufferFixed + buffer.StartIndex;
                                        *(int*)bufferStart = 'G' + ('E' << 8) + ('T' << 16) + (' ' << 24);
                                        if (path.Length == 0)
                                        {
                                            bufferStart[sizeof(int)] = (byte)'/';
                                            index = sizeof(int) + 1;
                                        }
                                        else
                                        {
                                            if (uriFixed[path.StartIndex] != '/')
                                            {
                                                bufferStart[sizeof(int)] = (byte)'/';
                                                index = sizeof(int) + 1;
                                            }
                                            else index = sizeof(int);
                                            Buffer.BlockCopy(uri.Array, path.StartIndex, buffer.Buffer, buffer.StartIndex + index, path.Length);
                                            index += path.Length;
                                            if (hash.Length != 0)
                                            {
                                                Buffer.BlockCopy(googleEscapedFragment, 0, buffer.Buffer, buffer.StartIndex + index, googleEscapedFragment.Length);
                                                index += googleEscapedFragment.Length;
                                                if (hashCount == 0)
                                                {
                                                    Buffer.BlockCopy(uri.Array, hash.StartIndex, buffer.Buffer, buffer.StartIndex + index, hash.Length);
                                                    index += hash.Length;
                                                }
                                                else
                                                {
                                                    nameStart = bufferStart + index;
                                                    for (start = uriFixed + hash.StartIndex, end = start + hash.Length; start != end; ++start)
                                                    {
                                                        if ((uint)(*start - '0') >= 10 && (uint)((*start | 0x20) - 'a') >= 26)
                                                        {
                                                            *nameStart++ = (byte)'%';
                                                            *nameStart++ = (byte)Number.ToHex((uint)*start >> 4);
                                                            *nameStart++ = (byte)Number.ToHex((uint)*start & 0xf);
                                                        }
                                                        else *nameStart++ = *start;
                                                    }
                                                    index = (int)(nameStart - bufferStart);
                                                }
                                            }
                                        }
                                        Buffer.BlockCopy(httpVersion, 0, buffer.Buffer, buffer.StartIndex + index, httpVersion.Length);
                                        index += httpVersion.Length;
                                        Buffer.BlockCopy(uri.Array, host.StartIndex, buffer.Buffer, buffer.StartIndex + index, hostSize);
                                        *(int*)(bufferStart + (index += hostSize)) = 0x0a0d0a0d;
                                        index += sizeof(int);
                                    }
                                    bufferIndex = index + buffer.StartIndex;
                                    isHttps = hostIndex == 8;
                                    IPAddress ip = ips[0];
                                    Buffer.BlockCopy(uri.Array, host.StartIndex, hostBuffer.Buffer, hostBuffer.StartIndex, this.hostSize = hostSize);
                                    Socket socket = Socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                    socket.LingerState = lingerOption;
                                    socketAsyncType = SocketAsyncType.Connect;
                                    this.isLocation = isLocation;
#if DOTNET2
                                    socket.SendBufferSize = (int)HttpTask.MinBufferSize;
                                    socket.ReceiveBufferSize = Math.Min(4 << 10, buffer.Length);
                                    ipEndPoint = new IPEndPoint(ip, port);
                                    IAsyncResult async = socket.BeginConnect(ipEndPoint, socketCallback, socket);
                                    if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                                    return true;
#else
                                    if (isHttps) socketAsync.SetBuffer(buffer.StartIndex, 0);
                                    else
                                    {
                                        socket.SendBufferSize = (int)HttpTask.MinBufferSize;
                                        socket.ReceiveBufferSize = Math.Min(4 << 10, buffer.Length);
                                        socketAsync.SetBuffer(buffer.StartIndex, bufferIndex - buffer.StartIndex);
                                    }
                                    socketAsync.RemoteEndPoint = new IPEndPoint(ip, port);
                                    if (socket.ConnectAsync(socketAsync))
                                    {
                                        task.PushTimeout(this, socket);
                                        return true;
                                    }
                                    //if (socketAsync.SocketError == SocketError.Success)
                                    //{
                                    //    onSocket(null, socketAsync);
                                    //    return true;
                                    //}
#endif
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
#if DOTNET2
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private unsafe void onSocket(IAsyncResult async)
#else
        /// <summary>
        /// 套接字连接结束操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async"></param>
        private unsafe void onSocket(object sender, SocketAsyncEventArgs async)
#endif
        {
#if DOTNET2
            if (!async.CompletedSynchronously) task.CancelTimeout(this);
            Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
            if (socket == Socket)
            {
#else
            task.CancelTimeout(this);
            if (async.SocketError == SocketError.Success)
            {
                Socket socket = Socket;
#endif
                try
                {
                    switch (socketAsyncType)
                    {
                        case SocketAsyncType.Connect:
                            if (isHttps)
                            {
                                string host;
                                fixed (byte* hostFixed = hostBuffer.Buffer) host = AutoCSer.Extension.Memory_WebClient.BytesToStringNotEmpty(hostFixed + hostBuffer.StartIndex, hostSize);
                                if (validateCertificate == null)
                                {
                                    validateCertificate = onValidateCertificate;
                                    sslCallback = onSslSocket;
                                }
                                sslPolicyErrors = null;
                                sslStream = new SslStream(new NetworkStream(Socket, true), false, validateCertificate, null);
                                IAsyncResult result = sslStream.BeginAuthenticateAsClient(host, sslCallback, socket);
                                if (!result.CompletedSynchronously) task.PushTimeout(this, socket);
                                return;
                            }
#if DOTNET2
                            socketAsyncType = SocketAsyncType.Send;
                            async = socket.BeginSend(buffer.Buffer, buffer.StartIndex, bufferIndex - buffer.StartIndex, SocketFlags.None, out socketError, socketCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                                return;
                            }
                            break;
#else
                            if (async.BytesTransferred == 0)
                            {
                                socketAsyncType = SocketAsyncType.Send;
#if !DotNetStandard
                                while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                                socketAsync.SetBuffer(buffer.StartIndex, bufferIndex - buffer.StartIndex);
                                if (socket.SendAsync(socketAsync))
                                {
                                    task.PushTimeout(this, socket);
#if !DotNetStandard
                                    Interlocked.Exchange(ref asyncLock, 0);
#endif
                                    return;
                                }
#if !DotNetStandard
                                Interlocked.Exchange(ref asyncLock, 0);
#endif
                                if (socketAsync.SocketError != SocketError.Success) break;
                            }
                            goto ONSEND;
#endif
                        case SocketAsyncType.Send:
#if DOTNET2
                            if (socket.EndSend(async, out socketError) == bufferIndex - buffer.StartIndex && socketError == SocketError.Success)
                            {
                                isHeader = false;
                                socketAsyncType = SocketAsyncType.Recieve;
                                async = socket.BeginReceive(buffer.Buffer, bufferIndex = currentIndex = buffer.StartIndex, bufferSize, SocketFlags.None, out socketError, socketCallback, socket);
                                if (socketError == SocketError.Success)
                                {
                                    if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                                    return;
                                }
                            }
#else
                            ONSEND:
                            if (async.BytesTransferred == bufferIndex - buffer.StartIndex)
                            {
                                isHeader = false;
                                socketAsyncType = SocketAsyncType.Recieve;
                                bufferIndex = currentIndex = buffer.StartIndex;
#if !DotNetStandard
                                while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                                socketAsync.SetBuffer(bufferIndex, bufferSize);
                                if (socket.ReceiveAsync(socketAsync))
                                {
                                    task.PushTimeout(this, socket);
#if !DotNetStandard
                                    Interlocked.Exchange(ref asyncLock, 0);
#endif
                                    return;
                                }
                                if (socketAsync.SocketError == SocketError.Success)
                                {
#if !DotNetStandard
                                    Interlocked.Exchange(ref asyncLock, 0);
#endif
                                    goto ONRECEIVE;
                                }
                            }
#endif
                            break;
                        case SocketAsyncType.Recieve:
#if DOTNET2
                            int count = socket.EndReceive(async, out socketError);
                            if (socketError == SocketError.Success && onReceive(count)) return;
#else
                            ONRECEIVE:
                            if (onReceive(async.BytesTransferred)) return;
#endif
                            break;
                    }
                }
                catch (Exception error)
                {
                    task.Log.Add(Log.LogType.Error, error);
                }
            }
            callback(null);
        }
        /// <summary>
        /// 安全连接证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool onValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            this.sslPolicyErrors = sslPolicyErrors;
            return !task.IsValidateCertificate || sslPolicyErrors == SslPolicyErrors.None;
        }
        /// <summary>
        /// 证书验证完成处理
        /// </summary>
        /// <param name="async">异步操作状态</param>
        private void onSslSocket(IAsyncResult async)
        {
            if (!async.CompletedSynchronously) task.CancelTimeout(this);
            Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
            if (socket == Socket)
            {
                try
                {
                    switch (socketAsyncType)
                    {
                        case SocketAsyncType.Connect:
                            if (!task.IsValidateCertificate || sslPolicyErrors == SslPolicyErrors.None)
                            {
                                sslStream.EndAuthenticateAsClient(async);
#if !DOTNET2
                                socket.SendBufferSize = (int)HttpTask.MinBufferSize;
                                socket.ReceiveBufferSize = Math.Min(4 << 10, buffer.Length);
#endif
                                socketAsyncType = SocketAsyncType.Send;
                                async = sslStream.BeginWrite(buffer.Buffer, buffer.StartIndex, bufferIndex - buffer.StartIndex, sslCallback, socket);
                                if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                                return;
                            }
                            break;
                        case SocketAsyncType.Send:
                            sslStream.EndWrite(async);
                            isHeader = false;
                            socketAsyncType = SocketAsyncType.Recieve;
                            async = sslStream.BeginRead(buffer.Buffer, bufferIndex = currentIndex = buffer.StartIndex, bufferSize, sslCallback, socket);
                            if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                            return;
                        case SocketAsyncType.Recieve:
                            if (onReceive(sslStream.EndRead(async))) return;
                            break;
                    }
                }
                catch (Exception error)
                {
                    task.Log.Add(Log.LogType.Error, error);
                }
            }
            callback(null);
        }
        /// <summary>
        /// 接收数据操作
        /// </summary>
        /// <param name="count">接收数据字节数</param>
        /// <returns>是否处理完毕</returns>
        private unsafe bool onReceive(int count)
        {
#if !DOTNET2
            START:
#endif
            if (count > 0)
            {
                bufferIndex += count;
                if (isHeader)
                {
                    fixed (byte* bufferFixed = buffer.Buffer)
                    {
                        if (isChunked)
                        {
                            checkChunked(bufferFixed);
                            if (bufferIndex - buffer.StartIndex > contentLength && !isChunked) return false;
                        }
                        if (isGzip)
                        {
                            if (bufferIndex - buffer.StartIndex == contentLength && !isChunked) return parseGZip();
                        }
                        else if (parseTitle(ref buffer)) return true;
                        else if (bufferIndex - buffer.StartIndex == bufferSize && isHtml && currentSearchSize > 0)
                        {
                            int index = currentIndex - buffer.StartIndex;
                            if (title.StartIndex != 0 && index > title.StartIndex) index = title.StartIndex - 1;
                            if (index == 0)
                            {
                                if (isHeader && title.StartIndex != 0 && title.Length != 0)
                                {
                                    if (responseEncoding != null) callback(ref buffer, responseEncoding);
                                    else if (this.uri.Encoding != null) callback(ref buffer, this.uri.Encoding);
                                    else callback(ref buffer, AutoCSer.ChineseEncoder.ChineseEncoding(bufferFixed + buffer.StartIndex, isChunked || contentLength > bufferSize ? bufferSize : contentLength) ?? Encoding.UTF8);
                                    return true;
                                }
                                return false;
                            }
                            Buffer.BlockCopy(buffer.Buffer, buffer.StartIndex + index, buffer.Buffer, buffer.StartIndex, bufferIndex -= index);
                            currentSearchSize -= index;
                            currentIndex -= index;
                            if (title.StartIndex != 0)
                            {
                                title.StartIndex -= (short)index;
                                if (title.Length != 0) title.Length -= (short)index;
                            }
                        }
                    }
                }
                else
                {
                    int searchEndIndex = bufferIndex - sizeof(int);
                    if (currentIndex <= searchEndIndex)
                    {
                        fixed (byte* bufferFixed = buffer.Buffer)
                        {
                            byte* start = bufferFixed + currentIndex, searchEnd = bufferFixed + searchEndIndex, end = bufferFixed + bufferIndex;
                            *end = 13;
                            do
                            {
                                while (*start != 13) ++start;
                                if (start <= searchEnd)
                                {
                                    if (*(int*)start == 0x0a0d0a0d)
                                    {
                                        currentIndex = (int)(start - bufferFixed);
                                        bool isLocation = false;
                                        if (parseHeader(bufferFixed, ref isLocation))
                                        {
                                            if (isLocation) return true;
                                            if ((count = bufferIndex - (currentIndex += sizeof(int))) <= contentLength || isChunked)
                                            {
                                                Buffer.BlockCopy(buffer.Buffer, currentIndex, buffer.Buffer, buffer.StartIndex, count);
                                                bufferIndex = (currentIndex = buffer.StartIndex) + count;
                                                if (isChunked)
                                                {
                                                    chunkedLength = int.MinValue;
                                                    checkChunked(bufferFixed);
                                                    if (bufferIndex - buffer.StartIndex > contentLength && !isChunked) return false;
                                                }
                                                htmlEncoding = null;
                                                title.Value = 0;
                                                isHeader = true;
                                                isHtml = false;
                                                currentSearchSize = task.MaxSearchSize - bufferSize;
                                                if (isGzip)
                                                {
                                                    if (bufferIndex - buffer.StartIndex == contentLength && !isChunked) return parseGZip();
                                                }
                                                else if (parseTitle(ref buffer)) return true;
                                                break;
                                            }
                                        }
                                        return false;
                                    }
                                    ++start;
                                }
                                else
                                {
                                    currentIndex = (int)(start - bufferFixed);
                                    break;
                                }
                            }
                            while (true);
                        }
                    }
                }
                if ((count = (!isHeader || isChunked || contentLength > bufferSize ? bufferSize : contentLength) + buffer.StartIndex - bufferIndex) > 0)
                {
                    Socket socket = Socket;
                    if (isHttps)
                    {
                        IAsyncResult async = sslStream.BeginRead(buffer.Buffer, bufferIndex, count, sslCallback, socket);
                        if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                        return true;
                    }
#if DOTNET2
                    else
                    {
                        IAsyncResult async = socket.BeginReceive(buffer.Buffer, bufferIndex, count, SocketFlags.None, out socketError, socketCallback, socket);
                        if (socketError == SocketError.Success)
                        {
                            if (!async.CompletedSynchronously) task.PushTimeout(this, socket);
                            return true;
                        }
                    }
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    socketAsync.SetBuffer(bufferIndex, count);
                    if (socket.ReceiveAsync(socketAsync))
                    {
                        task.PushTimeout(this, socket);
#if !DotNetStandard
                        Interlocked.Exchange(ref asyncLock, 0);
#endif
                        return true;
                    }
                    if (socketAsync.SocketError == SocketError.Success)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref asyncLock, 0);
#endif
                        count = socketAsync.BytesTransferred;
                        goto START;
                    }
#endif
                }
                if (isGzip) return parseGZip();
                if (isHeader && title.StartIndex != 0 && title.Length != 0)
                {
                    callback(ref buffer, Encoding.UTF8);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 解析头部数据
        /// </summary>
        /// <param name="bufferFixed">数据起始位置</param>
        /// <param name="isLocation">是否重定向处理</param>
        /// <returns>是否成功</returns>
        private unsafe bool parseHeader(byte* bufferFixed, ref bool isLocation)
        {
            byte* bufferStart = bufferFixed + buffer.StartIndex, end = bufferFixed + currentIndex, current = bufferStart;
            for (*end = 32; *current != 32; ++current) ;
            if (current == end) return false;
            *end = 13;
            while (*++current == 32) ;
            if (current == end) return false;
            int state = 0;
            do
            {
                uint number = (uint)(*current - '0');
                if (number >= 10) break;
                state *= 10;
                state += (int)number;
                ++current;
            }
            while (true);
            while (*current != 13) ++current;
            if (state == 301 || state == 302)
            {
                if (!this.isLocation)
                {
                    this.isLocation = true;
                    while (current != end)
                    {
                        if ((current += 2) >= end) return false;
                        if (current + 10 < end && (((*(int*)current | 0x20202020) ^ ('l' + ('o' << 8) + ('c' << 16) + ('a' << 24)))
                            | ((*(int*)(current + sizeof(int)) | 0x20202020) ^ ('t' + ('i' << 8) + ('o' << 16) + ('n' << 24)))
                            | (*(short*)(current + sizeof(int) * 2) ^ (':' + (' ' << 8)))) == 0)
                        {
                            if (*(current += 10) == '/' || (((*(int*)current | 0x20202020) ^ ('h' + ('t' << 8) + ('t' << 16) + ('p' << 24))) == 0
                                && (((*(int*)(current + sizeof(int)) & 0xffffff) ^ (':' + ('/' << 8) + ('/' << 16))) == 0 || ((*(int*)(current + sizeof(int)) | 0x20) ^ ('s' + (':' << 8) + ('/' << 16) + ('/' << 24))) == 0)))
                            {
                                byte* start = current;
                                while (*++current != 13) ;
                                int length = (int)(current - start);
                                AutoCSer.SubBuffer.PoolBufferFull uri = default(AutoCSer.SubBuffer.PoolBufferFull);
                                if (*start == '/')
                                {
#if DOTNET2
                                int hostIndex = isHttps ? 8 : 7, uriLength = length + hostSize + hostIndex, port = ipEndPoint.Port;
#else
                                    int hostIndex = isHttps ? 8 : 7, uriLength = length + hostSize + hostIndex, port = ((IPEndPoint)socketAsync.RemoteEndPoint).Port;
#endif
                                    AutoCSer.SubBuffer.Pool pool = AutoCSer.SubBuffer.Pool.GetPool(uriLength);
                                    pool.Get(ref uri);
                                    try
                                    {
                                        fixed (byte* uriFixed = uri.Buffer)
                                        {
                                            byte* uriStart = uriFixed + uri.StartIndex;
                                            * (int*)uriStart = 'h' + ('t' << 8) + ('t' << 16) + ('p' << 24);
                                            *(int*)(uriStart + sizeof(int)) = isHttps ? ('s' + (':' << 8) + ('/' << 16) + ('/' << 24)) : (':' + ('/' << 8) + ('/' << 16));
                                        }
                                        Buffer.BlockCopy(hostBuffer.Buffer, hostBuffer.StartIndex, uri.Buffer, uri.StartIndex + hostIndex, hostSize);
                                        Buffer.BlockCopy(buffer.Buffer, (int)(start - bufferFixed), uri.Buffer, uri.StartIndex + hostSize + hostIndex, length);
#if DotNetStandard
                                    AutoCSer.Net.TcpServer.CommandBase.CloseClientNotNull(Socket);
#else
                                        Socket.Dispose();
#endif
                                        Socket = null;
                                        SubArray<byte> uriArray = new SubArray<byte>(uri.StartIndex, uriLength, uri.Buffer);
                                        get(ref uriArray, port, true);
                                    }
                                    finally { uri.Free(); }
                                }
                                else
                                {
                                    AutoCSer.SubBuffer.Pool pool = AutoCSer.SubBuffer.Pool.GetPool(length);
                                    pool.Get(ref uri);
                                    try
                                    {
                                        Buffer.BlockCopy(buffer.Buffer, (int)(start - bufferFixed), uri.Buffer, uri.StartIndex, length);
#if DotNetStandard
                                    AutoCSer.Net.TcpServer.CommandBase.CloseClientNotNull(Socket);
#else
                                        Socket.Dispose();
#endif
                                        Socket = null;
                                        SubArray<byte> uriArray = new SubArray<byte>(uri.StartIndex, length, uri.Buffer);
                                        get(ref uriArray, 0, true);
                                    }
                                    finally { uri.Free(); }
                                }
                                return isLocation = true;
                            }
                        }
                        while (*current != 13) ++current;
                    }
                }
                return false;
            }
            //if (state == 200)
            {
                contentType.Value = contentLength = 0;
                isGzip = isUnknownEncoding = isChunked = isCloseConnection = false;
                while (current != end)
                {
                    if ((current += 2) >= end) return false;
                    byte* start = current;
                    for (*end = (byte)':'; *current != (byte)':'; ++current) ;
                    if (current == end) return false;
                    SubArray<byte> name = new SubArray<byte>((int)(start - bufferFixed), (int)(current - start), buffer.Buffer);
                    *end = 13;
                    while (*++current == ' ') ;
                    for (start = current; *current != 13; ++current) ;
                    Action<HttpClient, BufferIndex> parseHeaderName = null;
                    parses.Get(name, ref parseHeaderName);
                    if (parseHeaderName != null) parseHeaderName(this, new BufferIndex { StartIndex = (short)(start - bufferStart), Length = (short)(current - start) });
                }
                if (!isChunked && isCloseConnection && contentLength == 0) contentLength = int.MaxValue;
                if ((isChunked ? contentLength == 0 : (contentLength > 0 && (!isGzip || contentLength <= bufferSize))) && !isUnknownEncoding)
                {
                    if (contentType.Length == 0) return true;
                    int isHtml = 0, isEncoding = 0;
                    current = bufferStart + contentType.StartIndex;
                    end = current + contentType.Length;
                    while (current != end)
                    {
                        if (isHtml == 0)
                        {
                            if ((((*(int*)current | 0x20202020) ^ ('t' + ('e' << 8) + ('x' << 16) + ('t' << 24)))
                                | ((*(int*)(current + sizeof(int)) | 0x20202000) ^ ('/' + ('h' << 8) + ('t' << 16) + ('m' << 24)))
                                | ((*(current + sizeof(int) * 2) | 0x20) ^ 'l')) == 0)
                            {
                                isHtml = 1;
                                if (this.uri.Encoding != null || isEncoding != 0) return true;
                                current += 9;
                            }
                        }
                        if (isEncoding == 0)
                        {
                            if ((((*(int*)current | 0x20202020) ^ ('c' + ('h' << 8) + ('a' << 16) + ('r' << 24)))
                                | ((*(int*)(current + sizeof(int)) | 0x202020) ^ ('s' + ('e' << 8) + ('t' << 16) + ('=' << 24)))) == 0)
                            {
                                if ((current += 8) >= end) return false;
                                isEncoding = 1;
                                byte* start = current;
                                while (*current != 13 && *current != ';') ++current;
                                string encodingString = AutoCSer.Extension.Memory_WebClient.BytesToStringNotEmpty(start, (int)(current - start));
                                try
                                {
                                    responseEncoding = AutoCSer.EncodingCacheOther.GetEncoding(encodingString);
                                }
                                catch
                                {
                                    task.Log.Add(Log.LogType.Info, "编码解析错误 " + encodingString, (StackFrame)null, true);
                                    htmlEncoding = Encoding.UTF8;
                                }
                                if (isHtml != 0) return true;
                            }
                        }
                        for (*end = (byte)';'; *current != ';'; ++current) ;
                        if (current == end) break;
                        while (*++current == ' ') ;
                    }
                    if (isHtml != 0) return true;
                }
                return false;
            }
            //return false;
        }
        /// <summary>
        /// 检测分段传输长度
        /// </summary>
        /// <param name="bufferFixed">数据起始位置</param>
        private unsafe void checkChunked(byte* bufferFixed)
        {
            int startIndex = buffer.StartIndex + contentLength, copyIndex = startIndex;
            if (chunkedLength == int.MinValue)
            {
                if (bufferIndex - buffer.StartIndex < 3) return;
                byte* start = bufferFixed + buffer.StartIndex, end = bufferFixed + bufferIndex;
                *end = 13;
                int length = 0;
                do
                {
                    uint code = (uint)(*start - '0');
                    if (code < 10)
                    {
                        length <<= 4;
                        length += (int)code;
                    }
                    else if ((code = (uint)((*start | 0x20) - 'a')) <= ('f' - 'a'))
                    {
                        length <<= 4;
                        length += (int)code + 10;
                    }
                    else
                    {
                        int count = (int)(end - start);
                        if (count < 2) return;
                        if (*(short*)start != 0x0a0d)// || (isGzip && length > bufferSize)
                        {
                            isChunked = false;
                            contentLength = 0;
                            return;
                        }
                        chunkedLength = contentLength = length;
                        int index = (int)(start - bufferFixed) + 2;
                        if ((count -= 2) < length)
                        {
                            Buffer.BlockCopy(buffer.Buffer, index, buffer.Buffer, buffer.StartIndex, bufferIndex -= (index - buffer.StartIndex));
                            return;
                        }
                        Buffer.BlockCopy(buffer.Buffer, index, buffer.Buffer, buffer.StartIndex, length);
                        startIndex = length + buffer.StartIndex;
                        copyIndex = length + index;
                        break;
                    }
                    ++start;
                }
                while (true);
            }
            do
            {
                int count = bufferIndex - copyIndex;
                if (count < 2) goto COPY;
                byte* start = bufferFixed + copyIndex;
                if (*(short*)start != 0x0a0d)
                {
                    contentLength = 0;
                    isChunked = false;
                    return;
                }
                if (chunkedLength == 0)
                {
                    if (count == 2) bufferIndex = buffer.StartIndex + contentLength;
                    else contentLength = 0;
                    isChunked = false;
                    return;
                }
                if (count < 5) goto COPY;
                byte* end = bufferFixed + bufferIndex;
                start += 2;
                *end = 13;
                int length = 0;
                do
                {
                    uint code = (uint)(*start - '0');
                    if (code < 10)
                    {
                        length <<= 4;
                        length += (int)code;
                    }
                    else if ((code = (uint)((*start | 0x20) - 'a')) <= ('f' - 'a'))
                    {
                        length <<= 4;
                        length += (int)code + 10;
                    }
                    else
                    {
                        if ((count = (int)(end - start)) < 2) goto COPY;
                        if (*(short*)start != 0x0a0d)// || ((contentLength + length) > bufferSize && isGzip)
                        {
                            isChunked = false;
                            contentLength = 0;
                            return;
                        }
                        contentLength += length;
                        chunkedLength = length;
                        int index = (int)(start - bufferFixed) + 2;
                        if ((count -= 2) < length)
                        {
                            Buffer.BlockCopy(buffer.Buffer, index, buffer.Buffer, startIndex, count);
                            bufferIndex = startIndex + count;
                            return;
                        }
                        Buffer.BlockCopy(buffer.Buffer, index, buffer.Buffer, startIndex, length);
                        startIndex += length;
                        copyIndex = length + index;
                        break;
                    }
                    ++start;
                }
                while (true);
            }
            while (true);
        COPY:
            if (startIndex != copyIndex)
            {
                int count = bufferIndex - copyIndex;
                Buffer.BlockCopy(buffer.Buffer, copyIndex, buffer.Buffer, startIndex, count);
                bufferIndex = startIndex + count;
            }
        }
        /// <summary>
        /// 解析HTML标题
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <returns>是否成功</returns>
        private unsafe bool parseTitle(ref SubBuffer.PoolBufferFull buffer)
        {
            fixed (byte* bufferFixed = buffer.Buffer)
            {
                byte* bufferStart = bufferFixed + buffer.StartIndex, current = bufferFixed + currentIndex, end = bufferFixed + bufferIndex;
                do
                {
                    byte* start = current;
                    for (*end = (byte)'<'; *current != '<'; ++current) ;
                    if (current == end)
                    {
                        if ((title.StartIndex | title.Length) == 0) currentIndex = isChunked ? Math.Min(buffer.StartIndex + contentLength, bufferIndex) : bufferIndex;
                        return false;
                    }
                    if (title.StartIndex != 0 && title.Length == 0)
                    {
                        title.Length = (short)(current - bufferStart);
                        if (htmlEncoding != null)
                        {
                            callback(ref buffer, htmlEncoding);
                            return true;
                        }
                        bool isAscii = true;
                        while (*start != '<')
                        {
                            if ((uint)(*start++ - 32) > (126 - 32))
                            {
                                isAscii = false;
                                break;
                            }
                        }
                        if (isAscii)
                        {
                            callback(ref buffer, Encoding.ASCII);
                            return true;
                        }
                    }
                    currentIndex = (int)(current - bufferFixed);
                    for (*end = (byte)'>', start = current; *current != '>'; ++current) ;
                    if (current == end) return false;
                    int tagName = *(int*)++start | 0x20202020;
                    if (tagName == 'm' + ('e' << 8) + ('t' << 16) + ('a' << 24) && htmlEncoding == null)
                    {
                        *current = (byte)'=';
                        start += sizeof(int);
                        do
                        {
                            while (*start != '=') ++start;
                            if (start == current) break;
                            if ((*(int*)(++start - sizeof(int)) | 0x202020) == 's' + ('e' << 8) + ('t' << 16) + ('=' << 24)
                                && (*(int*)(start - sizeof(int) * 2) | 0x20202020) == 'c' + ('h' << 8) + ('a' << 16) + ('r' << 24))
                            {
                                while (*start == '"' || *start == '\'') ++start;
                                byte* encoding = start;
                                while ((uint)((*start | 0x20) - 'a') < 26 || (uint)(*start - '0') < 10 || *start == '-') ++start;
                                string encodingString = AutoCSer.Extension.Memory_WebClient.BytesToStringNotEmpty(encoding, (int)(start - encoding));
                                try
                                {
                                    htmlEncoding = AutoCSer.EncodingCacheOther.GetEncoding(encodingString);
                                }
                                catch
                                {
                                    task.Log.Add(Log.LogType.Info, "编码解析错误 " + encodingString, (StackFrame)null, true);
                                    htmlEncoding = Encoding.UTF8;
                                }
                                if (htmlEncoding != null && title.StartIndex != 0)
                                {
                                    callback(ref buffer, htmlEncoding);
                                    return true;
                                }
                                break;
                            }
                        }
                        while (true);
                    }
                    else if (tagName == 'h' + ('t' << 8) + ('m' << 16) + ('l' << 24)) isHtml = true;
                    else if (tagName == 't' + ('i' << 8) + ('t' << 16) + ('l' << 24))
                    {
                        if ((*(start + sizeof(int)) | 0x20) == 'e') title.StartIndex = (short)((int)(current - bufferStart) + 1);
                    }
                    else if (tagName == 'O' + ('h' << 8) + ('e' << 16) + ('a' << 24)
                        || tagName == 'b' + ('o' << 8) + ('d' << 16) + ('y' << 24))
                    {
                        if (title.StartIndex == 0) callback(null);
                        else if (responseEncoding != null) callback(ref buffer, responseEncoding);
                        else if (this.uri.Encoding != null) callback(ref buffer, this.uri.Encoding);
                        else
                        {
                            callback(ref buffer, AutoCSer.ChineseEncoder.ChineseEncoding(bufferStart, (int)(start - bufferStart))
                                ?? AutoCSer.ChineseEncoder.ChineseEncoding(start, bufferIndex - (int)(start - bufferFixed))
                                ?? Encoding.UTF8);
                        }
                        return true;
                    }
                    currentIndex = (int)(++current - bufferFixed);
                }
                while (true);
            }
        }
        /// <summary>
        /// 解析gzip+HTML标题
        /// </summary>
        /// <returns>是否成功</returns>
        private unsafe bool parseGZip()
        {
            if (bufferIndex > buffer.StartIndex)
            {
                AutoCSer.SubBuffer.PoolBufferFull newBuffer = default(AutoCSer.SubBuffer.PoolBufferFull);
                task.BufferPool.Get(ref newBuffer);
                try
                {
                    int count = AutoCSer.IO.Compression.GzipDeCompressor.Get(buffer.Buffer, buffer.StartIndex, bufferIndex - buffer.StartIndex, ref newBuffer);
                    if (count != 0)
                    {
                        bufferIndex = (currentIndex = newBuffer.StartIndex) + count;
                        if (parseTitle(ref newBuffer)) return true;
                    }
                }
                finally { newBuffer.Free(); }
            }
            return false;
        }

        /// <summary>
        /// 提交内容数据长度解析
        /// </summary>
        /// <param name="client">HTML标题获取客户端</param>
        /// <param name="value">提交内容数据长度索引位置</param>
        private unsafe static void parseContentLength(HttpClient client, BufferIndex value)
        {
            fixed (byte* dataFixed = client.buffer.Buffer)
            {
                for (byte* start = dataFixed + client.buffer.StartIndex + value.StartIndex, end = start + value.Length; start != end; ++start)
                {
                    client.contentLength *= 10;
                    client.contentLength += *start - '0';
                }
            }
        }
        /// <summary>
        /// HTTP响应内容类型解析
        /// </summary>
        /// <param name="client">HTML标题获取客户端</param>
        /// <param name="value">HTTP响应内容类型索引位置</param>
        private static void parseContentType(HttpClient client, BufferIndex value)
        {
            client.contentType = value;
        }
        /// <summary>
        /// HTTP响应压缩编码类型解析
        /// </summary>
        /// <param name="client">HTML标题获取客户端</param>
        /// <param name="value">HTTP响应压缩编码类型索引位置</param>
        private unsafe static void parseContentEncoding(HttpClient client, BufferIndex value)
        {
            if (value.Length == 4)
            {
                fixed (byte* bufferFixed = client.buffer.Buffer)
                {
                    if ((*(int*)(bufferFixed + client.buffer.StartIndex + value.StartIndex) | 0x20202020) == ('g' + ('z' << 8) + ('i' << 16) + ('p' << 24)))
                    {
                        client.isGzip = true;
                        return;
                    }
                }
            }
            client.isUnknownEncoding = true;
        }
        /// <summary>
        /// HTTP响应传输编码类型解析
        /// </summary>
        /// <param name="client">HTML标题获取客户端</param>
        /// <param name="value">HTTP响应传输编码类型索引位置</param>
        private unsafe static void parseTransferEncoding(HttpClient client, BufferIndex value)
        {
            if (value.Length == 7)
            {
                fixed (byte* bufferFixed = client.buffer.Buffer)
                {
                    byte* start = bufferFixed + client.buffer.StartIndex + value.StartIndex;
                    if ((((*(int*)start | 0x20202020) ^ ('c' + ('h' << 8) + ('u' << 16) + ('n' << 24)))
                        | ((*(int*)(start + 3) | 0x20202020) ^ ('n' + ('k' << 8) + ('e' << 16) + ('d' << 24)))) == 0)
                    {
                        client.isChunked = true;
                    }
                }
            }
        }
        /// <summary>
        /// HTTP连接状态解析
        /// </summary>
        /// <param name="client">HTML标题获取客户端</param>
        /// <param name="value">HTTP连接状态索引位置</param>
        private unsafe static void parseConnection(HttpClient client, BufferIndex value)
        {
            if (value.Length == 5)
            {
                fixed (byte* bufferFixed = client.buffer.Buffer)
                {
                    byte* start = bufferFixed + client.buffer.StartIndex + value.StartIndex;
                    if ((((*(int*)start | 0x20202020) ^ ('c' + ('l' << 8) + ('o' << 16) + ('s' << 24)))
                        | ((*(start + sizeof(int)) | 0x20) ^ ('e'))) == 0)
                    {
                        client.isCloseConnection = true;
                    }
                }
            }
        }
        /// <summary>
        /// HTTP头名称解析委托
        /// </summary>
        private static readonly UniqueDictionary<HeaderName, Action<HttpClient, BufferIndex>> parses;
        static HttpClient()
        {
            KeyValue<HeaderName, Action<HttpClient, BufferIndex>>[] parseArray = new KeyValue<HeaderName, Action<HttpClient, BufferIndex>>[5];
            parseArray[0] = new KeyValue<HeaderName, Action<HttpClient, BufferIndex>>(HeaderName.ContentLengthBytes, parseContentLength);
            parseArray[1] = new KeyValue<HeaderName, Action<HttpClient, BufferIndex>>(HeaderName.ContentTypeBytes, parseContentType);
            parseArray[2] = new KeyValue<HeaderName, Action<HttpClient, BufferIndex>>(HeaderName.ContentEncodingBytes, parseContentEncoding);
            parseArray[3] = new KeyValue<HeaderName, Action<HttpClient, BufferIndex>>(HeaderName.TransferEncodingBytes, parseTransferEncoding);
            parseArray[4] = new KeyValue<HeaderName, Action<HttpClient, BufferIndex>>(HeaderName.ConnectionBytes, parseConnection);
            parses = new UniqueDictionary<HeaderName, Action<HttpClient, BufferIndex>>(parseArray, 8);
        }
    }
}
