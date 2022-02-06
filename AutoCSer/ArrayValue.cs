using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组元素，用于一次性操作数据元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct ArrayValue<T> where T : class
    {
        /// <summary>
        /// 数组元素
        /// </summary>
        internal T Value;
        /// <summary>
        /// 弹出数组元素
        /// </summary>
        /// <returns>数组元素</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T Pop()
        {
            T value = Value;
            Value = null;
            return value;
        }
    }
}
