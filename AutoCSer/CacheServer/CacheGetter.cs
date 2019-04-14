using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 获取缓存数据
    /// </summary>
    internal sealed class CacheGetter : AutoCSer.Threading.Link<CacheGetter>
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        internal readonly CacheManager Cache;
        /// <summary>
        /// 获取缓存数据回调委托
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter>, bool> onCache;
        /// <summary>
        /// 初始化操作超时时钟周期
        /// </summary>
        private readonly long timeoutTicks;
        /// <summary>
        /// 缓存快照
        /// </summary>
        private Snapshot.Cache snapshot;
        /// <summary>
        /// 当前操作数据队列
        /// </summary>
        private Buffer currentQueue;
        /// <summary>
        /// 操作数据队列
        /// </summary>
        private Buffer.Queue queue = new Buffer.Queue(new Buffer());
        /// <summary>
        /// 初始化操作超时
        /// </summary>
        private DateTime timeout;
        /// <summary>
        /// 缓存数据获取阶段
        /// </summary>
        private CacheGetStep step;
        /// <summary>
        /// 快照数据大小
        /// </summary>
        private int snapshotSize;
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="cache">缓存管理</param>
        /// <param name="onCache"></param>
        internal CacheGetter(CacheManager cache, Func<AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter>, bool> onCache)
        {
            this.Cache = cache;
            this.onCache = onCache;
            timeoutTicks = cache.Config.GetCacheLoadTimeoutSeconds * TimeSpan.TicksPerSecond;
            timeout = DateTime.MaxValue;
            cache.AppendWait(this);
        }
        /// <summary>
        /// 取消获取缓存数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal CacheGetter Cancel()
        {
            if (step != CacheGetStep.Error)
            {
                onCache(default(CacheReturnParameter));
                step = CacheGetStep.Error;
            }
            return LinkNext;
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void error()
        {
            Buffer.DisposeLink(currentQueue);
            snapshot = null;
            currentQueue = null;
            step = CacheGetStep.Error;
        }
        /// <summary>
        /// 缓存加载完毕开始获取数据
        /// </summary>
        internal void Start()
        {
            bool isStart = false;
            try
            {
                snapshot = new Snapshot.Cache(Cache, false);
                snapshotSize = snapshot.NextSize();
                timeout = Date.Now.AddTicks(timeoutTicks);
                if (snapshotSize != 0)
                {
                    step = CacheGetStep.Snapshot;
                    if (onCache(new CacheReturnParameter { Getter = this })) isStart = true;
                }
                else
                {
                    step = CacheGetStep.TcpQueue;
                    isStart = true;
                    GetQueue();
                }
            }
            finally
            {
                if (!isStart)
                {
                    error();
                    onCache(default(CacheReturnParameter));
                    Cache.NextGetter();
                }
            }
        }
        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <param name="stream"></param>
        internal unsafe void Serialize(UnmanagedStream stream)
        {
            int startIndex;
            switch (step)
            {
                case CacheGetStep.Snapshot:
                    startIndex = stream.AddSize(sizeof(int));
                    snapshot.CopyTo(stream);
                    do
                    {
                        snapshotSize = snapshot.NextSize();
                    }
                    while (snapshotSize != 0 && snapshot.TryCopyTo(stream));
                    timeout = Date.Now.AddTicks(timeoutTicks);
                    *(int*)(stream.Data.Byte + (startIndex - sizeof(int))) = stream.ByteSize - startIndex;
                    if (snapshotSize != 0)
                    {
                        if (!onCache(new CacheReturnParameter { Getter = this }))
                        {
                            error();
                            *(int*)(stream.Data.Byte + (startIndex - sizeof(int))) = AutoCSer.BinarySerialize.Serializer.NullValue;
                            stream.ByteSize = startIndex;
                        }
                        return;
                    }
                    snapshot = null;
                    step = CacheGetStep.TcpQueue;
                    Cache.TcpServer.CallQueue.Add(new ServerCall.CacheGetterGetQueue(this));
                    return;
                case CacheGetStep.TcpQueue:
                    startIndex = stream.AddSize(sizeof(int));
                    currentQueue.CopyTo(stream);
                    do
                    {
                        currentQueue = currentQueue.LinkNext;
                    }
                    while (currentQueue != null && currentQueue.TryCopyTo(stream));
                    timeout = Date.Now.AddTicks(timeoutTicks);
                    *(int*)(stream.Data.Byte + (startIndex - sizeof(int))) = stream.ByteSize - startIndex;
                    if (currentQueue != null)
                    {
                        if (!onCache(new CacheReturnParameter { Getter = this }))
                        {
                            error();
                            *(int*)(stream.Data.Byte + (startIndex - sizeof(int))) = AutoCSer.BinarySerialize.Serializer.NullValue;
                            stream.ByteSize = startIndex;
                        }
                        return;
                    }
                    Cache.TcpServer.CallQueue.Add(new ServerCall.CacheGetterGetQueue(this));
                    return;
                case CacheGetStep.Loaded: stream.Write(0); return;
                case CacheGetStep.Error: stream.Write(AutoCSer.BinarySerialize.Serializer.NullValue); return;
            }
        }
        /// <summary>
        /// 获取缓存操作队列
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetQueue()
        {
            if (queue.IsEmpty)
            {
                step = CacheGetStep.Loaded;
                if (!onCache(new CacheReturnParameter { Getter = this })) error();
            }
            else
            {
                currentQueue = queue.GetClear();
                timeout = Date.Now.AddTicks(timeoutTicks);
                if (onCache(new CacheReturnParameter { Getter = this })) return;
                error();
            }
            Cache.NextGetter();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal bool Append(Buffer buffer)
        {
            switch (step)
            {
                case CacheGetStep.Loaded:
                    buffer.Reference();
                    if (onCache(new CacheReturnParameter { Buffer = buffer })) return true;
                    buffer.FreeReference();
                    return false;
                case CacheGetStep.Error: return false;
                default:
                    if (Date.Now <= timeout)
                    {
                        buffer.Reference();
                        queue.Push(buffer);
                        return true;
                    }
                    return false;
            }
        }
    }
}
