using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 配置加载
    /// </summary>
    internal static partial class ConfigLoader
    {
        /// <summary>
        /// SQL 配置
        /// </summary>
        internal static readonly Config Config = (Config)AutoCSer.Configuration.Common.Get(typeof(Config)) ?? new Config();
    }
}
