using System;
using System.Collections.Generic;
using System.Collections;
using fastCSharp.Extension;

namespace fastCSharp
{
    /// <summary>
    /// 数组子串
    /// </summary>
    public partial struct SubArray<valueType>
    {
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="size">容器大小</param>
        public SubArray(int size)
        {
            Array = size > 0 ? new valueType[size] : null;
            StartIndex = Length = 0;
        }
    }
}
