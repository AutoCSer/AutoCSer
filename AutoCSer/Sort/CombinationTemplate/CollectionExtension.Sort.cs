using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static unsafe partial class CollectionExtensionSort
    {
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns>数组</returns>
        public static ulong[] getArray(this System.Collections.Generic.ICollection<ulong> values)
        {
            if (values.count() == 0) return EmptyArray<ulong>.Array;
            ulong[] newValues = new ulong[values.Count];
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* write = newValueFixed;
                foreach (ulong value in values) *write++ = value;
            }
            return newValues;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public static ulong[] getArray<valueType>(this System.Collections.Generic.ICollection<valueType> values, Func<valueType, ulong> getValue)
        {
            if (values.count() == 0) return EmptyArray<ulong>.Array;
            ulong[] newValues = new ulong[values.Count];
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* write = newValueFixed;
                foreach (valueType value in values) *write++ = getValue(value);
            }
            return newValues;
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static LeftArray<ulong> getFind(this System.Collections.Generic.ICollection<ulong> values, Func<ulong, bool> isValue)
        {
            if (values.count() == 0) return new LeftArray<ulong>(0);
            ulong[] newValues = new ulong[values.Count];
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* write = newValueFixed;
                foreach (ulong value in values)
                {
                    if (isValue(value)) *write++ = value;
                }
                return new LeftArray<ulong> { Array = newValues, Length = (int)(write - newValueFixed) };
            }
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static LeftArray<ulong> getFind(this System.Collections.ICollection values, Func<ulong, bool> isValue)
        {
            if (values == null) return new LeftArray<ulong>(0);
            ulong[] newValues = new ulong[values.Count];
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* write = newValueFixed;
                foreach (ulong value in values)
                {
                    if (isValue(value)) *write++ = value;
                }
                return new LeftArray<ulong> { Array = newValues, Length = (int)(write - newValueFixed) };
            }
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] getFindArray(this System.Collections.Generic.ICollection<ulong> values, Func<ulong, bool> isValue)
        {
            return values.getFind(isValue).ToArray();
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] getFindArray(this System.Collections.ICollection values, Func<ulong, bool> isValue)
        {
            return values.getFind(isValue).ToArray();
        }
    }
}
