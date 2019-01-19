using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue.Abstract
{
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    public abstract class Consumer : IDisposable
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        private static readonly ConsumerConfig defaultConfig = MessageQueue.ConfigLoader.GetUnion(typeof(ConsumerConfig)).ConsumerConfig ?? new ConsumerConfig();

        /// <summary>
        /// 队列数据 读取配置
        /// </summary>
        internal readonly ConsumerConfig Config;
        /// <summary>
        /// 日志处理
        /// </summary>
        internal readonly AutoCSer.Log.ILog Log;
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        private readonly DataStructure.Parameter.Value getDequeueIdentityNode;
        /// <summary>
        /// 获取数据
        /// </summary>
        private readonly DataStructure.Parameter.Value getMessageNode;
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        private readonly DataStructure.Parameter.Value setDequeueIdentityNode;
        /// <summary>
        /// TCP 客户端
        /// </summary>
        private readonly MasterServer.TcpInternalClient client;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private AutoCSer.Net.TcpServer.CheckSocketVersion checkSocketVersion;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        internal volatile ConsumerStreamProcessor Processor;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <param name="readerIndexNode"></param>
        protected Consumer(MasterServer.TcpInternalClient client, ConsumerConfig config, AutoCSer.Log.ILog log, DataStructure.Abstract.Node readerIndexNode)
        {
            if (client == null) throw new InvalidOperationException();
            this.client = client;
            if (config == null) Config = defaultConfig;
            else
            {
                config.Format();
                Config = config;
            }
            Log = log ?? client._TcpClient_.Log;

            getDequeueIdentityNode = new DataStructure.Parameter.Value(readerIndexNode, OperationParameter.OperationType.MessageQueueGetDequeueIdentity);
            getDequeueIdentityNode.Parameter.SetJson((Cache.MessageQueue.ReaderConfig)Config);
            getMessageNode = new DataStructure.Parameter.Value(readerIndexNode, OperationParameter.OperationType.MessageQueueDequeue);
            setDequeueIdentityNode = new DataStructure.Parameter.Value(readerIndexNode, OperationParameter.OperationType.MessageQueueSetDequeueIdentity);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                (checkSocketVersion as IDisposable).Dispose();
                if (Processor != null) Processor.Free();
            }
        }
        /// <summary>
        /// 判断是否当前处理器
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsProcessor(ConsumerStreamProcessor processor)
        {
            return ReferenceEquals(Processor, processor) && isDisposed == 0;
        }
        /// <summary>
        /// 重启处理器
        /// </summary>
        /// <param name="processor"></param>
        internal void ReStartProcessor(ConsumerStreamProcessor processor)
        {
            if (IsProcessor(processor))
            {
                object setSocketLock = client._TcpClient_.OnSocketLock;
                Monitor.Enter(setSocketLock);
                try
                {
                    if (IsProcessor(processor))
                    {
                        Processor = null;
                        reStartProcessor();
                    }
                }
                finally { Monitor.Exit(setSocketLock); }
            }
            processor.Free();
        }
        /// <summary>
        /// 重启处理器
        /// </summary>
        protected abstract void reStartProcessor();
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="onClientSocket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setCheckSocketVersion(Action<AutoCSer.Net.TcpServer.ClientSocketEventParameter> onClientSocket)
        {
            checkSocketVersion = client._TcpClient_.CreateCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetDequeueIdentity(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            client.QueryAsynchronousStream(new OperationParameter.QueryNode { Node = getDequeueIdentityNode }, onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">读取消息起始标识</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback GetMessage(ulong identity, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            getMessageNode.Parameter.Set(identity);
            return client.QueryKeepCallbackStream(new OperationParameter.QueryNode { Node = getMessageNode }, onGet);
        }
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetDequeueIdentity(ulong identity)
        {
            setDequeueIdentityNode.Parameter.Set(identity);
            client.QueryOnly(new OperationParameter.QueryNode { Node = setDequeueIdentityNode });
        }

        /// <summary>
        /// 获取读取编号节点
        /// </summary>
        /// <param name="messageQueue"></param>
        /// <param name="readerIndex"></param>
        /// <returns></returns>
        protected static DataStructure.Abstract.Node getReaderIndexNode(DataStructure.Abstract.Node messageQueue, int readerIndex)
        {
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) return new DataStructure.Parameter.Value(messageQueue, readerIndex);
            throw new IndexOutOfRangeException();
        }
    }
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class Consumer<valueType> : Consumer
    {
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        protected Consumer(DataStructure.Abstract.MessageQueue<valueType> messageQueue, ConsumerConfig config, AutoCSer.Log.ILog log) : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, messageQueue) { }
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <param name="readerIndex">读取编号</param>
        protected Consumer(DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, ConsumerConfig config, AutoCSer.Log.ILog log, int readerIndex) 
            : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, getReaderIndexNode(messageQueue, readerIndex)) { }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setCheckSocketVersion()
        {
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            ConsumerStreamProcessor oldProcesser = Processor;
            switch (parameter.Type)
            {
                case AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket: createProcessor(); break;
                default: Processor = null; break;
            }
            if (oldProcesser != null) oldProcesser.Free();
        }
        /// <summary>
        /// 创建处理器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void createProcessor()
        {
            ConsumerProcessor<valueType> processor = CreateProcessor();
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
        /// <summary>
        /// 创建消息队列 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal abstract ConsumerProcessor<valueType> CreateProcessor();
    }
    /// <summary>
    /// 消息队列 客户端消费者
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class Consumer<nodeType, valueType> : Consumer
        where nodeType : DataStructure.Abstract.Node
    {
        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        internal readonly ValueData.GetData<valueType> GetValue;
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <param name="getValue">获取参数数据委托</param>
        protected Consumer(DataStructure.Abstract.MessageQueue<nodeType> messageQueue, ConsumerConfig config, AutoCSer.Log.ILog log, ValueData.GetData<valueType> getValue) : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, messageQueue)
        {
            if (getValue == null) throw new ArgumentNullException();
            GetValue = getValue;
        }
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        /// <param name="messageQueue">队列消费节点</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <param name="readerIndex">读取编号</param>
        /// <param name="getValue">获取参数数据委托</param>
        protected Consumer(DataStructure.MessageQueue.QueueConsumers<nodeType> messageQueue, ConsumerConfig config, AutoCSer.Log.ILog log, int readerIndex, ValueData.GetData<valueType> getValue)
            : base(messageQueue.ClientDataStructure.Client.MasterClient, config, log, getReaderIndexNode(messageQueue, readerIndex))
        {
            if (getValue == null) throw new ArgumentNullException();
            GetValue = getValue;
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setCheckSocketVersion()
        {
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            ConsumerStreamProcessor oldProcesser = Processor;
            switch (parameter.Type)
            {
                case AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket: createProcessor(); break;
                default: Processor = null; break;
            }
            if (oldProcesser != null) oldProcesser.Free();
        }
        /// <summary>
        /// 创建处理器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void createProcessor()
        {
            ConsumerProcessor<valueType> processor = CreateProcessor();
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
        /// <summary>
        /// 创建消息队列 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal abstract ConsumerProcessor<valueType> CreateProcessor();
    }
}
