using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息队列 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class ConsumerStreamProcessor<valueType> : Abstract.ConsumerStreamProcessor
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
        /// 消息处理时间
        /// </summary>
        private DateTime messageTime;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        internal ConsumerStreamProcessor(Abstract.Consumer consumer, Action<valueType> onMessage) : base(consumer)
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
        internal ConsumerStreamProcessor(Abstract.Consumer consumer, Action<valueType> onMessage, ValueData.GetData<valueType> getValue, ValueData.DataType dataType) : base(consumer)
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
                consumer.GetDequeueIdentity(onGetDequeueIdentity);
            }
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="identity"></param>
        private void onGetDequeueIdentity(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> identity)
        {
            int isIdentity = 0;
            try
            {
                if (identity.Type == Net.TcpServer.ReturnType.Success && onGetDequeueIdentity(ref identity.Value)) isIdentity = 1;
            }
            finally
            {
                if (isIdentity == 0 && consumer.IsProcessor(this)) AutoCSer.Threading.TimerTask.Default.Add(Start, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
            }
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetDequeueIdentity(ref ReturnParameter identity)
        {
            switch (identity.Parameter.ReturnType)
            {
                case ReturnType.Success:
                    if (identity.Parameter.Type == ValueData.DataType.ULong && consumer.IsProcessor(this))
                    {
                        messageTime = Date.NowTime.Now;
                        sendClientCount = (consumer.Config.SendClientCount >> 1) | 1U;
                        getMessageKeepCallback = consumer.GetMessage(this.identity = identity.Parameter.Int64.ULong, onGetMessage);
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
        private void onGetMessage(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> message)
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
        private bool onGetMessage(ref ReturnParameter message)
        {
            return message.Parameter.ReturnType == ReturnType.Success && onGetMessage(ref message.Parameter);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetMessage(ref ValueData.Data message)
        {
            if (message.Type == dataType)
            {
                if (consumer.IsProcessor(this))
                {
                    valueType value = getValue(ref message);
                    ONMESSAGE:
                    try
                    {
                        onMessage(value);
                    }
                    catch (Exception error)
                    {
                        consumer.Log.Add(Log.LogType.Error, error);
                        System.Threading.Thread.Sleep(1);
                        goto ONMESSAGE;
                    }
                    ++identity;
                    if (messageTime != Date.NowTime.Now || --sendClientCount == 0)
                    {
                        consumer.SetDequeueIdentity(identity);
                        messageTime = Date.NowTime.Now;
                        sendClientCount = (consumer.Config.SendClientCount >> 1) | 1U;
                    }
                    return true;
                }
            }
            else if (message.Type == ValueData.DataType.Null) return true;
            return false;
        }
    }
}
