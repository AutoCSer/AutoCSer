using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    /// <summary>
    /// 哈希集模拟
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
	public class HashSet<valueType> : IEnumerable<valueType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly Dictionary<valueType, bool> dictionary = AutoCSer.DictionaryCreator.CreateAny<valueType, bool>();
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 哈希集模拟
        /// </summary>
        public HashSet() { }
        /// <summary>
        /// 哈希集模拟
        /// </summary>
        /// <param name="array"></param>
        public HashSet(valueType[] array)
        {
            foreach (valueType value in array) dictionary[value] = true;
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.Keys.GetEnumerator();
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            return dictionary.Keys.GetEnumerator();
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 将指定的元素添加到集中
        /// </summary>
        /// <param name="value">要添加到集中的元素</param>
        /// <returns>如果该元素添加到 System.Collections.Generic.HashSet[T] 对象中则为 true；如果该元素已存在则为 false</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Add(valueType value)
        {
            if (dictionary.ContainsKey(value)) return false;
            dictionary.Add(value, false);
            return true;
        }
        /// <summary>
        /// 移除指定的元素
        /// </summary>
        /// <param name="value">要移除的元素</param>
        /// <returns>如果成功找到并移除该元素，则为 true；否则为 false</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(valueType value)
        {
            return dictionary.Remove(value);
        }
        /// <summary>
        /// 判断是否存在指定的元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(valueType value)
        {
            return dictionary.ContainsKey(value);
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] getArray()
        {
            return dictionary.Keys.getArray();
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="arrayType">返回数组类型</typeparam>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public arrayType[] getArray<arrayType>(Func<valueType, arrayType> getValue)
        {
            if (dictionary.Count == 0) return AutoCSer.NullValue<arrayType>.Array;
            arrayType[] newValues = new arrayType[dictionary.Count];
            int index = 0;
            foreach (valueType value in dictionary.Keys) newValues[index++] = getValue(value);
            if (index != newValues.Length) System.Array.Resize(ref newValues, index);
            return newValues;
        }
	}
}
