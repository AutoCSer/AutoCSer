using System;
using System.Linq.Expressions;
using AutoCSer.Extension;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public class IdentityArray<valueType, modelType, memberCacheType> : IdentityCache<valueType, modelType, memberCacheType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="memberCache">成员缓存</param>
        /// <param name="group">数据分组</param>
        /// <param name="baseIdentity">基础ID</param>
        /// <param name="isReset">是否初始化事件与数据</param>
        public IdentityArray(Sql.Table<valueType, modelType> table, Expression<Func<valueType, memberCacheType>> memberCache = null, int group = 0, int baseIdentity = 0, bool isReset = true)
            : base(table, memberCache, group, baseIdentity, isReset)
        {
            if (isReset)
            {
                table.OnInserted += onInserted;
                table.OnDeleted += onDeleted;

                reset(null);
            }
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="array">数据集合</param>
        protected void reset(LeftArray<valueType> array)
        {
            int maxIdentity = array.maxKey(value => GetKey(value), 0);
            if (memberGroup == 0) SqlTable.Identity64 = maxIdentity + baseIdentity;
            IdentityArray<valueType> newValues = new IdentityArray<valueType>(maxIdentity + 1);
            foreach (valueType value in array)
            {
                setMemberCacheAndValue(value);
                newValues[GetKey(value)] = value;
            }
            Array = newValues;
            Count = array.Length;
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        internal override void Reset(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            reset(SqlTable.SelectQueue(ref connection, ref query));
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        protected void onInserted(valueType value)
        {
            int identity = GetKey(value);
            if (identity >= Array.Length) Array.ToSize(identity + 1);
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<modelType>.Copy(newValue, value, MemberMap);
            setMemberCacheAndValue(newValue);
            Array[identity] = newValue;
            ++Count;
            callOnInserted(newValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        protected void onDeleted(valueType value)
        {
            int identity = GetKey(value);
            valueType cacheValue = Array.GetRemove(identity);
            --Count;
            callOnDeleted(cacheValue);
        }
    }
    /// <summary>
    /// 自增ID整表数组缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    public class IdentityArray<valueType, modelType> : IdentityArray<valueType, modelType, valueType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        /// <param name="baseIdentity">基础ID</param>
        /// <param name="isReset">是否初始化事件与数据</param>
        public IdentityArray(Sql.Table<valueType, modelType> table, int group = 0, int baseIdentity = 0, bool isReset = true)
            : base(table, null, group, baseIdentity, isReset)
        {
        }
    }
}
