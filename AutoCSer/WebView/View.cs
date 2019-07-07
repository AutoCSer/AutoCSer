using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 页面视图
    /// </summary>
    public abstract class View : ViewBase
    {
        /// <summary>
        /// 默认 WEB 视图配置
        /// </summary>
        internal static readonly ViewAttribute DefaultAttribute = ConfigLoader.GetUnion(typeof(ViewAttribute)).ViewAttribute ?? new ViewAttribute();
        /// <summary>
        /// HTTP请求头部处理[TRY+1]
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        /// <param name="socket">HTTP套接字接口</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket)
        {
            DomainServer = domainServer;
            Socket = socket;
            SocketIdentity = socket.Identity;
            if (loadHeader()) return true;
            serverError500();
            return false;
        }
        /// <summary>
        /// 加载查询参数[TRY]
        /// </summary>
        internal void LoadPage()
        {
            //long identity = SocketIdentity;
            //AutoCSer.Net.Http.SocketBase socket = Socket;
            //try
            //{
            if (loadView())
            {
                if (!IsAsynchronous) responsePage();
            }
            else
            {
                if (!IsLocationPath) LocationPath.Clear();
                responseLocationPath();
            }
            //    return;
            //}
            //catch (Exception error)
            //{
            //    DomainServer.RegisterServer.TcpServer.Log.add(AutoCSer.Log.LogType.Error, error);
            //}
            //serverError500(socket, identity);
        }
        /// <summary>
        /// 页面输出[TRY+1]
        /// </summary>
        private unsafe void responsePage()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            byte* buffer = null, encodeBuffer = null;
            try
            {
                if (ResponsePage(ref buffer, ref encodeBuffer) && SocketResponse(ref PageResponse)) return;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
                if (encodeBuffer != null) AutoCSer.UnmanagedPool.Default.Push(encodeBuffer);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 输出视图错误重定向路径[TRY+1]
        /// </summary>
        private unsafe void responseLocationPath()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            if (identity == socket.Identity)
            {
                try
                {
                    if (LocationPath.LocationPath == null)
                    {
                        if (LocationPath.ErrorPath == null)
                        {
                            socket.ResponseError(identity, Net.Http.ResponseState.NotFound404);
                            return;
                        }
                        ResponseLocationPath(socket);
                    }
                    else HttpResponse.SetLocation(socket.HttpHeader, LocationPath.LocationPath, Net.Http.ResponseState.Found302);
                    if (SocketResponse(ref PageResponse)) return;
                }
                catch (Exception error)
                {
                    DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
                serverError500(socket, identity);
            }
        }
        /// <summary>
        /// 加载查询参数
        /// </summary>
        internal unsafe void LoadAjax()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            try
            {
                if (loadView())
                {
                    if (!IsAsynchronous) responseAjax();
                }
                else
                {
                    if (!IsLocationPath) setLocationPath(notFound404);
                    responseLocationPathAjax();
                }
                return;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// AJAX 视图输出
        /// </summary>
        private unsafe void responseAjax()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            byte* buffer = null;
            try
            {
                ResponseAjax(ref buffer);
                if (SocketResponse(ref PageResponse)) return;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 输出视图错误重定向路径
        /// </summary>
        private unsafe void responseLocationPathAjax()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            byte* buffer = null;
            try
            {
                ResponseLocationPathAjax(ref buffer);
                if (SocketResponse(ref PageResponse)) return;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 异步输出
        /// </summary>
        public void AsynchronousResponse()
        {
            if (SocketIdentity == Socket.Identity && IsAsynchronous)
            {
                if (Socket.HttpHeader.IsAjax == 0)
                {
                    if (IsLocationPath) responseLocationPath();
                    else responsePage();
                }
                else
                {
                    if (IsLocationPath) responseLocationPathAjax();
                    else responseAjax();
                }
            }
        }
        /// <summary>
        /// 服务器发生不可预期的错误 
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serverError500()
        {
            Socket.ResponseError(SocketIdentity, Net.Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 服务器发生不可预期的错误 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serverError500(AutoCSer.Net.Http.SocketBase socket, long identity)
        {
            socket.ResponseError(identity, Net.Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// 加载HTML数据
        /// </summary>
        /// <param name="fileName">HTML文件</param>
        /// <param name="htmlCount">HTML片段数量验证</param>
        /// <param name="htmlLock">HTML 数据创建访问锁</param>
        /// <param name="htmls">HTML 数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected byte[][] loadHtml(string fileName, int htmlCount, object htmlLock, ref byte[][] htmls)
        {
            if (htmls == null) return LoadHtml(fileName, htmlCount, htmlLock, ref htmls);
            return htmls.Length == 0 ? null : htmls;
        }
    }
    /// <summary>
    /// WEB 页面视图，支持对象池
    /// </summary>
    /// <typeparam name="pageType">WEB页面类型</typeparam>
    [ClearMember(IsIgnoreCurrent = true)]
    public abstract class View<pageType> : ViewBase
        where pageType : View<pageType>
    {
        /// <summary>
        /// HTML 数据
        /// </summary>
        protected static byte[][] htmls;
        /// <summary>
        /// HTML 数据创建访问锁
        /// </summary>
        protected static readonly object htmlLock = new object();
        /// <summary>
        /// HTTP请求头部处理[TRY+1]
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="page"></param>
        /// <returns>是否成功</returns>
        internal bool LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket, pageType page)
        {
            DomainServer = domainServer;
            Socket = socket;
            SocketIdentity = socket.Identity;
            if (next == null) setPool(page);
            try
            {
                if (loadHeader()) return true;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            serverError500();
            return false;
        }
        /// <summary>
        /// 加载查询参数[TRY]
        /// </summary>
        internal void LoadPage()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            try
            {
                if (loadView())
                {
                    if (!IsAsynchronous) responsePage();
                }
                else
                {
                    if (!IsLocationPath) LocationPath.Clear();
                    responseLocationPath();
                }
                return;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 页面输出[TRY+1]
        /// </summary>
        private unsafe void responsePage()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            byte* buffer = null, encodeBuffer = null;
            try
            {
                if (ResponsePage(ref buffer, ref encodeBuffer) && SocketResponse(ref PageResponse))
                {
                    push();
                    return;
                }
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
                if (encodeBuffer != null) AutoCSer.UnmanagedPool.Default.Push(encodeBuffer);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 输出视图错误重定向路径[TRY+1]
        /// </summary>
        private unsafe void responseLocationPath()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            if (identity == socket.Identity)
            {
                try
                {
                    if (LocationPath.LocationPath == null)
                    {
                        if (LocationPath.ErrorPath == null)
                        {
                            if (socket.ResponseError(identity, Net.Http.ResponseState.NotFound404)) push();
                            return;
                        }
                        ResponseLocationPath(socket);
                    }
                    else HttpResponse.SetLocation(socket.HttpHeader, LocationPath.LocationPath, Net.Http.ResponseState.Found302);
                    if (SocketResponse(ref PageResponse))
                    {
                        push();
                        return;
                    }
                }
                catch (Exception error)
                {
                    DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
                serverError500(socket, identity);
            }
        }
        /// <summary>
        /// 加载查询参数
        /// </summary>
        internal unsafe void LoadAjax()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            try
            {
                if (loadView())
                {
                    if (!IsAsynchronous) responseAjax();
                }
                else
                {
                    if (!IsLocationPath) setLocationPath(notFound404);
                    responseLocationPathAjax();
                }
                return;
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// AJAX视图输出
        /// </summary>
        private unsafe void responseAjax()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            byte* buffer = null;
            try
            {
                ResponseAjax(ref buffer);
                if (SocketResponse(ref PageResponse))
                {
                    push();
                    return;
                }
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 输出视图错误重定向路径
        /// </summary>
        private unsafe void responseLocationPathAjax()
        {
            long identity = SocketIdentity;
            AutoCSer.Net.Http.SocketBase socket = Socket;
            byte* buffer = null;
            try
            {
                ResponseLocationPathAjax(ref buffer);
                if (SocketResponse(ref PageResponse))
                {
                    push();
                    return;
                }
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
            }
            serverError500(socket, identity);
        }
        /// <summary>
        /// 异步输出
        /// </summary>
        public void AsynchronousResponse()
        {
            if (SocketIdentity == Socket.Identity && IsAsynchronous)
            {
                if (Socket.HttpHeader.IsAjax == 0)
                {
                    if (IsLocationPath) responseLocationPath();
                    else responsePage();
                }
                else
                {
                    if (IsLocationPath) responseLocationPathAjax();
                    else responseAjax();
                }
            }
        }
        /// <summary>
        /// 服务器发生不可预期的错误 
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serverError500()
        {
            if (Socket.ResponseError(SocketIdentity, Net.Http.ResponseState.ServerError500)) push();
        }
        /// <summary>
        /// 服务器发生不可预期的错误 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serverError500(AutoCSer.Net.Http.SocketBase socket, long identity)
        {
            if (socket.ResponseError(identity, Net.Http.ResponseState.ServerError500)) push();
        }
        /// <summary>
        /// 加载HTML数据 XXX
        /// </summary>
        /// <param name="fileName">HTML文件</param>
        /// <param name="htmlCount">HTML片段数量验证</param>
        /// <returns></returns>
        protected bool isLoadHtml(string fileName, int htmlCount)
        {
            if (htmls != null) return true;
            if (File.Exists(fileName = DomainServer.WorkPath + fileName))
            {
                try
                {
                    ViewTreeBuilder treeBuilder = new ViewTreeBuilder(File.ReadAllText(fileName, DomainServer.ResponseEncoding.Encoding));
                    if (treeBuilder.Htmls.Count == htmlCount)
                    {
                        htmls = treeBuilder.HtmlArray.getArray(value => DomainServer.ResponseEncoding.GetBytesNotNull(value));
                        return true;
                    }
                    DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, "HTML模版文件不匹配 " + fileName);
                }
                catch (Exception error)
                {
                    DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error, fileName);
                }
            }
            else DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, "没有找到HTML模版文件 " + fileName);
            return false;
        }
        /// <summary>
        /// 加载HTML数据
        /// </summary>
        /// <param name="fileName">HTML文件</param>
        /// <param name="htmlCount">HTML片段数量验证</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected byte[][] loadHtml(string fileName, int htmlCount)
        {
            if (htmls == null) return LoadHtml(fileName, htmlCount, htmlLock, ref htmls);
            return htmls.Length == 0 ? null : htmls;
        }
        /// <summary>
        /// 加载HTML数据
        /// </summary>
        /// <param name="fileName">HTML文件</param>
        /// <param name="htmlCount">HTML片段数量验证</param>
        /// <param name="htmlLock">HTML 数据创建访问锁</param>
        /// <param name="htmls">HTML 数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected byte[][] loadHtml(string fileName, int htmlCount, object htmlLock, ref byte[][] htmls)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// WEB页面回收
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void push()
        {
            if (IsPool)
            {
                IsPool = false;
                if (Object.ReferenceEquals(this, next))
                {
                    clear();
                    PushNotNull(next);
                    return;
                }
            }
            throw new InvalidOperationException(typeof(pageType).fullName());
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static pageType Pop()
        {
            while (System.Threading.Interlocked.CompareExchange(ref popLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
            pageType headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    System.Threading.Interlocked.Exchange(ref popLock, 0);
                    return null;
                }
                if (System.Threading.Interlocked.CompareExchange(ref poolHead, headValue.next, headValue) == headValue)
                {
                    System.Threading.Interlocked.Exchange(ref popLock, 0);
                    System.Threading.Interlocked.Decrement(ref poolCount);
                    headValue.setPool(headValue);
                    return headValue;
                }
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
            }
            while (true);
        }
        /// <summary>
        /// 下一个节点
        /// </summary>
        private pageType next;
        /// <summary>
        /// 使用对象池状态设置
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setPool(pageType next)
        {
            IsPool = true;
            this.next = next;
        }
        /// <summary>
        /// 清除页面信息
        /// </summary>
        protected override void clear()
        {
            ViewKeywords = ViewDescription = null;
            IsLocationPath = false;
#if !NOJIT
            ClearMember<pageType>.Cleaner(next);
#endif
            base.clear();
        }
        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int poolMaxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(pageType));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static pageType poolHead;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private static int popLock;
        /// <summary>
        /// 缓存数量
        /// </summary>
        private static int poolCount;
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PushNotNull(pageType value)
        {
            if (poolCount >= poolMaxCount)
            {
                value.Dispose();
                return;
            }
            System.Threading.Interlocked.Increment(ref poolCount);
            pageType headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    value.next = null;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, null) == null) return;
                }
                else
                {
                    value.next = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
            }
            while (true);
        }
        /// <summary>
        /// 添加链表
        /// </summary>
        /// <param name="value">链表头部</param>
        /// <param name="end">链表尾部</param>
        /// <param name="count">数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void pushLink(pageType value, pageType end, int count)
        {
            System.Threading.Interlocked.Add(ref poolCount, count);
            pageType headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    end.next = null;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, null) == null) return;
                }
                else
                {
                    end.next = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
            }
            while (true);
        }
        /// <summary>
        /// 释放列表
        /// </summary>
        /// <param name="value"></param>
        private static void disposeLink(pageType value)
        {
            do
            {
                value.Dispose();
            }
            while ((value = value.next) != null);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            pageType headValue = System.Threading.Interlocked.Exchange(ref poolHead, null);
            poolCount = 0;
            if (headValue != null)
            {
                if (count == 0) disposeLink(headValue);
                else
                {
                    int pushCount = count;
                    pageType end = headValue;
                    while (--count != 0)
                    {
                        if (end.next == null)
                        {
                            pushLink(headValue, end, pushCount - count);
                            return;
                        }
                        end = end.next;
                    }
                    pageType next = end.next;
                    end.next = null;
                    pushLink(headValue, end, pushCount);
                    if (next != null) disposeLink(next);
                }
            }
        }
        static View()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
