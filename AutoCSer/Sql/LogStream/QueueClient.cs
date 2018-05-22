using System;
using System.Reflection;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 先进先出队列缓存客户端
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    public sealed partial class QueueClient<valueType, modelType, keyType> : Client<valueType, modelType, keyType>
        where valueType : class, modelType, IMemberMapValueLink<valueType>
        where modelType : class
         where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 虚拟空日志流客户端
        /// </summary>
        public static readonly QueueClient<valueType, modelType, keyType> Null = new QueueClient<valueType, modelType, keyType>();
        /// <summary>
        /// 缓存数据
        /// </summary>
        private FifoPriorityQueue<RandomKey<keyType>, KeyValue<valueType, EventWaitHandle>> queue;
        /// <summary>
        /// 日志流处理保持回调
        /// </summary>
        private AutoCSer.Net.TcpServer.KeepCallback keepCallback;
        /// <summary>
        /// 访问锁
        /// </summary>
        private readonly object logLock;
        /// <summary>
        /// 最大数量
        /// </summary>
        private int maxCount;
        /// <summary>
        /// 是否错误
        /// </summary>
        private volatile int isError;
        /// <summary>
        /// 是否已经初步加载日志数据
        /// </summary>
        private bool isLog;
        /// <summary>
        /// 是否检测输出成员位图错误信息
        /// </summary>
        private bool isErrorMemberMap;
        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public valueType this[keyType key]
        {
            get { return Get(key); }
        }
        /// <summary>
        /// 虚拟空日志流客户端
        /// </summary>
        private QueueClient() : base() { }
        /// <summary>
        /// 日志流客户端
        /// </summary>
        /// <param name="getLog">获取日志数据委托</param>
        /// <param name="getKey">获取关键字委托</param>
        /// <param name="getValue">获取数据委托</param>
        /// <param name="getValueAwaiter">获取数据委托</param>
        /// <param name="maxCount">最大数量</param>
        /// <param name="log">日志处理</param>
        public QueueClient(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog
            , Func<modelType, keyType> getKey, Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue, Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter
            , int maxCount, ILog log = null)
            : base(getLog, getKey, getValue, getValueAwaiter, null, log)
        {
            this.maxCount = Math.Max(maxCount, 1);
            logLock = new object();
            queue = new FifoPriorityQueue<RandomKey<keyType>, KeyValue<valueType, EventWaitHandle>>();
            load();
        }
        /// <summary>
        /// 虚拟客户端创建先进先出队列缓存客户端
        /// </summary>
        /// <param name="tcpCallType">TCP 调用类型</param>
        /// <param name="getKey">获取关键字委托</param>
        /// <param name="maxCount">最大数量</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        public QueueClient<valueType, modelType, keyType> CreateNull(Type tcpCallType, Func<modelType, keyType> getKey, int maxCount, ILog log = null)
        {
            if (this == Null)
            {
                Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog = (Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback>)Delegate.CreateDelegate(typeof(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback>), tcpCallType.GetMethod("onSqlLogQueue", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>) }, null));
                Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue = (Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>>)Delegate.CreateDelegate(typeof(Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>>), tcpCallType.GetMethod("getSqlCache", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(keyType) }, null));
                Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter = (Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>>)Delegate.CreateDelegate(typeof(Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>>), tcpCallType.GetMethod("getSqlCacheAwaiter", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(keyType) }, null));
                return new QueueClient<valueType, modelType, keyType>(getLog, getKey, getValue, getValueAwaiter, maxCount, log);
            }
            return this;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        private void load()
        {
            try
            {
                keepCallback = getLog(onLog);
                if (keepCallback != null)
                {
                    Monitor.Enter(logLock);
                    queue.Clear();
                    isLog = true;
                    Monitor.Exit(logLock);
                }
            }
            catch (Exception error)
            {
                log.Add(AutoCSer.Log.LogType.Error, error);
            }
            this.error();
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        private void error()
        {
            if (Interlocked.CompareExchange(ref isError, 1, 0) == 0)
            {
                Monitor.Enter(logLock);
                queue.Clear();
                isLog = false;
                Monitor.Exit(logLock);
                AutoCSer.Threading.ThreadPool.TinyBackground.Start(onError);
                if (keepCallback != null) keepCallback.Dispose();
            }
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        private void onError()
        {
            Thread.Sleep(1000);
            load();
        }
        /// <summary>
        /// 日志流数据
        /// </summary>
        /// <param name="data"></param>
        private void onLog(AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data> data)
        {
            if (isError == 0)
            {
                if (data.Type == Net.TcpServer.ReturnType.Success)
                {
                    try
                    {
                        valueType value = data.Value.Value.Value;
                        RandomKey<keyType> key = getKey(data.Value.Value.Value);
                        KeyValue<valueType, EventWaitHandle> queueValue;
                        switch (data.Value.Type)
                        {
                            case LogType.Insert:
                                Monitor.Enter(logLock);
                                if (queue.TryGetOnly(key, out queueValue))
                                {
                                    queue.SetOnly(key, new KeyValue<valueType, EventWaitHandle>(value, null));
                                    Monitor.Exit(logLock);
                                }
                                else
                                {
                                    try
                                    {
                                        queue.Set(ref key, new KeyValue<valueType, EventWaitHandle>(value, null));
                                        if (queue.Count > maxCount) queue.UnsafePopNode();
                                    }
                                    finally { Monitor.Exit(logLock); }
                                }
                                return;
                            case LogType.Update:
                                Monitor.Enter(logLock);
                                if (queue.TryGetOnly(key, out queueValue))
                                {
                                    if (queueValue.Value == null)
                                    {
                                        try
                                        {
                                            AutoCSer.MemberCopy.Copyer<modelType>.Copy(queueValue.Key, value, data.Value.Value.MemberMap);
                                        }
                                        finally { Monitor.Exit(logLock); }
                                        if (data.Value.Value.MemberMap == null && !isErrorMemberMap)
                                        {
                                            isErrorMemberMap = true;
                                            log.Add(AutoCSer.Log.LogType.Warn, "客户端缓存数据缺少成员位图信息 " + typeof(valueType).fullName());
                                        }
                                        return;
                                    }
                                    queue.Remove(ref key, out queueValue);
                                }
                                Monitor.Exit(logLock);
                                MemberMapValueLinkPool<valueType>.PushNotNull(value);
                                return;
                            case LogType.Delete:
                                MemberMapValueLinkPool<valueType>.PushNotNull(value);
                                Monitor.Enter(logLock);
                                queue.Remove(ref key, out queueValue);
                                Monitor.Exit(logLock);
                                return;
                        }
                    }
                    catch (Exception error)
                    {
                        log.Add(AutoCSer.Log.LogType.Error, error);
                    }
                }
                this.error();
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public valueType Get(keyType key)
        {
            if (isLog)
            {
                RandomKey<keyType> randomKey = key;
                KeyValue<valueType, EventWaitHandle> value;
                TRYGET:
                Monitor.Enter(logLock);
                if (queue.TryGetValue(ref randomKey, out value))
                {
                    Monitor.Exit(logLock);
                    if (value.Value == null) return value.Key;
                    value.Value.WaitOne();
                    goto TRYGET;
                }
                try
                {
                    value.Value = new EventWaitHandle(false, EventResetMode.ManualReset);
                    queue.Set(ref randomKey, value);
                    if (queue.Count > maxCount) queue.UnsafePopNode();
                }
                finally { Monitor.Exit(logLock); }
                try
                {
                    value.Key = getValue(key);
                }
                finally
                {
                    KeyValue<valueType, EventWaitHandle> cacheValue;
                    Monitor.Enter(logLock);
                    if (queue.TryGetOnly(key, out cacheValue) && cacheValue.Value == value.Value)
                    {
                        try
                        {
                            queue.SetOnly(key, new KeyValue<valueType, EventWaitHandle>(value.Key, null));
                        }
                        finally { Monitor.Exit(logLock); }
                    }
                    else Monitor.Exit(logLock);
                    value.Value.Set();
                }
                return value.Key;
            }
            return getValue(key);
        }
    }
}
