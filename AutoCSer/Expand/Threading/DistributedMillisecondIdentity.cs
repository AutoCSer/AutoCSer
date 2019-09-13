using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 分布式毫秒ID生成器（毫秒内超出计算范围时自动移动到下一个毫秒数据）
    /// </summary>
    public sealed class DistributedMillisecondIdentity : MillisecondIdentityBase
    {
        /// <summary>
        /// 分布式编号
        /// </summary>
        private readonly long distributed;
        /// <summary>
        /// ID 增量
        /// </summary>
        private readonly long identityIncrement;
        /// <summary>
        /// 分布式毫秒ID生成器
        /// </summary>
        /// <param name="bits">毫秒内计数 2 进制位数 + 分布式编号 2 进制位数</param>
        /// <param name="distributedBits">分布式编号 2 进制位数</param>
        /// <param name="identityIncrement">ID 增量</param>
        private DistributedMillisecondIdentity(byte bits, byte distributedBits, long identityIncrement) : base((1L << bits) - identityIncrement, bits, distributedBits)
        {
            this.identityIncrement = identityIncrement;
        }
        /// <summary>
        /// 分布式毫秒ID生成器
        /// </summary>
        /// <param name="distributed">分布式编号</param>
        /// <param name="distributedBits">分布式编号 2 进制位数，默认为 10 表示支持 1024 台服务器</param>
        /// <param name="identityBits">毫秒内计数 2 进制位数，默认为 10 表示支持每毫秒 1024 个 ID</param>
        public DistributedMillisecondIdentity(ushort distributed = 0, byte distributedBits = 10, byte identityBits = 10)
            : this((byte)(distributedBits + identityBits), distributedBits, 1L << distributedBits)
        {
            currentIdentity |= (this.distributed = distributed);
        }
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
                long identity = (currentIdentity += identityIncrement);
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
                long identity = (((currentIdentity >> bits) + 1) << bits) | distributed;
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
                currentIdentity = (identity = (identity << bits) | distributed);
                System.Threading.Interlocked.Exchange(ref identityLock, 0);
                return identity;
            }
        }
    }
}
