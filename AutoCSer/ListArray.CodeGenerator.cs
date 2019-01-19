using System;
using fastCSharp.Extension;
using System.Collections.Generic;
using System.Collections;

namespace fastCSharp
{
    /// <summary>
    /// 单向动态数组
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public partial class ListArray<valueType> : IList<valueType>
    {
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return Length; }
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public void Clear()
        {
            if (Array.Length != 0)
            {
                if (IsClearArray) System.Array.Clear(Array, 0, Array.Length);
                Length = 0;
            }
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public bool Contains(valueType value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        public void Insert(int index, valueType value)
        {
            if ((uint)index >= (uint)Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] >= length[" + Length.toString() + "]");
            if (index == Length) Add(value);
            else
            {
                if (Length != Array.Length)
                {
                    fastCSharp.Extension.ArrayExtension.MoveNotNull(Array, index, index + 1, Length - index);
                    Array[index] = value;
                    ++Length;
                }
                else
                {
                    valueType[] values = GetNewArray(Array.Length << 1);
                    System.Array.Copy(Array, 0, values, 0, index);
                    values[index] = value;
                    System.Array.Copy(Array, index, values, index + 1, Length++ - index);
                    Array = values;
                }
            }
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public void CopyTo(valueType[] values, int index)
        {
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (Length + index > values.Length) throw new IndexOutOfRangeException("Length" + Length.toString() + " + index" + index.toString() + " > values.Length[" + values.Length.toString() + "]");
            System.Array.Copy(Array, 0, values, index, Length);
        }
    }
}
