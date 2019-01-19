using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static unsafe partial class Collection
    {
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns>数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getArray(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values)
        {
            if (values.count() == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = newValueFixed;
                foreach (/*Type[0]*/ulong/*Type[0]*/ value in values) *write++ = value;
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
        public static /*Type[0]*/ulong/*Type[0]*/[] getArray<valueType>(this System.Collections.Generic.ICollection<valueType> values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (values.count() == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = newValueFixed;
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
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> getFind(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (values.count() == 0) return default(LeftArray</*Type[0]*/ulong/*Type[0]*/>);
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = newValueFixed;
                foreach (/*Type[0]*/ulong/*Type[0]*/ value in values)
                {
                    if (isValue(value)) *write++ = value;
                }
                return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = newValues, Length = (int)(write - newValueFixed) };
            }
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> getFind(this System.Collections.ICollection values, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (values == null) return default(LeftArray</*Type[0]*/ulong/*Type[0]*/>);
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Count];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = newValueFixed;
                foreach (/*Type[0]*/ulong/*Type[0]*/ value in values)
                {
                    if (isValue(value)) *write++ = value;
                }
                return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = newValues, Length = (int)(write - newValueFixed) };
            }
        }
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray(this System.Collections.Generic.ICollection</*Type[0]*/ulong/*Type[0]*/> values, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
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
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray(this System.Collections.ICollection values, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return values.getFind(isValue).ToArray();
        }
    }
}
