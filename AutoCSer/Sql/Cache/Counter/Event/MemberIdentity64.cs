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
    public sealed class MemberIdentity64<valueType, modelType, memberCacheType, targetType> : Member<valueType, modelType, memberCacheType, long, targetType>
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
        public MemberIdentity64(Whole.Event.Cache<valueType, modelType, memberCacheType> cache
            , Func<long, targetType> getByKey, Func<valueType, targetType> getValue, Expression<Func<targetType, KeyValue<valueType, int>>> member, int group = 1)
            : base(cache, group, DataModel.Model<modelType>.GetIdentity, getByKey, getValue, member)
        {
        }
    }
}
