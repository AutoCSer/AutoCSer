using System;
using AutoCSer.Metadata;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML序列化成员配置
    /// </summary>
    public class MemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 集合子节点名称(不能包含非法字符)
        /// </summary>
        public string ItemName;
    }
}
