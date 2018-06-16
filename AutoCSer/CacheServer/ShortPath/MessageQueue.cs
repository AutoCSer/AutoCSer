using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 消息队列节点 短路径
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class MessageQueue<valueType> : Node<MessageQueue<valueType>>
    {
        /// <summary>
        /// 数组节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal MessageQueue(DataStructure.Abstract.MessageQueue<valueType> node) : base(node) { }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetEnqueueNode(valueType value)
        {
            return ValueData.Data<valueType>.ToNode(this, value, OperationParameter.OperationType.MessageQueueEnqueue);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Enqueue(valueType value)
        {
            return Client.GetBool(Client.MasterQueryAsynchronous(GetEnqueueNode(value)));
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void Enqueue(valueType value, Action<ReturnValue<bool>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            Client.MasterQueryAsynchronous(GetEnqueueNode(value), onEnqueue);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void Enqueue(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            Client.MasterQueryAsynchronous(GetEnqueueNode(value), onEnqueue);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void EnqueueStream(valueType value, Action<ReturnValue<bool>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            Client.MasterQueryAsynchronousStream(GetEnqueueNode(value), onEnqueue);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void EnqueueStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            Client.MasterQueryAsynchronousStream(GetEnqueueNode(value), onEnqueue);
        }
    }
}
