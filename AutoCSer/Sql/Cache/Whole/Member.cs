using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 成员绑定缓存
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <typeparam name="modelType"></typeparam>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="targetType"></typeparam>
    /// <typeparam name="cacheType"></typeparam>
    public abstract class Member<valueType, modelType, keyType, targetType, cacheType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where targetType : class
        where cacheType : class
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 分组字典关键字获取器
        /// </summary>
        protected readonly Func<modelType, keyType> getKey;
        /// <summary>
        /// 获取缓存目标对象
        /// </summary>
        protected readonly Func<keyType, targetType> getValue;
        /// <summary>
        /// 获取缓存委托
        /// </summary>
        protected readonly Func<targetType, cacheType> getMember;
        /// <summary>
        /// 设置缓存委托
        /// </summary>
        protected readonly Action<targetType, cacheType> setMember;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getValue">获取目标对象委托</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getTargets">获取缓存目标对象集合</param>
        public Member(Event.Cache<valueType, modelType> cache, Func<modelType, keyType> getKey, Func<keyType, targetType> getValue
            , Expression<Func<targetType, cacheType>> member, Func<IEnumerable<targetType>> getTargets)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getValue == null) throw new ArgumentNullException("getValue is null");
            if (getTargets == null) throw new ArgumentNullException("getTargets is null");
            if (member == null) throw new ArgumentNullException("member is null");
            MemberExpression<targetType, cacheType> expression = new MemberExpression<targetType, cacheType>(member);
            if (expression.Field == null) throw new InvalidCastException("member is not MemberExpression");
            this.cache = cache;
            this.getKey = getKey;
            this.getValue = getValue;
            //this.getTargets = getTargets;
            getMember = expression.GetMember;
            setMember = expression.SetMember;
        }
    }
}
