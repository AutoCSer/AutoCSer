using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 验证时间戳
    /// </summary>
    public struct TimeVerifyTick
    {
        /// <summary>
        /// 最后一次验证时间
        /// </summary>
        private long lastVerifyTicks;
        /// <summary>
        /// 最后一次验证时间访问锁
        /// </summary>
        private int lastVerifyTickLock;
        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="ticks"></param>
        public TimeVerifyTick(long ticks)
        {
            lastVerifyTickLock = 0;
            lastVerifyTicks = ticks;
        }
        /// <summary>
        /// 检测当前时间戳
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="senderTimeVerifyTicks"></param>
        /// <returns></returns>
        public bool Check(ref long ticks, ref long senderTimeVerifyTicks)
        {
            if (ticks <= lastVerifyTicks && ticks != senderTimeVerifyTicks)
            {
                if (senderTimeVerifyTicks == 0)
                {
                    while (System.Threading.Interlocked.CompareExchange(ref lastVerifyTickLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimeVerifyServerSetTicks);
                    senderTimeVerifyTicks = ++lastVerifyTicks;
                    System.Threading.Interlocked.Exchange(ref lastVerifyTickLock, 0);
                }
                ticks = senderTimeVerifyTicks;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 设置最后一次验证时间
        /// </summary>
        /// <param name="ticks"></param>
        public void Set(long ticks)
        {
            if (ticks > lastVerifyTicks)
            {
                while (System.Threading.Interlocked.CompareExchange(ref lastVerifyTickLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimeVerifyServerSetTicks);
                if (ticks > lastVerifyTicks) lastVerifyTicks = ticks;
                System.Threading.Interlocked.Exchange(ref lastVerifyTickLock, 0);
            }
        }
    }
}
