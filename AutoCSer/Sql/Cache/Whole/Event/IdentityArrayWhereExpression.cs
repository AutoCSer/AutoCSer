using System;
using System.Linq.Expressions;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public class IdentityArrayWhereExpression<valueType, modelType, memberCacheType> : IdentityArray<valueType, modelType, memberCacheType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 数据匹配器
        /// </summary>
        private Func<modelType, bool> isValue;
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberCache"></param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="baseIdentity">基础ID</param>
        /// <param name="group">数据分组</param>
        public IdentityArrayWhereExpression(Sql.Table<valueType, modelType> sqlTool, Expression<Func<valueType, memberCacheType>> memberCache
            , Expression<Func<modelType, bool>> isValue, int group = 0, int baseIdentity = 0)
            : base(sqlTool, memberCache, group, baseIdentity, false)
        {
            if (isValue == null) throw new ArgumentNullException();
            this.isValue = isValue.Compile();

            sqlTool.OnInserted += onInserted;
            sqlTool.OnUpdated += onUpdated;
            sqlTool.OnDeleted += onDeleted;

            reset(isValue);
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onInserted(valueType value)
        {
            if (isValue(value)) base.onInserted(value);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private new void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (isValue(value)) base.onUpdated(value, oldValue, memberMap);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private new void onDeleted(valueType value)
        {
            if (isValue(value)) base.onDeleted(value);
        }
    }
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    public sealed class IdentityArrayWhereExpression<valueType, modelType> : IdentityArrayWhereExpression<valueType, modelType, valueType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 自增ID整表数组缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="isValue">数据匹配器,必须保证更新数据的匹配一致性</param>
        /// <param name="baseIdentity">基础ID</param>
        /// <param name="group">数据分组</param>
        public IdentityArrayWhereExpression(Sql.Table<valueType, modelType> sqlTool, Expression<Func<modelType, bool>> isValue, int group = 0, int baseIdentity = 0)
            : base(sqlTool, null, isValue, group, baseIdentity)
        {
        }
    }
}
