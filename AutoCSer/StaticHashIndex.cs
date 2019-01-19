using System;
using fastCSharp.Extension;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希索引
    /// </summary>
    public unsafe abstract class StaticHashIndex : IDisposable
    {
        /// <summary>
        /// 索引集合
        /// </summary>
        protected Pointer.Size indexs;
        /// <summary>
        /// 索引&amp;值
        /// </summary>
        protected int indexAnd;
        /// <summary>
        /// 获取哈希数据数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="hashs">哈希集合</param>
        /// <returns>哈希数据数组</returns>
        protected valueType[] getValues<valueType>(valueType[] values, int* hashs)
        {
            int indexBits = ((uint)values.Length).bits();
            if (indexBits < defaultArrayLengthBits) indexBits = defaultArrayLengthBits;
            else if ((1 << (indexBits - 1)) == values.Length) --indexBits;
            indexAnd = 1 << indexBits;
            indexs = Unmanaged.GetSize64(indexAnd-- * sizeof(Range), true);
            Range* indexFixed = (Range*)indexs.Data;
            for (int* hash = hashs + values.Length; hash != hashs; ++indexFixed[*--hash & indexAnd].StartIndex) ;
            int startIndex = 0;
            for (Range* index = indexFixed, endIndex = indexFixed + indexAnd + 1; index != endIndex; ++index)
            {
                int nextIndex = startIndex + (*index).StartIndex;
                (*index).StartIndex = (*index).EndIndex = startIndex;
                startIndex = nextIndex;
            }
            valueType[] newValues = new valueType[values.Length];
            foreach (valueType value in values) newValues[indexFixed[*hashs++ & indexAnd].EndIndex++] = value;
            //for (int index = 0; index != values.Length; newValues[indexFixed[*hashs++ & indexAnd].EndIndex++] = values[index++]) ;
            return newValues;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (indexs.Data != defaultIndexs.Data) Unmanaged.Free(ref indexs);
        }

        /// <summary>
        /// 默认索引集合尺寸二进制位数
        /// </summary>
        protected const int defaultArrayLengthBits = 4;
        /// <summary>
        /// 默认索引集合
        /// </summary>
        protected static Pointer.Size defaultIndexs = Unmanaged.GetStaticSize64((1 << defaultArrayLengthBits) * sizeof(Range), true);
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="value">数据</param>
        /// <returns>哈希值</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        protected static int getHashCode<valueType>(valueType value)
        {
            if (value != null)
            {
                int code = value.GetHashCode();
                return code ^ (code >> defaultArrayLengthBits);
            }
            return 0;
        }
    }
}
