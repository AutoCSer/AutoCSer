using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列缓存(不适应于update/delete)
    /// </summary>
    public sealed partial class MemberQueueList<valueType, modelType, memberCacheType, keyType>
    {
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private sealed class GetByKeyAwaiter : Threading.Awaiter<LeftArray<valueType>>
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private MemberQueueList<valueType, modelType, memberCacheType, keyType> queue;
            /// <summary>
            /// 成员缓存
            /// </summary>
            private memberCacheType node;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="node"></param>
            /// <param name="key"></param>
            internal GetByKeyAwaiter(MemberQueueList<valueType, modelType, memberCacheType, keyType> queue, memberCacheType node, keyType key)
            {
                this.queue = queue;
                this.node = node;
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
                    Value = queue.get(ref connection, node, key);
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
        public Threading.Awaiter<LeftArray<valueType>> GetAwaiter(keyType key)
        {
            memberCacheType node = getTarget(key);
            if (node != null)
            {
                GetByKeyAwaiter task = new GetByKeyAwaiter(this, node, key);
                sqlTable.AddQueue(task);
                return task;
            }
            return new Threading.Awaiter<LeftArray<valueType>>.NullValue();
        }
    }
}
