using System;
using AutoCSer.Metadata;

namespace AutoCSer.Data
{
    /// <summary>
    /// 数据关键字 代码生成配置
    /// </summary>
    public class PrimaryKeyAttribute : MemberFilterAttribute.PublicInstanceField
    {
        /// <summary>
        /// 默认代码生成配置
        /// </summary>
        internal static readonly PrimaryKeyAttribute Default = new PrimaryKeyAttribute();
        /// <summary>
        /// 是否有序比较
        /// </summary>
        public bool IsComparable;
    }
}
