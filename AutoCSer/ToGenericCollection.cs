using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// ICollection 泛型转换
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ToGenericCollection<T> : ICollection<T>
    {
        /// <summary>
        /// ICollection数据集合
        /// </summary>
        private readonly ICollection collection;
        /// <summary>
        /// ICollection泛型转换
        /// </summary>
        /// <param name="collection">ICollection数据集合</param>
        public ToGenericCollection(ICollection collection)
        {
            this.collection = collection;
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return collection != null ? collection.Count : 0; }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get { return true; } }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (collection != null)
            {
                foreach (T value in collection) yield return value;
            }
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(T value)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 移除数据(不可用)
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public bool Remove(T value)
        {
            return false;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CopyTo(T[] values, int index)
        {
            if (collection != null) collection.CopyTo(values, index);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        public bool Contains(T value)
        {
            if (collection != null)
            {
                foreach (T nextValue in collection)
                {
                    if (nextValue.Equals(value)) return true;
                }
            }
            return false;
        }
    }
}
