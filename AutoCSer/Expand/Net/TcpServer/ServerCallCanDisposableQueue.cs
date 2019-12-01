using AutoCSer.Extension;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 自定义队列数据
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    public abstract class ServerCallCanDisposableQueue<keyType, valueType> : ServerCallCanDisposableQueue
        where keyType : IEquatable<keyType>
        where valueType : class
    {
        /// <summary>
        /// 活动计数
        /// </summary>
        private sealed class LiveCount
        {
            /// <summary>
            /// 活动计数
            /// </summary>
            internal int Count;
            /// <summary>
            /// 数据
            /// </summary>
            internal readonly valueType Value;
            /// <summary>
            /// 活动计数
            /// </summary>
            /// <param name="value"></param>
            /// <param name="isLive"></param>
            internal LiveCount(valueType value, bool isLive)
            {
                Value = value;
                Count = isLive ? 1 : 0;
            }
        }
        /// <summary>
        /// 空闲数据集合
        /// </summary>
        private readonly FifoPriorityQueue<keyType, LiveCount> freeData = new FifoPriorityQueue<keyType, LiveCount>();
        /// <summary>
        /// 数据集合
        /// </summary>
        private readonly Dictionary<keyType, LiveCount> datas = DictionaryCreator<keyType>.Create<LiveCount>();
        /// <summary>
        /// 最大数据数量
        /// </summary>
        private readonly int maxDataCount;
        /// <summary>
        /// 当前关键字
        /// </summary>
        private keyType currentKey;
        /// <summary>
        /// 当前数据
        /// </summary>
        private LiveCount current;
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="maxDataCount">最大数据数量</param>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="log">日志接口</param>
        public ServerCallCanDisposableQueue(int maxDataCount, bool isBackground = true, AutoCSer.Log.ILog log = null) : base(isBackground, false, log)    
        {
            if (maxDataCount <= 0) throw new IndexOutOfRangeException();
            this.maxDataCount = maxDataCount;
            threadHandle.Start();
        }
        /// <summary>
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                ServerCallBase value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            current = null;
                            value.SingleRunTask(ref value);
                        }
                        break;
                    }
                    catch (Exception error)
                    {
                        log.Add(Log.LogType.Error, error);
                    }
                }
                while (true);
            }
            while (!isDisposed);
        }
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据</returns>
        protected abstract valueType create(ref keyType key);
        /// <summary>
        /// 获取当前数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isLive">设置为活动数据</param>
        /// <returns></returns>
        public valueType Get(ref keyType key, bool isLive = false)
        {
            if (current == null)
            {
                currentKey = key;
                if (!datas.TryGetValue(key, out current))
                {
                    datas.Add(key, current = new LiveCount(create(ref key), isLive));
                    if (datas.Count > maxDataCount && freeData.Count != 0)
                    {
                        keyType removeKey = freeData.UnsafePopNode().Key;
                        datas.Remove(removeKey);
                        LiveCount removeValue;
                        freeData.Remove(ref removeKey, out removeValue);
                    }
                    if (!isLive) freeData.Set(ref key, current);
                }
            }
            else if(isLive) setLive();
            return current.Value;
        }
        /// <summary>
        /// 设置为活动数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setLive()
        {
            if (current.Count++ == 0)
            {
                LiveCount removeValue;
                freeData.Remove(ref currentKey, out removeValue);
            }
        }
        /// <summary>
        /// 设置为活动数据
        /// </summary>
        /// <returns>是否设置成功，失败表示没有中找到当前数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SetLive()
        {
            if (current != null)
            {
                setLive();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 撤销一次活动计数
        /// </summary>
        /// <param name="isRemoveData">当数据处于非活动状态时是否从缓存中移除</param>
        /// <returns>是否撤销成功，失败表示没有中找到当前数据</returns>
        public bool RemoveLive(bool isRemoveData = false)
        {
            if (current != null)
            {
                if (--current.Count == 0)
                {
                    if (isRemoveData || datas.Count > maxDataCount) datas.Remove(currentKey);
                    else freeData.Set(ref currentKey, current);
                }
                return true;
            }
            return false;
        }
    }
}
