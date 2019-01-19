using System;

namespace fastCSharp
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal static partial class Memory
    {
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否相等</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public static bool equal(this byte[] left, byte[] right)
        {
            if (left == null) return right == null;
            return right != null && (Object.ReferenceEquals(left, right) || Equal(left, right));
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <returns>是否相等</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static bool Equal(byte[] left, byte[] right)
        {
            return left.Length == right.Length && Equal(left, right, left.Length);
        }
        /// <summary>
        /// 字节数组比较
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字节数,必须大于等于0</param>
        /// <returns>是否相等</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static unsafe bool Equal(byte[] left, byte[] right, int count)
        {
            fixed (byte* leftFixed = left, rightFixed = right) return equal(leftFixed, rightFixed, count);
        }
    }
}
