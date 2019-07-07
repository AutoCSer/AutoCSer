using System;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;
using System.IO;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTTP 调用
    /// </summary>
    public abstract class Call : CallSynchronize
    {
        /// <summary>
        /// 默认 WEB 调用配置
        /// </summary>
        internal static readonly CallAttribute DefaultAttribute = ConfigLoader.GetUnion(typeof(CallAttribute)).CallAttribute ?? new CallAttribute();
        /// <summary>
        /// HTTP请求头部处理
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="callInfo">HTTP 调用函数信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket, CallMethodInfo callInfo)
        {
            DomainServer = domainServer;
            Socket = socket;
            SocketIdentity = socket.Identity;
            MethodInfo = callInfo;
        }
        /// <summary>
        /// 结束同步输出
        /// </summary>
        /// <param name="responseStream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RepsonseEnd(ref UnmanagedStream responseStream)
        {
            if (SocketIdentity == Socket.Identity)
            {
                HttpResponse.SetBody(responseStream);
                responseStream.Dispose();
                ResponseStream = responseStream;
                responseStream = null;
                SocketResponse(ref PageResponse);
            }
        }
        /// <summary>
        /// 取消获取请求表单数据操作
        /// </summary>
        internal override void CancelGetForm() { }
    }
    /// <summary>
    /// HTTP 调用，支持对象池
    /// </summary>
    /// <typeparam name="callType">web调用类型</typeparam>
    [AutoCSer.WebView.ClearMember(IsIgnoreCurrent = true)]
    public abstract class Call<callType> : CallSynchronize
        where callType : Call<callType>
    {
        /// <summary>
        /// HTTP请求头部处理
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        /// <param name="socket">HTTP套接字接口</param>
        /// <param name="call"></param>
        /// <param name="callInfo">HTTP 调用函数信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void LoadHeader(AutoCSer.Net.HttpDomainServer.ViewServer domainServer, AutoCSer.Net.Http.SocketBase socket, callType call, CallMethodInfo callInfo)
        {
            DomainServer = domainServer;
            Socket = socket;
            SocketIdentity = socket.Identity;
            MethodInfo = callInfo;
            if (next == null) setPool(call);
        }
        /// <summary>
        /// 结束同步输出
        /// </summary>
        /// <param name="responseStream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RepsonseEnd(ref UnmanagedStream responseStream)
        {
            if (SocketIdentity == Socket.Identity)
            {
                HttpResponse.SetBody(responseStream);
                responseStream.Dispose();
                ResponseStream = responseStream;
                responseStream = null;
                if (SocketResponse(ref PageResponse)) PushPool();
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
            throw new InvalidOperationException(typeof(callType).fullName());
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static callType Pop()
        {
            while (System.Threading.Interlocked.CompareExchange(ref popLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
            callType headValue;
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
        private callType next;
        /// <summary>
        /// 使用对象池状态设置
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setPool(callType next)
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
            ClearMember<callType>.Cleaner(next);
#endif
            base.clear();
        }
        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int poolMaxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(callType));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static callType poolHead;
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
        internal static void PushNotNull(callType value)
        {
            if (poolCount >= poolMaxCount)
            {
                value.Dispose();
                return;
            }
            System.Threading.Interlocked.Increment(ref poolCount);
            callType headValue;
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
        private static void pushLink(callType value, callType end, int count)
        {
            System.Threading.Interlocked.Add(ref poolCount, count);
            callType headValue;
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
        private static void disposeLink(callType value)
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
            callType headValue = System.Threading.Interlocked.Exchange(ref poolHead, null);
            poolCount = 0;
            if (headValue != null)
            {
                if (count == 0) disposeLink(headValue);
                else
                {
                    int pushCount = count;
                    callType end = headValue;
                    while (--count != 0)
                    {
                        if (end.next == null)
                        {
                            pushLink(headValue, end, pushCount - count);
                            return;
                        }
                        end = end.next;
                    }
                    callType next = end.next;
                    end.next = null;
                    pushLink(headValue, end, pushCount);
                    if (next != null) disposeLink(next);
                }
            }
        }
        static Call()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
