using System;
using System.Runtime.InteropServices;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AutoCSer.Search
{
    /// <summary>
    /// 数组池参数
    /// </summary>
    internal static class ArrayPool
    {
        /// <summary>
        /// 节点池数组二进制位长度
        /// </summary>
        internal const int ArraySizeBit = 16;
        /// <summary>
        /// 节点池数组长度
        /// </summary>
        internal const int ArraySize = 1 << 16;
        /// <summary>
        /// 节点池数组最大索引位置
        /// </summary>
        internal const int ArraySizeAnd = ArraySize - 1;
    }
    /// <summary>
    /// 数组池
    /// </summary>
    /// <typeparam name="valueType">数组类型</typeparam>
    [StructLayout(LayoutKind.Auto)]
    internal struct ArrayPool<valueType>
    {
        /// <summary>
        /// 数组池
        /// </summary>
        internal valueType[][] Pool;
        /// <summary>
        /// 当前分配数组
        /// </summary>
        private valueType[] currentArray;
        /// <summary>
        /// 空闲索引集合
        /// </summary>
        private int[] freeIndexs;
        /// <summary>
        /// 当前分配数组位置
        /// </summary>
        internal int CurrentArrayIndex;
        /// <summary>
        /// 当前分配数组起始位置
        /// </summary>
        private int currentArrayBaseIndex;
        /// <summary>
        /// 池数组位置
        /// </summary>
        private int poolIndex;
        /// <summary>
        /// 当前空闲索引
        /// </summary>
        private int freeIndex;
        /// <summary>
        /// 数组池访问锁
        /// </summary>
        internal readonly object Lock;
        /// <summary>
        /// 数组池
        /// </summary>
        /// <param name="freeCount">空闲索引集合初始化数量</param>
        internal ArrayPool(int freeCount)
        {
            Lock = new object();
            Pool = new valueType[][] { currentArray = new valueType[ArrayPool.ArraySize] };
            freeIndexs = new int[freeCount];
            poolIndex = 1;
            CurrentArrayIndex = currentArrayBaseIndex = freeIndex = 0;
        }
        /// <summary>
        /// 获取可用索引
        /// </summary>
        /// <param name="array">当前分配数组</param>
        /// <returns>索引</returns>
        internal int GetNoLock(out valueType[] array)
        {
            if (CurrentArrayIndex != ArrayPool.ArraySize)
            {
                array = currentArray;
                return CurrentArrayIndex++ + currentArrayBaseIndex;
            }
            if (freeIndex != 0)
            {
                int index = freeIndexs[--freeIndex];
                array = Pool[index >> ArrayPool.ArraySizeBit];
                return index;
            }
            create();
            array = currentArray;
            return currentArrayBaseIndex;
        }
        /// <summary>
        /// 创建当前分配数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void create()
        {
            if (poolIndex == Pool.Length) Pool = Pool.copyNew(poolIndex << 1);
            Pool[poolIndex++] = currentArray = new valueType[ArrayPool.ArraySize];
            CurrentArrayIndex = 1;
            currentArrayBaseIndex += ArrayPool.ArraySize;
        }
        /// <summary>
        /// 获取可用索引
        /// </summary>
        /// <param name="array">当前分配数组</param>
        /// <returns>索引</returns>
        internal int Get(out valueType[] array)
        {
            Monitor.Enter(Lock);
            if (CurrentArrayIndex != ArrayPool.ArraySize)
            {
                array = currentArray;
                int index = CurrentArrayIndex++ + currentArrayBaseIndex;
                Monitor.Exit(Lock);
                return index;
            }
            if (freeIndex != 0)
            {
                int index = freeIndexs[--freeIndex];
                Monitor.Exit(Lock);
                array = Pool[index >> ArrayPool.ArraySizeBit];
                return index;
            }
            try
            {
                create();
                array = currentArray;
                return currentArrayBaseIndex;
            }
            finally { Monitor.Exit(Lock); }
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void FreeNoLock(int index)
        {
            if (freeIndex == freeIndexs.Length) freeIndexs = freeIndexs.copyNew(freeIndex << 1);
            freeIndexs[freeIndex++] = index;
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="indexs"></param>
        internal unsafe void FreeNoLock<keyType>(KeyValue<keyType, int>[] indexs)
        {
            int count = freeIndex + indexs.Length;
            if (count > freeIndexs.Length) freeIndexs = freeIndexs.copyNew((int)((uint)count).UpToPower2(), freeIndex);
            fixed (int* indexFixed = freeIndexs)
            {
                int* write = indexFixed + freeIndex;
                foreach (KeyValue<keyType, int> index in indexs) *write++ = index.Value;
            }
            freeIndex = count;
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="indexs"></param>
        internal void FreeNoLock(ICollection<int> indexs)
        {
            int count = indexs.Count;
            if (count != 0)
            {
                if ((count += freeIndex) > freeIndexs.Length) freeIndexs = freeIndexs.copyNew((int)((uint)count).UpToPower2(), freeIndex);
                foreach (int index in indexs) freeIndexs[freeIndex++] = index;
            }
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="indexs"></param>
        /// <param name="count"></param>
        internal unsafe void Free<keyType>(IEnumerable<StaticSearcher<keyType>.WordCounterIndex> indexs, int count)
            where keyType : IEquatable<keyType>
        {
            if (count != 0)
            {
                Monitor.Enter(Lock);
                try
                {
                    if ((count += freeIndex) > freeIndexs.Length) freeIndexs = freeIndexs.copyNew((int)((uint)count).UpToPower2(), freeIndex);
                    foreach (StaticSearcher<keyType>.WordCounterIndex index in indexs)
                    {
                        index.ClearResult();
                        freeIndexs[freeIndex++] = index.Index;
                    }
                }
                finally { Monitor.Exit(Lock); }
            }
        }
    }
}
