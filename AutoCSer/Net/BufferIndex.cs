using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 索引位置
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct BufferIndex
    {
        /// <summary>
        /// 索引位置
        /// </summary>
        [FieldOffset(0)]
        internal int Value;
        /// <summary>
        /// 起始位置
        /// </summary>
        [FieldOffset(0)]
        public short StartIndex;
        /// <summary>
        /// 长度
        /// </summary>
        [FieldOffset(2)]
        public short Length;
        /// <summary>
        /// 结束位置
        /// </summary>
        internal int EndIndex
        {
            get { return StartIndex + Length; }
        }
        /// <summary>
        /// 设置索引位置
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(short startIndex, short length)
        {
            StartIndex = startIndex;
            Length = length;
        }
        /// <summary>
        /// 设置索引位置
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(long startIndex, long length)
        {
            StartIndex = (short)startIndex;
            Length = (short)length;
        }
        /// <summary>
        /// 移动到下一个位置
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Next()
        {
            ++StartIndex;
            --Length;
        }
    }
}
