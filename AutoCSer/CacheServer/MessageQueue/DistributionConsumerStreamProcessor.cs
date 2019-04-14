using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息分发客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class DistributionConsumerStreamProcessor<valueType> : Abstract.DistributionConsumerStreamProcessor
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        private readonly ValueData.GetData<valueType> getValue;
        /// <summary>
        /// 数据类型
        /// </summary>
        private readonly ValueData.DataType dataType;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        internal DistributionConsumerStreamProcessor(Abstract.DistributionConsumer consumer, Action<valueType> onMessage) : base(consumer)
        {
            this.onMessage = onMessage;
            getValue = ValueData.Data<valueType>.GetData;
            dataType = ValueData.Data<valueType>.DataType;
        }
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="dataType">数据类型</param>
        internal DistributionConsumerStreamProcessor(Abstract.DistributionConsumer consumer, Action<valueType> onMessage, ValueData.GetData<valueType> getValue, ValueData.DataType dataType) : base(consumer)
        {
            this.onMessage = onMessage;
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
                if (isSendCount == 0 && consumer.IsProcessor(this)) AutoCSer.Threading.TimerTask.Default.Add(Start, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
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
                    //FreeMessageKeepCallback();
                    AutoCSer.Threading.TimerTask.Default.Add(reStart, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
                }
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="message"></param>
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetMessage(ulong identity, ref ValueData.Data message)
        {
            if (message.Type == dataType)
            {
                if (consumer.IsProcessor(this))
                {
                    valueType value = getValue(ref message);
                    do
                    {
                        try
                        {
                            onMessage(value);
                            consumer.SetDequeueIdentity(identity);
                            return true;
                        }
                        catch (Exception error)
                        {
                            consumer.Log.Add(Log.LogType.Error, error);
                            System.Threading.Thread.Sleep(1);
                        }
                    }
                    while (true);
                }
            }
            else if (message.Type == ValueData.DataType.Null) return true;
            return false;
        }
    }
}
