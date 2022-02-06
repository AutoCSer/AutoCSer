using System;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer
{
    /// <summary>
    /// 索引池
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IndexValuePool<valueType> where valueType : struct
    {
        /// <summary>
        /// 对象池
        /// </summary>
        internal valueType[] Array;
        /// <summary>
        /// 空闲索引集合
        /// </summary>
        private int[] freeIndexs;
        /// <summary>
        /// 对象池当前索引位置
        /// </summary>
        internal int ArrayIndex;
        /// <summary>
        /// 当前空闲索引
        /// </summary>
        private int freeIndex;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="size"></param>
        internal void Reset(int size)
        {
            Array = new valueType[size];
            freeIndexs = new int[size];
            ArrayIndex = freeIndex = 0;
        }
        /// <summary>
        /// 获取池索引
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetIndex()
        {
            if (ArrayIndex == Array.Length)
            {
                if (freeIndex != 0) return freeIndexs[--freeIndex];
                Array = Array.copyNew(ArrayIndex << 1);
            }
            return ArrayIndex++;
        }
        /// <summary>
        /// 释放对象(当前占用锁状态)
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free(int index)
        {
            if (freeIndex == freeIndexs.Length) freeIndexs = freeIndexs.copyNew(freeIndex << 1);
            freeIndexs[freeIndex++] = index;
        }
    }
}
