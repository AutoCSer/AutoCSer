using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Memory_HtmlTitle
    {
        /// <summary>
        /// 字节数组比较(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        /// <returns>是否相等</returns>
        private static bool equalCaseNotNull(byte* left, byte* right, int count)
        {
            for (byte* end = left + count; left != end; ++left, ++right)
            {
                if (*left != *right)
                {
                    if ((*left | 0x20) != (*right | 0x20) || (uint)(*left - 'a') >= 26) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 字节数组比较(忽略大小写)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否相等</returns>
        internal static bool EqualCase(this SubArray<byte> left, ref SubArray<byte> right)
        {
            if (left.Length == 0) return right.Length == 0 && !((left.Array == null) ^ (right.Array == null));
            if (right.Length == left.Length)
            {
                if (Object.ReferenceEquals(left.Array, right.Array) && left.Start == right.Start) return true;
                fixed (byte* leftFixed = left.Array, rightFixed = right.Array)
                {
                    return equalCaseNotNull(leftFixed + left.Start, rightFixed + right.Start, left.Length);
                }
            }
            return false;
        }
    }
}
