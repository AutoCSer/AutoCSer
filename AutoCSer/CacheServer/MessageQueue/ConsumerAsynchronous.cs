using System;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class ConsumerAsynchronous<valueType> : Abstract.Consumer<valueType>
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        internal readonly Action<valueType, Action> OnMessage;
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        public ConsumerAsynchronous(DataStructure.MessageQueue.QueueConsumer<valueType> messageQueue, Action<valueType, Action> onMessage, ConsumerConfig config, AutoCSer.Log.ILog log = null) : base(messageQueue, config, log)
        {
            if (onMessage == null) throw new ArgumentNullException();
            OnMessage = onMessage;
            setCheckSocketVersion();
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="log">日志处理</param>
        public ConsumerAsynchronous(DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, Action<valueType, Action> onMessage, ConsumerConfig config, int readerIndex, AutoCSer.Log.ILog log = null) : base(messageQueue, config, log, readerIndex)
        {
            if (onMessage == null) throw new ArgumentNullException();
            OnMessage = onMessage;
            setCheckSocketVersion();
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="log">日志处理</param>
        public ConsumerAsynchronous(DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, Action<valueType, Action> onMessage, ConsumerConfig config, IConvertible readerIndex, AutoCSer.Log.ILog log = null) : this(messageQueue, onMessage, config, readerIndex.ToInt32(null), log) { }
        /// <summary>
        /// 创建消息队列 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal override Abstract.ConsumerProcessor<valueType> CreateProcessor()
        {
            return new ConsumerProcessorAsynchronous<valueType>(this, OnMessage);
        }
    }
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class ConsumerAsynchronous<nodeType, valueType> : Abstract.Consumer<nodeType, valueType>
        where nodeType : DataStructure.Abstract.Node
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        internal readonly Action<valueType, Action> OnMessage;
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public ConsumerAsynchronous(DataStructure.MessageQueue.QueueConsumer<nodeType> messageQueue, Action<valueType, Action> onMessage, ConsumerConfig config, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : base(messageQueue, config, log, getValue)
        {
            if (onMessage == null) throw new ArgumentNullException();
            OnMessage = onMessage;
            setCheckSocketVersion();
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public ConsumerAsynchronous(DataStructure.MessageQueue.QueueConsumers<nodeType> messageQueue, Action<valueType, Action> onMessage, ConsumerConfig config, int readerIndex, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : base(messageQueue, config, log, readerIndex, getValue)
        {
            if (onMessage == null) throw new ArgumentNullException();
            OnMessage = onMessage;
            setCheckSocketVersion();
        }
        /// <summary>
        /// 多消费者消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public ConsumerAsynchronous(DataStructure.MessageQueue.QueueConsumers<nodeType> messageQueue, Action<valueType, Action> onMessage, ConsumerConfig config, IConvertible readerIndex, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : this(messageQueue, onMessage, config, readerIndex.ToInt32(null), getValue, log) { }
        /// <summary>
        /// 创建消息队列 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal override Abstract.ConsumerProcessor<valueType> CreateProcessor()
        {
            return new ConsumerProcessorAsynchronous<valueType>(this, GetValue, ValueData.Data<nodeType>.DataType, OnMessage);
        }
    }
}

