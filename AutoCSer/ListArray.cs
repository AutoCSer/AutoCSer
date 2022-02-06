using System;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 单向动态数组
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ListArray<T> : IList<T>
    {
        /// <summary>
        /// 数组子串
        /// </summary>
        internal LeftArray<T> Array;
        /// <summary>
        /// 长度
        /// </summary>
        public int Count
        {
            get { return Array.Length; }
        }
        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get { return Array.IsReadOnly; } }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
        public T this[int index]
        {
            get
            {
                return Array[index];
            }
            set
            {
                Array[index] = value;
            }
        }
        /// <summary>
        /// 单向动态数组
        /// </summary>
        /// <param name="capacity">容器大小</param>
        public ListArray(int capacity = 0)
        {
            Array = new LeftArray<T>(capacity);
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="array">数据数组</param>
        internal ListArray(T[] array)
        {
            Array = new LeftArray<T>(array);
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="array">数据数组</param>
        internal ListArray(LeftArray<T> array)
        {
            Array = array;
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="array">数据数组</param>
        internal ListArray(ref LeftArray<T> array)
        {
            Array = array;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Array.Length != 0) return new Enumerator<T>.Array(Array.Array, 0, Array.Length);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Array.Length != 0) return new Enumerator<T>.Array(Array.Array, 0, Array.Length);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(T value)
        {
            Array.Add(value);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(T[] array)
        {
            Array.Add(array);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Insert(int index, T value)
        {
            Array.Insert(index, value);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(T value)
        {
            return Array.Contains(value);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CopyTo(T[] values, int index)
        {
            Array.CopyTo(values, index);
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(T value)
        {
            return Array.IndexOf(value);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(T value)
        {
            return Array.Remove(value);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            Array.RemoveAt(index);
        }
    }
}
