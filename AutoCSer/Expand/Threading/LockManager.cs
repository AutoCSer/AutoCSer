using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 锁管理
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    public sealed class LockManager<keyType> where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 锁集合
        /// </summary>
        private readonly Dictionary<keyType, object> locks = DictionaryCreator<keyType>.Create<object>();
        /// <summary>
        /// 锁集合 访问锁
        /// </summary>
        private readonly object lockLock = new object();
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(keyType key)
        {
            if (key == null) throw new ArgumentNullException();
            object lockObject;
            Monitor.Enter(lockLock);
            try
            {
                if (!locks.TryGetValue(key, out lockObject)) locks.Add(key, lockObject = new object());
            }
            finally { Monitor.Exit(lockLock); }
            return lockObject;
        }
        /// <summary>
        /// 删除锁
        /// </summary>
        /// <param name="key"></param>
        public void Remove(keyType key)
        {
            Monitor.Enter(lockLock);
            try
            {
                locks.Remove(key);
            }
            finally { Monitor.Exit(lockLock); }
        }
    }
}