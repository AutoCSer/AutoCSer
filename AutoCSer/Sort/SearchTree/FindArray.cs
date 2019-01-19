using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 查找数据
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FindArray<valueType>
    {
        /// <summary>
        /// 数据匹配委托
        /// </summary>
        internal Func<valueType, bool> IsValue;
        /// <summary>
        /// 数据集合
        /// </summary>
        internal LeftArray<valueType> Array;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(valueType value)
        {
            if (IsValue(value)) Array.Add(value);
        }
    }
}
