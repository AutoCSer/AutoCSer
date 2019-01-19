using System;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 索引池
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IndexValuePool<valueType> where valueType : struct
    {
        /// <summary>
        /// 对象池访问锁
        /// </summary>
        internal object ArrayLock;
        /// <summary>
        /// 对象池
        /// </summary>
        internal valueType[] Array;
        /// <summary>
        /// 对象池当前索引位置
        /// </summary>
        internal int PoolIndex;
        /// <summary>
        /// 空闲索引集合
        /// </summary>
        private int[] freeIndexs;
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
            ArrayLock = new object();
            Array = new valueType[size];
            freeIndexs = new int[size];
            PoolIndex = freeIndex = 0;
        }
        ///// <summary>
        ///// 申请对象池访问锁
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal bool TryEnter()
        //{
        //    Monitor.Enter(ArrayLock);
        //    if (Array == null)
        //    {
        //        Monitor.Exit(ArrayLock);
        //        return false;
        //    }
        //    return true;
        //}
        /// <summary>
        /// 获取池索引(当前占用锁状态)并保持锁状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetIndexContinue() 
        {
            //if (freeIndex == 0)
            //{
            //    if (PoolIndex == Array.Length) Array = Array.copyNew(PoolIndex << 1);
            //    return PoolIndex++;
            //}
            //return freeIndexs[--freeIndex];
            if (PoolIndex == Array.Length)
            {
                if (freeIndex != 0) return freeIndexs[--freeIndex];
                Array = Array.copyNew(PoolIndex << 1);
            }
            return PoolIndex++;
        }
        /// <summary>
        /// 释放对象(当前占用锁状态)
        /// </summary>
        /// <param name="index"></param>
        internal void FreeExit(int index)
        {
            if (freeIndex == freeIndexs.Length)
            {
                try
                {
                    newFree(index);
                }
                finally { Monitor.Exit(ArrayLock); }
                return;
            }
            freeIndexs[freeIndex++] = index;
            Monitor.Exit(ArrayLock);
        }
        /// <summary>
        /// 释放对象(当前占用锁状态)
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeContinue(int index)
        {
            if (freeIndex == freeIndexs.Length) newFree(index);
            else freeIndexs[freeIndex++] = index;
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void newFree(int index)
        {
            freeIndexs = freeIndexs.copyNew(freeIndex << 1);
            freeIndexs[freeIndex++] = index;
        }
        /// <summary>
        /// 清除索引数据
        /// </summary>
        /// <param name="poolIndex">对象池当前索引位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClearIndexExit(int poolIndex)
        {
            PoolIndex = poolIndex;
            freeIndex = 0;
            Monitor.Exit(ArrayLock);
        }
        /// <summary>
        /// 清除索引数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClearIndexContinue()
        {
            PoolIndex = freeIndex = 0;
        }
    }
}
