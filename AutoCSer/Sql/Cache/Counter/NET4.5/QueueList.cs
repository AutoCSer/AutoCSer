using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列 列表缓存
    /// </summary>
    public partial class QueueList<valueType, modelType, counterKeyType, keyType>
    {
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private sealed class ArrayAwaiter : Threading.Awaiter<valueType[]>
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private QueueList<valueType, modelType, counterKeyType, keyType> queue;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="key"></param>
            internal ArrayAwaiter(QueueList<valueType, modelType, counterKeyType, keyType> queue, keyType key)
            {
                this.queue = queue;
                this.key = key;
            }
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    Value = new LeftArray<valueType>(queue.getList(ref connection, key)).GetArray();
                }
                finally
                {
                    if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
                }
                return LinkNext;
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public Threading.Awaiter<valueType[]> GetArrayAwaiter(keyType key)
        {
            ArrayAwaiter task = new ArrayAwaiter(this, key);
            counter.SqlTable.AddQueue(task);
            return task;
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数据集合</returns>
        public async Task<LeftArray<valueType>> GetFindArrayTask(keyType key, Func<valueType, bool> isValue)
        {
            valueType[] array = await GetArrayAwaiter(key);
            int index = 0;
            foreach (valueType value in array)
            {
                if (isValue(value)) array[index++] = value;
            }
            return new LeftArray<valueType>(index, array);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class FindAwaiter : Threading.Awaiter<valueType>
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private QueueList<valueType, modelType, counterKeyType, keyType> queue;
            /// <summary>
            /// 数据匹配器
            /// </summary>
            private Func<valueType, bool> isValue;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="key"></param>
            /// <param name="isValue"></param>
            internal FindAwaiter(QueueList<valueType, modelType, counterKeyType, keyType> queue, keyType key, Func<valueType, bool> isValue)
            {
                this.queue = queue;
                this.key = key;
                this.isValue = isValue;
            }
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    Value = new LeftArray<valueType>(queue.getList(ref connection, key)).FirstOrDefault(isValue);
                }
                finally
                {
                    if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) new Task(continuation).Start();
                }
                return LinkNext;
            }
        }
        /// <summary>
        /// 获取一个匹配数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止数据库与锁操作</param>
        /// <returns>匹配数据,失败返回null</returns>
        public Threading.Awaiter<valueType> FirstOrDefaultAwaiter(keyType key, Func<valueType, bool> isValue)
        {
            if (isValue == null) throw new ArgumentNullException();
            FindAwaiter task = new FindAwaiter(this, key, isValue);
            counter.SqlTable.AddQueue(task);
            return task;
        }
    }
}
