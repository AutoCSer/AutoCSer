using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.WebView
{
    /// <summary>
    /// AJAX 调用
    /// </summary>
    public abstract class Ajax : AjaxBase
    {
        /// <summary>
        /// 默认 AJAX 调用配置
        /// </summary>
        internal static readonly AjaxAttribute DefaultAttribute = ConfigLoader.GetUnion(typeof(AjaxAttribute)).AjaxAttribute ?? new AjaxAttribute();
        /// <summary>
        /// HTTP请求头部处理
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="isAsynchronous">是否异步</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket, bool isAsynchronous)
        {
            DomainServer = domainServer;
            Socket = socket;
            SocketIdentity = socket.Identity;
            IsAsynchronous = isAsynchronous;
        }
        /// <summary>
        /// AJAX 响应输出
        /// </summary>
        /// <typeparam name="valueType">输出数据类型</typeparam>
        /// <param name="value">输出数据</param>
        internal unsafe void Response<valueType>(ref valueType value) where valueType : struct
        {
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                Response(ref value, buffer);
                SocketResponse(ref PageResponse);
            }
            finally { AutoCSer.UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// AJAX响应输出
        /// </summary>
        internal unsafe void Response()
        {
            if (Socket.HttpHeader.AjaxCallBackNameIndex.Length == 0 && PageResponse == null)
            {
                AutoCSer.Net.Http.Response response = (Socket.HttpHeader.Flag & Net.Http.HeaderFlag.IsVersion) == 0 ? DomainServer.NullAjaxResponse : DomainServer.NullAjaxResponseStaticFile;
                SocketResponse(ref response);
            }
            else
            {
                byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                try
                {
                    Response(buffer);
                    SocketResponse(ref PageResponse);
                }
                finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
            }
        }

        /// <summary>
        /// 取消获取请求表单数据操作
        /// </summary>
        internal override void CancelGetForm() { }
        /// <summary>
        /// 服务器发生不可预期的错误 
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ServerError500()
        {
            Socket.ResponseError(SocketIdentity, Net.Http.ResponseState.ServerError500);
        }
    }
    /// <summary>
    /// AJAX 调用，支持对象池
    /// </summary>
    /// <typeparam name="ajaxType">AJAX 调用类型</typeparam>
    [AutoCSer.WebView.ClearMember(IsIgnoreCurrent = true)]
    public abstract class Ajax<ajaxType> : AjaxBase
        where ajaxType : Ajax<ajaxType>
    {
        /// <summary>
        /// HTTP请求头部处理
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="isAsynchronous">是否异步</param>
        /// <param name="ajax"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket, bool isAsynchronous, ajaxType ajax)
        {
            DomainServer = domainServer;
            Socket = socket;
            SocketIdentity = socket.Identity;
            IsAsynchronous = isAsynchronous;
            if (next == null) setPool(ajax);
        }
        /// <summary>
        /// AJAX响应输出
        /// </summary>
        /// <typeparam name="valueType">输出数据类型</typeparam>
        /// <param name="value">输出数据</param>
        internal unsafe void Response<valueType>(ref valueType value) where valueType : struct
        {
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                Response(ref value, buffer);
                if (SocketResponse(ref PageResponse)) PushPool();
            }
            finally { AutoCSer.UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// AJAX响应输出
        /// </summary>
        internal unsafe void Response()
        {
            if (Socket.HttpHeader.AjaxCallBackNameIndex.Length == 0 && PageResponse == null)
            {
                AutoCSer.Net.Http.Response response = (Socket.HttpHeader.Flag & Net.Http.HeaderFlag.IsVersion) == 0 ? DomainServer.NullAjaxResponse : DomainServer.NullAjaxResponseStaticFile;
                if (SocketResponse(ref response)) PushPool();
            }
            else
            {
                byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                try
                {
                    Response(buffer);
                    if (SocketResponse(ref PageResponse)) PushPool();
                }
                finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
            }
        }

        /// <summary>
        /// 取消获取请求表单数据操作
        /// </summary>
        internal override void CancelGetForm()
        {
            PushPool();
        }
        /// <summary>
        /// 服务器发生不可预期的错误 
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ServerError500()
        {
            if (Socket.ResponseError(SocketIdentity, Net.Http.ResponseState.ServerError500)) PushPool();
        }
        /// <summary>
        /// WEB页面回收
        /// </summary>
        internal void PushPool()
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
            throw new InvalidOperationException(typeof(ajaxType).fullName());
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ajaxType Pop()
        {
            while (System.Threading.Interlocked.CompareExchange(ref popLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
            ajaxType headValue;
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
        private ajaxType next;
        /// <summary>
        /// 使用对象池状态设置
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setPool(ajaxType next)
        {
            IsPool = true;
            this.next = next;
        }
        /// <summary>
        /// 清除页面信息
        /// </summary>
        protected override void clear()
        {
#if !NOJIT
            ClearMember<ajaxType>.Cleaner(next);
#endif
            base.clear();
        }
        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int poolMaxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(ajaxType));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static ajaxType poolHead;
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
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PushNotNull(ajaxType value)
        {
            if (poolCount >= poolMaxCount)
            {
                value.Dispose();
                return;
            }
            System.Threading.Interlocked.Increment(ref poolCount);
            ajaxType headValue;
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
        private static void pushLink(ajaxType value, ajaxType end, int count)
        {
            System.Threading.Interlocked.Add(ref poolCount, count);
            ajaxType headValue;
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
        private static void disposeLink(ajaxType value)
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
            ajaxType headValue = System.Threading.Interlocked.Exchange(ref poolHead, null);
            poolCount = 0;
            if (headValue != null)
            {
                if (count == 0) disposeLink(headValue);
                else
                {
                    int pushCount = count;
                    ajaxType end = headValue;
                    while (--count != 0)
                    {
                        if (end.next == null)
                        {
                            pushLink(headValue, end, pushCount - count);
                            return;
                        }
                        end = end.next;
                    }
                    ajaxType next = end.next;
                    end.next = null;
                    pushLink(headValue, end, pushCount);
                    if (next != null) disposeLink(next);
                }
            }
        }
        static Ajax()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
