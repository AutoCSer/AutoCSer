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
        private AutoCSer.Threading.SpinLock lastVerifyTickLock;
        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="ticks"></param>
        public TimeVerifyTick(long ticks)
        {
            lastVerifyTickLock = default(AutoCSer.Threading.SpinLock);
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
                    lastVerifyTickLock.EnterYield();
                    senderTimeVerifyTicks = ++lastVerifyTicks;
                    lastVerifyTickLock.Exit();
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
                lastVerifyTickLock.EnterYield();
                if (ticks > lastVerifyTicks) lastVerifyTicks = ticks;
                lastVerifyTickLock.Exit();
            }
        }
    }
}
