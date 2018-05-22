using System;
using AutoCSer.Metadata;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// SQL 表格缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    public abstract class Table<valueType, modelType> : IDisposable
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 缓存更新事件
        /// </summary>
        /// <param name="cacheValue">缓存数据</param>
        /// <param name="newValue">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新数据成员</param>
        public delegate void OnCacheUpdated(valueType cacheValue, valueType newValue, valueType oldValue, MemberMap<modelType> memberMap);

        /// <summary>
        /// SQL操作工具
        /// </summary>
        public readonly Sql.Table<valueType, modelType> SqlTable;
        /// <summary>
        /// 数据成员位图
        /// </summary>
        internal readonly AutoCSer.Metadata.MemberMap<modelType> MemberMap;
        /// <summary>
        /// 成员分组
        /// </summary>
        protected readonly int memberGroup;
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        protected Table(Sql.Table<valueType, modelType> table, int group)
        {
            if (table == null) throw new ArgumentNullException();
            memberGroup = group;
            SqlTable = table;
            MemberMap = DataModel.Model<modelType>.GetCacheMemberMap(group);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            MemberMap.Dispose();
        }
    }
}
