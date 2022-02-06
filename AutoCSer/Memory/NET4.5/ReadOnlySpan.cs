using System;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 内存数据段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe struct ReadOnlySpan<T>
    {
        /// <summary>
        /// 数据起始位置
        /// </summary>
        internal readonly void* Data;
        /// <summary>
        /// 数据长度
        /// </summary>
        internal readonly int Length;
        /// <summary>
        /// 内存数据段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        internal ReadOnlySpan(void* data, int size)
        {
#if DEBUG
            if (size < 0) throw new Exception(size.toString() + " < 0");
#endif
            Data = data;
            Length = size;
        }
    }
}
