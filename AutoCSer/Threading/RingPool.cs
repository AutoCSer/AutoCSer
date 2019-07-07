using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 环池
    /// </summary>
    internal static class RingPool
    {
        /// <summary>
        /// 数组前后填充数量
        /// </summary>
        internal static readonly unsafe int PadCount = 64 / sizeof(IntPtr);
    }
    /// <summary>
    /// 环池数据类型
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal sealed class RingPool<valueType>
        where valueType : class
    {
        /// <summary>
        /// 数组元素
        /// </summary>
        private struct ArrayValue
        {
            /// <summary>
            /// 数组元素
            /// </summary>
            internal valueType Value;
            /// <summary>
            /// 弹出数组元素
            /// </summary>
            /// <returns>数组元素</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal valueType Pop()
            {
                valueType value = Value;
                Value = null;
                return value;
            }
        }
        private readonly ulong pad0, pad1, pad2, pad3, pad4, pad5, pad6;
        /// <summary>
        /// 环池数组
        /// </summary>
        private readonly ArrayValue[] ring;
        /// <summary>
        /// 环大小
        /// </summary>
        private readonly int count;
        /// <summary>
        /// 环索引值
        /// </summary>
        private readonly uint countLess;
        private readonly ulong pad10, pad11, pad12, pad13, pad14, pad15, pad16;
        /// <summary>
        /// 预写位置
        /// </summary>
        private volatile int writeIndex;
        /// <summary>
        /// 可写结束位置
        /// </summary>
        private volatile int writeEndIndex;
        /// <summary>
        /// 可写结束位置访问锁
        /// </summary>
        private int writeEndLock = 1;
        /// <summary>
        /// 已写入位置
        /// </summary>
        private volatile int writedIndex;
        private readonly ulong pad20, pad21, pad22, pad23, pad24, pad25, pad26;
        /// <summary>
        /// 预读位置
        /// </summary>
        private volatile int readIndex;
        /// <summary>
        /// 可读结束位置
        /// </summary>
        private volatile int readEndIndex;
        /// <summary>
        /// 可读结束位置访问锁
        /// </summary>
        private int readEndLock;
        /// <summary>
        /// 已读取位置
        /// </summary>
        private volatile int readedIndex;
        private readonly ulong pad30, pad31, pad32, pad33, pad34, pad35, pad36;
        /// <summary>
        /// 环池
        /// </summary>
        private RingPool()
        {
            count = AutoCSer.Config.Pub.Default.GetYieldPoolCount(typeof(valueType));
            if (count <= 0) count = AutoCSer.Config.Pub.DefaultPoolCount;
            else
            {
                count = (int)((uint)count).UpToPower2();
                if ((uint)count > 0x40000000) count = 0x40000000;
            }
            countLess = (uint)(count - 1);
            ring = new ArrayValue[count + (RingPool.PadCount << 1)];
            writeEndIndex = count;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        internal void PushNotNull(valueType value)
        {
            START:
            int index = writeIndex;
            if (index != writeEndIndex)
            {
                int nextIndex = (int)((uint)index + 1);
                if (System.Threading.Interlocked.CompareExchange(ref writeIndex, nextIndex, index) == index)
                {
                    ring[(int)((uint)index & countLess) + RingPool.PadCount].Value = value;
                    //if (nextIndex == writeEndIndex) AutoCSer.Threading.Interlocked.writeEndLock = 0;
                    if (nextIndex == writeEndIndex) System.Threading.Interlocked.Exchange(ref writeEndLock, 0);
                    while (writedIndex != index) ThreadYield.YieldOnly();
                    writedIndex = nextIndex;
                    return;
                }
                ThreadYield.YieldOnly();
                goto START;
            }
            if (System.Threading.Interlocked.CompareExchange(ref writeEndLock, 1, 0) == 0)
            {
                int newWriteEndIndex = readedIndex + count;
                if (newWriteEndIndex != writeEndIndex)
                {
                    writeEndIndex = newWriteEndIndex;
                    goto START;
                }
                //AutoCSer.Threading.Interlocked.writeEndLock = 0;
                System.Threading.Interlocked.Exchange(ref writeEndLock, 0);
            }
        }
        /// <summary>
        /// 弹出数据
        /// </summary>
        /// <returns></returns>
        internal valueType Pop()
        {
            int tryCount = 2;
            START:
            int index = readIndex;
            if (index != readEndIndex)
            {
                int nextIndex = (int)((uint)index + 1);
                if (System.Threading.Interlocked.CompareExchange(ref readIndex, nextIndex, index) == index)
                {
                    valueType value = ring[(int)((uint)index & countLess) + RingPool.PadCount].Pop();
                    //if (nextIndex == readEndIndex) AutoCSer.Threading.Interlocked.readEndLock = 0;
                    if (nextIndex == readEndIndex) System.Threading.Interlocked.Exchange(ref readEndLock, 0);
                    if (readedIndex != index) ThreadYield.YieldOnly();
                    readedIndex = nextIndex;
                    return value;
                }
                ThreadYield.YieldOnly();
                goto START;
            }
            if (System.Threading.Interlocked.CompareExchange(ref readEndLock, 1, 0) == 0)
            {
                int newReadEndIndex = writedIndex;
                if (newReadEndIndex == readEndIndex)
                {
                    ThreadYield.YieldOnly();
                    newReadEndIndex = writedIndex;
                }
                if (newReadEndIndex != readEndIndex)
                {
                    readEndIndex = newReadEndIndex;
                    tryCount = 0;
                    goto START;
                }
                System.Threading.Interlocked.Exchange(ref readEndLock, 0);
                //AutoCSer.Threading.Interlocked.readEndLock = 0;
            }
            else if (tryCount != 0)
            {
                ThreadYield.YieldOnly();
                --tryCount;
                goto START;
            }
            return null;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private void clearCache(int count)
        {
            if (count < 0) count = 0;
            while (writedIndex - readedIndex > count) Pop();
        }

        /// <summary>
        /// 默认环池
        /// </summary>
        internal static readonly RingPool<valueType> Default = new RingPool<valueType>();
        static RingPool()
        {
            if (typeof(IDisposable).IsAssignableFrom(typeof(valueType))) AutoCSer.Log.Pub.Log.Add(Log.LogType.Fatal, "环池不支持资源对象类型 " + typeof(valueType).fullName());
            Default = new RingPool<valueType>();
            AutoCSer.Pub.ClearCaches += Default.clearCache;
        }
    }
}
