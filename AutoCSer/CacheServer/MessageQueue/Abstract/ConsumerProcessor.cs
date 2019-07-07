using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue.Abstract
{
    /// <summary>
    /// 消息队列 客户端消费者 处理器
    /// </summary>
    internal abstract class ConsumerProcessor : ConsumerStreamProcessor
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
        /// 写入消息标识
        /// </summary>
        protected volatile uint writeIdentity;
        /// <summary>
        /// 是否正在操作队列消息
        /// </summary>
        protected int isRead;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        internal ConsumerProcessor(Consumer consumer) : base(consumer) { }
    }
    /// <summary>
    /// 消息队列 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal abstract class ConsumerProcessor<valueType> : ConsumerProcessor
    {
        /// <summary>
        /// 消息集合
        /// </summary>
        protected valueType[] messages;
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
        protected ConsumerProcessor(Consumer consumer) : base(consumer)
        {
            getValue = ValueData.Data<valueType>.GetData;
            dataType = ValueData.Data<valueType>.DataType;
        }
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="dataType">数据类型</param>
        protected ConsumerProcessor(Consumer consumer, ValueData.GetData<valueType> getValue, ValueData.DataType dataType) : base(consumer)
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
                if (isIdentity == 0 && consumer.IsProcessor(this))
                {
                    messages = null;
                    AutoCSer.Threading.TimerTask.Default.Add(Start, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
                }
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
                        messages = new valueType[consumer.Config.SendClientCount];
                        this.identity = identity.Parameter.Int64.ULong;
                        messageIndex = messageWriteIndex = 0;
                        writeIdentity = (uint)this.identity;
                        sendClientCount = ((uint)messages.Length >> 1) | 1U;
                        getMessageKeepCallback = consumer.GetMessage(this.identity, onGetMessage);
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
        private bool onGetMessage(ref ReturnParameter message)
        {
            return message.Parameter.ReturnType == ReturnType.Success && onGetMessage(ref message.Parameter);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onGetMessage(ref ValueData.Data message)
        {
            if (message.Type == dataType)
            {
                if (consumer.IsProcessor(this))
                {
                    messages[messageWriteIndex] = getValue(ref message);
                    if (++messageWriteIndex == messages.Length) messageWriteIndex = 0;
                    ++writeIdentity;
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
