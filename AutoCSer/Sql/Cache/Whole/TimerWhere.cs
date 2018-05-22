using System;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 缓存时间事件
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <typeparam name="modelType"></typeparam>
    public sealed class TimerWhere<valueType, modelType> : Timer<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private readonly Func<valueType, bool> isValue;
        /// <summary>
        /// 缓存时间事件
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getTime">时间获取器</param>
        /// <param name="run">事件委托</param>
        /// <param name="isValue">数据匹配器</param>
        public TimerWhere(Event.Cache<valueType, modelType> cache, Func<valueType, DateTime> getTime, Action run, Func<valueType, bool> isValue)
            : base(cache, getTime, run, false)
        {
            if (isValue == null) throw new ArgumentNullException();
            this.isValue = isValue;

            foreach (valueType value in cache.Values)
            {
                if (isValue(value))
                {
                    DateTime time = getTime(value);
                    if (time < minTime && time > AutoCSer.Pub.MinTime) minTime = time;
                }
            }
            Append(minTime);
            cache.OnInserted += onInserted;
            cache.OnUpdated += onUpdated;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onInserted(valueType value)
        {
            if (isValue(value)) base.onInserted(value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (isValue(value)) base.onInserted(cacheValue);
        }
    }
}
