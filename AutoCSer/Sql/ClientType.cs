using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL 客户端类型默认配置信息
    /// </summary>
    public sealed class ClientTypeAttribute : Attribute
    {
        /// <summary>
        /// SQL 客户端处理类型
        /// </summary>
        public Type ClientType;
        /// <summary>
        /// SQL 常量转换处理类型
        /// </summary>
        public Type ConverterType = typeof(ConstantConverter);
        /// <summary>
        /// 名称是否忽略大小写
        /// </summary>
        public bool IgnoreCase;
    }
    /// <summary>
    /// SQL 客户端类型
    /// </summary>
    public enum ClientType : byte
    {
        /// <summary>
        /// 第三方
        /// </summary>
        [ClientType(IgnoreCase = true)]
        ThirdParty,
        /// <summary>
        /// SQL Server2000
        /// </summary>
        [ClientType(ClientType = typeof(MsSql.Sql2000), IgnoreCase = true)]
        Sql2000,
        /// <summary>
        /// SQL Server2005
        /// </summary>
        [ClientType(ClientType = typeof(MsSql.Sql2005), IgnoreCase = true)]
        Sql2005,
        /// <summary>
        /// SQL Server2008
        /// </summary>
        [ClientType(ClientType = typeof(MsSql.Sql2005), IgnoreCase = true)]
        Sql2008,
        /// <summary>
        /// SQL Server2012
        /// </summary>
        [ClientType(ClientType = typeof(MsSql.Sql2005), IgnoreCase = true)]
        Sql2012,
        /// <summary>
        /// Excel
        /// </summary>
#if XAMARIN
        [ClientType(IgnoreCase = true)]
#else
        [ClientType(ClientType = typeof(Excel.Client))]
#endif
        Excel,
    }
}
