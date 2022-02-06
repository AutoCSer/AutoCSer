using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static partial class SubArrayExtensionSort
    {
        /// <summary>
        /// 获取反转数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static valueType[] GetReverse<valueType>(this SubArray<valueType> values)
        {
            if (values.Length == 0) return EmptyArray<valueType>.Array;
            valueType[] newArray = new valueType[values.Length];
            if (values.Start == 0)
            {
                int index = values.Length;
                foreach (valueType value in values.Array)
                {
                    newArray[--index] = value;
                    if (index == 0) break;
                }
            }
            else
            {
                int index = values.Length, copyIndex = values.Start;
                valueType[] array = values.Array;
                do
                {
                    newArray[--index] = array[copyIndex++];
                }
                while (index != 0);
            }
            return newArray;
        }
    }
}
