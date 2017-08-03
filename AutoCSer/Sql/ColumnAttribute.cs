using System;
using AutoCSer.Metadata;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据列配置
    /// </summary>
    public class ColumnAttribute : MemberFilterAttribute.PublicInstanceField
    {
        /// <summary>
        /// 默认空属性
        /// </summary>
        internal static readonly ColumnAttribute Default = new ColumnAttribute();
        /// <summary>
        /// null 字符串是否转换为 string.Empty
        /// </summary>
        public bool IsNullStringEmpty = true;
    }
}
