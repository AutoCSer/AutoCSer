using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal struct DateTimeSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public DateTime Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(long))]
        public int Index;
        /// <summary>
        /// 设置排序索引
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="index">位置索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(DateTime value, int index)
        {
            Value = value;
            Index = index;
        }
        /// <summary>
        /// 根据数组获取排序索引
        /// </summary>
        /// <typeparam name="valueType">数组类型</typeparam>
        /// <param name="indexFixed">排序索引数组</param>
        /// <param name="values">数组</param>
        /// <param name="getValue">数据排序值获取器</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Create<valueType>(DateTimeSortIndex* indexFixed, valueType[] values, Func<valueType, DateTime> getValue)
        {
            int index = 0;
            foreach (valueType value in values) (*indexFixed++).Set(getValue(value), index++);
        }
        /// <summary>
        /// 根据数组获取排序索引
        /// </summary>
        /// <typeparam name="valueType">数组类型</typeparam>
        /// <param name="indexFixed">排序索引数组</param>
        /// <param name="values">数组</param>
        /// <param name="getValue">数据排序值获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void Create<valueType>(DateTimeSortIndex* indexFixed, valueType[] values, Func<valueType, DateTime> getValue, int startIndex, int count)
        {
            for (int endIndex = startIndex + count; startIndex != endIndex; (*indexFixed++).Set(getValue(values[startIndex]), startIndex++)) ;
        }
        /// <summary>
        /// 根据排序索引获取数组
        /// </summary>
        /// <typeparam name="valueType">数组类型</typeparam>
        /// <param name="indexFixed">排序索引数组</param>
        /// <param name="values">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static valueType[] Create<valueType>(DateTimeSortIndex* indexFixed, valueType[] values, int count)
        {
            valueType[] newValues = new valueType[count];
            for (int index = 0; index != count; ++index) newValues[index] = values[(*indexFixed++).Index];
            return newValues;
        }
    }
}
