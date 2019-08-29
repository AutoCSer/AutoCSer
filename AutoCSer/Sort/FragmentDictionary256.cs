using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 256 基分片 字典
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    public sealed class FragmentDictionary256<keyType, valueType> where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly Dictionary<keyType, valueType>[] dictionarys = new Dictionary<keyType, valueType>[256];
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get; internal set; }
        /// <summary>
        /// 获取或者设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public valueType this[keyType key]
        {
            get
            {
                return dictionarys[key.GetHashCode() & 0xff][key];
            }
            set
            {
                Dictionary<keyType, valueType> dictionary = GetOrCreateDictionary(key);
                int count = dictionary.Count;
                dictionary[key] = value;
                Count += dictionary.Count - count;
            }
        }
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<KeyValuePair<keyType, valueType>> KeyValues
        {
            get
            {
                foreach (Dictionary<keyType, valueType> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (KeyValuePair<keyType, valueType> value in dictionary) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<keyType> Keys
        {
            get
            {
                foreach (Dictionary<keyType, valueType> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (keyType key in dictionary.Keys) yield return key;
                    }
                }
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<valueType> Values
        {
            get
            {
                foreach (Dictionary<keyType, valueType> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (valueType value in dictionary.Values) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            foreach (Dictionary<keyType, valueType> dictionary in dictionarys)
            {
                if (dictionary != null) dictionary.Clear();
            }
            Count = 0;
        }
        /// <summary>
        /// 清除数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClearArray()
        {
            Array.Clear(dictionarys, 0, 256);
            Count = 0;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(keyType key, valueType value)
        {
            GetOrCreateDictionary(key).Add(key, value);
            ++Count;
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(keyType key)
        {
            Dictionary<keyType, valueType> dictionary = dictionarys[key.GetHashCode() & 0xff];
            return dictionary != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(keyType key)
        {
            Dictionary<keyType, valueType> dictionary = dictionarys[key.GetHashCode() & 0xff];
            if (dictionary != null && dictionary.Remove(key))
            {
                --Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(keyType key, out valueType value)
        {
            Dictionary<keyType, valueType> dictionary;
            return TryGetValue(key, out value, out dictionary);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public bool TryGetValue(keyType key, out valueType value, out Dictionary<keyType, valueType> dictionary)
        {
            dictionary = dictionarys[key.GetHashCode() & 0xff];
            if (dictionary != null) return dictionary.TryGetValue(key, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 根据关键字获取字典，不存在时创建字典
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal Dictionary<keyType, valueType> GetOrCreateDictionary(keyType key)
        {
            int index = key.GetHashCode() & 0xff;
            Dictionary<keyType, valueType> dictionary = dictionarys[index];
            if (dictionary == null) dictionarys[index] = dictionary = DictionaryCreator<keyType>.Create<valueType>();
            return dictionary;
        }
    }
}
