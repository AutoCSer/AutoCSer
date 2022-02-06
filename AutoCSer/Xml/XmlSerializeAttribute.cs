using System;
using AutoCSer.Metadata;

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化类型配置
    /// </summary>
    public class XmlSerializeAttribute : MemberFilterAttribute.PublicInstance 
    {
        /// <summary>
        /// 匿名类型序列化配置
        /// </summary>
        internal static readonly XmlSerializeAttribute AnonymousTypeMember = new XmlSerializeAttribute { IsBaseType = false, Filter = MemberFilters.InstanceField };
        /// <summary>
        /// 是否作用与派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
    }
}
