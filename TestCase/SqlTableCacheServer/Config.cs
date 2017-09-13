using System;

namespace AutoCSer.TestCase.SqlTableCacheServer
{
    /// <summary>
    /// 配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// 数据库连接配置名称
        /// </summary>
        internal const string SqlConnectionName = "Demo";
        /// <summary>
        /// 数据读取服务名称
        /// </summary>
        internal const string DataReaderServer = "DataReader";
        /// <summary>
        /// 数据写服务名称
        /// </summary>
        internal const string DataWriterServer = "DataWriter";

        /// <summary>
        /// SQL 配置
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.Sql.Config SqlConfig
        {
            get
            {
                return new AutoCSer.Sql.Config
                {
                    CheckConnectionName = SqlConnectionName,
                    TableNameDepth = 3
                };
            }
        }
        /// <summary>
        /// 数据库连接信息配置
        /// </summary>
        [AutoCSer.Config.Member(Name = SqlConnectionName)]
        public static AutoCSer.Sql.Connection SqlConnection
        {
            get
            {
                return new AutoCSer.Sql.Connection
                {
                    Type = Sql.ClientKind.Sql2008,
                    ConnectionString = "server=127.0.0.1;database=AutoCSerExample;uid=Example;pwd=Example"
                };
                //return new AutoCSer.Sql.Connection
                //{
                //    Attribute = new AutoCSer.Sql.ClientTypeAttribute { ClientType = typeof(AutoCSer.Sql.MySql.Client) },
                //    ConnectionString = "Host=127.0.0.1;DataBase=AutoCSerExample;User id=Example;Password=Example"
                //};
            }
        }
    }
}
