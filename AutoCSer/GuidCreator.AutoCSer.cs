using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Guid 联合体
    /// </summary>
    internal partial struct GuidCreator
    {
        [FieldOffset(0)]
        internal byte Int0;
        [FieldOffset(sizeof(int))]
        internal byte Int1;
        [FieldOffset(sizeof(int) * 2)]
        internal byte Int2;
        [FieldOffset(sizeof(int) * 3)]
        internal byte Int3;

        [FieldOffset(0)]
        internal byte ULong0;
        [FieldOffset(sizeof(ulong))]
        internal byte ULong1;

        /// <summary>
        /// 获取哈希值
        /// </summary>
        internal int HashCode
        {
            get { return Int0 ^ Int1 ^ Int2 ^ Int3; }
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe bool Equals(Guid* other)
        {
            return ((ULong0 ^ *(ulong*)other) | (ULong1 ^ *(ulong*)(((byte*)other) + sizeof(ulong)))) == 0;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Equals(GuidCreator other)
        {
            return ((ULong0 ^ other.ULong0) | (ULong1 ^ other.ULong1)) == 0;
        }
    }
}
