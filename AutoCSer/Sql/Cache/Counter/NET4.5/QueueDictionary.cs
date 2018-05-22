using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列 字典缓存
    /// </summary>
    public sealed partial class QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType>
    {
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private sealed class GetByKeyAwaiter : Threading.Awaiter<valueType>
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType> queue;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 字典关键字
            /// </summary>
            private dictionaryKeyType dictionaryKey;
            /// <summary>
            /// 失败返回值
            /// </summary>
            private valueType nullValue;
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="key">关键字</param>
            /// <param name="dictionaryKey">字典关键字</param>
            /// <param name="nullValue">失败返回值</param>
            internal GetByKeyAwaiter(QueueDictionary<valueType, modelType, counterKeyType, keyType, dictionaryKeyType> queue, keyType key, dictionaryKeyType dictionaryKey, valueType nullValue)
            {
                this.queue = queue;
                this.key = key;
                this.dictionaryKey = dictionaryKey;
                this.nullValue = nullValue;
            }
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    Value = queue.get(ref connection, key, dictionaryKey);
                }
                finally
                {
                    if (Value == null) Value = nullValue;
                    if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
                }
                return LinkNext;
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="dictionaryKey">字典关键字</param>
        /// <param name="nullValue">失败返回值</param>
        /// <returns>缓存数据</returns>
        public Threading.Awaiter<valueType> GetAwaiter(keyType key, dictionaryKeyType dictionaryKey, valueType nullValue)
        {
            GetByKeyAwaiter task = new GetByKeyAwaiter(this, key, dictionaryKey, nullValue);
            counter.SqlTable.AddQueue(task);
            return task;
        }
    }
}
