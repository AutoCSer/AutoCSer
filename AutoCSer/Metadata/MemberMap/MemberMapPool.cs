using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图内存池
    /// </summary>
    internal sealed unsafe class MemberMapPool
    {
        /// <summary>
        /// 空闲内存地址
        /// </summary>
        private byte* free;
        /// <summary>
        /// 空闲内存地址访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock freeLock;
        /// <summary>
        /// 成员位图字节数量
        /// </summary>
        private readonly int size;
        /// <summary>
        /// 填充整数数量
        /// </summary>
        private readonly int clearCount;
        /// <summary>
        /// 成员位图内存池
        /// </summary>
        /// <param name="size">成员位图字节数量</param>
        private MemberMapPool(int size)
        {
            this.size = size;
            clearCount = size >> 3;
        }
        /// <summary>
        /// 获取成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        public byte* Get()
        {
            byte* value;
            freeLock.EnterYield();
            if (free != null)
            {
                value = free;
                free = *(byte**)free;
                freeLock.Exit();
                return value;
            }
            freeLock.Exit();

            memoryLock.Enter();
            value = memoryStart;
            if ((memoryStart += size) <= memoryEnd)
            {
                memoryLock.Exit();
                return value;
            }
            memoryLock.SleepFlag = 1;
            try
            {
                value = (byte*)AutoCSer.Memory.Unmanaged.GetStatic(memorySize, false);
                memoryStart = value + size;
                memoryEnd = value + memorySize;
            }
            finally
            {
                ++memoryCount;
                memoryLock.ExitSleepFlag();
            }
            return value;
        }
        /// <summary>
        /// 获取成员位图
        /// </summary>
        /// <returns>成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte* GetClear()
        {
            byte* value = Get();
            AutoCSer.Memory.Common.Clear((ulong*)value, clearCount);
            return value;
        }
        /// <summary>
        /// 成员位图入池
        /// </summary>
        /// <param name="map">成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Push(ref byte* map)
        {
            if (map != null)
            {
                freeLock.EnterYield();
                *(byte**)map = free;
                free = map;
                freeLock.Exit();
                map = null;
            }
        }

        /// <summary>
        /// 成员位图内存池集合
        /// </summary>
        private static readonly MemberMapPool[] pools = new MemberMapPool[(1 << 10) >> 6];
        /// <summary>
        /// 成员位图内存池集合访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock poolLock;
        /// <summary>
        /// 获取成员位图内存池
        /// </summary>
        /// <param name="size">成员位图字节数量</param>
        /// <returns></returns>
        internal static MemberMapPool GetPool(int size)
        {
            if (size > 0)
            {
                int index = size >> 3;
                if (index < pools.Length)
                {
                    MemberMapPool pool = pools[index];
                    if (pool != null) return pool;
                    poolLock.EnterSleep();
                    if ((pool = pools[index]) == null)
                    {
                        try
                        {
                            pools[index] = pool = new MemberMapPool(size);
                        }
                        finally { poolLock.Exit(); }
                    }
                    else poolLock.Exit();
                    return pool;
                }
            }
            return null;
        }

        /// <summary>
        /// 成员位图内存池字节大小
        /// </summary>
        private const int memorySize = 8 << 10;
        /// <summary>
        /// 成员位图内存池结束位置
        /// </summary>
        private static byte* memoryEnd;
        /// <summary>
        /// 内存申请数量
        /// </summary>
        private static int memoryCount;
        /// <summary>
        /// 成员位图内存池起始位置
        /// </summary>
        private static byte* memoryStart;
        /// <summary>
        /// 成员位图内存池访问锁
        /// </summary>
        private static AutoCSer.Threading.SleepFlagSpinLock memoryLock;
    }
}
