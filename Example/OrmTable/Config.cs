using System;
using System.Collections.Generic;

namespace AutoCSer.Example.OrmTable
{
    /// <summary>
    /// 配置
    /// </summary>
    internal sealed class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// SQL 配置
        /// </summary>
        [AutoCSer.Configuration.Member]
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
        [AutoCSer.Configuration.Member(Name = AutoCSer.Example.OrmConfig.Pub.ConnectionName)]
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
        [AutoCSer.Configuration.Member(Name = "Weixin")]
        public static AutoCSer.Sql.Connection WeixinSqlConnection
        {
            get
            {
                return new AutoCSer.Sql.Connection
                {
                    Type = Sql.ClientKind.Sql2008,
                    ConnectionString = "server=127.0.0.1;database=AutoCSerExample;User Id=Example;password=Example"
                };
            }
        }
    }
}
