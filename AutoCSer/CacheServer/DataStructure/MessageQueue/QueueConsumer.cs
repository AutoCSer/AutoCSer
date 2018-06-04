using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 队列消费节点（有序消费）
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public sealed partial class QueueConsumer<valueType> : Abstract.MessageQueue<valueType>
    {
        /// <summary>
        /// 队列消费节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private QueueConsumer(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.MessageQueueConsumer);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryULongAsynchronous GetDequeueIdentityNode(Cache.MessageQueue.Config.QueueReader config)
        {
            DataStructure.Parameter.QueryULongAsynchronous node = new DataStructure.Parameter.QueryULongAsynchronous(this, OperationParameter.OperationType.MessageQueueGetDequeueIdentity);
            node.Parameter.SetJson(config);
            return node;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        public ReturnValue<ulong> GetDequeueIdentity(Cache.MessageQueue.Config.QueueReader config)
        {
            return Client.GetULong(ClientDataStructure.Client.QueryAsynchronous(GetDequeueIdentityNode(config)));
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void GetDequeueIdentity(Cache.MessageQueue.Config.QueueReader config, Action<ReturnValue<ulong>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryAsynchronous(GetDequeueIdentityNode(config), onEnqueue);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void GetDequeueIdentity(Cache.MessageQueue.Config.QueueReader config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryAsynchronous(GetDequeueIdentityNode(config), onEnqueue);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetDequeueIdentityStream(Cache.MessageQueue.Config.QueueReader config, Action<ReturnValue<ulong>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryAsynchronousStream(GetDequeueIdentityNode(config), onEnqueue);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetDequeueIdentityStream(Cache.MessageQueue.Config.QueueReader config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryAsynchronousStream(GetDequeueIdentityNode(config), onEnqueue);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">读取消息起始标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryKeepCallbackReturnValue<valueType> GetMessageNode(ulong identity)
        {
            Parameter.QueryKeepCallbackReturnValue<valueType> node = new Parameter.QueryKeepCallbackReturnValue<valueType>(this, OperationParameter.OperationType.MessageQueueDequeue);
            node.Parameter.Set(identity);
            return node;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(ulong identity, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallback(GetMessageNode(identity), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallback(GetMessageNode(identity), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(ulong identity, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallbackStream(GetMessageNode(identity), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallbackStream(GetMessageNode(identity), onGet);
        }

        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryOnly GetSetDequeueIdentityNode(ulong identity)
        {
            DataStructure.Parameter.QueryOnly node = new DataStructure.Parameter.QueryOnly(this, OperationParameter.OperationType.MessageQueueSetDequeueIdentity);
            node.Parameter.Set(identity);
            return node;
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        public void SetDequeueIdentity(ulong identity)
        {
            ClientDataStructure.Client.QueryOnly(GetSetDequeueIdentityNode(identity));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, QueueConsumer<valueType>> constructor;
#if NOJIT
        /// <summary>
        /// 创建消息队列节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static QueueConsumer<valueType> create(Abstract.Node parent)
        {
            return new QueueConsumer<valueType>(parent);
        }
#endif
        static QueueConsumer()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, QueueConsumer<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, QueueConsumer<valueType>>), typeof(QueueConsumer<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, QueueConsumer<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(QueueConsumer<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
