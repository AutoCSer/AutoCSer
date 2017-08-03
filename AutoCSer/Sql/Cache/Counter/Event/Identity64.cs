using System;

namespace AutoCSer.Sql.Cache.Counter.Event
{
    /// <summary>
    /// 自增id标识缓存计数器
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    public sealed class Identity64<valueType, modelType> : Cache<valueType, modelType, long>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 自增id标识缓存计数器
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        public Identity64(Sql.Table<valueType, modelType> sqlTool, int group = 0)
            : base(sqlTool, group, DataModel.Model<modelType>.GetIdentity)
        {
        }
    }
}
