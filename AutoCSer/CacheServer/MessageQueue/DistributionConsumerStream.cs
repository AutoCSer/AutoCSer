using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息分发 客户端消费者
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class DistributionConsumerStream<valueType> : Abstract.DistributionConsumer
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消息分发节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        public DistributionConsumerStream(DataStructure.MessageQueue.Distributor<valueType> node, Action<valueType> onMessage, DistributionConsumerConfig config, AutoCSer.Log.ILog log = null) : base(node, config, log)
        {
            if (onMessage == null) throw new ArgumentNullException();
            this.onMessage = onMessage;
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            Abstract.DistributionConsumerStreamProcessor oldProcesser = Processor;
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
            DistributionConsumerStreamProcessor<valueType> processor = new DistributionConsumerStreamProcessor<valueType>(this, onMessage);
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
    /// 消息分发 客户端消费者
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class DistributionConsumerStream<nodeType, valueType> : Abstract.DistributionConsumer
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
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消息分发节点</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public DistributionConsumerStream(DataStructure.MessageQueue.Distributor<nodeType> node, Action<valueType> onMessage, DistributionConsumerConfig config, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : base(node, config, log)
        {
            if (getValue == null || onMessage == null) throw new ArgumentNullException();
            this.getValue = getValue;
            this.onMessage = onMessage;
            setCheckSocketVersion(onClientSocket);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onClientSocket(AutoCSer.Net.TcpServer.ClientSocketEventParameter parameter)
        {
            Abstract.DistributionConsumerStreamProcessor oldProcesser = Processor;
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
            DistributionConsumerStreamProcessor<valueType> processor = new DistributionConsumerStreamProcessor<valueType>(this, onMessage, getValue, ValueData.Data<nodeType>.DataType);
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
