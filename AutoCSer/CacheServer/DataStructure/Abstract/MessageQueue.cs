using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 消息队列节点
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public abstract partial class MessageQueue<valueType> : Node, IValueNode
    {
        /// <summary>
        /// 消息队列节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected MessageQueue(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return this;
        }

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<ShortPath.MessageQueue<valueType>> CreateShortPath()
        {
            if (Parent != null) return new ShortPath.MessageQueue<valueType>(this).Create();
            return new ReturnValue<ShortPath.MessageQueue<valueType>> { Type = ReturnType.CanNotCreateShortPath };
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPath(Action<ReturnValue<ShortPath.MessageQueue<valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.MessageQueue<valueType>(this).Create(onCreated);
            else onCreated(new ReturnValue<ShortPath.MessageQueue<valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPathStream(Action<ReturnValue<ShortPath.MessageQueue<valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.MessageQueue<valueType>(this).CreateStream(onCreated);
            else onCreated(new ReturnValue<ShortPath.MessageQueue<valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Abstract.Node getEnqueueNode(valueType value)
        {
            return ValueData.Data<valueType>.ToNode(this, value, OperationParameter.OperationType.MessageQueueEnqueue);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> Enqueue(valueType value)
        {
            return Client.GetBool(ClientDataStructure.Client.MasterQueryAsynchronous(getEnqueueNode(value)));
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
            ClientDataStructure.Client.MasterQueryAsynchronous(getEnqueueNode(value), onEnqueue);
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
            ClientDataStructure.Client.MasterQueryAsynchronous(getEnqueueNode(value), onEnqueue);
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
            ClientDataStructure.Client.MasterQueryAsynchronousStream(getEnqueueNode(value), onEnqueue);
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
            ClientDataStructure.Client.MasterQueryAsynchronousStream(getEnqueueNode(value), onEnqueue);
        }

        static MessageQueue()
        {
            ValueData.Data<valueType>.CheckValueType();
        }
    }
}
