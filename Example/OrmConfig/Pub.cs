using System;

namespace AutoCSer.Example.OrmConfig
{
    /// <summary>
    /// 公共配置
    /// </summary>
    public static class Pub
    {
        /// <summary>
        /// 数据库连接配置名称
        /// </summary>
        public const string ConnectionName = "Example";

        /// <summary>
        /// 数据读服务名称
        /// </summary>
        public const string DataReaderServerName = "ExampleReader";
        /// <summary>
        /// 数据写服务名称
        /// </summary>
        public const string DataWriterServerName = "ExampleWriter";
        /// <summary>
        /// 数据日志推送服务名称
        /// </summary>
        public const string DataLogServerName = "ExampleDataLog";
    }
}
