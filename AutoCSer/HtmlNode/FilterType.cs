using System;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 节点筛选器类型
    /// </summary>
    internal enum FilterType : byte
    {
        /// <summary>
        /// 未知节点
        /// </summary>
        Uunknown,
        /// <summary>
        /// 子孙节点筛选
        /// </summary>
        Node,
        /// <summary>
        /// class 样式筛选
        /// </summary>
        Class,
        /// <summary>
        /// 子节点筛选
        /// </summary>
        Child,
        /// <summary>
        /// 属性值筛选
        /// </summary>
        Value,
    }
}
