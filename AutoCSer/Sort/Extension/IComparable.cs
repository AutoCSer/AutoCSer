using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 比较接口扩展
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal static class IComparableExtension<valueType> where valueType : IComparable<valueType>
    {
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CompareTo(valueType left, valueType right)
        {
            return left.CompareTo(right);
        }
        /// <summary>
        /// 比较委托
        /// </summary>
        public static readonly Func<valueType, valueType, int> CompareToHandle = CompareTo;
    }
}
