using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

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

        /// <summary>
        /// 修改数据库记录
        /// </summary>
        /// <param name="value">待修改数据</param>
        /// <param name="memberMap">需要修改的字段成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否修改成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Update(valueType value, MemberMap<modelType> memberMap, bool isIgnoreTransaction = false)
        {
            valueType cacheLock = this[AutoCSer.Sql.DataModel.Model<modelType>.GetIdentity32(value)];
            return cacheLock != null && SqlTable.UpdateQueue(value, memberMap, isIgnoreTransaction);
        }
        /// <summary>
        /// 修改数据库记录
        /// </summary>
        /// <param name="value">待修改数据</param>
        /// <param name="memberMap">需要修改的字段成员位图</param>
        /// <param name="onUpdated">更新数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否修改成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Update(valueType value, MemberMap<modelType> memberMap, Action<valueType> onUpdated, bool isIgnoreTransaction = false)
        {
            valueType cacheLock = this[AutoCSer.Sql.DataModel.Model<modelType>.GetIdentity32(value)];
            if (cacheLock != null) SqlTable.UpdateQueue(value, memberMap, onUpdated, isIgnoreTransaction);
            else if (onUpdated != null) onUpdated(null);
        }
        /// <summary>
        /// 删除数据库记录
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Delete(int identity, bool isIgnoreTransaction = false)
        {
            valueType value = this[identity];
            return value != null && SqlTable.DeleteQueue(value, isIgnoreTransaction);
        }
        /// <summary>
        /// 删除数据库记录
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Delete(int identity, Action<valueType> onDeleted, bool isIgnoreTransaction = false)
        {
            valueType value = this[identity];
            if (value != null) SqlTable.DeleteQueue(value, onDeleted, isIgnoreTransaction);
            else if (onDeleted != null) onDeleted(null);
        }
    }
}
