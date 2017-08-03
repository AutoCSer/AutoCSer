using System;

namespace AutoCSer.Sql.Cache.Counter.Event
{
    /// <summary>
    /// 关键字缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed class PrimaryKey<valueType, modelType, keyType> : Cache<valueType, modelType, keyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字缓存计数器
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        public PrimaryKey(Sql.Table<valueType, modelType, keyType> sqlTool, int group = 0)
            : base(sqlTool, group, sqlTool.GetPrimaryKey)
        {
        }
    }
}
