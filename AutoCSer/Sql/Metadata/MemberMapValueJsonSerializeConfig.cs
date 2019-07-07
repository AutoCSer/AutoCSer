using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// JSON 序列化配置参数
    /// </summary>
    internal sealed class MemberMapValueJsonSerializeConfig : AutoCSer.Json.SerializeConfig
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal MemberMapValueJsonSerializeConfig LinkNext;

        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int maxCount = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(MemberMapValueJsonSerializeConfig));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static MemberMapValueJsonSerializeConfig head;
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
        internal static void PushNotNull(MemberMapValueJsonSerializeConfig value)
        {
            if (count >= maxCount) return;
            System.Threading.Interlocked.Increment(ref count);
            MemberMapValueJsonSerializeConfig headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    value.LinkNext = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                }
                else
                {
                    value.LinkNext = headValue;
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
        internal static MemberMapValueJsonSerializeConfig Pop()
        {
            while (System.Threading.Interlocked.CompareExchange(ref popLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPop);
            MemberMapValueJsonSerializeConfig headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    System.Threading.Interlocked.Exchange(ref popLock, 0);
                    return null;
                }
                if (System.Threading.Interlocked.CompareExchange(ref head, headValue.LinkNext, headValue) == headValue)
                {
                    System.Threading.Interlocked.Exchange(ref popLock, 0);
                    System.Threading.Interlocked.Decrement(ref count);
                    headValue.LinkNext = null;
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
        private static void pushLink(MemberMapValueJsonSerializeConfig value, MemberMapValueJsonSerializeConfig end, int pushCount)
        {
            System.Threading.Interlocked.Add(ref count, pushCount);
            MemberMapValueJsonSerializeConfig headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    end.LinkNext = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
                }
                else
                {
                    end.LinkNext = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkPush);
            }
            while (true);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="saveCount">保留缓存数据数量</param>
        private static void clearCache(int saveCount)
        {
            MemberMapValueJsonSerializeConfig headValue = System.Threading.Interlocked.Exchange(ref head, null);
            count = 0;
            if (headValue != null && saveCount != 0)
            {
                int pushCount = saveCount;
                MemberMapValueJsonSerializeConfig end = headValue;
                while (--saveCount != 0)
                {
                    if (end.LinkNext == null)
                    {
                        pushLink(headValue, end, pushCount - saveCount);
                        return;
                    }
                    end = end.LinkNext;
                }
                end.LinkNext = null;
                pushLink(headValue, end, pushCount);
            }
        }
        static MemberMapValueJsonSerializeConfig()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
