using System;
using System.Linq.Expressions;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 自增ID整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public abstract class IdentityMemberMap<valueType, modelType, memberCacheType> : Key<valueType, modelType, memberCacheType, int>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 基础ID
        /// </summary>
        protected readonly int baseIdentity;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get; protected set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        internal override int ValueCount { get { return Count; } }
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="memberCache"></param>
        /// <param name="group">数据分组</param>
        /// <param name="baseIdentity">基础ID</param>
        protected IdentityMemberMap(Sql.Table<valueType, modelType> table, Expression<Func<valueType, memberCacheType>> memberCache, int group, int baseIdentity)
            : base(table, memberCache, DataModel.Model<modelType>.IdentityGetter(baseIdentity), group)
        {
            this.baseIdentity = baseIdentity;
        }
        /// <summary>
        /// 创建自增id标识缓存计数器
        /// </summary>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="group">数据分组</param>
        /// <returns></returns>
        public Counter.Event.MemberIdentity<valueType, modelType, memberCacheType> CreateCounter
            (Expression<Func<memberCacheType, KeyValue<valueType, int>>> member, int group = 1)
        {
            return new Counter.Event.MemberIdentity<valueType, modelType, memberCacheType>(this, member, group);
        }
        /// <summary>
        /// 创建先进先出优先队列缓存
        /// </summary>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="group">数据分组</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        /// <returns></returns>
        public Counter.MemberQueue<valueType, modelType, memberCacheType, int> CreateCounterMemberQueue
            (Expression<Func<memberCacheType, KeyValue<valueType, int>>> member, Expression<Func<memberCacheType, valueType>> valueMember
            , Expression<Func<memberCacheType, memberCacheType>> previousMember, Expression<Func<memberCacheType, memberCacheType>> nextMember, int group = 1, int maxCount = 0)
        {
            return CreateCounter(member, group).CreateMemberQueue(valueMember, previousMember, nextMember, maxCount);
        }
    }
}
