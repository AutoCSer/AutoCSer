﻿using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    public sealed partial class Queue<valueType, modelType, keyType>
    {
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private sealed class GetByKeyAwaiter : Threading.Awaiter<ReturnValue<valueType>>
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private Queue<valueType, modelType, keyType> queue;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="key"></param>
            internal GetByKeyAwaiter(Queue<valueType, modelType, keyType> queue, keyType key)
            {
                this.queue = queue;
                this.key = key;
            }
            /// <summary>
            /// 获取缓存数据
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    Value = queue.get(ref connection, key);
                }
                catch (Exception error)
                {
                    Value = error;
                }
                finally
                {
                    IsCompleted = true;
                    if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
                    {
                        continuation();
                    }
                }
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public Threading.Awaiter<ReturnValue<valueType>> GetAwaiter(keyType key)
        {
            GetByKeyAwaiter task = new GetByKeyAwaiter(this, key);
            counter.SqlTable.AddQueue(task);
            return task;
        }
    }
}
