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
    internal sealed class LockEquatableLastDictionary<keyType, valueType> where keyType : struct, IEquatable<keyType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        internal Dictionary<keyType, valueType> Dictionary = DictionaryCreator<keyType>.Create<valueType>();
        /// <summary>
        /// 访问锁
        /// </summary>
        internal readonly object Lock = new object();
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
            Monitor.Enter(Lock);
            try
            {
                if (Dictionary.Count != 0) Dictionary = DictionaryCreator<keyType>.Create<valueType>();
                lastKey = default(keyType);
                lastValue = default(valueType);
            }
            finally { Monitor.Exit(Lock); }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGetValue(ref keyType key, out valueType value)
        {
            if (TryGetValueEnter(ref key, out value)) return true;
            Monitor.Exit(Lock);
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        internal bool TryGetValueEnter(ref keyType key, out valueType value)
        {
            Monitor.Enter(Lock);
            if (lastKey.Equals(key))
            {
                value = lastValue;
                Monitor.Exit(Lock);
                return true;
            }
            if (Dictionary.TryGetValue(key, out value))
            {
                lastKey = key;
                lastValue = value;
                Monitor.Exit(Lock);
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
        public void Set(ref keyType key, valueType value)
        {
            Monitor.Enter(Lock);
            try
            {
                Dictionary[lastKey = key] = lastValue = value;
            }
            finally { Monitor.Exit(Lock); }
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetOnly(ref keyType key, valueType value)
        {
            Dictionary[lastKey = key] = lastValue = value;
        }
        /// <summary>
        /// 释放目标
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Exit()
        {
            Monitor.Exit(Lock);
        }
    }
}
