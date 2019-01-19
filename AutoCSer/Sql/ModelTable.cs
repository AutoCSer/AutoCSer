using System;
using AutoCSer.Metadata;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据表格
    /// </summary>
    /// <typeparam name="modelType">模型类型</typeparam>
    public class ModelTable<modelType> : Table<modelType, modelType>
        where modelType : class
    {
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        private ModelTable(Connection connection, TableAttribute attribute, bool isCreateCacheWait) : base(connection, attribute, null, isCreateCacheWait) { }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        private ModelTable(TableAttribute attribute, bool isCreateCacheWait) : base(attribute, null, isCreateCacheWait) { }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        public static ModelTable<modelType> Get(bool isCreateCacheWait = false)
        {
            Type type = typeof(modelType);
            TableAttribute attribute = TypeAttribute.GetAttribute<TableAttribute>(type, false);
            if (attribute != null)// && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1
            {
                ModelTable<modelType> table = new ModelTable<modelType>(attribute, isCreateCacheWait);
                if (!table.IsError) return table;
            }
            return null;
        }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        public static ModelTable<modelType> Get(Connection connection, TableAttribute attribute, bool isCreateCacheWait = false)
        {
            if (attribute != null)// && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1
            {
                ModelTable<modelType> table = new ModelTable<modelType>(connection, attribute, isCreateCacheWait);
                if (!table.IsError) return table;
            }
            return null;
        }
    }
    /// <summary>
    /// 数据表格
    /// </summary>
    /// <typeparam name="modelType">模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class ModelTable<modelType, keyType> : Table<modelType, modelType, keyType>
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        private ModelTable(Connection connection, TableAttribute attribute, bool isCreateCacheWait) : base(connection, attribute, null, isCreateCacheWait) { }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        private ModelTable(TableAttribute attribute, bool isCreateCacheWait) : base(attribute, null, isCreateCacheWait) { }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        public static ModelTable<modelType, keyType> Get(bool isCreateCacheWait = false)
        {
            Type type = typeof(modelType);
            TableAttribute attribute = TypeAttribute.GetAttribute<TableAttribute>(type, false);
            if (attribute != null)// && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1
            {
                ModelTable<modelType, keyType> table = new ModelTable<modelType, keyType>(attribute, isCreateCacheWait);
                if (!table.IsError) return table;
            }
            return null;
        }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        public static ModelTable<modelType, keyType> Get(Connection connection, TableAttribute attribute, bool isCreateCacheWait = false)
        {
            if (attribute != null)// && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1
            {
                ModelTable<modelType, keyType> table = new ModelTable<modelType, keyType>(connection, attribute, isCreateCacheWait);
                if (!table.IsError) return table;
            }
            return null;
        }
    }
}
