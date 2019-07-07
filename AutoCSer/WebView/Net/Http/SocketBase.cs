using System;
using System.Threading;
using System.Text;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字
    /// </summary>
    public abstract unsafe class SocketBase : SocketTimeoutLink
    {
        /// <summary>
        /// 会话标识
        /// </summary>
        internal long Identity;
        /// <summary>
        /// 当前输出 HTTP 响应字节数
        /// </summary>
        internal long ResponseSize;
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long TaskTicks;
        /// <summary>
        /// 操作超时时间
        /// </summary>
        internal DateTime Timeout;
        /// <summary>
        /// HTTP 服务
        /// </summary>
        internal Server Server;
        /// <summary>
        /// HTTP 头部
        /// </summary>
        internal Header HttpHeader;
        /// <summary>
        /// 域名服务
        /// </summary>
        internal HttpDomainServer.Server DomainServer;
        /// <summary>
        /// 当前未完成 HTTP 响应输出
        /// </summary>
        internal Response HttpResponse;
        /// <summary>
        /// 当前发送文件
        /// </summary>
        internal FileStream SendFileStream;
        /// <summary>
        /// HTTP 请求表单
        /// </summary>
        internal Form Form;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 临时数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull BigBuffer;
        /// <summary>
        /// HTTP 请求表单数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull FormBuffer;
        /// <summary>
        /// HTTP 请求表单数据结束位置
        /// </summary>
        internal int FormBufferReceiveEndIndex;
        /// <summary>
        /// 获取请求表单数据回调对象
        /// </summary>
        internal AutoCSer.WebView.Page GetFormPage;
        /// <summary>
        /// 当前操作数据
        /// </summary>
        internal SubArray<byte> Data;
        /// <summary>
        /// HTTP 套接字标志位
        /// </summary>
        internal SocketFlag Flag;
        /// <summary>
        /// 获取请求表单数据回调类型
        /// </summary>
        internal GetFormType GetFormType;
        /// <summary>
        /// 当前发送数据类型
        /// </summary>
        internal SendType SendType;
        /// <summary>
        /// 当前接收数据类型
        /// </summary>
        internal ReceiveType ReceiveType;
        /// <summary>
        /// 是否 SSL 链接
        /// </summary>
        internal bool IsSsl;
        /// <summary>
        /// 发送数据量过低次数
        /// </summary>
        internal byte SendSizeLessCount;
        /// <summary>
        /// HTTP 套接字
        /// </summary>
        protected SocketBase()
        {
            BufferPool.Get(ref Buffer);
            Form = new Form(this);
        }
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="identity">操作标识</param>
        /// <param name="state">错误状态</param>
        /// <returns>是否匹配会话标识</returns>
        public abstract bool ResponseError(long identity, ResponseState state);
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="state">错误状态</param>
        internal abstract void ResponseErrorIdentity(ResponseState state);
        /// <summary>
        /// 输出HTTP响应数据
        /// </summary>
        /// <param name="identity">HTTP操作标识</param>
        /// <param name="response">HTTP响应数据</param>
        /// <returns>是否匹配会话标识</returns>
        public abstract bool Response(long identity, ref Response response);
        /// <summary>
        /// 输出HTTP响应数据
        /// </summary>
        /// <param name="response">HTTP响应数据</param>
        internal abstract void ResponseIdentity(ref Response response);
        /// <summary>
        /// 开始处理请求
        /// </summary>
        internal void Request()
        {
            long identity = this.Identity;
            try
            {
                if (HttpHeader.IsWebSocket) DomainServer.WebSocketRequest(this);
                else DomainServer.Request(this);
                return;
            }
            catch (Exception error)
            {
                Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error);
            }
            ResponseError(identity, ResponseState.ServerError500);
        }
        /// <summary>
        /// 输出 HTTP 响应数据
        /// </summary>
        /// <param name="identity">HTTP 操作标识</param>
        /// <param name="response">HTTP 响应数据</param>
        /// <returns>是否匹配会话标识</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Response(long identity, Response response)
        {
            return Response(identity, ref response);
        }
        /// <summary>
        /// 输出 HTTP 响应数据
        /// </summary>
        /// <param name="response">HTTP 响应数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ResponseIdentity(Response response)
        {
            ResponseIdentity(ref response);
        }
        /// <summary>
        /// 304 检测
        /// </summary>
        /// <param name="response"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CheckNotChanged304(ref Response response)
        {
            if ((HttpHeader.Flag & HeaderFlag.IsSetIfModifiedSince) != 0 && (response.Flag & ResponseFlag.LastModified) != 0 && HttpHeader.IfModifiedSinceIndex.Length == response.LastModifiedData.Length)
            {
                fixed (byte* bufferFixed = HttpHeader.Buffer.Buffer, responseFixed = response.LastModifiedData)
                {
                    if (Memory.EqualNotNull(responseFixed, bufferFixed + (HttpHeader.Buffer.StartIndex + HttpHeader.IfModifiedSinceIndex.StartIndex), HttpHeader.IfModifiedSinceIndex.Length))
                    {
                        response.Push();
                        response = Http.Response.NotChanged304;
                    }
                }
            }
        }
        /// <summary>
        /// 获取 HTTP 响应头部缓冲区长度
        /// </summary>
        /// <param name="response"></param>
        /// <param name="index"></param>
        /// <param name="cookie">Cookie</param>
        /// <returns></returns>
        internal int GetResponseHeaderIndex(Response response, int index, ref Cookie cookie)
        {
            ResponseFlag responseFlag = response.Flag;
            if (isResponseServer) index += AutoCSerServer.Length;
            if ((responseFlag & ResponseFlag.Location) != 0) index += locationSize + response.Location.Length + 2;
            if ((responseFlag & ResponseFlag.LastModified) != 0) index += lastModifiedSize + response.LastModifiedData.Length + 2;
            if ((responseFlag & ResponseFlag.CacheControl) != 0) index += cacheControlSize + response.CacheControlData.Length + 2;
            if ((responseFlag & ResponseFlag.ContentType) != 0) index += contentTypeSize + response.ContentTypeData.Length + 2;
            if ((responseFlag & ResponseFlag.ContentEncoding) != 0) index += contentEncodingSize + response.ContentEncoding.Length + 2;
            if ((responseFlag & ResponseFlag.ETag) != 0) index += eTagSize + response.ETagData.Length + 2 + 1;
            if ((responseFlag & ResponseFlag.ContentDisposition) != 0) index += contentDispositionSize + response.ContentDispositionData.Length + 2;
            if ((responseFlag & ResponseFlag.AccessControlAllowOrigin) != 0) index += accessControlAllowOriginSize + response.AccessControlAllowOrigin.Length + 2;
            if (HttpHeader.IsKeepAlive != 0) index += keepAliveSize;
            if (isResponseDate) index += dateSize + Date.ToByteLength + 2;
            if ((responseFlag & ResponseFlag.Cookie) != 0)
            {
                Cookie nextCookie = cookie = response.GetCookieClear();
                while ((nextCookie = nextCookie.GetSize(ref index)) != null) ;
            }
            //int checkIndex = index;
            if (HttpHeader.Method == MethodType.HEAD)
            {
                ResponseSize = 0;
                index += 5;
            }
            else if (ResponseSize > 1500 - 20 || (int)ResponseSize <= 5) index += 5;
            else if ((index += (int)ResponseSize) > Http.Header.NameStartIndex) index -= (int)ResponseSize - 5;
            return index;
        }
        /// <summary>
        /// 创建 HTTP 响应头部
        /// </summary>
        /// <param name="response"></param>
        /// <param name="cookie"></param>
        /// <param name="write"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal byte* CreateResponseHeader(Response response, Cookie cookie, byte* write, int index)
        {
            ResponseFlag responseFlag = response.Flag;
            if ((responseFlag & ResponseFlag.Location) != 0)
            {
                writeLocation(write);
                fixed (byte* locationFixed = response.Location.Array) Memory.SimpleCopyNotNull64(locationFixed + response.Location.Start, write += locationSize, index = response.Location.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if ((responseFlag & ResponseFlag.CacheControl) != 0)
            {
                writeCacheControl(write);
                Memory.SimpleCopyNotNull64(response.CacheControlData, write += cacheControlSize, index = response.CacheControlData.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if ((responseFlag & ResponseFlag.ContentType) != 0)
            {
                writeContentType(write);
                Memory.SimpleCopyNotNull64(response.ContentTypeData, write += contentTypeSize, index = response.ContentTypeData.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if ((responseFlag & ResponseFlag.ContentEncoding) != 0)
            {
                writeContentEncoding(write);
                Memory.SimpleCopyNotNull64(response.ContentEncoding, write += contentEncodingSize, index = response.ContentEncoding.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if ((responseFlag & ResponseFlag.ETag) != 0)
            {
                writeETag(write);
                Memory.SimpleCopyNotNull64(response.ETagData, write += eTagSize, index = response.ETagData.Length);
                *(int*)(write += index) = '"' + 0x0a0d00;
                write += 3;
            }
            if ((responseFlag & ResponseFlag.ContentDisposition) != 0)
            {
                writeContentDisposition(write);
                Memory.SimpleCopyNotNull64(response.ContentDispositionData, write += contentDispositionSize, index = response.ContentDispositionData.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if ((responseFlag & ResponseFlag.AccessControlAllowOrigin) != 0)
            {
                writeAccessControlAllowOrigin(write);
                fixed (byte* requestBufferFixed = HttpHeader.Buffer.Buffer) Memory.SimpleCopyNotNull64(requestBufferFixed + HttpHeader.Buffer.StartIndex + response.AccessControlAllowOrigin.StartIndex, write += accessControlAllowOriginSize, index = response.AccessControlAllowOrigin.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if (cookie != null)
            {
                Cookie writeCookie = cookie;
                int cookieCount = 0;
                do
                {
                    Cookie nextCookie = writeCookie.Write(ref write);
                    ++cookieCount;
                    if (nextCookie == null)
                    {
                        if (cookieCount == 1) Cookie.YieldPool.Default.PushNotNull(cookie);
                        else Cookie.YieldPool.Default.PushLink(cookie, writeCookie, cookieCount);
                        break;
                    }
                    writeCookie = nextCookie;
                }
                while (true);
            }
            if ((responseFlag & ResponseFlag.LastModified) != 0)
            {
                writeLastModified(write);
                Memory.SimpleCopyNotNull64(response.LastModifiedData, write += lastModifiedSize, index = response.LastModifiedData.Length);
                *(short*)(write += index) = 0x0a0d;
                write += sizeof(short);
            }
            if (HttpHeader.IsKeepAlive != 0)
            {
                writeKeepAlive(write);
                write += keepAliveSize;
            }
            if (isResponseServer)
            {
                Memory.SimpleCopyNotNull64(responseServer.Byte, write, AutoCSerServer.Length);
                write += AutoCSerServer.Length;
            }
            if (isResponseDate)
            {
                writeDate(write);
                getDate(write += dateSize);
                *(int*)(write += Date.ToByteLength) = 0x0a0d0a0d;
                write += sizeof(int);
            }
            else
            {
                *(short*)(write) = 0x0a0d;
                write += sizeof(short);
            }
            return write;
        }
        /// <summary>
        /// FORM表单解析
        /// </summary>
        /// <param name="dataToType"></param>
        internal void ParseForm(PostType dataToType = PostType.Data)
        {
            switch (HttpHeader.PostType)
            {
                case PostType.Json: Form.SetText(GetFormText(), Http.Header.QueryJsonNameChar); return;
                case PostType.Xml: Form.SetText(GetFormText(), Http.Header.QueryXmlNameChar); return;
                case PostType.Data:
                    if (FormBufferReceiveEndIndex == 0) return;
                    switch (dataToType)
                    {
                        case PostType.Json: Form.SetText(GetFormText(), Http.Header.QueryJsonNameChar); return;
                        case PostType.Xml: Form.SetText(GetFormText(), Http.Header.QueryXmlNameChar); return;
                        default: return;
                    }
                default: Form.Parse(); return;
            }
        }
        /// <summary>
        /// 获取表单字符串
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string GetFormText()
        {
            return HttpHeader.RequestEncoding.GetString(FormBuffer.Buffer, FormBuffer.StartIndex, FormBufferReceiveEndIndex);
        }
        /// <summary>
        /// 获取表单字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string GetFormText(byte[] buffer)
        {
            return buffer == null ? string.Empty : HttpHeader.RequestEncoding.GetString(buffer, 0, buffer.Length);
        }
        ///// <summary>
        ///// 获取请求表单数据
        ///// </summary>
        ///// <param name="identity">HTTP操作标识</param>
        ///// <param name="page">WEB 页面</param>
        ///// <param name="type">获取请求表单数据回调类型</param>
        ///// <returns>是否匹配会话标识</returns>
        //internal abstract bool GetForm(long identity, AutoCSer.WebView.Page page, GetFormType type);
        /// <summary>
        /// 获取请求表单数据
        /// </summary>
        /// <param name="page">WEB 页面</param>
        /// <param name="type">获取请求表单数据回调类型</param>
        internal abstract void GetForm(AutoCSer.WebView.Page page, GetFormType type);
        /// <summary>
        /// 获取请求表单数据回调处理
        /// </summary>
        /// <returns></returns>
        internal bool OnGetForm()
        {
            AutoCSer.WebView.Page page = GetFormPage;
            Flag &= SocketFlag.All ^ SocketFlag.GetForm;
            GetFormPage = null;
            ++page.SocketIdentity;
            switch (GetFormType)
            {
                case Http.GetFormType.Call:
                    if (page.DomainServer.Call(new AutoCSer.WebView.UnionType { Value = page }.CallSynchronize)) return true;
                    break;
                case Http.GetFormType.CallAsynchronous:
                    if (page.DomainServer.Call(new AutoCSer.WebView.UnionType { Value = page }.CallAsynchronousBase)) return true;
                    break;
                case Http.GetFormType.Ajax:
                    if (new AutoCSer.WebView.UnionType { Value = page }.AjaxBase.CallAjax()) return true;
                    break;
                case Http.GetFormType.PubAjax:
                    if (new AutoCSer.WebView.UnionType { Value = page }.PubAjax.Call()) return true;
                    break;
            }
            page.CancelGetForm();
            return false;
        }
        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetTimeout(int size)
        {
            Timeout = Config.GetTimeout(size);
        }
        /// <summary>
        /// 是否缓冲区
        /// </summary>
        internal void Free()
        {
            if (SendFileStream != null)
            {
                SendFileStream.Dispose();
                SendFileStream = null;
            }
            if ((Flag & SocketFlag.IsLoadForm) != 0)
            {
                Form.Clear();
                FormBuffer.Clear();
            }
            if ((Flag & SocketFlag.GetForm) != 0)
            {
                GetFormPage.CancelGetForm();
                GetFormPage = null;
            }
            if ((Flag & SocketFlag.BigBuffer) != 0) BigBuffer.Free();
            Http.Response.Push(ref HttpResponse);
            Flag = SocketFlag.None;
        }
        /// <summary>
        /// 获取数据缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SubBuffer.PoolBufferFull GetBuffer(int size)
        {
            if (size <= Buffer.Length) return Buffer;
            if (size > BigBuffer.NullLength)
            {
                if ((Flag & SocketFlag.BigBuffer) != 0) BigBuffer.Free();
                SubBuffer.Pool.GetBuffer(ref BigBuffer, size);
            }
            Flag |= SocketFlag.BigBuffer;
            return BigBuffer;
        }

        /// <summary>
        /// HTTP 配置
        /// </summary>
        internal static readonly Config Config = ConfigLoader.GetUnion(typeof(Config)).Config ?? new Config();
        /// <summary>
        /// HTTP 头部缓冲区池
        /// </summary>
        internal static readonly SubBuffer.Pool BufferPool;
        /// <summary>
        /// 服务器类型(长度不能为0)
        /// </summary>
        protected const string AutoCSerServer = @"Server: AutoCSer.HTTP/1.1
";
        /// <summary>
        /// HTTP服务版本号
        /// </summary>
        protected const string httpVersionString = "HTTP/1.1";
        /// <summary>
        /// 服务器类型
        /// </summary>
        protected static Pointer responseServer;
        /// <summary>
        /// 最后一次生成的时间字节数组
        /// </summary>
        protected static Pointer dateCache;
        /// <summary>
        /// 最后一次生成的时间
        /// </summary>
        protected static long dateCacheSecond;
        /// <summary>
        /// 时间字节数组访问锁
        /// </summary>
        protected static readonly object dateCacheLock = new object();
        /// <summary>
        /// 获取当前时间字节数组
        /// </summary>
        /// <param name="data">输出数据起始位置</param>
        protected static unsafe void getDate(byte* data)
        {
            DateTime now = Date.NowTime.Now;
            if (Monitor.TryEnter(dateCacheLock))
            {
                try
                {
                    byte* cachePoint = dateCache.Byte;
                    long second = now.Ticks / 10000000;
                    if (dateCacheSecond != second)
                    {
                        dateCacheSecond = second;
                        Date.ToBytes(now, cachePoint);
                    }
                    *(ulong*)data = *(ulong*)cachePoint;
                    *(ulong*)(data + sizeof(ulong)) = *(ulong*)(cachePoint + sizeof(ulong));
                    *(ulong*)(data + sizeof(ulong) * 2) = *(ulong*)(cachePoint + sizeof(ulong) * 2);
                    *(ulong*)(data + sizeof(ulong) * 3) = *(ulong*)(cachePoint + sizeof(ulong) * 3);
                }
                finally { Monitor.Exit(dateCacheLock); }
            }
            else Date.ToBytes(now, data);
        }
        /// <summary>
        /// 是否输出服务器信息
        /// </summary>
        protected static readonly bool isResponseServer = Config.IsResponseServer;
        /// <summary>
        /// HTTP服务版本号数据长度
        /// </summary>
        protected const int httpVersionSize = sizeof(int) * 2;
        /// <summary>
        /// HTTP服务版本号
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeHttpVersion(byte* buffer)
        {//HTTP/1.1
            *(int*)buffer = 'H' + ('T' << 8) + ('T' << 16) + ('P' << 24);
            *(int*)(buffer + sizeof(int)) = '/' + ('1' << 8) + ('.' << 16) + ('1' << 24);
        }
        /// <summary>
        /// HTTP响应输出内容长度名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeContentLength(byte* buffer)
        {//Content-Length
            *(int*)buffer = 'C' + ('o' << 8) + ('n' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int)) = 'e' + ('n' << 8) + ('t' << 16) + ('-' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'L' + ('e' << 8) + ('n' << 16) + ('g' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 't' + ('h' << 8) + (':' << 16) + (' ' << 24);
        }
        /// <summary>
        /// HTTP响应输出内容长度名称数据长度
        /// </summary>
        protected const int contentLengthSize = sizeof(int) * 4;
        /// <summary>
        /// 请求范围名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeRange(byte* buffer)
        {//Accept-Ranges: bytes
            //Content-Range: bytes 
            *(int*)buffer = 'A' + ('c' << 8) + ('c' << 16) + ('e' << 24);
            *(int*)(buffer + sizeof(int)) = 'p' + ('t' << 8) + ('-' << 16) + ('R' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'a' + ('n' << 8) + ('g' << 16) + ('e' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 's' + (':' << 8) + (' ' << 16) + ('b' << 24);
            *(int*)(buffer + sizeof(int) * 4) = 'y' + ('t' << 8) + ('e' << 16) + ('s' << 24);
            *(int*)(buffer + sizeof(int) * 5) = 0x0a0d + ('C' << 16) + ('o' << 24);
            *(int*)(buffer + sizeof(int) * 6) = 'n' + ('t' << 8) + ('e' << 16) + ('n' << 24);
            *(int*)(buffer + sizeof(int) * 7) = 't' + ('-' << 8) + ('R' << 16) + ('a' << 24);
            *(int*)(buffer + sizeof(int) * 8) = 'n' + ('g' << 8) + ('e' << 16) + (':' << 24);
            *(int*)(buffer + sizeof(int) * 9) = ' ' + ('b' << 8) + ('y' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int) * 10) = 'e' + ('s' << 8) + (' ' << 16);
        }
        /// <summary>
        /// 请求范围名称数据长度
        /// </summary>
        protected const int rangeSize = sizeof(int) * 10 + 3;
        /// <summary>
        /// 重定向名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeLocation(byte* buffer)
        {//Location: 
            *(int*)buffer = 'L' + ('o' << 8) + ('c' << 16) + ('a' << 24);
            *(int*)(buffer + sizeof(int)) = 't' + ('i' << 8) + ('o' << 16) + ('n' << 24);
            *(short*)(buffer + sizeof(int) * 2) = ':' + (' ' << 8);
        }
        /// <summary>
        /// 重定向名称数据长度
        /// </summary>
        protected const int locationSize = sizeof(int) * 2 + sizeof(short);
        /// <summary>
        /// HTTP响应输出最后修改名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeLastModified(byte* buffer)
        {//Last-Modified: 
            *(int*)buffer = 'L' + ('a' << 8) + ('s' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int)) = '-' + ('M' << 8) + ('o' << 16) + ('d' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'i' + ('f' << 8) + ('i' << 16) + ('e' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 'd' + (':' << 8) + (' ' << 16);
        }
        /// <summary>
        /// HTTP响应输出最后修改名称数据长度
        /// </summary>
        protected const int lastModifiedSize = sizeof(int) * 3 + 3;
        /// <summary>
        /// 缓存参数名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeCacheControl(byte* buffer)
        {//Cache-Control
            *(int*)buffer = 'C' + ('a' << 8) + ('c' << 16) + ('h' << 24);
            *(int*)(buffer + sizeof(int)) = 'e' + ('-' << 8) + ('C' << 16) + ('o' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'n' + ('t' << 8) + ('r' << 16) + ('o' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 'l' + (':' << 8) + (' ' << 16);
        }
        /// <summary>
        /// 缓存参数名称数据长度
        /// </summary>
        protected const int cacheControlSize = sizeof(int) * 3 + 3;
        /// <summary>
        /// 内容类型名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeContentType(byte* buffer)
        {//Content-Type
            *(int*)buffer = 'C' + ('o' << 8) + ('n' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int)) = 'e' + ('n' << 8) + ('t' << 16) + ('-' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'T' + ('y' << 8) + ('p' << 16) + ('e' << 24);
            *(short*)(buffer + sizeof(int) * 3) = ':' + (' ' << 8);
        }
        /// <summary>
        /// 内容类型名称数据长度
        /// </summary>
        protected const int contentTypeSize = sizeof(int) * 3 + sizeof(short);
        /// <summary>
        /// 内容压缩编码名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeContentEncoding(byte* buffer)
        {//Content-Encoding
            *(int*)buffer = 'C' + ('o' << 8) + ('n' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int)) = 'e' + ('n' << 8) + ('t' << 16) + ('-' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'E' + ('n' << 8) + ('c' << 16) + ('o' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 'd' + ('i' << 8) + ('n' << 16) + ('g' << 24);
            *(short*)(buffer + sizeof(int) * 4) = ':' + (' ' << 8);
        }
        /// <summary>
        /// 内容压缩编码名称数据长度
        /// </summary>
        protected const int contentEncodingSize = sizeof(int) * 4 + sizeof(short);
        /// <summary>
        /// 缓存匹配标识名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeETag(byte* buffer)
        {//ETag
            *(int*)buffer = 'E' + ('T' << 8) + ('a' << 16) + ('g' << 24);
            *(int*)(buffer + sizeof(int)) = ':' + (' ' << 8) + ('"' << 16);
        }
        /// <summary>
        /// 缓存匹配标识名称数据长度
        /// </summary>
        protected const int eTagSize = sizeof(int) + 3;
        /// <summary>
        /// 内容描述名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeContentDisposition(byte* buffer)
        {//Content-Disposition
            *(int*)buffer = 'C' + ('o' << 8) + ('n' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int)) = 'e' + ('n' << 8) + ('t' << 16) + ('-' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'D' + ('i' << 8) + ('s' << 16) + ('p' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 'o' + ('s' << 8) + ('i' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int) * 4) = 'i' + ('o' << 8) + ('n' << 16) + (':' << 24);
            *(buffer + sizeof(int) * 5) = (byte)' ';
        }
        /// <summary>
        /// 内容描述名称数据长度
        /// </summary>
        protected const int contentDispositionSize = sizeof(int) * 5 + 1;
        /// <summary>
        /// 跨域权限控制
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeAccessControlAllowOrigin(byte* buffer)
        {//Access-Control-Allow-Origin
            *(int*)buffer = 'A' + ('c' << 8) + ('c' << 16) + ('e' << 24);
            *(int*)(buffer + sizeof(int)) = 's' + ('s' << 8) + ('-' << 16) + ('C' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'o' + ('n' << 8) + ('t' << 16) + ('r' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 'o' + ('l' << 8) + ('-' << 16) + ('A' << 24);
            *(int*)(buffer + sizeof(int) * 4) = 'l' + ('l' << 8) + ('o' << 16) + ('w' << 24);
            *(int*)(buffer + sizeof(int) * 5) = '-' + ('O' << 8) + ('r' << 16) + ('i' << 24);
            *(int*)(buffer + sizeof(int) * 6) = 'g' + ('i' << 8) + ('n' << 16) + (':' << 24);
            *(buffer + sizeof(int) * 7) = (byte)' ';
        }
        /// <summary>
        /// 跨域权限控制数据长度
        /// </summary>
        protected const int accessControlAllowOriginSize = sizeof(int) * 7 + 1;
        /// <summary>
        /// HTTP响应输出保持连接
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeKeepAlive(byte* buffer)
        {//Connection: Keep-Alive
            *(int*)buffer = 'C' + ('o' << 8) + ('n' << 16) + ('n' << 24);
            *(int*)(buffer + sizeof(int)) = 'e' + ('c' << 8) + ('t' << 16) + ('i' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'o' + ('n' << 8) + (':' << 16) + (' ' << 24);
            *(int*)(buffer + sizeof(int) * 3) = 'K' + ('e' << 8) + ('e' << 16) + ('p' << 24);
            *(int*)(buffer + sizeof(int) * 4) = '-' + ('A' << 8) + ('l' << 16) + ('i' << 24);
            *(int*)(buffer + sizeof(int) * 5) = 'v' + ('e' << 8) + 0x0a0d0000;
        }
        /// <summary>
        /// HTTP响应输出保持连接数据长度
        /// </summary>
        protected const int keepAliveSize = sizeof(int) * 6;
        /// <summary>
        /// HTTP响应输出日期名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected unsafe static void writeDate(byte* buffer)
        {//Date: 
            *(int*)buffer = 'D' + ('a' << 8) + ('t' << 16) + ('e' << 24);
            *(short*)(buffer + sizeof(int)) = ':' + (' ' << 8);
        }
        /// <summary>
        /// HTTP响应输出日期名称数据长度
        /// </summary>
        protected const int dateSize = sizeof(int) + sizeof(short);
        /// <summary>
        /// 是否输出日期
        /// </summary>
        protected static readonly bool isResponseDate = Config.IsResponseDate;
        /// <summary>
        /// 100 Continue 确认
        /// </summary>
        protected static readonly byte[] continue100 = (httpVersionString + EnumAttribute<ResponseState, ResponseStateAttribute>.Array((byte)ResponseState.Continue100).Text + @"
").getBytes();
        /// <summary>
        /// 错误输出缓存数据
        /// </summary>
        protected static readonly byte[][] errorResponseDatas;
        /// <summary>
        /// 搜索引擎 404 提示
        /// </summary>
        protected static readonly byte[] searchEngine404Data;

        static SocketBase()
        {
            BufferPool = SubBuffer.Pool.GetPool((int)Config.BufferSize >= (int)Config.HeadSize ? Config.BufferSize : Config.HeadSize);

            int dateCacheSize = (Date.ToByteLength + 7) & (int.MaxValue - 7), AutoCSerServerSize = (AutoCSerServer.Length + 7) & (int.MaxValue - 7);
            dateCache = new Pointer { Data = Unmanaged.GetStatic64(dateCacheSize + AutoCSerServerSize, false) };
            responseServer = new Pointer { Data = dateCache.Byte + dateCacheSize };
            fixed (char* AutoCSerServerFixed = AutoCSerServer) StringExtension.WriteBytesNotNull(AutoCSerServerFixed, AutoCSerServer.Length, responseServer.Byte);

            byte[] responseServerEnd = (AutoCSerServer + @"Content-Length: 0

").getBytes();
            errorResponseDatas = new byte[EnumAttribute<ResponseState>.GetMaxValue(-1) + 1][];
            foreach (ResponseState type in System.Enum.GetValues(typeof(ResponseState)))
            {
                ResponseStateAttribute state = EnumAttribute<ResponseState, ResponseStateAttribute>.Array((int)type);
                if (state != null && state.IsError)
                {
                    string stateData = state.Text;
                    byte[] responseData = new byte[httpVersionSize + stateData.Length + responseServerEnd.Length];
                    fixed (byte* responseDataFixed = responseData)
                    {
                        writeHttpVersion(responseDataFixed);
                        state.Write(responseDataFixed + httpVersionSize);
                    }
                    int index = httpVersionSize + stateData.Length;
                    System.Buffer.BlockCopy(responseServerEnd, 0, responseData, index, responseServerEnd.Length);
                    errorResponseDatas[(int)type] = responseData;
                    if (type == ResponseState.NotFound404)
                    {
                        byte[] html = Encoding.UTF8.GetBytes("<html><head>" + AutoCSer.WebView.CharsetTypeAttribute.GetHtml(AutoCSer.WebView.CharsetType.Utf8) + "<title>404 Error, 请将链接中的 #! 或者 # 修改为 ?</title></head><body>404 Error, 请将链接中的 #! 或者 # 修改为 ?</body></html>");
                        byte[] contentLength = EncodingCache.Ascii.GetBytesNotEmpty(AutoCSerServer + "Content-Length: " + html.Length.toString() + @"

");
                        searchEngine404Data = new byte[httpVersionSize + stateData.Length + contentLength.Length + html.Length];
                        fixed (byte* responseDataFixed = searchEngine404Data)
                        {
                            writeHttpVersion(responseDataFixed);
                            state.Write(responseDataFixed + httpVersionSize);
                        }
                        System.Buffer.BlockCopy(contentLength, 0, searchEngine404Data, index = httpVersionSize + stateData.Length, contentLength.Length);
                        System.Buffer.BlockCopy(html, 0, searchEngine404Data, index += contentLength.Length, html.Length);
                    }
                }
            }
        }
    }
    /// <summary>
    /// HTTP 套接字
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal abstract class SocketBase<valueType> : SocketBase, IDisposable
        where valueType : SocketBase<valueType>
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal valueType NextTask;
        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// 链表
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct YieldLink
        {
            /// <summary>
            /// 缓存数量
            /// </summary>
            private readonly static int maxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(valueType));
            /// <summary>
            /// 链表头部
            /// </summary>
            private valueType head;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private int popLock;
            /// <summary>
            /// 缓存数量
            /// </summary>
            private int count;
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int PushNotNull(valueType value)
            {
                if (count >= maxCount)
                {
                    value.Dispose();
                    return 0;
                }
                System.Threading.Interlocked.Increment(ref count);
                valueType headValue;
                do
                {
                    if ((headValue = head) == null)
                    {
                        value.NextTask = null;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return 1;
                    }
                    else
                    {
                        value.NextTask = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return 1;
                    }
                    AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
                }
                while (true);
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public valueType Pop()
            {
                while (System.Threading.Interlocked.CompareExchange(ref popLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
                valueType headValue;
                do
                {
                    if ((headValue = head) == null)
                    {
                        System.Threading.Interlocked.Exchange(ref popLock, 0);
                        return null;
                    }
                    if (System.Threading.Interlocked.CompareExchange(ref head, headValue.NextTask, headValue) == headValue)
                    {
                        System.Threading.Interlocked.Exchange(ref popLock, 0);
                        System.Threading.Interlocked.Decrement(ref count);
                        headValue.NextTask = null;
                        return headValue;
                    }
                    AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
                }
                while (true);
            }
            /// <summary>
            /// 释放列表
            /// </summary>
            /// <param name="value"></param>
            private void disposeLink(valueType value)
            {
                do
                {
                    value.Dispose();
                }
                while ((value = value.NextTask) != null);
            }
            /// <summary>
            /// 清除缓存数据
            /// </summary>
            /// <param name="count">保留缓存数据数量</param>
            internal void ClearCache(int count)
            {
                valueType head = Interlocked.Exchange(ref this.head, null);
                this.count = 0;
                if (head != null)
                {
                    if (count == 0) disposeLink(head);
                    else
                    {
                        int pushCount = count;
                        valueType end = head;
                        while (--count != 0)
                        {
                            if (end.NextTask == null)
                            {
                                PushLink(head, end, pushCount - count);
                                return;
                            }
                            end = end.NextTask;
                        }
                        valueType next = end.NextTask;
                        end.NextTask = null;
                        PushLink(head, end, pushCount);
                        if (next != null) disposeLink(next);
                    }
                }
            }
            /// <summary>
            /// 添加链表
            /// </summary>
            /// <param name="value">链表头部</param>
            /// <param name="end">链表尾部</param>
            /// <param name="count">数据数量</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void PushLink(valueType value, valueType end, int count)
            {
                System.Threading.Interlocked.Add(ref this.count, count);
                valueType headValue;
                do
                {
                    if ((headValue = head) == null)
                    {
                        end.NextTask = null;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                    }
                    else
                    {
                        end.NextTask = headValue;
                        if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
                    }
                    AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
                }
                while (true);
            }
        }
        /// <summary>
        /// 链表节点池
        /// </summary>
        internal static YieldLink Pool;
    }
}
