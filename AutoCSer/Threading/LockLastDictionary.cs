using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 最后关键字缓存字典
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class LockLastDictionary<keyType, valueType> where keyType : class
    {
        /// <summary>
        /// 访问锁
        /// </summary>
        private readonly object dictionaryLock = new object();
        /// <summary>
        /// 字典
        /// </summary>
        private Dictionary<keyType, valueType> dictionary = DictionaryCreator.CreateOnly<keyType, valueType>();
        /// <summary>
        /// 最后一次访问数据锁
        /// </summary>
        private int lastLock;
        /// <summary>
        /// 最后一次访问的关键字
        /// </summary>
        private keyType lastKey;
        /// <summary>
        /// 最后一次访问的数据
        /// </summary>
        private valueType lastValue;
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            Monitor.Enter(dictionaryLock);
            try
            {
                if (dictionary.Count != 0) dictionary = DictionaryCreator.CreateOnly<keyType, valueType>();
                while (System.Threading.Interlocked.CompareExchange(ref lastLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.LockLastDictionarySet);
                lastKey = default(keyType);
                lastValue = default(valueType);
                System.Threading.Interlocked.Exchange(ref lastLock, 0);
            }
            finally { Monitor.Exit(dictionaryLock); }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        internal bool TryGetValue(keyType key, out valueType value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref lastLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.LockLastDictionaryGet);
            if (lastKey == key)
            {
                value = lastValue;
                System.Threading.Interlocked.Exchange(ref lastLock, 0);
                return true;
            }
            System.Threading.Interlocked.Exchange(ref lastLock, 0);
            Monitor.Enter(dictionaryLock);
            if (dictionary.TryGetValue(key, out value))
            {
                while (System.Threading.Interlocked.CompareExchange(ref lastLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.LockLastDictionarySet);
                lastKey = key;
                lastValue = value;
                System.Threading.Interlocked.Exchange(ref lastLock, 0);
                Monitor.Exit(dictionaryLock);
                return true;
            }
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
            while (System.Threading.Interlocked.CompareExchange(ref lastLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.LockLastDictionarySet);
            lastKey = key;
            lastValue = value;
            System.Threading.Interlocked.Exchange(ref lastLock, 0);
            dictionary[key] = value;
        }
        /// <summary>
        /// 释放目标
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Exit()
        {
            Monitor.Exit(dictionaryLock);
        }
    }
}
