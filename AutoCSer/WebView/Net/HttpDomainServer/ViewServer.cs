using System;
using AutoCSer.Extension;
using AutoCSer.Log;
using System.Runtime.CompilerServices;
using System.IO;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// WEB 视图服务
    /// </summary>
    public abstract partial class ViewServer : FileServer
    {
        /// <summary>
        /// WEB视图URL重写路径集合
        /// </summary>
        protected virtual KeyValue<string[], string[]> rewrites
        {
            get { return new KeyValue<string[], string[]>(NullValue<string>.Array, NullValue<string>.Array); }
        }
        /// <summary>
        /// WEB视图URL重写路径集合
        /// </summary>
        private StateSearcher.AsciiSearcher<byte[]> rewritePaths;
        /// <summary>
        /// WEB视图页面索引集合
        /// </summary>
        protected virtual string[] views
        {
            get { return NullValue<string>.Array; }
        }
        /// <summary>
        /// WEB 视图页面索引集合
        /// </summary>
        private Pointer.Size viewIndexs;
        /// <summary>
        /// WEB 视图页面搜索器
        /// </summary>
        private StateSearcher.AsciiSearcher viewSearcher;
        /// <summary>
        /// WEB调用处理索引集合
        /// </summary>
        protected virtual string[] calls
        {
            get { return NullValue<string>.Array; }
        }
        /// <summary>
        /// WEB 调用处理索引集合
        /// </summary>
        private Pointer.Size callIndexs;
        /// <summary>
        /// WEB 调用处理搜索器
        /// </summary>
        private StateSearcher.AsciiSearcher callSearcher;
        /// <summary>
        /// WEB视图URL重写索引集合
        /// </summary>
        protected virtual string[] viewRewrites
        {
            get { return NullValue<string>.Array; }
        }
        /// <summary>
        /// WEB 视图 URL 重写索引集合
        /// </summary>
        private Pointer.Size rewriteIndexs;
        /// <summary>
        /// WEB 视图 URL 重写搜索器
        /// </summary>
        private StateSearcher.AsciiSearcher rewriteSearcher;
        /// <summary>
        /// WebSocket调用处理集合
        /// </summary>
        protected virtual string[] webSockets
        {
            get { return NullValue<string>.Array; }
        }
        /// <summary>
        /// WebSocket 调用处理委托集合
        /// </summary>
        private Pointer.Size webSocketIndexs;
        /// <summary>
        /// WebSocket 调用处理搜索器
        /// </summary>
        private StateSearcher.AsciiSearcher webSocketSearcher; 
        /// <summary>
        /// 空 AJAX 响应输出
        /// </summary>
        internal readonly AutoCSer.Net.Http.Response NullAjaxResponse;
        /// <summary>
        /// 空 AJAX 响应输出（静态资源处理）
        /// </summary>
        internal readonly AutoCSer.Net.Http.Response NullAjaxResponseStaticFile;
        /// <summary>
        /// 静态资源版本字符串
        /// </summary>
        internal string StaticFileVersion;
        /// <summary>
        /// 客户端日志处理
        /// </summary>
        protected virtual ILog webClientLog { get { return null; } }
        /// <summary>
        /// 客户端日志处理
        /// </summary>
        internal ILog WebClientLog;
        /// <summary>
        /// WEB 视图服务
        /// </summary>
        protected ViewServer()
            : base()
        {
            (NullAjaxResponse = AutoCSer.Net.Http.Response.New()).SetJsContentType(this);
            NullAjaxResponse.SetBody();
            (NullAjaxResponseStaticFile = AutoCSer.Net.Http.Response.New()).SetJsContentType(this);
            NullAjaxResponseStaticFile.CacheControl = AutoCSer.Net.Http.Response.StaticFileCacheControl;
            NullAjaxResponseStaticFile.SetBody();
            WebClientLog = webClientLog ?? AutoCSer.Log.Pub.Log;
        }
        /// <summary>
        /// 启动HTTP服务
        /// </summary>
        /// <param name="registerServer">HTTP 注册管理服务</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="onStop">停止服务处理</param>
        /// <returns>是否启动成功</returns>
        public override bool Start(HttpRegister.Server registerServer, HttpRegister.Domain[] domains, Action onStop)
        {
            KeyValue<string[], string[]> rewrites = this.rewrites;
            rewritePaths = new StateSearcher.AsciiSearcher<byte[]>(rewrites.Key, rewrites.Value.getArray(value => value.getBytes()), false);
            viewIndexs = StateSearcher.AsciiBuilder.Create(views, false);
            viewSearcher = new StateSearcher.AsciiSearcher(viewIndexs.Pointer);
            callIndexs = StateSearcher.AsciiBuilder.Create(calls, false);
            callSearcher = new StateSearcher.AsciiSearcher(callIndexs.Pointer);
            rewriteIndexs = StateSearcher.AsciiBuilder.Create(viewRewrites, false);
            rewriteSearcher = new StateSearcher.AsciiSearcher(rewriteIndexs.Pointer);
            webSocketIndexs = StateSearcher.AsciiBuilder.Create(webSockets, false);
            webSocketSearcher = new StateSearcher.AsciiSearcher(webSocketIndexs.Pointer);
            if (base.Start(registerServer, domains, onStop))
            {
                if (WebConfig.IsHtmlLinkVersion) VersionFileWatcher.Add(this);
                return true;
            }
            return false;
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        unsafe ~ViewServer()
        {
            if (webSocketIndexs.Data != null)
            {
                Unmanaged.Free(ref viewIndexs);
                Unmanaged.Free(ref callIndexs);
                Unmanaged.Free(ref rewriteIndexs);
                Unmanaged.Free(ref webSocketIndexs);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected new unsafe bool dispose()
        {
            if (base.dispose())
            {
                if (rewritePaths != null) rewritePaths.Dispose();
                viewSearcher.State = callSearcher.State = rewriteSearcher.State = webSocketSearcher.State = null;
                Unmanaged.Free(ref viewIndexs);
                Unmanaged.Free(ref callIndexs);
                Unmanaged.Free(ref rewriteIndexs);
                Unmanaged.Free(ref webSocketIndexs);
                if(WebConfig.IsHtmlLinkVersion) VersionFileWatcher.Remove(this);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            dispose();
        }
        /// <summary>
        /// HTTP请求处理[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public override unsafe void Request(Http.SocketBase socket)
        {
            Http.Header header = socket.HttpHeader;
            SubArray<byte> path = header.Path;
            int index;
            if (header.IsSearchEngine == 0)
            {
                byte[] rewritePath;
#if !MONO
                if (WebConfigIgnoreCase)
                {
                    if ((index = callSearcher.SearchLower(ref path)) >= 0)
                    {
                        call(index, socket);
                        return;
                    }
                    rewritePath = rewritePaths.GetLower(ref path);
                }
                else
#endif
                {
                    if ((index = callSearcher.Search(ref path)) >= 0)
                    {
                        call(index, socket);
                        return;
                    }
                    rewritePath = rewritePaths.Get(ref path);

                }
                if (rewritePath != null)
                {
                    Http.Response response = null;
                    file(header, file(rewritePath, (header.Flag & Http.HeaderFlag.IsSetIfModifiedSince) == 0 ? default(SubArray<byte>) : header.IfModifiedSince, ref response, false), ref response);
                    if (response != null)
                    {
                        socket.ResponseIdentity(ref response);
                        return;
                    }
                    socket.ResponseErrorIdentity(Http.ResponseState.NotFound404);
                }
            }
            else
            {
#if !MONO
                if (WebConfigIgnoreCase)
                {
                    if ((index = rewriteSearcher.SearchLower(ref path)) >= 0)
                    {
                        request(index, socket);
                        return;
                    }
                    if ((index = viewSearcher.SearchLower(ref path)) >= 0)
                    {
                        request(index, socket);
                        return;
                    }
                    if ((index = callSearcher.SearchLower(ref path)) >= 0)
                    {
                        call(index, socket);
                        return;
                    }
                }
                else
#endif
                {
                    if ((index = rewriteSearcher.Search(ref path)) >= 0)
                    {
                        request(index, socket);
                        return;
                    }
                    if ((index = viewSearcher.Search(ref path)) >= 0)
                    {
                        request(index, socket);
                        return;
                    }
                    if ((index = callSearcher.Search(ref path)) >= 0)
                    {
                        call(index, socket);
                        return;
                    }
                }
            }
            if (beforeFile(socket)) base.Request(socket);
        }
        /// <summary>
        /// 视图页面处理
        /// </summary>
        /// <param name="viewIndex"></param>
        /// <param name="socket"></param>
        protected virtual void request(int viewIndex, Http.SocketBase socket) { }
        /// <summary>
        /// WEB 调用处理
        /// </summary>
        /// <param name="callIndex"></param>
        /// <param name="socket"></param>
        protected virtual void call(int callIndex, Http.SocketBase socket) { }
        /// <summary>
        /// 文件服务处理之前
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>是否需要继续提交给文件服务处理</returns>
        protected virtual bool beforeFile(Http.SocketBase socket)
        {
            return true;
        }
        /// <summary>
        /// 获取WEB视图URL重写路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte[] GetViewRewrite(SubArray<byte> path)
        {
#if MONO
            return rewritePaths.Get(ref path);
#else
            return WebConfigIgnoreCase ? rewritePaths.GetLower(ref path) : rewritePaths.Get(ref path);
#endif
        }
        /// <summary>
        /// WebSocket请求处理
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public override void WebSocketRequest(Http.SocketBase socket)
        {
            int index = webSocketSearcher.Search(socket.HttpHeader.Path);
            if (index >= 0)
            {
                long socketIdentity = socket.Identity;
                try
                {
                    callWebSocket(index, socket);
                    return;
                }
                catch (Exception error)
                {
                    socket.ResponseError(socketIdentity, Http.ResponseState.ServerError500);
                    RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.NotFound404);
        }
        /// <summary>
        /// webSocket调用处理
        /// </summary>
        /// <param name="callIndex"></param>
        /// <param name="socket"></param>
        protected virtual void callWebSocket(int callIndex, Http.SocketBase socket)
        {
            socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 加载页面视图[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="view">WEB视图接口</param>
        protected void loadPage<viewType>(Http.SocketBase socket, viewType view)
            where viewType : AutoCSer.WebView.View<viewType>
        {
            if (socket.HttpHeader.Method == Http.MethodType.GET)
            {
                if (view.LoadHeader(this, socket, view)) view.LoadPage();
            }
            else
            {
                AutoCSer.WebView.View<viewType>.PushNotNull(view);
                socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
            }
        }
        /// <summary>
        /// 加载页面视图[TRY]
        /// </summary>
        /// <param name="view">WEB视图接口</param>
        /// <param name="isAsynchronous">是否异步</param>
        /// <param name="isAwaitMethod">是否存在 await 函数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void setPage(AutoCSer.WebView.ViewBase view, bool isAsynchronous, bool isAwaitMethod)
        {
            view.SetAsynchronousAwaitMethod(isAsynchronous, isAwaitMethod);
        }
        /// <summary>
        /// 加载页面视图[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="view">WEB视图接口</param>
        /// <param name="isAsynchronous">是否异步</param>
        /// <param name="isAwaitMethod">是否存在 await 函数</param>
        protected void loadPage(Http.SocketBase socket, AutoCSer.WebView.View view, bool isAsynchronous, bool isAwaitMethod)
        {
            if (socket.HttpHeader.Method == Http.MethodType.GET)
            {
                view.SetAsynchronousAwaitMethod(isAsynchronous, isAwaitMethod);
                if (view.LoadHeader(this, socket)) view.LoadPage();
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 加载页面视图[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="view">WEB视图接口</param>
        protected void loadPage(Http.SocketBase socket, AutoCSer.WebView.View view)
        {
            if (socket.HttpHeader.Method == Http.MethodType.GET)
            {
                if (view.LoadHeader(this, socket)) view.LoadPage();
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 加载页面视图
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="view">WEB视图接口</param>
        internal void LoadAjax<viewType>(Http.SocketBase socket, viewType view)
            where viewType : AutoCSer.WebView.View<viewType>
        {
            if (socket.HttpHeader.Method == Http.MethodType.GET)
            {
                if (view.LoadHeader(this, socket, view)) view.LoadAjax();
            }
            else
            {
                AutoCSer.WebView.View<viewType>.PushNotNull(view);
                socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
            }
        }
        /// <summary>
        /// 加载页面视图
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="view">WEB视图接口</param>
        internal void LoadAjax(Http.SocketBase socket, AutoCSer.WebView.View view)
        {
            if (socket.HttpHeader.Method == Http.MethodType.GET)
            {
                if (view.LoadHeader(this, socket)) view.LoadAjax();
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 加载 HTTP 调用
        /// </summary>
        /// <typeparam name="callType"></typeparam>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="call">HTTP 调用</param>
        /// <param name="callInfo">HTTP 调用函数信息</param>
        protected void load<callType>(Http.SocketBase socket, callType call, AutoCSer.WebView.CallMethodInfo callInfo)
            where callType : AutoCSer.WebView.Call<callType>
        {
            Http.Header header = socket.HttpHeader;
            if (header.ContentLength <= callInfo.MaxPostDataSize && (header.Method == Http.MethodType.POST || !callInfo.IsOnlyPost))
            {
                call.LoadHeader(this, socket, call, callInfo);
                if (header.Method == Http.MethodType.POST && header.ContentLength != 0)
                {
                    socket.GetForm(call, Http.GetFormType.Call);
                    return;
                }
                long socketIdentity = socket.Identity;
                if (this.callSynchronize(call)) return;
                call.PushPool();
                socket.ResponseError(socketIdentity, Http.ResponseState.ServerError500);
            }
            else
            {
                AutoCSer.WebView.Call<callType>.PushNotNull(call);
                socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
            }
        }
        /// <summary>
        /// 加载web调用
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="call">web调用</param>
        /// <param name="callInfo">HTTP 调用函数信息</param>
        protected void load(Http.SocketBase socket, AutoCSer.WebView.Call call, AutoCSer.WebView.CallMethodInfo callInfo)
        {
            Http.Header header = socket.HttpHeader;
            if (header.ContentLength <= callInfo.MaxPostDataSize && (header.Method == Http.MethodType.POST || !callInfo.IsOnlyPost))
            {
                call.LoadHeader(this, socket, callInfo);
                if (header.Method == Http.MethodType.POST && header.ContentLength != 0)
                {
                    socket.GetForm(call, Http.GetFormType.Call);
                    return;
                }
                long socketIdentity = socket.Identity;
                if (this.callSynchronize(call)) return;
                socket.ResponseError(socketIdentity, Http.ResponseState.ServerError500);
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 加载 HTTP 调用[TRY]
        /// </summary>
        /// <typeparam name="callType"></typeparam>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="call">HTTP 调用</param>
        /// <param name="callInfo">HTTP 调用函数信息</param>
        protected void loadAsynchronous<callType>(Http.SocketBase socket, callType call, AutoCSer.WebView.CallMethodInfo callInfo)
            where callType : AutoCSer.WebView.CallAsynchronous<callType>
        {
            Http.Header header = socket.HttpHeader;
            if (header.ContentLength <= callInfo.MaxPostDataSize && (header.Method == Http.MethodType.POST || !callInfo.IsOnlyPost))
            {
                call.LoadHeader(this, socket, call, callInfo);
                if (header.Method == Http.MethodType.POST && header.ContentLength != 0)
                {
                    socket.GetForm(call, Http.GetFormType.CallAsynchronous);
                    return;
                }
                long socketIdentity = socket.Identity;
                if (this.call(call)) return;
                call.PushPool();
                socket.ResponseError(socketIdentity, Http.ResponseState.ServerError500);
            }
            else
            {
                AutoCSer.WebView.CallAsynchronous<callType>.PushNotNull(call);
                socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
            }
        }
        /// <summary>
        /// 加载web调用[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="call">web调用</param>
        /// <param name="callInfo">HTTP 调用函数信息</param>
        protected void loadAsynchronous(Http.SocketBase socket, AutoCSer.WebView.CallAsynchronous call, AutoCSer.WebView.CallMethodInfo callInfo)
        {
            Http.Header header = socket.HttpHeader;
            if (header.ContentLength <= callInfo.MaxPostDataSize && (header.Method == Http.MethodType.POST || !callInfo.IsOnlyPost))
            {
                call.LoadHeader(this, socket, callInfo);
                if (header.Method == Http.MethodType.POST && header.ContentLength != 0)
                {
                    socket.GetForm(call, Http.GetFormType.CallAsynchronous);
                    return;
                }
                long socketIdentity = socket.Identity;
                if (this.call(call)) return;
                socket.ResponseError(socketIdentity, Http.ResponseState.ServerError500);
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 初始化 AJAX 调用加载
        /// </summary>
        /// <param name="ajaxLoader"></param>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void ajaxLoader<callType>(callType ajaxLoader, AutoCSer.Net.Http.SocketBase socket)
            where callType : AutoCSer.WebView.CallAsynchronous<callType>
        {
            ajaxLoader.LoadHeader(this, socket, ajaxLoader);
        }
        /// <summary>
        /// WEB 调用同步处理
        /// </summary>
        /// <param name="call"></param>
        /// <param name="responseStream"></param>
        /// <returns></returns>
        protected virtual bool call(AutoCSer.WebView.CallBase call, ref UnmanagedStream responseStream) { return false; }
        /// <summary>
        /// WEB 调用异步处理
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        protected virtual bool call(AutoCSer.WebView.CallBase call) { return false; }
        /// <summary>
        /// WEB 调用处理
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Call(AutoCSer.WebView.CallSynchronize call)
        {
            return this.callSynchronize(call);
        }
        /// <summary>
        /// WEB 调用处理
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Call(AutoCSer.WebView.CallAsynchronousBase call)
        {
            return this.call(call);
        }
        /// <summary>
        /// WEB 调用处理
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        private unsafe bool callSynchronize(AutoCSer.WebView.CallBase call)
        {
            //if (call.IsAsynchronous) return this.call(callInfo.MethodIndex, call);
            UnmanagedStream responseStream = null;
            long identity = call.SocketIdentity;
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                responseStream = call.GetResponseStream(buffer, AutoCSer.UnmanagedPool.DefaultSize);
                return this.call(call, ref responseStream);
            }
            finally
            {
                AutoCSer.UnmanagedPool.Default.Push(buffer);
                if (responseStream != null)
                {
                    responseStream.Dispose();
                    call.ResponseStream = responseStream;
                }
            }
        }
        /// <summary>
        /// 同步输出
        /// </summary>
        /// <typeparam name="callType"></typeparam>
        /// <param name="call"></param>
        /// <param name="responseStream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void repsonseCall<callType>(callType call, ref UnmanagedStream responseStream)
            where callType : AutoCSer.WebView.Call<callType>
        {
            call.RepsonseEnd(ref responseStream);
        }
        /// <summary>
        /// 同步输出
        /// </summary>
        /// <param name="call"></param>
        /// <param name="responseStream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void repsonseCall(AutoCSer.WebView.Call call, ref UnmanagedStream responseStream)
        {
            call.RepsonseEnd(ref responseStream);
        }
    }
    /// <summary>
    /// WEB视图服务
    /// </summary>
    public abstract class ViewServer<sessionType> : ViewServer
    {
#if NOJIT
        /// <summary>
        /// 启动HTTP服务
        /// </summary>
        /// <param name="registerServer">HTTP 注册管理服务</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="onStop">停止服务处理</param>
        /// <returns>是否启动成功</returns>
        public override bool Start(HttpRegister.Server registerServer, HttpRegister.Domain[] domains, Action onStop)
        {
            Session = getSession();
            return base.Start(registerServer, domains, onStop);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <returns>Session</returns>
        protected virtual ISession getSession()
        {
            return new Session<sessionType>();
        }
#else
        /// <summary>
        /// Session
        /// </summary>
        private ISession<sessionType> session;
        /// <summary>
        /// 启动HTTP服务
        /// </summary>
        /// <param name="registerServer">HTTP 注册管理服务</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="onStop">停止服务处理</param>
        /// <returns>是否启动成功</returns>
        public override bool Start(HttpRegister.Server registerServer, HttpRegister.Domain[] domains, Action onStop)
        {
            Session = session = getSession();
            return base.Start(registerServer, domains, onStop);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <returns>Session</returns>
        protected virtual ISession<sessionType> getSession()
        {
            return new Session<sessionType>();
        }
#endif
    }
}
