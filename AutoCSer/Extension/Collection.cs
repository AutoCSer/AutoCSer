using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// ICollection 泛型转换
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Collection<valueType> : ICollection<valueType>
    {
        /// <summary>
        /// ICollection数据集合
        /// </summary>
        private ICollection collection;
        /// <summary>
        /// ICollection泛型转换
        /// </summary>
        /// <param name="collection">ICollection数据集合</param>
        public Collection(ICollection collection)
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
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            if (collection != null)
            {
                foreach (valueType value in collection) yield return value;
            }
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<valueType>)this).GetEnumerator();
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear() { }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(valueType value) { }
        /// <summary>
        /// 移除数据(不可用)
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public bool Remove(valueType value)
        {
            return false;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CopyTo(valueType[] values, int index)
        {
            if (collection != null) collection.CopyTo(values, index);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        public bool Contains(valueType value)
        {
            if (collection != null)
            {
                foreach (valueType nextValue in collection)
                {
                    if (nextValue.Equals(value)) return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static partial class Collection
    {
        /// <summary>
        /// ICollection泛型转换
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>泛型数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ICollection<valueType> toGeneric<valueType>(this ICollection value)
        {
            return new Collection<valueType>(value);
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>null为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int count<valueType>(this ICollection<valueType> value)
        {
            return value != null ? value.Count : 0;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <returns>数组</returns>
        public static valueType[] getArray<valueType>(this ICollection<valueType> values)
        {
            if (values.count() == 0) return NullValue<valueType>.Array;
            valueType[] newValues = new valueType[values.Count];
            int index = 0;
            foreach (valueType value in values) newValues[index++] = value;
            if (index != newValues.Length) System.Array.Resize(ref newValues, index);
            return newValues;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="arrayType">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public static arrayType[] getArray<valueType, arrayType>(this ICollection<valueType> values, Func<valueType, arrayType> getValue)
        {
            if (values.count() == 0) return NullValue<arrayType>.Array;
            arrayType[] newValues = new arrayType[values.Count];
            int index = 0;
            foreach (valueType value in values) newValues[index++] = getValue(value);
            if (index != newValues.Length) System.Array.Resize(ref newValues, index);
            return newValues;
        }
        ///// <summary>
        ///// 根据集合内容返回数组
        ///// </summary>
        ///// <typeparam name="valueType">枚举值类型</typeparam>
        ///// <typeparam name="arrayType">返回数组类型</typeparam>
        ///// <param name="values">值集合</param>
        ///// <param name="getValue">获取数组值的委托</param>
        ///// <returns>数组</returns>
        //public static arrayType[] getArray<valueType, arrayType>(this ICollection<valueType> values, Func<valueType, arrayType> getValue)
        //{
        //    if (values.count() == 0) return NullValue<arrayType>.Array;
        //    arrayType[] newValues = new arrayType[values.Count];
        //    int index = 0;
        //    foreach (valueType value in values) newValues[index++] = getValue(value);
        //    if (index != newValues.Length) Array.Resize(ref newValues, index);
        //    return newValues;
        //}
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="arrayType">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public static arrayType[] getFindArray<valueType, arrayType>(this ICollection<valueType> values, Func<valueType, arrayType> getValue, Func<arrayType, bool> isValue)
        {
            if (values.count() == 0) return NullValue<arrayType>.Array;
            arrayType[] newValues = new arrayType[values.Count];
            int index = 0;
            foreach (valueType value in values)
            {
                arrayType arrayValue = getValue(value);
                if (isValue(arrayValue)) newValues[index++] = getValue(value);
            }
            if (index != newValues.Length) System.Array.Resize(ref newValues, index);
            return newValues;
        }
    }
}
