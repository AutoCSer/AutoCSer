using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SubBuffer
{
    /// <summary>
    /// 缓冲区索引信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct PoolBuffer
    {
        /// <summary>
        /// 缓冲区池
        /// </summary>
        internal Pool Pool;
        /// <summary>
        /// 缓冲区索引信息
        /// </summary>
        internal uint Index;
        /// <summary>
        /// 设置缓冲区索引信息
        /// </summary>
        /// <param name="pool">缓冲区池</param>
        /// <param name="index">缓冲区索引信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(Pool pool, uint index)
        {
            Pool = pool;
            Index = index;
        }
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            if (Pool != null) Pool.Push(ref this);
        }
        /// <summary>
        /// 复制数据并清除数据源
        /// </summary>
        /// <param name="targetBuffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyToClear(ref PoolBuffer targetBuffer)
        {
            targetBuffer.Pool = Pool;
            targetBuffer.Index = Index;
            Pool = null;
        }
        ///// <summary>
        ///// 不相等则释放缓冲区
        ///// </summary>
        ///// <param name="other"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void FreeNotEquals(ref PoolBuffer other)
        //{
        //    if (Pool != null && (Index != other.Index || Pool != other.Pool))
        //    {
        //        if (Index == other.Index && Pool == other.Pool) Pool = null;
        //        else Pool.Push(ref this);
        //    }
        //}
        ///// <summary>
        ///// 缓冲区
        ///// </summary>
        //public SubArray<byte> Buffer
        //{
        //    get { return Pool.IndexToBuffer(Index); }
        //}
    }
}
