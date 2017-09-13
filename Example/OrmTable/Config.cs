using System;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
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
                    CheckConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName,
                    TableNameDepth = 3
                };
            }
        }
        /// <summary>
        /// 数据库连接信息配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Example.OrmConfig.Pub.ConnectionName)]
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
