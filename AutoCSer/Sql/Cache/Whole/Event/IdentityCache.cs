using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 自增ID整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public abstract class IdentityCache<valueType, modelType, memberCacheType> : IdentityMemberMap<valueType, modelType, memberCacheType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 缓存数据集合
        /// </summary>
        internal IdentityArray<valueType> Array;
        /// <summary>
        /// 数据集合
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get { return Array.Values; }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity">数据自增ID</param>
        /// <returns>数据</returns>
        public override valueType this[int identity]
        {
            get
            {
                return (uint)identity < (uint)Array.Length ? Array[identity] : null;
            }
        }
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="memberCache"></param>
        /// <param name="baseIdentity">基础ID</param>
        /// <param name="group">数据分组</param>
        /// <param name="isEvent">是否绑定更新事件</param>
        protected IdentityCache(Sql.Table<valueType, modelType> table, Expression<Func<valueType, memberCacheType>> memberCache, int group, int baseIdentity, bool isEvent)
            : base(table, memberCache, group, baseIdentity)
        {
            if (isEvent) table.OnUpdated += onUpdated;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        protected void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            valueType cacheValue = Array[GetKey(value)];
            update(cacheValue, value, oldValue, memberMap);
            callOnUpdated(cacheValue, value, oldValue, memberMap);
        }
    }
}
