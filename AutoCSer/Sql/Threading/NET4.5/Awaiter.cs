using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// awaiter
    /// </summary>
    /// <typeparam name="valueType">返回值类型</typeparam>
    public abstract class Awaiter<valueType> : Threading.LinkQueueTaskNode, INotifyCompletion
    {
        /// <summary>
        /// 空 awaiter
        /// </summary>
        internal class NullValue : Awaiter<valueType>
        {
            /// <summary>
            /// 空统计数量
            /// </summary>
            internal NullValue()
            {
                continuation = Pub.EmptyAction;
                isCompleted = true;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="connection"></param>
            /// <returns></returns>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        protected Action continuation;
        /// <summary>
        /// 完成状态
        /// </summary>
        protected bool isCompleted;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return isCompleted; } }
        /// <summary>
        /// 返回值
        /// </summary>
        protected valueType Value;
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType GetResult()
        {
            return Value;
        }
        /// <summary>
        /// 获取 awaiter
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Awaiter<valueType> GetAwaiter()
        {
            return this;
        }
    }
}
