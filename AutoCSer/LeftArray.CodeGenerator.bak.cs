using System;
using System.Collections.Generic;
using fastCSharp.Extension;

namespace fastCSharp
{
    /// <summary>
    /// 数组子串
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public partial struct LeftArray<valueType>
    {
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(valueType[] values)
        {
            int count = values.count();
            if (count != 0)
            {
                addToLength(Length + count);
                System.Array.Copy(values, 0, Array, Length, count);
                Length += count;
            }
        }
    }
}
