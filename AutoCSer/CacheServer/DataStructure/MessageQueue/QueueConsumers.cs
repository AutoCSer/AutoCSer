using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 多消费者队列消费节点（有序消费）
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public sealed partial class QueueConsumers<valueType> : Abstract.MessageQueue<valueType>
    {
        /// <summary>
        /// 多消费者队列消费节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private QueueConsumers(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.MessageQueueConsumers);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Parameter.Value getDequeueIdentityNode(int readerIndex, Cache.MessageQueue.ReaderConfig config)
        {
            Parameter.Value node = new Parameter.Value(new Parameter.Value(this, readerIndex), OperationParameter.OperationType.MessageQueueGetDequeueIdentity);
            node.Parameter.SetJson(config);
            return node;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        public ReturnValue<ulong> GetDequeueIdentity(int readerIndex, Cache.MessageQueue.ReaderConfig config)
        {
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) return Client.GetULong(ClientDataStructure.Client.MasterQueryAsynchronous(getDequeueIdentityNode(readerIndex, config)));
            return new ReturnValue<ulong> { Type = ReturnType.MessageQueueReaderIndexOutOfRange };
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void GetDequeueIdentity(int readerIndex, Cache.MessageQueue.ReaderConfig config, Action<ReturnValue<ulong>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) ClientDataStructure.Client.MasterQueryAsynchronous(getDequeueIdentityNode(readerIndex, config), onEnqueue); 
            else onEnqueue(new ReturnValue<ulong> { Type = ReturnType.MessageQueueReaderIndexOutOfRange });
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void GetDequeueIdentity(int readerIndex, Cache.MessageQueue.ReaderConfig config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) ClientDataStructure.Client.MasterQueryAsynchronous(getDequeueIdentityNode(readerIndex, config), onEnqueue);
            else onEnqueue(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Value = new ReturnParameter(ReturnType.MessageQueueReaderIndexOutOfRange) });
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetDequeueIdentityStream(int readerIndex, Cache.MessageQueue.ReaderConfig config, Action<ReturnValue<ulong>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) ClientDataStructure.Client.MasterQueryAsynchronousStream(getDequeueIdentityNode(readerIndex, config), onEnqueue);
            else onEnqueue(new ReturnValue<ulong> { Type = ReturnType.MessageQueueReaderIndexOutOfRange });
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetDequeueIdentityStream(int readerIndex, Cache.MessageQueue.ReaderConfig config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) ClientDataStructure.Client.MasterQueryAsynchronousStream(getDequeueIdentityNode(readerIndex, config), onEnqueue);
            else onEnqueue(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Value = new ReturnParameter(ReturnType.MessageQueueReaderIndexOutOfRange) });
        }

        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<ulong> GetDequeueIdentity(IConvertible readerIndex, Cache.MessageQueue.ReaderConfig config)
        {
            return GetDequeueIdentity(readerIndex.ToInt32(null), config);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetDequeueIdentity(IConvertible readerIndex, Cache.MessageQueue.ReaderConfig config, Action<ReturnValue<ulong>> onEnqueue)
        {
            GetDequeueIdentity(readerIndex.ToInt32(null), config, onEnqueue);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetDequeueIdentity(IConvertible readerIndex, Cache.MessageQueue.ReaderConfig config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            GetDequeueIdentity(readerIndex.ToInt32(null), config, onEnqueue);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetDequeueIdentityStream(IConvertible readerIndex, Cache.MessageQueue.ReaderConfig config, Action<ReturnValue<ulong>> onEnqueue)
        {
            GetDequeueIdentityStream(readerIndex.ToInt32(null), config, onEnqueue);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetDequeueIdentityStream(IConvertible readerIndex, Cache.MessageQueue.ReaderConfig config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            GetDequeueIdentityStream(readerIndex.ToInt32(null), config, onEnqueue);
        }
        
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Parameter.Value getMessageNode(int readerIndex, ulong identity)
        {
            return new Parameter.Value(new Parameter.Value(this, readerIndex), OperationParameter.OperationType.MessageQueueDequeue, identity);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(int readerIndex, ulong identity, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallback(getMessageNode(readerIndex, identity), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(int readerIndex, ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallback(getMessageNode(readerIndex, identity), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(int readerIndex, ulong identity, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallbackStream(getMessageNode(readerIndex, identity), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(int readerIndex, ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallbackStream(getMessageNode(readerIndex, identity), onGet);
        }
        
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(IConvertible readerIndex, ulong identity, Action<ReturnValue<valueType>> onGet)
        {
            return GetMessage(readerIndex.ToInt32(null), identity, onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(IConvertible readerIndex, ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            return GetMessage(readerIndex.ToInt32(null), identity, onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(IConvertible readerIndex, ulong identity, Action<ReturnValue<valueType>> onGet)
        {
            return GetMessageStream(readerIndex.ToInt32(null), identity, onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(IConvertible readerIndex, ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            return GetMessageStream(readerIndex.ToInt32(null), identity, onGet);
        }
        
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">确认已完成消息标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Parameter.Value getSetDequeueIdentityNode(int readerIndex, ulong identity)
        {
            return new Parameter.Value(new Parameter.Value(this, readerIndex), OperationParameter.OperationType.MessageQueueSetDequeueIdentity, identity);
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">确认已完成消息标识</param>
        public void SetDequeueIdentity(int readerIndex, ulong identity)
        {
            ClientDataStructure.Client.MasterQueryOnly(getSetDequeueIdentityNode(readerIndex, identity));
        }
        
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="identity">确认已完成消息标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetDequeueIdentity(IConvertible readerIndex, ulong identity)
        {
            SetDequeueIdentity(readerIndex.ToInt32(null), identity);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, QueueConsumers<valueType>> constructor;
#if NOJIT
        /// <summary>
        /// 创建多消费者队列消费节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static QueueConsumers<valueType> create(Abstract.Node parent)
        {
            return new QueueConsumers<valueType>(parent);
        }
#endif
        static QueueConsumers()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, QueueConsumers<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, QueueConsumers<valueType>>), typeof(QueueConsumers<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, QueueConsumers<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(QueueConsumers<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
