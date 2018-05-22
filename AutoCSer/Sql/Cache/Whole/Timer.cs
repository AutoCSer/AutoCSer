using System;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Metadata;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 缓存时间事件
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <typeparam name="modelType"></typeparam>
    public class Timer<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 时间获取器
        /// </summary>
        protected readonly Func<valueType, DateTime> getTime;
        /// <summary>
        /// 事件委托
        /// </summary>
        private readonly Action runTimeHandle;
        /// <summary>
        /// 事件委托
        /// </summary>
        private readonly Action run;
        /// <summary>
        /// 最小事件时间
        /// </summary>
        protected DateTime minTime;
        /// <summary>
        /// 事件时间集合
        /// </summary>
        private LeftArray<DateTime> times;
        /// <summary>
        /// 事件时间访问锁
        /// </summary>
        private readonly object timeLock = new object();
        /// <summary>
        /// 缓存时间事件
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getTime">时间获取器</param>
        /// <param name="run">事件委托</param>
        /// <param name="isReset">是否绑定事件与重置数据</param>
        public Timer(Event.Cache<valueType, modelType> cache, Func<valueType, DateTime> getTime, Action run, bool isReset)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getTime == null) throw new ArgumentNullException("getTime is null");
            if (run == null) throw new ArgumentNullException("run is null");
            runTimeHandle = runTime;
            this.cache = cache;
            this.getTime = getTime;
            this.run = run;
            minTime = DateTime.MaxValue;

            if (isReset)
            {
                foreach (valueType value in cache.Values)
                {
                    DateTime time = getTime(value);
                    if (time < minTime && time > AutoCSer.Pub.MinTime) minTime = time;
                }
                Append(minTime);
                cache.OnInserted += onInserted;
                cache.OnUpdated += onUpdated;
            }
        }
        /// <summary>
        /// 添加事件时间
        /// </summary>
        /// <param name="time"></param>
        public void Append(DateTime time)
        {
            if (time < minTime && time > AutoCSer.Pub.MinTime)
            {
                Monitor.Enter(timeLock);
                if (time < minTime)
                {
                    try
                    {
                        times.Add(time);
                        minTime = time;
                    }
                    finally { Monitor.Exit(timeLock); }
                }
                else Monitor.Exit(timeLock);
                if (time <= Date.NowTime.Now) AutoCSer.Threading.ThreadPool.TinyBackground.Start(runTimeHandle);
                else AutoCSer.Threading.TimerTask.Default.Add(runTimeHandle, time);
            }
        }
        /// <summary>
        /// 时间事件
        /// </summary>
        private unsafe void runTime()
        {
            DateTime now = Date.NowTime.Set();
            Monitor.Enter(timeLock);
            if (times.Length != 0)
            {
                fixed (DateTime* timeFixed = times.Array)
                {
                    if (*timeFixed <= now)
                    {
                        times.Length = 0;
                        minTime = DateTime.MaxValue;
                    }
                    else
                    {
                        DateTime* end = timeFixed + times.Length;
                        while (*--end <= now) ;
                        minTime = *end;
                        times.Length = (int)(end - timeFixed) + 1;
                    }
                }
            }
            Monitor.Exit(timeLock);
            run();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onInserted(valueType value)
        {
            Append(getTime(value));
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            Append(getTime(value));
        }
    }
}
