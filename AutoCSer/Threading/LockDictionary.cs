using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 字典
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LockDictionary<keyType, valueType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private Dictionary<keyType, valueType> dictionary = DictionaryCreator.CreateAny<keyType, valueType>();
        /// <summary>
        /// 访问锁
        /// </summary>
        private readonly object dictionaryLock = new object();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        public bool TryGetValue(keyType key, out valueType value)
        {
            Monitor.Enter(dictionaryLock);
            if (dictionary.TryGetValue(key, out value))
            {
                Monitor.Exit(dictionaryLock);
                return true;
            }
            Monitor.Exit(dictionaryLock);
            return false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(keyType key, valueType value)
        {
            Monitor.Enter(dictionaryLock);
            try
            {
                dictionary[key] = value;
            }
            finally { Monitor.Exit(dictionaryLock); }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldValue">被替换的数据</param>
        /// <returns>是否存在数据</returns>
        public bool Set(keyType key, valueType value, out valueType oldValue)
        {
            Monitor.Enter(dictionaryLock);
            if (dictionary.TryGetValue(key, out oldValue))
            {
                try
                {
                    dictionary[key] = value;
                }
                finally { Monitor.Exit(dictionaryLock); }
                return true;
            }
            try
            {
                dictionary.Add(key, value);
            }
            finally { Monitor.Exit(dictionaryLock); }
            return false;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        internal void Clear()
        {
            Monitor.Enter(dictionaryLock);
            try
            {
                if (dictionary.Count != 0) dictionary = DictionaryCreator.CreateAny<keyType, valueType>();
            }
            finally { Monitor.Exit(dictionaryLock); }
        }
    }
}
