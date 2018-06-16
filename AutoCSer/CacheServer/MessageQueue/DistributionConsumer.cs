using System;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息分发 客户端消费者
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class DistributionConsumer<valueType> : Abstract.DistributionConsumer<valueType>
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消费分发节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        public DistributionConsumer(DataStructure.MessageQueue.Distributor<valueType> node, Action<valueType> onMessage, DistributionConsumerConfig config, AutoCSer.Log.ILog log = null) : base(node, config, log)
        {
            if (onMessage == null) throw new ArgumentNullException();
            this.onMessage = onMessage;
            setCheckSocketVersion();
        }
        /// <summary>
        /// 创建消息队列 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal override Abstract.DistributionConsumerProcessor<valueType> CreateProcessor()
        {
            return new DistributionConsumerProcessor<valueType>(this, onMessage);
        }
    }
    /// <summary>
    /// 消息分发 客户端消费者
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class DistributionConsumer<nodeType, valueType> : Abstract.DistributionConsumer<nodeType, valueType>
        where nodeType : DataStructure.Abstract.Node
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        /// <param name="node">消费分发节点</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="log">日志处理</param>
        public DistributionConsumer(DataStructure.MessageQueue.Distributor<nodeType> node, Action<valueType> onMessage, DistributionConsumerConfig config, ValueData.GetData<valueType> getValue, AutoCSer.Log.ILog log = null) : base(node, config, log, getValue)
        {
            if (onMessage == null) throw new ArgumentNullException();
            this.onMessage = onMessage;
            setCheckSocketVersion();
        }
        /// <summary>
        /// 创建消息队列 客户端消费者 处理器
        /// </summary>
        /// <returns></returns>
        internal override Abstract.DistributionConsumerProcessor<valueType> CreateProcessor()
        {
            return new DistributionConsumerProcessor<valueType>(this, GetValue, ValueData.Data<nodeType>.DataType, onMessage);
        }
    }
}
