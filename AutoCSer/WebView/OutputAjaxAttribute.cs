using System;
using AutoCSer.Metadata;

namespace AutoCSer.WebView
{
    /// <summary>
    /// Ajax视图输出参数
    /// </summary>
    public sealed class OutputAjaxAttribute : IgnoreMemberAttribute
    {
        /// <summary>
        /// 默认Ajax视图输出参数
        /// </summary>
        internal static readonly OutputAjaxAttribute Null = new OutputAjaxAttribute();
        /// <summary>
        /// 输出绑定名称
        /// </summary>
        public string BindingName;
        /// <summary>
        /// 默认为 false 表示不忽略 null 值输出
        /// </summary>
        public bool IsIgnoreNull;
        /// <summary>
        /// 默认为 false 表示仅输出当前成员，否则输出所有子级成员
        /// </summary>
        public bool IsAllMember;
    }
}
