using System;
using System.Threading;
using fastCSharp.Extension;
using fastCSharp.Threading;

namespace fastCSharp.SubBuffer
{
    /// <summary>
    /// 缓冲区池
    /// </summary>
    internal unsafe sealed class Pool
    {
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="minSize">数据字节长度</param>
        /// <returns>缓冲区</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public void Get(ref PoolBufferFull buffer, int minSize)
        {
            if (minSize <= Size) Get(ref buffer);
            else buffer.Set(new byte[minSize], 0);
        }
        ///// <summary>
        ///// 根据缓冲区索引获取缓冲区
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //[System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        //internal SubArray<byte> IndexToBuffer(uint index)
        //{
        //    return new SubArray<byte> { Array = Buffers[(int)(index >> arrayBits)], StartIndex = (int)(index & arrayIndexMark) << sizeBits, Length = Size };
        //}

    }
}
