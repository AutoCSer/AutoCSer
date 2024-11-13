using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    public static unsafe partial class Common
    {
        /// <summary>
        /// 填充整数
        /// </summary>
        /// <param name="src">串起始地址，不能为 null</param>
        /// <param name="count">整数数量，必须大于 0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Clear(ulong* src, int count)
        {
#if DEBUG
            if (src == null) throw new Exception("src == null");
            if (count <= 0) throw new Exception(count.toString() + " <= 0");
#endif
            ulong* end = src + count;
            do
            {
                *src = 0;
            }
            while (++src != end);
        }
        /// <summary>
        /// 填充整数(用Buffer.BlockCopy可能比指针快)
        /// </summary>
        /// <param name="src">串起始地址，不能为 null</param>
        /// <param name="count">字节数量，必须大于 0</param>
        internal unsafe static void Clear(byte* src, int count)
        {
#if DEBUG
            if (src == null) throw new Exception("src == null");
            if (count <= 0) throw new Exception(count.toString() + " <= 0");
#endif
            int end = count & (sizeof(ulong) - 1);
            if (end == 0) Clear((ulong*)src, count >> 3);
            else
            {
                if ((count >>= 3) != 0)
                {
                    Clear((ulong*)src, count);
                    src += count << 3;
                }
                if ((end & 4) != 0)
                {
                    *((uint*)src) = 0;
                    src += 4;
                }
                if ((end & 2) != 0)
                {
                    *((short*)src) = 0;
                    src += 2;
                }
                if ((end & 1) != 0) *src = 0;
            }
        }
        /// <summary>
        /// 填充整数(用Buffer.BlockCopy可能比指针快)
        /// </summary>
        /// <param name="src">串起始地址,不能为null</param>
        /// <param name="value">填充整数</param>
        /// <param name="count">整数数量,大于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Fill(ulong* src, ulong value, int count)
        {
#if DEBUG
            if (src == null) throw new Exception("src == null");
            if (count <= 0) throw new Exception(count.toString() + " <= 0");
#endif
            ulong* end = src + count;
            do
            {
                *src = value;
            }
            while (++src != end);
        }
        /// <summary>
        /// 计算 32 位 HASH 值
        /// </summary>
        /// <param name="data">数据起始位置，不允许为 null</param>
        /// <param name="length">数据长度</param>
        /// <returns>32 位 HASH 值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int GetHashCode(void* data, int length)
        {
            ulong value = GetHashCode64((byte*)data, length);
            return (int)((uint)value ^ (uint)(value >> 32));
        }
        /// <summary>
        /// 计算 64 位 HASH 值
        /// </summary>
        /// <param name="start">数据起始位置</param>
        /// <param name="length">数据长度</param>
        /// <returns>64 位 HASH 值</returns>
        internal static ulong GetHashCode64(byte* start, int length)
        {
#if DEBUG
            if (start == null) throw new Exception("start == null");
            if (length < 0) throw new Exception(length.toString() + " < 0");
#endif
            //一般编码都以字节为基本单位,也就是说基本单位长度为 8bit;
            //常用编码可能比较集中,造成编码中出现伪固定位(大多时候某些固定位都是同一值)
            //采用移位的方式:当移位量为 1 或 7 时,一般只能覆盖掉 1 个固定位;当移位量为 3 或 5 时,一般能覆盖掉 3 个固定位;所以本程序使用的移位量为 8x+5/3
            //由于64 = 5+59 = 13+3*17 = 3*7+43 = 29+5*7 = 37+3*3*3 = 5*9+19 = 53+11 = 61+3,其中(5+59),(53+11),(61+3)为素数对成为最佳移位量,本程序选择中性素数对 53+11
            //由于32 = 5+3*3*3 = 13+19 = 3*7+11 = 29+3,其中(13+19),(29+3)为素数对成为最佳移位量,本程序选择中性素数对 13+19
            if (length >= sizeof(ulong))
            {
                ulong value = *(ulong*)start;
                for (byte* end = start + (length & (int.MaxValue - sizeof(ulong) + 1)); (start += sizeof(ulong)) != end; value ^= *(ulong*)start)
                {
                    value = (value << 53) | (value >> 11);
                }
                if ((length & (sizeof(ulong) - 1)) != 0)
                {
                    value = (value << 53) | (value >> 11);
                    value ^= *(ulong*)start << ((sizeof(ulong) - (length & (sizeof(ulong) - 1))) << 3);
                }
                return value ^ ((ulong)length << 19);
            }
            return (*(ulong*)start << ((sizeof(ulong) - length) << 3)) ^ ((ulong)length << 19);
        }

        /// <summary>
        /// 字节数组比较，可以修改为 ReadOnlySpan.SequenceEqual
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        internal static bool EqualNotNull(void* left, void* right, int count)
        {
            int shift = (int)left & (sizeof(ulong) - 1);
            if (count > shift)
            {
                return (shift == 0 || (*((ulong*)left) ^ *((ulong*)right)) << (64 - (shift << 3)) == 0)
                    && equal((byte*)left + shift, (byte*)right + shift, count - shift);
            }
            return SimpleEqualNotNull((byte*)left, (byte*)right, count);
        }
        /// <summary>
        /// 字节数组比较，可以修改为 ReadOnlySpan.SequenceEqual
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数</param>
        /// <returns>是否相等</returns>
        private static bool equal(byte* left, byte* right, int count)
        {
#if DEBUG
            if (left == null) throw new Exception("left == null");
            if (right == null) throw new Exception("right == null");
            if (count < 0) throw new Exception(count.toString() + " < 0");
#endif
            while (count >= sizeof(ulong) * 4)
            {
                if (((*((ulong*)left) ^ *((ulong*)right)) |
                    (*((ulong*)(left + sizeof(ulong))) ^ *((ulong*)(right + sizeof(ulong)))) |
                    (*((ulong*)(left + sizeof(ulong) * 2)) ^ *((ulong*)(right + sizeof(ulong) * 2))) |
                    (*((ulong*)(left + sizeof(ulong) * 3)) ^ *((ulong*)(right + sizeof(ulong) * 3)))) != 0) return false;
                left += sizeof(ulong) * 4;
                right += sizeof(ulong) * 4;
                count -= sizeof(ulong) * 4;
            }
            if (count < sizeof(ulong) * 4)
            {
                ulong isEqual = 0;
                if ((count & (sizeof(ulong) * 2)) != 0)
                {
                    isEqual |= (*((ulong*)left) ^ *((ulong*)right))
                        | (*((ulong*)(left + sizeof(ulong))) ^ *((ulong*)(right + sizeof(ulong))));
                    left += sizeof(ulong) * 2;
                    right += sizeof(ulong) * 2;
                }
                if ((count & sizeof(ulong)) != 0)
                {
                    isEqual |= *((ulong*)left) ^ *((ulong*)right);
                    left += sizeof(ulong);
                    right += sizeof(ulong);
                }
                if ((count &= (sizeof(ulong) - 1)) != 0)
                {
                    count <<= 3;
                    isEqual |= (*((ulong*)left) ^ *((ulong*)right)) << (64 - count);
                }
                return isEqual == 0;
            }
            return true;
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe bool SimpleEqualNotNull(byte[] left, byte[] right, int count)
        {
#if DEBUG
            if (count > left.Length) throw new Exception(count.toString() + " > " + left.Length.toString());
            if (count > right.Length) throw new Exception(count.toString() + " > " + right.Length.toString());
#endif
            fixed (byte* leftFixed = left, rightFixed = right) return SimpleEqualNotNull(leftFixed, rightFixed, count);
        }
        /// <summary>
        /// 字节数组比较，可以修改为 ReadOnlySpan.SequenceEqual
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        internal static bool SimpleEqualNotNull(byte* left, byte* right, int count)
        {
#if DEBUG
            if (left == null) throw new Exception("left == null");
            if (right == null) throw new Exception("right == null");
            if (count < 0) throw new Exception(count.toString() + " < 0");
#endif
            for (byte* end = left + (count & (int.MaxValue - (sizeof(ulong) - 1))); left != end; left += sizeof(ulong), right += sizeof(ulong))
            {
                if (*(ulong*)left != *(ulong*)right) return false;
            }
            return (count &= (sizeof(ulong) - 1)) == 0 || (*((ulong*)left) ^ *((ulong*)right)) << (64 - (count << 3)) == 0;
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否相等</returns>
        public static bool equal(this SubArray<byte> left, ref SubArray<byte> right)
        {
            switch (left.ReferenceEqual(ref right))
            {
                case NullableBool.False: return false;
                case NullableBool.True: return true;
                default:
                    fixed (byte* leftFixed = left.GetFixedBuffer(), rightFixed = right.GetFixedBuffer())
                    {
                        return EqualNotNull(leftFixed + left.Start, rightFixed + right.Start, left.Length);
                    }
            }
        }
        /// <summary>
        /// 字节数组转换成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count">必须大于 0</param>
        /// <returns></returns>
        internal static string ToString(byte* data, int count)
        {
#if DEBUG
            if (data == null) throw new Exception("data == null");
            if (count < 0) throw new Exception(count.toString() + " < 0");
#endif
            string value = AutoCSer.Extensions.StringExtension.FastAllocateString(count);
            byte* end = data + (count & (int.MaxValue - 3));
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                while (data != end)
                {
                    *write = (char)*data;
                    *(write + 1) = (char)*(data + 1);
                    *(write + 2) = (char)*(data + 2);
                    *(write + 3) = (char)*(data + 3);
                    data += 4;
                    write += 4;
                }
                if ((count & 2) != 0)
                {
                    *write = (char)*data;
                    *(write + 1) = (char)*(data + 1);
                    data += 2;
                    write += 2;
                }
                if ((count & 1) != 0) *write = (char)*data;
            }
            return value;
        }

        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原字节起始地址,不能为null</param>
        /// <param name="destination">目标串数组,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void CopyNotNull(void* source, byte[] destination, int length)
        {
#if DEBUG
            if (length > destination.Length) throw new Exception(length.toString() + " > " + destination.Length.toString());
#endif
            fixed (byte* data = destination) CopyNotNull(source, (void*)data, length);
        }
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
#if !MONO
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
        internal unsafe static void CopyNotNull(void* source, void* destination, int length)
        {
#if DEBUG
            if (source == null) throw new Exception("source == null");
            if (destination == null) throw new Exception("destination == null");
            if (length < 0) throw new Exception(length.toString() + " < 0");
#endif
#if MONO
            int shift = (int)destination & (sizeof(ulong) - 1);
            if (shift == 0) copy((byte*)source, (byte*)destination, length);
            else
            {
                shift = sizeof(ulong) - shift;
                if (shift > length) shift = length;
                if ((shift & 1) != 0)
                {
                    *(byte*)destination = *(byte*)source;
                    if ((shift & 2) != 0)
                    {
                        *(ushort*)((byte*)destination + 1) = *(ushort*)((byte*)source + 1);
                        if ((shift & 4) != 0) *(uint*)((byte*)destination + 3) = *(uint*)((byte*)source + 3);
                    }
                    else if ((shift & 4) != 0) *(uint*)((byte*)destination + 1) = *(uint*)((byte*)source + 1);
                }
                else if ((shift & 2) != 0)
                {
                    *(ushort*)destination = *(ushort*)source;
                    if ((shift & 4) != 0) *(uint*)((byte*)destination + 2) = *(uint*)((byte*)source + 2);
                }
                else if ((shift & 4) != 0) *(uint*)destination = *(uint*)source;
                copy((byte*)source + shift, (byte*)destination + shift, length -= (int)shift);
            }
#else
            Win32.Kernel32.RtlMoveMemory(destination, source, length);
#endif
        }
#if MONO
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        unsafe static void copy(byte* source, byte* destination, int length)
        {
#if DEBUG
            if (source == null) throw new Exception("source == null");
            if (destination == null) throw new Exception("destination == null");
            if (length < 0) throw new Exception(length.toString() + " < 0");
#endif
            if (length >= sizeof(ulong) * 4)
            {
                do
                {
                    *((ulong*)destination) = *((ulong*)source);
                    *((ulong*)(destination + sizeof(ulong))) = *((ulong*)(source + sizeof(ulong)));
                    *((ulong*)(destination + sizeof(ulong) * 2)) = *((ulong*)(source + sizeof(ulong) * 2));
                    *((ulong*)(destination + sizeof(ulong) * 3)) = *((ulong*)(source + sizeof(ulong) * 3));
                    destination += sizeof(ulong) * 4;
                    source += sizeof(ulong) * 4;
                }
                while ((length -= sizeof(ulong) * 4) >= sizeof(ulong) * 4);
            }
            if ((length & (sizeof(ulong) * 2)) != 0)
            {
                *((ulong*)destination) = *((ulong*)source);
                *((ulong*)(destination + sizeof(ulong))) = *((ulong*)(source + sizeof(ulong)));
                destination += sizeof(ulong) * 2;
                source += sizeof(ulong) * 2;
            }
            if ((length & sizeof(ulong)) != 0)
            {
                *((ulong*)destination) = *((ulong*)source);
                destination += sizeof(ulong);
                source += sizeof(ulong);
            }
            if ((length & sizeof(uint)) != 0)
            {
                *((uint*)destination) = *((uint*)source);
                destination += sizeof(uint);
                source += sizeof(uint);
            }
            if ((length & 2) != 0)
            {
                *((ushort*)destination) = *((ushort*)source);
                destination += 2;
                source += 2;
            }
            if ((length & 1) != 0) *destination = *source;
        }
#endif
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        internal unsafe static void SimpleCopyNotNull(byte* source, byte* destination, int length)
        {
#if DEBUG
            if (source == null) throw new Exception("source == null");
            if (destination == null) throw new Exception("destination == null");
            if (length < 0) throw new Exception(length.toString() + " < 0");
#endif
            for (byte* end = source + (length & (int.MaxValue - (sizeof(ulong) - 1))); source != end; source += sizeof(ulong), destination += sizeof(ulong))
            {
                *(ulong*)destination = *(ulong*)source;
            }
            if ((length &= (sizeof(ulong) - 1)) != 0)
            {
                if ((length & sizeof(uint)) != 0)
                {
                    *(uint*)destination = *(uint*)source;
                    source += sizeof(uint);
                    destination += sizeof(uint);
                }
                if ((length & sizeof(ushort)) != 0)
                {
                    *(ushort*)destination = *(ushort*)source;
                    source += sizeof(ushort);
                    destination += sizeof(ushort);
                }
                if ((length & 1) != 0) *destination = *source;
            }
        }
        /// <summary>
        /// 复制字节数组(不足8字节按8字节算)
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void SimpleCopyNotNull64(byte* source, byte* destination, int length)
        {
#if DEBUG
            if (source == null) throw new Exception("source == null");
            if (destination == null) throw new Exception("destination == null");
            if (length <= 0) throw new Exception(length.toString() + " <= 0");
#endif
            byte* end = source + ((length + (sizeof(ulong) - 1)) & (int.MaxValue - (sizeof(ulong) - 1)));
            do
            {
                *(ulong*)destination = *(ulong*)source;
                source += sizeof(ulong);
                destination += sizeof(ulong);
            }
            while (source != end);
        }

        /// <summary>
        /// 清除缓存数据委托集合
        /// </summary>
        private static readonly HashSet<ReferenceHashKey<Action<int>>> clearCountCaches = HashSetCreator<ReferenceHashKey<Action<int>>>.Create();
        /// <summary>
        /// 清除缓存数据委托集合
        /// </summary>
        private static readonly HashSet<ReferenceHashKey<Action>> clearCaches = HashSetCreator<ReferenceHashKey<Action>>.Create();
        /// <summary>
        /// 清除缓存数据委托集合 访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock clearCacheLock;
        /// <summary>
        /// 添加清除缓存数据委托
        /// </summary>
        /// <param name="clearCache"></param>
        /// <returns>false 表示已经添加过，无需重复添加</returns>
        public static bool AddClearCache(Action<int> clearCache)
        {
            ReferenceHashKey<Action<int>> hashKey = new ReferenceHashKey<Action<int>>(clearCache);
            clearCacheLock.EnterSleep();
            try
            {
                return clearCountCaches.Add(hashKey);
            }
            finally { clearCacheLock.Exit(); }
        }
        /// <summary>
        /// 添加清除缓存数据委托
        /// </summary>
        /// <param name="clearCache"></param>
        /// <param name="ObjectRootScanType"></param>
        /// <returns>false 表示已经添加过，无需重复添加</returns>
        internal static bool AddClearCache(Action<int> clearCache, Type ObjectRootScanType)
        {
            if (AddClearCache(clearCache))
            {
                AutoCSer.Memory.ObjectRoot.ScanType.Add(ObjectRootScanType);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除清除缓存数据委托
        /// </summary>
        /// <param name="clearCache"></param>
        /// <returns>false 表示没有找到委托</returns>
        public static bool RemoveClearCache(Action<int> clearCache)
        {
            ReferenceHashKey<Action<int>> hashKey = new ReferenceHashKey<Action<int>>(clearCache);
            clearCacheLock.EnterSleep();
            try
            {
                return clearCountCaches.Remove(hashKey);
            }
            finally { clearCacheLock.Exit(); }
        }
        /// <summary>
        /// 添加清除缓存数据委托
        /// </summary>
        /// <param name="clearCache"></param>
        /// <returns>false 表示已经添加过，无需重复添加</returns>
        public static bool AddClearCache(Action clearCache)
        {
            ReferenceHashKey<Action> hashKey = new ReferenceHashKey<Action>(clearCache);
            clearCacheLock.EnterSleep();
            try
            {
                return clearCaches.Add(hashKey);
            }
            finally { clearCacheLock.Exit(); }
        }
        /// <summary>
        /// 添加清除缓存数据委托
        /// </summary>
        /// <param name="clearCache"></param>
        /// <param name="ObjectRootScanType"></param>
        /// <param name="internalTaskSeconds"></param>
        /// <returns></returns>
        internal static bool AddClearCache(Action clearCache, Type ObjectRootScanType, int internalTaskSeconds)
        {
            if (AddClearCache(clearCache))
            {
                AutoCSer.Memory.ObjectRoot.ScanType.Add(ObjectRootScanType);
                if (internalTaskSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(clearCache, internalTaskSeconds, Threading.SecondTimerThreadMode.Synchronous, Threading.SecondTimerKeepMode.Before, internalTaskSeconds);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除清除缓存数据委托
        /// </summary>
        /// <param name="clearCache"></param>
        /// <returns>false 表示没有找到委托</returns>
        public static bool RemoveClearCache(Action clearCache)
        {
            ReferenceHashKey<Action> hashKey = new ReferenceHashKey<Action>(clearCache);
            clearCacheLock.EnterSleep();
            try
            {
                return clearCaches.Remove(hashKey);
            }
            finally { clearCacheLock.Exit(); }
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        /// <param name="IsGCCollect">是否调用 GC 回收</param>
        public static void ClearCache(int count = 0, bool IsGCCollect = true)
        {
            foreach (ReferenceHashKey<Action> clearCache in clearCaches)
            {
                try
                {
                    clearCache.Value();
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.Exception(exception, null, LogLevel.AutoCSer | LogLevel.Exception);
                }
            }
            foreach (ReferenceHashKey<Action<int>> clearCache in clearCountCaches)
            {
                try
                {
                    clearCache.Value(count);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.Exception(exception, null, LogLevel.AutoCSer | LogLevel.Exception);
                }
            }
            if(IsGCCollect) GC.Collect();
            AutoCSer.Memory.UnmanagedPool.ClearCache(count);
        }
        /// <summary>
        /// 清除缓存数据空操作
        /// </summary>
        /// <param name="count"></param>
        private static void emptyClearCache(int count) { }
        /// <summary>
        /// 清除缓存数据空操作
        /// </summary>
        internal static readonly Action<int> EmptyClearCache = emptyClearCache;
    }
}
