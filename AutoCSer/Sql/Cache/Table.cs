using System;

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
        /// SQL操作工具
        /// </summary>
        internal readonly Sql.Table<valueType, modelType> SqlTable;
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
