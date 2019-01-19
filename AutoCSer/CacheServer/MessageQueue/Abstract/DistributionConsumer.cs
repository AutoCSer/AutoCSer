using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue.Abstract
{
    /// <summary>
    /// 消息分发 客户端消费者
    /// </summary>
    public abstract class DistributionConsumer : IDisposable
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        private static readonly DistributionConsumerConfig defaultConfig = MessageQueue.ConfigLoader.GetUnion(typeof(DistributionConsumerConfig)).DistributionConsumerConfig ?? new DistributionConsumerConfig();

        /// <summary>
        /// 消息分发 读取配置
        /// </summary>
        internal readonly DistributionConsumerConfig Config;
        /// <summary>
        /// 日志处理
        /// </summary>
        internal readonly AutoCSer.Log.ILog Log;
        /// <summary>
        /// 消费分发节点
        /// </summary>
        private readonly DataStructure.Abstract.Node node;
        /// <summary>
        /// TCP 客户端
        /// </summary>
        private readonly MasterServer.TcpInternalClient client;
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        private readonly DataStructure.Parameter.Value getSendCountNode;
        /// <summary>
        /// 获取数据
        /// </summary>
        private readonly DataStructure.Parameter.Value getMessageNode;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private AutoCSer.Net.TcpServer.CheckSocketVersion checkSocketVersion;
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        internal volatile DistributionConsumerStreamProcessor Processor;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消费分发节点</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        protected DistributionConsumer(DataStructure.Abstract.Node node, DistributionConsumerConfig config, AutoCSer.Log.ILog log)
        {
            client = node.ClientDataStructure.Client.MasterClient;
            if (client == null) throw new InvalidOperationException();
            this.node = node;
            if (config == null) Config = defaultConfig;
            else
            {
                config.Format();
                Config = config;
            }
            Log = log ?? client._TcpClient_.Log;

            getSendCountNode = new DataStructure.Parameter.Value(node, OperationParameter.OperationType.MessageQueueGetDequeueIdentity);
            getSendCountNode.Parameter.SetJson((Cache.MessageQueue.DistributionConfig)Config);
            getMessageNode = new DataStructure.Parameter.Value(node, OperationParameter.OperationType.MessageQueueDequeue, config.LoopSendCount);
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
        internal bool IsProcessor(DistributionConsumerStreamProcessor processor)
        {
            return ReferenceEquals(Processor, processor) && isDisposed == 0;
        }
        /// <summary>
        /// 重启处理器
        /// </summary>
        /// <param name="processor"></param>
        internal void ReStartProcessor(DistributionConsumerStreamProcessor processor)
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
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetSendCount(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            client.QueryAsynchronousStream(new OperationParameter.QueryNode { Node = getSendCountNode }, onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.KeepCallback GetMessage(Action<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>> onGet)
        {
            return client.QueryKeepCallbackStream(new OperationParameter.QueryNode { Node = getMessageNode }, onGet);
        }
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetDequeueIdentity(ulong identity)
        {
            client.QueryOnly(new OperationParameter.QueryNode { Node = new DataStructure.Parameter.Value(node, OperationParameter.OperationType.MessageQueueSetDequeueIdentity, identity) });
        }
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        /// <param name="identitys">确认已完成消息标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetDequeueIdentity(ulong[] identitys)
        {
            DataStructure.Parameter.Value node = new DataStructure.Parameter.Value(this.node, OperationParameter.OperationType.MessageQueueSetDequeueIdentity);
            node.Parameter.SetBinary(identitys);
            client.QueryOnly(new OperationParameter.QueryNode { Node = node });
        }
    }
    /// <summary>
    /// 消息分发 客户端消费者
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class DistributionConsumer<valueType> : DistributionConsumer
    {
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消费分发节点</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        protected DistributionConsumer(DataStructure.MessageQueue.Distributor<valueType> node, DistributionConsumerConfig config, AutoCSer.Log.ILog log) : base(node, config, log) { }
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
            DistributionConsumerStreamProcessor oldProcesser = Processor;
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
            DistributionConsumerProcessor<valueType> processor = CreateProcessor();
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
        /// 创建消息分发 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal abstract DistributionConsumerProcessor<valueType> CreateProcessor();
    }
    /// <summary>
    /// 消息分发 客户端消费者
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class DistributionConsumer<nodeType, valueType> : DistributionConsumer
        where nodeType : DataStructure.Abstract.Node
    {
        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        internal readonly ValueData.GetData<valueType> GetValue;
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消费分发节点</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <param name="getValue">获取参数数据委托</param>
        protected DistributionConsumer(DataStructure.MessageQueue.Distributor<nodeType> node, DistributionConsumerConfig config, AutoCSer.Log.ILog log, ValueData.GetData<valueType> getValue) : base(node, config, log)
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
            DistributionConsumerStreamProcessor oldProcesser = Processor;
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
            DistributionConsumerProcessor<valueType> processor = CreateProcessor();
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
        internal abstract DistributionConsumerProcessor<valueType> CreateProcessor();
    }
}
