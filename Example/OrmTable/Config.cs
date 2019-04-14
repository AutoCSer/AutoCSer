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
                //return new AutoCSer.Sql.Config
                //{
                //    CheckConnectionName = AutoCSer.Example.OrmConfig.Pub.ConnectionName,
                //    TableNameDepth = 3
                //};
                return new AutoCSer.Sql.Config
                {
                    CheckConnectionNames = new string[] { "Weixin", AutoCSer.Example.OrmConfig.Pub.ConnectionName},
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
                //    Attribute = new AutoCSer.Sql.ClientKindAttribute { ClientType = typeof(AutoCSer.Sql.MySql.Client) },
                //    ConnectionString = "Host=127.0.0.1;DataBase=AutoCSerExample;User id=Example;Password=Example"
                //};
                //grant usage on *.* to Example@127.0.0.1 identified by 'Example' with grant option;
                //flush privileges;
                //create database AutoCSerExample;
                //grant all privileges on AutoCSerExample.* to Example@127.0.0.1 identified by 'Example';
                //flush privileges;
            }
        }
        /// <summary>
        /// 数据库连接信息配置
        /// </summary>
        [AutoCSer.Config.Member(Name = "Weixin")]
        public static AutoCSer.Sql.Connection WeixinSqlConnection
        {
            get
            {
                return new AutoCSer.Sql.Connection
                {
                    Type = Sql.ClientKind.Sql2008,
                    ConnectionString = "server=122.49.32.118;database=yh_marketing;User Id=yhAzure.Alpha;password=x1FlqqnI59BaHWrca8-SBq_w5c2xSfQJ"
                };
            }
        }
    }
}
