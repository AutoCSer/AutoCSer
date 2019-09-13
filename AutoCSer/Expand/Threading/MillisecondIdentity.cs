using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 毫秒ID生成器
    /// </summary>
    public abstract class MillisecondIdentityBase
    {
        /// <summary>
        /// 开始计数时间
        /// </summary>
        protected static readonly DateTime startTime = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 初始化时间戳
        /// </summary>
        protected static readonly long startTimestamp = Date.GetTimestampByTicks(AutoCSer.Date.StartTime.Ticks - startTime.Ticks);

        /// <summary>
        /// 毫秒内计数掩码
        /// </summary>
        protected readonly long mask;
        /// <summary>
        /// 当前最大时间戳
        /// </summary>
        protected long maxTimestamp;
        /// <summary>
        /// 当前ID
        /// </summary>
        protected long currentIdentity;
        /// <summary>
        /// ID生成访问锁
        /// </summary>
        protected int identityLock;
        /// <summary>
        /// 允许连续时间戳数量
        /// </summary>
        protected ushort timestampCount;
        /// <summary>
        /// 毫秒内计数 2 进制位数 + 分布式编号 2 进制位数
        /// </summary>
        protected readonly byte bits;
        /// <summary>
        /// 分布式编号 2 进制位数
        /// </summary>
        private readonly byte distributedBits;
        /// <summary>
        /// 毫秒ID生成器
        /// </summary>
        /// <param name="mask">毫秒内计数掩码</param>
        /// <param name="bits">毫秒内计数 2 进制位数 + 分布式编号 2 进制位数</param>
        /// <param name="distributedBits">分布式编号 2 进制位数</param>
        protected MillisecondIdentityBase(long mask, byte bits, byte distributedBits)
        {
            this.mask = mask;
            this.bits = bits;
            this.distributedBits = distributedBits;
        }
        /// <summary>
        /// 根据 ID 获取时间
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DateTime GetTime(long identity)
        {
            return startTime.AddTicks((long)(identity >> bits) * TimeSpan.TicksPerMillisecond);
        }
        /// <summary>
        /// 根据 ID 获取时间
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DateTime GetLocalTime(long identity)
        {
            return startTime.AddTicks((long)(identity >> bits) * TimeSpan.TicksPerMillisecond).ToLocalTime();
        }
    }
    /// <summary>
    /// 毫秒ID生成器（毫秒内超出计算范围时自动移动到下一个毫秒数据）
    /// </summary>
    public sealed class MillisecondIdentity : MillisecondIdentityBase
    {
        /// <summary>
        /// 毫秒ID生成器
        /// </summary>
        /// <param name="bits">毫秒内计数 2 进制位数，默认为 20 表示支持持续 278 年每秒 10亿 个 ID</param>
        public MillisecondIdentity(byte bits = 20) : base((1L << bits) - 1, bits, 0) { }
        /// <summary>
        /// 获取下一个ID
        /// </summary>
        /// <returns></returns>
        public long GetNext()
        {
            while (System.Threading.Interlocked.CompareExchange(ref identityLock, 1, 0) != 0) ThreadYield.YieldOnly();
            long timestamp = startTimestamp + Date.TimestampDifference;
            if (timestamp < maxTimestamp)
            {
                long identity = ++currentIdentity;
                if ((identity & mask) != 0)
                {
                    System.Threading.Interlocked.Exchange(ref identityLock, 0);
                    return identity;
                }
                if (--timestampCount == 0)
                {
                    maxTimestamp += Date.MillisecondTimestampDifferencePerSecond;
                    timestampCount = 1000;
                }
                maxTimestamp += Date.TimestampPerMillisecond;
                System.Threading.Interlocked.Exchange(ref identityLock, 0);
                return identity;
            }
            else if (timestamp == maxTimestamp)
            {
                long identity = ((currentIdentity >> bits) + 1) << bits;
                if (--timestampCount == 0)
                {
                    maxTimestamp += Date.MillisecondTimestampDifferencePerSecond;
                    timestampCount = 1000;
                }
                currentIdentity = identity;
                maxTimestamp += Date.TimestampPerMillisecond;
                System.Threading.Interlocked.Exchange(ref identityLock, 0);
                return identity;
            }
            else
            {
                long identity = Date.GetMillisecondsByTimestamp(timestamp), lastIdentity = currentIdentity >> bits;
                while (identity <= lastIdentity) ++identity;
                timestampCount = 1000;
                maxTimestamp = Date.GetTimestampByMilliseconds(identity + 1);
                currentIdentity = (identity <<= bits);
                System.Threading.Interlocked.Exchange(ref identityLock, 0);
                return identity;
            }
        }
    }
}
