using System;
using AutoCSer.CacheServer;
using AutoCSer.CacheServer.DataStructure.Value;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 缓存服务扩展方法
    /// </summary>
    public static partial class CacheServer
    {
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValue<Binary<valueType>> value)
        {
            return value.Value != null ? value.Value.Get(ref value) : new ReturnValue<valueType> { Type = value.Type, TcpReturnType = value.TcpReturnType };
        }
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValue<Json<valueType>> value)
        {
            return value.Value != null ? value.Value.Get(ref value) : new ReturnValue<valueType> { Type = value.Type, TcpReturnType = value.TcpReturnType };
        }

        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<Json<valueType>> messageQueue, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<Json<valueType>, valueType>(messageQueue, onMessage, config, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<Json<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<Json<valueType>> messageQueue, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<Json<valueType>, valueType>(messageQueue, onMessage, config, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<Json<valueType>> messageQueue, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Json<valueType>, valueType>(messageQueue, onMessage, config, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<Binary<valueType>> messageQueue, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<Binary<valueType>, valueType>(messageQueue, onMessage, config, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<Binary<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<Binary<valueType>> messageQueue, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<Binary<valueType>, valueType>(messageQueue, onMessage, config, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<Binary<valueType>> messageQueue, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Binary<valueType>, valueType>(messageQueue, onMessage, config, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<valueType> messageQueue, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<valueType>(messageQueue, onMessage, config, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<valueType> messageQueue, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<valueType>(messageQueue, onMessage, config, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumer<valueType> messageQueue, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<valueType>(messageQueue, onMessage, config, log);
        }

        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Json<valueType>> messageQueue, int readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<Json<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<Json<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Json<valueType>> messageQueue, int readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<Json<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Json<valueType>> messageQueue, int readerIndex, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Json<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Binary<valueType>> messageQueue, int readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<Binary<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<Binary<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Binary<valueType>> messageQueue, int readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<Binary<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Binary<valueType>> messageQueue, int readerIndex, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Binary<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, int readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<valueType>(messageQueue, onMessage, config, readerIndex, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, int readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<valueType>(messageQueue, onMessage, config, readerIndex, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, int readerIndex, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<valueType>(messageQueue, onMessage, config, readerIndex, log);
        }

        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Json<valueType>> messageQueue, IConvertible readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<Json<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<Json<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Json<valueType>> messageQueue, IConvertible readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<Json<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Json<valueType>> messageQueue, IConvertible readerIndex, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Json<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Binary<valueType>> messageQueue, IConvertible readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<Binary<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<Binary<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Binary<valueType>> messageQueue, IConvertible readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<Binary<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<Binary<valueType>> messageQueue, IConvertible readerIndex, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<Binary<valueType>, valueType>(messageQueue, onMessage, config, readerIndex, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.Consumer<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, IConvertible readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.Consumer<valueType>(messageQueue, onMessage, config, readerIndex, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerStream<valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, IConvertible readerIndex, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerStream<valueType>(messageQueue, onMessage, config, readerIndex, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="messageQueue">消息队列</param>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.QueueConsumers<valueType> messageQueue, IConvertible readerIndex, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.ConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.ConsumerAsynchronous<valueType>(messageQueue, onMessage, config, readerIndex, log);
        }

        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumer<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<Json<valueType>> distributor, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumer<Json<valueType>, valueType>(distributor, onMessage, config, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumerStream<Json<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<Json<valueType>> distributor, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumerStream<Json<valueType>, valueType>(distributor, onMessage, config, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumerAsynchronous<Json<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<Json<valueType>> distributor, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumerAsynchronous<Json<valueType>, valueType>(distributor, onMessage, config, Json<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumer<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<Binary<valueType>> distributor, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumer<Binary<valueType>, valueType>(distributor, onMessage, config, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumerStream<Binary<valueType>, valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<Binary<valueType>> distributor, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumerStream<Binary<valueType>, valueType>(distributor, onMessage, config, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumerAsynchronous<Binary<valueType>, valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<Binary<valueType>> distributor, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumerAsynchronous<Binary<valueType>, valueType>(distributor, onMessage, config, Binary<valueType>.GetData, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumer<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<valueType> distributor, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumer<valueType>(distributor, onMessage, config, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumerStream<valueType> CreateConsumerStream<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<valueType> distributor, Action<valueType> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumerStream<valueType>(distributor, onMessage, config, log);
        }
        /// <summary>
        /// 创建客户端消费者
        /// </summary>
        /// <param name="distributor">消息分发</param>
        /// <param name="onMessage">消息处理委托</param>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.MessageQueue.DistributionConsumerAsynchronous<valueType> CreateConsumer<valueType>(this AutoCSer.CacheServer.DataStructure.MessageQueue.Distributor<valueType> distributor, Action<valueType, Action> onMessage, AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig config, AutoCSer.Log.ILog log = null)
        {
            return new AutoCSer.CacheServer.MessageQueue.DistributionConsumerAsynchronous<valueType>(distributor, onMessage, config, log);
        }
    }
}
