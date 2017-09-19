using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// 字典数组
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DictionaryArray<keyType, valueType>
        where keyType : IEquatable<keyType>
        where valueType : class
    {
        /// <summary>
        /// 字典数组
        /// </summary>
        private Dictionary<keyType, valueType>[] array;
        /// <summary>
        /// 数据数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// 数据集合
        /// </summary>
        internal IEnumerable<valueType> Values
        {
            get
            {
                foreach (Dictionary<keyType, valueType> dictionary in array)
                {
                    if (dictionary != null)
                    {
                        foreach (valueType value in dictionary.Values) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据</returns>
        internal valueType this[keyType key]
        {
            get
            {
                Dictionary<keyType, valueType> dictionary = array[key.GetHashCode() & 0xff];
                if (dictionary != null)
                {
                    valueType value;
                    if (dictionary.TryGetValue(key, out value)) return value;
                }
                return null;
            }
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Reset()
        {
            array = new Dictionary<keyType, valueType>[256];
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal void Add(keyType key, valueType value)
        {
            int index = key.GetHashCode() & 0xff;
            Dictionary<keyType, valueType> dictionary = array[index];
            if (dictionary == null) array[index] = dictionary = DictionaryCreator<keyType>.Create<valueType>();
            dictionary.Add(key, value);
            ++Count;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal void Remove(keyType key, out valueType value)
        {
            int index = key.GetHashCode() & 0xff;
            Dictionary<keyType, valueType> dictionary = array[index];
            if (dictionary != null && dictionary.TryGetValue(key, out value))
            {
                dictionary.Remove(key);
                --Count;
                return;
            }
            value = null;
        }
    }
}
