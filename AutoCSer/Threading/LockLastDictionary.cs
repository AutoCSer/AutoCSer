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
    internal sealed class LockLastDictionary<keyType, valueType>
        where keyType : IEquatable<keyType>
        where valueType : class
    {
        /// <summary>
        /// 获取关键字
        /// </summary>
        private readonly Func<valueType, keyType> getKey;
        /// <summary>
        /// 字典
        /// </summary>
        internal Dictionary<keyType, valueType> Dictionary = DictionaryCreator<keyType>.Create<valueType>();
        /// <summary>
        /// 最后一次访问数据锁
        /// </summary>
        internal SleepFlagSpinLock DictionaryLock;
        /// <summary>
        /// 最后一次访问的数据
        /// </summary>
        private valueType lastValue;
        /// <summary>
        /// 最后关键字缓存字典
        /// </summary>
        /// <param name="getKey">获取关键字</param>
        internal LockLastDictionary(Func<valueType, keyType> getKey)
        {
            this.getKey = getKey;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            if (Dictionary.Count != 0)
            {
                DictionaryLock.EnterSleepFlag();
                try
                {
                    if (Dictionary.Count != 0) Dictionary = DictionaryCreator<keyType>.Create<valueType>();
                }
                finally { DictionaryLock.ExitSleepFlag(); }
            }
            lastValue = null;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在数据，如果不存在则进入锁申请状态</returns>
        internal bool TryGetValue(keyType key, out valueType value)
        {
            value = lastValue;
            if (value != null && key.Equals(getKey(value))) return true;
#if DEBUG
            if (key == null) throw new Exception("key == null");
#endif
            DictionaryLock.Enter();
            if (!Dictionary.TryGetValue(key, out value)) return false;
            DictionaryLock.SleepFlag = 1;
            DictionaryLock.Exit();
            return true;
        }
        /// <summary>
        /// 锁申请状态中设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(keyType key, valueType value)
        {
            lastValue = value;
            Dictionary[key] = value;
        }
        /// <summary>
        /// 释放目标
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Exit()
        {
            DictionaryLock.ExitSleepFlag();
        }
    }
}
