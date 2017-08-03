using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.TcpServerPerformance
{
    /// <summary>
    /// 加法计算
    /// </summary>
    public struct Add
    {
        /// <summary>
        /// 左值
        /// </summary>
        public int Left;
        /// <summary>
        /// 右值
        /// </summary>
        public int Right;
        /// <summary>
        /// 和数
        /// </summary>
        public int Sum;
        /// <summary>
        /// 加法计算
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public Add(int left, int right)
        {
            Left = left;
            Right = right;
            Sum = left + right;
        }
        /// <summary>
        /// 加法验证
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CheckSum(int left, out int right)
        {
            right = Right;
            return (left ^ Left) | ((left + Right) ^ Sum);
        }
    }
}
