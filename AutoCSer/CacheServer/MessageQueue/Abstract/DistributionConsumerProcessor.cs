using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue.Abstract
{
    /// <summary>
    /// 消息分发 客户端消费者 处理器
    /// </summary>
    internal abstract class DistributionConsumerProcessor : DistributionConsumerStreamProcessor
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        protected Action messageHandle;
        /// <summary>
        /// 当前处理消息索引位置
        /// </summary>
        protected int messageIndex;
        /// <summary>
        /// 当前消息写入索引位置
        /// </summary>
        protected volatile int messageWriteIndex;
        /// <summary>
        /// 是否正在操作队列消息
        /// </summary>
        protected int isRead;
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息分发 客户端消费者</param>
        internal DistributionConsumerProcessor(DistributionConsumer consumer) : base(consumer) { }
    }
    /// <summary>
    /// 消息分发 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal abstract class DistributionConsumerProcessor<valueType> : DistributionConsumerProcessor
    {
        /// <summary>
        /// 消息集合
        /// </summary>
        protected KeyValue<ulong, valueType>[] messages;
        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        private readonly ValueData.GetData<valueType> getValue;
        /// <summary>
        /// 数据类型
        /// </summary>
        private readonly ValueData.DataType dataType;
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        protected DistributionConsumerProcessor(DistributionConsumer consumer) : base(consumer)
        {
            getValue = ValueData.Data<valueType>.GetData;
            dataType = ValueData.Data<valueType>.DataType;
        }
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="dataType">数据类型</param>
        protected DistributionConsumerProcessor(DistributionConsumer consumer, ValueData.GetData<valueType> getValue, ValueData.DataType dataType) : base(consumer)
        {
            this.getValue = getValue;
            this.dataType = dataType;
        }
        /// <summary>
        /// 开始处理数据
        /// </summary>
        internal void Start()
        {
            if (consumer.IsProcessor(this))
            {
                consumer.GetSendCount(onGetSendCount);
            }
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="sendCount"></param>
        private void onGetSendCount(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> sendCount)
        {
            int isSendCount = 0;
            try
            {
                if (sendCount.Type == Net.TcpServer.ReturnType.Success && onGetSendCount(ref sendCount.Value)) isSendCount = 1;
            }
            finally
            {
                if (isSendCount == 0 && consumer.IsProcessor(this))
                {
                    messages = null;
                    AutoCSer.Threading.TimerTask.Default.Add(Start, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
                }
            }
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="sendCount"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetSendCount(ref ReturnParameter sendCount)
        {
            switch (sendCount.Parameter.ReturnType)
            {
                case ReturnType.Success:
                    if (sendCount.Parameter.Type == ValueData.DataType.UInt && consumer.IsProcessor(this))
                    {
                        messages = new KeyValue<ulong, valueType>[sendCount.Parameter.Int64.UInt + 1];
                        messageIndex = messageWriteIndex = 0;
                        getMessageKeepCallback = consumer.GetMessage(onGetMessage);
                        return true;
                    }
                    break;
            }
            return false;
        }
        /// <summary>
        /// 重启处理器
        /// </summary>
        private void reStart()
        {
            consumer.ReStartProcessor(this);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="message"></param>
        private void onGetMessage(AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter> message)
        {
            int isMessage = 0;
            try
            {
                if (message.Type == Net.TcpServer.ReturnType.Success && onGetMessage(ref message.Value)) isMessage = 1;
            }
            finally
            {
                if (isMessage == 0 && consumer.IsProcessor(this))
                {
                    messages = null;
                    //freeKeepCallback();
                    AutoCSer.Threading.TimerTask.Default.Add(reStart, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
                }
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetMessage(ref IdentityReturnParameter message)
        {
            return message.Parameter.ReturnType == ReturnType.Success && onGetMessage(message.Identity, ref message.Parameter);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetMessage(ulong identity, ref ValueData.Data message)
        {
            if (message.Type == dataType)
            {
                if (consumer.IsProcessor(this))
                {
                    messages[messageWriteIndex].Set(identity, getValue(ref message));
                    if (++messageWriteIndex == messages.Length) messageWriteIndex = 0;
                    if (Interlocked.CompareExchange(ref isRead, 1, 0) == 0) AutoCSer.Threading.ThreadPool.Tiny.FastStart(messageHandle);
                    return true;
                }
            }
            else if (message.Type == ValueData.DataType.Null) return true;
            return false;
        }
        /// <summary>
        /// 释放保持回调
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void freeKeepCallback()
        {
            messages = null;
            FreeMessageKeepCallback();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal override void Free()
        {
            freeKeepCallback();
        }
    }
}
