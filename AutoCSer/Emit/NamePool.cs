using System;
using System.Threading;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 名称申请池
    /// </summary>
    internal unsafe static class NamePool
    {
        /// <summary>
        /// 申请池起始位置
        /// </summary>
        private static char* start;
        /// <summary>
        /// 申请池结束未知
        /// </summary>
        private static char* end;
        /// <summary>
        /// 申请池创建访问锁
        /// </summary>
        private static readonly object createLock = new object();
        /// <summary>
        /// 申请池获取访问锁
        /// </summary>
        private static int getLock;
        /// <summary>
        /// 申请池大小
        /// </summary>
        private const int poolSize = 4 << 10;
        /// <summary>
        /// 获取名称空间
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static char* GetChar(int length)
        {
            while (System.Threading.Interlocked.CompareExchange(ref getLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.NamePoolGet);
            char* value = start;
            if ((start += length) <= end)
            {
                System.Threading.Interlocked.Exchange(ref getLock, 0);
                return value;
            }
            System.Threading.Interlocked.Exchange(ref getLock, 0);
            Monitor.Enter(createLock);
            while (System.Threading.Interlocked.CompareExchange(ref getLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.NamePoolGet);
            if ((start += length) <= end)
            {
                value = start - length;
                System.Threading.Interlocked.Exchange(ref getLock, 0);
                Monitor.Exit(createLock);
                return value;
            }
            System.Threading.Interlocked.Exchange(ref getLock, 0);
            try
            {
                value = (char*)Unmanaged.GetStatic64(poolSize, false);
                while (System.Threading.Interlocked.CompareExchange(ref getLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.NamePoolGet);
                start = value + length;
                end = value + (poolSize >> 1);
                System.Threading.Interlocked.Exchange(ref getLock, 0);
            }
            finally { Monitor.Exit(createLock); }
            return value;
        }
        /// <summary>
        /// 获取名称空间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="seek">前缀字符长度</param>
        /// <param name="endSize">后缀字符长度</param>
        /// <returns></returns>
        public static char* Get(string name, int seek, int endSize)
        {
            char* value = GetChar(name.Length + (seek + endSize));
            fixed (char* nameFixed = name) AutoCSer.Extension.StringExtension.SimpleCopyNotNull(nameFixed, value + seek, name.Length);
            return value;
        }
    }
}
