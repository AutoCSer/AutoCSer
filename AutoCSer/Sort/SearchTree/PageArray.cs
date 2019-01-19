using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 分页数组数据
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PageArray<valueType>
    {
        /// <summary>
        /// 跳过数据
        /// </summary>
        internal int SkipCount;
        /// <summary>
        /// 数组位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 数组
        /// </summary>
        internal valueType[] Array;
        /// <summary>
        /// 数组写入是否完成
        /// </summary>
        internal bool IsArray
        {
            get { return Index == Array.Length; }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组写入是否完成</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Add(valueType value)
        {
            Array[Index++] = value;
            return IsArray;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int AddDesc(valueType value)
        {
            Array[--Index] = value;
            return Index;
        }
    }
    /// <summary>
    /// 分页数组数据
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <typeparam name="arrayType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PageArray<valueType, arrayType>
    {
        /// <summary>
        /// 跳过数据
        /// </summary>
        internal int SkipCount;
        /// <summary>
        /// 数组位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 数组
        /// </summary>
        internal arrayType[] Array;
        /// <summary>
        /// 获取数据
        /// </summary>
        internal Func<valueType, arrayType> GetValue;
        /// <summary>
        /// 数组写入是否完成
        /// </summary>
        internal bool IsArray
        {
            get { return Index == Array.Length; }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组写入是否完成</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Add(valueType value)
        {
            Array[Index++] = GetValue(value);
            return IsArray;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组位置</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int AddDesc(valueType value)
        {
            Array[--Index] = GetValue(value);
            return Index;
        }
    }
}
