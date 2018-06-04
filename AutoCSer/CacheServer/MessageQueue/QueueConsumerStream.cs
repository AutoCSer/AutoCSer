using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class QueueConsumerStream<valueType> : Abstract.QueueConsumer
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        public QueueConsumerStream(DataStructure.MessageQueue.QueueConsumer<valueType> messageQueue, Action<valueType> onMessage, Config.QueueConsumer config, AutoCSer.Log.ILog log = null) : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, messageQueue)
        {
            if (onMessage == null) throw new ArgumentNullException();
            this.onMessage = onMessage;
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="log">日志处理</param>
        public QueueConsumerStream(DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, Action<valueType> onMessage, Config.QueueConsumer config, int readerIndex, AutoCSer.Log.ILog log = null) : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, getReaderIndexNode(messageQueue, readerIndex))
        {
            if (onMessage == null) throw new ArgumentNullException();
            this.onMessage = onMessage;
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="log">日志处理</param>
        public QueueConsumerStream(DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, Action<valueType> onMessage, Config.QueueConsumer config, IConvertible readerIndex, AutoCSer.Log.ILog log = null) : this(messageQueue, onMessage, config, readerIndex.ToInt32(null), log) { }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="socket"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketBase socket)
        {
            Abstract.QueueConsumerStreamProcessor oldProcesser = Processor;
            if (socket != null) createProcessor();
            else Processor = null;
            if (oldProcesser != null) oldProcesser.Free();
        }
        /// <summary>
        /// 创建处理器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void createProcessor()
        {
            QueueConsumerStreamProcessor<valueType> processor = new QueueConsumerStreamProcessor<valueType>(this, onMessage);
            Processor = processor;
            processor.Start();
        }
        /// <summary>
        /// 重启处理器
        /// </summary>
        protected override void reStartProcessor()
        {
            createProcessor();
        }
    }
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class QueueConsumerStream<nodeType, valueType> : Abstract.QueueConsumer
        where nodeType : DataStructure.Abstract.Node
    {
        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        private readonly ValueData.GetData<valueType> getValue;
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public QueueConsumerStream(DataStructure.MessageQueue.QueueConsumer<nodeType> messageQueue, Action<valueType> onMessage, Config.QueueConsumer config, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, messageQueue)
        {
            if (getValue == null || onMessage == null) throw new ArgumentNullException();
            this.getValue = getValue;
            this.onMessage = onMessage;
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public QueueConsumerStream(DataStructure.MessageQueue.QueueConsumers<nodeType> messageQueue, Action<valueType> onMessage, Config.QueueConsumer config, int readerIndex, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, getReaderIndexNode(messageQueue, readerIndex))
        {
            if (getValue == null || onMessage == null) throw new ArgumentNullException();
            this.getValue = getValue;
            this.onMessage = onMessage;
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public QueueConsumerStream(DataStructure.MessageQueue.QueueConsumers<nodeType> messageQueue, Action<valueType> onMessage, Config.QueueConsumer config, IConvertible readerIndex, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : this(messageQueue, onMessage, config, readerIndex.ToInt32(null), getValue, log) { }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="socket"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketBase socket)
        {
            Abstract.QueueConsumerStreamProcessor oldProcesser = Processor;
            if (socket != null) createProcessor();
            else Processor = null;
            if (oldProcesser != null) oldProcesser.Free();
        }
        /// <summary>
        /// 创建处理器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void createProcessor()
        {
            QueueConsumerStreamProcessor<valueType> processor = new QueueConsumerStreamProcessor<valueType>(this, onMessage, getValue, ValueData.Data<nodeType>.DataType);
            Processor = processor;
            processor.Start();
        }
        /// <summary>
        /// 重启处理器
        /// </summary>
        protected override void reStartProcessor()
        {
            createProcessor();
        }
    }
}
