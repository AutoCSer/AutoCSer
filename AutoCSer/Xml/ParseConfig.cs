using System;
using AutoCSer.Metadata;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML 解析配置参数
    /// </summary>
    public sealed class ParseConfig
    {
        /// <summary>
        /// 自定义构造函数
        /// </summary>
        public Func<Type, object> Constructor;
        /// <summary>
        /// 根节点名称(不能包含非法字符)
        /// </summary>
        public string BootNodeName = "xml";
        /// <summary>
        /// 集合子节点名称(不能包含非法字符)
        /// </summary>
        public string ItemName = "item";
        ///// <summary>
        ///// 成员选择
        ///// </summary>
        //public AutoCSer.Metadata.MemberFilters MemberFilter = Metadata.MemberFilters.Instance;
        /// <summary>
        /// 是否保存属性索引
        /// </summary>
        public bool IsAttribute;
        /// <summary>
        /// 是否临时字符串(可修改)
        /// </summary>
        public bool IsTempString;
        /// <summary>
        /// 是否强制匹配枚举值
        /// </summary>
        public bool IsMatchEnum;
    }
}
