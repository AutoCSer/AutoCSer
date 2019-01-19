using System;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB页面
    /// </summary>
    public abstract class Page : Request, IDisposable
    {
        /// <summary>
        /// 未知数据流解析转换类型
        /// </summary>
        protected internal virtual AutoCSer.Net.Http.PostType dataToType { get { return AutoCSer.Net.Http.PostType.Json; } }
        /// <summary>
        /// HTTP 响应输出
        /// </summary>
        internal AutoCSer.Net.Http.Response PageResponse;
        /// <summary>
        /// HTTP 响应输出
        /// </summary>
        public AutoCSer.Net.Http.Response HttpResponse
        {
            get
            {
                if (PageResponse == null) PageResponse = AutoCSer.Net.Http.Response.Get();
                return PageResponse;
            }
        }
        /// <summary>
        /// 设置 JS 内容类型
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetJsContentType()
        {
            HttpResponse.SetJsContentType(DomainServer);
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="domain">有效域名</param>
        /// <param name="path">有效路径</param>
        /// <param name="expires">超时时间</param>
        /// <param name="isSecure">是否安全</param>
        /// <param name="isHttpOnly">是否 HTTP Only</param>
        public void SetCookie(byte[] name, byte[] value, DateTime expires, byte[] domain = null, byte[] path = null, bool isSecure = false, bool isHttpOnly = false)
        {
            if (name != null && name.Length != 0)
            {
                AutoCSer.Net.Http.Cookie cookie = HttpResponse.GetCookie(name);
                if (cookie == null)
                {
                    HttpResponse.AppendCookie(cookie = AutoCSer.Net.Http.Cookie.YieldPool.Default.Pop() ?? new AutoCSer.Net.Http.Cookie());
                    cookie.Name = name;
                }
                if (domain == null) cookie.Set(value ?? NullValue<byte>.Array, expires, Socket.HttpHeader.Host, path, isSecure, isHttpOnly);
                else cookie.Set(value ?? NullValue<byte>.Array, expires, domain, path, isSecure, isHttpOnly);
            }
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void SetCookie(byte[] name, byte[] value)
        {
            if (name != null && name.Length != 0)
            {
                AutoCSer.Net.Http.Cookie cookie = HttpResponse.GetCookie(name);
                if (cookie == null)
                {
                    HttpResponse.AppendCookie(cookie = AutoCSer.Net.Http.Cookie.YieldPool.Default.Pop() ?? new AutoCSer.Net.Http.Cookie());
                    cookie.Name = name;
                }
                cookie.Set(value ?? NullValue<byte>.Array, DateTime.MinValue, Socket.HttpHeader.Host, AutoCSer.Net.Http.Cookie.DefaultPath, false, false);
            }
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void SetCookie(string name, string value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                byte[] nameData = name.getBytes();
                AutoCSer.Net.Http.Cookie cookie = HttpResponse.GetCookie(nameData);
                if (cookie == null)
                {
                    HttpResponse.AppendCookie(cookie = AutoCSer.Net.Http.Cookie.YieldPool.Default.Pop() ?? new AutoCSer.Net.Http.Cookie());
                    cookie.Name = nameData;
                }
                cookie.Set(string.IsNullOrEmpty(value) ? NullValue<byte>.Array : value.getBytes(), DateTime.MinValue, Socket.HttpHeader.Host, AutoCSer.Net.Http.Cookie.DefaultPath, false, false);
            }
        }
        /// <summary>
        /// 删除 Cookie
        /// </summary>
        /// <param name="name">名称</param>
        public void RemoveCookie(byte[] name)
        {
            if (name != null && name.Length != 0)
            {
                AutoCSer.Net.Http.Cookie cookie = HttpResponse.GetCookie(name);
                if (cookie == null)
                {
                    HttpResponse.AppendCookie(cookie = AutoCSer.Net.Http.Cookie.YieldPool.Default.Pop() ?? new AutoCSer.Net.Http.Cookie());
                    cookie.Name = name;
                }
                cookie.Set(NullValue<byte>.Array, Pub.MinTime, Socket.HttpHeader.Host, AutoCSer.Net.Http.Cookie.DefaultPath, false, false);
            }
        }
        /// <summary>
        /// 删除 Cookie
        /// </summary>
        /// <param name="name">名称</param>
        public void RemoveCookie(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                byte[] nameData = name.getBytes();
                AutoCSer.Net.Http.Cookie cookie = HttpResponse.GetCookie(nameData);
                if (cookie == null)
                {
                    HttpResponse.AppendCookie(cookie = AutoCSer.Net.Http.Cookie.YieldPool.Default.Pop() ?? new AutoCSer.Net.Http.Cookie());
                    cookie.Name = nameData;
                }
                cookie.Set(NullValue<byte>.Array, Pub.MinTime, Socket.HttpHeader.Host, AutoCSer.Net.Http.Cookie.DefaultPath, false, false);
            }
        }
        /// <summary>
        /// Session 名称
        /// </summary>
        private static readonly byte[] sessionName = AutoCSer.Net.Http.SocketBase.Config.SessionName.getBytes();
        /// <summary>
        /// 会话标识
        /// </summary>
        private AutoCSer.Net.HttpDomainServer.SessionId sessionId;
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <returns>Session值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType GetSession<valueType>() where valueType : class
        {
#if NOJIT
            AutoCSer.Net.HttpDomainServer.ISession session = Session;
#else
            AutoCSer.Net.HttpDomainServer.ISession<valueType> session = Session as AutoCSer.Net.HttpDomainServer.ISession<valueType>;
#endif
            if (session != null)
            {
                setSessionId();
#if NOJIT
                if (sessionId.Ticks != 0) return (valueType)session.Get(ref sessionId, null);
#else
                if (sessionId.Ticks != 0) return session.Get(ref sessionId, null);
#endif
            }
            return null;
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="nullValue">默认空值</param>
        /// <returns>Session值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType GetSession<valueType>(valueType nullValue) where valueType : struct
        {
#if NOJIT
            AutoCSer.Net.HttpDomainServer.ISession session = Session;
#else
            AutoCSer.Net.HttpDomainServer.ISession<valueType> session = Session as AutoCSer.Net.HttpDomainServer.ISession<valueType>;
#endif
            if (session != null)
            {
                setSessionId();
#if NOJIT
                if (sessionId.Ticks != 0) return (valueType)session.Get(ref sessionId, nullValue);
#else
                if (sessionId.Ticks != 0) return session.Get(ref sessionId, nullValue);
#endif
            }
            return nullValue;
        }
        /// <summary>
        /// 初始化请求会话标识
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setSessionId()
        {
            if (sessionId.CookieValue == 0)
            {
                SubArray<byte> cookie = default(SubArray<byte>);
                Socket.HttpHeader.GetCookie(sessionName, ref cookie);
                sessionId.FromCookie(ref cookie);
            }
        }
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="value">值</param>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <returns>是否设置成功</returns>
        public bool SetSession<valueType>(valueType value)
        {
#if NOJIT
            AutoCSer.Net.HttpDomainServer.ISession session = Session;
#else
            AutoCSer.Net.HttpDomainServer.ISession<valueType> session = Session as AutoCSer.Net.HttpDomainServer.ISession<valueType>;
#endif
            if (session != null)
            {
                setSessionId();
                if (session.Set(ref sessionId, value)) setSessionCookie();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置 Session 值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public bool SetSession(object value)
        {
            AutoCSer.Net.HttpDomainServer.ISession session = Session;
            if (session != null)
            {
                setSessionId();
                if (session.Set(ref sessionId, value)) setSessionCookie();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取 Session Cookie
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private AutoCSer.Net.Http.Cookie getSessionCookie()
        {
            AutoCSer.Net.Http.Cookie cookie = HttpResponse.GetCookie(sessionName);
            if (cookie == null)
            {
                HttpResponse.AppendCookie(cookie = AutoCSer.Net.Http.Cookie.YieldPool.Default.Pop() ?? new AutoCSer.Net.Http.Cookie());
                cookie.Name = sessionName;
            }
            return cookie;
        }
        /// <summary>
        /// 设置 Session Cookie
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setSessionCookie()
        {
            getSessionCookie().Set(sessionId.ToCookie(), DateTime.MinValue, Socket.HttpHeader.Host, AutoCSer.Net.Http.Cookie.DefaultPath, false, true);
        }
        /// <summary>
        /// 删除 Session
        /// </summary>
        public void RemoveSession()
        {
            AutoCSer.Net.HttpDomainServer.ISession session = Session;
            if (session != null)
            {
                setSessionId();
                if (sessionId.Ticks != 0)
                {
                    session.Remove(ref sessionId);
                    getSessionCookie().Set(NullValue<byte>.Array, Pub.MinTime, Socket.HttpHeader.Host, AutoCSer.Net.Http.Cookie.DefaultPath, false, true);
                }
            }
        }
        /// <summary>
        /// 客户端缓存版本号
        /// </summary>
        public virtual int ETagVersion
        {
            get { return 0; }
        }
        ///// <summary>
        ///// 是否异步
        ///// </summary>
        //protected bool isAsynchronous;
        /// <summary>
        /// 是否支持压缩
        /// </summary>
        protected virtual bool isGZip { get { return true; } }
        /// <summary>
        /// 内存流最大字节数
        /// </summary>
        internal virtual SubBuffer.Size MaxMemoryStreamSize { get { return SubBuffer.Size.Kilobyte4; } }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            AutoCSer.Net.Http.SocketBase socket = Socket;
            Socket = null;
            if (socket != null) socket.ResponseError(SocketIdentity, AutoCSer.Net.Http.ResponseState.ServerError500);
        }
        ///// <summary>
        ///// HTTP请求头部处理
        ///// </summary>
        ///// <param name="domainServer">域名服务</param>
        ///// <param name="socket">HTTP套接字接口</param>
        ///// <param name="socketIdentity">套接字操作编号</param>
        ///// <param name="isAsynchronous">是否异步</param>
        ///// <param name="isPool">是否使用WEB页面池</param>
        //internal void LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket, long socketIdentity, bool isAsynchronous, bool isPool)
        //{
        //    DomainServer = domainServer;
        //    Socket = socket;
        //    SocketIdentity = socketIdentity;
        //    IsAsynchronous = isAsynchronous;
        //    IsPool = isPool;
        //}
        ///// <summary>
        ///// 加载查询参数
        ///// </summary>
        ///// <param name="form">HTTP请求表单</param>
        ///// <param name="isAjax">是否ajax请求</param>
        ///// <returns>是否成功</returns>
        //internal virtual void Load(AutoCSer.Net.Http.Form form, bool isAjax)
        //{
        //    throw new InvalidOperationException();
        //}
        /// <summary>
        /// 清除当前请求数据
        /// </summary>
        protected virtual void clear()
        {
            if (PageResponse != null)
            {
                PageResponse.Push();
                PageResponse = null;
            }
#if !NOJIT
            Socket = null;
            DomainServer = null;
            sessionId.Null();
#endif
        }
        /// <summary>
        /// 取消获取请求表单数据操作
        /// </summary>
        internal virtual void CancelGetForm()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 根据HTTP请求表单值获取保存文件全称
        /// </summary>
        /// <param name="name">表单名称</param>
        /// <returns>文件全称</returns>
        internal virtual string GetSaveFileName(byte[] name) { return null; }
        /// <summary>
        /// AJAX回调输出
        /// </summary>
        /// <param name="jsStream">输出数据流</param>
        /// <returns>是否存在回调参数</returns>
        internal bool ResponseAjaxCallBack(CharStream jsStream)
        {
            if (Socket.HttpHeader.AjaxCallBackNameIndex.Length != 0)
            {
                jsStream.JavascriptUnescape(Socket.HttpHeader.AjaxCallBackName);
                jsStream.UnsafeWrite('(');
                return true;
            }
            return false;
        }
        /// <summary>
        /// 输出结束
        /// </summary>
        /// <param name="response">HTTP响应输出</param>
        /// <returns>是否操作成功</returns>
        internal unsafe bool SocketResponse(ref AutoCSer.Net.Http.Response response)
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            try
            {
                AutoCSer.Net.Http.Header header = socket.HttpHeader;
                AutoCSer.Net.Http.HeaderFlag flag = header.Flag;
                if ((flag & AutoCSer.Net.Http.HeaderFlag.IsVersion) != 0 && (flag & Net.Http.HeaderFlag.IsRange) == 0) response.TryStaticFileCacheControl();
                switch (response.Type)
                {
                    case Net.Http.ResponseType.ByteArray:
                    case Net.Http.ResponseType.SubByteArray:
                    case Net.Http.ResponseType.SubBuffer:
                        if ((flag & Net.Http.HeaderFlag.IsRange) == 0)
                        {
#if DOTNET2 || DOTNET4
                            if ((flag & Net.Http.HeaderFlag.IsGZip) != 0 && isGZip) response.Compress();
#else
                            if ((flag & Net.Http.HeaderFlag.IsGZip) != 0 && isGZip) response.Compress(DomainServer.WebConfigIsFastestCompressionLevel);
#endif
                            response.NoStore200(DomainServer.HtmlContentType);
                        }
                        else if (header.FormatRange(response.Body.Array.Length)) response.NoStore200(DomainServer.HtmlContentType);
                        else if (socket.ResponseError(identity, AutoCSer.Net.Http.ResponseState.RangeNotSatisfiable416)) return true;
                        break;
                        //case Net.Http.ResponseType.File: response.SetState(AutoCSer.Net.Http.ResponseState.Ok200); break;
                }
                if (socket.Response(identity, ref response)) return true;
            }
            finally { AutoCSer.Net.Http.Response.Push(ref response); }
            return false;
        }
        /// <summary>
        /// 解析web调用参数
        /// </summary>
        /// <typeparam name="parameterType">web调用参数类型</typeparam>
        /// <param name="parameter">web调用参数</param>
        /// <returns>是否成功</returns>
        public bool ParseParameter<parameterType>(ref parameterType parameter) where parameterType : struct
        {
            switch (Socket.HttpHeader.PostType)
            {
                case AutoCSer.Net.Http.PostType.Json:
                    Socket.ParseForm();
                    string json = Socket.Form.Text;
                    return !string.IsNullOrEmpty(json) && parseJson(ref parameter, json);
                case AutoCSer.Net.Http.PostType.Xml:
                    Socket.ParseForm();
                    return AutoCSer.Xml.Parser.Parse(Socket.Form.Text, ref parameter, xmlParserConfig);
                case AutoCSer.Net.Http.PostType.Data:
                    Socket.ParseForm(dataToType);
                    return parseParameter(ref parameter);
                case AutoCSer.Net.Http.PostType.Form:
                    Socket.ParseForm();
                    return parseParameter(ref parameter);
                case AutoCSer.Net.Http.PostType.FormData:
                    return parseParameter(ref parameter);
                default: return parseParameterQuery(ref parameter);
            }
        }
        /// <summary>
        /// 解析web调用参数
        /// </summary>
        /// <typeparam name="parameterType">web调用参数类型</typeparam>
        /// <param name="parameter">web调用参数</param>
        /// <returns>是否成功</returns>
        private bool parseParameter<parameterType>(ref parameterType parameter) where parameterType : struct
        {
            AutoCSer.Net.Http.Form form = Socket.Form;
            switch (form.TextQueryChar)
            {
                case AutoCSer.Net.Http.Header.QueryJsonNameChar:
                    string json = form.Text;
                    return !string.IsNullOrEmpty(json) && parseJson(ref parameter, json);
                case AutoCSer.Net.Http.Header.QueryXmlNameChar:
                    return AutoCSer.Xml.Parser.Parse(form.Text, ref parameter, xmlParserConfig);
                default: return parseParameterQuery(ref parameter);
            }
        }
        /// <summary>
        /// 解析web调用参数
        /// </summary>
        /// <typeparam name="parameterType">web调用参数类型</typeparam>
        /// <param name="parameter">web调用参数</param>
        /// <returns>是否成功</returns>
        public bool ParseParameterAny<parameterType>(ref parameterType parameter)
        {
            switch (Socket.HttpHeader.PostType)
            {
                case AutoCSer.Net.Http.PostType.Json:
                    Socket.ParseForm();
                    return AutoCSer.Json.Parser.Parse(Socket.Form.Text, ref parameter, jsonParserConfig);
                case AutoCSer.Net.Http.PostType.Xml:
                    Socket.ParseForm();
                    return AutoCSer.Xml.Parser.Parse(Socket.Form.Text, ref parameter, xmlParserConfig);
                case AutoCSer.Net.Http.PostType.Data:
                    Socket.ParseForm(dataToType);
                    return parseParameterAny(ref parameter);
                case AutoCSer.Net.Http.PostType.Form:
                    Socket.ParseForm();
                    return parseParameterAny(ref parameter);
                case AutoCSer.Net.Http.PostType.FormData:
                    return parseParameterAny(ref parameter);
                default: return parseParameterQueryAny(ref parameter);
            }
        }
        /// <summary>
        /// 解析web调用参数
        /// </summary>
        /// <typeparam name="parameterType">web调用参数类型</typeparam>
        /// <param name="parameter">web调用参数</param>
        /// <returns>是否成功</returns>
        private bool parseParameterAny<parameterType>(ref parameterType parameter)
        {
            AutoCSer.Net.Http.Form form = Socket.Form;
            switch (form.TextQueryChar)
            {
                case AutoCSer.Net.Http.Header.QueryJsonNameChar:
                    return AutoCSer.Json.Parser.Parse(form.Text, ref parameter, jsonParserConfig);
                case AutoCSer.Net.Http.Header.QueryXmlNameChar:
                    return AutoCSer.Xml.Parser.Parse(form.Text, ref parameter, xmlParserConfig);
                default: return parseParameterQueryAny(ref parameter);
            }
        }
    }
}
