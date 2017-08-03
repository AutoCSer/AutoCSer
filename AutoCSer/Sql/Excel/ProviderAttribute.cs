using System;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// 数据接口属性
    /// </summary>
    public sealed class ProviderAttribute : Attribute
    {
        /// <summary>
        /// 连接名称
        /// </summary>
        public string Name;
        /// <summary>
        /// Excel版本号
        /// </summary>
        public string Excel;
    }
}
