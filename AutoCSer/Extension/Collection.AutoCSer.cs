using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static partial class Collection
    {
        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static LeftArray<valueType> getFind<valueType>(this ICollection<valueType> values, Func<valueType, bool> isValue)
        {
            int count = values.count();
            if (count != 0)
            {
                valueType[] value = new valueType[count];
                count = 0;
                foreach (valueType nextValue in values)
                {
                    if (isValue(nextValue)) value[count++] = nextValue;
                }
                return new LeftArray<valueType> { Array = value, Length = count };
            }
            return default(LeftArray<valueType>);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="values">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString(this ICollection<string> values, string join)
        {
            return values != null ? string.Join(join, values.getArray()) : null;
        }
    }
}
