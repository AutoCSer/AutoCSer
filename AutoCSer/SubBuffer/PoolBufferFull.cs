using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SubBuffer
{
    /// <summary>
    /// 缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PoolBufferFull 
    {
        /// <summary>
        /// 缓冲区索引信息
        /// </summary>
        internal PoolBuffer PoolBuffer;
        /// <summary>
        /// 缓冲区
        /// </summary>
        internal byte[] Buffer;
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        internal int StartIndex;
        /// <summary>
        /// 缓冲区长度
        /// </summary>
        internal int Length
        {
            get { return PoolBuffer.Pool == null ? Buffer.Length : PoolBuffer.Pool.Size; }
        }
        /// <summary>
        /// 缓冲区长度，可能返回 0
        /// </summary>
        internal int NullLength
        {
            get { return PoolBuffer.Pool == null ? (Buffer == null ? 0 : Buffer.Length) : PoolBuffer.Pool.Size; }
        }
        /// <summary>
        /// 设置缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] buffer, int startIndex)
        {
            Buffer = buffer;
            StartIndex = startIndex;
        }
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            Buffer = null;
            PoolBuffer.Free();
        }
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryFree()
        {
            if (Buffer != null)
            {
                PoolBuffer.Free();
                Buffer = null;
            }
        }
        /// <summary>
        /// 复制数据并清除数据源
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyToClear(ref PoolBufferFull buffer)
        {
            buffer.Buffer = Buffer;
            buffer.StartIndex = StartIndex;
            PoolBuffer.CopyToClear(ref buffer.PoolBuffer);
            Buffer = null;
        }
        /// <summary>
        /// 设置数组子串
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ToSubByteArray(ref SubArray<byte> data)
        {
            data.Set(Buffer, StartIndex, Length);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(ref PoolBufferFull buffer, int count)
        {
            System.Buffer.BlockCopy(Buffer, StartIndex, buffer.Buffer, buffer.StartIndex, count);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            Buffer = null;
            PoolBuffer.Pool = null;
        }
        ///// <summary>
        ///// 不相等则释放缓冲区
        ///// </summary>
        ///// <param name="other"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void FreeNotEquals(ref PoolBufferFull other)
        //{
        //    Buffer = null;
        //    PoolBuffer.FreeNotEquals(ref other.PoolBuffer);
        //}
    }
}
