using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL 数据库配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 检测链接类型集合
        /// </summary>
        public string[] CheckConnectionNames = NullValue<string>.Array;
        /// <summary>
        /// 检测链接类型
        /// </summary>
        public string CheckConnectionName
        {
            set { CheckConnectionNames = new string[] { value }; }
        }
        /// <summary>
        /// SQL表格名称前缀集合
        /// </summary>
        public string[] TableNamePrefixs = NullValue<string>.Array;
        /// <summary>
        /// SQL表格名称缺省前缀深度，默认为 2
        /// </summary>
        public int TableNameDepth = 2;
        /// <summary>
        /// 连接池大小，默认为 1K
        /// </summary>
        public int ConnectionPoolSize = 1 << 10;
        /// <summary>
        /// 计数缓存默认最大容器大小，默认为 1K
        /// </summary>
        public int CacheMaxCount = 1 << 10;
    }
}
