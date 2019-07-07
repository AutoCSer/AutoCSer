using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 成员位图绑定对象链表池
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal static class MemberMapValueLinkPool<valueType>
        where valueType : class, IMemberMapValueLink<valueType>
    {
        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int maxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(valueType));
        /// <summary>
        /// 是否需要释放资源
        /// </summary>
        private readonly static bool isDisponse = typeof(IDisposable).IsAssignableFrom(typeof(valueType));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static valueType head;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private static int popLock;
        /// <summary>
        /// 缓存数量
        /// </summary>
        private static int count;
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PushNotNull(valueType value)
        {
            if (count >= maxCount)
            {
                if (isDisponse) ((IDisposable)value).Dispose();
                return;
            }
            System.Threading.Interlocked.Increment(ref count);
            valueType headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    value.MemberMapValueLink = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                }
                else
                {
                    value.MemberMapValueLink = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
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
        internal static valueType Pop()
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
                if (System.Threading.Interlocked.CompareExchange(ref head, headValue.MemberMapValueLink, headValue) == headValue)
                {
                    System.Threading.Interlocked.Exchange(ref popLock, 0); 
                    System.Threading.Interlocked.Decrement(ref count);
                    headValue.MemberMapValueLink = null;
                    return headValue;
                }
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
            }
            while (true);
        }
        /// <summary>
        /// 添加链表
        /// </summary>
        /// <param name="value">链表头部</param>
        /// <param name="end">链表尾部</param>
        /// <param name="pushCount">数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void pushLink(valueType value, valueType end, int pushCount)
        {
            System.Threading.Interlocked.Add(ref count, pushCount);
            valueType headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    end.MemberMapValueLink = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                }
                else
                {
                    end.MemberMapValueLink = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
            }
            while (true);
        }
        /// <summary>
        /// 释放列表
        /// </summary>
        /// <param name="value"></param>
        private static void disposeLink(valueType value)
        {
            do
            {
                ((IDisposable)value).Dispose();
            }
            while ((value = value.MemberMapValueLink) != null);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="saveCount">保留缓存数据数量</param>
        private static void clearCache(int saveCount)
        {
            valueType headValue = System.Threading.Interlocked.Exchange(ref head, null);
            count = 0;
            if (headValue != null)
            {
                if (saveCount == 0)
                {
                    if (isDisponse) disposeLink(headValue);
                }
                else
                {
                    int pushCount = saveCount;
                    valueType end = headValue;
                    while (--saveCount != 0)
                    {
                        if (end.MemberMapValueLink == null)
                        {
                            pushLink(headValue, end, pushCount - saveCount);
                            return;
                        }
                        end = end.MemberMapValueLink;
                    }
                    valueType next = end.MemberMapValueLink;
                    end.MemberMapValueLink = null;
                    pushLink(headValue, end, pushCount);
                    if (isDisponse && next != null) disposeLink(next);
                }
            }
        }
        static MemberMapValueLinkPool()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
