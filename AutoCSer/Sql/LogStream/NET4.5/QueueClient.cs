using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 先进先出队列缓存客户端
    /// </summary>
    public sealed partial class QueueClient<valueType, modelType, keyType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<valueType> GetAsync(keyType key)
        {
            if (isLog)
            {
                RandomKey<keyType> randomKey = key;
                KeyValue<valueType, EventWaitHandle> value;
                Monitor.Enter(logLock);
                if (queue.TryGetValue(ref randomKey, out value))
                {
                    Monitor.Exit(logLock);
                    if (value.Value == null) return value.Key;
                    return await getValueAwaiter(key);
                }
                try
                {
                    value.Value = new EventWaitHandle(false, EventResetMode.ManualReset);
                    queue.Set(ref randomKey, value);
                    if (queue.Count > maxCount) queue.UnsafePopNode();
                }
                finally { Monitor.Exit(logLock); }
                try
                {
                    value.Key = await getValueAwaiter(key);
                }
                finally
                {
                    KeyValue<valueType, EventWaitHandle> cacheValue;
                    Monitor.Enter(logLock);
                    if (queue.TryGetOnly(key, out cacheValue) && cacheValue.Value == value.Value)
                    {
                        try
                        {
                            queue.SetOnly(key, new KeyValue<valueType, EventWaitHandle>(value.Key, null));
                        }
                        finally { Monitor.Exit(logLock); }
                    }
                    else Monitor.Exit(logLock);
                    value.Value.Set();
                }
                return value.Key;
            }
            return await getValueAwaiter(key);
        }
    }
}
