using System;
using System.Linq.Expressions;

namespace AutoCSer.Sql.Cache.Counter.Event
{
    /// <summary>
    /// 自增id标识缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="targetType"></typeparam>
    public class MemberIdentity<valueType, modelType, memberCacheType, targetType> : Member<valueType, modelType, memberCacheType, int, targetType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
        where targetType : class
    {
        /// <summary>
        /// 自增id标识缓存计数器
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="getByKey"></param>
        /// <param name="getValue"></param>
        /// <param name="member"></param>
        /// <param name="group">数据分组</param>
        public MemberIdentity(Whole.Event.Cache<valueType, modelType, memberCacheType> cache
            , Func<int, targetType> getByKey, Func<valueType, targetType> getValue, Expression<Func<targetType, KeyValue<valueType, int>>> member, int group = 1)
            : base(cache, group, DataModel.Model<modelType>.GetIdentity32, getByKey, getValue, member)
        {
        }
    }
    /// <summary>
    /// 自增id标识缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType"></typeparam>
    public sealed class MemberIdentity<valueType, modelType, memberCacheType> : MemberIdentity<valueType, modelType, memberCacheType, memberCacheType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 自增id标识缓存计数器
        /// </summary>
        /// <param name="cache">关键字整表缓存</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="group">数据分组</param>
        public MemberIdentity(Whole.Event.Key<valueType, modelType, memberCacheType, int> cache
            , Expression<Func<memberCacheType, KeyValue<valueType, int>>> member, int group = 1)
            : base(cache, cache.GetMemberCacheByKey, cache.GetMemberCache, member, group)
        {
        }

        /// <summary>
        /// 创建先进先出优先队列缓存
        /// </summary>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        /// <returns></returns>
        public MemberQueue<valueType, modelType, memberCacheType, int> CreateMemberQueue(Expression<Func<memberCacheType, valueType>> valueMember
            , Expression<Func<memberCacheType, memberCacheType>> previousMember, Expression<Func<memberCacheType, memberCacheType>> nextMember, int maxCount = 0)
        {
            return new MemberQueue<valueType, modelType, memberCacheType, int>(this, SqlTable.GetQueue, valueMember, previousMember, nextMember, maxCount);
        }
    }
}
