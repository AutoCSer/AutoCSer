using System;
using System.Reflection;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 随机数
    /// </summary>
    public unsafe sealed class Random
    {
        /// <summary>
        /// 公用种子
        /// </summary>
        private uint* seeds;
        /// <summary>
        /// 安全种子
        /// </summary>
        private uint* secureSeeds;
        /// <summary>
        /// 32位种子位置
        /// </summary>
        private int current;
        /// <summary>
        /// 64位种子位置
        /// </summary>
        private int current64;
        /// <summary>
        /// 64位种子位置访问锁
        /// </summary>
        private int currentLock;
        /// <summary>
        /// 随机位缓存
        /// </summary>
        private uint bits;
        /// <summary>
        /// 随机位缓存数量
        /// </summary>
        private int bitCount;
        /// <summary>
        /// 字节缓存访问锁
        /// </summary>
        private int byteLock;
        /// <summary>
        /// 字节缓存
        /// </summary>
        private ulong bytes;
        /// <summary>
        /// 字节缓存数量
        /// </summary>
        private int byteCount;
        /// <summary>
        /// 双字节缓存访问锁
        /// </summary>
        private int ushortLock;
        /// <summary>
        /// 双字节缓存
        /// </summary>
        private ulong ushorts;
        /// <summary>
        /// 双字节缓存数量
        /// </summary>
        private int ushortCount;
        /// <summary>
        /// 随机数
        /// </summary>
        private Random()
        {
            secureSeeds = (uint*)Unmanaged.GetStatic(64 * sizeof(uint) + 5 * 11 * sizeof(uint), false);
            seeds = secureSeeds + 64;
            current64 = 5 * 11 - 2;
            ulong tick = (ulong)Pub.StartTime.Ticks ^ (ulong)Environment.TickCount ^ ((ulong)Pub.Identity32 << 8) ^ ((ulong)Date.NowTimerInterval << 24);
            int isSeedArray = 0;
            FieldInfo seedField = typeof(Random).GetField("SeedArray", BindingFlags.Instance | BindingFlags.NonPublic);
            if (seedField != null)
            {
                int[] seedArray = seedField.GetValue(new Random()) as int[];
                if (seedArray != null && seedArray.Length == 5 * 11 + 1)
                {
                    tick *= 0xb163dUL;
                    fixed (int* seedFixed = seedArray)
                    {
                        for (uint* write = seeds, end = seeds + 5 * 11, read = (uint*)seedFixed; write != end; tick >>= 1)
                        {
                            *write++ = *++read ^ (((uint)tick & 1U) << 31);
                        }
                    }
                    isSeedArray = 1;
                }
            }
            if (isSeedArray == 0)
            {
                for (uint* start = seeds, end = start + 5 * 11; start != end; ++start)
                {
                    *start = (uint)tick ^ (uint)(tick >> 32);
                    tick *= 0xb163dUL;
                    tick += tick >> 32;
                }
            }
            for (ulong* start = (ulong*)secureSeeds; start != seeds; *start++ = NextULong()) ;
            bits = (uint)Next();
            bitCount = 32;
        }
        /// <summary>
        /// 获取随机种子位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int nextIndex()
        {
            int index = Interlocked.Increment(ref current);
            if (index >= 5 * 11)
            {
                int cacheIndex = index;
                do
                {
                    index -= 5 * 11;
                }
                while (index >= 5 * 11);
                Interlocked.CompareExchange(ref current, index, cacheIndex);
            }
            return index;
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        public int Next()
        {
            int index = nextIndex();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7)) return (int)(*seed -= *(seed + 3 * 7));
            return (int)(*seed ^= *(seed - (5 * 11 - 3 * 7)));
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        public float NextFloat()
        {
            int index = nextIndex();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7)) *seed -= *(seed + 3 * 7);
            else *seed ^= *(seed - (5 * 11 - 3 * 7));
            return *(float*)seed;
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        /// <param name="mod">求余取模数</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int Next(int mod)
        {
            if (mod <= 1) return 0;
            int value = Next() % mod;
            return value >= 0 ? value : (value + mod);
        }
        /// <summary>
        /// 获取下一个随机位
        /// </summary>
        /// <returns></returns>
        public uint NextBit()
        {
            int count = Interlocked.Decrement(ref bitCount);
            while (count < 0)
            {
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextBit);
                count = Interlocked.Decrement(ref bitCount);
            }
            if (count == 0)
            {
                uint value = bits & 1;
                bits = (uint)Next();
                bitCount = 32;
                return value;
            }
            return bits & (1U << count);
        }
        /// <summary>
        /// 获取下一个随机字节
        /// </summary>
        /// <returns></returns>
        public byte NextByte()
        {
        START:
            while (System.Threading.Interlocked.CompareExchange(ref byteLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextByte);
            if (byteCount == 0)
            {
                byteCount = -1;
                System.Threading.Interlocked.Exchange(ref byteLock, 0);
                byte value = (byte)(bytes = NextULong());
                bytes >>= 8;
                while (System.Threading.Interlocked.CompareExchange(ref byteLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextByte);
                byteCount = 7;
                System.Threading.Interlocked.Exchange(ref byteLock, 0);
                return value;
            }
            else if (byteCount > 0)
            {
                byte value = (byte)bytes;
                --byteCount;
                bytes >>= 8;
                System.Threading.Interlocked.Exchange(ref byteLock, 0);
                return value;
            }
            else
            {
                System.Threading.Interlocked.Exchange(ref byteLock, 0);
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextByte);
                goto START;
            }
        }
        /// <summary>
        /// 获取下一个随机双字节
        /// </summary>
        /// <returns></returns>
        public ushort NextUShort()
        {
        START:
            while (System.Threading.Interlocked.CompareExchange(ref ushortLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextUShort);
            if (ushortCount == 0)
            {
                System.Threading.Interlocked.Exchange(ref ushortLock, 0);
                ushort value = (ushort)(ushorts = NextULong());
                ushorts >>= 16;
                while (System.Threading.Interlocked.CompareExchange(ref ushortLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextUShort);
                ushortCount = 3;
                System.Threading.Interlocked.Exchange(ref ushortLock, 0);
                return value;
            }
            else if (ushortCount > 0)
            {
                ushort value = (ushort)ushorts;
                --ushortCount;
                ushorts >>= 16;
                System.Threading.Interlocked.Exchange(ref ushortLock, 0);
                return value;
            }
            else
            {
                System.Threading.Interlocked.Exchange(ref ushortLock, 0);
                AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNextUShort);
                goto START;
            }
        }
        /// <summary>
        /// 获取随机种子位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int nextIndex64()
        {
            while (System.Threading.Interlocked.CompareExchange(ref currentLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.RandomNext64);
            int index = current64;
            if ((current64 -= 2) < 0) current64 = (5 * 11 - 4) - current64;
            System.Threading.Interlocked.Exchange(ref currentLock, 0);
            return index;
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        public ulong NextULong()
        {
            int index = nextIndex64();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7 - 1)) return *(ulong*)seed -= *(ulong*)(seed + 3 * 7);
            if (index == (5 * 11 - 3 * 7 - 1)) return *(ulong*)seed -= *(ulong*)seeds;
            return *(ulong*)seed ^= *(ulong*)(seed - (5 * 11 - 3 * 7));
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        public double NextDouble()
        {
            int index = nextIndex64();
            uint* seed = seeds + index;
            if (index < (5 * 11 - 3 * 7 - 1)) *(ulong*)seed -= *(ulong*)(seed + 3 * 7);
            else if (index == (5 * 11 - 3 * 7 - 1)) *(ulong*)seed -= *(ulong*)seeds;
            else *(ulong*)seed ^= *(ulong*)(seed - (5 * 11 - 3 * 7));
            return *(double*)seed;
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        public int SecureNext()
        {
            int seed = Next(), leftIndex = seed & 63, rightIndex = (seed >> 6) & 63;
            if (leftIndex == rightIndex) return (int)((secureSeeds[leftIndex] ^= (uint)seed) - (uint)seed);
            if ((seed & (1 << ((seed >> 12) & 31))) == 0) return (int)((secureSeeds[leftIndex] -= secureSeeds[rightIndex]) ^ (uint)seed);
            return (int)((secureSeeds[leftIndex] ^= secureSeeds[rightIndex]) - (uint)seed);
        }
        /// <summary>
        /// 获取下一个非0随机数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public uint SecureNextUIntNotZero()
        {
            uint value = (uint)SecureNext();
            while (value == 0) value = (uint)SecureNext();
            return value;
        }
        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        public ulong SecureNextULong()
        {
            ulong seed = NextULong();
            int leftIndex = (int)(uint)seed & 63, rightIndex = (int)((uint)seed >> 6) & 63;
            if (leftIndex == 63) leftIndex = 62;
            if (rightIndex == 63) rightIndex = 62;
            if (leftIndex == rightIndex) return (*(ulong*)(secureSeeds + leftIndex) ^= seed) - seed;
            if (((uint)seed & (1U << ((int)((uint)seed >> 12) & 31))) == 0) return (*(ulong*)(secureSeeds + leftIndex) -= *(ulong*)(secureSeeds + rightIndex)) ^ seed;
            return (*(ulong*)(secureSeeds + leftIndex) ^= *(ulong*)(secureSeeds + rightIndex)) - seed;
        }
        /// <summary>
        /// 获取下一个非0随机数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ulong SecureNextULongNotZero()
        {
            ulong value = SecureNextULong();
            while (value == 0) value = SecureNextULong();
            return value;
        }
        /// <summary>
        /// 默认随机数
        /// </summary>
        public static Random Default = new Random();
        /// <summary>
        /// 随机Hash值(用于防构造)
        /// </summary>
        public static readonly int Hash = Default.Next();
    }
}
